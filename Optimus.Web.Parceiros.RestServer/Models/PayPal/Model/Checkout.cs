using System;
using System.Collections.Generic;
using System.Linq;
using Optimus.Web.Parceiros.Model.PayPal;
using System.Web;
using Optimus.Web.Parceiros.Model.PayPal.Enum;
using Optimus.Web.Parceiros.Model.PayPal.ExpressCheckout.Enum;
using Optimus.Web.Parceiros.Model.PayPal.ExpressCheckout;
using Optimus.Web.Parceiros.Model.PayPal.Nvp;
using Optimus.Web.Parceiros.Model.Optimus.Web.Parceiros;

	
	/// <summary>
	/// Exemplo simples de implementação da integração com a API Express Checkout do PayPal.
	/// </summary>
	public class Checkout {
		private Checkout () {}
		
		/// <summary>
		/// Cria uma instância de ExpressCheckoutApi que será utilizada na integração.
		/// </summary>
		/// <returns>
		/// A instância de ExpressCheckoutApi
		/// </returns>
		private static ExpressCheckoutApi expressCheckout() {

        /*
         
         Username:
vaz.rodrigo_api1.gmail.com
Password:
MG2EPVD4CRTWFM4X
Signature:
AFcWxV21C7fd0v3bYYYRCpSSRl31Ade4qOudnHDZXSouRdM0toywFMuR

         */

        string email = "vaz.rodrigo_api1.gmail.com";
            string password = "MG2EPVD4CRTWFM4X";
            string apiKey = "AFcWxV21C7fd0v3bYYYRCpSSRl31Ade4qOudnHDZXSouRdM0toywFMuR";
			
			//Descomente a linha abaixo após definir suas credenciais da API.
			//throw new Exception("Você precisa definir suas credenciais da API antes de rodar o exemplo");
			
			return PayPalApiFactory.instance.ExpressCheckout(
			    email,
			    password,
			    apiKey
			);
		}
		
		/// <summary>
		/// Executa a operação no Sandbox ou em produção. Esse método existe para facilitar a modificação
		/// entre SandBox e produção, evitando ter que trocar todas as chamadas em pontos distintos do código.
		/// </summary>
		/// <param name='operation'>
		/// A operação que deverá ser executada.
		/// </param>
		private static void execute(ExpressCheckoutApi.Operation operation) {
			operation.sandbox().execute();
		}
		
		/// <summary>
		/// Configura a moeda e idioma da página de pagamento do PayPal.
		/// </summary>
		/// <param name='operation'>
		/// A operação que terá a moeda e idioma configurados.
		/// </param>
		private static void configureLocalization(ExpressCheckoutApi.Operation operation) {
			operation.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;
			operation.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
		}

		/// <summary>
		/// Configura o Express Checkout utilizando a operação SetExpressCheckout
		/// </summary>
		/// <param name='success'>
		/// Define a URL que o cliente será redirecionado pelo PayPal em caso de sucesso.
		/// </param>
		/// <param name='cancel'>
		/// Define a URL que o cliente será redirecionado pelo PayPal caso o ciente cancele.
		/// </param>
		public static string start(string success, string cancel , Cart cart, double frete, string codped) {
			//Cria a instância de SetExpressCheckoutOperation que usaremos para fazer a integração
			SetExpressCheckoutOperation SetExpressCheckout = expressCheckout().SetExpressCheckout(success, cancel);
			PaymentRequest paymentRequest = SetExpressCheckout.PaymentRequest(0);

			//Pega os itens do carrinho do cliente
            Dictionary<Product, int> items = cart.Items;

			//Adiciona todos os itens do carrinho do cliente
			foreach (Product product in items.Keys) {
				//int quantity = Convert.ToInt32(product.Quantidade);
                int quantity = items[product];
				paymentRequest.addItem(product.Name, quantity, product.Price);
			}
            if (frete > 0)
            {
                paymentRequest.ShippingAmount = frete;
            }
            paymentRequest.InvoiceNum = codped;
            
			//Configura moeda e idioma
			configureLocalization(SetExpressCheckout);

			//Executa a operação
			execute(SetExpressCheckout);

			//Retorna a URL de redirecionamento
			return SetExpressCheckout.RedirectUrl;
		}
		
		/// <summary>
		/// Recupera os dados da transação usando a operação GetExpressCheckoutDetails e, então, utiliza a
		/// operação DoExpressCheckout para completar a transação.
		/// </summary>
		/// <param name='token'>
		/// O Token enviado pelo PayPal após o redirecionamento do cliente.
		/// </param>
		/// <param name='PayerID'>
		/// O id do cliente no PayPal, recebido após o redirecionamento do cliente.
		/// </param>
		/// <exception cref='Exception'>
		/// Se a transação falhar, uma exceção é disparada.
		/// </exception>
		public static ResponseNVP finalize(string token, string PayerID) {
			GetExpressCheckoutDetailsOperation GetExpressCheckout = expressCheckout().GetExpressCheckoutDetails(
				token
			);

			execute(GetExpressCheckout); //Executa a operação GetExpressCheckout

			//NVP da resposta do GetExpressCheckout
			ResponseNVP responseNVP = GetExpressCheckout.ResponseNVP;

			if (GetExpressCheckout.ResponseNVP.Ack == Ack.SUCCESS) {
				DoExpressCheckoutPaymentOperation DoExpressCheckout = expressCheckout().DoExpressCheckoutPayment(
					token, PayerID, PaymentAction.SALE
				);

				DoExpressCheckout.PaymentRequest(0).Amount = responseNVP.GetDouble("PAYMENTREQUEST_0_AMT");

				configureLocalization(DoExpressCheckout); //Configura moeda e idioma
				execute(DoExpressCheckout); //Executa a operação DoExpressCheckout

                //if (DoExpressCheckout.ResponseNVP.Ack != Ack.SUCCESS) {
                //    throw new Exception();
                //}
			}

			return responseNVP;
		}
	}


