
    'use strict';
    var j = jQuery.noConflict();

    var app = angular.module("SPaaSApp");   

    app.controller('homeController', homeController);

    homeController.$inject = ['$scope'];

    function homeController($scope) {
        console.log("Home Controller");
        
    };

