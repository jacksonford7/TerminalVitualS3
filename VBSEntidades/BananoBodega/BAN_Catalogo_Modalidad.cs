using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Catalogo_Modalidad : Cls_Bil_Base
    {
        #region "Propiedades"
        public int id { get; set; }
        public string nombre { get; set; }
        public bool? estado { get; set; }
        #endregion

        public BAN_Catalogo_Modalidad()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Modalidad> ConsultarLista(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Modalidad>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Modalidad_Consultar]", null, out OnError);
        }


    }
}

