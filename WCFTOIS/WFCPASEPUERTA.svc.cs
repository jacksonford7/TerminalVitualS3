using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using ClSPOLTOIS;


namespace WCFTOIS
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    public class WFCPASEPUERTA : IWFCPASEPUERTA
    {
        public System.Data.DataSet GetContainerinfo(String xmlparameter)
        {

              DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA ();
                wresult = wcontainer.GetContainerCgsaN4(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Contenedor {0} ",  exc.Message.ToString()));
            }
            return wresult;

            
        }
        public System.Data.DataSet GetPNinfo(String xmlparameter)
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetPN(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Pase a Puerta {0} ",  exc.Message.ToString()));
            }
            return wresult;

        }
        public System.Data.DataSet GetChoferinfo()
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetChofer();
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Chofer {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public System.Data.DataSet GetEmpresainfo(String xmlparameter)
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetEmpresa(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Empresa {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public System.Data.DataSet GetPlacainfo()
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetPlaca();
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Placa {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public System.Data.DataSet GetTurnoinfo(String xmlparameter, String wuser)
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetTurno(xmlparameter, wuser);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Turno {0} ",  exc.Message.ToString()));
            }
            return wresult;


        }
        public System.Data.DataSet GetFacturainfo(String[] xmlparameter)
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetFactura(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Factura {0} ",  exc.Message.ToString()));
            }
            return wresult;


        }
        public System.Data.DataSet SaveFactura(String[] xmlparameter, String wuser, String Wtype, String WAgente, String WComentario)
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.SaveFactura(xmlparameter, wuser, Wtype, WAgente, WComentario);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Generar de Factura {0} ",  exc.Message.ToString()));
            }
            return wresult;

        }
        public DataSet SavePasePuerta(String xmlparameter, String[] xmlparameterN4, String wtype)
        {

            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.SavePasePuerta(xmlparameter, xmlparameterN4, wtype);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al generar el Pase a Puerta {0} ",  exc.Message.ToString()));
            }

            return wresult;

        }
        public DataSet GetContainerN4info(string xmlparameter, string xmlparameter1)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetContainerN4(xmlparameter, xmlparameter1);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Contenedor {0} ",  exc.Message.ToString()));
            }
            return wresult;

        }
        public DataSet GetBreakBulkN4info(string xmlparameter, string xmlparameter1)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetBreakBulk(xmlparameter, xmlparameter1);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de BreakBulk {0} ",  exc.Message.ToString()));
            }
            return wresult;

        }
        public void SaveTempTurno(string xmlparameter)
        {
            try
            {
                ClSCONTAINERCGSA wTempTurno = new ClSCONTAINERCGSA();
                wTempTurno.SaveTempTurno(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al generar Turno Temporal {0} ",  exc.Message.ToString()));
            }

        }
        public void SaveDAI(String[] xmlparameter)
        {
            try
            {
                ClSCONTAINERCGSA wDAi = new ClSCONTAINERCGSA();
                wDAi.SaveDAI(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al generar Turno Temporal {0} ",  exc.Message.ToString()));
            }

        }
        public String IsTempTurnoInfo(String xmlparameter)
        {
            Boolean wresult = false;
            try
            {
                ClSCONTAINERCGSA wTempTurno = new ClSCONTAINERCGSA();
                if (wTempTurno.IsTempTurno (xmlparameter))
                {
                    wresult = true;
              }
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al generar Turno Temporal {0} ",  exc.Message.ToString()));
            }
            return wresult.ToString();
        }
        public String IsManifContenedorInfo(String xmlparameter)
        {
            Boolean wresult = false;
            try
            {
                ClSCONTAINERCGSA wContenedor = new ClSCONTAINERCGSA();
                if (wContenedor.IsManifContenedor(xmlparameter))
                {
                    wresult = true;
              }
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Consulta Contenedor Manifiesto {0} ",  exc.Message.ToString()));
            }
            return wresult.ToString();
        }
        public DataSet GetRIDT(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.RIDT(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de RIDT {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet GetServicesinfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetServices(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Servicio {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet GetBookinginfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetBooking(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de BooKing {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet GetSysproControlCreditoinfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetSysproControlCredito(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Syspro Control Credito {0} ",  exc.Message.ToString()));
            }
            return wresult;
        }
        public void SaveRefeer(String[] xmlparameter)
        {

            
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                 wcontainer.SaveRefeer(xmlparameter);
            }
            catch (Exception exc)
            {
         
            }

            

        }
        public void SaveCancelarPasePuerta(String wxml, String[] xmlparameter, String[] xmlparameter2)
        {
            try
            {
                ClSCONTAINERCGSA wCancelarPasePuerta = new ClSCONTAINERCGSA();
                 wCancelarPasePuerta.SaveCancelarPasePuerta(wxml, xmlparameter, xmlparameter2);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Cancelar Pase Web {0} ",  exc.Message.ToString()));
            }
        }
        public void SaveActualizarPasePuerta(String wxml, String user)
        {
            try
            {
                ClSCONTAINERCGSA wCancelarPasePuerta = new ClSCONTAINERCGSA();
                wCancelarPasePuerta.SaveActualizarPasePuerta(wxml, user);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Actualizar Pase Web {0} ", exc.Message.ToString()));
            }
        }
        public DataSet RImprePasePuerta(string xmlparameter, string Wtype)
        {
            DataSet wresult = new DataSet();

            try
            {
                ClSCONTAINERCGSA wRimpre = new ClSCONTAINERCGSA();
                wresult = wRimpre.RImprePasePuerta(xmlparameter, Wtype);
            }
            catch (Exception exc)
            {
                throw new DataException( exc.Message.ToString());
            }

            return wresult;
        }
        public void EXECICU(string xmlparameter)
        {            
            try
            {
                ClSCONTAINERCGSA wICU = new ClSCONTAINERCGSA();
                wICU.EXECICU(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al generar ICU {0} ",  exc.Message.ToString()));
            }

        }
        public void EXECICU_tmp(string xmlparameter, int tipo)
        {
            try
            {
                ClSCONTAINERCGSA wICU = new ClSCONTAINERCGSA();
                wICU.EXECICU_tmp(xmlparameter,tipo);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al generar ICU {0} ", exc.Message.ToString()));
            }

        }
        public String ISDateValidoLiblockCliente(string xmlparameter)
        {
            Boolean wresult = false;
            try
            {
                ClSCONTAINERCGSA wContenedor = new ClSCONTAINERCGSA();
                if (wContenedor.ISDateValidoLiblockCliente(xmlparameter))
                {
                    wresult = true;
                }            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Consulta Validacion de Fecha Lib o Block Cliente {0} ", exc.Message.ToString()));
            }
            return wresult.ToString();
        }
        public void SaveLiberacion_Clientes(string xmlparameter)
        {
            try
            {
                ClSCONTAINERCGSA wLiberacion = new ClSCONTAINERCGSA();
                wLiberacion.SaveLiberacion_Clientes(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Liberar Cliente {0} ", exc.Message.ToString()));
            }
        }
        public void SaveLock_Clientes(string xmlparameter)
        {
            try
            {
                ClSCONTAINERCGSA wLockCliente = new ClSCONTAINERCGSA();
                wLockCliente.SaveLock_Clientes(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Lock Cliente {0} ", exc.Message.ToString()));
            }
        }
        public DataSet GetLibClientesinfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetLibClientes(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Liberacion Clientes {0} ", exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet GetLockClientesinfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetLockClientes(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Lock Clientes {0} ", exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet RImpLibLockCliente(string xmlparameter)
        {
            DataSet wresult = new DataSet();

            try
            {
                ClSCONTAINERCGSA wRimpre = new ClSCONTAINERCGSA();
                wresult = wRimpre.RLibLockCliente(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(exc.Message.ToString());
            }

            return wresult;
        }
        public DataSet GetCatalogoinfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcatalogo = new ClSCONTAINERCGSA();
                wresult = wcatalogo.GetCatalogoCgsa(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Catalogo {0} ", exc.Message.ToString()));
            }
            return wresult;

        }
        public DataSet GetCFSSubIteminfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetCFSSubItem(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta del Subitem CFS {0} ", exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet GetObserFactura(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetObserFactura(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de Obervacion de Factura {0} ", exc.Message.ToString()));
            }
            return wresult;
        }
        public DataSet GetPNAsignacion(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetPNAsignacion(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error el la consulta de De Asignación de Turno {0} ", exc.Message.ToString()));
            }
            return wresult;
        }
        public void SavePNAsignacion(string xmlparameter)
        {
            try
            {
                ClSCONTAINERCGSA wLiberacion = new ClSCONTAINERCGSA();
                wLiberacion.SavePNAsignacion(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error al Asignar Turno {0} ", exc.Message.ToString()));
            }
        }

        public DataSet GetUserFacturainfo(string xmlparameter)
        {
            DataSet wresult = new DataSet();
            try
            {
                ClSCONTAINERCGSA wcontainer = new ClSCONTAINERCGSA();
                wresult = wcontainer.GetUserFactura(xmlparameter);
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error en el reporte de factura {0} ", exc.Message.ToString()));
            }
            return wresult;

        }
    }
}
