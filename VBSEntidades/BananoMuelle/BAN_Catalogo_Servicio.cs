using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Catalogo_Servicio : Cls_Bil_Base
    {
        #region "Propiedades"
        public int id { get; set; }
        public string nombre { get; set; }
        public string aisv { get; set; }
        public string codigoN4 { get; set; }
        public int horaGracia { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public BAN_Catalogo_Servicio()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Servicio> Consultar(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Servicio>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Servicio_Consultar]", null, out OnError);
        }


        public static BAN_Catalogo_Servicio GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Servicio>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Servicio_Consultar]", parametros);
            return obj;
        }
    }
}
