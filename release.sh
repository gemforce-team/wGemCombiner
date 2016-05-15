#!/bin/bash

### wGC release script ###

buildfolder="WGemCombiner/bin/x86/Release"
xbuild /p:win32icon=Resources/graphics/GCIcon.ico /p:Configuration=Release

version=$(git describe --tags | rev | cut -d '-' -f2- | rev)
releasefolder="WGemCombiner-${version}"

mkdir -p "${releasefolder}"
cp ${buildfolder}/WGemCombiner.exe  ${releasefolder}/WGemCombiner.exe
cp LICENSE.txt                      ${releasefolder}/LICENSE.txt
cp README.md                        ${releasefolder}/README.txt
cp WGemCombiner/recipes.txt         ${releasefolder}/recipes.txt

echo "Building archive"
zip -rTm "${releasefolder}.zip" "${releasefolder}"
