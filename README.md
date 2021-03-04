docker build -t hummgroup/test-service ./test-service

docker run -p 5001:5001 -e WAIT_FOR_IT_URL=http://192.168.99.100:8000 --log-driver=splunk --log-opt splunk-token=abcd1234 --log-opt splunk-url=http://192.168.99.100:8088 hummgroup/test-service

curl -k http://192.168.99.100:8088/services/collector -H 'Authorization: Splunk abcd1234' -d '{\"event\": \"Hello from event collector\"}'
