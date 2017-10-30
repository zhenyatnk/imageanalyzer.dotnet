@echo off
pushd "%~dp0"

set build_path=.build
set disk_path=""
if not "%1" == "" (set build_path=%1 
set disk_path=%~d1)

IF NOT EXIST %build_path% mkdir %build_path%

if not %disk_path% == "" (%disk_path%)

cd %build_path%

if not "%2" == "" (cmake %~dp0 -DINSTALL_DIRECTORY="%2") else (cmake %~dp0)

cmake --build . --target install

popd 