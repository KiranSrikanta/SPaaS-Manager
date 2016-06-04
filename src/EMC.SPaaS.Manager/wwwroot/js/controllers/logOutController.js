"use strict";

var j = jQuery.noConflict();

var app = angular.module("SPaaSApp");

app.controller('logOutController', logOutController);

logOutController.$inject = ['$scope', '$http','$cookies', '$location', '$rootScope'];

function logOutController($scope, $http, $cookies, $location, $rootScope) {
    console.log("successfully inside logout controller");   
       
        $rootScope.User = null;
        if ($cookies.get("Authorization") != null) {

            $cookies.remove("Authorization");
            $cookies.remove('Types')
        }     
        $location.url('/Logout');
};




