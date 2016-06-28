angular.module('galileoWebApp', []);

angular.module('galileoWebApp').controller('webController', function ($scope, $location, $http, $window) {
    console.log('angular');
    
    // --> api call
    //
    function api(method,url,_callback) {
        $http({
          method: method,
          url: url
        }).then(function successCallback(response) {
            _callback(false,response);
        }, function errorCallback(response) {
            _callback(response);
        });
    };
    
    // --> light
    //
    $scope.lightStatus;
    function readLight() {
        api('GET','/api/light', function(err, response){
            if(!err) {
                if(response.data.message) {
                    $scope.lightStatus = 'on';   
                } else {
                    $scope.lightStatus = 'off';
                }
            }
                
        });
    };
    readLight();
    $scope.toggleLight= function () {
        api('PUT','/api/light', function(err, response){
            if(!err) {
                if(response.data.message) {
                    $scope.lightStatus = 'on';   
                } else {
                    $scope.lightStatus = 'off';
                }
            }   
        });
    };
    
    // --> air
    //
    $scope.airStatus;
    function readAir() {
        api('GET','/api/air', function(err, response){
            if(!err) {
                if(response.data.message) {
                    $scope.airStatus = 'on';   
                } else {
                    $scope.airStatus = 'off';
                }
            }
                
        });
    };
    readAir();
    $scope.toggleAir = function () {
        api('PUT','/api/air', function(err, response){
            if(!err) {
                if(response.data.message) {
                    $scope.airStatus = 'on';   
                } else {
                    $scope.airStatus = 'off';
                }
            }   
        });
    };
    
    // --> smell
    //
    $scope.toggleSmell= function () {
        api('PUT','/api/smell', function(err, response){});
    };
});