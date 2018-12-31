<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaypalReturn.aspx.cs" Inherits="Optimus.Web.UI.Parceiros.PaypalReturn" %>

<!DOCTYPE html>
<!--
This is a starter template page. Use this page to start your new project from
scratch. This page gets rid of all links and provides the needed markup only.
-->
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Multivisi - Parceiros</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="dist/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="dist/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <link href="plugins/iCheck/all.css" rel="stylesheet" />

    <link href="plugins/angular/angular-wizard.min.css" rel="stylesheet" />
    <!-- AdminLTE Skins. We have chosen the skin-blue for this starter
          page. However, you can choose any other skin. Make sure you
          apply the skin class to the body tag so the changes take effect. -->
    <link href="plugins/angular/loading-bar.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="dist/css/skins/skin-blue.min.css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <!-- Google Font -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
</head>
<!--
BODY TAG OPTIONS:
=================
Apply one or more of the following classes to get the
desired effect
|---------------------------------------------------------|
| SKINS         | skin-blue                               |
|               | skin-black                              |
|               | skin-purple                             |
|               | skin-yellow                             |
|               | skin-red                                |
|               | skin-green                              |
|---------------------------------------------------------|
|LAYOUT OPTIONS | fixed                                   |
|               | layout-boxed                            |
|               | layout-top-nav                          |
|               | sidebar-collapse                        |
|               | sidebar-mini                            |
|---------------------------------------------------------|
-->
<body class="hold-transition login-page">
    <div class="col-md-12" ng-app="optimusPartner" ng-controller="pedidoCtrl" ng-init="FinalizaPagamentoPaypal()">
        <div class="col-lg-11" ng-show="!retornoprocessado">
            <label>Processando as informações.....</label>
        </div>
        <section class="invoice" ng-show="retornoprocessado">
            <!-- title row -->
            <div class="row">
                <div class="col-xs-12">
                    <h2 class="page-header">Recibo da transação
            <small class="pull-right">{{dtpedidopay}}
            </small>
                    </h2>
                </div>
                <!-- /.col -->
            </div>
            <!-- info row -->
            <div class="row invoice-info">
                <div class="col-sm-4 invoice-col">
                    De
                  <address>
                      <strong>{{entregarpara}}</strong><br>
                      {{enderecopay}}<br>
                      {{cidadepay}}-{{estadopay}}<br>
                      Cep: {{ceppay}}<br>
                  </address>
                </div>
                <!-- /.col -->
                <div class="col-sm-4 invoice-col">
                    Para
                     <address>
                         <strong>Multivisi Comércio e Importação Eireli</strong><br>
                         Av. Floriano Peixoto, 1713, Loja 41, NSª Aparecida<br>
                         Uberlândia-MG - 38400-700<br>
                         CNPJ 10.409.455/0001-19<br>
                         Inscrição Estadual: 001094892.00-68<br>
                     </address>
                </div>
                <!-- /.col -->
                <div class="col-sm-4 invoice-col">
                    <b>Data do pedido:</b>{{dtpedidopay}}<br>
                    <b>Email:</b>{{emailcliente}}<br>
                    <b>Forma de pagamento:</b> {{formapagamentoselr}}<br>
                    <b>ID do cliente no PayPal:</b> {{payerid}}<br>
                    <b>Token transação:</b> {{tokenpaypal}}<br>
                    <b>Status paypal:</b> {{paypalstatus}}<br>
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

            <!-- Table row -->
            <div class="row">
                <div class="col-xs-12 table-responsive">
                    <div class="table-responsive">
                        <table class="table">
                            <thead>
                                <tr role="row">
                                    <th>Produto</th>
                                    <th>Qtd.</th>
                                    <th>Preço venda</th>
                                    <th>Subtotal</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="cesta in cestalist" role="row">
                                    <td>{{cesta.nomeprod}}</td>
                                    <td class="col-sm-1">{{cesta.quantfaturada}}</td>
                                    <td><span class="text-green Bold">{{cesta.preco | currency}}</span></td>
                                    <td><span class="text-green Bold">{{cesta.valortotalview}}</span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!-- /.col -->
            </div>

        </section>
    </div>
    <!-- ./wrapper -->
    <!-- REQUIRED JS SCRIPTS -->
    <!-- jQuery 3.1.1 -->
    <script src="plugins/jQuery/jquery-3.1.1.min.js"></script>
    <script src="plugins/jQuery/jquery.mask.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <script src="plugins/angular/angular.js"></script>
    <script src="plugins/angular/angular-route.js"></script>
    <script src="plugins/angular/angular-ui-router.js"></script>
    <script src="plugins/angular/ngStorage.min.js"></script>
    <script src="dist/js/app.js"></script>
    <script src="dist/js/services.js"></script>
    <script src="dist/js/controllers.js"></script>
    <script src="dist/js/routes.js"></script>
    <script src="dist/js/directive.js"></script>
    <script src="plugins/angular/angular-wizard.min.js"></script>
    <script src="plugins/fastclick/fastclick.js"></script>
    <script src="plugins/sparkline/jquery.sparkline.min.js"></script>
    <script src="plugins/slimScroll/jquery.slimscroll.min.js"></script>
    <script src="plugins/angular/loading-bar.min.js"></script>
    <script src="plugins/angular/angular-locale_pt-br.js"></script>
    <script src="plugins/angular/angular_masks.js"></script>

    <script src="dist/js/demo.js"></script>
    <script src="dist/js/dirPagination.js"></script>
    <script src="plugins/iCheck/icheck.min.js"></script>
    <!-- Optionally, you can add Slimscroll and FastClick plugins.
         Both of these plugins are recommended to enhance the
         user experience. -->
</body>
</html>
