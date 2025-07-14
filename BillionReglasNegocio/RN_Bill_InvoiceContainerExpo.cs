using BillionAccesoDatos;
using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionReglasNegocio
{
    public class RN_Bill_InvoiceContainerExpo
    {
        private DA_Bill_InvoiceContainerExpo _invoiceContainerExpo;

        public RN_Bill_InvoiceContainerExpo()
        {
            _invoiceContainerExpo = new DA_Bill_InvoiceContainerExpo();
        }

        public string grabarEntidad(List<Cls_Bill_CabeceraExpo> _ListaFacturas)
        {
            string v_OnError = string.Empty;
            try
            {
                foreach (var v_cabecera in _ListaFacturas)
                {
                    string OnError = string.Empty;
                    _invoiceContainerExpo.Cabecera = v_cabecera;
                    _invoiceContainerExpo.SaveTransaction(out OnError);
                    v_OnError += v_OnError == string.Empty? OnError: '\n' + OnError;
                }
            }
            catch
            {
                throw;
            }
            return v_OnError;
        }

        public string actualizarStatus(long _codigo, string _estado)
        {
            string v_OnError = string.Empty;
            try
            {
                    string OnError = string.Empty;
                    _invoiceContainerExpo.UpdateStatus(_codigo,_estado, out OnError);
                    v_OnError += v_OnError == string.Empty ? OnError : '\n' + OnError;
            }
            catch
            {
                throw;
            }
            return v_OnError;
        }

        
        public List<Cls_Bill_CabeceraExpo> consultaEntidad( string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            return  _invoiceContainerExpo.ConsultaEntidad (_CNTR_VEPR_REFERENCE, out OnError).ToList();
        }
        
        public List<Cls_Bill_CabeceraExpo> consultaEntidad_Full(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaEntidad_Full(_CNTR_VEPR_REFERENCE, out OnError).ToList();
        }

        public List<Cls_Bill_CabeceraExpo> consultaEntidad_Pan(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaEntidad_Pan(_CNTR_VEPR_REFERENCE, out OnError).ToList();
        }

        public List<Cls_Bill_CabeceraExpo> consultaEntidad_Reefer(string _CNTR_VEPR_REFERENCE, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaEntidad_Reefer(_CNTR_VEPR_REFERENCE, out OnError).ToList();
        }

        public List<Cls_Bill_CabeceraExpo> consultaCabeceraPendiente(int registros ,string _estado, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaCabecerasPendientes(registros, _estado, out OnError).ToList();
        }

        public List<Cls_Bill_Container_Expo> consultaDetalle(long? _id, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaDetalle(_id, out OnError).ToList();
        }

        public List<Cls_Bill_Container_Expo_Det_Validacion> consultaSubDetalle(long? _id, long? _consecutivo, string _cntr_vepr_reference, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaSubdetalle(_id, _consecutivo, _cntr_vepr_reference, out OnError).ToList();
        }

        public List<Cls_FacturasExpo> consultaFacturas(string _cntr_vepr_reference,string _booking, out string OnError)
        {
            return _invoiceContainerExpo.ConsultaFacturas( _cntr_vepr_reference, _booking, out OnError).ToList();
        }
    }
}
