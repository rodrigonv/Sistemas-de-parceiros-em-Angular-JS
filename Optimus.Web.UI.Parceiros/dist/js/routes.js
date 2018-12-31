


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
        templateUrl: "pages/Afiliados/main.html",
        controller: "mainCtrl"
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
    }).when("/CadastroColaborador/:cdcolaborador", {
        templateUrl: "pages/Afiliados/CadColaborador.html",
        controller: "colaboradorCtrl"
    }).when("/PesquisarColaborador", {
        templateUrl: "pages/Afiliados/PesquisaColaborador.html",
        controller: "colaboradorCtrl"
    }).when("/EditarPedido/:cdpedido", {
        templateUrl: "pages/Afiliados/CadPedido.html",
        controller: "pedidoCtrl"
    }).when("/CadastroProdutoParceiro", {
        templateUrl: "pages/Afiliados/CadProdutoParceiro.html",
        controller: "cadprodutoCtrl"
    }).when("/EditProdutoParceiro/:cdforn", {
        templateUrl: "pages/Afiliados/CadProdutoParceiro.html",
        controller: "cadprodutoCtrl"
    }).when("/PesquisarBanner", {
        templateUrl: "pages/Afiliados/PesquisaBanner.html",
        controller: "bannerCtrl"
    }).when("/EditarBanner/:cdbanner", {
        templateUrl: "pages/Afiliados/CadBanner.html",
        controller: "bannerCtrl"
    }).when("/CadastrarBanner", {
        templateUrl: "pages/Afiliados/CadBanner.html",
        controller: "bannerCtrl"
    }).when("/PesquisarComissao", {
        templateUrl: "pages/Afiliados/PesquisaComissao.html",
        controller: "parceiroCtrl"
    }).when("/EnviarNf", {
        templateUrl: "pages/Afiliados/EnviaNF.html",
        controller: "EnviaNFCtrl"
    }).when("/TabelaPreco", {
        templateUrl: "pages/Afiliados/PesquisaTabelaPreco.html",
        controller: "tabelaPrecoCtrl"
    }).when("/Gerencial", {
        templateUrl: "pages/Afiliados/Gerencial.html",
        controller: "gerencialCtrl"
    }).otherwise({ redirectTo: '/login' });
    $locationProvider.html5Mode(true);
}).run(function ($rootScope, $localStorage, $location, $state) {
    
    // register listener to watch route changes
    $rootScope.$on("$routeChangeStart", function (event, next, current) {
        //console.log("$routeChangeStart");
        //console.log("$rootScope.perfil" + $rootScope.perfil);
        //console.log("$rootScope.logado" + $rootScope.logado);
        
        if ($localStorage.logado == "N") {
            // no logged user, we should be going to #login
            if (next.templateUrl == "pages/Afiliados/login.html") {
                //console.log("logoff templare");
                $localStorage.$reset();
                $localStorage.userlogado = false;
                $rootScope.userlogado = false;
                $localStorage.usernamelogado = "";
                $localStorage.logado = "N";
                $localStorage.cdusuariooptimus = "";
                $localStorage.cdfuncionario = "";
                $localStorage.perfil = "PARCEIRO";
                $rootScope.perfil = "PARCEIRO";
                $localStorage.codforn = "";
                $location.path('/login');
            } else {
                console.log("logoff else templare");
               // $localStorage.$reset();
                $localStorage.userlogado = false;
                $rootScope.userlogado = false;
                $localStorage.usernamelogado = "";
                $localStorage.logado = "N";
                $localStorage.cdusuariooptimus = "";
                $localStorage.cdfuncionario = "";
                $localStorage.perfil = "PARCEIRO";
                $rootScope.perfil = "PARCEIRO";
                $localStorage.codforn = "";
                $location.path('/login');
                
            }
        } else {
            var dataatual = new Date();
            var datalogin = Date.parse($localStorage.datalogin);
            var diferencahour = dataatual - datalogin;
            var hDiff = diferencahour / 3600 / 1000;
            var minDiff = diferencahour / 60 / 1000;
            console.log("dataatual " + dataatual);
            console.log("datalogin " + datalogin);
            console.log("hDiff" + Math.floor(hDiff));
            console.log("minDiff" + Math.floor(minDiff));
            //console.log("diferenca min" + minDiff - 60 * Math.floor(hDiff));
            if (Math.floor(hDiff) > 2) {
                console.log("logoff else");
                //$scope.userlogado = false;
                $localStorage.$reset();
                //$scope.user = {};
                //$scope.errologin = "";
                //$scope.alertloginerror = false;
                //$scope.userlogado = false;
                $localStorage.userlogado = false;
                $rootScope.userlogado = false;
                //$scope.usernamelogado = "";
                $localStorage.usernamelogado = "";
                $localStorage.logado = "N";
                $localStorage.cdusuariooptimus = "";
                $localStorage.cdfuncionario = "";
                $localStorage.perfil = "PARCEIRO";
                $rootScope.perfil = "PARCEIRO";
                $localStorage.codforn = "";
                $location.path('/login');
            }
            
        }
    });
});