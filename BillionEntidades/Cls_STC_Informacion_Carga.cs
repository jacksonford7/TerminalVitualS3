using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_STC_Informacion_Carga : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _CODIGO_NDI;
        private string _NUMERO_CARGA = string.Empty;
        private string _CONSIGNATARIO_ID = string.Empty;
        private string _CONSIGNATARIO_DESC = string.Empty;
        private string _BUQUE = string.Empty;
        private string _DEPOSITO_TEMPORAL = string.Empty;
        private decimal? _PESO_MANIFESTADO;
        private string _NAVIERA = string.Empty;
        private DateTime? _FECHA_EST_ARRIBO = null;
        private int? _CANTIDAD_BULTOS;
        private string _ESTADO = string.Empty;
        private DateTime? _FECHA_REGISTRO = null;
        private string _BL = string.Empty;  
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private int _FILA;
        private string _CONTENEDOR = string.Empty;
        private string _TIPO = string.Empty;
        private string _DESCRIPCION = string.Empty;
        private decimal? _PESO;
        private string _SELLO1 = string.Empty;
        private string _SELLO2 = string.Empty;
        private string _SELLO3 = string.Empty;
        private string _CONDICION = string.Empty;
        private decimal? _BULTOS_DET;
        private string _TIPO_EMBALAJE = string.Empty;
        private string _MARCAS = string.Empty;
        private string _CHARACTERISTIC_CODE = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 CODIGO_NDI { get => _CODIGO_NDI; set => _CODIGO_NDI = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public string CONSIGNATARIO_ID { get => _CONSIGNATARIO_ID; set => _CONSIGNATARIO_ID = value; }
        public string CONSIGNATARIO_DESC { get => _CONSIGNATARIO_DESC; set => _CONSIGNATARIO_DESC = value; }
        public string BUQUE { get => _BUQUE; set => _BUQUE = value; }
        public string DEPOSITO_TEMPORAL { get => _DEPOSITO_TEMPORAL; set => _DEPOSITO_TEMPORAL = value; }
        public decimal? PESO_MANIFESTADO { get => _PESO_MANIFESTADO; set => _PESO_MANIFESTADO = value; }
        public string NAVIERA { get => _NAVIERA; set => _NAVIERA = value; }
        public DateTime? FECHA_EST_ARRIBO { get => _FECHA_EST_ARRIBO; set => _FECHA_EST_ARRIBO = value; }
        public int? CANTIDAD_BULTOS { get => _CANTIDAD_BULTOS; set => _CANTIDAD_BULTOS = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public DateTime? FECHA_REGISTRO { get => _FECHA_REGISTRO; set => _FECHA_REGISTRO = value; }
        public string BL { get => _BL; set => _BL = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public int FILA { get => _FILA; set => _FILA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }
        public decimal? PESO { get => _PESO; set => _PESO = value; }
        public string SELLO1 { get => _SELLO1; set => _SELLO1 = value; }
        public string SELLO2 { get => _SELLO2; set => _SELLO2 = value; }
        public string SELLO3 { get => _SELLO3; set => _SELLO3 = value; }
        public string CONDICION { get => _CONDICION; set => _CONDICION = value; }
        public decimal? BULTOS_DET { get => _BULTOS_DET; set => _BULTOS_DET = value; }
        public string TIPO_EMBALAJE { get => _TIPO_EMBALAJE; set => _TIPO_EMBALAJE = value; }
        public string MARCAS { get => _MARCAS; set => _MARCAS = value; }
        public string CHARACTERISTIC_CODE { get => _CHARACTERISTIC_CODE; set => _CHARACTERISTIC_CODE = value; }
        #endregion

        public Cls_STC_Informacion_Carga()
        {
            init();

        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("ADUANA");
        }

        public static List<Cls_STC_Informacion_Carga> Detalle(string MRN, string MSN, string HSN, string CONSIGNATARIO_ID, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("MRN", MRN);
            parametros.Add("MSN", MSN);
            parametros.Add("HSN", HSN);
            parametros.Add("CONSIGNATARIO_ID", CONSIGNATARIO_ID);
            return sql_puntero.ExecuteSelectControl<Cls_STC_Informacion_Carga>(nueva_conexion, 6000, "STC_INFORMACION_MANIFIESTOS", parametros, out OnError);

        }

       

    }
}
