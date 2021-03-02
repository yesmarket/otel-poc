FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /tmp

COPY ./*.sln ./*/*.csproj ./*.sh ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

COPY ./nuget.docker.config ./NuGet.Config
RUN dotnet restore ./TestService.WebApi/TestService.WebApi.csproj

COPY . ./

RUN dotnet publish ./TestService.WebApi/TestService.WebApi.csproj -c Release --self-contained false -o /opt/TestService.WebApi

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

ARG CONFIG_DIR=/etc/opt/TestService.WebApi/
ENV CONFIG_DIR $CONFIG_DIR
ENV ASPNETCORE_URLS=http://*:5001

EXPOSE 5001

COPY --from=build /opt/TestService.WebApi/ /opt/TestService.WebApi/
COPY --from=build /opt/TestService.WebApi/appsettings*.json $CONFIG_DIR

WORKDIR /opt/TestService.WebApi

ENTRYPOINT ["dotnet", "TestService.WebApi.dll"]
