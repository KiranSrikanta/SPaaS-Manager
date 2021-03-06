﻿(function () {
    'use strict';
    var app = angular.module('SPaaSApp', ['ngRoute','ngCookies']);

    app.run(['$rootScope', '$http', 'httpGetService', function ($rootScope, $http, httpGetService) {
        var promise = httpGetService.GETME();
        promise.then(function (docs) {
            console.log('Success: ' + docs.data.UserName);
            $rootScope.User = docs.data.UserName
        }, function () {
           
        });
    }])

    var interceptor = ['$q',function ($q) {
        return {
            response: function (response) {
                // do something on success
                return response;
            },
            responseError: function (response) {
                if (response.status === 401) {
                    $location.url('/Login');
                }
                return $q.reject(response);
            }
        };
        }]

    app.config(['$routeProvider', '$httpProvider', function ($routeProvider, $httpProvider) {

        $httpProvider.interceptors.push(interceptor)

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
            .when('/Logout', {
                templateUrl: 'templates/logout.html',
                controller: 'logOutController'
            })
        .otherwise({
            redirectTo: '/Home'
        })

    }]);

})()