angular
    .module("camoItApp", [])
    .constant('$', window.$)
    .constant('rules', {
        NotMapped: 0,
        RequiredOne: 1,
        RequiredMany: 2,
        NotRequiredOne: 3,
        NotRequiredMany: 4,
        Ignore: 5
    });
