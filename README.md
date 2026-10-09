<h1 align="center">
<a href="https://github.com/doombubbles/template-mod/releases/latest/download/ProjectileGoRound.dll">
    <img align="left" alt="Icon" height="90" src="Icon.png">
    <img align="right" alt="Download" height="75" src="https://raw.githubusercontent.com/gurrenm3/BTD-Mod-Helper/master/BloonsTD6%20Mod%20Helper/Resources/DownloadBtn.png">
</a>
ProjectileGoRound
</h1>

                                                                                                                                          

Replicate the "Projectile Go Round" Quest with an addition -> Sub Projectiles are working                                              

Currently Disabled Tower/Upgrades to avoid issues:                                              
-Alchemist                                                                                            
-Plasma Accelerator / Ray of Doom                                              
-Paragon Green Sentry (Master Builder)                                              
-Beast Handler                                              

Currently Disabled Tower/Upgrades to avoid lag:                                              
Sky Shredder Anti Moab Missile Attack                                                                                            

[![Requires BTD6 Mod Helper](https://raw.githubusercontent.com/gurrenm3/BTD-Mod-Helper/master/banner.png)](https://github.com/gurrenm3/BTD-Mod-Helper#readme)

## Building

Build against a BloonsTD6 installation with MelonLoader and Mod Helper installed.
Launch the game once to generate the interop assemblies, then run:

```sh
dotnet build -c Release -p:GameDir="/path/to/BloonsTD6"
```

Install `bin/Release/net6.0/ProjectileGoRound.dll` into the game's `Mods` folder.
Building without `GameDir` continues to use `../btd6.targets`.
