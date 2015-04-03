##Configure Token##

### Getting the Token ###
You will need to tell Trello to allow us to send updates.  Go to the below link and click the **Allow** button to create the authorization token. 

[Authorize in Trello]({0})

[![Authorize in Trello](Assets/Trello_Authorize.png)]({0})

You will be redirected to a page displaying your brand new shiny token

![Trello Token](Assets/Trello_Token.png)

### Setting the Token ###

Log on to the [Azure Management Portal](https://manage.windowsazure.com/)  and go to the configuration tab for this website. 

![](Assets/Azure_Config.png)


Scroll down to the **app settings** section and enter a new setting 

- **KEY**:  Trello.Token
- **VALUE**:  [the token you got from Trello]

![](Assets/Azure_AppSettings.png)


*Note*: Trello.Key was filled in during our previous step.

### Next Steps ###
Once you complete this step, refresh the page.  If you did everything right you will see the next steps. 