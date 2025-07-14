using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public class Crane : Base
    {
        private Int64 _gKey = 0;
        private String _id = string.Empty;

        private static String v_mensaje = string.Empty;

        public Crane()
        {
            init();
        }

        public long GKey { get => _gKey; set => _gKey = value; }
        public string Id { get => _id; set => _id = value; }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

        public static List<Crane> ListCrane()
        {
            OnInit();

            var v_conexion = app_configurations.get_configuration("N5_DB");
            return sql_pointer.ExecuteSelectControl<Crane>(v_conexion.value, 2000, "PC_C_CRANE", null, out v_mensaje);//retornar la lista de gruas q estan en los documentos

        }

        public static Crane GetCrane(long _gkey )
        {
            OnInit();
            var v_conexion = app_configurations.get_configuration("N5_DB");
            return sql_pointer.ExecuteSelectControl<Crane>(v_conexion.value, 2000, "PC_C_CRANE_ESP", new Dictionary<string, object>() { {"i_gkey",_gkey }}, out v_mensaje).FirstOrDefault(); //retornar la lista de gruas q estan en los documentos
        }

        //estatico de existencia, retorna true: en uso, false: no esta en uso, null: se cayo
        public static bool? ValidateCraneActiveTransaction(long gkey, out string msg)
        {
            //inicializar variables.
            OnInit();

            //->ExecuteSelectOnly
            parametros.Clear();
            parametros.Add("i_Crane_GKey", gkey);
            //te retorna un data_message
            var dm = sql_pointer.ExecuteSelectOnly(sql_pointer.basic_con, 4000, "PC_C_Valida_Grua", parametros);
            if (dm.code != 0)
            {
                msg = dm.message;
                return true;
            }
            //retorna si no existe
            msg = string.Empty;
            return false;
        }

    }
}
