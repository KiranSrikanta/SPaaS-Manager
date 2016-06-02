
'use strict';
var j = jQuery.noConflict();

var app = angular.module("SPaaSApp");

app.controller('buildController', buildController);

buildController.$inject = ['$scope'];

function buildController($scope) {
    console.log("Build Controller");

};