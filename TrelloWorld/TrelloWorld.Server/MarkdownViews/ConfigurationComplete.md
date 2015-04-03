##Setup Complete!##
Now that the TrelloWorld service is set up and running, all you need to do is set up a service to call it.  It was written with GitHub's Webhooks in mind.

###GitHub Integration###
[Webhook Documentation on GitHub](https://developer.github.com/webhooks/creating/#setting-up-a-webhook)

1. Go to the GitHub project you want to connect to Trello and click the **Settings** button.

  ![](Assets/GitHub_OpenSettings.png)

2. Click on the **Webhooks & Services link on the left**.

  ![](Assets/GitHub_OpenWebhooks.png)

3. Click the **Add webhook** button on the top right.

  ![](Assets/GitHub_AddWebHook.png)

4.  Configure the webhook to match what we have below

  ![](Assets/GitHub_ConfigureWebhook.png)

  * **Payload URL**:  The path to the TrelloWorld service running on Azure
  * **Content type**:  application/json
  * **Secret**:  This is not used, you can leave it blank or add any value you want here.  It will be ignored.
  * **Which events would you like to trigger this webhook?**:  Any option will work, but **Let me select individual events** is the best.  Other options will call your service more often than needed; the extra calls are just ignored.
  * Which events to send
    * **Push**
  * **Active**:  Ensure this is checked

### Test the Setup ###
Now that everything is setup, you can test it by committing and pushing a change to your repo with a comment like this:
  
    Testing TrelloWorld
    Trello([TrelloCardId]

Then log in to trello and verify that the card has a comment added.
