docker build -t hummgroup/test-service --build-arg FLEXIGROUP_UNIVERSE_ARTIFACTS_URL=REDACTED --build-arg FLEXIGROUP_ARTIFACTS_USER=REDACTED --build-arg FLEXIGROUP_ARTIFACTS_USER_TOKEN=REDACTED ./test-service

docker run -p 5001:5001 --log-driver=splunk --log-opt splunk-token=abcd1234 --log-opt splunk-url=http://localhost:8088 hummgroup/test-service

curl -k https://localhost:8088/services/collector -H 'Authorization: Splunk abcd1234' -d '{\"event\": \"Hello from event collector\"}'