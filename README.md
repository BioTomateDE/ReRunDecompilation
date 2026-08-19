# RE:RUN Decompilation

A C# project decompilation of the Unity game [RE:RUN](https://danidev.itch.io/rerun) by DaniDev.

This only includes the C# logic in `RERUN_Data/Managed/Assembly-CSharp.dll` and is not a full Unity project decompilation.

## How to use
1. Go into your root game directory (where `RERUN.exe` is located).
1. Clone this repository: `git clone https://github.com/BioTomateDE/ReRunDecompilation DECOMPILED && cd DECOMPILED`
3. Modify the code in the `src` directory however you like!
4. Compile the project with `dotnet build`.
5. Replace the `Assembly-CSharp.dll` with the freshly compiled version: `cp bin/Debug/netstandard2.0/Assembly-CSharp.dll ../RERUN_Data/Managed/Assembly-CSharp.dll`.
6. Run the game!

There is a helper shell script to automatically compile the project and replace the DLL: [`patch.sh`](patch.sh).

### Compilation Errors
If you get lots of errors like this, then .NET can't find Unity libraries.
```text
error CS0246: The type or namespace name 'MonoBehaviour' could not be found (are you missing a using directive or an assembly reference?)
```
Make sure you are in the correct directory: there should be a directory called `RERUN_Data` in the parent directory of this repo.
This path should exist: `../RERUN_Data/Managed/UnityEngine.UI.dll`. You can also adjust the `<HintPath>` nodes in [`RERUN.csproj`](RERUN.csproj) to point at the correct DLL locations.

## Linux Support
Just follow the normal steps, it should work out of the box.
Be aware that the `RERUN_Data` directory is called `RERUN_linux_Data` instead.
> [!NOTE]
> The Linux release of RE:RUN is very buggy. I recommend using the Windows release with Wine.

## Decompilation Notes
I decompiled this C# game using [AssetRipper](https://github.com/AssetRipper/AssetRipper).
This repo only covers the `Assembly-CSharp.dll` which is located in `Scripts/Assembly-CSharp/`
in your export directory after pressing *Export Primary Content*.

Local variables were renamed to something more sensible, since their names were lost during compilation.
Almost all local variables start with an underscore now to differentiate them from member variables (Dani's naming conventions are terrible).
A few local variable names do *not* start with an underscore:
* Collision components `other` (from `OnCollisionEnter` methods)
* Loop variables `i`/`j`
* All local variables in static methods (since they can't access member variables)

I also made some small potential behavioral changes and a better <kbd>Tab</kbd> console.
If you want the exact original behavior, do `git checkout 6f6e3c4482d898000d39be3005a23014debdf763`.

## License
This repository contains modified decompiled source code from **RE:RUN** (copyright holder **DaniDev**).
I am not affiliated with Dani or itch.io!
If you are a rights holder and have concerns, please contact [legal@biotomate.dev](mailto:legal@biotomate.dev?subject=go%20fuck%20yourself%2C%20don%27t%20decompile%20my%20game).
