FROM microsoft/dotnet:2.1-sdk AS dotnet

FROM dotnet AS restore
COPY SimCivil.Contract/SimCivil.Contract.csproj SimCivil.Contract/SimCivil.Contract.csproj
COPY SimCivil.Gate/SimCivil.Gate.csproj SimCivil.Gate/SimCivil.Gate.csproj
COPY SimCivil.Orleans.Grains/SimCivil.Orleans.Grains.csproj SimCivil.Orleans.Grains/SimCivil.Orleans.Grains.csproj
COPY SimCivil.Orleans.Interfaces/SimCivil.Orleans.Interfaces.csproj SimCivil.Orleans.Interfaces/SimCivil.Orleans.Interfaces.csproj
COPY SimCivil.Orleans.Server/SimCivil.Orleans.Server.csproj SimCivil.Orleans.Server/SimCivil.Orleans.Server.csproj
COPY SimCivil.Rpc/SimCivil.Rpc.csproj SimCivil.Rpc/SimCivil.Rpc.csproj
RUN dotnet restore SimCivil.Orleans.Server

FROM restore AS build
COPY . .
RUN dotnet publish -f netcoreapp2.0 SimCivil.Orleans.Server

FROM dotnet AS entry
EXPOSE 20170
WORKDIR /silo
COPY --from=build SimCivil.Orleans.Server/bin/Debug/netcoreapp2.0/publish .
ENTRYPOINT ["dotnet", "SimCivil.Orleans.Server.dll"]