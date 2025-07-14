using BillionAccesoDatos;
using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionReglasNegocio
{
    public class RN_Bill_Proforma_Listado
    {

        private DA_Bill_Proforma_Listado _DA_Bill_Proforma_Listado;

        public RN_Bill_Proforma_Listado()
        {
            _DA_Bill_Proforma_Listado = new DA_Bill_Proforma_Listado();
        }

        public List<Cls_Bil_Proforma_Listado> BackOffice_Entidad_Listado_Proforma_Impo(DateTime PF_FECHA_DESDE, DateTime PF_FECHA_HASTA, out string OnError)
        {
            return _DA_Bill_Proforma_Listado.BackOffice_Listado_Proformas(PF_FECHA_DESDE, PF_FECHA_HASTA, out OnError).ToList();
        }

        public List<Cls_Bil_Proforma_Listado> Entidad_Listado_Proforma_Impo(DateTime PF_FECHA_DESDE, DateTime PF_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
            return _DA_Bill_Proforma_Listado.Listado_Proformas(PF_FECHA_DESDE, PF_FECHA_HASTA, Usuario, Agente, out OnError).ToList();
        }
    }
}
