using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlOPC.Entidades
{
    public class Concepto : Base
    {
        private int _id = 0;
        private string _name = string.Empty;
        private bool active = false;
        private decimal _price = 0;
        private static String v_mensaje = string.Empty;

        public Concepto()
        {
            init();
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public bool Active { get => active; set => active = value; }
        public decimal price { get => _price; set => _price = value; }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

        public static List<Concepto> ListConceptos(out string OnError)
        {
            return sql_pointer.ExecuteSelectControl<Concepto>(sql_pointer.basic_con, 2000, "PC_C_Concepto", null,  out OnError);
        }

        public static List<Concepto> GetConcepto(Int32 _id)
        {
            OnInit();

            var v_conexion = app_configurations.get_configuration("OC_DB");
            return sql_pointer.ExecuteSelectControl<Concepto>(sql_pointer.basic_con, 2000, "PC_G_Concepto", new Dictionary<string, object>() { { "id", _id } }, out v_mensaje);
        }
    }
}
