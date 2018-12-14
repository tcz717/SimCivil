if [[ -z "${DOTNETCORE}" ]]; then
	dotnet test SimCivil.Test --filter "Category!=Performance"
else
	msbuild SimCivil.Test
	mono ./testrunner/xunit.runner.console.*/tools/net461/xunit.console.exe ./SimCivil.Test/bin/Debug/net461/SimCivil.Test.dll
fi