var express = require('express');
var logger = require('morgan');
var bodyParser = require('body-parser');
var app = express();

app.use(logger('dev'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));


//Please replace these values with your Secret Key and Validation Key found in the SocketLabs Control Panel
var secretKey = "YOUR-SECRET-KEY",
    validationKey = "YOUR-VALIDATION-KEY";

//This handles how our application will respond to POST requests to the '/notifications' URL
app.post('/notifications', function(req,res){

    //This if statement checks to make sure that the correct secret key is being passed with the POST.
    if(req.body.SecretKey === secretKey) {
        //If the secret key was correct, we loop through each parameter in the POST body
        for (var propName in req.body) {
            //Since the POST body is converted to a Javascript object, we use the "hasOwnProperty" method to make sure that we are not using properties inherited from Object
            if (req.body.hasOwnProperty(propName)) {
                //For our example application, we are just logging each parameter from the POST to the console.  In the real world, you'd probably write this information to a DB such as MongoDB.
                console.log(propName, req.body[propName]);
            }
        }
        //Here our application responds back to the SocketLabs server with our validation key.
        res.end(validationKey);
    }
    //If the secret key in the POST does not match your secret key, do not accept the notification.
    else {
        res.end("Invalid Secret Key")
    }
});

//This starts our application on port 3000.  When you validate your application in the SocketLabs Control Panel, please remember to include the port number in the URL.
app.listen(3000, function(){
  console.log("Started on PORT 3000")
})


// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error('Not Found');
  err.status = 404;
  next(err);
});

// error handlers

// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
  app.use(function(err, req, res, next) {
    res.status(err.status || 500);
    res.render('error', {
      message: err.message,
      error: err
    });
  });
}

// production error handler
// no stacktraces leaked to user
app.use(function(err, req, res, next) {
  res.status(err.status || 500);
  res.render('error', {
    message: err.message,
    error: {}
  });
});


module.exports = app;
