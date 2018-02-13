angular
    .module("camoItApp")
    .controller('ProductsController', ['$scope', '$http', '$', 'hubproxyService', 'fileuploadService', function ($scope, $http, $, hubproxyService, fileuploadService) {
        $scope.files = [];
        $scope.parameters = [];
        $scope.mapping = [];

        $scope.progress = 0;
        $scope.message = "";
        $scope.update = "";

        $scope.processing = false;

        $scope.upload = function (files) {
            $scope.message = "";
            $scope.update = "";
            $scope.processing = true;
            fileuploadService.uploadFile({ name: 'file', file: files[0] }, { name: 'mapping', data: $scope.mapping }, '/home/uploadproducts', function (result) {
                $scope.processing = false;
                var data = result.data;
                if (!data.success) {
                    $scope.update = data.message;
                }
            });
        };

        //firefox 'bug' fix
        var isFirefox = /firefox/i.test(window.navigator.userAgent);
        $scope.firefoxReset = function () {
            if (isFirefox) {
                $scope.files = [];
            }
        };

        //fires once on firefox
        $scope.$watch('files', function () {
            $scope.message = "";
            $scope.update = "";
            $scope.mapping = [];
            if ($scope.fileError()) {
                return;
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                $scope.$apply(function () {
                    var text = e.target.result;
                    var line = text.split('\r\n', 1)[0];
                    var names = line.split(',');
                    for (var i = 0; i < names.length; i++) {
                        $scope.mapping[i] = {};
                        $scope.mapping[i].Name = names[i];
                    }
                });
            };

            reader.readAsText($scope.files[0]);
        });

        $http
            .get('/home/productparameters')
            .then(function (result) {
                $scope.parameters = result.data;
            });

        $scope.fileError = function () {
            if ($scope.files && $scope.files[0]) {
                return !$scope.files[0].name.endsWith('.csv');
            }
            return true;
        };

        hubproxyService.on('updateprogress', function (percent, message) {
            $scope.progress = percent;
            $scope.message = message;
        });
    }]);