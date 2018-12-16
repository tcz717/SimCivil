FROM microsoft/dotnet:2.1-sdk AS dotnet-mono

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --batch --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian stable-stretch main" > /etc/apt/sources.list.d/mono-official.list \
 && apt-get update \
&& apt-get install -y mono-runtime \
&& rm -rf /var/lib/apt/lists/* /tmp/*	

RUN apt-get update \
 && apt-get install -y binutils curl mono-devel ca-certificates-mono mono-vbnc nuget referenceassemblies-pcl \
&& rm -rf /var/lib/apt/lists/* /tmp/*

FROM dotnet-mono AS base

EXPOSE 20170

COPY . .
RUN msbuild /restore SimCivil.Gate
WORKDIR SimCivil.Gate/bin/Debug/net461/
ENTRYPOINT ["mono", "SimCivil.Gate.exe"]