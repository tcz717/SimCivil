if [[ ${DOTNETCORE} ]]; then
	# dotnet test SimCivil.Test --filter "Category!=Performance" -f netcoreapp2.0
	dotnet build SimCivil.Test -f netcoreapp2.0 -p:RunTool=false
	dotnet ./xunit.runner.console.*/tools/netcoreapp2.0/xunit.console.dll   \
	./SimCivil.Test/bin/Debug/netcoreapp2.0/SimCivil.Test.dll               \
	-maxthreads unlimited -notrait "Category=Performance"
else
	msbuild SimCivil.Test -p:RunTool=false
	mono ./testrunner/xunit.runner.console.*/tools/net461/xunit.console.exe \
	./SimCivil.Test/bin/Debug/net461/SimCivil.Test.dll                      \
	-maxthreads unlimited -notrait "Category=Performance"
fi