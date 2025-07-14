using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public class Opc : Base
    {
        private Decimal _gKey = 0;
        private String _nombre = string.Empty;
        private String _ruc = string.Empty;
        private static String v_mensaje = string.Empty;
        public string Mail { get; set; }

        public Decimal GKey { get => _gKey; set => _gKey = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Ruc { get => _ruc; set => _ruc = value; }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
        }

        public static Opc GetOPC(string _ruc)
        {
            OnInit();

            var v_conexion = app_configurations.get_configuration("OC_DB");
            return sql_pointer.ExecuteSelectControl<Opc >(v_conexion.value, 2000, "PC_C_OPC_ESP", new Dictionary<string, object>() { { "i_ruc", _ruc } },out v_mensaje).FirstOrDefault();
        }


        public static string get_mails()
        {
            OnInit();
            return app_configurations.get_configuration("OPC_MAIL")?.value;
        }
        public static string get_copia()
        {
            OnInit();
            return app_configurations.get_configuration("CGSA_MAIL")?.value;
        }



    }
}
