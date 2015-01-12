# Using this Example Notification API Endpoint

By following these steps, you will be able to test the Socketlabs Email On-Demand Notification API, confirm that we have access to this endpoint running on your server, and that authentication is configured correctly.

After this project is built and deployed, the following should be done in order to use it.

1. Add authentication information to the Web.config file. This invovles setting the Server Id, Secret Key, and Validation Key fields so that they match your Email On-Demand server's Notification API settings.

2. Validate the endpoint from the Email On-Demand Control Panel's Notification API page. The URL should look similar to 'http://10.0.0.1/NotificationEndpoint' with your domain or IP address in place of 10.0.0.1.

3. In a browser, visit the domain or IP that the endpoint is running on in order to see state information (e.g. http://10.0.0.1/).

4. Generate Notification API events by sending and opening email message through your account. JSON describing these events is viewable via the web site mentioned in the step above.

Now that the example endpoint is working, we recommend writing a production endpoint (potentially based on this example) to handle your Notification API events.