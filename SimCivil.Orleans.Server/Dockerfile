FROM microsoft/dotnet:2.1-sdk AS base

EXPOSE 30000

COPY . .
RUN dotnet build SimCivil.sln
WORKDIR SimCivil.Orleans.Server
ENTRYPOINT ["dotnet", "SimCivil.Orleans.Server.csproj"]