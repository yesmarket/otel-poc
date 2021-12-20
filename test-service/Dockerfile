# no official release has been created for the opentelemetry-dotnet-instrumentation repo, so manual build is requried for installation
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS tracer

WORKDIR /tmp

RUN apt-get update && \
    apt-get -y install g++ clang cmake

RUN git clone https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation.git

RUN cd opentelemetry-dotnet-instrumentation && \
    dotnet tool restore && \
    dotnet nuke

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

WORKDIR /tmp

COPY ./*.sln ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

COPY ./nuget.docker.config ./NuGet.Config
RUN dotnet restore ./TestService.WebApi/TestService.WebApi.csproj

COPY . ./

RUN dotnet publish ./TestService.WebApi/TestService.WebApi.csproj -c Release --self-contained false -o /opt/TestService.WebApi

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime

ARG CONFIG_DIR=/etc/opt/TestService.WebApi/
ARG TRACER_VERSION=1.24.0
ENV ASPNETCORE_URLS=http://*:5001
ENV CONFIG_DIR $CONFIG_DIR

EXPOSE 5001

COPY --from=build /opt/TestService.WebApi/ /opt/TestService.WebApi/
COPY --from=build /opt/TestService.WebApi/appsettings*.json $CONFIG_DIR

RUN mkdir -p /opt/opentelemetry && \
    mkdir -p /var/log/opentelemetry/dotnet

COPY --from=tracer /tmp/opentelemetry-dotnet-instrumentation/bin/tracer-home/ /opt/opentelemetry/

ENV COR_ENABLE_PROFILING=1 \
    COR_PROFILER={918728DD-259F-4A6A-AC2B-B85E1B658318} \
    COR_PROFILER_PATH=/opt/opentelemetry/OpenTelemetry.ClrProfiler.Native.so \
    CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={918728DD-259F-4A6A-AC2B-B85E1B658318} \
    CORECLR_PROFILER_PATH=/opt/opentelemetry/OpenTelemetry.ClrProfiler.Native.so \
    OTEL_DOTNET_TRACER_HOME=/opt/opentelemetry/ \
    OTEL_PROFILER_PROCESSES=dotnet \
    OTEL_SERVICE_NAME=test \
    #OTEL_INTEGRATIONS=/opt/opentelemetry/integrations.json \
    OTEL_DOTNET_TRACER_INSTRUMENTATIONS=HttpClient,AspNet \
    OTEL_DOTNET_TRACER_DISABLED_INSTRUMENTATIONS=SqlClient,MongoDb \
    OTEL_TRACE_LOG_DIRECTORY=/var/log/opentelemetry/dotnet \
    OTEL_DOTNET_TRACER_CONSOLE_EXPORTER_ENABLED=true
    #OTEL_EXPORTER=otlp \
    #OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317

WORKDIR /opt/TestService.WebApi

ENTRYPOINT ["dotnet", "TestService.WebApi.dll"]
