# Cities Skylines: Ambient Sounds Tuner
[![Build status](https://ci.appveyor.com/api/projects/status/pt4toy5d9o5mb1bo/branch/master?svg=true)](https://ci.appveyor.com/project/Archomeda/csl-ambient-sounds-tuner/branch/master)

Sick of hearing those wind sounds when zoomed out, but you still want to hear
other ambient sounds? This mod will allow you to tune each different ambient
sound type individually.

After installing this mod, go to the audio settings. You should see a new button
appear that will open a new window where you can change the volume of both the
ambient sounds and the effect sounds.

**Note:** Although it's not needed anymore, if the button does not show up after
enabling, try restarting the game.

## Installation
Go to the
[Steam Workshop](http://steamcommunity.com/sharedfiles/filedetails/?id=455958878)
and subscribe to the mod, it will install automatically. This will also keep it
updated with newer releases. If you want to do it manually, you can clone this
repo, compile the code yourself and place the DLL file in your mods folder.

## Compatibility
This mod is based on version 1.1.0 of Cities Skylines, and it is not guaranteed
that it will work on later versions. I'll try to keep it updated when newer
versions are released however.

If a newer version of Cities Skylines breaks your game, don't worry. This mod
should not break your saves (as far as I know). Just disable the mod for the
time being until an update of this mod is released.

### Mods
This mod should be compatible with all mods, as long as they don't change the
following stuff:
- The values of `AudioManager.instance.m_properties.m_ambients`.
- The values of `m_audioInfo` within an `EffectInfo` that is a `SoundEffect`.

*Incompatible:*
- [ARIS] Remove Seagulls
  - This mod basically removes the sound effects of the seagulls in the same
    location where Ambient Sounds Tuner changes the volume. However, Ambient
    Sounds Tuner allows you to customize the volume of the seagulls. Therefore,
    [ARIS] Remove Seagulls is not needed when using Ambient Sounds Tuner and has
    to be disabled or removed.
- SilenceObnoxiousSirens
  - This mod basically sets the volume of every siren to 0 in the same location
    as Ambient Sounds Tuner does. However, Ambient Sounds Tuner allows you to
    customize the volume of each siren. Therefore, SilenceObnoxiousSirens is not
    needed when using Ambient Sounds Tuner and has to be disabled or removed.

## Contributing
I'm open for any contributions you can make. If you find a bug, create an issue
here on GitHub. GitHub is very nice with maintaining a list of issues.
Submitting a bug report on the Steam Workshop is also appreciated, but it might
take a little longer for me to respond, because I prefer GitHub. If you know C#,
you can try to fix it yourself and submit a pull request.

### Compilation Notes
Please note that setting up your development environment is a bit different from
the Cities Skylines wiki. Instead of hardcoding various dependencies in the
solution file, you have to specify the path yourself as a user configuration
that will not be pushed to the repo. In order to do this, after you have opened
the project in Visual Studio, go to the project settings, reference paths, and
add your `SteamApps/Common/Cities_Skylines/Cities_Data/Managed` folder there.

Also, please refrain from adding those hardcoded references in the solution
file if you want to submit a pull request.
