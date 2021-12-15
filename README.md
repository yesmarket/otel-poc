# Overview

The aim of this project is to build a telemetry pipeline POC that specifically uses the Open Telemetry (OTEL) collector agent to get the logs, metrics, & APM instrumentation data from a dummy microservice to a kafka streaming layer and then through to various telemetry solutions such as Elasticsearch and Splunk using Kafka Connect (or a heavy forwarder such as Logstash).

# Diagram

![telemetry pipeline](https://user-images.githubusercontent.com/10783372/145910644-96fbff84-759b-48a4-82df-db92f6601f06.png)

# Things to do

* Implement ingest pipeline in Elasticsearch. This will need to; a) handle bulk messages from Kafka, and b) format log messages appropriately. Note: potentially need to upgrade Elasticsearch, Kibana, and the Kafka Connect Elasticsearch sink connector.
* Add Elastic APM container to the docker-compose file.
* Add auto-instrumentation to the .NET Core microservice Dockerfile. This should be configured to send traces to the OTEL agent using the OTLP format.
* Implement traces in OTEL agent using the otlp receiver. Note: AFAIK we should use protobuf for traces, which is supported by Elastic APM.
* Make otel agent retrieve microservice logs (using file log receiver) from docker host instead of container bind mount. Low priority.

# Pre-requisites

* docker
* docker-compose

# Running the project

```
docker-compose -f ./docker-compose.otel-agent_kafka-connect_elastic.yml up
```
