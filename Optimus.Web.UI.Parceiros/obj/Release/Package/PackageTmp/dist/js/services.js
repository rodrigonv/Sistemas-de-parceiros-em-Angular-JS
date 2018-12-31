optimusApp.factory('cepFactory', function ($http, optconfig) {

    var cepFactory = {};

    var _RetornaEnderecoByCep = function (text) {

        console.log(text);

        return $http({
            method: 'GET',
            url: optconfig.url + "cep/" + text,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    cepFactory.RetornaEnderecoByCep = _RetornaEnderecoByCep;

    return cepFactory;

}).factory('clienteFactory', function ($http, optconfig) {

    var clienteFactory = {};

    var _RetornaClienteByParam = function (pnome, pcpfcnpj) {

        //console.log(optconfig.opttoken);


        var objclisearch = {
            nome: pnome,
            cpfcnpj: pcpfcnpj
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "cliente/search",
            data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _RetornaClienteByCdentidade = function (pcdentidade) {

        ////console.log(optconfig.opttoken);


        //var objclisearch = {
        //    nome: pnome,
        //    cpfcnpj: pcpfcnpj
        //};

        return $http({
            method: 'GET',
            url: optconfig.url + "cliente/search/" + pcdentidade,
            //data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaContatosClienteByCdentidade = function (pcdentidade) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cliente/contato/" + pcdentidade,
            //data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _RetornaEnderecoContatoByCdcontato = function (pcdontato) {

        return $http({
            method: 'GET',
            url: optconfig.url + "endereco/contato/" + pcdontato,
            //data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    clienteFactory.RetornaClienteByParam = _RetornaClienteByParam;
    clienteFactory.RetornaClienteByCdentidade = _RetornaClienteByCdentidade;
    clienteFactory.RetornaContatosClienteByCdentidade = _RetornaContatosClienteByCdentidade;
    clienteFactory.RetornaEnderecoContatoByCdcontato = _RetornaEnderecoContatoByCdcontato

    return clienteFactory;

}).factory('basicoFactory', function ($http, optconfig) {

    var basicoFactory = {};

    var _RetornaTipoTelefone = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "basico/tipotelefone",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaTipoEmail = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "basico/tipoemail",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaTipoContato = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "basico/tipocontato",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaTipoEndereco = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "basico/tipoendereco",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaStatusPedido = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "basico/statuspedido",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };


    basicoFactory.RetornaTipoTelefone = _RetornaTipoTelefone;
    basicoFactory.RetornaTipoEmail = _RetornaTipoEmail;
    basicoFactory.RetornaTipoContato = _RetornaTipoContato;
    basicoFactory.RetornaTipoEndereco = _RetornaTipoEndereco;
    basicoFactory.RetornaStatusPedido = _RetornaStatusPedido;
    

    return basicoFactory;

}).factory('produtoFactory', function ($http, optconfig) {

    var produtoFactory = {};

    var _RetornaProdutoByParam = function (pnome, pcodigoproduto) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            nome: pnome,
            codigoproduto: pcodigoproduto
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    produtoFactory.RetornaProdutoByParam = _RetornaProdutoByParam;

    return produtoFactory;

}).factory('cestaFactory', function ($http, optconfig) {
    var cestaFactory = {};

    var _GravaCesta = function (cdfilial, cdpedido, cliente, produto, finaliza, operador, codend, preco, subsquant, pcodforn) {

        var objcesta = {
            codempresa: cdfilial,
            codped: cdpedido,
            cliente: cliente,
            produ: [produto],
            finaliza: finaliza,
            operador: operador,
            codend: codend,
            preco: preco,
            subsquant: subsquant,
            codforn: pcodforn
        };
        console.log("--> objcesta");
        console.log(objcesta);
        //console.log(JSON.stringify(objcesta));
        return $http({
            method: 'POST',
            url: optconfig.url + "cesta",
            data: JSON.stringify(objcesta),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaCesta = function (cdcesta, cdforn) {

        //console.log(text);

        return $http({
            method: 'GET',
            url: optconfig.url + "cesta/" + cdcesta + "/" + cdforn + "/",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };


    var _UpdatePedido = function (cdpedido, vrfrete, txtipofrete, tokenaxado, cdenderecoentregra) {

        var objpedido = {
            cdpedido: cdpedido,
            vrfrete: vrfrete.replace(',', '.'),
            txtipofrete: txtipofrete,
            tokenaxado: tokenaxado,
            cdenderecoentregra: cdenderecoentregra
        };
        console.log("--> objpedido");
        console.log(objpedido);
        //console.log(JSON.stringify(objcesta));
        return $http({
            method: 'POST',
            url: optconfig.url + "cesta/pedido",
            data: JSON.stringify(objpedido),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaParcelamento = function (cdpedidopag, idbandeira) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cesta/split/" + cdpedidopag + "/" + idbandeira,
            //data: JSON.stringify(objcotacao),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaTotalPedido = function (cdpedidopag) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cesta/pedido/total/" + cdpedidopag ,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pcdforn, pnome) {

        var objpesq = {
            codigopedido: pcodigopedido,
            datainicio: pdatainicio,
            datafim: pdatafim,
            cpfcnpj: pcpfcnpj,
            statuspedido: pstatuspedido,
            cdforn: pcdforn,
            nome: pnome
        };
        return $http({
            method: 'POST',
            url: optconfig.url + "cesta/pedido/consulta",
            data: JSON.stringify(objpesq),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };


    cestaFactory.GravaCesta = _GravaCesta;
    cestaFactory.RetornaCesta = _RetornaCesta;
    cestaFactory.UpdatePedido = _UpdatePedido;
    cestaFactory.RetornaParcelamento = _RetornaParcelamento;
    cestaFactory.RetornaTotalPedido = _RetornaTotalPedido;
    cestaFactory.RetornaPedidos = _RetornaPedidos;

    return cestaFactory;

}).factory('Utils', function () {
    var service = {
        isUndefinedOrNull: function (obj) {
            return !angular.isDefined(obj) || obj === null || obj.trim().length === 0;
        }

    }

    return service;
}).factory('axadoFactory', function ($http, optconfig) {
    var axadoFactory = {};

    var _RetornaCotacao = function (idpedido, cepcliente) {

        //var objcotacao = {
        //    CepCliente: cepcliente,
        //    VrTotalPedido: vrtotalpedido,
        //    PesoTotal: pesototal,
        //    TotalItens: totalitens,
        //    IdPedido: idpedido
        //};

        return $http({
            method: 'GET',
            url: optconfig.url + "axado/tabela/" + idpedido + "/" + cepcliente + "/",
            //data: JSON.stringify(objcotacao),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    axadoFactory.RetornaCotacao = _RetornaCotacao;
    return axadoFactory;
}).factory('paypalFactory', function ($http, optconfig) {
    var paypalFactory = {};

    var _RetornaUrlPagamento = function (idpedido, codpedpagar, cdforn) {
        //{cdcesta}/{codpedpagar}/{cdforn}
        return $http({
            method: 'GET',
            url: optconfig.url + "paypal/" + idpedido + "/" + codpedpagar + "/" + cdforn,
            //data: JSON.stringify(objcotacao),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _FinalizaPagamento = function (token, payerid, codpag) {
        //finalize/{token}/{PayerID}
        return $http({
            method: 'GET',
            url: optconfig.url + "paypal/finalize/" + token + "/" + payerid + "/" + codpag,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };


    paypalFactory.RetornaUrlPagamento = _RetornaUrlPagamento;
    paypalFactory.FinalizaPagamento = _FinalizaPagamento;
    return paypalFactory;
}).factory('trayFactory', function ($http, optconfig) {
    var trayFactory = {};

    var _GravaTransacao = function (cdforn, cdcesta, order_number, split, card_name, card_number, card_expdate_month, card_expdate_year, card_cvv, payment_method_id, cdcontato) {

        var objtrans = {
            cdforn: cdforn,
            cdcesta: cdcesta,
            order_number: order_number,
            split: split,
            card_name: card_name,
            card_number: card_number,
            card_expdate_month: card_expdate_month,
            card_expdate_year: card_expdate_year,
            card_cvv: card_cvv,
            payment_method_id: payment_method_id,
            cdcontato: cdcontato
        };
        console.log("--> objpedido");
        console.log(objtrans);
        //console.log(JSON.stringify(objcesta));
        return $http({
            method: 'POST',
            url: optconfig.url + "traycheckout",
            data: JSON.stringify(objtrans),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _FinalizaPagamentoTray = function (token, payerid, codpag) {
        //finalize/{token}/{PayerID}
        return $http({
            method: 'GET',
            url: optconfig.url + "traycheckout/finalize/" + token + "/" + payerid + "/" + codpag,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    trayFactory.GravaTransacao = _GravaTransacao;
    trayFactory.FinalizaPagamentoTray = _FinalizaPagamentoTray;
    
    return trayFactory;
}).factory('pedidoFactory', function ($http, optconfig) {
    var pedidoFactory = {};

    var _RetornaPedidos = function (codigopedido, datainicio, datafim, cpfcnpj, statuspedido, cdforn) {

        var objpedqped = {
            codigopedido: codigopedido,
            datainicio: datainicio,
            datafim: datafim,
            cpfcnpj: cpfcnpj,
            statuspedido: statuspedido,
            cdforn: cdforn
        };
        console.log("--> objpedqped");
        console.log(objpedqped);
        //console.log(JSON.stringify(objcesta));
        return $http({
            method: 'POST',
            url: optconfig.url + "traycheckout",
            data: JSON.stringify(objpedqped),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _FinalizaPagamentoTray = function (token, payerid, codpag) {
        //finalize/{token}/{PayerID}
        return $http({
            method: 'GET',
            url: optconfig.url + "traycheckout/finalize/" + token + "/" + payerid + "/" + codpag,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    pedidoFactory.RetornaPedidos = _RetornaPedidos;

    return pedidoFactory;
}).factory('colaboradorFactory', function ($http, optconfig) {
    var colaboradorFactory = {};

    var _RetornaColaborador = function (pnomeuser,pusuario, pcdforn) {
        var objcola = {
            nomeuser: pnomeuser,
            usuario: pusuario,
            senha: "",
            cdforn: pcdforn
        };
        return $http({
            method: 'POST',
            url: optconfig.url + "/funcionario/pesquisa",
            data: JSON.stringify(objcola),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _CadastraColaborador = function (pnomeuser, pusuario, psenha, pcdforn) {
        var objcola = {
            nomeuser: pnomeuser,
            usuario: pusuario,
            senha: psenha,
            cdforn: pcdforn
        };
        return $http({
            method: 'POST',
            url: optconfig.url + "/funcionario",
            data: JSON.stringify(objcola),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _Logar = function (pusuario, psenha) {
        var objauth = {
            nomeuser: "",
            usuario: pusuario,
            senha: psenha,
            cdforn: ""
        };
        return $http({
            method: 'POST',
            url: optconfig.url + "/funcionario/login",
            data: JSON.stringify(objauth),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    colaboradorFactory.RetornaColaborador = _RetornaColaborador;
    colaboradorFactory.CadastraColaborador = _CadastraColaborador;
    colaboradorFactory.Logar = _Logar;

    return colaboradorFactory;
});
/*
 var _GravaCesta = function (cdfilial, cdpedido, cliente, produto, finaliza, operador, codend, preco, subsquant) {
        return $http.get(serviceBase + 'GravaCesta',
            {
                params:
                  {
                      codempresa: cdfilial,
                      codped: cdpedido,
                      cliente: cliente,
                      produ: produto,
                      finaliza: finaliza,
                      operador: operador,
                      codend: codend,
                      preco: preco,
                      subsquant: subsquant
                  }
            }).then(function (results) {

                return results;
            });
    };
*/