
    'use strict';
    var j = jQuery.noConflict();

    var app = angular.module("SPaaSApp");   

    app.controller('homeController',homeController);

    homeController.$inject = ['$scope', '$http', '$rootScope', 'httpGetService', 'messages'];

    function homeController($scope,$http,$rootScope,httpGetService,messages) {
        console.log("Home Controller");
        $scope.message;
        var promise = httpGetService.GETME();
        promise.then(function (docs) {
            console.log('Success: ' + docs.data.UserName);
            $rootScope.User = docs.data.UserName
        }, function () {
            $scope.message = {
                status: messages.danger,
                details: "Login pending"
            }
        });   
    }

