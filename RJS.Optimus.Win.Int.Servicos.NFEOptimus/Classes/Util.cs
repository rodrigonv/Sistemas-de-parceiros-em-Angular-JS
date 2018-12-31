using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Net;
using RJS.Optimus.Biblioteca;
using System.Security.Cryptography.X509Certificates;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class Util
    {
        public static MemoryStream StringXmlToStreamUTF8(string strXml)
        {
            byte[] byteArray = new byte[strXml.Length];
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public static MemoryStream StringXmlToStream(string strXml)
        {
            byte[] byteArray = new byte[strXml.Length];
            Encoding encoding = Encoding.GetEncoding("iso8859-1");
            byteArray = encoding.GetBytes(strXml);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        public static void SaveMemoryStreamToFile(string filename, MemoryStream ms)
        {
            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write);
            ms.WriteTo(file);
            file.Close();
            ms.Close();

        }

        public class DadosRetSit
        {
            public string tpAmb { get; set; }
            public string verAplic { get; set; }
            public string cStat { get; set; }
            public string xMotivo { get; set; }
            public string cUF { get; set; }
            public string chNFe { get; set; }

        }
        public class DadosRecClass
        {
            /// <summary>
            /// Recibo do lote de notas fiscais enviado
            /// </summary>
            public string nRec { get; set; }
            /// <summary>
            /// Status do Lote
            /// </summary>
            public string cStat { get; set; }

            public string dhRecbto { get; set; }

            public string nProt { get; set; }

            public string xMotivo { get; set; }

            public string chNFe { get; set; }

            public string xmlCompleto { get; set; }

            public string xmlAutorizado { get; set; }

            public string CaminhoArquivo { get; set; }

            public string NomeArquivo { get; set; }

        }

        public static DadosRecClass Recibo(string strXml, string caminhoArquivo, string NomeArquivo)
        {
            MemoryStream memoryStream = StringXmlToStream(strXml);
            DadosRecClass oDadosRec = new DadosRecClass();

            oDadosRec.cStat = string.Empty;
            oDadosRec.nRec = string.Empty;
            oDadosRec.dhRecbto = string.Empty;
            oDadosRec.nProt = string.Empty;
            oDadosRec.xMotivo = string.Empty;
            oDadosRec.chNFe = string.Empty;
            oDadosRec.xmlAutorizado = strXml;
            oDadosRec.CaminhoArquivo = caminhoArquivo;
            oDadosRec.NomeArquivo = NomeArquivo;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                retEnviNFeList = xml.GetElementsByTagName("retConsReciNFe");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;


                    oDadosRec.nRec = retEnviNFeElemento.GetElementsByTagName("nRec")[0].InnerText;

                    XmlNodeList protNFe = xml.GetElementsByTagName("protNFe");

                    foreach (XmlNode infRecNode in protNFe)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;

                        if (infRecElemento.GetElementsByTagName("nProt").Count > 0)
                            oDadosRec.nProt = infRecElemento.GetElementsByTagName("nProt")[0].InnerText;

                        oDadosRec.dhRecbto = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRecbto")[0].InnerText).ToString("dd/MM/yyyy HH:mm:ss");
                        oDadosRec.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                        oDadosRec.chNFe = infRecElemento.GetElementsByTagName("chNFe")[0].InnerText;
                        oDadosRec.cStat = infRecElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return oDadosRec;
        }

        public static DadosRecClass ReciboProtNFe(string strXml, string caminhoArquivo, string NomeArquivo)
        {
            MemoryStream memoryStream = StringXmlToStream(strXml);
            DadosRecClass oDadosRec = new DadosRecClass();

            oDadosRec.cStat = string.Empty;
            oDadosRec.nRec = string.Empty;
            oDadosRec.dhRecbto = string.Empty;
            oDadosRec.nProt = string.Empty;
            oDadosRec.xMotivo = string.Empty;
            oDadosRec.chNFe = string.Empty;
            oDadosRec.xmlCompleto = strXml;
            oDadosRec.CaminhoArquivo = caminhoArquivo;
            oDadosRec.NomeArquivo = NomeArquivo;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList protNFe = xml.GetElementsByTagName("protNFe");

                oDadosRec.xmlAutorizado = protNFe[0].InnerXml;

                foreach (XmlNode retEnviNFeNode in protNFe)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                    oDadosRec.nRec = "";
                    foreach (XmlNode infRecNode in protNFe)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;

                        if (infRecElemento.GetElementsByTagName("nProt").Count > 0)
                            oDadosRec.nProt = infRecElemento.GetElementsByTagName("nProt")[0].InnerText;

                        oDadosRec.dhRecbto = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRecbto")[0].InnerText).ToString("dd/MM/yyyy HH:mm:ss");
                        oDadosRec.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                        oDadosRec.chNFe = infRecElemento.GetElementsByTagName("chNFe")[0].InnerText;
                        oDadosRec.cStat = infRecElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return oDadosRec;
        }

        public static DadosRetSit RetornoSit(string caminhoArquivo)
        {
            DadosRetSit ret = new DadosRetSit();

            try
            {
                string strXml = Util.LerArquivo(caminhoArquivo);

                MemoryStream memoryStream = StringXmlToStream(strXml);

                XmlDocument xml = new XmlDocument();

                xml.Load(memoryStream);


                XmlNodeList retConsSitNFe = xml.GetElementsByTagName("retConsSitNFe");

                foreach (XmlNode retEnviNFeNode in retConsSitNFe)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                    foreach (XmlNode infRecNode in retConsSitNFe)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;

                        if (infRecElemento.GetElementsByTagName("tpAmb").Count > 0)
                            ret.tpAmb = infRecElemento.GetElementsByTagName("tpAmb")[0].InnerText;

                        if (infRecElemento.GetElementsByTagName("verAplic").Count > 0)
                            ret.verAplic = infRecElemento.GetElementsByTagName("verAplic")[0].InnerText;

                        if (infRecElemento.GetElementsByTagName("cStat").Count > 0)
                            ret.cStat = infRecElemento.GetElementsByTagName("cStat")[0].InnerText;

                        if (infRecElemento.GetElementsByTagName("xMotivo").Count > 0)
                            ret.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;

                        if (infRecElemento.GetElementsByTagName("cUF").Count > 0)
                            ret.cUF = infRecElemento.GetElementsByTagName("cUF")[0].InnerText;

                        if (infRecElemento.GetElementsByTagName("chNFe").Count > 0)
                            ret.chNFe = infRecElemento.GetElementsByTagName("chNFe")[0].InnerText;

                    }
                }

                /*
                 <retConsSitNFe versao="2.01" xmlns="http://www.portalfiscal.inf.br/nfe">
          <tpAmb>1</tpAmb>
          <verAplic>SP_NFE_PL_006q</verAplic>
          <cStat>217</cStat>
          <xMotivo>Rejeição: NF-e não consta na base de dados da SEFAZ</xMotivo>
          <cUF>35</cUF>
          <chNFe>35140304771058000270550010000319431000928496</chNFe>
        </retConsSitNFe>
                 */

            }
            catch
            {
                ret = null;
            }
            return ret;
        }

        /*
        <retCancNFe versao="2.00" xmlns="http://www.portalfiscal.inf.br/nfe">
         * <infCanc>
             <tpAmb>2</tpAmb>
         *   <verAplic>SP_NFE_PL_006j</verAplic>
         *   <cStat>101</cStat>
         *   <xMotivo>Cancelamento de NF-e homologado</xMotivo>
         *   <cUF>35</cUF>
         *   <chNFe>35121010900358000477550010000000601000163183</chNFe>
         *   <dhRecbto>2012-10-18T16:18:18</dhRecbto>
         *   <nProt>135120006375877</nProt>
         </infCanc>
         </retCancNFe>         
         */
        /*
            <retEnvEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
	            <idLote>000000000000068</idLote>
	            <tpAmb>2</tpAmb>
	            <verAplic>SP_EVENTOS_PL_100</verAplic>
	            <cOrgao>35</cOrgao>
	            <cStat>128</cStat>
	            <xMotivo>Lote de Evento Processado</xMotivo>
	            <retEvento versao="1.00">
		            <infEvento>
			            <tpAmb>2</tpAmb>
			            <verAplic>SP_EVENTOS_PL_100</verAplic>
			            <cOrgao>35</cOrgao>
			            <cStat>135</cStat>
			            <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
			            <chNFe>35130410900358000477550010000000681002311335</chNFe>
			            <tpEvento>110111</tpEvento>
			            <xEvento>Cancelamento registrado</xEvento>
			            <nSeqEvento>1</nSeqEvento>
			            <CNPJDest>99999999000191</CNPJDest>
			            <dhRegEvento>2013-04-09T16:59:26-03:00</dhRegEvento>
			            <nProt>135130002216742</nProt>
		            </infEvento>
	            </retEvento>
            </retEnvEvento>         
         */
        public static DadosRecClass ReciboCancelamento(string strXml)
        {
            MemoryStream memoryStream = StringXmlToStream(strXml);
            DadosRecClass oDadosRec = new DadosRecClass();

            oDadosRec.cStat = string.Empty;
            oDadosRec.nRec = string.Empty;
            oDadosRec.dhRecbto = string.Empty;
            oDadosRec.nProt = string.Empty;
            oDadosRec.xMotivo = string.Empty;
            oDadosRec.chNFe = string.Empty;
            oDadosRec.xmlAutorizado = strXml;

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                retEnviNFeList = xml.GetElementsByTagName("retEvento");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                    XmlNodeList protNFe = retEnviNFeElemento.GetElementsByTagName("infEvento");

                    foreach (XmlNode infRecNode in protNFe)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;

                        if (infRecElemento.GetElementsByTagName("nProt").Count > 0)
                            oDadosRec.nProt = infRecElemento.GetElementsByTagName("nProt")[0].InnerText;

                        oDadosRec.dhRecbto = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRegEvento")[0].InnerText).ToString("dd/MM/yyyy HH:mm:ss");
                        oDadosRec.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                        oDadosRec.chNFe = infRecElemento.GetElementsByTagName("chNFe")[0].InnerText;
                        oDadosRec.cStat = infRecElemento.GetElementsByTagName("cStat")[0].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return oDadosRec;
        }

        //public static DadosRecClass ReciboCancelamento(string strXml)
        //{
        //    MemoryStream memoryStream = StringXmlToStream(strXml);
        //    DadosRecClass oDadosRec = new DadosRecClass();

        //    oDadosRec.cStat = string.Empty;
        //    oDadosRec.nRec = string.Empty;
        //    oDadosRec.dhRecbto = string.Empty;
        //    oDadosRec.nProt = string.Empty;
        //    oDadosRec.xMotivo = string.Empty;
        //    oDadosRec.chNFe = string.Empty;
        //    oDadosRec.xml = strXml;

        //    try
        //    {
        //        XmlDocument xml = new XmlDocument();
        //        xml.Load(memoryStream);

        //        XmlNodeList retEnviNFeList = null;

        //        retEnviNFeList = xml.GetElementsByTagName("retCancNFe");

        //        foreach (XmlNode retEnviNFeNode in retEnviNFeList)
        //        {
        //            XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

        //            XmlNodeList protNFe = retEnviNFeElemento.GetElementsByTagName("infCanc");

        //            foreach (XmlNode infRecNode in protNFe)
        //            {
        //                XmlElement infRecElemento = (XmlElement)infRecNode;

        //                if (infRecElemento.GetElementsByTagName("nProt").Count > 0)
        //                    oDadosRec.nProt = infRecElemento.GetElementsByTagName("nProt")[0].InnerText;

        //                oDadosRec.dhRecbto = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRecbto")[0].InnerText).ToString("dd/MM/yyyy");
        //                oDadosRec.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
        //                oDadosRec.chNFe = infRecElemento.GetElementsByTagName("chNFe")[0].InnerText;
        //                oDadosRec.cStat = infRecElemento.GetElementsByTagName("cStat")[0].InnerText;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    return oDadosRec;
        //}
        public static DateTime PegarDataCancelamento(string strXml)
        {
            DateTime dtCancelamento = DateTime.MinValue;

            MemoryStream memoryStream = StringXmlToStream(strXml);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                retEnviNFeList = xml.GetElementsByTagName("procCancNFe");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                    XmlNodeList protNFe = xml.GetElementsByTagName("cancNFe");

                    foreach (XmlNode infRecNode in protNFe)
                    {
                        XmlElement infRecElemento = (XmlElement)infRecNode;
                        dtCancelamento = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRecbto")[0].InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return dtCancelamento;

        }

        //public static DadosRecClass ReciboCancelamento(string strXml)
        //{
        //    DateTime dtCancelamento = DateTime.MinValue;

        //    MemoryStream memoryStream = StringXmlToStream(strXml);
        //    try
        //    {
        //        XmlDocument xml = new XmlDocument();
        //        xml.Load(memoryStream);

        //        XmlNodeList retEnviNFeList = null;

        //        retEnviNFeList = xml.GetElementsByTagName("procCancNFe");

        //        foreach (XmlNode retEnviNFeNode in retEnviNFeList)
        //        {
        //            XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
        //            XmlNodeList protNFe = xml.GetElementsByTagName("cancNFe");

        //            foreach (XmlNode infRecNode in protNFe)
        //            {
        //                XmlElement infRecElemento = (XmlElement)infRecNode;
        //                dtCancelamento = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRecbto")[0].InnerText);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }

        //    return dtCancelamento;

        //}

        public static string LerArquivo(string cArquivo)
        {
            string Retorno = string.Empty;
            if (File.Exists(cArquivo))
            {
                //StreamReader SR = null;

                try
                {
                    using (TextReader txt = new StreamReader(cArquivo, Encoding.GetEncoding(1252), true))
                    {
                        Retorno = txt.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            return Retorno;
        }

        public static string LerArquivoNoEncoding(string cArquivo)
        {
            string Retorno = string.Empty;
            if (File.Exists(cArquivo))
            {
                //StreamReader SR = null;
                TextReader txt = null;
                try
                {
                    //txt = new StreamReader(cArquivo, Encoding.GetEncoding("iso8859-1"), true);
                    txt = new StreamReader(cArquivo);
                    //SR = File.OpenText(cArquivo);
                    //Retorno = SR.ReadToEnd();
                    Retorno = txt.ReadToEnd();

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    txt.Close();
                    //SR.Close();

                }
            }
            return Retorno;
        }

        public static void SalvarAnexoEmailNoDisco(System.Net.Mail.Attachment attachment, string PastaDestino, string NomeArquivo)
        {
            byte[] allBytes = new byte[attachment.ContentStream.Length];
            int bytesRead = attachment.ContentStream.Read(allBytes, 0, (int)attachment.ContentStream.Length);

            string destinationFile = PastaDestino + NomeArquivo;

            if (System.IO.File.Exists(destinationFile))
            {
                System.IO.File.Delete(destinationFile);
            }

            BinaryWriter writer = new BinaryWriter(new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete));
            writer.Write(allBytes);
            writer.Close();
            writer.Dispose();
        }

        #region Validação do XML
        public string MsgValidacaoXML { get; set; }

        public void ValidaXMLNFE(string _arquivo, string _arquivoXSD)
        {
            string arqTmp = _arquivo.Trim();

            if (!arqTmp.Contains("<Signature"))
            {
                arqTmp = arqTmp.Replace("</NFe>", string.Empty);

                StringBuilder sig = new StringBuilder();

                sig.AppendLine("<Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">");
                sig.AppendLine("<SignedInfo> ");
                sig.AppendLine("    <CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />");
                sig.AppendLine("    <SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\" />");
                sig.AppendLine("    <Reference URI=\"#NFe35130410900358000477550010000000671002255560\">");
                sig.AppendLine("    <Transforms> ");
                sig.AppendLine("        <Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" />");
                sig.AppendLine("        <Transform Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />");
                sig.AppendLine("    </Transforms> ");
                sig.AppendLine("    <DigestMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#sha1\" />");
                sig.AppendLine("    <DigestValue>CVLCRFyE+Nmf3KHsO+DtWfyqm9c=</DigestValue>");
                sig.AppendLine("    </Reference> ");
                sig.AppendLine("</SignedInfo> ");
                sig.AppendLine("<SignatureValue>OiPv1hKy5FVkXQlMSvwzaAYQef0L18JI8Haaeen0/5SJZiYYPoR+URu+GBll4REU4cwCMOKz5PvFsGC10CcVreuuuepWNh+1dQpj9hpgTuqYrf4Unr2NlrCOa9wwm68WJ94xUQlSDNxQD8BONGu1stueKrccNfnhPHni056GE4O8pZqipHBu73j8HMMuWO55W1WkeWoCL/BrjJom1nPU+3Cj3nraeOnL0QCuX/dmYButxGLr2NxjaHFUq4BIgIk/TZvhvTOyP9yXguBbI6TRLOpQGpotWeCvgA4R8z3KhAQZ5ST84sSe2BSehztJ0iZLGcBty3uTa5zq3XnvnmDbBw==</SignatureValue> ");
                sig.AppendLine("<KeyInfo> ");
                sig.AppendLine("    <X509Data> ");
                sig.AppendLine("    <X509Certificate>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</X509Certificate> ");
                sig.AppendLine("    </X509Data> ");
                sig.AppendLine("</KeyInfo> ");
                sig.AppendLine("</Signature> ");

                arqTmp += sig.ToString();

                arqTmp += "</NFe>";
            }

            XmlReader xmlReader = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("http://www.portalfiscal.inf.br/nfe", _arquivoXSD);
            //schemas.Add("", _arquivoXSD);


            settings.Schemas = schemas;

            settings.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);

            xmlReader = XmlReader.Create(new MemoryStream(UTF8Encoding.Default.GetBytes(arqTmp)), settings);

            while (xmlReader.Read()) { }

            xmlReader.Close();
            //XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(new StreamReader(_arquivo)));
            //XmlSchemaCollection schemaCollection = new XmlSchemaCollection();
            //schemaCollection.Add("http://www.portalfiscal.inf.br/nfe", _arquivoXSD + "\\" + "procNFe_v2.00.xsd");
            //reader.Schemas.Add(schemaCollection);
            //reader.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);
            //while (reader.Read()) { }
            //reader.Close();
        }

        private void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            //MsgValidacaoXML += "Linha: " + e.Exception.LineNumber + " Coluna: " + e.Exception.LinePosition + " Erro: " + e.Exception.Message + "\r\n";
            MsgValidacaoXML += e.Exception.Message + "\r\n";
        }
        #endregion

        public static NFE CarregarNfe(string cArquivoXML)
        {
            NFE _NFE = new NFE();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cArquivoXML);

                //Pegar a tag protNFe Autorização
                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

                XPathNavigator xpathNav = doc.CreateNavigator();

                //Pega a O XML de autorização                
                XPathNavigator node = xpathNav.SelectSingleNode("//nfe:protNFe", ns);
                _NFE.XML_AUT = node.OuterXml;

                //Pega a O XML da nfe sem a autorização                
                node = xpathNav.SelectSingleNode("//nfe:NFe", ns);
                _NFE.XML_NFE = node.OuterXml;

                XmlNodeList infProtList = doc.GetElementsByTagName("infProt");
                foreach (XmlNode infProtNode in infProtList)
                {
                    XmlElement infProtElemento = (XmlElement)infProtNode;
                    _NFE.STATUSNFE = Convert.ToInt32(infProtElemento.GetElementsByTagName("cStat")[0].InnerText);
                }

                XmlNodeList infNFeList = null;
                infNFeList = doc.GetElementsByTagName("infNFe");

                foreach (XmlNode infNFeNode in infNFeList)
                {
                    XmlElement infNFeElemento = (XmlElement)infNFeNode;

                    //Pegar a chave da NF-e
                    if (infNFeElemento.HasAttributes)
                    {
                        _NFE.IDE_ID = infNFeElemento.Attributes["Id"].InnerText.Remove(0, 3);
                    }

                    //Montar lista de tag´s da tag <ide>
                    XmlNodeList ideList = infNFeElemento.GetElementsByTagName("ide");

                    foreach (XmlNode ideNode in ideList)
                    {
                        XmlElement ideElemento = (XmlElement)ideNode;
                        _NFE.IDE_CUF = ideElemento.GetElementsByTagName("cUF")[0].InnerText;
                        _NFE.IDE_ID_RELATED = "";
                        _NFE.IDE_MOD = ideElemento.GetElementsByTagName("mod")[0].InnerText;
                        _NFE.IDE_NNF = Convert.ToInt32(ideElemento.GetElementsByTagName("nNF")[0].InnerText);
                        _NFE.IDE_SERIE = ideElemento.GetElementsByTagName("serie")[0].InnerText; ;
                        _NFE.IDE_TPEMIS = ideElemento.GetElementsByTagName("tpEmis")[0].InnerText;
                        _NFE.IDEID1 = "";
                        _NFE.IDEID2 = "";
                        _NFE.IDEID3 = "";
                        //dhEmi
                        if (ideElemento.GetElementsByTagName("dEmi").Count > 0)
                        {
                            _NFE.IDE_DEMI = Convert.ToDateTime(ideElemento.GetElementsByTagName("dEmi")[0].InnerText);
                        }
                        else
                        {
                            if (ideElemento.GetElementsByTagName("dhEmi").Count > 0)
                            {
                                _NFE.IDE_DEMI = Convert.ToDateTime(ideElemento.GetElementsByTagName("dhEmi")[0].InnerText);
                            }
                        }

                    }

                    //Montar lista de tag´s da tag <emit>
                    XmlNodeList emitList = infNFeElemento.GetElementsByTagName("emit");
                    foreach (XmlNode emitNode in emitList)
                    {
                        XmlElement emitElemento = (XmlElement)emitNode;
                        _NFE.EMIT_CNPJ = emitElemento.GetElementsByTagName("CNPJ")[0].InnerText;
                        _NFE.EMIT_IE = emitElemento.GetElementsByTagName("IE")[0].InnerText;
                        _NFE.RAZAOSOCIAL = emitElemento.GetElementsByTagName("xNome")[0].InnerText;
                    }

                    XmlNodeList destList = infNFeElemento.GetElementsByTagName("dest");
                    foreach (XmlNode destNode in destList)
                    {
                        XmlElement destElemento = (XmlElement)destNode;
                        if (destElemento.GetElementsByTagName("CNPJ").Count > 0)
                            _NFE.DEST_CNPJ = destElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                        if (destElemento.GetElementsByTagName("CPF").Count > 0)
                            _NFE.DEST_CPF = destElemento.GetElementsByTagName("CPF")[0].InnerText;

                        if (destElemento.GetElementsByTagName("IE").Count > 0)
                            _NFE.DEST_IE = destElemento.GetElementsByTagName("IE")[0].InnerText;

                        if (destElemento.GetElementsByTagName("xNome").Count > 0)
                            _NFE.DEST_NOME = destElemento.GetElementsByTagName("xNome")[0].InnerText;

                    }
                    //enderDest
                    XmlNodeList enderDestList = infNFeElemento.GetElementsByTagName("enderDest");
                    foreach (XmlNode enderDestNode in enderDestList)
                    {
                        XmlElement enderDestElemento = (XmlElement)enderDestNode;
                        _NFE.DEST_ENDERDEST_UF = enderDestElemento.GetElementsByTagName("UF")[0].InnerText;
                    }
                    /*
    <compra>
      <xPed>343441</xPed>
    </compra>
                     */
                    try
                    {
                        XmlNodeList compraList = infNFeElemento.GetElementsByTagName("compra");
                        foreach (XmlNode compraNode in compraList)
                        {
                            XmlElement compraElemento = (XmlElement)compraNode;
                            _NFE.CODPEDIDO = compraElemento.GetElementsByTagName("xPed")[0].InnerText;
                        }
                    }
                    catch
                    {
                        _NFE.CODPEDIDO = _NFE.IDE_NNF.ToString();
                    }

                }

                XmlNodeList ICMSTotList = null;
                ICMSTotList = doc.GetElementsByTagName("ICMSTot");

                foreach (XmlNode ICMSTotNode in ICMSTotList)
                {
                    XmlElement ICMSTotElemento = (XmlElement)ICMSTotNode;
                    //Desenvolvimento
                    //_NFE.TOTAL_ICMSTOT_VBC = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vBC")[0].InnerText.Replace('.', ','));
                    //_NFE.TOTAL_ICMSTOT_VBCST = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vBCST")[0].InnerText.Replace('.', ','));
                    //_NFE.TOTAL_ICMSTOT_VICMS = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vICMS")[0].InnerText.Replace('.', ','));
                    //_NFE.TOTAL_ICMSTOT_VPROD = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vProd")[0].InnerText.Replace('.', ','));
                    //_NFE.TOTAL_ICMSTOT_VST = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vST")[0].InnerText.Replace('.', ','));

                    //produção
                    _NFE.TOTAL_ICMSTOT_VBC = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vBC")[0].InnerText);
                    _NFE.TOTAL_ICMSTOT_VBCST = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vBCST")[0].InnerText);
                    _NFE.TOTAL_ICMSTOT_VICMS = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vICMS")[0].InnerText);
                    _NFE.TOTAL_ICMSTOT_VPROD = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vProd")[0].InnerText);
                    _NFE.TOTAL_ICMSTOT_VST = Convert.ToDecimal(ICMSTotElemento.GetElementsByTagName("vST")[0].InnerText);

                }


                XmlNodeList ISSQNtotList = null;
                ISSQNtotList = doc.GetElementsByTagName("ISSQNtot");
                foreach (XmlNode ISSQNtotNode in ISSQNtotList)
                {
                    XmlElement ISSQNtotElemento = (XmlElement)ISSQNtotNode;
                    if (ISSQNtotElemento.HasAttributes)
                    {
                        _NFE.TOTAL_ISSQNTOT_VBC = Convert.ToDecimal(ISSQNtotElemento.GetElementsByTagName("vBC")[0].InnerText);
                    }
                }

                XmlNodeList transpList = doc.GetElementsByTagName("transporta");
                foreach (XmlNode transpNode in transpList)
                {
                    XmlElement transpElemento = (XmlElement)transpNode;

                    if (transpElemento.GetElementsByTagName("IE").Count > 0)
                        _NFE.TRANSP_IE = transpElemento.GetElementsByTagName("IE")[0].InnerText;

                    if (transpElemento.GetElementsByTagName("CNPJ").Count > 0)
                        _NFE.TRANSP_TRANSPORTA_CNPJ = transpElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                    //_NFE.TRANSP_VEICTRANSP_PLACA = transpElemento.GetElementsByTagName("")[0].InnerText;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return _NFE;
        }

        public static bool ValidarEmail(string email)
        {
            bool retorno = false;
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            if (rg.IsMatch(email) && IsValidEmail(email))
                retorno = true;
            return retorno;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /*
 - <retEvento versao="1.00" xmlns="http://www.portalfiscal.inf.br/nfe">
- <infEvento>
  <tpAmb>2</tpAmb> 
  <verAplic>SP_EVENTOS_PL_100</verAplic> 
  <cOrgao>35</cOrgao> 
  <cStat>135</cStat> 
  <xMotivo>Evento registrado e vinculado a NF-e</xMotivo> 
  <chNFe>35130414364911000102550010000007851001656431</chNFe> 
  <tpEvento>110110</tpEvento> 
  <xEvento>Carta de Correção registrada</xEvento> 
  <nSeqEvento>1</nSeqEvento> 
  <CNPJDest>99999999000191</CNPJDest> 
  <dhRegEvento>2013-04-24T10:10:38-03:00</dhRegEvento> 
  <nProt>135130002463290</nProt> 
  </infEvento>
  </retEvento>        
         */
        public static RetEnvEvento RetornoEvento(string strxml, string NomeArquivo, string CaminhoArquivo)
        {
            MemoryStream memoryStream = StringXmlToStream(strxml);
            DadosRecClass oDadosRec = new DadosRecClass();

            RetEnvEvento retEnvRetorno = new RetEnvEvento();
            retEnvRetorno.NomeArquivo = NomeArquivo;
            retEnvRetorno.CaminhoArquivo = CaminhoArquivo;
            retEnvRetorno.XMLaprovado = strxml;
            XmlDocument xml = new XmlDocument();
            xml.Load(memoryStream);

            XmlNodeList retEnvEventoList = null;

            //retEnvEventoList = xml.GetElementsByTagName("retEnvEvento");
            retEnvEventoList = xml.GetElementsByTagName("retEvento");

            foreach (XmlNode retEnviNFeNode in retEnvEventoList)
            {
                XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                //retEnvRetorno.cOrgao = retEnviNFeElemento.GetElementsByTagName("cOrgao")[0].InnerText;
                //retEnvRetorno.cStat = retEnviNFeElemento.GetElementsByTagName("cStat")[0].InnerText;
                //retEnvRetorno.idLote = retEnviNFeElemento.GetElementsByTagName("idLote")[0].InnerText;
                // retEnvRetorno.tpAmb = retEnviNFeElemento.GetElementsByTagName("tpAmb")[0].InnerText;
                //retEnvRetorno.xMotivo = retEnviNFeElemento.GetElementsByTagName("xMotivo")[0].InnerText;

                XmlNodeList protNFe = xml.GetElementsByTagName("infEvento");


                XmlNode infRecNode = protNFe[1];//pego o segundo pq vem 2 no retorno

                //foreach (XmlNode infRecNode in protNFe)
                //{
                XmlElement infRecElemento = (XmlElement)infRecNode;

                if (infRecElemento.GetElementsByTagName("nProt").Count > 0)
                    retEnvRetorno.retEvento.nProt = infRecElemento.GetElementsByTagName("nProt")[0].InnerText;

                retEnvRetorno.retEvento.dhRegEvento = Convert.ToDateTime(infRecElemento.GetElementsByTagName("dhRegEvento")[0].InnerText).ToString("dd/MM/yyyy HH:mm:ss");
                retEnvRetorno.retEvento.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                retEnvRetorno.retEvento.chNfe = infRecElemento.GetElementsByTagName("chNFe")[0].InnerText;
                retEnvRetorno.retEvento.cStat = infRecElemento.GetElementsByTagName("cStat")[0].InnerText;
                //retEnvRetorno.retEvento.CNPJDest = infRecElemento.GetElementsByTagName("CNPJDest")[0] != null ? infRecElemento.GetElementsByTagName("CNPJDest")[0].InnerText : "";
                retEnvRetorno.retEvento.cOrgao = infRecElemento.GetElementsByTagName("cOrgao")[0].InnerText;
                retEnvRetorno.retEvento.nSeqEvento = Convert.ToInt32(infRecElemento.GetElementsByTagName("nSeqEvento")[0].InnerText);
                retEnvRetorno.retEvento.tpEvento = infRecElemento.GetElementsByTagName("tpEvento")[0].InnerText;
                retEnvRetorno.retEvento.xEvento = infRecElemento.GetElementsByTagName("xEvento")[0] != null ? infRecElemento.GetElementsByTagName("xEvento")[0].InnerText : "";
                retEnvRetorno.retEvento.xMotivo = infRecElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                //}



            }

            return retEnvRetorno;

        }

        /*
        <?xml version="1.0"?>
        <retEnvEvento xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.00">
          <idLote>1</idLote>
          <tpAmb>1</tpAmb>
          <verAplic>SP_EVENTOS_PL_100</verAplic>
          <cOrgao>35</cOrgao>
          <cStat>128</cStat>
          <xMotivo>Lote de Evento Processado</xMotivo>
          <retEvento versao="1.00">
            <infEvento>
              <tpAmb>1</tpAmb>
              <verAplic>SP_EVENTOS_PL_100</verAplic>
              <cOrgao>35</cOrgao>
              <cStat>135</cStat>
              <xMotivo>Evento registrado e vinculado a NF-e</xMotivo>
              <chNFe>35130314364911000102550010000005641001616575</chNFe>
              <tpEvento>110110</tpEvento>
              <xEvento>Carta de Correção registrada</xEvento>
              <nSeqEvento>1</nSeqEvento>
              <CNPJDest>00754307000188</CNPJDest>
              <dhRegEvento>2013-04-02T14:47:38-03:00</dhRegEvento>
              <nProt>135130190644156</nProt>
            </infEvento>
          </retEvento>
        </retEnvEvento>
 */

        public static string RetornaArquivoSitNfe(string ChaveNfe, string tpAmbiente)
        {
            StringBuilder retorno = new StringBuilder();
            retorno.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            retorno.AppendLine("<consSitNFe versao=\"3.10\" xmlns=\"http://www.portalfiscal.inf.br/nfe\">");
            retorno.AppendLine(string.Format("<tpAmb>{0}</tpAmb>", tpAmbiente));
            retorno.AppendLine("<xServ>CONSULTAR</xServ>");
            retorno.AppendLine(string.Format("<chNFe>{0}</chNFe>", ChaveNfe));
            retorno.AppendLine("</consSitNFe>");
            return retorno.ToString().Trim();
        }

        public class RetEnvEvento
        {
            public RetEnvEvento()
            {
                this.retEvento = new RetEvento();
            }
            //public string idLote { get; set; }
            public string cOrgao { get; set; }
            public string tpAmb { get; set; }
            public string CNPJDest { get; set; }
            public string dhEvento { get; set; }
            public string tpEvento { get; set; }
            public string nSeqEvento { get; set; }
            public RetEvento retEvento { get; set; }
            public string NomeArquivo { get; set; }
            public string CaminhoArquivo { get; set; }
            public string XMLaprovado { get; set; }
        }
        public class RetEvento
        {
            public string tpAmb { get; set; }
            public string cOrgao { get; set; }
            public string cStat { get; set; }
            public string xMotivo { get; set; }
            public string chNfe { get; set; }
            public string tpEvento { get; set; }
            public string xEvento { get; set; }
            public int nSeqEvento { get; set; }
            //public string CNPJDest { get; set; }
            public string dhRegEvento { get; set; }
            public string nProt { get; set; }
        }

        public class RetInutilizacao
        {
            public string cStat { get; set; }
            public string xMotivo { get; set; }
            public DateTime dhRecbto { get; set; }
            public string nProt { get; set; }
            public string IdInutilizacao { get; set; }
            public string CaminhoArquivo { get; set; }
            public string NomeArquivo { get; set; }
        }
        // Remember to add the following using statements to your code
        // using System.Net;
        // using System.IO;

        public static int DownloadFile(String remoteFilename,
                                       String localFilename)
        {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            int bytesProcessed = 0;

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try
            {
                // Create a request for the specified remote file name
                WebRequest request = WebRequest.Create(remoteFilename);
                request.Timeout = 60000;//1 minuto
                if (request != null)
                {
                    // Send the request to the server and retrieve the
                    // WebResponse object 
                    response = request.GetResponse();
                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();

                        // Create the local file
                        localStream = File.Create(localFilename);

                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Simple do/while loop to read from stream until
                        // no bytes are returned
                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);

                            // Increment total bytes processed
                            bytesProcessed += bytesRead;
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // Close the response and streams objects here 
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

            // Return total bytes processed to caller.
            return bytesProcessed;
        }

        public static string PegarCNPJTransportador(string strXml)
        {
            string _CNPJ = string.Empty;

            string xmlr = LerArquivo(strXml);

            MemoryStream memoryStream = StringXmlToStream(xmlr);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                retEnviNFeList = xml.GetElementsByTagName("transporta");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                    _CNPJ = retEnviNFeElemento.GetElementsByTagName("CNPJ")[0].InnerText;

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return _CNPJ;

        }

        public static void EnviaEmail(ObjEmail _objemail, string fromTo = "")
        {
            RJSOptimusEmail email = new RJSOptimusEmail(_objemail.SMTP, _objemail.PORTA, _objemail.USUARIO, _objemail.SENHA, _objemail.ENABLESSL);

            if (!string.IsNullOrEmpty(fromTo))
            {
                email.From = fromTo;
            }
            else
            {
                email.From = _objemail.USUARIO;
            }


            email.To = _objemail.TO;
            if (_objemail.CC.Count > 0)
            {
                foreach (string iemail in _objemail.CC)
                {
                    email.AddEmail(iemail);
                }

            }

            email.Subject = _objemail.SUBJECT;
            email.Body = _objemail.BODY;
            email.Enviar();

        }


        public static string PegarIDInutilizacao(string strXml)
        {
            string _IDINUTI = string.Empty;

            MemoryStream memoryStream = StringXmlToStream(strXml);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                retEnviNFeList = xml.GetElementsByTagName("infInut");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                    _IDINUTI = retEnviNFeElemento.GetAttribute("Id");

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            _IDINUTI = _IDINUTI.Substring(2, _IDINUTI.Length - 2);

            return _IDINUTI;

        }


        public static RetInutilizacao PegarRetornoInutilizacao(string strXml, string nomeArquivo)
        {
            RetInutilizacao retinut = new RetInutilizacao();

            MemoryStream memoryStream = StringXmlToStreamUTF8(strXml);
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(memoryStream);

                XmlNodeList retEnviNFeList = null;

                retEnviNFeList = xml.GetElementsByTagName("retInutNFe");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                    retinut.cStat = retEnviNFeElemento.GetElementsByTagName("cStat")[0].InnerText;
                    retinut.dhRecbto = Convert.ToDateTime(retEnviNFeElemento.GetElementsByTagName("dhRecbto")[0].InnerText);

                    if (retinut.cStat == "102")
                    {
                        retinut.nProt = retEnviNFeElemento.GetElementsByTagName("nProt")[0].InnerText;
                    }

                    retinut.xMotivo = retEnviNFeElemento.GetElementsByTagName("xMotivo")[0].InnerText;
                    retinut.IdInutilizacao = nomeArquivo.Substring(0, nomeArquivo.Length - 8);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return retinut;
        }

        public static string GeraChaveNFE(int cUF, DateTime DtEmissao, Int64 EmitCNPJ, int ModNFe, double CdSerie, int NrNF, int TpEmiss, int cNF, out int cDV)
        {
            //· cUF     - (02) - Código da UF do emitente do Documento Fiscal 
            //· AAMM    - (04) - Ano e Mês de emissão da NF-e
            //· CNPJ    - (14) - CNPJ do emitente
            //· mod     - (02) - Modelo do Documento Fiscal
            //· serie   - (03) - Série do Documento Fiscal
            //· nNF     - (09) - Número do Documento Fiscal
            //· tpEmis  – (01) - forma de emissão da NF-e
            //· cNF     - (08) - Código Numérico que compõe a Chave de Acesso
            //· cDV     - (01) - Dígito Verificador da Chave de Acesso

            string AAMM = DtEmissao.Year.ToString().Substring(2, 2).PadLeft(2, '0') + DtEmissao.Month.ToString().PadLeft(2, '0');
            String ChaveNFe = cUF.ToString() +
                               AAMM +
                               EmitCNPJ.ToString().PadLeft(14, '0') +
                               ModNFe.ToString().PadLeft(2, '0') +
                               CdSerie.ToString().PadLeft(3, '0') +
                               NrNF.ToString().Trim().PadLeft(9, '0') +
                               TpEmiss.ToString().Trim().PadLeft(1, '0') +
                               cNF.ToString().Trim().PadLeft(8, '0');

            //----------- digito verificador da chave da NF-e -----------//
            String Multiplicadores = "432987654329876543298765432987654329876543298765432987654329876543298765432";
            int digito = 0;
            int total = 0;

            for (int index = 0; index < ChaveNFe.Length; index++)
            {
                total += Convert.ToInt32(ChaveNFe.Substring(index, 1)) * Convert.ToInt32(Multiplicadores.Substring(index, 1));
            }

            digito = 11 - (total % 11);
            if ((total % 11) == 0 || (total % 11) == 1)
            {
                digito = 0;
            }
            /*
            if (digito > 9 || digito == 1)
            {
                digito = 0;
            }
            */
            cDV = digito;

            return ChaveNFe + digito.ToString();
        }


        public static string AlterarChaveETpemisaoNFE(string strXml, string tpEmissao, bool ehArquivo, out string NovaChaveNfe)
        {
            string _CNPJ = string.Empty;

            string xmlr = ehArquivo ? LerArquivo(strXml) : strXml;

            int _CUF = 0;
            DateTime _DTEMISSAO = DateTime.MinValue;
            Int64 _EMITCNPJ = 0;
            int _MODNFE = 0;
            double _CDSERIE = 0;
            int _NRNF = 0;
            int _TPEMISS = 0;
            int _CNF = 0;

            MemoryStream memoryStream = StringXmlToStream(xmlr);

            XmlDocument xml = new XmlDocument();

            try
            {
                xml.Load(memoryStream);

                XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);
                ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

                //XPathNavigator xpathNav = xml.CreateNavigator();

                XmlNodeList retEnviNFeList = null;
                /*
                  <cUF>31</cUF>
                  <cNF>00102814</cNF>
                  <natOp>VENDA DE MERCADORIA ADQ.TERCEIROS</natOp>
                  <indPag>1</indPag>
                  <mod>55</mod>
                  <serie>1</serie>
                  <nNF>46324</nNF>
                  <dhEmi>2015-06-23T00:00:00-03:00</dhEmi>
                  <dhSaiEnt>2015-06-23T00:00:00-03:00</dhSaiEnt>
                  <tpNF>1</tpNF>
                  <idDest>1</idDest>
                  <cMunFG>3170206</cMunFG>
                  <tpImp>2</tpImp>
                  <tpEmis>1</tpEmis>
                  <cDV>2</cDV>
                  <tpAmb>1</tpAmb>
                  <finNFe>1</finNFe>
                  <indFinal>1</indFinal>
                  <indPres>0</indPres>
                  <procEmi>0</procEmi>
                 */
                retEnviNFeList = xml.GetElementsByTagName("ide");

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;

                    //alterar a TP emissao
                    retEnviNFeElemento.GetElementsByTagName("tpEmis")[0].InnerText = tpEmissao;

                    XmlNode ide = xml.SelectSingleNode("//nfe:ide", ns);

                    XmlNode xmlRecordxJust = xml.CreateNode(XmlNodeType.Element, "xJust", "");
                    xmlRecordxJust.InnerText = "AMBIENTE DE ORIGEM FORA DE OPERACAO";

                    XmlNode xmlRecorddhCont = xml.CreateNode(XmlNodeType.Element, "dhCont", "");
                    xmlRecorddhCont.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                    //AAAA-MM-DDThh:mm:ssTZD
                    XmlNode LastChild = retEnviNFeElemento.LastChild;

                    if (LastChild.Name == "NFref")
                    {
                        XmlNode verProcChild = retEnviNFeElemento.GetElementsByTagName("verProc")[0];
                        ide.InsertAfter(xmlRecorddhCont, verProcChild);

                        XmlNode dhContChild = retEnviNFeElemento.GetElementsByTagName("dhCont")[0];
                        ide.InsertAfter(xmlRecordxJust, dhContChild);
                    }
                    else
                    {
                        ide.InsertAfter(xmlRecorddhCont, LastChild);

                        LastChild = retEnviNFeElemento.LastChild;
                        ide.InsertAfter(xmlRecordxJust, LastChild);
                    }




                    _CUF = Convert.ToInt32(retEnviNFeElemento.GetElementsByTagName("cUF")[0].InnerText);
                    _DTEMISSAO = Convert.ToDateTime(retEnviNFeElemento.GetElementsByTagName("dhSaiEnt")[0].InnerText);

                    _MODNFE = Convert.ToInt32(retEnviNFeElemento.GetElementsByTagName("mod")[0].InnerText);
                    _CDSERIE = Convert.ToDouble(retEnviNFeElemento.GetElementsByTagName("serie")[0].InnerText);
                    _NRNF = Convert.ToInt32(retEnviNFeElemento.GetElementsByTagName("nNF")[0].InnerText);
                    _TPEMISS = Convert.ToInt32(retEnviNFeElemento.GetElementsByTagName("tpEmis")[0].InnerText);
                    _CNF = Convert.ToInt32(retEnviNFeElemento.GetElementsByTagName("cNF")[0].InnerText);
                }

                XmlNodeList retEmitList = null;
                retEmitList = xml.GetElementsByTagName("emit");

                foreach (XmlNode retEmitNode in retEmitList)
                {
                    XmlElement retEmitElemento = (XmlElement)retEmitNode;
                    _EMITCNPJ = Convert.ToInt64(retEmitElemento.GetElementsByTagName("CNPJ")[0].InnerText);

                }

                //alterar o ID
                int cDV = 0;
                string chavenfe = GeraChaveNFE(_CUF, _DTEMISSAO, _EMITCNPJ, _MODNFE, _CDSERIE, _NRNF, _TPEMISS, _CNF, out cDV);

                foreach (XmlNode retEnviNFeNode in retEnviNFeList)
                {
                    XmlElement retEnviNFeElemento = (XmlElement)retEnviNFeNode;
                    //alterar a TP emissao
                    retEnviNFeElemento.GetElementsByTagName("cDV")[0].InnerText = cDV.ToString();
                }

                //XmlNodeList retNewIDList = null;

                xml.GetElementsByTagName("infNFe")[0].Attributes["Id"].Value = "NFe" + chavenfe;

                NovaChaveNfe = chavenfe;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return xml.InnerXml.Replace("<xJust xmlns=\"\">", "<xJust>").Replace("<dhCont xmlns=\"\">", "<dhCont>");

        }
        #region BuscaConfiguracaoCertificado
        public static X509Certificate2 BuscaConfiguracaoCertificado(string CertificadoDigitalThumbPrint, string CertificadoSubject)
        {
            X509Certificate2 x509Cert = null;


            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection collection1 = null;
            if (!string.IsNullOrEmpty(CertificadoDigitalThumbPrint))
                collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, CertificadoDigitalThumbPrint, false);
            else
                collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, CertificadoSubject, false);

            for (int i = 0; i < collection1.Count; i++)
            {
                //Verificar a validade do certificado
                if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                {
                    x509Cert = collection1[i];
                    break;
                }
            }

            //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
            if (x509Cert == null && collection1.Count > 0)
                x509Cert = collection1[0];

            return x509Cert;
        }
        #endregion

        public static String frmNumero2(Object data)
        {

            if (data == null) return "0.00";
            else if (data.ToString() == "") return "0.00";
            else
            {
                String s = data.ToString().Trim();
                if (s.Substring(0, 1) == ",") s = "0" + s;


                Double x = System.Double.Parse(s);
                return System.String.Format("{0:#0.00}", x).Replace(",", ".");
            }
        }
        public static string ValorGNRE(string pvalor)
        {
            string casadecimal = pvalor.Substring(pvalor.Length - 2, 2);
            string valor = pvalor.Substring(0, pvalor.Length - 2);

            return valor + "," + casadecimal;

        }

        /// <summary>
        /// Controla os arquivos de erros de um determinado folder.
        /// retorna uma string com o conteudo dos arquivos que leu.
        /// </summary>
        /// <param name="CNPJ"></param>
        /// <param name="SCHEMA"></param>
        /// <param name="FOLDERCONFIG">folder onde vai ficar as configs</param>
        /// <param name="ArquivosErro">Lista de Fileinfo com os arquivos de erro que vai controlar</param>
        /// <returns></returns>
        public static string GerenciarEmailErro(string CNPJ, string SCHEMA, string FOLDERCONFIG, System.IO.FileInfo[] ArquivosErro)
        {
            string arqconfigpath = string.Format(@"{0}config_send_email_error_{1}_{2}.json", FOLDERCONFIG, CNPJ, SCHEMA);

            System.Text.StringBuilder bodyemail = new System.Text.StringBuilder();

            if (File.Exists(arqconfigpath))
            {
                ConfigEnvioEmailErro conf = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigEnvioEmailErro>(File.ReadAllText(arqconfigpath));
                if (DateTime.Now > conf.dtexecucao)
                {
                    string arquivo = string.Empty;
                    bodyemail.AppendLine("TOTAL DE ARQ COM ERRO:" +ArquivosErro.Count().ToString());
                    foreach (var arq in ArquivosErro)
                    {
                        arquivo = string.Empty;
                        if (System.IO.File.Exists(arq.FullName))
                        {
                            arquivo = LerArquivo(arq.FullName);
                            bodyemail.AppendLine("");
                            bodyemail.AppendFormat("{0}-{1}-{2}-->ERR0:{3}", SCHEMA, CNPJ, arq.FullName, arquivo);
                            bodyemail.AppendLine("");
                            System.IO.File.Delete(arq.FullName);
                        }
                    }
                    ConfigEnvioEmailErro confnew = new ConfigEnvioEmailErro();
                    confnew.CNPJ = CNPJ;
                    confnew.Schema = SCHEMA;
                    confnew.dtexecucao = DateTime.Now.AddMinutes(20);
                    System.IO.File.WriteAllText(arqconfigpath, Newtonsoft.Json.JsonConvert.SerializeObject(confnew));
                }
            }
            else
            {
                if (!System.IO.Directory.Exists(FOLDERCONFIG))
                {
                    System.IO.Directory.CreateDirectory(FOLDERCONFIG);
                }
                ConfigEnvioEmailErro conf = new ConfigEnvioEmailErro();
                conf.CNPJ = CNPJ;
                conf.Schema = SCHEMA;
                conf.dtexecucao = DateTime.Now;
                System.IO.File.WriteAllText(arqconfigpath, Newtonsoft.Json.JsonConvert.SerializeObject(conf));
            }
            return bodyemail.ToString();
        }

        public class ConfigEnvioEmailErro
        {
            public DateTime dtexecucao { get; set; }
            public string CNPJ { get; set; }

            public string Schema { get; set; }
        }

    }
}
