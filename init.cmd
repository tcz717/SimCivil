@echo Init for windows environment
@pushd %~dp0
@set SCROOT=%~dp0
@start %SCROOT%\SimCivil.sln
@popd
@echo Init complete