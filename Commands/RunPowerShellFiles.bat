@echo off

pushd "%~dp0"

powershell -file UpdateDatabase.ps1 C:\Git\DatabaseScript dbserver dbinstance dbuser userpassword 1

pause

popd
