FROM microsoft/dotnet:2.1-sdk AS dotnet-mono

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --batch --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian stable-stretch main" > /etc/apt/sources.list.d/mono-official.list \
 && apt-get update \
&& apt-get install -y mono-runtime \
&& rm -rf /var/lib/apt/lists/* /tmp/*	

RUN apt-get update \
 && apt-get install -y binutils curl mono-devel ca-certificates-mono mono-vbnc nuget referenceassemblies-pcl \
&& rm -rf /var/lib/apt/lists/* /tmp/*

FROM dotnet-mono AS restore
COPY SimCivil.Contract/SimCivil.Contract.csproj SimCivil.Contract/SimCivil.Contract.csproj
COPY SimCivil.Gate/SimCivil.Gate.csproj SimCivil.Gate/SimCivil.Gate.csproj
COPY SimCivil.Orleans.Grains/SimCivil.Orleans.Grains.csproj SimCivil.Orleans.Grains/SimCivil.Orleans.Grains.csproj
COPY SimCivil.Orleans.Interfaces/SimCivil.Orleans.Interfaces.csproj SimCivil.Orleans.Interfaces/SimCivil.Orleans.Interfaces.csproj
COPY SimCivil.Orleans.Server/SimCivil.Orleans.Server.csproj SimCivil.Orleans.Server/SimCivil.Orleans.Server.csproj
COPY SimCivil.Rpc/SimCivil.Rpc.csproj SimCivil.Rpc/SimCivil.Rpc.csproj
RUN msbuild -target:restore SimCivil.Gate

FROM restore AS build
COPY . .
RUN msbuild SimCivil.Gate

FROM dotnet-mono AS entry
EXPOSE 20170
WORKDIR /gate
COPY --from=build SimCivil.Gate/bin/Debug/net461/ .
ENTRYPOINT ["mono", "SimCivil.Gate.exe"]