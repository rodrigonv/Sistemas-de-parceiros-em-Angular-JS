
optimusApp.controller('clienteCtrl', ['$scope', 'clienteFactory', '$location', '$routeParams',
function ($scope, clienteFactory, $location, $routeParams) {

    $scope.alerterror = false;
    $scope.msgerror = "";
    $scope.nomecliente = "";
    $scope.cpfcnpjcliente = "";
    //console.log($stateParams.Item);
    $scope.teste = "$stateParams.Item";

    //if ($scope.item.urlnota != "" && $scope.item.urlnota != undefined)
    //    $scope.btnotadis = false;

    //$scope.goBack = function () {
    //    $ionicHistory.goBack();
    //}
    //$scope.openWindow = function (url) {
    //    //$window.open(trustSrc(url), '_blank', 'location=yes');
    //    //var trusturl = $scope.trustSrc(url);
    //    window.open(url, '_system', 'location=yes');

    //};

    $scope.PesquisarCliente = function () {

        if ($scope.nomecliente == "" && $scope.cpfcnpjcliente == "") {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a iniciar pesquisa.";
        } else {
            clienteFactory.RetornaClienteByParam($scope.nomecliente, $scope.cpfcnpjcliente).then(
           function (results) {
               $scope.alerterror = false;
               $scope.msgerror = "";
               $scope.clientes = {};
               $scope.clientes = results.data;
               //console.log($scope.clientes);
           }
       );
        }
    }
    $scope.getCliCPFCNPJ = function (cli) {
        var c = "";

        c = cli.CPF == "" ? cli.CNPJ : cli.CPF;

        return c;
    }


    $scope.NovoCliente = function () {
        $location.path('/CadastroCliente');
    }
    $scope.GravarCliente = function (cli) {
        $location.path('/CadastroCliente');
    }
    $scope.EditarCliente = function (cdentidade) {
        $location.path('/CadastroCliente/' + cdentidade);
    }

}
]).controller('clienteCadastroCtrl', ['$scope', 'clienteFactory', 'cepFactory', '$location', '$routeParams', '$localStorage', 'basicoFactory', '$filter',
function ($scope, clienteFactory, cepFactory, $location, $routeParams, $localStorage, basicoFactory, $filter) {

    //console.log($routeParams.cdentidade);
    $scope.tipopessoas = ['Física', 'Jurídica'];

    //$scope.UFS = ['AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'];
    $scope.states = ['AC', 'AL', 'AM', 'AP', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
                 'MG', 'MS', 'MT', 'PA', 'PB', 'PE', 'PI', 'PR', 'RJ', 'RN', 'RO', 'RR',
                 'RS', 'SC', 'SE', 'SP', 'TO'];
    //console.log($scope.tipopessoas);
    $scope.cli = {};
    $scope.ende = {};
    $scope.contato = {};
    $scope.tele = {};
    $scope.email = {};
    $scope.enderecos = [];
    $scope.contatos = [];
    $scope.telefones = [];

    $localStorage.enderecos = {};
    $localStorage.contatos = {};
    $localStorage.telefones = {};
    $scope.cdenderecosel = 0;
    $scope.emails = [];
    $scope.TiposTelefone = {};
    $scope.TiposEmail = {};
    $scope.TiposContato = {};
    $scope.TiposEndereco = {};

    $scope.contatoselecionado = false;
    $scope.cdentidade = $routeParams.cdentidade;
    $localStorage.cdentidade = $routeParams.cdentidade;

    $scope.cli.tipopessoa = "F";
    $scope.ende.principal = "1";
    $scope.ende.cdendereco = "0";
    $scope.ende.excluir = 0;

    $scope.contato.cdcontato = 0;
    $scope.contato.excluir = 0;
    $scope.contato.stdefault = "1";
    $scope.contato.Emails = [];
    $scope.contato.Telefones = [];
    $scope.cdcontatoselecionado = 0;
    $scope.idxcontatoselecionado = 99;
    $scope.msgerros = [];
    $scope.nmcontatoselecionado = "";
    $scope.idxtelefonesel = 99;
    $scope.cdtelefoneresi = 0;
    $scope.dddresi = 0;
    $scope.telresidencial = "";
    $scope.cdtelefonecel = 0;
    $scope.dddcel = 0;
    $scope.telcelular = "";
    $scope.cdemail = 0;
    $scope.email = "";

    $scope.idxtelefonesel = 99;
    $scope.tele.cdtipotelefone = "";
    $scope.tele.ddd = "";
    $scope.tele.telefone = "";
    $scope.tele.excluir = 0;
    $scope.tele.idx = 99;

    $scope.idxemailsel = 99;
    $scope.email.cdtipoemail = "";
    $scope.email.ddd = "";
    $scope.email.txemail = "";
    $scope.email.excluir = 0;
    $scope.email.idx = 99;
    $scope.clienteexiste = false;
    $scope.editando = false;
    $scope.getTipoEndereco = function (id) {
        var str = "";
        angular.forEach($scope.TiposEndereco, function (ende, key) {
            if (ende.valor == id) {
                str = ende.nome;
            }
        });
        return str;
    };
    $scope.getTipoTelefone = function (id) {
        var str = "";
        angular.forEach($scope.TiposTelefone, function (ende, key) {
            if (ende.valor == id) {
                str = ende.nome;
            }
        });
        return str;
    };
    $scope.getTipoEmail = function (id) {
        var str = "";
        angular.forEach($scope.TiposEmail, function (ende, key) {
            if (ende.valor == id) {
                str = ende.nome;
            }
        });
        return str;
    };
    $scope.getTipoContato = function (id) {
        var str = "";
        angular.forEach($scope.TiposContato, function (ende, key) {
            if (ende.valor == id) {
                str = ende.nome;
            }
        });
        return str;
    };
    $scope.getEnderecoPrincipal = function (id) {
        return id == "1" ? "SIM" : "NÃO";
    };

    $scope.LoadBasicos = function () {

        $scope.TiposTelefone = {};
        $scope.TiposEmail = {};
        $scope.TiposContato = {};
        $scope.TiposEndereco = {};

        basicoFactory.RetornaTipoTelefone().then(
                function (result) {
                    $scope.TiposTelefone = result.data;
                }
                , function (error) {
                    alert(error);
                }
            );

        basicoFactory.RetornaTipoEmail().then(
                function (result) {
                    $scope.TiposEmail = result.data;
                }
                , function (error) {
                    alert(error);
                }
            );

        basicoFactory.RetornaTipoContato().then(
               function (result) {
                   $scope.TiposContato = result.data;
               }
               , function (error) {
                   alert(error);
               }
           );
        basicoFactory.RetornaTipoEndereco().then(
               function (result) {
                   $scope.TiposEndereco = result.data;
               }
               , function (error) {
                   alert(error);
               }
           );
    }

    if (parseInt($routeParams.cdentidade) > 0) {
        clienteFactory.RetornaClienteByCdentidade($routeParams.cdentidade).then(
                function (resultcli) {
                    $localStorage.clienteselecionado = resultcli.data;
                    $scope.cli.cdentidade = resultcli.data.cdentidade;
                    $scope.cli.nome = resultcli.data.nome;
                    $scope.cli.fantasia = resultcli.data.fantasia;
                    $scope.cli.CPF = resultcli.data.CPF;
                    $scope.cli.CNPJ = resultcli.data.CNPJ;
                    $scope.cli.IE = resultcli.data.IE;
                    $scope.cli.IM = resultcli.data.IM;
                    $scope.cli.tipopessoa = resultcli.data.tipopessoa;
                    $scope.cli.identidade = resultcli.data.identidade;
                    $scope.cli.sexo = resultcli.data.sexo;
                    $scope.enderecos = resultcli.data.Enderecos;
                    $scope.contatos = resultcli.data.Contatos;
                    //console.log(resultcli.data);
                    $scope.editando = true;
                }
                , function (error) {
                    alert(error);
                }
            );
    } else {
        //novo

    }

    $scope.GravarCliente = function (cli) {
        //$location.path('/CadastroCliente');
        cli.cdentiresp = $localStorage.codforn;
        $scope.msgerros = [];
        var podegravar = true;
        //VALIDAR
        if ($scope.enderecos.length == 0) {
            $scope.msgerros.push("Cadastre o endereço do cliente");
            $scope.alerterror = true;
            podegravar = false;
        } else {
            var excluidosCount = $filter('filter')($scope.enderecos, { excluir: 1 }).length;
            if (excluidosCount == $scope.enderecos.length) {
                $scope.msgerros.push("Cadastre um endereço para o cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
        }

        var excluidosContaCount = $filter('filter')($scope.contatos, { excluir: 1 }).length;
        if (excluidosContaCount == $scope.contatos.length) {
            $scope.msgerros.push("Cadastre um contato para o cliente");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (cli.tipopessoa == 'F') {
            if (angular.isUndefined(cli.nome) || cli.nome === null || cli.nome == "") {
                $scope.msgerros.push("Informe o nome do cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(cli.CPF) || cli.CPF === null || cli.CPF == "") {
                $scope.msgerros.push("Informe o CPF do cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(cli.identidade) || cli.identidade === null || cli.identidade == "") {
                $scope.msgerros.push("Informe identidade do cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
        } else {
            if (angular.isUndefined(cli.nome) || cli.nome === null || cli.nome == "") {
                $scope.msgerros.push("Informe o nome do cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(cli.fantasia) || cli.fantasia === null || cli.fantasia == "") {
                $scope.msgerros.push("Informe o nome fantasia do cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(cli.IE) || cli.IE === null || cli.IE == "") {
                $scope.msgerros.push("Informe a Inscrição Estadual do cliente");
                $scope.alerterror = true;
                podegravar = false;
            }
        }
        //}
        if (podegravar) {
            var cpfcnpj = cli.tipopessoa == 'F' ? cli.CPF : cli.CNPJ;

            clienteFactory.Verificarcpfcnpj(cpfcnpj.replace(".", "").replace("/", "").replace("-", "").replace(".", "")).then(
                    function (resultcli) {
                        if (resultcli.data.cdentidade > 0 && !$scope.editando ) {
                            console.log(resultcli.data);
                            $scope.clienteexiste = true;
                            $scope.msgerros.push("Ja existe um cliente com o CPF/CNPJ informado!");
                            $scope.alerterror = true;
                        }
                        else {
                            if (podegravar) {
                                cli.Enderecos = $scope.enderecos;
                                cli.Contatos = $scope.contatos;
                                clienteFactory.GravaCliente(cli).then(
                                        function (resultcli) {

                                            //console.log(resultcli.data);

                                            if (resultcli.data > 0) {
                                                //console.log("resultcli.data" + resultcli.data);
                                                $scope.msgerros = [];
                                                $scope.alerterror = false;
                                                $location.path('/CadastroCliente/' + resultcli.data);
                                                alert("Cliente salvo com sucesso!");

                                            }
                                        }
                                        , function (error) {
                                            alert(error);
                                        }
                                    );
                            }
                        }
                    }
                    , function (error) {
                        alert(error);
                    }
                );
        }
    }
    $scope.PesquisarCep = function (cep) {
        //alert($scope.cep.replace("-", ""));
        cepFactory.RetornaEnderecoByCep(cep.replace("-", "")).then(
                function (resultcep) {
                    //console.log(resultcep.data);
                    if (resultcep.data == null) {
                        $scope.msgerror = "cep não encontrado";
                        $scope.alerterror = true;
                    } else {
                        $scope.ende.logradouro = resultcep.data.Endereco;
                        $scope.ende.bairro = resultcep.data.Bairro;
                        $scope.ende.UF = resultcep.data.Uf;
                        $scope.ende.CEP = resultcep.data.Cep;
                        $scope.ende.cidade = resultcep.data.Cidade;
                        $scope.ende.cdmunicipioibge = resultcep.data.CdmunicipioIbge;
                        $scope.msgerror = "";
                        $scope.alerterror = false;
                    }

                }
                , function (error) {
                    alert(error);
                    //console.log(error);
                }
            );
    }
    $scope.SalvarEndereco = function (end) {
        //$location.path('/CadastroCliente');
        $scope.msgerros = [];
        var podegravar = true;
        console.log(end.CEP);
        if (angular.isUndefined(end.CEP) || end.CEP === null || end.CEP == "") {
            $scope.msgerros.push("Informe o CEP");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(end.UF) || end.UF === null || end.UF == "") {
            $scope.msgerros.push("Informe a UF");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(end.cidade) || end.cidade === null || end.cidade =="") {
            $scope.msgerros.push("Informe a Cidade");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(end.cdtipoendereco) || end.cdtipoendereco === null || end.cdtipoendereco =="") {
            $scope.msgerros.push("Informe o tipo do endereço");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(end.logradouro) || end.logradouro === null || end.logradouro =="") {
            $scope.msgerros.push("Informe o logradouro.");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(end.numero) || end.numero === null || end.numero =="") {
            $scope.msgerros.push("Informe o número ou 0 se não tiver.");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(end.bairro) || end.bairro === null || end.bairro =="") {
            $scope.msgerros.push("Informe o bairro.");
            $scope.alerterror = true;
            podegravar = false;
        }


        if (podegravar) {

            //console.log(end);
            var endsa = end;
            var index = end.idx;
            if (index < 99) {
                $scope.enderecos.splice(index, 1);
            }

            //$scope.$apply();
            $scope.enderecos.push(
                {
                    cdendereco: end.cdendereco,
                    cdtipoendereco: end.cdtipoendereco,
                    UF: end.UF,
                    CEP: end.CEP,
                    cidade: end.cidade,
                    logradouro: end.logradouro,
                    bairro: end.bairro,
                    numero: end.numero,
                    complemento: end.complemento,
                    cdmunicipioibge: end.cdmunicipioibge,
                    cdcontato: end.cdcontato,
                    nmenderecoweb: end.nmenderecoweb,
                    cdenderecoweb: end.cdenderecoweb,
                    referencia: end.referencia,
                    nmtipoendereco: end.nmtipoendereco,
                    principal: end.principal,
                    excluir: end.excluir
                });
            //console.log(end);
            // $scope.ende = {};
            $scope.ende.cdendereco = "";
            $scope.ende.cdtipoendereco = "";
            $scope.ende.UF = "";
            $scope.ende.CEP = "";
            $scope.ende.cidade = "";
            $scope.ende.logradouro = "";
            $scope.ende.bairro = "";
            $scope.ende.numero = "";
            $scope.ende.complemento = "";
            $scope.ende.cdmunicipioibge = "";
            $scope.ende.cdcontato = "";
            $scope.ende.nmenderecoweb = "";
            $scope.ende.cdenderecoweb = "";
            $scope.ende.referencia = "";
            $scope.ende.nmtipoendereco = "";
            $scope.ende.principal = "";
            $scope.ende.excluir = 0;
            $scope.ende.idx = 99;
            //console.log($scope.enderecos);
            $scope.alerterror = false;
        }

    }
    $scope.LimparEndereco = function () {
        $scope.ende.cdendereco = "";
        $scope.ende.cdtipoendereco = "";
        $scope.ende.UF = "";
        $scope.ende.CEP = "";
        $scope.ende.cidade = "";
        $scope.ende.logradouro = "";
        $scope.ende.bairro = "";
        $scope.ende.numero = "";
        $scope.ende.complemento = "";
        $scope.ende.cdmunicipioibge = "";
        $scope.ende.cdcontato = "";
        $scope.ende.nmenderecoweb = "";
        $scope.ende.cdenderecoweb = "";
        $scope.ende.referencia = "";
        $scope.ende.nmtipoendereco = "";
        $scope.ende.principal = "";
        $scope.ende.excluir = 0;
        $scope.ende.idx = 99;
    }
    $scope.EditarEndereco = function (end, idx, excluir) {

        if (excluir == 1) {
            end.excluir = 1;
            end.idx = idx;
            $scope.SalvarEndereco(end);
        } else {
            //var absolute_index = idx + ($scope.currentPage - 1) * $scope.pageSize;
            //console.log(absolute_index);
            $scope.cdenderecosel = end.cdendereco;
            $scope.ende.cdendereco = end.cdendereco;
            $scope.ende.cdtipoendereco = end.cdtipoendereco;
            $scope.ende.UF = end.UF;
            $scope.ende.CEP = end.CEP;
            $scope.ende.cidade = end.cidade;
            $scope.ende.logradouro = end.logradouro;
            $scope.ende.bairro = end.bairro;
            $scope.ende.numero = end.numero;
            $scope.ende.complemento = end.complemento;
            $scope.ende.cdmunicipioibge = end.cdmunicipioibge;
            $scope.ende.cdcontato = end.cdcontato;
            $scope.ende.nmenderecoweb = end.nmenderecoweb;
            $scope.ende.cdenderecoweb = end.cdenderecoweb;
            $scope.ende.referencia = end.referencia;
            $scope.ende.nmtipoendereco = end.nmtipoendereco;
            $scope.ende.principal = end.principal;
            $scope.ende.excluir = end.excluir;
            $scope.ende.idx = idx;
        }


    }

    $scope.SalvarContato = function (cont) {

        $scope.msgerros = [];
        var podegravar = true;
        console.log("cont.dddresi" + cont.dddresi);
        if (angular.isUndefined(cont.cdtipocontato) || cont.cdtipocontato === null || cont.cdtipocontato == 0) {
            $scope.msgerros.push("Informe o tipo de contato");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.stdefault) || cont.stdefault === null) {
            $scope.msgerros.push("Informe se e o contato principal");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.nome) || cont.nome === null || cont.nome == "") {
            $scope.msgerros.push("Informe o nome do contato");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.dddresi) || cont.dddresi === null || cont.dddresi == "") {
            $scope.msgerros.push("Informe o DDD do telefone");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.telresidencial) || cont.telresidencial === null || cont.telresidencial == "") {
            $scope.msgerros.push("Informe o telefone residencial");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.dddcel) || cont.dddcel === null || cont.dddcel == "") {
            $scope.msgerros.push("Informe o DDD do celular");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.telcelular) || cont.telcelular === null || cont.telcelular == "") {
            $scope.msgerros.push("Informe o telefone celular");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cont.email) || cont.email === null || cont.email == "") {
            $scope.msgerros.push("Informe o e-mail do contato");
            $scope.alerterror = true;
            podegravar = false;
        }

        if (podegravar) {
            var index = cont.idx;
            console.log("cont.idx " + cont.idx);
            console.log(cont);
            console.log(cont.apelido);
            if (index < 99) {
                $scope.contatos.splice(index, 1);
            }

            $scope.contatos.push(
                {
                    cdcontato: cont.cdcontato,
                    cdtipocontato: cont.cdtipocontato,
                    stdefault: cont.stdefault,
                    nome: cont.nome,
                    apelido: cont.apelido,
                    observacao: cont.observacao,
                    nmtipocontato: "",
                    excluir: cont.excluir,
                    Emails: $scope.emails,
                    Telefones: $scope.telefones,
                    cdtelefoneresi: cont.cdtelefoneresi,
                    dddresi: cont.dddresi,
                    telresidencial: cont.telresidencial,
                    cdtelefonecel: cont.cdtelefonecel,
                    dddcel: cont.dddcel,
                    telcelular: cont.telcelular,
                    cdemail: cont.cdemail,
                    email: cont.email
                });
            console.log($scope.contatos);
            $scope.contato.cdcontato = 0;
            $scope.contato.cdtipocontato = 0;
            $scope.contato.stdefault = "0";
            $scope.contato.nome = "";
            $scope.contato.apelido = "";
            $scope.contato.observacao = "";
            $scope.contato.nmtipocontato = "";
            $scope.contato.excluir = 0;
            $scope.contato.Emails = [];
            $scope.contato.Telefones = [];
            $scope.telefones = [];
            $scope.emails = [];
            $scope.nmcontatoselecionado = "";
            $scope.cdcontatoselecionado = 0;
            $scope.idxcontatoselecionado = 99;
            $scope.contato.cdtelefoneresi = 0;
            $scope.contato.dddresi = "";
            $scope.contato.telresidencial = "";
            $scope.contato.cdtelefonecel = 0;
            $scope.contato.dddcel = "";
            $scope.contato.telcelular = "";
            $scope.contato.cdemail = 0;
            $scope.contato.email = "";
        }

    }
    $scope.LimparContato = function () {
        $scope.contato.cdcontato = 0;
        $scope.contato.cdtipocontato = 0;
        $scope.contato.stdefault = "0";
        $scope.contato.nome = "";
        $scope.contato.apelido = "";
        $scope.contato.observacao = "";
        $scope.contato.nmtipocontato = "";
        $scope.contato.excluir = 0;
        $scope.contato.Emails = [];
        $scope.contato.Telefones = [];
        $scope.telefones = [];
        $scope.emails = [];
        $scope.nmcontatoselecionado = "";
        $scope.cdcontatoselecionado = 0;
        $scope.idxcontatoselecionado = 99;
        $scope.contato.cdtelefoneresi = 0;
        $scope.contato.dddresi = "";
        $scope.contato.telresidencial = "";
        $scope.contato.cdtelefonecel = 0;
        $scope.contato.dddcel = "";
        $scope.contato.telcelular = "";
        $scope.contato.cdemail = 0;
        $scope.contato.email = "";
    }
    $scope.EditarContato = function (idx, contato) {
        $scope.contatoselecionado = true;
        $scope.cdcontatoselecionado = contato.cdcontato;
        $scope.telefones = {};
        $scope.emails = {};
        //console.log(contato);
        $scope.contato.cdtipocontato = contato.cdtipocontato;
        $scope.contato.cdcontato = contato.cdcontato;
        $scope.contato.stdefault = contato.stdefault;
        $scope.contato.nome = contato.nome;
        $scope.contato.apelido = contato.apelido;
        $scope.contato.observacao = contato.observacao;
        $scope.contato.idx = idx;
        $scope.telefones = contato.Telefones;
        $scope.emails = contato.Emails;
        $scope.idxcontatoselecionado = idx;
        $scope.nmcontatoselecionado = contato.nome;

        $scope.contato.cdtelefoneresi = contato.cdtelefoneresi;
        $scope.contato.dddresi = contato.dddresi;
        $scope.contato.telresidencial = contato.telresidencial;
        $scope.contato.cdtelefonecel = contato.cdtelefonecel;
        $scope.contato.dddcel = contato.dddcel;
        $scope.contato.telcelular = contato.telcelular;
        $scope.contato.cdemail = contato.cdemail;
        $scope.contato.email = contato.email;
        console.log(contato);

    }
    $scope.ExcluirContato = function (cont, idx) {
        cont.excluir = 1;
        //cont.idx = idx;

        $scope.contatos.splice(idx, 1);
        $scope.contatos.push(
            {
                cdcontato: cont.cdcontato,
                cdtipocontato: cont.cdtipocontato,
                stdefault: cont.stdefault,
                nome: cont.nome,
                apelido: cont.apelido,
                observacao: cont.observacao,
                nmtipocontato: "",
                excluir: cont.excluir,
                Emails: $scope.emails,
                Telefones: $scope.telefones
            });


        $scope.contato.cdcontato = 0;
        $scope.contato.cdtipocontato = 0;
        $scope.contato.stdefault = "2";
        $scope.contato.nome = "";
        $scope.contato.apelido = "";
        $scope.contato.observacao = "";
        $scope.contato.nmtipocontato = "";
        $scope.contato.excluir = 0;
        $scope.contato.Emails = [];
        $scope.contato.Telefones = [];
        $scope.telefones = [];
        $scope.emails = [];
        $scope.nmcontatoselecionado = "";
        $scope.cdcontatoselecionado = 0;
        $scope.idxcontatoselecionado = 99;
        $scope.cdtelefoneresi = 0;
        $scope.dddresi = "";
        $scope.telresidencial = "";
        $scope.cdtelefonecel = 0;
        $scope.dddcel = "";
        $scope.telcelular = "";
        $scope.cdemail = 0;
        $scope.email = "";


    }

    $scope.SalvarTelefone = function (tel) {
        if ($scope.idxcontatoselecionado == 99) {
            $scope.msgerros = [];
            $scope.msgerros.push("Para salvar o telefone selecione um contato.");
            $scope.alerterror = true;
        } else {
            $scope.msgerros = [];
            var podegravar = true;
            if (angular.isUndefined(tel.cdtipotelefone) || tel.cdtipotelefone === null) {
                $scope.msgerros.push("Informe o tipo de telefone");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(tel.telefone) || tel.telefone === null) {
                $scope.msgerros.push("Informe o telefone");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(tel.ddd) || tel.ddd === null) {
                $scope.msgerros.push("Informe o ddd");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (podegravar) {

                //if ($scope.idxtelefonesel < 99)//edicao do tel
                //{
                //    $scope.telefones.splice($scope.idxtelefonesel, 1);
                //}

                $scope.telefones.push(
                {
                    cdtelefone: tel.cdtelefone,
                    cdcontato: $scope.cdcontatoselecionado,
                    cdtipotelefone: tel.cdtipotelefone,
                    ddd: tel.ddd,
                    telefone: tel.telefone,
                    nmtipotelefone: "",
                    excluir: tel.excluir,
                });
                //console.log($scope.telefones);
                $scope.idxtelefonesel = 99;
                $scope.tele.cdtipotelefone = "";
                $scope.tele.ddd = "";
                $scope.tele.telefone = "";
                $scope.tele.excluir = 0;
                $scope.tele.idx = 99;
            }
            //console.log($scope.contatos);
        }

    }
    $scope.EditarTelefone = function (tel, idx) {
        $scope.idxtelefonesel = idx;
        $scope.tele.idx = idx;

        //console.log("idx sel tel-->" + $scope.tele.idx);

        $scope.tele.cdtipotelefone = tel.cdtipotelefone;
        $scope.tele.cdtelefone = tel.cdtelefone;
        $scope.tele.ddd = tel.ddd;
        $scope.tele.telefone = tel.telefone;
        $scope.tele.excluir = 0;

    }
    $scope.ExcluirTelefone = function (tel, idx) {
        //tel.idx = idx;
        tel.excluir = 1;
        $scope.telefones.splice(idx, 1);
        $scope.telefones.push(
                {
                    cdtelefone: tel.cdtelefone,
                    cdcontato: $scope.cdcontatoselecionado,
                    cdtipotelefone: tel.cdtipotelefone,
                    ddd: tel.ddd,
                    telefone: tel.telefone,
                    nmtipotelefone: "",
                    excluir: tel.excluir,
                    idx: 99
                });

        //console.log($scope.telefones);
    }

    $scope.EditarEmail = function (email, idx) {
        $scope.idxemailsel = idx;
        $scope.email.cdemail = email.cdemail;
        $scope.email.cdtipoemail = email.cdtipoemail;
        $scope.email.txemail = email.txemail;
        $scope.email.excluir = email.excluir;
        $scope.email.idx = idx;
    }
    $scope.ExcluirEmail = function (email, idx) {
        //email.idx = idx;
        email.excluir = 1;
        $scope.emails.splice(idx, 1);
        $scope.emails.push(
                {
                    cdemail: email.cdemail,
                    cdtipoemail: email.cdtipoemail,
                    txemail: email.txemail,
                    excluir: email.excluir,
                    idx: 99
                });

        //$scope.SalvarEmail(email);
    }
    $scope.SalvarEmail = function (email) {
        if ($scope.idxcontatoselecionado == 99) {
            $scope.msgerros = [];
            $scope.msgerros.push("Para salvar o e-mail selecione um contato.");
            $scope.alerterror = true;
        } else {
            $scope.msgerros = [];
            var podegravar = true;
            if (angular.isUndefined(email.cdtipoemail) || email.cdtipoemail === null) {
                $scope.msgerros.push("Informe o tipo de email");
                $scope.alerterror = true;
                podegravar = false;
            }
            if (angular.isUndefined(email.txemail) || email.txemail === null) {
                $scope.msgerros.push("Informe o email");
                $scope.alerterror = true;
                podegravar = false;
            }

            if (podegravar) {

                $scope.emails.push(
                {
                    cdemail: email.cdemail,
                    cdtipoemail: email.cdtipoemail,
                    txemail: email.txemail,
                    excluir: email.excluir,
                    idx: 99
                });
                $scope.idxemailsel = 99;
                $scope.email.cdtipoemail = "";
                $scope.email.ddd = "";
                $scope.email.txemail = "";
                $scope.email.excluir = 0;
                $scope.email.idx = 99;
                //console.log($scope.emails);
            }
            //console.log($scope.contatos);
        }
    }

    $scope.VerificarcpfcnpjCliente = function (cpfcnpj) {

        if (angular.isUndefined(cpfcnpj) || cpfcnpj === null || cpfcnpj == "") {

        } else {
            clienteFactory.Verificarcpfcnpj(cpfcnpj.replace(".", "").replace("/", "").replace("-", "").replace(".", "")).then(
                           function (resultcli) {
                               if (resultcli.data.cdentidade > 0) {
                                   console.log(resultcli.data);
                                   $scope.clienteexiste = true;
                                   $scope.msgerros.push("Ja existe um cliente com o CPF/CNPJ informado!");
                                   $scope.alerterror = true;
                                   $scope.alertinfo = false;
                               }
                               else {
                                   $scope.msginfo = "Não existe nenhum cliente com o CPF/CNPJ informado!";
                                   $scope.alertinfo = true;
                                   $scope.alerterror = false;
                               }
                           }
                           , function (error) {
                               alert(error);
                           }
                       );
        }
    }
}
]).controller('pedidoCtrl', ['$scope', 'clienteFactory', '$location', '$routeParams', 'Utils', '$localStorage', 'produtoFactory', 'cestaFactory', 'axadoFactory', '$filter', 'paypalFactory', '$window', 'WizardHandler', 'trayFactory', 'produtoAfiliadoFactory',
function ($scope, clienteFactory, $location, $routeParams, Utils, $localStorage, produtoFactory, cestaFactory, axadoFactory, $filter, paypalFactory, $window, WizardHandler, trayFactory, produtoAfiliadoFactory) {

    $scope.alerterror = false;
    $scope.msgerror = "";
    $scope.alertok = false;
    $scope.msgok = "";
    $scope.alertinfo = false;
    $scope.msginfo = "";
    $localStorage.cdentidadesel = 0;
    $localStorage.cli = {};
    $scope.clinome = "";
    $scope.produtos = {};
    $scope.nomeprodutobusca = "";
    $scope.cdprodutobusca = "";
    $scope.cotacoes = {};
    $scope.cotacaosel = "";
    $scope.cotacaoselValor = 0;
    $localStorage.cotacaoselValor = 0;
    $localStorage.cestalist = {};
    $localStorage.cotacaosel = "";

    $scope.cdenderecosel = 0;
    $localStorage.cdenderecosel = 0;
    $scope.enderecosel = {};
    $localStorage.enderecosel = {};
    // $localStorage.cdenderecosel = 0;
    $localStorage.datapedido = "";
    $localStorage.cdentidade = 0;
    $localStorage.cdentidadesel = 0;
    $localStorage.cli = {};
    $localStorage.codped = 0;
    $scope.codpedPagar = 0;
    $localStorage.codpedPagar = 0;
    $localStorage.clienteselecionado = {};
    $localStorage.subtotal = 0;
    $localStorage.tokenaxado = "";
    $localStorage.formapagamentosel = "";
    $scope.formapagamentosel = "";
    $scope.pagamentoselecionado = false;
    $scope.pagamentoclicado = false;
    $scope.retornoprocessado = false;
    $scope.card = {};
    $scope.msgerros = [];
    $localStorage.cdcontatosel = 0;
    $scope.parcelamento = [];
    $localStorage.statuspagtotray = "";
    $scope.statuspagtotray = "";
    $scope.statuspagtotraydesc = "";
    $scope.step = 0;
    $scope.pagamentosucesso = false;
    $scope.alerterrorcard = false;
    $scope.msgerrorcard = "";
    $scope.cartaoselecionado = false;

    $scope.card.split = "";
    $scope.card.card_name = "";
    $scope.card.card_number = "";
    $scope.card.card_expdate_month = "";
    $scope.card.card_expdate_year = "";
    $scope.card.card_cvv = "";
    $scope.card.payment_method_id = "";
    $scope.pagamentoefetuado = false;
    $scope.tokentray = "";
    $scope.transactionidtray = "";
    $scope.codpagamento = "";
    $scope.splitpagamento = "";
    $localStorage.splitpagamento = "";
    $localStorage.valortotalpedido = "";
    $scope.valortotalpedido = "";
    $scope.IsClickEnable = true;


    if (parseInt($routeParams.cdpedido) > 0) {
        cestaFactory.Criarcesta($routeParams.cdpedido).then(
                function (result) {
                    $scope.cestalist = result.data;
                    $localStorage.cestalist = result.data;
                    $scope.cdpedido = result.data[0].codped;
                    $localStorage.codped = result.data[0].codped;
                    $scope.codpedPagar = $localStorage.codped;
                    $localStorage.codpedPagar = result.data[0].zcodpedgravado;
                    $scope.totalcesta = result.data[0].TotalCesta;
                    $scope.subtotal = result.data[0].TotalCesta;
                    $localStorage.subtotal = result.data[0].TotalCesta;
                    $localStorage.datapedido = result.data[0].datapedido.substr(0, 10);
                    $scope.datapedido = $localStorage.datapedido;
                    $scope.descontos = parseFloat(result.data[0].TotalDescontoCesta).toFixed(2);
                    $scope.pagamentos = 0;
                    $scope.QtdeTotal = result.data[0].QuantidadeTotalCesta;
                    $scope.QuantidadeValue = 1;
                    $scope.ProdutoValue = "";
                    $scope.produtolist = null;
                    $scope.totalprodutos = 0;


                    clienteFactory.RetornaClienteByCdentidade(result.data[0].codcli).then(
                        function (resultcli) {

                            $localStorage.cdentidadesel = parseInt(result.data[0].codcli);
                            $localStorage.cli = resultcli.data;
                            $scope.clinome = resultcli.data.nome;

                            clienteFactory.RetornaEnderecoContatoByCdentidade($localStorage.cdentidadesel).then(
                                            function (resultsend) {
                                                $scope.enderecoscontato = resultsend.data;
                                                if (resultsend.data.length == 0) {
                                                    $scope.alertinfo = true;
                                                    $scope.alertok = false;
                                                    $scope.alerterror = false;
                                                    $scope.msginfo = "O cliente não possui nenhum endereco cadastrado, efetue o cadastro.";
                                                    //WizardHandler.wizard("pedidoWizard").goTo(2);
                                                } else {
                                                    $scope.alertinfo = false;
                                                    $scope.alertok = false;
                                                    $scope.alerterror = false;
                                                    $scope.cdenderecosel = resultsend.data[0].cdendereco;
                                                    $localStorage.cdenderecosel = resultsend.data[0].cdendereco;
                                                    $scope.enderecosel = resultsend.data[0];
                                                    $localStorage.enderecosel = resultsend.data[0];
                                                    if (resultsend.data.length == 1)//se possui 1 endereco ja busca na achado os tipos de frete
                                                    {
                                                        // WizardHandler.wizard("pedidoWizard").goTo(2);
                                                        axadoFactory.RetornaCotacao($scope.cdpedido, resultsend.data[0].cdcep).then(
                                                            function (resultscota) {
                                                                //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                                //console.log(resultscota.data.Cotacoes);
                                                                //console.log("cotacao 3-->");
                                                                //console.log(resultscota.data);
                                                                $scope.cotacoes = resultscota.data.Cotacoes;
                                                                $localStorage.tokenaxado = resultscota.data.consulta_token;
                                                            }
                                                        );
                                                    }
                                                }
                                            }
                                            );


                            //clienteFactory.RetornaContatosClienteByCdentidade($localStorage.cdentidadesel).then(
                            //function (resultscont) {

                            //    if (resultscont.data.length == 0) {
                            //        $scope.alertinfo = true;
                            //        $scope.alertok = false;
                            //        $scope.alerterror = false;
                            //        $scope.msginfo = "O cliente não possui nenhum contato/endereco cadastrado, efetue o cadastro.";

                            //    } else {
                            //        $scope.alertinfo = false;
                            //        $scope.alertok = false;
                            //        $scope.alerterror = false;
                            //        $scope.contatos = resultscont.data;
                            //        //WizardHandler.wizard("pedidoWizard").goTo(2);
                            //        //se possui 1 contato carrega os enderecos
                            //        if (resultscont.data.length == 1) {

                            //            $localStorage.cdcontatosel = resultscont.data[0].cdcontato;

                            //            WizardHandler.wizard("pedidoWizard").goTo(2);

                            //        } else {
                            //            $scope.umContatoEndereco = false;
                            //            WizardHandler.wizard("pedidoWizard").goTo(2);
                            //        }
                            //    }
                            //}
                            //);//RetornaContatosClienteByCdentidade
                        }
                        , function (error) {
                            alert(error);
                        }
                    );
                    //$localStorage.cestalist = result.data;WizardHandler

                    //WizardHandler.wizard("pedidoWizard").goTo(2);
                }
                , function (error) {
                    alert(error);
                }
            );
    }



    $scope.PesquisarClientePed = function (nomeclienteped) {
        var cpfcnpjunmusk = $('.cpfOuCnpj').cleanVal();

        if (Utils.isUndefinedOrNull(nomeclienteped) && Utils.isUndefinedOrNull(cpfcnpjunmusk)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa do cliente."
            // console.log("err");
        } else {
            clienteFactory.RetornaClienteByParam(nomeclienteped, cpfcnpjunmusk).then(
                 function (results) {
                     $scope.clientes = {};
                     //$scope.promocoes = {};

                     if (results.data.length == 0) {
                         $scope.alertinfo = true;
                         $scope.alertok = false;
                         $scope.alerterror = false;
                         $scope.msginfo = "Sua pesquisa não retornou nenhum resultado.";
                         //console.log(results.data.length);
                     } else {
                         $scope.alertinfo = false;
                         $scope.alertok = false;
                         $scope.alerterror = false;
                         $scope.clientes = results.data;

                     }
                 }
             );
        }


    }

    $scope.NovoCliente = function () {
        $location.path('/CadastroCliente');
    }

    $scope.SelecionarCliente = function (cli) {
        clienteFactory.RetornaContatosSeTemTelefone(cli.cdentidade).then(
            function (results) {
                if (!results.data) {
                    $scope.alertinfo = false;
                    $scope.alertok = false;
                    $scope.alerterror = true;
                    $scope.msgerror = "Cadastre um telefone para o cliente selecionado.";
                    //console.log(results.data.length);
                } else {
                    $scope.alertinfo = false;
                    $scope.alertok = false;
                    $scope.alerterror = false;
                    //$scope.clientes = results.data;
                    $localStorage.cdentidadesel = cli.cdentidade;
                    $scope.cdentidadesel = cli.cdentidade;
                    $localStorage.cli = cli;
                    $scope.clinome = cli.nome;
                    console.log(cli.nome);
                }
            }
            );

    }

    $scope.EditarCliente = function (cdentidade) {
        $location.path('/CadastroCliente/' + cdentidade);
    }
    //###############################
    //Wizard
    $scope.goBack = function () {
        $window.location.reload();
    };

    $scope.finishedWizard = function () {
        return true;
    };
    $scope.enterValidation = function () {
        return true;
    };

    $scope.exitValidationcli = function () {
        // console.log("exitv" + $localStorage.cdentidadesel);
        if ($localStorage.cdentidadesel == 0) {
            $scope.alerterror = true;
            $scope.msgerror = "Selecione um cliente."
        } else {
            $scope.alerterror = false;
            $scope.msgerror = ""
        }
        return $localStorage.cdentidadesel > 0;
    };

    $scope.exitValidationprod = function () {
        //console.log("$localStorage.cestalist");
        //console.log($localStorage.cestalist);
        var passou = false;
        if (angular.isUndefined($localStorage.cestalist.length)) {
            $scope.alerterror = true;
            $scope.msgerror = "Adicione um produto para prosseguir para o Pedido."
        } else {

            if ($localStorage.cestalist.length == 0) {
                $scope.alerterror = true;
                $scope.msgerror = "Adicione um produto para prosseguir para o Pedido."
            } else {
                passou = true;
                $scope.alerterror = false;
                $scope.msgerror = ""

                $scope.contatos = {};
                $scope.enderecoscontato = {};

                $scope.umContatoEndereco = true;

                clienteFactory.RetornaEnderecoContatoByCdentidade($localStorage.cdentidadesel).then(
                                function (resultsend) {
                                    $scope.enderecoscontato = resultsend.data;
                                    if (resultsend.data.length == 0) {
                                        $scope.alertinfo = true;
                                        $scope.alertok = false;
                                        $scope.alerterror = false;
                                        $scope.msginfo = "O cliente não possui nenhum endereco cadastrado, efetue o cadastro.";
                                        //WizardHandler.wizard("pedidoWizard").goTo(2);
                                    } else {
                                        $scope.alertinfo = false;
                                        $scope.alertok = false;
                                        $scope.alerterror = false;
                                        $scope.cdenderecosel = resultsend.data[0].cdendereco;
                                        $localStorage.cdenderecosel = resultsend.data[0].cdendereco;
                                        $scope.enderecosel = resultsend.data[0];
                                        $localStorage.enderecosel = resultsend.data[0];
                                        if (resultsend.data.length == 1)//se possui 1 endereco ja busca na achado os tipos de frete
                                        {
                                            // WizardHandler.wizard("pedidoWizard").goTo(2);
                                            axadoFactory.RetornaCotacao($scope.cdpedido, resultsend.data[0].cdcep).then(
                                                function (resultscota) {
                                                    //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                    //console.log(resultscota.data.Cotacoes);
                                                    //console.log("cotacao 3-->");
                                                    //console.log(resultscota.data);
                                                    $scope.cotacoes = resultscota.data.Cotacoes;
                                                    $localStorage.tokenaxado = resultscota.data.consulta_token;
                                                }
                                            );
                                        }
                                    }
                                }
                                );
            }
        }
        return passou;
    };
    //example using context object

    //example using promises

    //###############################

    $scope.PesquisarProduto = function (pcdprodutobusca, pnomeprodutobusca) {

        if (Utils.isUndefinedOrNull(pcdprodutobusca) && Utils.isUndefinedOrNull(pnomeprodutobusca)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa do produto."
            //console.log("err");
        } else {
            produtoAfiliadoFactory.GetProdutosSel($localStorage.cdpai, pnomeprodutobusca, pcdprodutobusca).then(
               function (results) {
                   $scope.produtos = {};
                   if (results.data.length == 0) {
                       $scope.alertinfo = true;
                       $scope.alertok = false;
                       $scope.alerterror = false;
                       $scope.msginfo = "Sua pesquisa não retornou nenhum resultado.";
                       //console.log(results.data.length);
                   } else {
                       $scope.alertinfo = false;
                       $scope.alertok = false;
                       $scope.alerterror = false;
                       $scope.produtos = results.data;

                   }
               }
               , function (error) {
                   alert(error);
               }
           );

            //    produtoFactory.RetornaProdutoByParamParceiro($localStorage.codforn).then(
            //    function (results) {
            //        $scope.produtos = {};
            //        if (results.data.length == 0) {
            //            $scope.alertinfo = true;
            //            $scope.alertok = false;
            //            $scope.alerterror = false;
            //            $scope.msginfo = "Sua pesquisa não retornou nenhum resultado.";
            //            //console.log(results.data.length);
            //        } else {
            //            $scope.alertinfo = false;
            //            $scope.alertok = false;
            //            $scope.alerterror = false;
            //            $scope.produtos = results.data;

            //        }
            //    }
            //);
        }
    }

    $scope.cestalist = {};

    $scope.AddCesta = function (item, tipo) {
        $scope.IsClickEnable = false;
        var codempresa = $localStorage.CdFilial;
        var codped = $scope.cdpedido == null ? "" : $scope.cdpedido;
        var cliente = $localStorage.cdentidadesel;
        var podeincluir = true;
        // console.log("item cesta");
        // console.log(item);
        //console.log("Cod.ped->" + $scope.cdpedido);
        if (tipo == 1) {
            var produ = item.cdproduto + "|1|" + item.nmproduto;
        } else {

            //if (item.preco > item.precomaximo) {
            //    $scope.alertinfo = false;
            //    $scope.alertok = false;
            //    $scope.alerterror = true;
            //    $scope.msgerror = "O valor máximo para o  produto " + item.nomeprod + " é de " + $filter('currency')(item.precomaximo);
            //    podeincluir = false;
            //} else if (item.preco < item.precominio) {
            //    $scope.alertinfo = false;
            //    $scope.alertok = false;
            //    $scope.alerterror = true;
            //    podeincluir = false;
            //    $scope.msgerror = "O valor mínimo para o produto " + item.nomeprod + " é de " + $filter('currency')(item.precominio);
            //} else {
            //    $scope.alertinfo = false;
            //    $scope.alertok = false;
            //    $scope.alerterror = false;
            //}

            var produ = item.codprod + "|" + item.quantfaturada + "|" + item.nomeprod;
        }

        var finaliza = "S";
        var operador = $localStorage.CdFuncionario;
        var codend = $localStorage.cdenderecosel;
        $localStorage.codped = "";
        $localStorage.codpedPagar = "";
        $localStorage.subtotal = "";

        //$localStorage.codforn = $localStorage.cdfuncionario;//TODO pegar no login do usuario

        if (podeincluir) {
            cestaFactory.GravaCesta(codempresa, codped, cliente, produ, finaliza, operador, codend, item.preco, 0, $localStorage.codforn).then(
                                function (resultcesta) {
                                    $scope.IsClickEnable = true;
                                    $scope.cestalist = resultcesta.data;
                                    $localStorage.cestalist = resultcesta.data;
                                    if (resultcesta.data.length > 0) {
                                        console.log($scope.cestalist);
                                        $scope.cdpedido = $scope.cestalist[0].codped;
                                        $localStorage.codped = $scope.cestalist[0].codped;
                                        $scope.codpedPagar = $localStorage.codped;
                                        $localStorage.codpedPagar = $scope.cestalist[0].zcodpedgravado;
                                        $scope.totalcesta = $scope.cestalist[0].TotalCesta;
                                        $scope.subtotal = $scope.cestalist[0].TotalCesta;
                                        $localStorage.subtotal = $scope.cestalist[0].TotalCesta;
                                        $localStorage.datapedido = $scope.cestalist[0].datapedido.substr(0, 10);
                                        $scope.datapedido = $localStorage.datapedido;
                                        $scope.descontos = parseFloat($scope.cestalist[0].TotalDescontoCesta).toFixed(2);
                                        $scope.pagamentos = 0;
                                        $scope.QtdeTotal = $scope.cestalist[0].QuantidadeTotalCesta;
                                        //codped = $scope.cdpedido;
                                        //alert($scope.cdpedido);
                                        $scope.QuantidadeValue = 1;
                                        $scope.ProdutoValue = "";
                                        $scope.produtolist = null;
                                        $scope.totalprodutos = 0;
                                    }

                                }, function (error) {
                                    alert(error.message);
                                }
                                );
        } else {

        }

    }

    $scope.AtualizaItemCesta = function (item) {
        console.log(item);
        if (item.preco < item.precovenda) {
            $scope.alertinfo = false;
            $scope.alertok = false;
            $scope.alerterror = true;
            $scope.msgerror = "O valor do produto não pode ser menor que " + $filter('currency')(item.precovenda);
            podeincluir = false;
        } else {
            $scope.alertinfo = false;
            $scope.alertok = false;
            $scope.alerterror = false;
            $scope.msgerror = "";
            $scope.AddCesta(item);
        }

    }

    $scope.RemoverProdutoCesta = function (item) {
        item.precovenda = 0;
        item.preco = 0;
        item.quantfaturada = 0;
        $scope.AddCesta(item);
    }
    $scope.SelecionarContato = function (cont) {
        $localStorage.cdcontatosel = cont.cdcontato;
        $scope.cdcontatosel = cont.cdcontato;
        clienteFactory.RetornaEnderecoContatoByCdcontato(cont.cdcontato).then(
                            function (resultsend) {
                                $scope.enderecoscontato = resultsend.data;
                                if (resultsend.data.length == 0) {
                                    $scope.alertinfo = true;
                                    $scope.alertok = false;
                                    $scope.alerterror = false;
                                    $scope.msginfo = "O cliente não possui nenhum endereco cadastrado, efetue o cadastro.";
                                } else {
                                    $scope.alertinfo = false;
                                    $scope.alertok = false;
                                    $scope.alerterror = false;
                                    //console.log("cep-->" + resultsend.data[0].cdcep);
                                    //console.log(resultsend.data);
                                    if (resultsend.data.length == 1)//se possui 1 endereco ja busca na achado os tipos de frete
                                    {
                                        $scope.cdenderecosel = resultsend.data[0].cdendereco;
                                        $localStorage.cdenderecosel = resultsend.data[0].cdendereco;
                                        $scope.enderecosel = resultsend.data[0];
                                        $localStorage.enderecosel = resultsend.data[0];
                                        axadoFactory.RetornaCotacao($scope.cdpedido, resultsend.data[0].cdcep).then(
                                            function (resultscota) {
                                                //console.log("cotacao 1-->");
                                                //console.log(resultscota.data);
                                                $scope.cotacoes = resultscota.data.Cotacoes;
                                                $localStorage.tokenaxado = resultscota.data.consulta_token;
                                            }
                                        );
                                        //update a cesta com o codigo do endereco que ja pega do localstorage
                                        $scope.AddCesta($scope.cestalist[0]);
                                    }
                                }
                            }
                            );
    }
    $scope.SelecionarEnderEntrega = function (ende) {
        $scope.cdenderecosel = ende.cdendereco;
        $localStorage.cdenderecosel = ende.cdendereco;
        $scope.enderecosel = ende;
        $localStorage.enderecosel = ende;
        $scope.alertinfo = false;
        $scope.alertok = false;
        $scope.alerterror = false;
        axadoFactory.RetornaCotacao($scope.cdpedido, ende.cdcep).then(
                                            function (resultscota) {
                                                //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                //console.log(resultscota.data.Cotacoes);
                                                //console.log("cotacao 2-->");
                                                //console.log(resultscota.data);
                                                $scope.cotacoes = resultscota.data.Cotacoes;
                                                $localStorage.tokenaxado = resultscota.data.consulta_token;
                                            }
                                        );
        //update a cesta com o codigo do endereco que ja pega do localstorage
        $scope.AddCesta($scope.cestalist[0]);

    }
    $scope.SelecionarModalidade = function (moda) {
        $scope.cotacaosel = moda.servico_nome;
        $localStorage.cotacaosel = moda.servico_nome;
        $scope.cotacaoselValor = moda.cotacao_preco;
        $localStorage.cotacaoselValor = moda.cotacao_preco;
        //console.log("moda-->");
        //console.log(moda);
        //cdpedido, vrfrete, txtipofrete, tokenaxado
        cestaFactory.UpdatePedido($localStorage.codpedPagar, $scope.cotacaoselValor, $localStorage.cotacaosel, $localStorage.tokenaxado, $localStorage.cdenderecosel).then(
                                            function (resultupd) {
                                                //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                //console.log(resultupd);
                                                //$scope.cotacoes = resultscota.data.Cotacoes;

                                                cestaFactory.RetornaTotalPedido($localStorage.codpedPagar).then(
                                            function (resultparc) {
                                                //console.log(resultparc);
                                                $localStorage.valortotalpedido = resultparc.data;
                                                $scope.valortotalpedido = resultparc.data;
                                            }, function (error) {
                                                alert(error.message);
                                            }
                                            );


                                            }
                                        );

    }

    $scope.exitValidationPed = function () {
        //console.log("exitv");
        if ($scope.cotacaosel == "") {
            $scope.alerterror = true;
            $scope.msgerror = "Selecione uma modalidade de envio."
        } else {
            $scope.alerterror = false;
            $scope.msgerror = ""

            /*
                                                    <b>Endereço:</b> 4F3S8J<br>
                                                    <b>Número:</b> 4F3S8J<br>
                                                    <b>Complemento:</b> 4F3S8J<br>
                                                    <b>Bairro:</b> 2/22/2014<br>
                                                    <b>Cidade:</b> 968-34567<br>
                                                    <b>UF:</b> 968-34567<br>
                                                    <b>Cep:</b> 968-34567<br>
                                                    {{endcont.cdendereco}}</td>
                                                    {{endcont.cdcep}}</td>
                                                    {{endcont.cduf}}</td>
                                                    {{endcont.txcidade}}</td>
                                                    {{endcont.txlogradouro}}</td>
                                                    {{endcont.txbairro}}</td>
                                                    {{endcont.txnumero}}</td>
                                                    {{endcont.txcomplemento}}</td>
            */

            $scope.enderecocomp = $scope.enderecosel.txlogradouro;
            $scope.numerocomp = $scope.enderecosel.txnumero;
            $scope.complementocomp = $scope.enderecosel.txcomplemento;
            $scope.bairrocomp = $scope.enderecosel.txbairro;
            $scope.cidadecomp = $scope.enderecosel.txcidade;
            $scope.ufcomp = $scope.enderecosel.cduf;
            $scope.cepcomp = $scope.enderecosel.cdcep;



        }
        return $scope.cotacaosel != "";
    };


    $scope.SetParcelamento = function (parc) {
        angular.forEach($scope.parcelamento, function (value, key) {
            //console.log(value);
            if (value.parcela == parc) {
                $scope.splitpagamento = value.descricao;
                $localStorage.splitpagamento = $scope.splitpagamento;
            }
        });
    }


    $scope.getValorpagar = function () {
        var total = 0;

        total = parseFloat($localStorage.subtotal).toFixed(2) + parseFloat($localStorage.cotacaoselValor).toFixed(2);
        //console.log("total---->" + total);
        return total;
    }

    $scope.updateParcelas = function (idbandeira) {
        //console.log("idbandeira");
        //console.log(idbandeira);
        cestaFactory.RetornaParcelamento($localStorage.codpedPagar, idbandeira).then(
                           function (resultparc) {
                               $scope.parcelamento = resultparc.data.splits;
                           }, function (error) {
                               alert(error.message);
                           }
                           );
    }
    $scope.ImprmirBoleto = function () {
        if (angular.isUndefined($scope.url_payment) || $scope.url_payment === null || $scope.url_payment == "") {
            $scope.alerterror = true;
            $scope.msgerror = "Não foi possível imprimir o boleto!";
        } else {
            $scope.alerterror = false;
            var w = $window.open($scope.url_payment, '', 'width=1200,height=728,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
            //w.focus();
        }
    }
    $scope.SelecioanarForma = function (forma) {
        $localStorage.formapagamentosel = forma;
        $scope.formapagamentosel = forma;
        $scope.pagamentoselecionado = true;
        if (forma == "boleto") {
            $scope.cartaoselecionado = false;
            $scope.card.payment_method_id = "6";
            console.log($scope.card.payment_method_id);
        } else {
            $scope.cartaoselecionado = true;
        }
    }
    $scope.exitValidationPagamento = function () {
        if ($scope.formapagamentosel == "cartao") {
            if ($scope.card.card_number != undefined) {
                if ($scope.card.card_number == "") {
                    $scope.add("Informe o número do cartão com 16 digitos");
                    $scope.alerterrorcard = true;
                } else {
                    if ($scope.card.card_number.toString().length < 16 || $scope.card.card_number.toString().length > 16) {
                        $scope.add("Informe o número do cartão com 16 digitos");
                        $scope.alerterrorcard = true;
                    }
                }

            } else {
                $scope.add("Informe o número do cartão com 16 digitos");
                $scope.alerterrorcard = true;
            }

            if ($scope.card.payment_method_id == "") {
                $scope.add("Informe a bandeira");
                $scope.alerterrorcard = true;
            }
            if ($scope.card.split == "") {
                $scope.add("Selecione o valor de pagamento");
                $scope.alerterrorcard = true;
            }
            if ($scope.card.card_name == "") {
                $scope.add("Informe o nome impresso no cartão");
                $scope.alerterrorcard = true;
            }
            if ($scope.card.card_expdate_month == "") {
                $scope.add("Informe o mês de vencimento do cartão");
                $scope.alerterrorcard = true;
            }
            if ($scope.card.card_expdate_year == "") {
                $scope.add("Informe o ano de vencimento do cartão");
                $scope.alerterrorcard = true;
            }
            if ($scope.card.card_cvv == "") {
                $scope.add("Informe o cvv do cartão");
                $scope.alerterrorcard = true;
            } else {
                if ($scope.card.card_cvv.toString().length < 3 || $scope.card.card_cvv.toString().length > 4) {
                    $scope.add("O campo CVV deve conter entre 3 a 4 numeros");
                    $scope.alerterrorcard = true;
                }
            }
        }
        //console.log($scope.itemserror);

        if (!$scope.alerterrorcard) {
            trayFactory.GravaTransacao($localStorage.codforn, $localStorage.codped, $localStorage.codpedPagar, $scope.card.split, $scope.card.card_name, $scope.card.card_number, $scope.card.card_expdate_month, $scope.card.card_expdate_year, $scope.card.card_cvv, $scope.card.payment_method_id, $localStorage.cdcontatosel).then(
                   function (results) {
                       //console.log(results.data);
                       $scope.pagamentoclicado = true;
                       console.log(results.data);
                       if (results.data.sucessResponse) {
                           console.log(results.data);
                           $scope.alerterror = false;
                           $localStorage.statuspagtotray = results.data.status_name;
                           $scope.statuspagtotray = results.data.status_name;
                           $scope.statuspagtotraydesc = results.data.payment_response;
                           $scope.url_payment = results.data.url_payment;
                           $localStorage.url_payment = results.data.url_payment;
                           //WizardHandler.wizard("pedidoWizard").next();
                           //WizardHandler.wizard("pedidoWizard").finish();
                           // $scope.step = 5;
                           if (results.data.payment_method_id == "6") {
                               $scope.ImprmirBoleto();
                           }

                           $scope.pagamentosucesso = true;
                           $scope.pagamentoefetuado = true;
                           $scope.retornoprocessado = true;
                           //return $scope.pagamentosucesso;
                       } else {
                           $scope.alerterror = true;
                           $scope.msgerros = results.data.validation_errors;
                           $scope.ee = [];
                           $scope.ee = results.data.validation_errors;
                           console.log($scope.ee);
                           angular.forEach($scope.ee, function (err, key) {
                               $scope.msgerros.push(err.message);
                           });

                           // $scope.msgerros = results.data.validation_errors;
                           $scope.pagamentoclicado = false;
                       }
                   }
               );
        }

        return !$scope.alerterrorcard;
    }


    //}

    //$scope.$watch(function () {
    //    return WizardHandler.wizard();
    //}, function (wizard) {
    //    if (wizard) {
    //        console.log("Entrou no wizard");
    //        if ($scope.step > 0) {
    //            wizard.goTo($scope.step);
    //        }

    //    } else {
    //        console.log("nao Entrou no wizard");
    //    }

    //});

    $scope.getTotal = function () {
        var total = 0;
        for (var i = 0; i < $scope.cestalist.length; i++) {
            var cestaobj = $scope.cestalist[i];
            total += (cestaobj.preco * cestaobj.quantfaturada);
        }
        return total;
    }
    $scope.getTotalComissao = function () {
        var total = 0;
        for (var i = 0; i < $scope.cestalist.length; i++) {
            var cestaobj = $scope.cestalist[i];
            total += (cestaobj.totalcomissao);
        }
        //console.log("getTotalComissao" + total);
        return total;
    }

    $scope.getTotalComissao80 = function () {
        var total = 0;
        for (var i = 0; i < $scope.cestalist.length; i++) {
            var cestaobj = $scope.cestalist[i];
            total += (cestaobj.percentualcomissaocalc);
        }
        //console.log("getTotalComissao" + total);
        return total;
    }

    $scope.getTotalOver = function () {
        var total = 0;
        for (var i = 0; i < $scope.cestalist.length; i++) {
            var cestaobj = $scope.cestalist[i];
            total += (cestaobj.over);
        }
        //console.log("getTotalComissao" + total);
        return total;
    }

    $scope.Pagar = function (card) {
        //console.log(card);
        $scope.pagamentoclicado = true;
        //GravaTransacao = function (cdforn, cdcesta, order_number, split, card_name, card_number, card_expdate_month, card_expdate_year, payment_method_id)
        trayFactory.GravaTransacao($localStorage.codforn, $localStorage.codped, $localStorage.codpedPagar, card.split, card.card_name, card.card_number, card.card_expdate_month, card.card_expdate_year, card.card_cvv, card.payment_method_id, $localStorage.cdcontatosel).then(
                    function (results) {
                        // console.log(results.data);
                        if (results.data.sucessResponse) {
                            $scope.alerterror = false;
                            $localStorage.statuspagtotray = results.data.status_name;
                            $scope.statuspagtotray = results.data.status_name;
                            //WizardHandler.wizard("pedidoWizard").next();
                            //WizardHandler.wizard("pedidoWizard").finish();
                            $scope.retornoprocessado = true;
                            $scope.tokentray = results.data.token_transaction;
                            $scope.transactionidtray = results.data.transaction_id;
                            $scope.codpagamento = results.data.codpagamento;
                            if (results.data.status_id == 6) {//pagamento aprovado
                                //pagar
                                trayFactory.FinalizaPagamentoTray($scope.tokentray, $scope.transactionidtray, codpag).then(
                                        function (results) {
                                            console.log(results.data);

                                        }
                                    );
                            }
                        } else {
                            $scope.alerterror = true;
                            $scope.msgerros = results.data.validation_errors;
                        }
                    }
                );


        //if ($localStorage.formapagamentosel == "") {
        //    $scope.alerterror = true;
        //    $scope.msgerror = "Selecione uma forma de pagamento."
        //} else {
        //    $scope.pagamentoclicado = true;
        //    if ($localStorage.formapagamentosel == "paypal") {
        //        paypalFactory.RetornaUrlPagamento($localStorage.codped, $localStorage.codpedPagar, $localStorage.codforn).then(
        //            function (results) {
        //                //console.log(results.Redirect);
        //                console.log(results.data.urlRedirect);
        //                var w = $window.open(results.data.urlRedirect, '', 'width=1200,height=728,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
        //                w.focus();
        //                WizardHandler.wizard().goTo(4);
        //            }
        //        );
        //        //WizardHandler.wizard().next();

        //    } else {
        //        console.log($localStorage.formapagamentosel + "<---");
        //    }
        //}

        //return $localStorage.formapagamentosel != "";
    }



    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    $scope.FinalizaPagamentoPaypal = function () {
        var tokenpay = getParameterByName('token');
        var PayerID = getParameterByName('PayerID');
        var codpag = getParameterByName('codpag');

        console.log("tokenpay-->" + tokenpay);
        console.log("PayerID-->" + PayerID);
        console.log("codpag-->" + codpag);

        paypalFactory.FinalizaPagamento(tokenpay, PayerID, codpag).then(
                    function (results) {

                        console.log(results.data);

                        $scope.dtpedidopay = $localStorage.datapedido;
                        $scope.formapagamentoselr = $localStorage.formapagamentosel;
                        $scope.entregarpara = results.data.shiptoname;
                        $scope.enderecopay = results.data.shiptostreet + " " + results.data.shiptostreet2;
                        $scope.cidadepay = results.data.shiptocity;
                        $scope.estadopay = results.data.shiptostate;
                        $scope.ceppay = results.data.shiptozip;
                        $scope.paypalstatus = results.data.statusakc;
                        $scope.payerid = results.data.payerid;
                        $scope.tokenpaypal = results.data.email;
                        $scope.emailcliente = results.data.email;
                        $scope.retornoprocessado = true;
                    }
                );
    }


    //$scope.PrintNFCE = function () {
    //    var arr = $localStorage.cdnotafiscalsaida.split("|");
    //    var url = "http://" + $window.location.host + "/ReportPDV.aspx?cdnf=" + arr[0];
    //    //$window.open(url);
    //    var w = $window.open(url, '', 'width=600,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
    //    w.focus();
    //    //$window.location.href = url;
    //    //console.log(url);
    //}


}
]).controller('mainCtrl', ['$scope', 'clienteFactory', '$location', '$routeParams', '$window', '$localStorage', 'bannerFactory',
function ($scope, clienteFactory, $location, $routeParams, $window, $localStorage, bannerFactory) {

    $scope.usernamelogado = $localStorage.usernamelogado;
    $scope.userlogado = $localStorage.userlogado;
    $scope.perfil = $localStorage.perfil;
    $localStorage.banners = [];
    $scope.banners = [];


    $scope.myInterval = 5000;
    $scope.noWrapSlides = false;
    $scope.active = 0;
    var slides = $scope.slides = [];
    var currIndex = 0;

    $scope.isUserlogado = function () {
        return $localStorage.userlogado;
    }

    $scope.isAdm = function () {
        return $localStorage.perfil == "PARCEIROADM";
    }

    $scope.load = function () {
        //console.log($localStorage.banners);

        if (!angular.isUndefined($localStorage.banners)) {

            if ($localStorage.banners.length == 0) {
                bannerFactory.GetBannersAtivos().then(
                                         function (results) {
                                             console.log(results.data);
                                             $scope.banners = results.data;
                                             $localStorage.banners = results.data;

                                         }, function (error) {
                                             alert(error.message);
                                         }
                                 );
            } else {
                $scope.banners = $localStorage.banners;
            }

            // console.log($localStorage.banners);
        }

        //console.log("load");
        $scope.usernamelogado = $localStorage.usernamelogado;
        $scope.userlogado = $localStorage.userlogado;
        $scope.perfil = $localStorage.perfil;
        //$window.location.reload(); 
    }
    if ($scope.userlogado == undefined) {
        $scope.load();
        //console.log("user unde");
    }

    $scope.mostramenu = function () {

    }

    $scope.bannersativos = function () {
        if (!angular.isUndefined($localStorage.banners)) {

            $scope.banners = $localStorage.banners;
            console.log("carregou mem");

        } else {
            bannerFactory.GetBannersAtivos().then(
                                         function (results) {

                                             $scope.banners = results.data;
                                             $localStorage.banners = results.data;
                                             console.log(results.data);
                                         }, function (error) {
                                             alert(error.message);
                                         }
                                 );
        }


    }
    /*
    


Brutatec


   
    */
    $scope.myInterval = 12000;
    $scope.slides = [
      {
          link: "https://www.multivisi.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoMULTIVISI.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.brutatec.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoBRUTATEC.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.visutec.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoVISUTEC.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.faciltec.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoFACILTEC.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.lessfrizz.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoLESSFRIZZ.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.sorvetec.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoSORVETEC.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.designplantas.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoDESIGNPLANTAS.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.feelsound.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoFEELSOUND.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.laranjaexpress.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoLARANJAEXPRESS.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.osolarbrasil.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoOSOLARBRASIL.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.pizzatec.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoPIZZATEC.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      },
      {
          link: "https://www.finessiacabamentos.com.br/",
          image: "https://images.tcdn.com.br/196157/themes/143/img/2017/logoMarcas/logoFINESSI.png?ebe5f72df5d37c033f2fef9b2188532c1509563321"
      }
    ];

    // Autenticado($localStorage, $window, $location, $scope);

    //console.log($stateParams.Item);
    $scope.teste = "$stateParams.Item";

    //$scope.usernamelogado = "Rodrigo vaz";

    //$scope.userlogado = true;

    //if ($scope.item.urlnota != "" && $scope.item.urlnota != undefined)
    //    $scope.btnotadis = false;

    //$scope.goBack = function () {
    //    $ionicHistory.goBack();
    //}
    //$scope.openWindow = function (url) {
    //    //$window.open(trustSrc(url), '_blank', 'location=yes');
    //    //var trusturl = $scope.trustSrc(url);
    //    window.open(url, '_system', 'location=yes');

    //};




    $scope.NovoCliente = function () {
        $location.path('/CadastroCliente');
    }
    $scope.GravarCliente = function (cli) {
        $location.path('/CadastroCliente');
    }
    $scope.EditarCliente = function (cdentidade) {
        $location.path('/CadastroCliente/' + cdentidade);
    }
    $scope.logoff = function () {
        $scope.userlogado = false;
        $localStorage.$reset();
        $scope.user = {};
        $scope.errologin = "";
        $scope.alertloginerror = false;
        $scope.userlogado = false;
        $localStorage.userlogado = false;
        $scope.usernamelogado = "";
        $localStorage.usernamelogado = "";
        $localStorage.logado = "N";
        $localStorage.cdusuariooptimus = "";
        $localStorage.cdfuncionario = "";
        $localStorage.perfil = "";
        $localStorage.codforn = "";
        $location.path('/login');
    }
}
]).controller('pesquisaPedidoCtrl', ['$scope', 'cestaFactory', '$location', '$routeParams', 'basicoFactory', '$localStorage', '$window', '$modal', 'colaboradorFactory', 'Utils',
function ($scope, cestaFactory, $location, $routeParams, basicoFactory, $localStorage, $window, $modal, colaboradorFactory, Utils) {
    $localStorage.lststatuspedido = {};
    $scope.lststatuspedido = {};

    $scope.pedidos = [];
    //$localStorage.pedidos = {};
    $scope.pesq = {};
    //$scope.msgerror = "";
    //$scope.alerterror = false;
    //console.log($stateParams.Item);
    var dataatual = new Date();
    $scope.pesq.pdtinicio = moment(dataatual).subtract(1, "days").format("DD/MM/YYYY");
    $scope.pesq.pdtfim = moment(new Date()).format("DD/MM/YYYY");
    //$scope.usernamelogado = "Rodrigo vaz";

    //$scope.userlogado = true;

    //if ($scope.item.urlnota != "" && $scope.item.urlnota != undefined)
    //    $scope.btnotadis = false;

    //$scope.goBack = function () {
    //    $ionicHistory.goBack();
    //}
    //$scope.openWindow = function (url) {
    //    //$window.open(trustSrc(url), '_blank', 'location=yes');
    //    //var trusturl = $scope.trustSrc(url);
    //    window.open(url, '_system', 'location=yes');

    //};

    $scope.ehAdm = function () {
        return $localStorage.perfil == "PARCEIROADM";
    }
    $scope.GetColaboradoresParceiro = function (cdforn) {
        console.log(cdforn.cdparceiro);
        if (cdforn.cdparceiro > 0) {
            colaboradorFactory.GetColaboradorParceiros(cdforn.cdparceiro).then(
            function (resuts) {
                console.log(resuts.data);
                $scope.colaboradores = resuts.data;
            }
            , function (error) {
                alert(error);
            }
            );
        }

    }
    $scope.iniciarParceiro = function () {
        if ($localStorage.perfil == "PARCEIROADM") {
            colaboradorFactory.GetParceiros().then(
                           function (resuts) {

                               $scope.parceiros = resuts.data;
                           }
                           , function (error) {
                               alert(error);
                           }
                       );
        } else {

            colaboradorFactory.GetColaboradorParceiros($localStorage.codforn).then(
            function (resuts) {
                console.log(resuts.data);
                $scope.colaboradores = resuts.data;
            }
            , function (error) {
                alert(error);
            }
        );
        }

    }

    $scope.Carregarstatus = function () {
        // $scope.lststatuspedido = {};

        $scope.iniciarParceiro();

        if ($localStorage.lststatuspedido.length == undefined) {
            basicoFactory.RetornaStatusPedido().then(
            function (results) {
                $scope.lststatuspedido = results.data;
                $localStorage.lststatuspedido = results.data;
                //console.log(results.data);
            }
        );
        } else {
            $scope.lststatuspedido = $localStorage.lststatuspedido;
        }
        //console.log($localStorage.pedidos);
        if (!angular.isUndefined($localStorage.pedidos)) {

            $scope.pedidos = $localStorage.pedidos;
            //console.log("carregou mem");
        }

    }
    $scope.Carregarstatus();
    //RetornaPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pcdforn)
    $scope.PesquisarPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pnome) {
        //console.log("pcodigopedido" + pcodigopedido);
        if (!angular.isUndefined(pcodigopedido) ||
            !angular.isUndefined(pdatainicio) ||
            !angular.isUndefined(pdatafim) ||
            !angular.isUndefined(pcpfcnpj) ||
            !angular.isUndefined(pstatuspedido) ||
            !angular.isUndefined(pnome)) {
            //console.log("err");
            var codpai = 0;
            if ($localStorage.perfil == "PARCEIROADM") {
                codpai = $scope.pesq.cdparceiro;
                console.log("CDPAI adm" + codpai);
            } else {
                codpai = $localStorage.cdpai;
                console.log("CDPAI" + codpai);
            }
            cestaFactory.RetornaPedidos(pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, $localStorage.codforn, pnome, $localStorage.perfil, codpai).then(
              function (results) {
                  //$scope.promocoes = {};
                  $scope.pedidos = [];
                  if (results.data.length == 0) {
                      $scope.alertinfo = true;
                      $scope.msginfo = "Sua pesquisa não retornou nenhum resultado.";
                  } else {
                      $scope.pedidos = results.data;
                      $localStorage.pedidos = results.data;
                      console.log($scope.pedidos);
                      //console.log(results.data);
                      $scope.alerterror = false;
                      $scope.msgerror = "";
                      $scope.alertinfo = false;
                      $scope.msginfo = "";
                  }

              });
        } else {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para iniciar a pesquisa";
        }
    }

    $scope.Imprimir = function (cdp) {
        //console.log(cdp);
        var url = "http://multivisi.optimuserp.com.br" + "/optimus/conferencia/RelatorioOSSeparacao.aspx?ID=" + cdp.cdpedido;

        //if ($scope.OSValue != null) {
        //ValidarUsuario();
        //alert($location.absUrl());
        //alert($localStorage.autenticado);
        //if ($localStorage.autenticado == "S") {
        //baseUri = url + "/conferencia/RelatorioOSSeparacao.aspx?ID=" + $scope.OSValue;
        //baseUri = "/conferencia/RelatorioOSSeparacao.aspx?ID=" + $scope.OSValue;
        $window.open(url, '_blank');
        //}

        //}
    };
    $scope.ImprimirLista = function () {

        peds = "";
        for (var i = 0, len = $scope.oslist.length; i < len; ++i) {
            if ($scope.oslist[i].isSelected)
                peds = peds + $scope.oslist[i].cdpedidotele + '|';
        }

        if (peds != "") {
            baseUri = location.host + "/conferencia/RelatorioOSSeparacao.aspx?ID=" + peds.substring(0, peds.length - 1);
            baseUri = "/conferencia/RelatorioOSSeparacao.aspx?ID=" + peds.substring(0, peds.length - 1);
            $window.open(baseUri, '_blank');
        }
    };

    $scope.NovoCliente = function () {
        $location.path('/CadastroCliente');
    }
    $scope.GravarCliente = function (cli) {
        $location.path('/CadastroCliente');
    }
    $scope.EditarCliente = function (cdentidade) {
        $location.path('/CadastroCliente/' + cdentidade);
    }
    $scope.EditPedido = function (cdpedido) {
        //console.log("cdpedido ->" + cdpedido);
        $location.path('/EditarPedido/' + cdpedido);
    }
    $scope.open = function (cdpedido) {
        $localStorage.cdpedidodetalhe = cdpedido;
        $scope.pedidodetalhe = [];
        cestaFactory.RetornaPedidoDetalhes($localStorage.cdpedidodetalhe).then(
                      function (resultparc) {
                          $localStorage.pedidodetalhe = resultparc.data;
                          $scope.pedidodetalhe = resultparc.data;
                          console.log($localStorage.pedidodetalhe);
                      }, function (error) {
                          alert(error.message);
                      }
                      );
        //console.log("a" + $localStorage.cdpedidodetalhe);
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'detalhespedido.tmpl.html',
            controller: 'pesquisaPedidoCtrl',
            scope: $scope,
            resolve: {
                scope: function () {
                    return $scope;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
    $scope.close = function () {
        //$scope.modalInstance.dismiss();//
        // $scope.modalInstance.close() //also works I think
        $scope.$close();
    };
    $scope.gett = function () {
        return "asdfasdfasdf";
    }

    $scope.getnpedido = function () {
        //console.log("d" + $localStorage.cdpedidodetalhe);
        return $localStorage.cdpedidodetalhe;
    }
    $scope.getTotalComissaoGeral = function () {
        var total = 0;
        //console.log($scope.pedidodetalhe);
        if (!Utils.lengthListaVazio($scope.pedidodetalhe)) {
            for (var i = 0; i < $scope.pedidodetalhe.lstprodutos.length; i++) {
                var cestaobj = $scope.pedidodetalhe.lstprodutos[i];
                total += (cestaobj.totalcomissao);
            }
        }

        //console.log("getTotalComissao" + total);
        return total;
    }
    $scope.getTotalComissao = function () {
        var total = 0;
        //console.log($scope.pedidodetalhe);
        if (!Utils.lengthListaVazio($scope.pedidodetalhe)) {
            for (var i = 0; i < $scope.pedidodetalhe.lstprodutos.length; i++) {
                var cestaobj = $scope.pedidodetalhe.lstprodutos[i];
                total += (cestaobj.comissao);
            }
        }
        //console.log("getTotalComissao" + total);
        return total;
    }
    $scope.getTotalProdutos = function () {
        var total = 0;
        if (!Utils.lengthListaVazio($scope.pedidodetalhe)) {
            var produts = $scope.pedidodetalhe.lstprodutos;
            //console.log(produts);
            for (var i = 0; i < produts.length; i++) {
                var pobj = produts[i];
                total += (pobj.preco);
            }
        }
        // console.log("getTotalProdutos" + total);
        return total;
    }
    $scope.getTotalQtde = function () {
        var total = 0;
        if (!Utils.lengthListaVazio($scope.pedidodetalhe)) {
            var produts = $scope.pedidodetalhe.lstprodutos;
            //console.log(produts);
            for (var i = 0; i < produts.length; i++) {
                var pobj = produts[i];
                total += (pobj.quantidade);
            }
        }
        // console.log("getTotalProdutos" + total);
        return total;
    }
    $scope.getTotalOver = function () {
        var total = 0;
        if (!Utils.lengthListaVazio($scope.pedidodetalhe)) {
            var produts = $scope.pedidodetalhe.lstprodutos;
            //console.log(produts);
            for (var i = 0; i < produts.length; i++) {
                var pobj = produts[i];
                total += (pobj.over);
            }
        }
        // console.log("getTotalProdutos" + total);
        return total;
    }


    $scope.getTotalOverGeral = function () {
        var total = 0;
        if (!Utils.lengthListaVazio($scope.pedidos)) {
            for (var i = 0; i < $scope.pedidos.length; i++) {
                var pobj = $scope.pedidos[i];
                total += (pobj.over);
            }
        }
        // console.log("getTotalProdutos" + total);
        return total;
    }
    $scope.getTotalComissaoProdutoGeral = function () {
        var total = 0;
        if (!Utils.lengthListaVazio($scope.pedidos)) {
            for (var i = 0; i < $scope.pedidos.length; i++) {
                var pobj = $scope.pedidos[i];
                total += (pobj.comissao);
            }
        }
        // console.log("getTotalProdutos" + total);
        return total;
    }
    $scope.getTotalComissaoPeriodoGeral = function () {
        var total = 0;
        if (!Utils.lengthListaVazio($scope.pedidos)) {
            for (var i = 0; i < $scope.pedidos.length; i++) {
                var pobj = $scope.pedidos[i];
                total += (pobj.totalcomissao);
            }
        }
        // console.log("getTotalProdutos" + total);
        return total;
    }
}
]).controller('colaboradorCtrl', ['$scope', 'cestaFactory', '$location', '$routeParams', 'basicoFactory', '$localStorage', 'colaboradorFactory',
function ($scope, cestaFactory, $location, $routeParams, basicoFactory, $localStorage, colaboradorFactory) {

    $scope.lststatuspedido = {};

    $scope.colaborador = {};
    $scope.colaboradores = {};

    $scope.colaborador.nomeuser = "";
    $scope.colaborador.email = "";
    $scope.colaborador.senha = "";
    $scope.colaborador.cdfuncionario = 0;
    $scope.colaboradorsel = 0;

    $scope.msgerros = [];


    if (parseInt($routeParams.cdcolaborador) > 0) {
        colaboradorFactory.GetByID($routeParams.cdcolaborador).then(
                function (resultcli) {
                    console.log("resultcli");
                    console.log(resultcli);
                    $scope.colaboradorsel = resultcli.data.cdfuncionario;
                    $scope.colaborador.cdfuncionario = resultcli.data.cdfuncionario;
                    $scope.colaborador.nomeuser = resultcli.data.nmfuncionario;
                    $scope.colaborador.email = resultcli.data.dsemail;
                    $scope.colaborador.senha = resultcli.data.senha;

                }
                , function (error) {
                    alert(error);
                }
            );
    }

    $scope.PesquisarColoborador = function (cola) {
        var podepesquisar = true;

        if (angular.isUndefined(cola.nomeColaborador) || cola.nomeColaborador === null) {
            $scope.msgerror = "Informe um parametro para a pesquisa";
            $scope.alerterror = true;
            podepesquisar = false;

            if (angular.isUndefined(cola.emailColaborador) || cola.emailColaborador === null) {
                $scope.msgerror = "Informe um parametro para a pesquisa";
                $scope.alerterror = true;
                podepesquisar = false;
            } else {
                $scope.msgerror = "";
                $scope.alerterror = false;
                podepesquisar = true;
            }

        } else {
            $scope.msgerror = "";
            $scope.alerterror = false;
            $scope.alertinfo = false;
            podepesquisar = true;
        }
        if (podepesquisar) {
            colaboradorFactory.RetornaColaborador(cola.nomeColaborador, cola.emailColaborador, $localStorage.codforn).then(
              function (results) {
                  console.log(results.data);
                  $scope.colaboradores = results.data;
                  if ($scope.colaboradores.length == 0) {
                      $scope.msgerror = "";
                      $scope.alerterror = false;
                      $scope.msginfo = "Sua pesquisa não retornou nenhum resultado";
                      $scope.alertinfo = true;

                  } else {
                      $scope.msgerror = "";
                      $scope.msginfo = "";
                      $scope.alerterror = false;
                      $scope.alertinfo = false;
                  }
              }
          );
        }

    }
    $scope.GravarColaborador = function (cola) {
        var podegravar = true;
        console.log(cola);
        $scope.msgerros = [];

        if (angular.isUndefined(cola.nomeuser) || cola.nomeuser === null || cola.nomeuser == "") {
            $scope.msgerros.push("Informe o nome do usuario");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cola.email) || cola.email === null || cola.email == "") {
            $scope.msgerros.push("Informe o email usuario");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (angular.isUndefined(cola.senha) || cola.senha === null || cola.senha == "") {
            $scope.msgerros.push("Informe a senha usuario");
            $scope.alerterror = true;
            podegravar = false;
        }
        if (podegravar) {

            if (cola.cdfuncionario > 0) {
                colaboradorFactory.CadastraColaborador(cola.nomeuser, cola.email, cola.senha, cola.cdfuncionario, $localStorage.codforn).then(
                function (results) {
                    console.log("results.data ->" + results.data);
                    var res = results.data.toString();
                    if (res == "ok") {
                        $scope.msgerror = "";
                        $scope.alerterror = false;
                        $scope.msginfo = "Funcionario salvo com sucesso!";
                        $scope.alertinfo = true;
                        $scope.colaborador.nomeuser = "";
                        $scope.colaborador.email = "";
                        $scope.colaborador.senha = "";
                        $scope.colaborador.cdfuncionario = 0;
                        $scope.colaboradorsel = 0;
                        console.log("fun");
                        //$location.path('/CadastroColaborador/' + cdentidade);
                    } else {

                    }
                    //$scope.colaboradores = results.data;
                }
                );
            } else {
                colaboradorFactory.GetByNome(cola.nomeuser).then(
                   function (resultcli) {

                       console.log(resultcli.data);

                       if (resultcli.data) {
                           $scope.msgerros.push("Ja existe um usuario com o nome " + cola.nomeuser + " cadastrado no sistema.");
                           $scope.alerterror = true;

                       } else {
                           colaboradorFactory.CadastraColaborador(cola.nomeuser, cola.email, cola.senha, cola.cdfuncionario, $localStorage.cdfuncionario).then(
                           function (results) {
                               console.log("results.data ->" + results.data);
                               var res = results.data.toString();
                               if (res == "ok") {
                                   $scope.msgerror = "";
                                   $scope.alerterror = false;
                                   $scope.msginfo = "Funcionario salvo com sucesso!";
                                   $scope.alertinfo = true;
                                   $scope.colaborador.nomeuser = "";
                                   $scope.colaborador.email = "";
                                   $scope.colaborador.senha = "";
                                   $scope.colaborador.cdfuncionario = 0;
                                   $scope.colaboradorsel = 0;
                                   console.log("fun");
                                   //$location.path('/CadastroColaborador/' + cdentidade);
                               } else {

                               }
                               //$scope.colaboradores = results.data;
                           }
                           );
                       }
                   }
                   , function (error) {
                       alert(error);
                   }
               );
            }




        }

    }


    //$scope.Carregarstatus();
    ////RetornaPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pcdforn)
    //$scope.PesquisarPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pnome) {
    //    cestaFactory.RetornaPedidos(pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, $localStorage.codforn, pnome).then(
    //        function (results) {
    //            //$scope.promocoes = {};
    //            $scope.pedidos = {};
    //            $scope.pedidos = results.data;

    //            console.log(results.data);
    //        }
    //    );

    //}


    $scope.ExcluirColaborador = function (cdfuncionario, cola) {
        colaboradorFactory.Excluir(cdfuncionario).then(
                        function (results) {
                            if (results.data) {
                                $scope.msgerror = "";
                                $scope.alerterror = false;
                                $scope.msginfo = "Funcionario excluido com sucesso!";
                                $scope.alertinfo = true;
                                //$location.path('/CadastroColaborador/' + cdentidade);

                                colaboradorFactory.RetornaColaborador(cola.nomeColaborador, cola.emailColaborador, $localStorage.cdfuncionario).then(
                                  function (results) {
                                      console.log(results.data);
                                      $scope.colaboradores = results.data;
                                      if ($scope.colaboradores.length == 0) {
                                          $scope.msgerror = "";
                                          $scope.alerterror = false;
                                          $scope.msginfo = "Sua pesquisa não retornou nenhum resultado";
                                          $scope.alertinfo = true;

                                      } else {

                                      }
                                  }
                              );

                            } else {

                            }
                            //$scope.colaboradores = results.data;
                        }
                        );
    }
    $scope.NovoColaborador = function () {
        $location.path('/CadastroColaborador');
    }

    $scope.EditarColaborador = function (cdentidade) {
        $location.path('/CadastroColaborador/' + cdentidade);
    }

}
]).controller('cadprodutoCtrl', ['$scope', 'cestaFactory', '$location', '$routeParams', 'produtoFactory', '$localStorage', 'produtoAfiliadoFactory', 'Utils', 'WizardHandler', '$filter',
function ($scope, cestaFactory, $location, $routeParams, produtoFactory, $localStorage, produtoAfiliadoFactory, Utils, WizardHandler, $filter) {

    $scope.clientes = [];

    $scope.msgerros = [];
    $localStorage.cdentiparcsel = 0;
    $scope.cdentiparcsel = 0;
    $scope.parcnome = "";
    $scope.produtossel = [];
    $scope.produtos = [];

    $scope.prds = {};
    $scope.prds.cdprodutobusca = "";
    $scope.prds.nomeprodutobusca = "";
    $scope.prdss = {};
    $scope.prdss.cdprodutobusca = "";
    $scope.prdss.nomeprodutobusca = "";
    $scope.prdss.cdprolegadoobusca = "";
    if (parseInt($routeParams.cdentidade) > 0) {
        produtoAfiliadoFactory.GetProdutos($localStorage.codforn).then(
                function (results) {
                    $scope.produtossel = {};
                    $scope.produtossel = results.data;
                    $localStorage.cdentiparcsel = $routeParams.cdentidade;
                    $scope.cdentiparcsel = $routeParams.cdentidade;
                    //$scope.parcnome = cli.nome;

                    WizardHandler.wizard("produtoWizard").goTo(1);
                }
                , function (error) {
                    alert(error);
                }
            );
    }


    $scope.PesquisarClientePed = function (nomeclienteped) {
        var cpfcnpjunmusk = $('.cpfOuCnpj').cleanVal();

        if (Utils.isUndefinedOrNull(nomeclienteped) && Utils.isUndefinedOrNull(cpfcnpjunmusk)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa do cliente."
            // console.log("err");
        } else {
            produtoAfiliadoFactory.RetornaAfiliadoByParam(nomeclienteped, cpfcnpjunmusk).then(
                 function (results) {
                     $scope.clientes = [];
                     //$scope.promocoes = {};
                     console.log(results.data);
                     if (results.data.length == 0) {
                         $scope.alertinfo = true;
                         $scope.alertok = false;
                         $scope.alerterror = false;
                         $scope.msginfo = "Sua pesquisa não retornou nenhum resultado.";
                         //console.log(results.data.length);
                     } else {
                         $scope.alertinfo = false;
                         $scope.alertok = false;
                         $scope.alerterror = false;
                         $scope.clientes = results.data;

                     }
                 }
             );
        }


    }

    $scope.SelecionarParceiro = function (cli) {
        $scope.alertinfo = false;
        $scope.alertok = false;
        $scope.alerterror = false;
        $localStorage.cdentiparcsel = cli.cdentidade;
        $scope.cdentiparcsel = cli.cdentidade;
        $scope.parcnome = cli.nome;

        produtoAfiliadoFactory.GetProdutos($localStorage.cdentiparcsel).then(
                function (results) {
                    $scope.produtossel = [];
                    $scope.produtossel = results.data;
                }
                , function (error) {
                    alert(error);
                }
            );

    }

    $scope.exitValidationcli = function () {
        // console.log("exitv" + $localStorage.cdentidadesel);
        if ($localStorage.cdentiparcsel == 0) {
            $scope.alerterror = true;
            $scope.msgerror = "Selecione um parceiro."
        } else {
            $scope.alerterror = false;
            $scope.msgerror = ""
        }
        return $localStorage.cdentiparcsel > 0;
    };
    $scope.AddProduto = function (pcdproduto) {
        produtoAfiliadoFactory.GravaProduto($localStorage.cdentiparcsel, pcdproduto).then(
        function (results) {
            $scope.produtossel = [];
            $scope.produtossel = results.data;
            //console.log("$scope.nomeprodutobusca" + $scope.prds.nomeprodutobusca);
            $scope.PesquisarProduto($scope.prds.cdprodutobusca, $scope.prds.nomeprodutobusca, $scope.prds.cdprolegadoobusca);
            $scope.alertinfo = false;
            $scope.msginfo = "";
        }
        );
    }
    $scope.AdicionarSelecionados = function () {
        var cont = 0;
        var selecionadosCount = $filter('filter')($scope.produtos, { checado: true }).length;
        console.log("selecionadosCount" + selecionadosCount);
        if (selecionadosCount > 0) {
            $scope.alerterror = false;
            $scope.msgerror = ""
            angular.forEach($scope.produtos, function (prd, key) {
                if (prd.checado) {
                    cont++;
                    produtoAfiliadoFactory.GravaProdutonolist($localStorage.cdentiparcsel, prd.cdproduto).then(
                    function (results) {
                        // console.log(prd.cdproduto);
                    }
                    );
                    console.log("cont" + cont);
                    if (cont == selecionadosCount) {
                        console.log("contd" + cont);
                        produtoAfiliadoFactory.GetProdutos($localStorage.cdentiparcsel).then(
                        function (results) {
                            $scope.produtossel = [];
                            $scope.produtossel = results.data;
                        }
                        , function (error) {
                            alert(error);
                        }
                        );
                        $scope.PesquisarProduto($scope.prds.cdprodutobusca, $scope.prds.nomeprodutobusca, $scope.prds.cdprolegadoobusca);
                        $scope.alertinfo = true;
                        $scope.msginfo = "Produto(s) adicionados com sucesso.";
                    }
                }
            });

        } else {
            $scope.alerterror = true;
            $scope.msgerror = "Selecione um produto!"
        }


    }
    $scope.RemoverSelecionados = function () {
        var cont = 0;
        console.log($scope.produtossel);
        var selecionadosCount = $filter('filter')($scope.produtossel, { checado: true }).length;
        if (selecionadosCount > 0) {
            $scope.alerterror = false;
            $scope.msgerror = ""
            angular.forEach($scope.produtossel, function (prd, key) {
                if (prd.checado) {
                    cont++;
                    produtoAfiliadoFactory.DeleteProdutoParceiro(prd.cdprodparceiro).then(
                    function (results) {
                        console.log(prd.cdprodparceiro);
                    }
                    );
                    if (cont == selecionadosCount) {
                        produtoAfiliadoFactory.GetProdutos($localStorage.cdentiparcsel).then(
                           function (results) {
                               $scope.produtossel = [];
                               $scope.produtossel = results.data;
                               $scope.alertinfo = true;
                               $scope.msginfo = "Produto(s) removidos com sucesso.";
                           }
                           , function (error) {
                               alert(error);
                           }
                       );


                    }
                }
            });

        } else {
            $scope.alerterror = true;
            $scope.msgerror = "Selecione um produto!"
        }
    }
    $scope.RemoverProdutosel = function (cdprodparceiro) {
        produtoAfiliadoFactory.DeleteProdutoParceiro(cdprodparceiro).then(
                  function (results) {
                      produtoAfiliadoFactory.GetProdutos($localStorage.codforn).then(
                         function (results) {
                             $scope.produtossel = [];
                             $scope.produtossel = results.data;
                             $scope.alertinfo = true;
                             $scope.msginfo = "Produto(s) removidos com sucesso.";
                         }
                         , function (error) {
                             alert(error);
                         }
                      );
                  }
                  );

    }


    $scope.PesquisarProduto = function (pcdprodutobusca, pnomeprodutobusca, pcdprodutolegado) {

        if (Utils.isUndefinedOrNull(pcdprodutobusca) && Utils.isUndefinedOrNull(pnomeprodutobusca) && Utils.isUndefinedOrNull(pcdprodutolegado)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa do produto."
            //console.log("err");
        } else {
            produtoFactory.RetornaProdutoByParamNoFilial(pnomeprodutobusca, pcdprodutobusca, $localStorage.cdentiparcsel,pcdprodutolegado).then(
            function (results) {
                $scope.produtos = [];
                console.log("NOVA");
                //$scope.promocoes = {};
                //console.log(results.data);
                if (results.data.length == 0) {
                    $scope.alertinfo = true;
                    $scope.alertok = false;
                    $scope.alerterror = false;
                    $scope.msginfo = "Sua pesquisa não retornou nenhum resultado ou o produto já foi inserido.";
                    //console.log(results.data.length);
                } else {
                    $scope.alertinfo = false;
                    $scope.alertok = false;
                    $scope.alerterror = false;
                    $scope.produtos = results.data;

                }
            }
        );
        }
    }
    $scope.PesquisarProdutoSelecionado = function (pcdprodutobusca, pnomeprodutobusca) {
        console.log(pcdprodutobusca + pnomeprodutobusca);
        if (Utils.isUndefinedOrNull(pcdprodutobusca) && Utils.isUndefinedOrNull(pnomeprodutobusca)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa dos produtos selecionados."
            //console.log("err");
        } else {
            //pcodforn, pnome, pcodigoproduto
            produtoAfiliadoFactory.GetProdutosSel($localStorage.cdentiparcsel, pnomeprodutobusca, pcdprodutobusca).then(
                function (results) {
                    $scope.produtossel = [];
                    $scope.produtossel = results.data;
                }
                , function (error) {
                    alert(error);
                }
            );

        }
    }

}
]).controller('authCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'colaboradorFactory', '$window', '$state', 'Utils', '$rootScope',
function ($scope, $location, $routeParams, $localStorage, colaboradorFactory, $window, $state, Utils, $rootScope) {

    //$rootScope.logado = "N";
    $scope.user = {};
    $scope.errologin = "";
    $scope.alertloginerror = false;
    $scope.userlogado = false;
    //$rootScope.userlogado = false;
    $localStorage.userlogado = false;
    $scope.usernamelogado = "";
    $localStorage.usernamelogado = "";
    $localStorage.logado = "N";
    $localStorage.cdusuariooptimus = "";
    $localStorage.cdfuncionario = "";
    $localStorage.perfil = "PARCEIRO";
    //$rootScope.perfil = "PARCEIRO";
    $scope.perfil = "";
    $localStorage.codforn = "";
    $localStorage.datalogin = new Date();
    $scope.Logar = function (usuario, senha) {

        if (Utils.isUndefinedOrNull(usuario) && Utils.isUndefinedOrNull(senha)) {
            $scope.alertloginerror = true;
            $scope.errologin = "Informe seu usuário e senha."
            // console.log("err");
        } else {
            colaboradorFactory.Logar(usuario, senha).then(
                          function (results) {
                              $localStorage.logado = "N";
                              $rootScope.logado = "N";
                              //$localStorage.logado = "N";
                              $localStorage.usernamelogado = "";
                              //console.log("login");
                              //console.log(results);

                              if (!results.data.Logado) {
                                  $scope.alertloginerror = true;
                                  $scope.errologin = "Usuário ou senha inválidos";
                              }
                              else {
                                  //$scope.usuario = results.data.NmFuncionario;
                                  //alert(results.data.CdFuncionario);
                                  $localStorage.usernamelogado = results.data.nmfuncionario;
                                  $localStorage.logado = "S";
                                  //$rootScope.logado = "S";
                                  $scope.userlogado = true;
                                  // $rootScope.userlogado = true;
                                  $localStorage.userlogado = true;
                                  $localStorage.cdusuariooptimus = results.data.cdusuariooptimus;
                                  $localStorage.cdfuncionario = results.data.cdfuncionario;
                                  $localStorage.perfil = results.data.perfil;
                                  // $rootScope.perfil = results.data.perfil;
                                  $localStorage.codforn = results.data.cdfuncionario;
                                  $localStorage.cdpai = results.data.cdentidadepai;
                                  $localStorage.datalogin = new Date();
                                  //console.log(results.data);
                                  //var url = "http://" + $window.location.host + "/index";
                                  // $log.log(url);
                                  // $window.location.href = url;
                                  //$state.go('index');
                                  $location.path('/index');
                              }

                          }, function (error) {
                              alert(error.message);
                          }
                  );
        }

    }
    /*
    $scope.$watch(function () { return $localStorage.userlogado; }, function (newVal, oldVal) {
        console.log("watch $localStorage.userlogado" + $localStorage.userlogado);
        if (oldVal !== newVal && newVal !== undefined) {
            if (!$localStorage.userlogado) {
                console.log("watch oldVal !$localStorage.userlogado" + $localStorage.userlogado);
                $scope.user = {};
                $scope.errologin = "";
                $scope.alertloginerror = false;
                $scope.userlogado = false;
                $localStorage.userlogado = false;
                $scope.usernamelogado = "";
                $localStorage.usernamelogado = "";
                $localStorage.logado = "N";
                $localStorage.cdusuariooptimus = "";
                $localStorage.cdfuncionario = "";
                $localStorage.perfil = "PARCEIRO";
                $scope.perfil = "";
                $localStorage.codforn = "";
                $localStorage.datalogin = new Date();
            }

        } else {

            console.log("watch oldVal !$localStorage.userlogado" + $localStorage.userlogado);
            $scope.user = {};
            $scope.errologin = "";
            $scope.alertloginerror = false;
            $scope.userlogado = false;
            $localStorage.userlogado = false;
            $scope.usernamelogado = "";
            $localStorage.usernamelogado = "";
            $localStorage.logado = "N";
            $localStorage.cdusuariooptimus = "";
            $localStorage.cdfuncionario = "";
            $localStorage.perfil = "PARCEIRO";
            $scope.perfil = "";
            $localStorage.codforn = "";
            $localStorage.datalogin = new Date();

        }
    })
    */


    //$scope.iniciar = function () {
    //    console.log("iniciar login");
    //    $scope.user = {};
    //    $scope.errologin = "";
    //    $scope.alertloginerror = false;
    //    $scope.userlogado = false;
    //    $localStorage.userlogado = false;
    //    $scope.usernamelogado = "";
    //    $localStorage.usernamelogado = "";
    //    $localStorage.logado = "N";
    //    $localStorage.cdusuariooptimus = "";
    //    $localStorage.cdfuncionario = "";
    //    $localStorage.perfil = "PARCEIRO";
    //    $scope.perfil = "";
    //    $localStorage.codforn = "";
    //    $localStorage.datalogin = new Date();
    //}
    //$scope.PesquisarColoborador = function (cola) {
    //    colaboradorFactory.RetornaColaborador(cola.nomeColaborador, cola.emailColaborador, "2577").then(
    //        function (results) {
    //            console.log(results.data);
    //            $scope.colaboradores = results.data;
    //        }
    //    );
    //}
    //$scope.GravarColaborador = function (cola) {
    //    colaboradorFactory.CadastraColaborador(cola.nomeuser, cola.email, cola.senha, "2577").then(
    //        function (results) {
    //            console.log(results.data);
    //            //$scope.colaboradores = results.data;
    //        }
    //    );
    //}
}
]).controller('bannerCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'colaboradorFactory', '$window', '$state', 'Utils', '$rootScope', 'bannerFactory',
function ($scope, $location, $routeParams, $localStorage, colaboradorFactory, $window, $state, Utils, $rootScope, bannerFactory) {

    $scope.banners = [];
    var dataatual = new Date();
    $scope.msgerros = [];
    $scope.banner = {};
    $scope.banner.nmbanner = "";
    $scope.banner.cdbanner = 0;
    $scope.banner.stativo = 1;
    $scope.banner.dtinicio = moment(dataatual).subtract(1, "days").format("DD/MM/YYYY");
    $scope.banner.dtfim = moment(new Date()).format("DD/MM/YYYY");

    $scope.PesquisarBanner = function (banner) {

        bannerFactory.PesquisaBanner(banner).then(
                             function (results) {
                                 console.log(results.data.length);
                                 if (results.data.length == 0) {
                                     $scope.alertinfo = true;
                                     $scope.msginfo = "Sua pesquisa não retornou nenhum resultado";
                                 } else {
                                     $scope.alertinfo = false;
                                     $scope.msginfo = "";
                                     $scope.banners = results.data;
                                     console.log(results.data);
                                 }

                             }, function (error) {
                                 alert(error.message);
                             }
                     );
    }


    $scope.DeleteBanner = function (banner) {

        bannerFactory.DeleteBanner(banner).then(
                             function (results) {

                                 $scope.alertinfo = true;
                                 $scope.msginfo = "Banner excluido com sucesso!";

                                 $scope.PesquisarBanner($scope.banner);


                             }, function (error) {
                                 alert(error.message);
                             }
                     );
    }


    $scope.EditarBanner = function (banner) {
        $location.path('/EditarBanner/' + banner.cdbanner);
    }
    $scope.CadastrarBanner = function () {
        $location.path('/CadastrarBanner');
    }


    if (parseInt($routeParams.cdbanner) > 0) {

        $scope.banner.cdbanner = $routeParams.cdbanner;
        $scope.cdbanner = $routeParams.cdbanner;
        //console.log($scope.banner);
        bannerFactory.GetBannerById($scope.banner).then(
                       function (results) {
                           console.log(results.data);
                           $scope.banner = results.data;


                       }, function (error) {
                           alert(error.message);
                       }
               );
    }

    $scope.GravarBanner = function (banner) {

        $scope.podegravar = true;
        $scope.msgerros = [];
        console.log(banner);
        if (Utils.isUndefinedOrNull(banner.nmbanner)) {
            $scope.msgerros.push("Informe o nome do banner");
            $scope.alerterror = true;
            $scope.podegravar = false;
        }
        if (Utils.isUndefinedOrNull(banner.txurlimagem)) {
            $scope.msgerros.push("Informe a url da imagem");
            $scope.alerterror = true;
            $scope.podegravar = false;
        }
        if (Utils.isUndefinedOrNull(banner.txurlredirect)) {
            $scope.msgerros.push("Informe o link para redirecionamento  ");
            $scope.alerterror = true;
            $scope.podegravar = false;
        }
        if (Utils.isUndefinedOrNull(banner.dtinicio) || Utils.isUndefinedOrNull(banner.dtfim)) {
            $scope.msgerros.push("Informe a data de inicio e a data fim");
            $scope.alerterror = true;
            $scope.podegravar = false;
        }
        if ($scope.podegravar) {
            bannerFactory.GravaBanner(banner).then(
                        function (results) {
                            $scope.alertinfo = true;
                            $scope.msginfo = "Banner cadastrado com sucesso!";
                        }, function (error) {
                            alert(error.message);
                        }
                );
        }


    }

}])
    .controller('parceiroCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'colaboradorFactory', '$window', '$state', 'Utils', '$rootScope', 'bannerFactory', 'parceiroComissaoFactory',
function ($scope, $location, $routeParams, $localStorage, colaboradorFactory, $window, $state, Utils, $rootScope, bannerFactory, parceiroComissaoFactory) {

    $scope.comissoes = [];
    $scope.parceiros = [];
    $scope.colaboradores = [];
    $scope.pesq = {};
    $scope.pesq.cdentidadepai = 0;
    $scope.msgerros = [];
    var dataatual = new Date();
    $scope.pesq.dtinicio = moment(dataatual).subtract(1, "days").format("DD/MM/YYYY");
    $scope.pesq.dtfim = moment(new Date()).format("DD/MM/YYYY");




    $scope.ehAdm = function () {

        return $localStorage.perfil == "PARCEIROADM";
    }

    $scope.iniciarParceiro = function () {
        if ($localStorage.perfil == "PARCEIROADM") {
            colaboradorFactory.GetParceiros().then(
                           function (resuts) {
                               console.log(resuts.data);
                               $scope.parceiros = resuts.data;
                           }
                           , function (error) {
                               alert(error);
                           }
                       );
        } else {
            //console.log("else->" + $localStorage.codforn);
            colaboradorFactory.GetColaboradorParceiros($localStorage.codforn).then(
            function (resuts) {
                console.log(resuts.data);
                $scope.colaboradores = resuts.data;
            }
            , function (error) {
                alert(error);
            }
        );
        }

    }
    $scope.iniciarParceiro();
    $scope.GetColaboradoresParceiro = function (cola) {

        if ($scope.pesq.cdentidadepai > 0) {
            colaboradorFactory.GetColaboradorParceiros($scope.pesq.cdentidadepai).then(
            function (resuts) {
                console.log(resuts.data);
                $scope.colaboradores = resuts.data;
            }
            , function (error) {
                alert(error);
            }
            );
        }

    }

    $scope.pesquisarcomissao = function () {
        //console.log("$scope.pesq.cdentidadepai" + $scope.pesq.cdentidadepai);
        if ($scope.pesq.cdentidadepai == 0) {
            $scope.pesq.cdentidadepai = $localStorage.cdpai;
        }
        $scope.podegravar = true;

        if (Utils.isUndefinedOrNull($scope.pesq.dtinicio) || Utils.isUndefinedOrNull($scope.pesq.dtfim)) {
            $scope.msgerros.push("Informe a data de inicio e a data fim");
            $scope.alerterror = true;
            $scope.podegravar = false;
        }
        if ($scope.podegravar) {
            parceiroComissaoFactory.PesquisaComissao($scope.pesq).then(
                             function (results) {
                                 console.log(results.data.length);
                                 if (results.data.length == 0) {
                                     $scope.alertinfo = true;
                                     $scope.msginfo = "Sua pesquisa não retornou nenhum resultado";
                                     $scope.comissoes = [];
                                 } else {
                                     $scope.alertinfo = false;
                                     $scope.msginfo = "";
                                     $scope.comissoes = results.data;
                                     console.log(results.data);
                                 }

                             }, function (error) {
                                 alert(error.message);
                             }
                     );
        }

    }


}]).controller('EnviaNFCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'uploadFactory', '$window', '$state', 'Utils', 'colaboradorFactory',
function ($scope, $location, $routeParams, $localStorage, uploadFactory, $window, $state, Utils, colaboradorFactory) {

    $scope.files = [];
    $scope.parceiros = [];
    $scope.arquivo = {};
    $scope.arquivo.txinfo = "";
    $scope.arquivo.Cdfornecedor = $localStorage.cdpai;
    $scope.nfes = [];
    $scope.pesq = {};

    var dataatual = new Date();
    $scope.pesq.dtinicio = moment(dataatual).subtract(1, "days").format("DD/MM/YYYY");
    $scope.pesq.dtfim = moment(new Date()).format("DD/MM/YYYY");
    $scope.msgerros = [];

    $scope.ehAdm = function () {
        return $localStorage.perfil == "PARCEIROADM";
    }


    $scope.uploadFiles = function (files, arquivo) {
        console.log(files);
        console.log(arquivo);
        if (files.length == 0 || Utils.isUndefinedOrNull($scope.arquivo.txinfo)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe o arquivo e a descrição.";
        } else {
            $scope.alerterror = false;
            $scope.msgerror = "";
            arquivo.Cdfornecedor = $localStorage.cdpai;

            console.log($scope.arquivo);

            uploadFactory.EnviaArquivo(files, arquivo, $localStorage.cdpai).then(
                     function (results) {
                         console.log(results.data);

                         $scope.files = [];
                         $scope.arquivo.txinfo = "";
                         if (results.data == "OK") {
                             $scope.alertinfo = true;
                             $scope.msginfo = "Arquivo enviado com sucesso!";
                             $scope.files = [];
                         } else {
                             $scope.alertinfo = false;
                             $scope.msginfo = "";
                             $scope.comissoes = results.data;
                             console.log(results.data);
                         }

                     }, function (error) {
                         alert(error.message);
                     }
             );
        }


    }

    $scope.PesquisaNfe = function (pesq) {

        if (Utils.isUndefinedOrNull($scope.pesq.dtinicio) || Utils.isUndefinedOrNull($scope.pesq.dtfim)) {
            $scope.msgerros.push("Informe a data de inicio e a data fim");
            $scope.alerterror = true;
            $scope.podegravar = false;
        } else {
            $scope.alerterror = false;
            $scope.msgerror = "";

            var cdpai = $localStorage.perfil == "PARCEIROADM" ? pesq.cdparceiro : $localStorage.cdpai;

            uploadFactory.PesquisaNfe(pesq, cdpai).then(
                     function (results) {
                         console.log(results.data);
                         if (results.data.length == 0) {
                             $scope.alertinfo = true;
                             $scope.msginfo = "Sua pesquisa não retornou nenhum resultado";
                             $scope.nfes = [];
                         } else {
                             $scope.alertinfo = false;
                             $scope.msginfo = "";
                             $scope.nfes = results.data;
                             console.log(results.data);
                         }

                     }, function (error) {
                         alert(error.message);
                     }
             );
        }


    }

    $scope.AbrirArquivo = function (txpath) {
        $window.open('https://parceiros.multivisi.com.br' + txpath, '_blank');
        //$window.open('http://localhost:47905/' + txpath, '_blank');
    }

    $scope.iniciarParceiro = function () {
        if ($localStorage.perfil == "PARCEIROADM") {
            colaboradorFactory.GetParceiros().then(
                           function (resuts) {

                               $scope.parceiros = resuts.data;
                           }
                           , function (error) {
                               alert(error);
                           }
                       );
        }

    }
    $scope.iniciarParceiro();

}]).controller('tabelaPrecoCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'uploadFactory', '$window', '$state', 'Utils', 'colaboradorFactory', 'produtoAfiliadoFactory',
function ($scope, $location, $routeParams, $localStorage, uploadFactory, $window, $state, Utils, colaboradorFactory, produtoAfiliadoFactory) {

    $scope.produtossel = [];
    $scope.PesquisarProdutoSelecionado = function (pcdprodutobusca, pnomeprodutobusca) {
        console.log(pcdprodutobusca + pnomeprodutobusca);
        if (Utils.isUndefinedOrNull(pcdprodutobusca) && Utils.isUndefinedOrNull(pnomeprodutobusca)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa dos produtos."
            //console.log("err");
        } else {
            //pcodforn, pnome, pcodigoproduto
            produtoAfiliadoFactory.GetProdutosSel($localStorage.cdpai, pnomeprodutobusca, pcdprodutobusca).then(
                function (results) {
                    $scope.produtossel = [];
                    $scope.produtossel = results.data;
                }
                , function (error) {
                    alert(error);
                }
            );

        }
    }


    $scope.PesquisarTodosProdutos = function () {

        produtoAfiliadoFactory.GetProdutos($localStorage.cdpai).then(
                function (results) {
                    $scope.produtossel = [];
                    $scope.produtossel = results.data;
                    console.log(results.data);
                    //$localStorage.cdentiparcsel = $routeParams.cdentidade;
                    //$scope.cdentiparcsel = $routeParams.cdentidade;
                    //$scope.parcnome = cli.nome;


                }
                , function (error) {
                    alert(error);
                }
            );
    }

    table = function (data, columns) {
        //console.log(data);
        //console.log(columns);
        return {
            style: 'tblcss',
            table: {
                headerRows: 1,
                body: this.buildTableBody(data, columns)
            }
        };
    }

    buildTableBody = function (data, columns) {
        var body = [];

        body.push(columns);

        data.forEach(function (row) {
            var dataRow = [];

            columns.forEach(function (column) {
                dataRow.push(row[column]);
            })

            body.push(dataRow);
        });

        return body;
    }

    generateReport = function (payrolls) {
        var tempArr = [];
        //console.log(payrolls);
        for (var i = 0; i < payrolls.length; i++) {

            tempArr.push(
              {
                  Código: payrolls[i].cdproduto,
                  Nome: payrolls[i].nmproduto,
                  Estoque: payrolls[i].estoque,
                  Preço: payrolls[i].precoview
              }
           );
        }
        console.log(tempArr);
        var dd = {
            content: [
              { text: 'Listagem de produtos', style: 'header' },
              this.table(tempArr, ['Código', 'Nome', 'Estoque', 'Preço'])
            ],
            styles: {
                header: {
                    fontSize: 14,
                    bold: true,
                    margin: [0, 0, 0, 10]
                },
                subheader: {
                    fontSize: 16,
                    bold: true,
                    margin: [0, 10, 0, 5]
                },
                tableExample: {
                    fontSize: 11,
                    margin: [0, 5, 0, 15]
                },
                tableHeader: {
                    bold: true,
                    fontSize: 13,
                    color: 'black'
                }
            },
            defaultStyle: {
                fontSize: 9
            }
        };
        var dataatual = new Date();
        var day = dataatual.getDate();
        var monthIndex = dataatual.getMonth();
        var year = dataatual.getFullYear();

        var nomearquivo = "listagemdeprodutos" + day + monthIndex + year + ".pdf";

        pdfMake.createPdf(dd).download(nomearquivo);



        // pdfMake.createPdf(docDefinition3).download("listadeprodutos.pdf");
    }
    $scope.Imprimir = function () {
        if ($scope.produtossel.length == 0) {

        } else {
            generateReport($scope.produtossel);
        }

    }

}]).controller('gerencialCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'uploadFactory', '$window', '$state', 'Utils', 'colaboradorFactory', 'produtoAfiliadoFactory',
function ($scope, $location, $routeParams, $localStorage, uploadFactory, $window, $state, Utils, colaboradorFactory, produtoAfiliadoFactory) {

    $scope.produtossel = [];
    $scope.PesquisarProdutoSelecionado = function (pcdprodutobusca, pnomeprodutobusca) {
        console.log(pcdprodutobusca + pnomeprodutobusca);
        if (Utils.isUndefinedOrNull(pcdprodutobusca) && Utils.isUndefinedOrNull(pnomeprodutobusca)) {
            $scope.alerterror = true;
            $scope.msgerror = "Informe um parametro para a pesquisa dos produtos."
            //console.log("err");
        } else {
            //pcodforn, pnome, pcodigoproduto
            produtoAfiliadoFactory.GetProdutosSel($localStorage.cdpai, pnomeprodutobusca, pcdprodutobusca).then(
                function (results) {
                    $scope.produtossel = [];
                    $scope.produtossel = results.data;
                }
                , function (error) {
                    alert(error);
                }
            );

        }
    }


    $scope.PesquisarTodosProdutos = function () {

        produtoAfiliadoFactory.GetProdutos($localStorage.cdpai).then(
                function (results) {
                    $scope.produtossel = [];
                    $scope.produtossel = results.data;
                    console.log(results.data);
                    //$localStorage.cdentiparcsel = $routeParams.cdentidade;
                    //$scope.cdentiparcsel = $routeParams.cdentidade;
                    //$scope.parcnome = cli.nome;


                }
                , function (error) {
                    alert(error);
                }
            );
    }

    var pivot_dataset = [
         { Amount: 100, Country: "Canada", Date: "FY 2005", Product: "Bike", Quantity: 2, State: "Alberta" },
         { Amount: 200, Country: "Canada", Date: "FY 2006", Product: "Van", Quantity: 3, State: "British Columbia" },
         { Amount: 300, Country: "Canada", Date: "FY 2007", Product: "Car", Quantity: 4, State: "Brunswick" },
         { Amount: 150, Country: "Canada", Date: "FY 2008", Product: "Bike", Quantity: 3, State: "Manitoba" },
         { Amount: 200, Country: "Canada", Date: "FY 2006", Product: "Car", Quantity: 4, State: "Ontario" },
         { Amount: 100, Country: "Canada", Date: "FY 2007", Product: "Van", Quantity: 1, State: "Quebec" },
         { Amount: 200, Country: "France", Date: "FY 2005", Product: "Bike", Quantity: 2, State: "Charente-Maritime" },
         { Amount: 250, Country: "France", Date: "FY 2006", Product: "Van", Quantity: 4, State: "Essonne" },
         { Amount: 300, Country: "France", Date: "FY 2007", Product: "Car", Quantity: 3, State: "Garonne (Haute)" },
         { Amount: 150, Country: "France", Date: "FY 2008", Product: "Van", Quantity: 2, State: "Gers" },
         { Amount: 200, Country: "Germany", Date: "FY 2006", Product: "Van", Quantity: 3, State: "Bayern" },
         { Amount: 250, Country: "Germany", Date: "FY 2007", Product: "Car", Quantity: 3, State: "Brandenburg" },
         { Amount: 150, Country: "Germany", Date: "FY 2008", Product: "Car", Quantity: 4, State: "Hamburg" },
         { Amount: 200, Country: "Germany", Date: "FY 2008", Product: "Bike", Quantity: 4, State: "Hessen" },
         { Amount: 150, Country: "Germany", Date: "FY 2007", Product: "Van", Quantity: 3, State: "Nordrhein-Westfalen" },
         { Amount: 100, Country: "Germany", Date: "FY 2005", Product: "Bike", Quantity: 2, State: "Saarland" },
         { Amount: 150, Country: "United Kingdom", Date: "FY 2008", Product: "Bike", Quantity: 5, State: "England" },
         { Amount: 250, Country: "United States", Date: "FY 2007", Product: "Car", Quantity: 4, State: "Alabama" },
         { Amount: 200, Country: "United States", Date: "FY 2005", Product: "Van", Quantity: 4, State: "California" },
         { Amount: 100, Country: "United States", Date: "FY 2006", Product: "Bike", Quantity: 2, State: "Colorado" },
         { Amount: 150, Country: "United States", Date: "FY 2008", Product: "Car", Quantity: 3, State: "New Mexico" },
         { Amount: 200, Country: "United States", Date: "FY 2005", Product: "Bike", Quantity: 4, State: "New York" },
         { Amount: 250, Country: "United States", Date: "FY 2008", Product: "Car", Quantity: 3, State: "North Carolina" },
         { Amount: 300, Country: "United States", Date: "FY 2007", Product: "Van", Quantity: 4, State: "South Carolina" }
    ];
    var dataSource = {
        data: pivot_dataset,
        rows: [
             {
                 fieldName: "Country",
                 fieldCaption: "Country"
             },
            {
                fieldName: "State",
                fieldCaption: "State"
            }
        ],
        columns: [
            {
                fieldName: "Product",
                fieldCaption: "Product"
            }
        ],
        values: [
            {
                fieldName: "Amount",
                fieldCaption: "Amount"
            },
            {
                fieldName: "Quantity",
                fieldCaption: "Quantity"
            }
        ]
    };


    $scope.datasource = dataSource;

}]);

