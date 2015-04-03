# TrelloWorld[![Build Status](https://travis-ci.org/jquintus/TrelloWorld.svg)](https://travis-ci.org/jquintus/TrelloWorld)

A .Net web service to add a comment to a Trello card when committing from Git.


##  Make sure to configure the Trello app key in Azure  ##

### Getting the App Key ###

The app key can be found on [Trello's Developer Page](https://trello.com/app-key) 

Paste the value found in the **Key** section. 

![Trello's Developer Page](Assets/Trello_Developer_Page.png)

### Setting the App Key ###
Log on to the [Azure Management Portal](https://manage.windowsazure.com/)  and go to the configuration tab for this website. 

![](Assets/Azure_Config.png)


Scroll down to the **app settings** section and enter a new setting 

- **KEY**:  Trello.Key
- **VALUE**:  [the key you got from Trello]

![](Assets/Azure_AppSettings.png)


*Note*: Trello.Token will be filled in during the next step.

##Configure Token##

### Getting the Token ###
You will need to tell Trello to allow us to send updates.  Go to the below link and click the **Allow** button to create the authorization token. 

[Authorize in Trello](https://trello.com/1/connect?key=your_app_key&name=TrelloWorld&response_type=token&scope=read,write&expiration=never)

[![Authorize in Trello](Assets/Trello_Authorize.png)](https://trello.com/1/connect?key=your_app_key&name=TrelloWorld&response_type=token&scope=read,write&expiration=never)

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