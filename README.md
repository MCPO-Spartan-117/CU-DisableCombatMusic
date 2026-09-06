# Repair

A small BepInEx plugin for Casualties Unknown to modify `MusicManager::Update` to remove the code that starts the combat music.

## Overview 
Currently simply just removes code.

## Build
1. (Optional) Mod source can be placed inside of the game directory, (game/dir/mod)
2. Open the project in Visual Studio, JetBrains Rider or use the dotnet SDK CLI. (Or any other IDE)
3. Build `ScavTemplate/Template.csproj` via Ctrl + Shift + B (`dotnet build`)
4. If auto-detection misses your setup, open the linked `vars.targets` file from the project and override `BaseGamePath`
