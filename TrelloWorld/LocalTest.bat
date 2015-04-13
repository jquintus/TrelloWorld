@echo Sending: 
@cat SampleCommit.json

curl localhost:8696 -X POST -d @SampleCommit.json -H "Content-Type: application/json"
