using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BreakBulk
{
    public class carrier : Cls_Bil_Base
    {
        #region "Propiedades"
        public string carrier_id { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        #endregion

        public carrier()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static carrier GetCurrier(string _ruc)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_ruc", _ruc);
            return sql_puntero.ExecuteSelectOnly<carrier>(nueva_conexion, 4000, "[brbk].consultarCarrier", parametros);
        }

        public static carrier GetCurrierFacturacion(string _ruc, string _i_mrn)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_ruc", _ruc);
            parametros.Add("i_mrn",_i_mrn);
            return sql_puntero.ExecuteSelectOnly<carrier>(nueva_conexion, 6000, "[brbk].consultarCarrier_Facturacion", parametros);
        }



    }
}
