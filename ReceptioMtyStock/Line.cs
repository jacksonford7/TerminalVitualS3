using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlOPC.Entidades;
    
namespace ReceptioMtyStock
{
    public class Line : Base 
    {

        public int id_line { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string mailStock { get; set; }
        public int stock { get; set; }
        public string notes { get; set; }
        public DateTime? createDate { get; set; }

        private static String v_mensaje = string.Empty;

        public Line(string _id, string _name)
        {
            this.id = _id; this.name = name;
        }
        public Line(string _id)
        {
            this.id = _id;
        }
        public Line()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("RECEPTIO");
        }

        public static List<Line> ListLine(string _Id)
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<Line>(v_conexion, 2000, "pc_c_line", new Dictionary<string, object>() { { "ID", _Id } }, out v_mensaje);

        }


    }
}
