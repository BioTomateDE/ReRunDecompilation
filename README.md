# RE:RUN Decompilation

A C# project decompilation of the Unity game [RE:RUN](https://danidev.itch.io/rerun) by DaniDev.

This only includes the C# logic in `RERUN_Data/Managed/Assembly-CSharp.dll` and is not a full Unity project decompilation.

## How to use
1. Go into your root game directory (where `RERUN.exe` is located).
1. Clone this repository: `git clone https://github.com/BioTomateDE/ReRunDecompilation DECOMPILED`
3. Modify the code in the `DECOMPILED/src` directory however you like!
4. Go into the `DECOMPILED` directory and compile the project with `dotnet build`.
5. Replace the `Assembly-CSharp.dll` with the freshly compiled version: `cp bin/Debug/netstandard2.0/Assembly-CSharp.dll ../RERUN_Data/Managed/Assembly-CSharp.dll`.
6. Run the game!

### Compilation Errors
If you get lots of errors like `The type or namespace name does not exist (are you missing an assembly reference?)`, then .NET can't find Unity libraries.
Make sure you are in the correct directory. There should be a directory called `RERUN_Data` in the parent directory of this repo.
This path should exist: `../RERUN_Data/Managed/UnityEngine.UI.dll`. You can also adjust the `<HintPath>`s in [`RERUN.csproj`](RERUN.csproj) to point at the correct paths.

## Linux Support
Just follow the normal tutorial but use `RERUN_linux_Data` instead of `RERUN_Data`.
However, the Linux release of RE:RUN is very buggy. It is better to use the Windows release with Wine anyway.

## Decompilation Notes
I decompiled this C# game using [AssetRipper](https://github.com/AssetRipper/AssetRipper).
This repo only covers the `Assembly-CSharp.dll` which is located in `Scripts/Assembly-CSharp/`
in your export directory after pressing *Export Primary Content*.

I renamed all local variable names to something more sensible (since they were lost during compilation).
Almost all local variables start with an underscore now to differentiate them from member variables (Dani's naming conventions are terrible).
Local variables `other` (from `OnCollisionEnter` methods), `i`/`j` (from loops) kept their names because they would look very ugly with underscores.
All local variables in static methods also kept their names (since they can't access member variables).

I also made some small potential behavioral changes and a better TAB console.
If you want the exact original behavior, do `git checkout 938d2a2e73a33d86840116276b3b3a4a96cf75b1`.

## License
This repository contains modified decompiled source code from **RE:RUN** (copyright holder **DaniDev**).
I am not affiliated with Dani or itch.io!
If you are a rights holder and have concerns, please contact [legal@biotomate.dev](mailto:legal@biotomate.dev?subject=go%20fuck%20yourself%2C%20don%27t%20decompile%20my%20game).
