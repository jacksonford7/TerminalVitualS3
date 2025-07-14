using BillionAccesoDatos;
using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BillionReglasNegocio
{
    public class RN_Bill_Invoice_Listado
    {

        private DA_Bill_Invoice_Listado _DA_Bill_Invoice_Listado;

        public RN_Bill_Invoice_Listado()
        {
            _DA_Bill_Invoice_Listado = new DA_Bill_Invoice_Listado();
        }

        public List<Cls_Bil_Invoice_Listado> BackOffice_Entidad_Listado_Facturas_Impo(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, out string OnError)
        {
            return _DA_Bill_Invoice_Listado.BackOffice_Listado_Facturas(IV_FECHA_DESDE, IV_FECHA_HASTA, out OnError).ToList();
        }

        public List<Cls_Bil_Invoice_Listado> Entidad_Listado_Facturas_Impo(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
            return _DA_Bill_Invoice_Listado.Listado_Facturas(IV_FECHA_DESDE, IV_FECHA_HASTA, Usuario, Agente, out OnError).ToList();
        }

        public List<Cls_Bil_Invoice_Listado> Entidad_Listado_Facturas_Impo_CFS(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
            return _DA_Bill_Invoice_Listado.Listado_Facturas_cfs(IV_FECHA_DESDE, IV_FECHA_HASTA, Usuario, Agente, out OnError).ToList();
        }

        public List<Cls_Bil_Invoice_Listado> BackOffice_Entidad_Listado_Facturas_Expo(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, out string OnError)
        {
            return _DA_Bill_Invoice_Listado.BackOffice_Listado_Facturas_Expo(IV_FECHA_DESDE, IV_FECHA_HASTA, out OnError).ToList();
        }

        public List<Cls_Bil_Invoice_Listado> Entidad_Listado_Facturas_Impo_Brbk(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
            return _DA_Bill_Invoice_Listado.Listado_Facturas_Brbk(IV_FECHA_DESDE, IV_FECHA_HASTA, Usuario, Agente, out OnError).ToList();
        }

    }
}
