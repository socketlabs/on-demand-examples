## SocketLabs Email On-Demand Notification API Endpoint in Node.js

This is an example application that can be used as an endpoint for the [SocketLabs Email On-Demand Notification API](www.socketlabs.com/api-reference/notification-api/).  The application uses [Node](https://nodejs.org/) and [Express](http://expressjs.com/) to create a simple endpoint that can listen for and process HTTP notifications from our platform.  The application will listen for POST requests on port 3000 at your application's URL/notifications, for example `http://api.example.com:3000/notifications` 

To get started, you must first navigate to the Notification API configuration page in the [SocketLabs Email On-Demand Control Panel](https://cp.socketlabs.com).  From here, you'll need to grab your Secret Key and Validation key, and plug those keys into the Node application's app.js file (we have included the placeholder text where you will insert these keys).

Once you have updated the app.js file to include your Secret Key and Validation Key, you are ready to upload the application to your web server or hosting provider.  You can start the application by running the command `node app.js` in your web server's console or terminal (please be sure that you are in the directory that contains the app.js file for this application).

The final step is to validate your application in the SocketLabs Control Panel.  Navigate back to the Notification API Configuration page, and enter your application's URL.  Please be sure to include the port number and the /notifications route.  If everything was configured correctly, your application should now be validated, and you can choose which types of events to receive notifications for.  Finally, click Update to confirm your settings.

###Other notes:
* The application will accept any parameter that is passed along with the notification.  This allows for flexibility of the application should there be any future changes to the parameters included with any notifications.
* This demo application will simply accept the notification, and log all of the data from that notification to the console.  In a "real world" use case, you'd want to write these notifications to a database such as [MongoDB](https://www.mongodb.org/), or write some other sort of logic for handling and processing the notifications.