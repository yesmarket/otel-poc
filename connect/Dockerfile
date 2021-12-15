FROM curlimages/curl as temp

WORKDIR /tmp

RUN curl -o ./confluentinc-kafka-connect-elasticsearch-10.0.3.zip https://d1i4a15mxbxib1.cloudfront.net/api/plugins/confluentinc/kafka-connect-elasticsearch/versions/10.0.3/confluentinc-kafka-connect-elasticsearch-10.0.3.zip
RUN unzip ./confluentinc-kafka-connect-elasticsearch-10.0.3.zip

FROM debezium/connect:1.0 as connect

COPY --from=temp /tmp/confluentinc-kafka-connect-elasticsearch-10.0.3/lib ./connect/elasticsearch
COPY ./connect-elasticsearch-sink.properties ./config

USER root
RUN sed -i 's/distributed/standalone/g' /docker-entrypoint.sh
#RUN sed -i '/\s*exec/s/$/ .\/config\/connect-elasticsearch-sink.properties/' /docker-entrypoint.sh
USER kafka
