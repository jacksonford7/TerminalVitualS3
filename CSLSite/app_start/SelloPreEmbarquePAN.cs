﻿using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite.app_start
{
    public class SelloPreEmbarquePAN : BillionEntidades.Cls_Bil_Base
    {
        public long ROW_ID { get; set; }
        public string CONTENEDOR { get; set; }
        public string SELLOCGSA { get; set; }
        public string SELLO1 { get; set; }
        public string SELLO2 { get; set; }
        public string SELLO3 { get; set; }
        public bool ESTADO { get; set; }
        public string IP { get; set; }
        public string MENSAJE { get; set; }
        public DateTime DATE { get; set; }
        public string USUARIO_CREA { get; set; }
        public long GKEY { get; set; }
        public List<SelloPreEmbarqueFotoPAN> fotos { get; set; }

        #region "Constructores"
        public SelloPreEmbarquePAN()
        {

            base.init();
        }
        #endregion

        #region "Metodos"

        private static void OnInit(string Base)
        {
            //sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            //parametros = new Dictionary<string, object>();
            //v_conexion = Extension.Nueva_Conexion("RECEPTIO");

            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.CONTENEDOR))
            {
                msg = "Especifique el contenedor";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }

        public static List<SelloPreEmbarquePAN> ListSellos(DateTime? _desde, DateTime? _hasta, string _container)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_fechaDesde", _desde);
            parametros.Add("i_fechaHasta", _hasta);
            parametros.Add("i_contenedor", _container);
            return sql_puntero.ExecuteSelectControl<SelloPreEmbarquePAN>(nueva_conexion, 2000, "PNA_SELLO_PRE_EMBARQUE", parametros, out msg);
        }

        public static SelloPreEmbarquePAN Sello(long _id, out string msg)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var resultado = sql_puntero.ExecuteSelectControl<SelloPreEmbarquePAN>(nueva_conexion, 2000, "PNA_SELLO_PRE_EMBARQUE_XID", parametros, out msg).FirstOrDefault();

            if (resultado != null)
            {
                if (resultado.ROW_ID > 0)
                {
                    resultado.fotos = SelloPreEmbarqueFotoPAN.ListFotosSellos(_id);
                }
            }

            return resultado;
        }
        #endregion
    }
    public class SelloPreEmbarqueFotoPAN : BillionEntidades.Cls_Bil_Base
    {
        public long id { get; set; }
        public long idSealPreEmbarque { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime fechaModifica { get; set; }

        #region "Constructores"
        public SelloPreEmbarqueFotoPAN()
        {
            base.init();
        }
        #endregion

        #region "Metodos"

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<SelloPreEmbarqueFotoPAN> ListFotosSellos(long _id)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectControl<SelloPreEmbarqueFotoPAN>(nueva_conexion, 2000, "PNA_SELLO_PRE_EMBARQUE_FOTO_XID", parametros, out msg);
        }

        #endregion
    }
}