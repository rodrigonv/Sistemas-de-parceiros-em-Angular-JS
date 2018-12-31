optimusApp.factory('cepFactory', function ($http, optconfig) {

    var cepFactory = {};

    var _RetornaEnderecoByCep = function (text) {

        //console.log(text);

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
    var _RetornaEnderecoContatoByCdentidade = function (pcdentidade) {

        return $http({
            method: 'GET',
            url: optconfig.url + "endereco/get/" + pcdentidade,
            //data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaContatosSeTemTelefone = function (pcdentidade) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cliente/contato/validartelefone/" + pcdentidade,
            //data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GravaCliente = function (cliente) {

        return $http({
            method: 'POST',
            url: optconfig.url + "cliente/insert",
            data: JSON.stringify(cliente),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _verificarcpfcnpj = function (pcpfcnpj) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cliente/verificarcpfcnpj/" + pcpfcnpj,
            //data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };




    clienteFactory.RetornaClienteByParam = _RetornaClienteByParam;
    clienteFactory.RetornaClienteByCdentidade = _RetornaClienteByCdentidade;
    clienteFactory.RetornaContatosClienteByCdentidade = _RetornaContatosClienteByCdentidade;
    clienteFactory.RetornaEnderecoContatoByCdcontato = _RetornaEnderecoContatoByCdcontato;
    clienteFactory.RetornaContatosSeTemTelefone = _RetornaContatosSeTemTelefone;
    clienteFactory.RetornaEnderecoContatoByCdentidade = _RetornaEnderecoContatoByCdentidade;
    clienteFactory.Verificarcpfcnpj = _verificarcpfcnpj;

    clienteFactory.GravaCliente = _GravaCliente;

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

    var _RetornaProdutoByParam = function (pnome, pcodigoproduto, pcodforn) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            nome: pnome,
            codigoproduto: pcodigoproduto,
            codforn: pcodforn
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
    var _RetornaProdutoByParamNoFilial = function (pnome, pcodigoproduto, pcodforn, pcodigolegado) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            nome: pnome,
            codigoproduto: pcodigoproduto,
            codforn: pcodforn,
            codigolegado: pcodigolegado
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/searchnofilial",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    

    var _RetornaProdutoByParamParceiro = function (pcodforn) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            codforn: pcodforn

        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/search",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    produtoFactory.RetornaProdutoByParam = _RetornaProdutoByParam;
    produtoFactory.RetornaProdutoByParamParceiro = _RetornaProdutoByParamParceiro;
    produtoFactory.RetornaProdutoByParamNoFilial = _RetornaProdutoByParamNoFilial;
    
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
            url: optconfig.url + "cesta/pedido/total/" + cdpedidopag,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _RetornaPedidos = function (pcodigopedido, pdatainicio, pdatafim, pcpfcnpj, pstatuspedido, pcdforn, pnome, pperfil, pcdpai) {

        var objpesq = {
            codigopedido: pcodigopedido,
            datainicio: pdatainicio,
            datafim: pdatafim,
            cpfcnpj: pcpfcnpj,
            statuspedido: pstatuspedido,
            cdforn: pcdforn,
            nome: pnome,
            perfil: pperfil,
            cdpai: pcdpai
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

    var _RetornaPedidoDetalhes = function (cdpedido) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cesta/pedido/detalhe/" + cdpedido,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _Criarcesta = function (cdpedido) {

        return $http({
            method: 'GET',
            url: optconfig.url + "cesta/criarcesta/" + cdpedido,
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
    cestaFactory.RetornaPedidoDetalhes = _RetornaPedidoDetalhes;
    cestaFactory.Criarcesta = _Criarcesta;

    return cestaFactory;

}).factory('Utils', function () {
    var service = {
        isUndefinedOrNull: function (obj) {
            return !angular.isDefined(obj) || obj === null || obj.trim().length === 0;
        },
        lengthListaVazio: function (obj) {
            return obj.length == 0;
            console.log(obj);
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
        //console.log("--> objpedqped");
        //console.log(objpedqped);
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

    var _RetornaColaborador = function (pnomeuser, pusuario, pcdforn) {
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
    var _CadastraColaborador = function (pnomeuser, pusuario, psenha, pcdfuncionario, pcdforn) {
        var objcola = {
            cdfuncionario: pcdfuncionario,
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
        console.log(optconfig.url);
        return $http({
            method: 'POST',
            url: optconfig.url + "/funcionario/login",
            data: JSON.stringify(objauth),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _GetByID = function (pcdcolaborador) {

        return $http({
            method: 'GET',
            url: optconfig.url + "/funcionario/pesquisa/" + pcdcolaborador,
            //data: JSON.stringify(objauth),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _GetByNome = function (pnome) {

        return $http({
            method: 'GET',
            url: optconfig.url + "/funcionario/pesquisanome/" + pnome,
            //data: JSON.stringify(objauth),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _Excluir = function (pcdfuncionario) {

        return $http({
            method: 'GET',
            url: optconfig.url + "/funcionario/excluir/" + pcdfuncionario,
            //data: JSON.stringify(objauth),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GetParceiros = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "/funcionario/parceiros",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GetColaboradorParceiros = function (cdforn) {

        return $http({
            method: 'GET',
            url: optconfig.url + "/funcionario/colaboradorparceiro/" + cdforn,
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    colaboradorFactory.RetornaColaborador = _RetornaColaborador;
    colaboradorFactory.CadastraColaborador = _CadastraColaborador;
    colaboradorFactory.Logar = _Logar;
    colaboradorFactory.GetByID = _GetByID;
    colaboradorFactory.GetByNome = _GetByNome;
    colaboradorFactory.Excluir = _Excluir;
    colaboradorFactory.GetParceiros = _GetParceiros;
    colaboradorFactory.GetColaboradorParceiros = _GetColaboradorParceiros;


    return colaboradorFactory;
}).factory('produtoAfiliadoFactory', function ($http, optconfig) {
    var produtoAfiliadoFactory = {};

    var _RetornaAfiliadoByParam = function (pnome, pcpfcnpj) {

        //console.log(optconfig.opttoken);


        var objclisearch = {
            nome: pnome,
            cpfcnpj: pcpfcnpj
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "parceiro/search",
            data: JSON.stringify(objclisearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GravaProduto = function (pcodforn, pcdproduto) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            codforn: pcodforn,
            cdproduto: pcdproduto
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/insert",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GravaProdutonolist = function (pcodforn, pcdproduto) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            codforn: pcodforn,
            cdproduto: pcdproduto
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/insertnolist",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GetProdutos = function (pcodforn) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            codforn: pcodforn
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/search",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _GetProdutosSel = function (pcodforn, pnome, pcodigoproduto) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            codforn: pcodforn,
            nome: pnome,
            codigoproduto: pcodigoproduto
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/searchprodsel",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _DeleteProdutoParceiro = function (pcdprodparceiro) {

        //console.log(optconfig.opttoken);


        var objprodsearch = {
            cdprodparceiro: pcdprodparceiro
        };

        return $http({
            method: 'POST',
            url: optconfig.url + "produto/delete",
            data: JSON.stringify(objprodsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    produtoAfiliadoFactory.RetornaAfiliadoByParam = _RetornaAfiliadoByParam;
    produtoAfiliadoFactory.GravaProduto = _GravaProduto;
    produtoAfiliadoFactory.GetProdutos = _GetProdutos;
    produtoAfiliadoFactory.GetProdutosSel = _GetProdutosSel;
    produtoAfiliadoFactory.GravaProdutonolist = _GravaProdutonolist;
    produtoAfiliadoFactory.DeleteProdutoParceiro = _DeleteProdutoParceiro;



    return produtoAfiliadoFactory;
}).factory('bannerFactory', function ($http, optconfig) {
    var bannerFactory = {};

    var _GravaBanner = function (banner) {

        return $http({
            method: 'POST',
            url: optconfig.url + "banner/insert",
            data: JSON.stringify(banner),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _DeleteBanner = function (banner) {

        return $http({
            method: 'POST',
            url: optconfig.url + "banner/delete",
            data: JSON.stringify(banner),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };
    var _PesquisaBanner = function (banner) {

        return $http({
            method: 'POST',
            url: optconfig.url + "banner/pesquisa",
            data: JSON.stringify(banner),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _GetBannerById = function (banner) {

        return $http({
            method: 'POST',
            url: optconfig.url + "banner/byid",
            data: JSON.stringify(banner),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _GetBannersAtivos = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "banner/ativos",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    bannerFactory.GravaBanner = _GravaBanner;
    bannerFactory.DeleteBanner = _DeleteBanner;
    bannerFactory.PesquisaBanner = _PesquisaBanner;
    bannerFactory.GetBannerById = _GetBannerById;
    bannerFactory.GetBannersAtivos = _GetBannersAtivos;


    return bannerFactory;
}).factory('parceiroComissaoFactory', function ($http, optconfig) {
    var parceiroComissaoFactory = {};


    var _PesquisaComissao = function (parsearch) {

        return $http({
            method: 'POST',
            url: optconfig.url + "parceiro/comissaoagg",
            data: JSON.stringify(parsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    var _GetBannersAtivos = function () {

        return $http({
            method: 'GET',
            url: optconfig.url + "banner/ativos",
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    parceiroComissaoFactory.PesquisaComissao = _PesquisaComissao;



    return parceiroComissaoFactory;
}).factory('uploadFactory', function ($http, optconfig) {
    var uploadFactory = {};


    var _EnviaArquivo = function (file, arquivo, cdforn) {
        //console.log(arquivo);
        return $http({
            method: 'POST',
            url: optconfig.url + "upload/send",
            headers: { 'opttoken': optconfig.opttoken, 'Content-Type': undefined },
            transformRequest: function (data) {
                var formData = new FormData();
                console.log("arquivo");
                console.log(JSON.stringify(arquivo));
                formData.append("model", JSON.stringify(arquivo));
                formData.append("cdforn", cdforn);
                //for (var i = 0; i < data.files.length; i++) {
                formData.append("file", data.file);
                //}
                return formData;
            },
            //data: { file: files }
            data: { model: arquivo, file: file, cdforn: cdforn }

        }).then(function (results) {
            return results;
        });
    };

    var _PesquisaNfe = function (parsearch, codpai) {

        return $http({
            method: 'POST',
            url: optconfig.url + "upload/consulta/" + codpai,
            data: JSON.stringify(parsearch),
            headers: { 'opttoken': optconfig.opttoken },
        }).then(function (results) {
            return results;
        });
    };

    uploadFactory.EnviaArquivo = _EnviaArquivo;
    uploadFactory.PesquisaNfe = _PesquisaNfe;



    return uploadFactory;
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