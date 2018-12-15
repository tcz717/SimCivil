FROM microsoft/dotnet:2.1-sdk AS base

EXPOSE 20170

COPY . .
RUN dotnet build SimCivil.sln
WORKDIR SimCivil.Gate
ENTRYPOINT ["dotnet", "SimCivil.Gate.csproj"]