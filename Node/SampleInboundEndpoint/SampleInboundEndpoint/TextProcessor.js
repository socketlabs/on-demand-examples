'use strict';

var parse = function(text) {
        var re = /\b((\w*'\w+)|(\w+))\b/g;
        return text.match(re);
    },
    countWords = function(arrayOfWords, countHash) {
        var i, len, word, result = countHash || {},
            acceptedWords = ['hammer', 'wrench', 'screwdriver', 'bandsaw', 'nailgun', 'sawhorse', 'toolbox'];

        for (i = 0, len = arrayOfWords.length; i < len; i = i + 1) {
            word = arrayOfWords[i].toLowerCase();
            // For this example, only use words in a controlled set to control content
            // since this will be a public-facing example
            if (acceptedWords.indexOf(word) > -1) {
                if (result[word]) {
                    result[word] = result[word] + 1;
                } else {
                    result[word] = 1;
                }
            }
        }
        return result;
    };

module.exports.parse = parse;
module.exports.countWords = countWords;