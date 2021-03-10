FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /tmp

COPY ./*.sln ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

COPY ./nuget.docker.config ./NuGet.Config
RUN dotnet restore ./TestService.WebApi/TestService.WebApi.csproj

COPY . ./

RUN dotnet publish ./TestService.WebApi/TestService.WebApi.csproj -c Release --self-contained false -o /opt/TestService.WebApi

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

ARG CONFIG_DIR=/etc/opt/TestService.WebApi/
ARG TRACER_VERSION=1.24.0
ENV CONFIG_DIR $CONFIG_DIR
ENV ASPNETCORE_URLS=http://*:5001

EXPOSE 5001

COPY --from=build /opt/TestService.WebApi/ /opt/TestService.WebApi/
COPY --from=build /opt/TestService.WebApi/appsettings*.json $CONFIG_DIR

# Set up Datadog APM
RUN mkdir -p /var/log/datadog
RUN mkdir -p /opt/datadog
RUN curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb
RUN dpkg -i ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={918728DD-259F-4A6A-AC2B-B85E1B658318}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV OTEL_INTEGRATIONS=/opt/datadog/integrations.json
ENV OTEL_DOTNET_TRACER_HOME=/opt/datadog

#https://docs.datadoghq.com/tracing/setup_overview/setup/dotnet-core/?tab=linux#required-environment-variables
#https://app.datadoghq.com/signup/agent#debian
#RUN apt-get update && apt-get install -y gnupg2
#RUN DD_INSTALL_ONLY=true DD_AGENT_MAJOR_VERSION=7 DD_API_KEY=REDACTED DD_SITE="datadoghq.com" bash -c "$(curl -L https://s3.amazonaws.com/dd-agent/scripts/install_script.sh)"

WORKDIR /opt/TestService.WebApi

ENTRYPOINT ["dotnet", "TestService.WebApi.dll"]
