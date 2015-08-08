using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AmbientSoundsTuner.SoundPatchers;
using ColossalFramework.UI;
using CommonShared.Utils;

namespace AmbientSoundsTuner.Detour
{
    /// <summary>
    /// This static class detours the calls for playing click sounds so we can have our own volume level.
    /// </summary>
    internal static class CustomPlayClickSound
    {
        private static readonly MethodInfo playClickSoundOriginal = typeof(UIComponent).GetMethod("PlayClickSound", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo playClickSoundReplacement = typeof(CustomPlayClickSound).GetMethod("PlayClickSound");
        private static DetourCallsState playClickSoundState;
        private static readonly MethodInfo playDisabledClickSoundOriginal = typeof(UIComponent).GetMethod("PlayDisabledClickSound", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo playDisabledClickSoundReplacement = typeof(CustomPlayClickSound).GetMethod("PlayDisabledClickSound");
        private static DetourCallsState playDisabledClickSoundState;

        public static void Detour()
        {
            try
            {
                playClickSoundState = DetourUtils.RedirectCalls(playClickSoundOriginal, playClickSoundReplacement);
                Mod.Instance.Log.Debug("UIComponent.PlayClickSound() has been detoured");
            }
            catch (Exception ex)
            {
                Mod.Instance.Log.Error("Exception while detouring UIComponent.PlayClickSound(): {0}", ex);
            }

            try
            {
                playDisabledClickSoundState = DetourUtils.RedirectCalls(playDisabledClickSoundOriginal, playDisabledClickSoundReplacement);
                Mod.Instance.Log.Debug("UIComponent.PlayDisabledClickSound() has been detoured");
            }
            catch (Exception ex)
            {
                Mod.Instance.Log.Error("Exception while detouring UIComponent.PlayDisabledClickSound(): {0}", ex);
            }
        }

        public static void UnDetour()
        {
            try
            {
                DetourUtils.RevertRedirect(playClickSoundOriginal, playClickSoundState);
                Mod.Instance.Log.Debug("UIComponent.PlayClickSound() detour has been reverted");
            }
            catch (Exception ex)
            {
                Mod.Instance.Log.Error("Exception while reverting detour UIComponent.PlayClickSound(): {0}", ex);
            }

            try
            {
                DetourUtils.RevertRedirect(playDisabledClickSoundOriginal, playDisabledClickSoundState);
                Mod.Instance.Log.Debug("UIComponent.PlayDisabledClickSound() detour has been reverted");
            }
            catch (Exception ex)
            {
                Mod.Instance.Log.Error("Exception while reverting detour UIComponent.PlayDisabledClickSound(): {0}", ex);
            }
        }

        public static void PlayClickSound(UIComponent @this, UIComponent comp)
        {
            if (@this.playAudioEvents && comp == @this && UIView.playSoundDelegate != null)
            {
                if (@this.clickSound != null)
                {
                    UIView.playSoundDelegate(@this.clickSound, SoundPatchersManager.instance.MiscPatcher.UIClickSoundVolume);
                    return;
                }
                if (@this.GetUIView().defaultClickSound != null)
                {
                    UIView.playSoundDelegate(@this.GetUIView().defaultClickSound, SoundPatchersManager.instance.MiscPatcher.UIClickSoundVolume);
                }
            }
        }

        public static void PlayDisabledClickSound(UIComponent @this, UIComponent comp)
        {
            if (@this.playAudioEvents && comp == @this && UIView.playSoundDelegate != null)
            {
                if (@this.disabledClickSound != null)
                {
                    UIView.playSoundDelegate(@this.disabledClickSound, SoundPatchersManager.instance.MiscPatcher.UIDisabledClickSoundVolume);
                    return;
                }
                if (@this.GetUIView().defaultDisabledClickSound != null)
                {
                    UIView.playSoundDelegate(@this.GetUIView().defaultDisabledClickSound, SoundPatchersManager.instance.MiscPatcher.UIDisabledClickSoundVolume);
                }
            }
        }
    }
}
