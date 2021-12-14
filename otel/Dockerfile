FROM otel/opentelemetry-collector-contrib
#FROM opentelemetry-collector-contrib-ryan
COPY ./otel-agent-config.yaml /etc/
CMD ["--config=/etc/otel-agent-config.yaml"]
