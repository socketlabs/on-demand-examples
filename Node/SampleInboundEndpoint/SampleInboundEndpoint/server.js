/*
    Use the Node HTTP server to handle requests, process incoming messages (count words
    and save to DB), and serve up our viewable 'site' page that has the chart (with socket.io
    to update the chart over WebSockets).
*/

'use strict';

var http = require('http'),
    fs = require('fs'),
    TextProcessor = require('./TextProcessor'),
    MongoClient = require('mongodb').MongoClient,
    io = require('socket.io');

// TODO: Set your validation key value for your server here
var VALIDATION_KEY = 'My_server_validation_key_goes_here',
    persistToDb = function(data) {
                MongoClient.connect("mongodb://localhost:27017/salesEmails", function(err, db) {
                    if (err) {
                        return console.dir(err);
                    }
                    var collection = db.collection('parsedMessages');
                    collection.insert(data, function(err, inserted) {
                        console.log(err || inserted);
                    });
                });
            },
    processInboundMessage = function(request, response, wordCountHash) {
        var postData = '';

        request.on('data', function(chunk) {
            postData += chunk;
        });

        request.on('end', function() {
            var arrayOfWords, data = JSON.parse(postData),
                // If HTML body exists, use that. Otherwise fall back to text body
                text = data.Message.HtmlBody ? data.Message.HtmlBody : data.Message.TextBody;
            //console.log(data.Message.HtmlBody);
            response.writeHead(200, {
                'Content-Type': 'text/plain'
            });
            response.write(VALIDATION_KEY);
            response.end();

            arrayOfWords = TextProcessor.parse(text);
            TextProcessor.countWords(arrayOfWords, wordCountHash);
            console.log(wordCountHash);

            // Push to client websites, and save to DB
            io.sockets.emit('inboundParsed', wordCountHash);
            persistToDb(data);
        });
    },
    wordCountHash = {},
    fileToContentTypes = {
        js: 'text/javascript',
        css: 'text/css'
    },
    getContentType = function(url) {
        var splitUrl = url.split('.'),
                fileExtension = splitUrl[splitUrl.length - 1];
        if (fileToContentTypes[fileExtension]) {
            return fileToContentTypes[fileExtension];
        }
        return 'text/plain';
    },
    returnNotFound = function(request, response) {
        response.writeHead(404, {'Content-Type': 'text/plain'});
        response.write(request.url + " not found!");
        response.end();
    },
    processGet = function(request, response) {
        if (request.url === '/') {
            returnNotFound(request, response);
        } else if (request.url === '/site') {
            response.writeHead(200, {'Content-Type': 'text/html'});
            fs.createReadStream(__dirname + '/site/index.html').pipe(response);
        } else {
            fs.exists(__dirname + request.url, function(exists) {
                if (exists) {
                    response.writeHead(200, {'Content-Type': getContentType(request.url)});
                    fs.createReadStream(__dirname + request.url).pipe(response);
                } else {
                    returnNotFound(request, response);
                }
            });
        }
    },
    start = function() {
        var server = http.createServer(function(request, response) {
            // Simple routing based on HTTP method
            if (request.method === 'POST') {
                processInboundMessage(request, response, wordCountHash);
            } else if (request.method === 'GET') {
                processGet(request, response);
            }
        });
        io = io.listen(server);
        server.listen(8080);

        io.sockets.on('connection', function (socket) {
            socket.emit('news', { hello: 'world' });
            socket.emit('inboundParsed', wordCountHash);
        });


    };
exports.start = start;