using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
   
        public class VBS_ConsultarTipoBloques : Cls_Bil_Base
        {


            private int _Id_Bloque = 0;
            private string _Codigo = string.Empty;


            public int IdBloque { get => _Id_Bloque; set => _Id_Bloque = value; }
            public string Codigo { get => _Codigo; set => _Codigo = value; }



            public VBS_ConsultarTipoBloques()
            {
                init();
            }
            public static List<VBS_ConsultarTipoBloques> ConsultarTipoBloques(out string OnError)
            {
                parametros.Clear();

                return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarTipoBloques>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_TIPO_BLOQUE", null, out OnError);

            }

            public static List<VBS_ConsultarTipoBloques> ConsultarTipoBloquesZAL(out string OnError)
            {
                parametros.Clear();
                return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarTipoBloques>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_TIPO_BLOQUE_ZAL", null, out OnError);
            }
        }
    }






