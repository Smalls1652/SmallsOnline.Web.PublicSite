FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env

RUN apt update; \
    apt install -y nodejs python3

RUN curl -qL https://www.npmjs.com/install.sh | sh
RUN dotnet dotnet workload install wasm-tools; \
    dotnet tool install --global PowerShell

WORKDIR /app
COPY . ./

RUN dotnet msbuild ./src/SmallsOnline.Web.PublicSite/Client/ -t:"InitProject_Combined"
RUN dotnet restore ./src/SmallsOnline.Web.PublicSite/Server/
RUN dotnet publish ./src/SmallsOnline.Web.PublicSite/Server/ --configuration Release --output out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "SmallsOnline.Web.PublicSite.Server.dll" ]
EXPOSE 80