(function () {
    'use strict';
    var app = angular.module('SPaaSApp', ['ngRoute']);

    app.config(function ($routeProvider) {
        $routeProvider
            .when('/Home', {
                templateUrl: 'templates/home.html',
                controller: 'homeController'
            })
        .when('/Login', {
            templateUrl: 'templates/login.html',
            controller: 'authController'
        })
        .otherwise({
            redirectTo: '/Home'
        })

    });

})()