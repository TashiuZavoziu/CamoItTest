angular
    .module('camoItApp')
    .service('hubproxyService', ['$rootScope', '$', function ($rootScope, $) {
        var productsHub = $.connection.productsHub

        $.connection.hub.start().done(function () {
            console.log("Products Hub has started");
        });

        this.on = function (eventName, callback) {
            productsHub
                .on(eventName, function () {
                    var args = Array.prototype.slice.call(arguments, 0);
                    if (callback) {
                        $rootScope.$apply(function () {
                            callback.apply(this, args);
                        });
                    }
                });
        },
        this.invoke = function (methodName, callback) {
            productsHub
                .invoke(methodName)
                .done(function (result) {
                    debugger
                    if (callback) {
                        $rootScope.$apply(function () {
                            callback(result);
                        });
                    }
                });
        }
    }]);