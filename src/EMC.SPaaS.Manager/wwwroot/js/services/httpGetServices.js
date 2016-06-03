"use strict";

app.factory("httpGetService", ['$q', '$http', '$rootScope','$cookies', function ($q, $http, $rootScope,$cookies) {

    //var authorizationValue = 'Bearer ' + document.cookie.substring(14, document.cookie.length).toString();
    var authorizationValue;
    if ($cookies.get("Authorization") != null) {
        authorizationValue = 'Bearer ' + $cookies.get("Authorization");
    }
    


    function getMe() {
        var deferred = $q.defer();
        if (authorizationValue !== undefined) {
            var req = {
                method: 'GET',
                url: '/api/me',
                headers: {
                    'Authorization': authorizationValue
                }
            }
            $http(req).then(function (docs) {
                console.log(docs)
                //$rootScope.User = docs.data.UserName
                deferred.resolve(docs);
            })
        }
        else {
            deferred.reject();
        }
        return deferred.promise;
    }

    function getVMType() {
        console.log("GetVMType")
        var deferred = $q.defer();
        if (document.cookie !== "") {
            var req = {
                method: 'GET',
                url: '/api/cloudProviderOptions/vmtype',
                headers: {
                    'Authorization': authorizationValue
                }
            }
            $http(req).then(function (docs) {
                console.log(docs)
                deferred.resolve(docs);
            })
        }
        else {
            deferred.reject();
        }
        return deferred.promise;
    }

    function getOS() {
        console.log("GetOSType")
        var deferred = $q.defer();
        if (document.cookie !== "") {
            var req = {
                method: 'GET',
                url: '/api/cloudProviderOptions/os',
                headers: {
                    'Authorization': authorizationValue
                }
            }
            $http(req).then(function (docs) {
                console.log(docs)
                deferred.resolve();
            })
        }
        else {
            deferred.reject();
        }
        return deferred.promise;
    }


    return {
        GETME: getMe,
        GETVMTYPE: getVMType,
        GETOS: getOS
    }
}])