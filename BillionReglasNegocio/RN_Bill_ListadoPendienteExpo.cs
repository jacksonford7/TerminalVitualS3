using BillionAccesoDatos;
using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BillionReglasNegocio
{
    public class RN_Bill_ListadoPendienteExpo
    {

        private DA_Bill_ListadoPendienteExpo _DA_Bill_ListadoPendienteExpo;

        public RN_Bill_ListadoPendienteExpo()
        {
            _DA_Bill_ListadoPendienteExpo = new DA_Bill_ListadoPendienteExpo();
        }

        public List<Cls_Bill_CabeceraExpo> Entidad_Listado_Pendiente_Expo( out string OnError)
        {
            return _DA_Bill_ListadoPendienteExpo.Listado_Pendiente_Expo(out OnError).ToList();
        }

      
    }
}
