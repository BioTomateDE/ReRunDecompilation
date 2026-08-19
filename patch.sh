#!/bin/sh
# Script licensed under GPL-3.0-only
set -eu
if [ -d ../RERUN_Data ]; then
    game=../RERUN_Data/Managed/Assembly-CSharp.dll
elif [ -d ../RERUN_linux_Data ]; then
    game=../RERUN_linux_Data/Managed/Assembly-CSharp.dll
else
    echo "Cannot find RE:RUN data directory"
    exit 1
fi
built=bin/Debug/netstandard2.0/Assembly-CSharp.dll
dotnet build --property WarningLevel=0
cp $built $game
echo "Patched game!"
