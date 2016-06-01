
    'use strict';
    var j = jQuery.noConflict();

    var app = angular.module("SPaaSApp");

    app.controller('authController', authController);

    authController.$inject = ['$scope', '$http'];

    function authController($scope, $http) {
        console.log("authentication Controller");

        $scope.login = function () {
            console.log("button event received")
          
            window.location = "/api/auth/azure";
        }
    };

