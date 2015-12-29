#!/bin/bash

# wGC release script
# you need to copy wGC .exe in the repo root
# the compilation command with mono is:
# xbuild /p:win32icon=Resources/graphics/GCIcon.ico /p:Configuration=Release


if [ ! -e "WGemCombiner.exe" ]; then
	echo "No wGC executable found, aborting..."
	exit 1
fi

version=$(git describe --tags | cut -f1-2 -d "-")
foldername="WGemCombiner-${version}"

mkdir -p "${foldername}"
cp WGemCombiner.exe         ${foldername}/WGemCombiner.exe
cp LICENSE.txt              ${foldername}/LICENSE.txt
cp README.md                ${foldername}/README.txt
cp WGemCombiner/recipes.txt ${foldername}/recipes.txt

echo "Building archive"
zip -r "${foldername}.zip" "${foldername}"
