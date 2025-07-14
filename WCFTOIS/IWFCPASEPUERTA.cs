using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Xml;
namespace WCFTOIS
{
    
    [ServiceContract]
    public interface IWFCPASEPUERTA
    {
        [OperationContract]
        DataSet GetContainerinfo(String xmlparameter);
        [OperationContract]
        DataSet GetUserFacturainfo(String xmlparameter);
        [OperationContract]
        DataSet GetCFSSubIteminfo(String xmlparameter);
        [OperationContract]
        DataSet GetContainerN4info(String xmlparameter, String xmlparameter2);
        [OperationContract]
        DataSet GetChoferinfo();
        [OperationContract]
        DataSet GetEmpresainfo(String xmlparameter);
        [OperationContract]
        DataSet GetPlacainfo();
        [OperationContract]
        DataSet GetTurnoinfo(String xmlparameter, String wuser);
        [OperationContract]
        DataSet GetFacturainfo(String[] xmlparameter);
        [OperationContract]
        DataSet SavePasePuerta(String xmlparameter, String[] xmlparameterN4, String wtype);
        [OperationContract]
        DataSet RImprePasePuerta(String xmlparameter, String Wtype);
        [OperationContract]
        DataSet GetObserFactura(String xmlparameter);
        [OperationContract]
        DataSet SaveFactura(String[] xmlparameter, String wuser, String Wtype, String WAgente, String WComentario);
        [OperationContract]
        void SaveCancelarPasePuerta(String wxml, String[] xmlparameter, String[] xmlparameter2);
        [OperationContract]
        void SaveActualizarPasePuerta(String wxml, String user);
        [OperationContract]
        void SaveTempTurno(String xmlparameter);
        [OperationContract]
        void SaveDAI(String[] xmlparameter);
        [OperationContract]
        String IsTempTurnoInfo(String xmlparameter);
        [OperationContract]
        String IsManifContenedorInfo(String xmlparameter);
        [OperationContract]
        DataSet GetRIDT(String xmlparameter);
        [OperationContract]
        DataSet GetServicesinfo(String xmlparameter);
        [OperationContract]
        DataSet GetBookinginfo(String xmlparameter);
        [OperationContract]
        DataSet GetSysproControlCreditoinfo(String xmlparameter);
        [OperationContract]
        DataSet GetBreakBulkN4info(String xmlparameter, String xmlparameter2);
        [OperationContract]
        void SaveRefeer(String[] xmlparameter);
        [OperationContract]
        void EXECICU(String xmlparameter);
        [OperationContract]
        void EXECICU_tmp(String xmlparameter,int tipo);
        [OperationContract]
        DataSet GetPNinfo(String xmlparameter);
        [OperationContract]
        String ISDateValidoLiblockCliente(String xmlparameter);
        [OperationContract]
        void SaveLiberacion_Clientes(String xmlparameter);
        [OperationContract]
        void SaveLock_Clientes(String xmlparameter);
        [OperationContract]
        DataSet GetLibClientesinfo(String xmlparameter);
        [OperationContract]
        DataSet GetLockClientesinfo(String xmlparameter);
        [OperationContract]
        DataSet RImpLibLockCliente(String xmlparameter);
        [OperationContract]
        DataSet GetCatalogoinfo(String xmlparameter);
        [OperationContract]
        DataSet GetPNAsignacion(String xmlparameter);
        [OperationContract]
        void SavePNAsignacion(String xmlparameter);
        
        
    }


   
   
}
