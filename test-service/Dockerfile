FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ARG FLEXIGROUP_UNIVERSE_ARTIFACTS_URL='https:\/\/flexigroup.pkgs.visualstudio.com\/_packaging\/Flexigroup-Universe\/nuget\/v3\/index.json'
ARG FLEXIGROUP_ARTIFACTS_USER
ARG FLEXIGROUP_ARTIFACTS_USER_TOKEN

WORKDIR /tmp

COPY ./*.sln ./
COPY ./nuget.docker.config ./NuGet.Config

RUN sed -i "s@#{Flexigroup_Universe_artifacts_url}@${FLEXIGROUP_UNIVERSE_ARTIFACTS_URL}@g" ./NuGet.Config
RUN sed -i "s/#{Flexigroup_artifacts_user}/${FLEXIGROUP_ARTIFACTS_USER}/g" ./NuGet.Config
RUN sed -i "s/#{Flexigroup_artifacts_user_token}/${FLEXIGROUP_ARTIFACTS_USER_TOKEN}/g" ./NuGet.Config

COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

RUN dotnet restore ./TestService.WebApi/TestService.WebApi.csproj

COPY . ./

RUN dotnet publish ./TestService.WebApi/TestService.WebApi.csproj -c Release --self-contained false -o /opt/TestService.WebApi

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

ARG CONFIG_DIR=/etc/opt/TestService.WebApi/
ENV CONFIG_DIR $CONFIG_DIR
ENV ENVIRONMENT=dev
ENV ASPNETCORE_URLS=http://*:5001

EXPOSE 5001

COPY --from=build /opt/TestService.WebApi/ /opt/TestService.WebApi/
COPY --from=build /opt/TestService.WebApi/appsettings*.json $CONFIG_DIR

WORKDIR /opt/TestService.WebApi

ENTRYPOINT ["dotnet", "TestService.WebApi.dll"]
