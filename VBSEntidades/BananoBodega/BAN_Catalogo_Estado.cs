using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Catalogo_Estado : Cls_Bil_Base
    {
        #region "Propiedades"
        public string id { get; set; }
        public string nombre { get; set; }
        public bool? estado { get; set; }
        #endregion

        public BAN_Catalogo_Estado()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Estado> ConsultarLista(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Estado>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_Catalogo_Estado_Consultar", parametros, out OnError);
        }

        public static BAN_Catalogo_Estado GetEntidad(string _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Estado>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_Catalogo_Estado_Consultar", parametros);
        }
    }
}
