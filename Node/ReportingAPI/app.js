var request = require('request'),

    userName = "YOUR-USERNAME", //The username you use to log into the SocketLabs On-Demand Control Panel
    apiKey = "YOUR-API-KEY", //Your SocketLabs Reporting API key
    serverId = "XXXXX", //Your 4 or 5 digit ServerID number
    method = "messagesFailed"; //The API method to call.  Options are accountData, messagesFailed, messagesQueued, messagesProcessed, messagesFblReported, messagesOpenClick

request.get('https://api.socketlabs.com/v1/' + method, {
    'auth': {
        'user': userName,
        'pass': apiKey,
        'sendImmediately': true
    },
    'qs': { //The query string builder.  Please note that these fields are associated with the failedMessages method.  For the correct fields for the method you choose, please see the API documentation at http://www.socketlabs.com/api-reference/reporting-api/

        "serverId":serverId,
        "type":"JSON",
        "count":"",         //Optional field, specifies the number of results to return (500 maximum)
        "endDate":"",       //Optional field, specifies the end date for the result set (format yyyy-mm-dd hh:mm:ss)
        "index":"",         //Optional field, specifies the zero based index of the first record to return from the result set
        "mailingId":"",     //Optional field, only records matching the specified mailingId will be returned
        "messageId":"",     //Optional field, only records matching the specified messageId will be returned
        "startDate":"",     //Optional field, specifies the start date for the result set (format yyyy-mm-dd hh:mm:ss)
        "timeZone":""       //Optional field, specifies the timeZone offset that will be used by the API (format see http://www.socketlabs.com/api-reference/reporting-api/#timezones)
    },
    'json': 'true'
}, function(error, response, body){
    console.log("Response: " + response.statusCode + " " + response.statusMessage);
    console.log(body)
});

