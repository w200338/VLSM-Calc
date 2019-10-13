"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" /consoleloggerparameters:ErrorsOnly /maxcpucount /nologo /property:Configuration=Release /verbosity:quiet "VLSM Calc.sln"
move /Y "VLSM Calc\bin\Release\app.publish\VLSM Calc.exe" "VLSM Calc.exe"
PAUSE