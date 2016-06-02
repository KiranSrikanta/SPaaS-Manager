(function () {
    'use strict';
    var app = angular.module('SPaaSApp', ['ngRoute']);

    app.config(function ($routeProvider) {
        $routeProvider
            .when('/Home', {
                templateUrl: 'templates/home.html',
                controller: 'homeController'
            })
            .when('/BuildEnvironment', {
                templateUrl: 'templates/build.html',
                controller: 'buildController'
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