var optimusApp = angular.module('optimusPartner', ['ui.router',
    'ngRoute',
    'angularUtils.directives.dirPagination',
    'ngStorage',
    'mgo-angular-wizard',
'angular-loading-bar',
'ui.utils.masks'])
.value("optconfig", {
    url: "http://localhost:19552/",
    opttokenname: "opttoken",
    optpkgname: "",
    opttoken: "ZKsn/Woq9tpABm9C4nA5u8DyBPuIAgF/xvjlHCr8IxO3M8u7KV4jjg==",
    optfilial: "",
    optcodapp: "",
    optcsstopo: "",
    optcssfundotopo: "",
    opturllogo: "",
    opttxhoraatendimento: "",
    optemailcontato: "",
    opttelefones: "",
    optdadosempresa: "",
    optsenderidios: "",
    optsenderidandroid: "",
    filial: "",
    esquema: ""
}).run(function ($window) {
    var windowElement = angular.element($window);
    windowElement.on('beforeunload', function (event) {
        // do whatever you want in here before the page unloads.        
        console.log("preventDefault");
        // the following line of code will prevent reload or navigating away.
        event.preventDefault();
    });
});