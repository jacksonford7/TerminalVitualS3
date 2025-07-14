using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class ZAL_Detalle_Factura : Cls_Bil_Base
    {



        #region "Propiedades"
        public int fila { get; set; }
        public string turno_id { get; set; }
        public DateTime? turno_fecha { get; set; }
        public string turno_hora { get; set; }
        public string unidad_id { get; set; }
        public string unidad_tamano { get; set; }
        public string unidad_linea { get; set; }
        public string unidad_booking { get; set; }
        public string unidad_referencia { get; set; }
        public string unidad_estatus { get; set; }
        public Int64? unidad_key { get; set; }

        public string chofer_licencia { get; set; }
        public string chofer_nombre { get; set; }
        public string vehiculo_placa { get; set; }
        public string vehiculo_desc { get; set; }

        public string creado_usuario { get; set; }
        public DateTime? creado_fecha { get; set; }

        public Int64? n4_unit_key { get; set; }
        public string n4_message { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string documento_id { get; set; }
        public string estado_pago { get; set; }
        public string estado { get; set; }

        public string ruc_cliente { get; set; }
        public string name_cliente { get; set; }
        public string ruc_asume { get; set; }
        public string name_asume { get; set; }
        public int cantidad { get; set; }
        public string ruc_facturar { get; set; }
        public string cliente_facturar { get; set; }
        public string numero_factura { get; set; }

        public string asume_ruc_facturar { get; set; }
        public string asume_cliente_facturar { get; set; }
        public Int64? id_facturar { get; set; }
        public bool visto { get; set; }
        public string invoice_type { get; set; }
        #endregion


        public ZAL_Detalle_Factura()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<ZAL_Detalle_Factura> Detalle_Pases_Repcontver(DateTime desde, DateTime hasta,  Int64 depot, out string OnError)
        {
        
            parametros.Clear();
            parametros.Add("desde", desde);
            parametros.Add("hasta", hasta);
            parametros.Add("depot", depot);
            return sql_puntero.ExecuteSelectControl<ZAL_Detalle_Factura>(sql_puntero.Conexion_Local, 6000, "zal_pases_pendientes_Repcontver", parametros, out OnError);
        }


        public static List<ZAL_Detalle_Factura> Detalle_Pases(DateTime desde, DateTime hasta, out string OnError)
        {
            OnInit("SERVICE");

            parametros.Clear();
            parametros.Add("desde", desde);
            parametros.Add("hasta", hasta);
            return sql_puntero.ExecuteSelectControl<ZAL_Detalle_Factura>(nueva_conexion, 6000, "sav_pases_pendientes", parametros, out OnError);
        }

        public static List<ZAL_Detalle_Factura> Detalle_Pases_Por_Clientes(DateTime desde, DateTime hasta, string ruc, out string OnError)
        {
            OnInit("SERVICE");

            parametros.Clear();
            parametros.Add("desde", desde);
            parametros.Add("hasta", hasta);
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<ZAL_Detalle_Factura>(nueva_conexion, 6000, "sav_pases_pendientes_cliente", parametros, out OnError);
        }


        public static List<ZAL_Detalle_Factura> Detalle_Pases_Facturados_Por_Clientes(DateTime desde, DateTime hasta, string ruc, out string OnError)
        {
            OnInit("SERVICE");

            parametros.Clear();
            parametros.Add("desde", desde);
            parametros.Add("hasta", hasta);
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<ZAL_Detalle_Factura>(nueva_conexion, 6000, "sav_listado_pases_facturados_cliente", parametros, out OnError);
        }

        public static List<ZAL_Detalle_Factura> Detalle_Pases_Facturados_Por_Cgsa(DateTime desde, DateTime hasta,  out string OnError)
        {
 
            parametros.Clear();
            parametros.Add("desde", desde);
            parametros.Add("hasta", hasta);
            return sql_puntero.ExecuteSelectControl<ZAL_Detalle_Factura>(sql_puntero.Conexion_Local, 6000, "zal_listado_pases_facturados", parametros, out OnError);
        }

    }
}
