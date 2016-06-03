"use strict";

var j = jQuery.noConflict();

var app = angular.module("SPaaSApp");

app.controller('logOutController', logOutController);

logOutController.$inject = ['$scope', '$http', '$location', '$rootScope'];

function logOutController($scope, $http, $cookies, $location, $rootScope) {
    console.log("successfully inside logout controller");   
       
        $rootScope.User = null;
        if (document.cookie !== "") {
            document.cookie = "";
        }     
        $location.url('/logout');
};




