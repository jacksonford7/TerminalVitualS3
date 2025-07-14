using BillionEntidades;
using CSLSite.peso_cls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace CSLSite.vgmCls
{
    public class Cls_VGM : Cls_Bil_Base
    {
        // Propiedades desde la API + las originales
        public long ukey { get; set; }
        public string cntr { get; set; }
        public DateTime? fecha { get; set; }
        public decimal? tara { get; set; }
        public decimal? peso { get; set; }
        public decimal? payload { get; set; }
        public decimal? goods_ctr_wt_kg_yard_measured { get; set; }
        public string nave { get; set; }
        public string referen { get; set; }
        public string export { get; set; }
        public string exportador_aisv { get; set; }
        public string NOMBRE_BUQUE { get; set; }
        public string VIAJE { get; set; }
        public bool? enviada { get; set; }
        public string correo { get; set; }
        public string ruc { get; set; }
        public string ci { get; set; }
        public string cm { get; set; }
        public string equipo { get; set; }
        public DateTime? fecha_reg { get; set; }
        public DateTime? fecha_mod { get; set; }
        public long codigo { get; set; }
        public string usuario_reg { get; set; }
        public string usuario_mod { get; set; }
        public string mail_cc { get; set; }
        public string line { get; set; }
        public string certificado { get; set; }
        public string secuencia { get; set; }



        // Constructor vacío
        public Cls_VGM() : base() { }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_VGM> ObtenerRegistroVGM(string gkey)
        {
            try
            {
                OnInit("APPCGSA");
                parametros.Clear();

                if (string.IsNullOrWhiteSpace(gkey))
                {
                    throw new ArgumentException("El parámetro id no puede ser nulo o vacío.");
                }


                parametros.Add("gkey", gkey);

                string controlError;

                var listaVGM = sql_puntero.ExecuteSelectControl<Cls_VGM>(
                    nueva_conexion,
                    4000,
                    "[pdf].[ObtenerRegistrosVGM_Gkey]",
                    parametros,
                    out controlError
                );

                return listaVGM;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar los registros VGM.", ex);
            }
        }
        public static List<Cls_peso_expo> ObtenerRegistroPeso(string gkey)
        {
            try
            {
                OnInit("N4Middleware");
                parametros.Clear();

                if (string.IsNullOrWhiteSpace(gkey))
                {
                    throw new ArgumentException("El parámetro id no puede ser nulo o vacío.");
                }


                parametros.Add("CONTENEDOR", gkey);

                string controlError;

                var listaVGM = sql_puntero.ExecuteSelectControl<Cls_peso_expo>(
                    nueva_conexion,
                    4000,
                    "sp_reporte_certificado_peso_tv",
                    parametros,
                    out controlError
                );

                return listaVGM;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar los registros VGM.", ex);
            }
        }
    }
}
