using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AmbientSoundsTuner.Utils
{
    internal static class ReflectionUtils
    {
        public static T GetPrivateField<T>(object obj, string name)
        {
            return (T)obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
        }

        public static void SetPrivateField<T>(object obj, string name, T value)
        {
            obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
        }

        public static void InvokePrivateMethod(object obj, string name, params object[] args)
        {
            MethodInfo method = obj.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(obj, args);
        }

        public static T InvokePrivateMethod<T>(object obj, string name, params object[] args)
        {
            MethodInfo method = obj.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)method.Invoke(obj, args);
        }

        public static void InvokePrivateStaticMethod(Type type, string name, params object[] args)
        {
            MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
            method.Invoke(null, args);
        }

        public static T InvokePrivateStaticMethod<T>(Type type, string name, params object[] args)
        {
            MethodInfo method = type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
            return (T)method.Invoke(null, args);
        }
    }
}
