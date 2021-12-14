# Overview

The aim of this project is to build a telemetry pipeline POC that specifically uses the Open Telemetry (OTEL) collector agent to get logs, metrics, & APM instrumentation data through to a kafka streaming layer and then through to various telemetry solutions such as Elasticsearch and Splunk using Kafka Connect and/or a heavy forwarder such as Logstash.

# Diagram

![telemetry pipeline](https://user-images.githubusercontent.com/10783372/145910644-96fbff84-759b-48a4-82df-db92f6601f06.png)

# Things to do

* Enable Elasticsearch & Kibana containers in the docker-compose file (these are currently commented out).
* Update OTEL contrib image so that it can send logs to Kafka in JSON format.
* Add Kafka Connect container to docker compose file `docker-compose.otel-agent_kafka-connect_elastic.yml`. This should use the Elasticsearch sink connector.
* Implement ingest pipeline in Elasticsearch. This will need to; a) handle bulk messages from Kafka, and b) format log messages appropriately.
* Implement metrics in OTEL agent using prometheus receiver.
* Add Elastic APM container to the docker-compose file.
* Add auto-instrumentation to the .NET Core microservice Dockerfile. This should be configured to send traces to the OTEL agent using the OTLP format.
* Implement traces in OTEL agent using the otlp receiver. Note: AFAIK we should use protobuf for traces, which is supported by Elastic APM.

# Pre-requisites

* docker
* docker-compose

# Running the project

```
docker-compose -f ./docker-compose.otel-agent_kafka-connect_elastic.yml up
```
