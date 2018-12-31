


//optimusApp.config(function (cfpLoadingBarProvider) {
//    cfpLoadingBarProvider.includeSpinner = true;
//});

//optimusApp.config(function ($stateProvider) {
//    var mainstate = {
//        name: 'main',
//        url: '/main',
//        template: '<h3>hello world!</h3>'
//    }

//    $stateProvider.state(mainstate);


//});


optimusApp.config(function ($routeProvider, $locationProvider) {
    $routeProvider.when("/login", {
        templateUrl: "pages/Afiliados/login.html",
        controller: "authCtrl",
        public: true
    }).when("/index", {
        templateUrl: "pages/Afiliados/main.html"
    }).when("/PesquisarCliente", {
        templateUrl: "pages/Afiliados/PesquisaCliente.html",
        controller: "clienteCtrl"
    }).when("/CadastroCliente", {
        templateUrl: "pages/Afiliados/CadCliente.html"
    }).when("/CadastroCliente/:cdentidade", {
        templateUrl: "pages/Afiliados/CadCliente.html",
        controller: "clienteCadastroCtrl"
    }).when("/NovoPedido", {
        templateUrl: "pages/Afiliados/CadPedido.html",
        controller: "pedidoCtrl"
    }).when("/PesquisarPedido", {
        templateUrl: "pages/Afiliados/PesquisaPedido.html",
        controller: "pesquisaPedidoCtrl"
    }).when("/CadastroColaborador", {
        templateUrl: "pages/Afiliados/CadColaborador.html",
        controller: "colaboradorCtrl"
    }).when("/PesquisarColaborador", {
        templateUrl: "pages/Afiliados/PesquisaColaborador.html",
        controller: "colaboradorCtrl"
    }).otherwise({ redirectTo: '/login' });
    $locationProvider.html5Mode(true);
}).run(function ($rootScope, $localStorage, $location, $state) {

    // register listener to watch route changes
    $rootScope.$on("$routeChangeStart", function (event, next, current) {
        //console.log("$routeChangeStart");
        if ($localStorage.logado == "N") {
            // no logged user, we should be going to #login
            if (next.templateUrl == "pages/Afiliados/login.html") {
                // already going to #login, no redirect needed
                //$scope.usernamelogado = $localStorage.usernamelogado;
                //$scope.userlogado = $localStorage.userlogado;
                //$scope.perfil = $localStorage.perfil;
                //console.log("rout");
            } else {
                $localStorage.$reset();
                // not going to #login, we should redirect now
                event.preventDefault(); // stop current execution
                //$state.go('login');
                $location.path('/login');
            }
        }
    });
});