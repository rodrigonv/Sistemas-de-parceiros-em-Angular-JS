var optimusApp = angular.module('optimusPartner', ['ui.router',
    'ngRoute',
    'angularUtils.directives.dirPagination',
    'ngStorage',
    'mgo-angular-wizard',
'angular-loading-bar',
'ui.utils.masks',
'ui.bootstrap', 'ngFileUpload', 'ejangular'])
.value("optconfig", {
    url: "http://localhost:19552/",
    //url: " https://parceiros.multivisi.com.br/rest/",
    opttokenname: "opttoken",
    optpkgname: "",
    opttoken: "ZKsn/Woq9tpABm9C4nA5u8DyBPuIAgF/xvjlHCr8IxO3M8u7KV4jjg==",
    //opttoken: "au51797coFM6NBtddL416Zzv/rT2EJYsZpKoZtJYWsQz2+bpikUbNQ==",
    //opttoken: "c1HfYloceYc6NBtddL416TJR9ORBcEgIOI7Ba/h4errmhzLQzSdfGlk3uxx5DUFG", produ
    //c1HfYloceYc6NBtddL416TJR9ORBcEgIOI7Ba/h4errmhzLQzSdfGlk3uxx5DUFG
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
}).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
    cfpLoadingBarProvider.includeBar = true;
    cfpLoadingBarProvider.loadingBarTemplate = '<div id="loading-bar"><div class="bar"><div class="peg"></div></div></div>';
}]);