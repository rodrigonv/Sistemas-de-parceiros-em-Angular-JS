
optimusApp.controller('clienteCtrl', ['$scope', 'clienteFactory', '$location', '$routeParams',
function ($scope, clienteFactory, $location, $routeParams) {

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
        //console.log("CarregaPedidos");

        clienteFactory.RetornaClienteByParam($scope.nomecliente, $scope.cpfcnpjcliente).then(
            function (results) {
                //$scope.promocoes = {};
                $scope.clientes = {};
                $scope.clientes = results.data;

                console.log(results.data);
            }
        );

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
]).controller('clienteCadastroCtrl', ['$scope', 'clienteFactory', 'cepFactory', '$location', '$routeParams', '$localStorage', 'basicoFactory',
function ($scope, clienteFactory, cepFactory, $location, $routeParams, $localStorage, basicoFactory) {

    //console.log($routeParams.cdentidade);
    $scope.tipopessoas = ['Física', 'Jurídica'];

    //$scope.UFS = ['AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'];
    $scope.states = ['AC', 'AL', 'AM', 'AP', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA',
                 'MG', 'MS', 'MT', 'PA', 'PB', 'PE', 'PI', 'PR', 'RJ', 'RN', 'RO', 'RR',
                 'RS', 'SC', 'SE', 'SP', 'TO'];
    //console.log($scope.tipopessoas);
    $scope.cli = {};
    $scope.cont = {};
    $scope.tele = {};
    $scope.email = {};
    $scope.enderecos = {};
    $scope.contatos = {};
    $scope.telefones = {};
    $scope.emails = {};
    $scope.TiposTelefone = {};
    $scope.TiposEmail = {};
    $scope.TiposContato = {};
    $scope.TiposEndereco = {};

    $scope.contatoselecionado = false;
    $scope.cdentidade = $routeParams.cdentidade;
    $localStorage.cdentidade = $routeParams.cdentidade;

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
                    $scope.cli.nome = resultcli.data.nome;
                    $scope.cli.CPFCNPJ = resultcli.data.CPFCNPJ;
                    $scope.cli.IE = resultcli.data.IE;
                    $scope.cli.IM = resultcli.data.IM;
                    $scope.enderecos = resultcli.data.Enderecos;
                    $scope.contatos = resultcli.data.Contatos;
                    console.log(resultcli.data);
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
        console.log(cli);
    }

    $scope.PesquisarCep = function (cep) {
        //alert($scope.cep.replace("-", ""));
        cepFactory.RetornaEnderecoByCep(cep.replace("-", "")).then(
                function (resultcep) {
                    console.log(resultcep.data);
                    $scope.cli.logradouro = resultcep.data.Endereco;
                    $scope.cli.bairro = resultcep.data.Bairro;
                    $scope.cli.Uf = resultcep.data.Uf;
                    $scope.cli.CEP = resultcep.data.Cep;
                    $scope.cli.cidade = resultcep.data.Cidade;
                }
                , function (error) {
                    alert(error);
                    console.log(error);
                }
            );
    }
    $scope.SalvarEndereco = function (cli) {
        $location.path('/CadastroCliente');
    }
    $scope.SalvarContato = function (cont) {
        $location.path('/CadastroCliente');
    }

    $scope.EditarContato = function (contato) {
        $scope.contatoselecionado = true;
        $scope.telefones = {};
        $scope.emails = {};
        //console.log(contato);
        $scope.cont.cdtipocontato = contato.cdtipocontato;
        $scope.cont.stdefault = 1;
        $scope.cont.nome = contato.nome;
        $scope.cont.apelido = contato.apelido;
        $scope.cont.observacao = contato.observacao;

        $scope.telefones = contato.Telefones;
        $scope.emails = contato.Emails;
    }


}
]).controller('pedidoCtrl', ['$scope', 'clienteFactory', '$location', '$routeParams', 'Utils', '$localStorage', 'produtoFactory', 'cestaFactory', 'axadoFactory', '$filter', 'paypalFactory', '$window', 'WizardHandler', 'trayFactory',
function ($scope, clienteFactory, $location, $routeParams, Utils, $localStorage, produtoFactory, cestaFactory, axadoFactory, $filter, paypalFactory, $window, WizardHandler, trayFactory) {

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
    $scope.step = 0;
    $scope.pagamentosucesso = false;
    $scope.alerterrorcard = false;
    $scope.msgerrorcard = "";

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
        $localStorage.cdentidadesel = cli.cdentidade;
        $localStorage.cli = cli;
        $scope.clinome = cli.nome;
    }

    $scope.EditarCliente = function (cdentidade) {
        $location.path('/CadastroCliente/' + cdentidade);
    }
    //###############################
    //Wizard


    $scope.finishedWizard = function () {
        alert("fim");
    };
    $scope.enterValidation = function () {
        return true;
    };

    $scope.exitValidationcli = function () {
        //console.log("exitv");
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

                clienteFactory.RetornaContatosClienteByCdentidade($localStorage.cdentidadesel).then(
                function (resultscont) {

                    if (resultscont.data.length == 0) {
                        $scope.alertinfo = true;
                        $scope.alertok = false;
                        $scope.alerterror = false;
                        $scope.msginfo = "O cliente não possui nenhum contato/endereco cadastrado, efetue o cadastro.";
                    } else {
                        $scope.alertinfo = false;
                        $scope.alertok = false;
                        $scope.alerterror = false;
                        $scope.contatos = resultscont.data;
                        //se possui 1 contato carrega os enderecos
                        if (resultscont.data.length == 1) {

                            $localStorage.cdcontatosel = resultscont.data[0].cdcontato;

                            console.log("cdcont-->" + resultscont.data[0].cdcontato);

                            clienteFactory.RetornaEnderecoContatoByCdcontato(resultscont.data[0].cdcontato).then(
                                function (resultsend) {
                                    $scope.enderecoscontato = resultsend.data;
                                    if (resultsend.data.length == 0) {
                                        $scope.alertinfo = true;
                                        $scope.alertok = false;
                                        $scope.alerterror = false;
                                        $scope.msginfo = "O cliente não possui nenhum endereco cadastrado, efetue o cadastro.";
                                    } else {
                                        $scope.cdenderecosel = resultsend.data[0].cdendereco;
                                        $localStorage.cdenderecosel = resultsend.data[0].cdendereco;
                                        $scope.enderecosel = resultsend.data[0];
                                        $localStorage.enderecosel = resultsend.data[0];
                                        if (resultsend.data.length == 1)//se possui 1 endereco ja busca na achado os tipos de frete
                                        {
                                            axadoFactory.RetornaCotacao($scope.cdpedido, resultsend.data[0].cdcep).then(
                                                function (resultscota) {
                                                    //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                    //console.log(resultscota.data.Cotacoes);

                                                    console.log("cotacao 3-->");
                                                    console.log(resultscota.data);
                                                    $scope.cotacoes = resultscota.data.Cotacoes;
                                                    $localStorage.tokenaxado = resultscota.data.consulta_token;
                                                }
                                            );
                                        }
                                    }
                                }
                                );
                        } else {
                            $scope.umContatoEndereco = false;
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
            produtoFactory.RetornaProdutoByParam(pnomeprodutobusca, pcdprodutobusca).then(
            function (results) {
                $scope.produtos = {};
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
                    $scope.produtos = results.data;

                }
            }
        );
        }
    }

    $scope.cestalist = {};

    $scope.AddCesta = function (item, tipo) {
        var codempresa = $localStorage.CdFilial;
        var codped = $scope.cdpedido == null ? "" : $scope.cdpedido;
        var cliente = $localStorage.cdentidadesel;
        var podeincluir = true;
        console.log("item cesta");
        console.log(item);
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
        var operador = 1;//$localStorage.CdFuncionario;
        var codend = $localStorage.cdenderecosel;
        $localStorage.codped = "";
        $localStorage.codpedPagar = "";
        $localStorage.subtotal = "";

        $localStorage.codforn = "2577";//TODO pegar no login do usuario

        if (podeincluir) {
            cestaFactory.GravaCesta(codempresa, codped, cliente, produ, finaliza, operador, codend, item.preco, 0, $localStorage.codforn).then(
                                function (resultcesta) {
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
        $scope.AddCesta(item);
    }

    $scope.RemoverProdutoCesta = function (item) {
        item.precovenda = 0;
        item.preco = 0;
        item.quantfaturada = 0;
        $scope.AddCesta(item);
    }
    $scope.SelecionarContato = function (cont) {
        $localStorage.cdcontatosel = cont.cdcontato;
        clienteFactory.RetornaEnderecoContatoByCdcontato(cont.cdcontato).then(
                            function (resultsend) {
                                $scope.enderecoscontato = resultsend.data;
                                if (resultsend.data.length == 0) {
                                    $scope.alertinfo = true;
                                    $scope.alertok = false;
                                    $scope.alerterror = false;
                                    $scope.msginfo = "O cliente não possui nenhum endereco cadastrado, efetue o cadastro.";
                                } else {
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
                                                console.log("cotacao 1-->");
                                                console.log(resultscota.data);
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
        axadoFactory.RetornaCotacao($scope.cdpedido, ende.cdcep).then(
                                            function (resultscota) {
                                                //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                //console.log(resultscota.data.Cotacoes);
                                                console.log("cotacao 2-->");
                                                console.log(resultscota.data);
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
        console.log("moda-->");
        console.log(moda);
        //cdpedido, vrfrete, txtipofrete, tokenaxado
        cestaFactory.UpdatePedido($localStorage.codpedPagar, $scope.cotacaoselValor, $localStorage.cotacaosel, $localStorage.tokenaxado, $localStorage.cdenderecosel).then(
                                            function (resultupd) {
                                                //console.log("cotacao-->" + resultscont.data[0].cdcontato);
                                                console.log(resultupd);
                                                //$scope.cotacoes = resultscota.data.Cotacoes;

                                                cestaFactory.RetornaTotalPedido($localStorage.codpedPagar).then(
                                            function (resultparc) {
                                                console.log(resultparc);
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
            console.log(value);
            if (value.parcela == parc) {
                $scope.splitpagamento = value.descricao;
                $localStorage.splitpagamento = $scope.splitpagamento;
            }
        });
    }


    $scope.getValorpagar = function () {
        var total = 0;

        total = parseFloat($localStorage.subtotal).toFixed(2) + parseFloat($localStorage.cotacaoselValor).toFixed(2);
        console.log("total---->" + total);
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

    $scope.SelecioanarForma = function (forma) {
        $localStorage.formapagamentosel = forma;
        $scope.formapagamentosel = forma;
        $scope.pagamentoselecionado = true;
    }
    $scope.exitValidationPagamento = function () {
        console.log($scope.card);

        //$scope.alerterrorcard = true;
        //$scope.msgerrorcard = "";
        $scope.itemserror = [];

        $scope.itemsToAdd = [{
            error: ''
        }];

        $scope.add = function (itemToAdd) {

            var index = $scope.itemsToAdd.indexOf(itemToAdd);

            $scope.itemsToAdd.splice(index, 1);

            $scope.itemserror.push(angular.copy(itemToAdd))
        }

        //$scope.addNew = function () {

        //    $scope.itemsToAdd.push({
        //        firstName: '',
        //        lastName: ''
        //    })
        //}
        console.log("car " + $scope.card.card_number);
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
        //console.log($scope.itemserror);

        if (!$scope.alerterrorcard) {
            trayFactory.GravaTransacao($localStorage.codforn, $localStorage.codped, $localStorage.codpedPagar, $scope.card.split, $scope.card.card_name, $scope.card.card_number, $scope.card.card_expdate_month, $scope.card.card_expdate_year, $scope.card.card_cvv, $scope.card.payment_method_id, $localStorage.cdcontatosel).then(
                   function (results) {
                       console.log(results.data);
                       $scope.pagamentoclicado = true;
                       if (results.data.sucessResponse) {
                           $scope.alerterror = false;
                           $localStorage.statuspagtotray = results.data.status_name;
                           $scope.statuspagtotray = results.data.status_name;
                           //WizardHandler.wizard("pedidoWizard").next();
                           //WizardHandler.wizard("pedidoWizard").finish();
                           // $scope.step = 5;
                           $scope.pagamentosucesso = true;
                           $scope.pagamentoefetuado = true;
                           $scope.retornoprocessado = true;
                           //return $scope.pagamentosucesso;
                       } else {
                           $scope.alerterror = true;
                           $scope.msgerros = results.data.validation_errors;
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
            total += (cestaobj.percentualcomissaocalc);
        }
        return total;
    }

    $scope.Pagar = function (card) {
        console.log(card);
        $scope.pagamentoclicado = true;
        //GravaTransacao = function (cdforn, cdcesta, order_number, split, card_name, card_number, card_expdate_month, card_expdate_year, payment_method_id)
        trayFactory.GravaTransacao($localStorage.codforn, $localStorage.codped, $localStorage.codpedPagar, card.split, card.card_name, card.card_number, card.card_expdate_month, card.card_expdate_year, card.card_cvv, card.payment_method_id, $localStorage.cdcontatosel).then(
                    function (results) {
                        console.log(results.data);
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
]).controller('mainCtrl', ['$scope', 'clienteFactory', '$location', '$routeParams', '$window', '$localStorage',
function ($scope, clienteFactory, $location, $routeParams, $window, $localStorage) {

    //$scope.usernamelogado = $localStorage.usernamelogado;
    //$scope.userlogado = $localStorage.userlogado;
    //$scope.perfil = $localStorage.perfil;

    $scope.load = function () {
        console.log("load");
        $scope.usernamelogado = $localStorage.usernamelogado;
        $scope.userlogado = $localStorage.userlogado;
        $scope.perfil = $localStorage.perfil;
        //$window.location.reload(); 
    }
    if ($scope.userlogado == undefined) {
        $scope.load();
        console.log("user unde");
    }

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

    $scope.PesquisarCliente = function () {
        //console.log("CarregaPedidos");

        clienteFactory.RetornaClienteByParam($scope.nomecliente, $scope.cpfcnpjcliente).then(
            function (results) {
                //$scope.promocoes = {};
                $scope.clientes = {};
                $scope.clientes = results.data;

                console.log(results.data);
            }
        );

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
]).controller('pesquisaPedidoCtrl', ['$scope', 'cestaFactory', '$location', '$routeParams', 'basicoFactory', '$localStorage',
function ($scope, cestaFactory, $location, $routeParams, basicoFactory, $localStorage) {
    $localStorage.lststatuspedido = {};
    $scope.lststatuspedido = {};

    $scope.pesq = {};

    //console.log($stateParams.Item);

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

    $scope.Carregarstatus = function () {
        // $scope.lststatuspedido = {};
        if ($localStorage.lststatuspedido.length == undefined) {
            basicoFactory.RetornaStatusPedido().then(
            function (results) {
                $scope.lststatuspedido = results.data;
                $localStorage.lststatuspedido = results.data;
                console.log(results.data);
            }
        );
        } else {
            $scope.lststatuspedido = $localStorage.lststatuspedido;
        }
    }
    $scope.Carregarstatus();
    //RetornaPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pcdforn)
    $scope.PesquisarPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pnome) {
        cestaFactory.RetornaPedidos(pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, $localStorage.codforn, pnome).then(
            function (results) {
                //$scope.promocoes = {};
                $scope.pedidos = {};
                $scope.pedidos = results.data;

                console.log(results.data);
            }
        );

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
]).controller('colaboradorCtrl', ['$scope', 'cestaFactory', '$location', '$routeParams', 'basicoFactory', '$localStorage', 'colaboradorFactory',
function ($scope, cestaFactory, $location, $routeParams, basicoFactory, $localStorage, colaboradorFactory) {

    $scope.lststatuspedido = {};

    $scope.colaborador = {};
    $scope.colaboradores = {};

    $scope.PesquisarColoborador = function (cola) {
        colaboradorFactory.RetornaColaborador(cola.nomeColaborador, cola.emailColaborador, "2577").then(
            function (results) {
                console.log(results.data);
                $scope.colaboradores = results.data;
            }
        );
    }
    $scope.GravarColaborador = function (cola) {
        colaboradorFactory.CadastraColaborador(cola.nomeuser, cola.email, cola.senha, "2577").then(
            function (results) {
                console.log(results.data);
                //$scope.colaboradores = results.data;
            }
        );
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
    $scope.NovoColaborador = function () {
        $location.path('/CadastroColaborador');
    }

    $scope.EditarColaborador = function (cdentidade) {
        $location.path('/CadastroColaborador/' + cdentidade);
    }

}
]).controller('authCtrl', ['$scope', '$location', '$routeParams', '$localStorage', 'colaboradorFactory', '$window','$state',
function ($scope, $location, $routeParams, $localStorage, colaboradorFactory, $window, $state) {

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
    $scope.Logar = function (usuario, senha) {
        colaboradorFactory.Logar(usuario, senha).then(
                      function (results) {
                          $localStorage.logado = "N";
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
                              $scope.userlogado = true;
                              $localStorage.userlogado = true;
                              $localStorage.cdusuariooptimus = results.data.cdusuariooptimus;
                              $localStorage.cdfuncionario = results.data.cdfuncionario;
                              $localStorage.perfil = results.data.perfil;
                              $localStorage.codforn = results.data.cdentidadepai;

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
])
function Autenticado($localStorage, $window, $location,$state) {
    console.log("Autenticado");
    if ($localStorage.logado == "N") {
        //$localStorage.userlogado = true;
        $localStorage.$reset();
        /*console.log("Autenticado " + $localStorage.logado);
        $localStorage.$reset();
        $localStorage.CdFilial = 0;
        $localStorage.usuariologado = "";
        $localStorage.logado = "N";
        $localStorage.Filiais = [];
        $localStorage.InterfacesAcesso = [];
        $localStorage.CdFuncionario = 0;
        $localStorage.MsgLogin = "Usuário não autenticado.";*/
        //var url = "http://" + $window.location.host + "/login";
        //$window.location.href = url;
        //console.log(url);
        $location.path('/login');
    }
}
;

