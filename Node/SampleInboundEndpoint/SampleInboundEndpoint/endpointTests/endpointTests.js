'use strict';
/* global describe, it */

var expect = require('expect.js'),
    TextProcessor = require('../TextProcessor');

describe("The Endpoint's text processor", function() {
    it("can break up a string into an array of words with no punctuation", function() {
        var testString = "This is an input string, and it has basic punctuation. Let's test it!",
            result = {};
        
        result = TextProcessor.parse(testString, result);

        expect(result).to.eql(["This", "is", "an", "input", "string", "and", "it", "has", "basic", "punctuation", "Let's", "test", "it"]);
    });

    it("can count up certain word occurrences in a word array and store counts in an object", function() {
        var testArray = ['word', 'hammer', 'hammer', 'hey', 'hey', 'toolbox', 'word', 'again', 'screwdriver'],
            result = TextProcessor.countWords(testArray);

        expect(result).to.eql({
            hammer: 2,
            toolbox: 1,
            screwdriver: 1
        });
    });
});