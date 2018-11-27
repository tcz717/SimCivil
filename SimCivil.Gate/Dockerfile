FROM microsoft/dotnet:2.1-sdk AS base

EXPOSE 20170

COPY . .
RUN dotnet build SimCivil.Gate
WORKDIR SimCivil.Gate
ENTRYPOINT ["dotnet", "run", "SimCivil.Gate.csproj"]
CMD ["-f", "netcoreapp2.0"]