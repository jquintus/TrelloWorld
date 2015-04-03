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

### Next Steps ###
Once you complete this step, refresh the page.  If you did everything right you will see the next steps. 