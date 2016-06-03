"use strict";

app.factory("httpGetService", ['$q','$http', '$rootScope', function ($q,$http,$rootScope) {
    
    var authorizationValue = 'Bearer ' + document.cookie.substring(14, document.cookie.length).toString();

    function getMe() {
        var deferred = $q.defer();
        if (document.cookie !== "") {
            var req = {
                method: 'GET',
                url: '/api/me',
                headers: {
                    'Authorization': authorizationValue
                }
            }
            $http(req).then(function (docs) {
                console.log(docs)
                $rootScope.User = docs.data.UserName
                deferred.resolve($rootScope.User);
            })
        }
        else {
            deferred.reject($rootScope.User);
        }
        return deferred.promise;
    }

    function getVMType(){
        console.log("GetVMType")
    }
    
    function getOS() {
        console.log("GetOSType")
    }


    return {
        GETME: getMe,
        GETVMTYPE: getVMType,
        GETOS:getOS
    }
}])