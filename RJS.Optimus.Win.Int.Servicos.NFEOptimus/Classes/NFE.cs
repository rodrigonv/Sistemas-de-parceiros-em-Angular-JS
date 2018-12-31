using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class NFE
    {
        public string IDE_SERIE { get; set; }//IDE_SERIE	NUMBER
        public int IDE_NNF { get; set; }//IDE_NNF	NUMBER
        public DateTime IDE_DEMI { get; set; }//IDE_DEMI	DATE

        public string eMIT_CNPJ; //EMIT_CNPJ	VARCHAR2(20 BYTE)
        public string EMIT_CNPJ
        {
            get { return eMIT_CNPJ; }
            set { if (value.Length > 20) eMIT_CNPJ = value.Substring(0, 20); else eMIT_CNPJ = value; }
        }

        public string dEST_CNPJ;//DEST_CNPJ	VARCHAR2(20 BYTE)
        public string DEST_CNPJ
        {
            get { return dEST_CNPJ; }
            set { if (value.Length > 20) dEST_CNPJ = value.Substring(0, 20); else dEST_CNPJ = value; }
        }


        public string dEST_CPF;//DEST_CPF	VARCHAR2(20 BYTE)
        public string DEST_CPF
        {
            get { return dEST_CPF; }
            set { if (value.Length > 20) dEST_CPF = value.Substring(0, 20); else dEST_CPF = value; }
        }

        public string iDE_CUF;//IDE_CUF	VARCHAR2(3 BYTE)
        public string IDE_CUF
        {
            get { return iDE_CUF; }
            set { if (value.Length > 3) iDE_CUF = value.Substring(0, 3); else iDE_CUF = value; }
        }

        public string iDE_MOD;//IDE_MOD	VARCHAR2(6 BYTE)
        public string IDE_MOD
        {
            get { return iDE_MOD; }
            set { if (value.Length > 6) iDE_MOD = value.Substring(0, 6); else iDE_MOD = value; }
        }

        public int IDE_CNF { get; set; }//IDE_CNF	NUMBER

        public string eMIT_IE;//EMIT_IE	VARCHAR2(14 BYTE)
        public string EMIT_IE
        {
            get { return eMIT_IE; }
            set { if (value.Length > 14) eMIT_IE = value.Substring(0, 14); else eMIT_IE = value; }
        }

        public string dEST_IE;//DEST_IE	VARCHAR2(20 BYTE)
        public string DEST_IE
        {
            get { return dEST_IE; }
            set { if (value.Length > 20) dEST_IE = value.Substring(0, 20); else dEST_IE = value; }
        }

        public string dEST_ENDERDEST_UF;//DEST_ENDERDEST_UF	VARCHAR2(2 BYTE)
        public string DEST_ENDERDEST_UF
        {
            get { return dEST_ENDERDEST_UF; }
            set { if (value.Length > 2) dEST_ENDERDEST_UF = value.Substring(0, 2); else dEST_ENDERDEST_UF = value; }
        }

        public decimal TOTAL_ICMSTOT_VBC { get; set; }//TOTAL_ICMSTOT_VBC	NUMBER
        public decimal TOTAL_ICMSTOT_VBCST { get; set; }//TOTAL_ICMSTOT_VBCST	NUMBER
        public decimal TOTAL_ICMSTOT_VPROD { get; set; }//TOTAL_ICMSTOT_VPROD	NUMBER
        public decimal TOTAL_ICMSTOT_VICMS { get; set; }//TOTAL_ICMSTOT_VICMS	NUMBER
        public decimal TOTAL_ICMSTOT_VST { get; set; }//TOTAL_ICMSTOT_VST	NUMBER
        public decimal TOTAL_ISSQNTOT_VBC { get; set; }//TOTAL_ISSQNTOT_VBC	NUMBER

        public string tRANSP_TRANSPORTA_CNPJ;//TRANSP_TRANSPORTA_CNPJ	VARCHAR2(20 BYTE)
        public string TRANSP_TRANSPORTA_CNPJ
        {
            get { return tRANSP_TRANSPORTA_CNPJ; }
            set { if (value.Length > 20) tRANSP_TRANSPORTA_CNPJ = value.Substring(0, 20); else tRANSP_TRANSPORTA_CNPJ = value; }
        }

        public string tRANSP_IE;//TRANSP_IE	VARCHAR2(14 BYTE)
        public string TRANSP_IE
        {
            get { return tRANSP_IE; }
            set { if (value.Length > 14) tRANSP_IE = value.Substring(0, 14); else tRANSP_IE = value; }
        }

        public string tRANSP_VEICTRANSP_PLACA;//TRANSP_VEICTRANSP_PLACA	VARCHAR2(10 BYTE)
        public string TRANSP_VEICTRANSP_PLACA
        {
            get { return tRANSP_VEICTRANSP_PLACA; }
            set { if (value.Length > 10) tRANSP_VEICTRANSP_PLACA = value.Substring(0, 10); else tRANSP_VEICTRANSP_PLACA = value; }
        }


        public string iDE_ID;//IDE_ID	VARCHAR2(47 BYTE)
        public string IDE_ID
        {
            get { return iDE_ID; }
            set { if (value.Length > 47) iDE_ID = value.Substring(0, 47); else iDE_ID = value; }
        }


        public string iDE_TPEMIS;//IDE_TPEMIS	VARCHAR2(6 BYTE)
        public string IDE_TPEMIS
        {
            get { return iDE_TPEMIS; }
            set { if (value.Length > 6) iDE_TPEMIS = value.Substring(0, 6); else iDE_TPEMIS = value; }
        }

        public int STATUSNFE { get; set; }//STATUSNFE NUMBER
        public string XML_NFE { get; set; }//XML_NFE LONG

        public string dOCSEQUENCE;//DOCSEQUENCE	VARCHAR2(20 BYTE)
        public string DOCSEQUENCE
        {
            get { return dOCSEQUENCE; }
            set { if (value.Length > 20) dOCSEQUENCE = value.Substring(0, 20); else dOCSEQUENCE = value; }
        }

        public string IDEID1 { get; set; }    //IDEID1	NUMBER
        public string IDEID2 { get; set; }    //IDEID2	NUMBER
        public string IDEID3 { get; set; }    //IDEID3	NUMBER

        public string iDE_ID_RELATED;//IDE_ID_RELATED	VARCHAR2(47 BYTE)
        public string IDE_ID_RELATED
        {
            get { return iDE_ID_RELATED; }
            set { if (value.Length > 47) iDE_ID_RELATED = value.Substring(0, 47); else iDE_ID_RELATED = value; }
        }

        //public DateTime DTENTRADAFISCAL { get; set; }    //DTENTRADAFISCAL	DATE
        //public DateTime DTENTRADAFISICA { get; set; }    //DTENTRADAFISICA	DATE
        //public int CDENTIDADE { get; set; }//CDENTIDADE	NUMBER
        //public int CDENTIFILIAL { get; set; }//CDENTIFILIAL	NUMBER

        public string XML_AUT { get; set; }//,[XML_AUT]
        public string XML_CANC { get; set; }//,[XML_CANC]
        public string XML_DADOSADIC { get; set; }//,[XML_DADOSADIC]
        public string RAZAOSOCIAL { get; set; }

        public string DEST_NOME { get; set; }

        public string CODPEDIDO { get; set; }
    }
}
