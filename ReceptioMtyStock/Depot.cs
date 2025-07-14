using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ReceptioMtyStock
{
    public class Depot : Base
    {
        public int id_depot { get; set; }
        public string name { get; set; }
        public string notes { get; set; }   
        public DateTime? createDate { get; set; }
        public int auxiliar_Stock { get; set; }
        private static String v_mensaje = string.Empty;
       

        public Depot()
        {
            init();
        }
        public Depot( string _name,string _notes=null)
        {
            this.name = _name;this.notes = _notes;
        }


        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("RECEPTIO");
        }

        public static List<Depot> ListDepot()
        {
            OnInit();
            
            return sql_pointer.ExecuteSelectControl<Depot>(v_conexion, 2000, "pc_c_depots", null, out v_mensaje);

        }


    }
}
