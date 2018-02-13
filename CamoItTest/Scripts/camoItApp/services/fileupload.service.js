angular
    .module('camoItApp')
    .service('fileuploadService', ['$http', function ($http) {
        this.uploadFile = function (file, post, url, success, error) {
            var fd = new FormData();
            fd.append(file.name, file.file);
            fd.append(post.name, angular.toJson(post.data));
            $http.post(url, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .then(success, error);
        }
    }]);
