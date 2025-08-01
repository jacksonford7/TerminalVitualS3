﻿using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite.app_start
{
    public class SelloAsignaSealExpo : BillionEntidades.Cls_Bil_Base
    {
        public long ROW_ID { get; set; }
        public string CONTENEDOR { get; set; }
        public string SELLOCGSA { get; set; }
        public string SELLO1 { get; set; }
        public string SELLO2 { get; set; }
        public string SELLO3 { get; set; }
        public string SELLO4 { get; set; }
        public bool ESTADO { get; set; }
        public string IP { get; set; }
        public string MENSAJE { get; set; }
        public DateTime DATE { get; set; }
        public string USUARIO_CREA { get; set; }
        public long GKEY { get; set; }
        public List<SelloAsignaSealExpoFoto> fotos { get; set; }

        #region "Constructores"
        public SelloAsignaSealExpo()
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

        public static List<SelloAsignaSealExpo> ListSellos(DateTime? _desde, DateTime? _hasta, string _container)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_fechaDesde", _desde);
            parametros.Add("i_fechaHasta", _hasta);
            parametros.Add("i_contenedor", _container);
            return sql_puntero.ExecuteSelectControl<SelloAsignaSealExpo>(nueva_conexion, 2000, "seal.SELLO_ASSIGN_EXPO", parametros, out msg);
        }

        public static SelloAsignaSealExpo Sello(long _id, out string msg)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var resultado = sql_puntero.ExecuteSelectControl<SelloAsignaSealExpo>(nueva_conexion, 2000, "seal.SELLO_ASSIGN_EXPO_XID", parametros, out msg).FirstOrDefault();

            if (resultado != null)
            {
                if (resultado.ROW_ID > 0)
                {
                    resultado.fotos = SelloAsignaSealExpoFoto.ListFotosSellos(_id);
                }
            }

            return resultado;
        }
        #endregion
    }
    public class SelloAsignaSealExpoFoto : BillionEntidades.Cls_Bil_Base
    {
        public long id { get; set; }
        public long idSealAssignExpo { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime fechaModifica { get; set; }

        #region "Constructores"
        public SelloAsignaSealExpoFoto()
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

        public static List<SelloAsignaSealExpoFoto> ListFotosSellos(long _id)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectControl<SelloAsignaSealExpoFoto>(nueva_conexion, 2000, "seal.SELLO_ASSIGN_EXPO_FOTO_XID", parametros, out msg);
        }

        #endregion
    }
}