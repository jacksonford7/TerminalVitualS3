using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Catalogo_Cargo : Cls_Bil_Base
    {
        #region "Propiedades"
        public int id { get; set; }
        public string nombre { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public BAN_Catalogo_Cargo()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Cargo> ConsultarListaCargos(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Cargo>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Cargo_Consultar]", null, out OnError);
        }


        public static BAN_Catalogo_Cargo GetCargos(long _id)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Cargo>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Cargo_Consultar]", parametros);
            return obj;
        }
    }
}
