
'use strict';
var j = jQuery.noConflict();

var app = angular.module("SPaaSApp");

app.controller('buildController', buildController);

buildController.$inject = ['$scope', '$http', '$rootScope', '$location','$cookies', 'httpGetService', 'messages'];

function buildController($scope, $http, $rootScope, $location, $cookies, httpGetService, messages) {
    console.log("Build Controller");
    $scope.message;
    var vmName = [];
    
    var promise = httpGetService.GETME();
    promise.then(function (docs) {
        //User Name Populate
        console.log('Success: ' + docs.data.UserName);
        $scope.UserId = docs.data.UserId
        //$rootScope.User = docs.data.UserName
        //Design ID populate
        var designId = generateId();
        $scope.DId = designId;
        //ID populate
        var vmId = generateId();
        $scope.ID = vmId;
        //VMType dropdown populate
        populateVMType($cookies,$scope);
        
        //End of OS dropdown populate
    }, function () {
        $location.url('/Login')
    });

    var generateId = function () {
        return (Math.floor(Math.random() * 100000 + 1));
    }

    var populateVMType = function () {
        if ($cookies.get('Types') == null) {
            $scope.vmType = [];
        } else {
            $scope.vmType = JSON.parse($cookies.get('Types'));
        }
        if ($scope.vmType.length <= 0) {
            var VMpromise = httpGetService.GETVMTYPE();
            VMpromise.then(function (vmArray) {
                console.log(vmArray)
                for (var i in vmArray.data) {
                    vmName.push(vmArray.data[i])
                }
                $scope.vmType = vmName;
                $cookies.put('Types', JSON.stringify($scope.vmType));
                $scope.Type = '(blank)';
            }, function () {
                $scope.Type = "(blank)"
            })
        }
    }
    
};



