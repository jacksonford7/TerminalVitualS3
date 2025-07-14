using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
 public   class VBS_ConsultarTipoCargas : Cls_Bil_Base
    {


        private int _Id_Carga = 0;
        private string _Desc_Carga = string.Empty;


        public int Id_Carga { get => _Id_Carga; set => _Id_Carga = value; }
        public string Desc_Carga { get => _Desc_Carga; set => _Desc_Carga = value; }

      

        public VBS_ConsultarTipoCargas()
        {
            init();
        }

        public static List<VBS_ConsultarTipoCargas> ConsultarTipoCargas(out string OnError)
        {
            parametros.Clear();       
            return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarTipoCargas>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_TIPO_CARGAS", null, out OnError);
        }


    }



   
}
