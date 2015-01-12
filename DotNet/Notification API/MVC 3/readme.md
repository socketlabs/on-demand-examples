#### SocketLabs Email On-Demand Notifications API Endpoint

This example contains an ASP.NET MVC 3 project that has been configured to work as an Endpoint for receiving notification events from the SocketLabs Mail Servers.

The Notifications API allows real-time notifications for message Delivery, Failures, and Feedback Loops (Complaints).

This sample uses an in-memory repository via static collection to store the data sent over from the notifications API.  If you are just testing, you could get this endpoint up and running in IIS directly and start accepting notification events from our system.

In the long term, we recommend storing the data in a database or file repository for you to later process.