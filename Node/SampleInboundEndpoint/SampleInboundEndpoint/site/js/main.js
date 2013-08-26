/* global d3, requirejs */

requirejs.config({
    baseUrl: './site',
    paths: {
        "jquery": "js/vendor/jquery-2.0.3",
        "jquery.bootstrap": "js/vendor/bootstrap",
        "socket.io": "js/vendor/socket.io"
    },
    shim: {
        "jquery.bootstrap": {
            deps: ["jquery"]
        }
    }
});

require(['jquery', 'socket.io', 'js/vendor/d3.v3', 'jquery.bootstrap'], function($, io) {
    'use strict';

    // TODO: Set the site URL here for websockets
    var siteUrl = "http://localhost:8080",
        entries,
        //words = ['hammer', 'wrench', 'screwdriver', 'bandsaw', 'nailgun', 'sawhorse', 'toolbox'],
        wordCount = {},
        // addRandomlyToWordCount = function() {
        //     var wordIndex = Math.round(Math.random() * (words.length - 1));
        //     wordCount[words[wordIndex]]++;
        // },
        compare = function(a, b) {
            return b.value - a.value;
        },
        howManyToSHow = 7,
        barWidth = 60,
        chartHeight = 260,
        plotAreaHeight = 200,
        chart = d3.select('.container').append('svg')
        .attr('class', 'chart')
        .attr('width', barWidth * howManyToSHow)
        .attr('height', chartHeight),
        yScale = d3.scale.linear().domain([0, 10]).rangeRound([0, plotAreaHeight]),
        updateChart = function() {
            var rects, labels, numberLabels, max;

            entries = d3.entries(wordCount).sort(compare);
            max = d3.max(entries, function(d) { return d.value; });
            yScale = d3.scale.linear().domain([0, max]).rangeRound([0, plotAreaHeight]);
            rects = chart.selectAll('rect').data(entries, function(d) { return d.key; });
            labels = chart.selectAll('text').data(entries, function(d) { return d.key; });
            numberLabels = chart.selectAll('text.quantities').data(entries, function(d) { return d.key; });

            rects.enter().append('rect')
                .attr('x', function(d, i) {
                    return i * barWidth;
                })
                .attr('y', function(d) {
                    return plotAreaHeight - yScale(d.value);
                })
                .attr('width', barWidth)
                .attr('height', function(d) {
                    return yScale(d.value);
                });
            labels.enter().append('text')
                .attr('x', function(d, i) {
                    return i * barWidth;
                })
                .attr('y', function() {
                    return plotAreaHeight + 18;
                })
                .attr('text-anchor', 'end')
                .attr('transform', function(d, i) {
                    return 'rotate(-90,' + (i * barWidth + 12) + ',' + (plotAreaHeight - 8) + ')';
                })
                .text(function(d) { return d.key; });

            numberLabels.enter().append('text')
                .attr('class', 'quantities')
                .attr('x', function(d, i) {
                    return i * barWidth + barWidth / 2;
                })
                .attr('y', function(d) {
                    return plotAreaHeight - yScale(d.value);
                })
                .attr('text-anchor', 'middle')
                .text(function(d) { return d.value; });

            rects.transition()
                .duration(1000)
                .attr('x', function(d, i) {
                    return i * barWidth;
                })
                .attr('y', function(d) {
                    return plotAreaHeight - yScale(d.value);
                })
                .attr('height', function(d) {
                    return yScale(d.value);
                });

            labels.transition('text')
                .duration(1000)
                .attr('x', function(d, i) {
                    return i * barWidth;
                })
                .attr('y', function() {
                    return plotAreaHeight + 18;
                })
                .attr('text-anchor', 'end')
                .attr('transform', function(d, i) {
                    return 'rotate(-90,' + (i * barWidth + 12) + ',' + (plotAreaHeight - 8) + ')';
                })
                .text(function(d) { return d.key; });

            numberLabels.transition('text.quantities')
                .duration(1000)
                .attr('x', function(d, i) {
                    return i * barWidth + barWidth / 2;
                })
                .attr('y', function(d) {
                    return plotAreaHeight - yScale(d.value) + 20;
                })
                .attr('text-anchor', 'middle')
                .text(function(d) { return d.value; });

            rects.exit().remove();
        },
        socket = io.connect(siteUrl);

    socket.on('news', function (data) {
        console.log(data);
        socket.emit('my other event', { my: 'data' });
    });
    socket.on('inboundParsed', function (data) {
        wordCount = data;
        updateChart();
    });
});