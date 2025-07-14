using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public class Operator : Base
    {
        private string _gKey = string.Empty;
        private decimal _OPC = 0;
        private String _nombre = string.Empty;
        private String _apellido = string.Empty;
        private String _identificacion = string.Empty;
        private static String v_mensaje = string.Empty;

        public string GKey { get => _gKey; set => _gKey = value; }
        public decimal OPC { get => _OPC; set => _OPC = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Apellidos { get => _apellido; set => _apellido = value; }
        public string Identificacion { get => _identificacion; set => _identificacion = value; }
        public string OPC_Nombre { get; set; }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
        }

        public static List<Operator> ListOperator(string _ruc)
        {
            OnInit();

            var v_conexion = app_configurations.get_configuration("OC_DB");
            return sql_pointer.ExecuteSelectControl<Operator>(v_conexion.value, 2000, "PC_C_OPERARIO_ESP", new Dictionary<string, object>() { { "i_ruc", _ruc } }, out v_mensaje);
        }

        public static List<Operator> ListOperator(string _ruc, string _pi)
        {
            OnInit();
            var v_conexion = app_configurations.get_configuration("OC_DB");
            return sql_pointer.ExecuteSelectControl<Operator>(v_conexion.value, 4000, "PC_C_Operario_Opc", new Dictionary<string, object>() { { "i_ruc", _ruc }, { "i_pista", _pi } }, out v_mensaje);
        }

    }
}
