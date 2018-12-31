using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Globalization;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFEHelper
    {
        //public DataTable NotaFiscalIntegracao()
        //{
        //    DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISINTEGRAR", null, "P_CURSOR", OracleHelper.BancoOracle.optimus);
        //    return ds.Tables[0];
        //}

        public DataTable NotaFiscalIntegracao(string datasource, string schema)
        {
            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISINTEGRAR", null, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        //public DataTable NotaFiscalIntegracaoFilial(int cdentifilial)
        //{
        //    List<OracleParameter> lstParam = new List<OracleParameter>();
        //    lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

        //    DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISINTEGRARFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus);
        //    return ds.Tables[0];
        //}

        public DataTable NotaFiscalIntegracaoFilial(int cdentifilial, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISINTEGRARFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        //public DataTable NotaFiscalBuscarRetorno()
        //{
        //    DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISBUSCARETORNO", null, "P_CURSOR", OracleHelper.BancoOracle.optimus);
        //    return ds.Tables[0];
        //}

        public DataTable NotaFiscalBuscarRetorno(string datasource, string schema)
        {
            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISBUSCARETORNO", null, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        //public DataTable NotaFiscalBuscarRetornoFilial(int cdentifilial)
        //{
        //    List<OracleParameter> lstParam = new List<OracleParameter>();
        //    lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

        //    DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISBUSCARETFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus);
        //    return ds.Tables[0];
        //}

        public DataTable NotaFiscalBuscarRetornoFilial(int cdentifilial, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISBUSCARETFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        //public int NotaFiscalAtualizarStatus(int CdNotaFiscalSaida, int StatusNFe, string erroValidacao = "")
        //{
        //    List<OracleParameter> lstParam = new List<OracleParameter>();
        //    lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", CdNotaFiscalSaida));
        //    lstParam.Add(new OracleParameter("P_CDNFESTATUS", StatusNFe));
        //    lstParam.Add(new OracleParameter("P_TXNFEERRO", OracleDbType.Clob, erroValidacao, ParameterDirection.Input));
        //    //P_TXNFEERRO IN NOTAFISCALSAIDA.txnfeerro%TYPE

        //    return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_NOTASFISCAISATUALIZAR", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus);
        //}
        /// <summary>
        /// Forma de emissão da NF-e
        ///1 - Normal;
        ///2 - Contingência FS
        ///3 - Contingência SCAN
        ///4 - Contingência DPEC
        ///5 - Contingência FSDA
        ///6 - Contingência SVC - AN
        ///7 - Contingência SVC - RS
        ///9 - Contingência off-line NFC-e
        /// </summary>
        /// <param name="CdNotaFiscalSaida"></param>
        /// <param name="StatusNFe"></param>
        /// <param name="NrChaveNfeDpec"></param>
        /// <param name="datasource"></param>
        /// <param name="schema"></param>
        /// <param name="erroValidacao"></param>
        /// <returns></returns>
        public int NotaFiscalAtualizarStatus(int CdNotaFiscalSaida, int StatusNFe, string NrChaveNfeDpec, string datasource, string schema, string erroValidacao = "")
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", CdNotaFiscalSaida));
            lstParam.Add(new OracleParameter("P_CDNFESTATUS", StatusNFe));
            lstParam.Add(new OracleParameter("P_NRCHAVEDPEC", NrChaveNfeDpec));
            lstParam.Add(new OracleParameter("P_TXNFEERRO", OracleDbType.Clob, erroValidacao, ParameterDirection.Input));

            /*
                 P_CDNOTAFISCALSAIDA IN NOTAFISCALSAIDA.CDNOTAFISCALSAIDA%TYPE,
    P_CDNFESTATUS IN NOTAFISCALSAIDA.CDNFESTATUS%TYPE,
    P_NRCHAVEDPEC IN  NOTAFISCALSAIDA.NRNFECHAVEDPEC%TYPE,
    P_TXNFEERRO in notafiscalsaida.txnfeerro%TYPE
             */


            return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_NOTASFISCAISATUALIZAR", lstParam, CommandType.StoredProcedure, datasource, schema);
        }


        public enum TipoNFe
        {
            Inclusao,
            Cancelamento
        }

        //public int NotaFiscalAtualizarNota(
        //    int CdNotaFiscalSaida,
        //    string xmlNFe,
        //    string xmlAutorizacao,
        //    string autMotivo,
        //    string chaveNfe,
        //    string NrNota,
        //    string NrSerie,
        //    string dtEmissaoNf,
        //    string TipoPessoa,
        //    string EMI_CNPJ,
        //    string CLI_CPF_CNPJ,
        //    string dtRetornoSefaz,
        //    string NumeroProtocolo,
        //    string StatusNfe,
        //    string xmlCancelamento,
        //    string xmlAutCancelamento,
        //    TipoNFe tpnfe)
        //{
        //    StringBuilder NotaFiscalSaida = new StringBuilder();
        //    List<string> partes = new List<string>();


        //    string TipoEmissao = "1"; //1 normal 4 depec

        //    string texto = Convert.ToString(xmlNFe);
        //    int quantidadeTotal = texto.Length;
        //    int quantidadePermitida = 30000;
        //    int quantidadePartes = quantidadeTotal / quantidadePermitida;
        //    int quantidadeRestante = quantidadeTotal % quantidadePermitida;

        //    if (quantidadeTotal > quantidadePermitida)
        //    {
        //        for (int i = 0; i < quantidadePartes; i++)
        //            partes.Add(texto.Substring(i == 0 ? i : (i * quantidadePermitida) + 1, quantidadePermitida));
        //        if (quantidadeRestante > 0)
        //            partes.Add(texto.Substring(quantidadeTotal - quantidadeRestante, quantidadeRestante));
        //    }
        //    else
        //    {
        //        partes.Add(xmlNFe);
        //    }

        //    NotaFiscalSaida.Append("DECLARE ");
        //    if (tpnfe == TipoNFe.Inclusao)
        //    {
        //        NotaFiscalSaida.Append(" Vtxfilenfe Clob; ");
        //        NotaFiscalSaida.Append(" Vtxfilenfeaut Clob; ");
        //        NotaFiscalSaida.Append(" Vtxnferetorno Clob; ");
        //    }
        //    else
        //    {
        //        NotaFiscalSaida.Append(" Vtxfilenfecanc Clob; ");

        //    }

        //    NotaFiscalSaida.Append(" Vtxfilenfeoutros Clob; ");

        //    NotaFiscalSaida.Append("BEGIN ");

        //    if (tpnfe == TipoNFe.Inclusao)
        //    {

        //        /*************************************************Juntando as partes para executar query***********************************************************************/
        //        for (int i = 0; i < partes.Count; i++)
        //        {
        //            if (i == 0)
        //                NotaFiscalSaida.Append("    Vtxfilenfe := to_Clob('" + partes[i] + "');");
        //            else
        //                NotaFiscalSaida.Append("    DBMS_LOB.writeappend (Vtxfilenfe,LENGTH ('" + partes[i] + "'), '" + partes[i] + "');");
        //        }
        //        /**************************************************************************************************************************************************************/

        //        NotaFiscalSaida.Append("    Vtxfilenfeaut := To_Clob('" + xmlAutorizacao + "');");
        //        NotaFiscalSaida.Append("    Vtxnferetorno := To_Clob('" + autMotivo + "');  ");
        //    }
        //    else
        //    {
        //        NotaFiscalSaida.Append("    Vtxfilenfecanc := To_Clob('" + xmlCancelamento + "');");
        //    }

        //    //NotaFiscalSaida.Append("    Vtxfilenfeoutros := To_Clob('" + Convert.ToString(Retorno["XML_DADOSADIC"]) + "');");


        //    NotaFiscalSaida.Append("    Update Notafiscalsaida SET ");
        //    if (tpnfe == TipoNFe.Inclusao)
        //    {
        //        NotaFiscalSaida.Append("    Txfilenfe = Vtxfilenfe, ");
        //        NotaFiscalSaida.Append("    Txfilenfeaut = Vtxfilenfeaut, ");
        //        NotaFiscalSaida.Append("    Nrnfechave = '" + chaveNfe + "', ");
        //        NotaFiscalSaida.Append("    CDNFETIPOEMISSAO = '" + TipoEmissao + "', ");
        //        NotaFiscalSaida.Append("    Tsnfeautorizacao = To_Date('" + dtRetornoSefaz + "', 'DD/MM/YYYY HH24:MI:SS'), ");
        //        NotaFiscalSaida.Append("    Nrnfeautorizacao = '" + NumeroProtocolo + "', ");
        //        NotaFiscalSaida.Append("    Txnferetorno = Vtxnferetorno ,");
        //    }
        //    else
        //    {
        //        NotaFiscalSaida.Append("    Txfilenfecanc = Vtxfilenfecanc, ");
        //        if (StatusNfe == "100")
        //        {
        //            NotaFiscalSaida.Append("    stnfsaida = 0, ");
        //        }

        //    }

        //    //NotaFiscalSaida.Append("    Txfilenfeoutros = Vtxfilenfeoutros, ");
        //    NotaFiscalSaida.Append("    Cdnfestatus = '" + StatusNfe + "'");
        //    //NotaFiscalSaida.Append(" Where Nrnota = '" + NrNota + "' ");
        //    NotaFiscalSaida.Append(" Where ");
        //    //NotaFiscalSaida.Append(" And Cdserie = '" + NrSerie + "' ");
        //    NotaFiscalSaida.Append(" Cdnotafiscalsaida = " + CdNotaFiscalSaida + ";  ");
        //    //NotaFiscalSaida.Append(" And trunc(DTEMISSAO) = To_Date('" + dtEmissaoNf + "','DD/MM/YYYY') ");

        //    //if (TipoPessoa.Equals("J"))
        //    //    NotaFiscalSaida.Append("And CDENTIDADE = (select max(Cdentipj) from pessjuridica pj, entidade e where pj.cdentipj = e.cdentidade and e.stexcluido = 0 and pj.cnpj = '" + CLI_CPF_CNPJ + "') ");
        //    //else
        //    //    NotaFiscalSaida.Append("And CDENTIDADE = (select max(CDENTIPF) from pessfisica pf, entidade e where pf.cdentipf = e.cdentidade and e.stexcluido = 0 and pf.cpf = '" + CLI_CPF_CNPJ + "' ) ");

        //    //NotaFiscalSaida.Append("And Cdentifilial = (select max(Cdentipj) from pessjuridica pj, entidade e where pj.cdentipj = e.cdentidade and e.stexcluido = 0 and pj.cnpj = '" + EMI_CNPJ + "'); ");

        //    NotaFiscalSaida.Append(" END;");

        //    //RJS.Optimus.Biblioteca.RJSOptimusLog.GravaLogEventViewer("NFE OPTIMUS QUERY", NotaFiscalSaida.ToString(), System.Diagnostics.EventLogEntryType.Information);
        //    string aaaa = NotaFiscalSaida.ToString();
        //    return OracleHelper.ExecProcedureNonQuery(NotaFiscalSaida.ToString(), null, OracleHelper.BancoOracle.optimus);

        //}

        public int NotasFiscalSaidaAtualizarProc(
            int _CDNOTAFISCALSAIDA,
            string _XMLNFE,
            string _XMLAUTORIZACAO,
            string _AUTMOTIVO,
            DateTime _DTRETORNOSEFAZ,
            string _NUMEROPROTOCOLO,
            string _STATUSNFE,
            string _XMLCANCELAMENTO,
            string _XMLAUTCANCELAMENTO,
            string _datasource,
            string _schema
          )
        {

            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", _CDNOTAFISCALSAIDA));
            lstParam.Add(new OracleParameter("P_TXFILENFE", OracleDbType.Clob, _XMLNFE, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_TXFILENFEAUT", OracleDbType.Clob, _XMLAUTORIZACAO, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_TXNFERETORNO", _AUTMOTIVO));
            lstParam.Add(new OracleParameter("P_NRNFEAUTORIZACAO", _NUMEROPROTOCOLO));
            lstParam.Add(new OracleParameter("P_TSNFEAUTORIZACAO", OracleDbType.Date, _DTRETORNOSEFAZ, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_TXFILENFECANC", OracleDbType.Clob, _XMLCANCELAMENTO, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_TXFILENFECANCAUT", OracleDbType.Clob, _XMLAUTCANCELAMENTO, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_CDNFESTATUS", _STATUSNFE));
            return Convert.ToInt32(OracleHelper.ExecProcedureScalarTrans("PK_NFE.SP_ATUALIZANOTASAIDA", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus, _datasource, _schema));
        }

        //public int NotasFiscalEntradaInsert(NFE _NFE)
        //{
        //    List<OracleParameter> lstParam = new List<OracleParameter>();
        //    lstParam.Add(new OracleParameter("XML_NFE", OracleDbType.Clob, _NFE.XML_NFE, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("XML_AUT", OracleDbType.Clob, _NFE.XML_AUT, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("XML_CANC", OracleDbType.Clob, _NFE.XML_CANC, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("XML_DADOSADIC", OracleDbType.Clob, _NFE.XML_DADOSADIC, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("IDE_SERIE", _NFE.IDE_SERIE));
        //    lstParam.Add(new OracleParameter("IDE_NNF", _NFE.IDE_NNF));
        //    lstParam.Add(new OracleParameter("IDE_DEMI", OracleDbType.Date, _NFE.IDE_DEMI, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("EMIT_CNPJ", _NFE.EMIT_CNPJ));
        //    lstParam.Add(new OracleParameter("DEST_CNPJ", _NFE.DEST_CNPJ));
        //    lstParam.Add(new OracleParameter("DEST_CPF", _NFE.DEST_CPF));
        //    lstParam.Add(new OracleParameter("IDE_CUF", _NFE.IDE_CUF));
        //    lstParam.Add(new OracleParameter("IDE_MOD", _NFE.IDE_MOD));
        //    lstParam.Add(new OracleParameter("IDE_CNF", _NFE.IDE_CNF));
        //    lstParam.Add(new OracleParameter("EMIT_IE", _NFE.EMIT_IE));
        //    lstParam.Add(new OracleParameter("DEST_IE", _NFE.DEST_IE));
        //    lstParam.Add(new OracleParameter("DEST_ENDERDEST_UF", _NFE.DEST_ENDERDEST_UF));
        //    lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VBC", _NFE.TOTAL_ICMSTOT_VBC));
        //    lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VBCST", _NFE.TOTAL_ICMSTOT_VBCST));
        //    lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VPROD", _NFE.TOTAL_ICMSTOT_VPROD));
        //    lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VICMS", _NFE.TOTAL_ICMSTOT_VICMS));
        //    lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VST", _NFE.TOTAL_ICMSTOT_VST));
        //    lstParam.Add(new OracleParameter("TOTAL_ISSQNTOT_VBC", _NFE.TOTAL_ISSQNTOT_VBC));
        //    lstParam.Add(new OracleParameter("TRANSP_TRANSPORTA_CNPJ", _NFE.TRANSP_TRANSPORTA_CNPJ));
        //    lstParam.Add(new OracleParameter("TRANSP_IE", _NFE.TRANSP_IE));
        //    lstParam.Add(new OracleParameter("TRANSP_VEICTRANSP_PLACA", _NFE.TRANSP_VEICTRANSP_PLACA));
        //    lstParam.Add(new OracleParameter("IDE_ID", _NFE.IDE_ID));
        //    lstParam.Add(new OracleParameter("IDE_TPEMIS", _NFE.IDE_TPEMIS));
        //    lstParam.Add(new OracleParameter("STATUSNFE", _NFE.STATUSNFE));
        //    lstParam.Add(new OracleParameter("DOCSEQUENCE", _NFE.DOCSEQUENCE));
        //    lstParam.Add(new OracleParameter("IDEID1", _NFE.IDEID1));
        //    lstParam.Add(new OracleParameter("IDEID2", _NFE.IDEID2));
        //    lstParam.Add(new OracleParameter("IDEID3", _NFE.IDEID3));
        //    lstParam.Add(new OracleParameter("IDE_ID_RELATED", _NFE.IDE_ID_RELATED));
        //    return Convert.ToInt32(OracleHelper.ExecProcedureScalarTrans("PK_NFE.SP_INSERT_NFE", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus));
        //}

        public int NotasFiscalEntradaInsert(NFE _NFE, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("XML_NFE", OracleDbType.Clob, _NFE.XML_NFE, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("XML_AUT", OracleDbType.Clob, _NFE.XML_AUT, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("XML_CANC", OracleDbType.Clob, _NFE.XML_CANC, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("XML_DADOSADIC", OracleDbType.Clob, _NFE.XML_DADOSADIC, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("IDE_SERIE", _NFE.IDE_SERIE));
            lstParam.Add(new OracleParameter("IDE_NNF", _NFE.IDE_NNF));
            lstParam.Add(new OracleParameter("IDE_DEMI", OracleDbType.Date, _NFE.IDE_DEMI, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("EMIT_CNPJ", _NFE.EMIT_CNPJ));
            lstParam.Add(new OracleParameter("DEST_CNPJ", _NFE.DEST_CNPJ));
            lstParam.Add(new OracleParameter("DEST_CPF", _NFE.DEST_CPF));
            lstParam.Add(new OracleParameter("IDE_CUF", _NFE.IDE_CUF));
            lstParam.Add(new OracleParameter("IDE_MOD", _NFE.IDE_MOD));
            lstParam.Add(new OracleParameter("IDE_CNF", _NFE.IDE_CNF));
            lstParam.Add(new OracleParameter("EMIT_IE", _NFE.EMIT_IE));
            lstParam.Add(new OracleParameter("DEST_IE", _NFE.DEST_IE));
            lstParam.Add(new OracleParameter("DEST_ENDERDEST_UF", _NFE.DEST_ENDERDEST_UF));
            lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VBC", _NFE.TOTAL_ICMSTOT_VBC));
            lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VBCST", _NFE.TOTAL_ICMSTOT_VBCST));
            lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VPROD", _NFE.TOTAL_ICMSTOT_VPROD));
            lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VICMS", _NFE.TOTAL_ICMSTOT_VICMS));
            lstParam.Add(new OracleParameter("TOTAL_ICMSTOT_VST", _NFE.TOTAL_ICMSTOT_VST));
            lstParam.Add(new OracleParameter("TOTAL_ISSQNTOT_VBC", _NFE.TOTAL_ISSQNTOT_VBC));
            lstParam.Add(new OracleParameter("TRANSP_TRANSPORTA_CNPJ", _NFE.TRANSP_TRANSPORTA_CNPJ));
            lstParam.Add(new OracleParameter("TRANSP_IE", _NFE.TRANSP_IE));
            lstParam.Add(new OracleParameter("TRANSP_VEICTRANSP_PLACA", _NFE.TRANSP_VEICTRANSP_PLACA));
            lstParam.Add(new OracleParameter("IDE_ID", _NFE.IDE_ID));
            lstParam.Add(new OracleParameter("IDE_TPEMIS", _NFE.IDE_TPEMIS));
            lstParam.Add(new OracleParameter("STATUSNFE", _NFE.STATUSNFE));
            lstParam.Add(new OracleParameter("DOCSEQUENCE", _NFE.DOCSEQUENCE));
            lstParam.Add(new OracleParameter("IDEID1", _NFE.IDEID1));
            lstParam.Add(new OracleParameter("IDEID2", _NFE.IDEID2));
            lstParam.Add(new OracleParameter("IDEID3", _NFE.IDEID3));
            lstParam.Add(new OracleParameter("IDE_ID_RELATED", _NFE.IDE_ID_RELATED));
            return Convert.ToInt32(OracleHelper.ExecProcedureScalarTrans("PK_NFE.SP_INSERT_NFE", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus, datasource, schema));
        }

        public DataTable NotaFiscalEnviarEmail(string datasource, string schema)
        {
            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISENVIAREMAIL", null, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        public DataTable NotaFiscalEnviarEmailFilial(int cdentifilial, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_NOTASFISCAISENVEMAILFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        public int NotaFiscalUpdateEmailEnviado(int CdNotaFiscalSaida, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", CdNotaFiscalSaida));
            return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_NOTASFISCAISATUASTATUSEMAIL", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        public int NotaFiscalUpdateEmailEnviado(int CdNotaFiscalSaida, int status, string datasource, string schema)
        {
            string query = string.Format("UPDATE NOTAFISCALSAIDA SET STEMAILENVIADO = {0} ,TSEMAILENVIADO = SYSDATE  WHERE CDNOTAFISCALSAIDA ={1}", status, CdNotaFiscalSaida);
            return OracleHelper.ExecProcedureNonQuery(query, null, CommandType.Text, datasource, schema);
        }
        //public int NotaFiscalEntradaValidar(string P_DEST_CNPJ, string P_EMIT_CNPJ, int P_IDE_NNF, int P_IDE_SERIE, DateTime P_IDE_DEMI)
        //{

        //    List<OracleParameter> lstParam = new List<OracleParameter>();
        //    lstParam.Add(new OracleParameter("P_DEST_CNPJ", P_DEST_CNPJ));
        //    lstParam.Add(new OracleParameter("P_EMIT_CNPJ", P_EMIT_CNPJ));
        //    lstParam.Add(new OracleParameter("P_IDE_NNF", P_IDE_NNF));
        //    lstParam.Add(new OracleParameter("P_IDE_SERIE", P_IDE_SERIE));
        //    lstParam.Add(new OracleParameter("P_IDE_DEMI", P_IDE_DEMI));
        //    lstParam.Add(new OracleParameter("P_COUNT", OracleDbType.Decimal, ParameterDirection.Output));

        //    return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_VALIDA_NFE", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus, "P_COUNT");
        //}

        public int NotaFiscalEntradaValidar(string P_DEST_CNPJ, string P_EMIT_CNPJ, int P_IDE_NNF, int P_IDE_SERIE, DateTime P_IDE_DEMI, string datasource, string schema)
        {

            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_DEST_CNPJ", P_DEST_CNPJ));
            lstParam.Add(new OracleParameter("P_EMIT_CNPJ", P_EMIT_CNPJ));
            lstParam.Add(new OracleParameter("P_IDE_NNF", P_IDE_NNF));
            lstParam.Add(new OracleParameter("P_IDE_SERIE", P_IDE_SERIE));
            lstParam.Add(new OracleParameter("P_IDE_DEMI", P_IDE_DEMI));
            lstParam.Add(new OracleParameter("P_COUNT", OracleDbType.Decimal, ParameterDirection.Output));

            return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_VALIDA_NFE", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus, "P_COUNT", datasource, schema);
        }



        /// <summary>
        /// Grava o log no banco do DOC-e
        /// </summary>
        /// <param name="txlog">log workflow</param>
        /// <param name="nrtempoexecucao">tempo gasto na execução</param>
        /// <param name="txexception">exception caso haja</param>
        /// <param name="nmservico">nome do servico.método</param>
        /// <param name="cnpjcliente">cnpj do cliente que esta usando o serviço</param>
        /// <returns></returns>
        //public int InserirLogDOCe(string txlog, double nrtempoexecucao, string txexception, string nmservico, string nmmetodo, decimal cnpjcliente, string datasource, string schema)
        //{
        //    List<OracleParameter> lstParam = new List<OracleParameter>();
        //    lstParam.Add(new OracleParameter("P_TXLOG", OracleDbType.Clob, txlog, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("P_NRTEMPOEXECUCAO", nrtempoexecucao));
        //    lstParam.Add(new OracleParameter("P_TXEXCEPTION", OracleDbType.Clob, txexception, ParameterDirection.Input));
        //    lstParam.Add(new OracleParameter("P_NMSERVICO", nmservico));
        //    lstParam.Add(new OracleParameter("P_CNPJCLIENTE", cnpjcliente));
        //    lstParam.Add(new OracleParameter("P_NMMETODO", nmmetodo));
        //    return OracleHelper.ExecProcedureNonQuery("PK_NFELOG.SP_INSERT_LOG", lstParam, CommandType.StoredProcedure, datasource, schema);
        //}

        public int InserirHistoricoNFE(string cnpj, string nrnota, string serie, string cdnotafiscalsaida, string cdentifilial,
                                        string dtemissao, string cdstatusnfe, string xmlnfeenv, string xmlnfeaut, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CNPJ", cnpj));
            lstParam.Add(new OracleParameter("P_NRNOTA", nrnota));
            lstParam.Add(new OracleParameter("P_SERIE", serie));
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", cdnotafiscalsaida));
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));
            lstParam.Add(new OracleParameter("P_DTEMISSAO", OracleDbType.Date, Convert.ToDateTime(dtemissao, new CultureInfo("pt-BR")), ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_CDSTATUSNFE", cdstatusnfe));
            lstParam.Add(new OracleParameter("P_XMLNFEENV", OracleDbType.Clob, xmlnfeenv, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_XMLNFEAUT", OracleDbType.Clob, xmlnfeaut, ParameterDirection.Input));

            return OracleHelper.ExecProcedureNonQuery("pk_HISTORICONFE.SP_INSERT_HIST", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        public int InserirHistoricoItem(string xmlretorno, string nrprotocolo, string cdstatsefaz, string txdescsefaz, string cdnotafiscalsaida, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_XMLRETORNO", OracleDbType.Clob, xmlretorno, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_NRPROTOCOLO", nrprotocolo));
            lstParam.Add(new OracleParameter("P_CDSTATSEFAZ", cdstatsefaz));
            lstParam.Add(new OracleParameter("P_TXDESCSEFAZ", txdescsefaz));
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", cdnotafiscalsaida));
            return OracleHelper.ExecProcedureNonQuery("pk_HISTORICONFE.SP_INSERT_HISTITENS", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        //SP_UPDATE_HIST
        public int UpdateHistorico(string xmlaut, string cdnotafiscalsaida, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", cdnotafiscalsaida));
            lstParam.Add(new OracleParameter("P_XMLNFEAUT", OracleDbType.Clob, xmlaut, ParameterDirection.Input));

            return OracleHelper.ExecProcedureNonQuery("pk_HISTORICONFE.SP_UPDATE_HIST", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        //SP_UPDATE_HIST
        public int UpdateHistoricoCanc(string xmlcanc, string xmlcancaut, string cdnotafiscalsaida, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", cdnotafiscalsaida));
            lstParam.Add(new OracleParameter("P_XMLCANC", OracleDbType.Clob, xmlcanc, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_XMLCANCAUT", OracleDbType.Clob, xmlcancaut, ParameterDirection.Input));

            return OracleHelper.ExecProcedureNonQuery("pk_HISTORICONFE.SP_UPDATE_HISTCANC", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        /// <summary>
        /// Atualiza o status da NFE
        /// </summary>
        /// <param name="cdnotafiscalsaida"></param>
        /// <param name="CdStatusNfe"></param>
        /// <returns></returns>
        public int UpdateStatusNf(string cdnotafiscalsaida, string CdStatusNfe, string datasource, string schema)
        {
            string query = string.Format("UPDATE NOTAFISCALSAIDA SET CDNFESTATUS = {0} WHERE CDNOTAFISCALSAIDA = {1}", CdStatusNfe, cdnotafiscalsaida);

            return OracleHelper.ExecProcedureNonQuery(query, null, CommandType.Text, datasource, schema);
        }

        //public DataTable CartaCorrecaoIntegracao()
        //{
        //    DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_CARTACORRECAOINTEGRAR", null, "P_CURSOR", OracleHelper.BancoOracle.optimus);
        //    return ds.Tables[0];
        //}

        public DataTable CartaCorrecaoIntegracao(string datasource, string schema)
        {
            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_CARTACORRECAOINTEGRAR", null, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        public DataTable CartaCorrecaoIntegracaoFilial(int cdentifilial, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_CARTACORRECAOINTEGRARFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        public int UpdateStatusCC(int cdnotafiscalsaidaCC, int cdstatus, int cdstatussefaz, string datasource, string schema)
        {
            string query = string.Format("UPDATE NOTAFISCALSAIDACC SET CDSTATUS = {0} , CDSTATUSsefaz = {1} WHERE CDNOTAFISCALSAIDACC = {2}", cdstatus, cdstatussefaz, cdnotafiscalsaidaCC);

            return OracleHelper.ExecProcedureNonQuery(query, null, CommandType.Text, datasource, schema);
        }

        public int CartaCorrecaoUpdate(int CDNOTAFISCALSAIDACC, int CDNFESTATUS, int CDNFESTATUSSEFAZ, string XMLRETORNO, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDACC", CDNOTAFISCALSAIDACC));
            lstParam.Add(new OracleParameter("P_CDNFESTATUS", CDNFESTATUS));
            lstParam.Add(new OracleParameter("P_CDNFESTATUSSEFAZ", CDNFESTATUSSEFAZ));
            lstParam.Add(new OracleParameter("P_XMLRETORNO", OracleDbType.Clob, XMLRETORNO, ParameterDirection.Input));

            return Convert.ToInt32(OracleHelper.ExecProcedureScalarTrans("PK_NFE.SP_CARTACORRECAOATUALIZAR", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus, datasource, schema));
        }

        //public int UpdateStatusNfStNfsaida(string cdnotafiscalsaida, string CdStatusNfe, string StNfesaida)
        //{
        //    string query = string.Format("UPDATE NOTAFISCALSAIDA SET CDNFESTATUS = {0}, STNFSAIDA={1} WHERE CDNOTAFISCALSAIDA = {2}", CdStatusNfe, StNfesaida, cdnotafiscalsaida);

        //    return OracleHelper.ExecProcedureNonQuery(query, null, OracleHelper.BancoOracle.optimus);
        //}


        public List<string> RetornaEmailTransportadora(string pCNJP, string datasource, string schema)
        {
            List<string> lstRetorno = new List<string>();

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT PJ.CNPJ, CE.TXEMAIL ");
            sql.AppendLine(" FROM ENTIDADE E, PESSFISICA PF, PESSJURIDICA PJ , TRANSPORTADOR T ,ENTIENDERECO EE , CONTATO C , CONTEMAIL CE ");
            sql.AppendLine(" WHERE PF.CDENTIPF(+) = E.CDENTIDADE ");
            sql.AppendLine(" AND PJ.CDENTIPJ(+) = E.CDENTIDADE ");
            sql.AppendLine(" AND E.CDENTIDADE = T.CDENTITRANSPORTADOR ");
            sql.AppendLine(" AND E.CDENTIDADE        = EE.CDENTIDADE(+) ");
            sql.AppendLine(" AND E.CDENTIDADE = C.CDENTIDADE(+)");
            sql.AppendLine(" AND EE.STPRINCIPAL(+)   = 1      ");
            sql.AppendLine(" AND C.CDCONTATO = CE.CDCONTATO");
            sql.AppendLine(" AND CE.CDTIPOEMAIL = 3 ");
            sql.AppendLine(string.Format(" AND PJ.CNPJ ='{0}' ", pCNJP));

            DataTable dt = OracleHelper.ExecSql(sql.ToString(), OracleHelper.BancoOracle.optimus, datasource, schema).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                lstRetorno.Add(dr["TXEMAIL"].ToString());
            }

            return lstRetorno;
        }


        #region INUTI
        /*
         PROCEDURE SP_INUTILIZACAOINTEGRAR ( P_CURSOR out SYS_REFCURSOR);
PROCEDURE SP_INUTILIZACAOINTEGRARFILIAL
   (
    P_CURSOR out SYS_REFCURSOR ,
    P_CDENTIFILIAL IN NOTAFISCALINUTNUMERACAO.CDENTIFILIAL%TYPE
   );
 PROCEDURE SP_INUTILIZACAOATUALIZAR
(
         P_CDNOTAFISCALINUTNUMERACAO IN NOTAFISCALINUTNUMERACAO.CDNOTAFISCALINUTNUMERACAO%TYPE,
         P_CDNFESTATUS IN NOTAFISCALINUTNUMERACAO.CDSTATUS%TYPE,
         P_CDNFESTATUSSEFAZ IN NOTAFISCALINUTNUMERACAO.CDSTATUSSEFAZ%TYPE,
         P_XMLRETORNO IN NOTAFISCALINUTNUMERACAO.XMLRETORNO%TYPE
) ; 
         */

        public DataTable InutilizacaoIntegracao(string datasource, string schema)
        {
            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_INUTILIZACAOINTEGRAR", null, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        public DataTable InutilizacaoIntegracaoFilial(int cdentifilial, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDENTIFILIAL", cdentifilial));

            DataSet ds = OracleHelper.ExecProcedure("PK_NFE.SP_INUTILIZACAOINTEGRARFILIAL", lstParam, "P_CURSOR", OracleHelper.BancoOracle.optimus, datasource, schema);
            return ds.Tables[0];
        }

        public int UpdateStatusInutilizacao(int CDNOTAFISCALINUTNUMERACAO, int cdstatus, int cdstatussefaz, string datasource, string schema)
        {
            string query = string.Format("UPDATE NOTAFISCALINUTNUMERACAO SET CDSTATUS = {0} , CDSTATUSsefaz = {1} WHERE CDNOTAFISCALINUTNUMERACAO = {2}", cdstatus, cdstatussefaz, CDNOTAFISCALINUTNUMERACAO);

            return OracleHelper.ExecProcedureNonQuery(query, null, CommandType.Text, datasource, schema);
        }

        public int InutilizacaoUpdate(int CDNOTAFISCALINUTNUMERACAO, int CDNFESTATUS, int CDNFESTATUSSEFAZ, string XMLRETORNO, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALINUTNUMERACAO", CDNOTAFISCALINUTNUMERACAO));
            lstParam.Add(new OracleParameter("P_CDNFESTATUS", CDNFESTATUS));
            lstParam.Add(new OracleParameter("P_CDNFESTATUSSEFAZ", CDNFESTATUSSEFAZ));
            lstParam.Add(new OracleParameter("P_XMLRETORNO", OracleDbType.Clob, XMLRETORNO, ParameterDirection.Input));

            return Convert.ToInt32(OracleHelper.ExecProcedureScalarTrans("PK_NFE.SP_INUTILIZACAOATUALIZAR", lstParam, CommandType.StoredProcedure, OracleHelper.BancoOracle.optimus, datasource, schema));
        }

        #endregion

        /*
         PROCEDURE SP_UPDATENOTANAOPROCESSADA
(
         P_NRNFECHAVE IN NOTAFISCALSAIDA.NRNFECHAVE%TYPE,
         P_TXFILENFE IN NOTAFISCALSAIDA.TXFILENFE%TYPE,
         P_TXFILENFEAUT IN NOTAFISCALSAIDA.TXFILENFEAUT%TYPE,
         P_TSNFEAUTORIZACAO IN NOTAFISCALSAIDA.TSNFEAUTORIZACAO%TYPE,
         P_NRNFEAUTORIZACAO IN NOTAFISCALSAIDA.NRNFEAUTORIZACAO%TYPE,
         P_TXNFERETORNO IN NOTAFISCALSAIDA.TXNFERETORNO%TYPE
         
)
         */

        public int NotaFiscalAtualizarNaoProcessada(string nrchavenfe, string txfilenfe, string txfilenfeaut, DateTime tsnfeautorizacao, string nrnfeautorizacao, string txnferetorno, string datasource, string schema)
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_NRNFECHAVE", nrchavenfe));
            lstParam.Add(new OracleParameter("P_TXFILENFE", OracleDbType.Clob, txfilenfe, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_TXFILENFEAUT", OracleDbType.Clob, txfilenfeaut, ParameterDirection.Input));
            lstParam.Add(new OracleParameter("P_TSNFEAUTORIZACAO", tsnfeautorizacao));
            lstParam.Add(new OracleParameter("P_NRNFEAUTORIZACAO", nrnfeautorizacao));
            lstParam.Add(new OracleParameter("P_TXNFERETORNO", txnferetorno));
            return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_UPDATENOTANAOPROCESSADA", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        public string GetXMLnotaLayout(int CDNOTAFISCALSAIDA, string datasource, string schema)
        {
            string query = string.Format("SELECT TXNDDLAYOUT FROM NOTAFISCALSAIDA WHERE CDNOTAFISCALSAIDA = {0}", CDNOTAFISCALSAIDA);

            string xml = string.Empty;

            DataTable dt = OracleHelper.ExecSql(query, OracleHelper.BancoOracle.optimus, datasource, schema).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                xml = dr["TXNDDLAYOUT"].ToString();
            }
            return xml;
        }

        #region GNRE
        public DataTable RetornaGNRE(string Cdentifilial, string multiplasfiliais, string datasource, string schema)
        {

            string sql = "select * from VW_NOTAFISCAL_GNRE";

            if (multiplasfiliais == "TRUE")
            {
                sql = sql + " WHERE CDENTIFILIAL = " + Cdentifilial;
            }

            DataTable dt = OracleHelper.ExecSql(sql.ToString(), OracleHelper.BancoOracle.optimus, datasource, schema).Tables[0];

            return dt;

        }

        public struct InfoUFICMS
        {
            public string urlgnre;
            public string cdReceitaGNRE;
            public string cdReceitaGNRECFP;
            public string cdCampoExtra;
        }

        public InfoUFICMS RetornaInfosUF(string uf, string datasource, string schema)
        {
            InfoUFICMS inforet = new InfoUFICMS();
            string sql = string.Format("SELECT URLGNRE,CDRECEITAGNREICMS,CDRECEITAGNREFCP,CDCAMPOEXTRA FROM OPTBDCOMUM.UF WHERE CDUF = '{0}'", uf);

            DataTable dt = OracleHelper.ExecSql(sql.ToString(), OracleHelper.BancoOracle.optimus, datasource, schema).Tables[0];

            if (dt.Rows.Count > 0)
            {
                inforet.urlgnre = dt.Rows[0]["URLGNRE"].ToString();
                inforet.cdReceitaGNRE = dt.Rows[0]["CDRECEITAGNREICMS"].ToString();
                inforet.cdReceitaGNRECFP = dt.Rows[0]["CDRECEITAGNREFCP"].ToString();
                inforet.cdCampoExtra = dt.Rows[0]["CDCAMPOEXTRA"].ToString();
            }

            return inforet;

        }

        /*
         PROCEDURE SP_GNREATUALIZAR
(
         P_CDNOTAFISCALSAIDA IN NOTAFISCALSAIDA.CDNOTAFISCALSAIDA%TYPE,
         P_PROTGNRE IN NOTAFISCALSAIDA.PROTGNRE%TYPE,
        P_DTAUTGNRE  IN NOTAFISCALSAIDA.DTAUTGNRE%TYPE,
        P_VRJUROSGNRE   IN NOTAFISCALSAIDA.VRJUROSGNRE%TYPE,
        P_VRMULTAGNRE  IN NOTAFISCALSAIDA.VRMULTAGNRE%TYPE,
        P_VRPRINCGNRE   IN NOTAFISCALSAIDA.VRPRINCGNRE%TYPE,
        P_CODBARRAGNRE  IN NOTAFISCALSAIDA.CODBARRAGNRE%TYPE,
        P_CDSTATUSGNRE  IN NOTAFISCALSAIDA.CDSTATUSGNRE%TYPE,
        P_TXMESANOREFEGNRE IN NOTAFISCALSAIDA.TXMESANOREFEGNRE%TYPE,
        P_CODREPRENUMEGNRE IN NOTAFISCALSAIDA.CODREPRENUMEGNRE%TYPE,
        P_TXINFOCOMPLEGNRE IN NOTAFISCALSAIDA.TXINFOCOMPLEGNRE%TYPE,
        P_CODRECEITAGNRE   IN NOTAFISCALSAIDA.CODRECEITAGNRE%TYPE
        P_TIPO_GNRE
);
         */


        public int NotaFiscalAtualizarGNRE(
            int cdnotafiscalsaida,
            string protgnre,
            DateTime dtautgnre,
            double vrjurosgnre,
            double vrmultagnre,
            double vrprincgnre,
            string codbarragnre,
            int cdstatusgnre,
            string txmesanorefegnre,
            string codreprenumegnre,
            string txinfocomplegnre,
            string codreceitagnre,
            string datasource,
            string schema,
            int P_TIPO_GNRE,
            double vratuamonetaria,
            DateTime dtlimitepagto
            )
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", cdnotafiscalsaida));
            lstParam.Add(new OracleParameter("P_PROTGNRE", protgnre));
            lstParam.Add(new OracleParameter("P_DTAUTGNRE", dtautgnre));
            lstParam.Add(new OracleParameter("P_VRJUROSGNRE", vrjurosgnre));
            lstParam.Add(new OracleParameter("P_VRMULTAGNRE", vrmultagnre));
            lstParam.Add(new OracleParameter("P_VRPRINCGNRE", vrprincgnre));
            lstParam.Add(new OracleParameter("P_CODBARRAGNRE", codbarragnre));
            lstParam.Add(new OracleParameter("P_CDSTATUSGNRE", cdstatusgnre));
            lstParam.Add(new OracleParameter("P_TXMESANOREFEGNRE", txmesanorefegnre));
            lstParam.Add(new OracleParameter("P_CODREPRENUMEGNRE", codreprenumegnre));
            lstParam.Add(new OracleParameter("P_TXINFOCOMPLEGNRE", txinfocomplegnre));
            lstParam.Add(new OracleParameter("P_CODRECEITAGNRE", codreceitagnre));
            lstParam.Add(new OracleParameter("P_TIPO_GNRE", P_TIPO_GNRE));
            lstParam.Add(new OracleParameter("P_VRATUAMONEGNRE", vratuamonetaria));
            lstParam.Add(new OracleParameter("P_DTLIMITEPAGTOGNRE", dtlimitepagto));

            return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_GNREATUALIZAR", lstParam, CommandType.StoredProcedure, datasource, schema);
        }


        public int NotaFiscalAtualizarGNREErro(
                int cdnotafiscalsaida,
                string txerro,
                string datasource,
                string schema,
            int P_TIPO_GNRE

    )
        {
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter("P_CDNOTAFISCALSAIDA", cdnotafiscalsaida));
            lstParam.Add(new OracleParameter("P_TXERROGNRE", txerro));
            lstParam.Add(new OracleParameter("P_TIPO_GNRE", P_TIPO_GNRE));
            return OracleHelper.ExecProcedureNonQuery("PK_NFE.SP_GNREATUALIZARERRO", lstParam, CommandType.StoredProcedure, datasource, schema);
        }

        public int NotafiscalAtualizarGNREStatusProt(int cdnotafiscalsaida, string numeroprotocolo, string datasource, string schema, int P_TIPO_GNRE)
        {
            string campoProtGnre = P_TIPO_GNRE == 1 ? "PROTGNRE" : "PROTGNREFCP";
            string campostatusGnre = P_TIPO_GNRE == 1 ? " ,CDSTATUSGNRE=1" : ",CDSTATUSGNREFCP=1";
            string query = string.Format("UPDATE NOTAFISCALSAIDA SET {0} = {1} {2} WHERE CDNOTAFISCALSAIDA ={3}", campoProtGnre, numeroprotocolo, campostatusGnre, cdnotafiscalsaida);
            return OracleHelper.ExecProcedureNonQuery(query, null, CommandType.Text, datasource, schema);
        }

        public int NotafiscalAtualizarGNREStatus(int cdnotafiscalsaida, int cdstatusgnre, string datasource, string schema, int P_TIPO_GNRE)
        {

            string campostatusGnre = P_TIPO_GNRE == 1 ? "CDSTATUSGNRE=" + cdstatusgnre.ToString() : "CDSTATUSGNREFCP=" + cdstatusgnre.ToString();
            string query = string.Format("UPDATE NOTAFISCALSAIDA SET {0} WHERE CDNOTAFISCALSAIDA ={1}", campostatusGnre, cdnotafiscalsaida);
            return OracleHelper.ExecProcedureNonQuery(query, null, CommandType.Text, datasource, schema);
        }

        #endregion
    }
}

