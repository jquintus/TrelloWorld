@echo Sending: 
@cat SampleCommit.json

curl trelloworld.azurewebsites.net -X POST -d @SampleCommit.json -H "Content-Type: application/json"
