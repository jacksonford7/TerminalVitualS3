using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public class VBS_ConsultarTipoContenedor : Cls_Bil_Base
    {

        private string _Id_Contenedor;
        private int _Id_Carga = 0;
        private string _Desc_Contenedor = string.Empty;



        public string Id_Contenedor { get => _Id_Contenedor; set => _Id_Contenedor = value; }
        public int Id_Carga { get => _Id_Carga; set => _Id_Carga = value; }
        public string Desc_Contenedor { get => _Desc_Contenedor; set => _Desc_Contenedor = value; }

      

        public VBS_ConsultarTipoContenedor()
        {
            init();
        }
        public static List<VBS_ConsultarTipoContenedor> ConsultarTipoContenedor(out string OnError)
        {
            parametros.Clear();
            return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarTipoContenedor>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_TIPO_CONTENEDOR", null, out OnError);
          
        }


    }



   
}
