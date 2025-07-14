using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class cfs_cargas_pendientes_detalle : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
       

        private string _mrn = string.Empty;
        private string _msn = string.Empty;
        private string _hsn = string.Empty;
        private string _cntr = string.Empty;
        private string _importador_id = string.Empty;
        private string _importador_name = string.Empty;
        private DateTime _descarga;
        private string _descripcion = string.Empty;
        private int _total_partida = 1;
        private decimal _volumen = 0;
        private decimal _peso = 0;
        private bool _visto =false;
        private string _numero_carga = string.Empty;

        #endregion

        #region "Propiedades"

        public string mrn { get => _mrn; set => _mrn = value; }
        public string msn { get => _msn; set => _msn = value; }
        public string hsn { get => _hsn; set => _hsn = value; }
        public string cntr { get => _cntr; set => _cntr = value; }
        public string importador_id { get => _importador_id; set => _importador_id = value; }
        public string importador_name { get => _importador_name; set => _importador_name = value; }
        public DateTime descarga { get => _descarga; set => _descarga = value; }
        public string descripcion { get => _descripcion; set => _descripcion = value; }
        public int total_partida { get => _total_partida; set => _total_partida = value; }
      
        public decimal volumen { get => _volumen; set => _volumen = value; }
        public decimal peso { get => _peso; set => _peso = value; }
        public bool visto { get => _visto; set => _visto = value; }
        public string numero_carga { get => _numero_carga; set => _numero_carga = value; }

        public string imdt { get; set ; }
        public string declaracion { get; set; }
        public string estado_ridt { get; set; }
        public string tipo_cliente { get; set; }
        #endregion


        public cfs_cargas_pendientes_detalle()
        {
            init();

          
         
        }

   
   

    }
}
