﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSLSite.WFCPASEPUERTA {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WFCPASEPUERTA.IWFCPASEPUERTA")]
    public interface IWFCPASEPUERTA {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetContainerinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetContainerinfoResponse")]
        System.Data.DataSet GetContainerinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetContainerinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetContainerinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetContainerinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetUserFacturainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetUserFacturainfoResponse")]
        System.Data.DataSet GetUserFacturainfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetUserFacturainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetUserFacturainfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetUserFacturainfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetCFSSubIteminfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetCFSSubIteminfoResponse")]
        System.Data.DataSet GetCFSSubIteminfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetCFSSubIteminfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetCFSSubIteminfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetCFSSubIteminfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetContainerN4info", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetContainerN4infoResponse")]
        System.Data.DataSet GetContainerN4info(string xmlparameter, string xmlparameter2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetContainerN4info", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetContainerN4infoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetContainerN4infoAsync(string xmlparameter, string xmlparameter2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetChoferinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetChoferinfoResponse")]
        System.Data.DataSet GetChoferinfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetChoferinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetChoferinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetChoferinfoAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetEmpresainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetEmpresainfoResponse")]
        System.Data.DataSet GetEmpresainfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetEmpresainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetEmpresainfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetEmpresainfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetPlacainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetPlacainfoResponse")]
        System.Data.DataSet GetPlacainfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetPlacainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetPlacainfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPlacainfoAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetTurnoinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetTurnoinfoResponse")]
        System.Data.DataSet GetTurnoinfo(string xmlparameter, string wuser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetTurnoinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetTurnoinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetTurnoinfoAsync(string xmlparameter, string wuser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetFacturainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetFacturainfoResponse")]
        System.Data.DataSet GetFacturainfo(string[] xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetFacturainfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetFacturainfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetFacturainfoAsync(string[] xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SavePasePuerta", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SavePasePuertaResponse")]
        System.Data.DataSet SavePasePuerta(string xmlparameter, string[] xmlparameterN4, string wtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SavePasePuerta", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SavePasePuertaResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> SavePasePuertaAsync(string xmlparameter, string[] xmlparameterN4, string wtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/RImprePasePuerta", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/RImprePasePuertaResponse")]
        System.Data.DataSet RImprePasePuerta(string xmlparameter, string Wtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/RImprePasePuerta", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/RImprePasePuertaResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> RImprePasePuertaAsync(string xmlparameter, string Wtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetObserFactura", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetObserFacturaResponse")]
        System.Data.DataSet GetObserFactura(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetObserFactura", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetObserFacturaResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetObserFacturaAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveFactura", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveFacturaResponse")]
        System.Data.DataSet SaveFactura(string[] xmlparameter, string wuser, string Wtype, string WAgente, string WComentario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveFactura", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveFacturaResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> SaveFacturaAsync(string[] xmlparameter, string wuser, string Wtype, string WAgente, string WComentario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveCancelarPasePuerta", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveCancelarPasePuertaResponse")]
        void SaveCancelarPasePuerta(string wxml, string[] xmlparameter, string[] xmlparameter2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveCancelarPasePuerta", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveCancelarPasePuertaResponse")]
        System.Threading.Tasks.Task SaveCancelarPasePuertaAsync(string wxml, string[] xmlparameter, string[] xmlparameter2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveTempTurno", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveTempTurnoResponse")]
        void SaveTempTurno(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveTempTurno", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveTempTurnoResponse")]
        System.Threading.Tasks.Task SaveTempTurnoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveDAI", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveDAIResponse")]
        void SaveDAI(string[] xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveDAI", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveDAIResponse")]
        System.Threading.Tasks.Task SaveDAIAsync(string[] xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/IsTempTurnoInfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/IsTempTurnoInfoResponse")]
        string IsTempTurnoInfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/IsTempTurnoInfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/IsTempTurnoInfoResponse")]
        System.Threading.Tasks.Task<string> IsTempTurnoInfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/IsManifContenedorInfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/IsManifContenedorInfoResponse")]
        string IsManifContenedorInfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/IsManifContenedorInfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/IsManifContenedorInfoResponse")]
        System.Threading.Tasks.Task<string> IsManifContenedorInfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetRIDT", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetRIDTResponse")]
        System.Data.DataSet GetRIDT(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetRIDT", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetRIDTResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRIDTAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetServicesinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetServicesinfoResponse")]
        System.Data.DataSet GetServicesinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetServicesinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetServicesinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetServicesinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetBookinginfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetBookinginfoResponse")]
        System.Data.DataSet GetBookinginfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetBookinginfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetBookinginfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetBookinginfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetSysproControlCreditoinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetSysproControlCreditoinfoResponse")]
        System.Data.DataSet GetSysproControlCreditoinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetSysproControlCreditoinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetSysproControlCreditoinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSysproControlCreditoinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetBreakBulkN4info", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetBreakBulkN4infoResponse")]
        System.Data.DataSet GetBreakBulkN4info(string xmlparameter, string xmlparameter2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetBreakBulkN4info", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetBreakBulkN4infoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetBreakBulkN4infoAsync(string xmlparameter, string xmlparameter2);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveRefeer", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveRefeerResponse")]
        void SaveRefeer(string[] xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveRefeer", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveRefeerResponse")]
        System.Threading.Tasks.Task SaveRefeerAsync(string[] xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/EXECICU", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/EXECICUResponse")]
        void EXECICU(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/EXECICU", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/EXECICUResponse")]
        System.Threading.Tasks.Task EXECICUAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/EXECICU_tmp", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/EXECICU_tmpResponse")]
        void EXECICU_tmp(string xmlparameter, int tipo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/EXECICU_tmp", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/EXECICU_tmpResponse")]
        System.Threading.Tasks.Task EXECICU_tmpAsync(string xmlparameter, int tipo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetPNinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetPNinfoResponse")]
        System.Data.DataSet GetPNinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetPNinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetPNinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPNinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/ISDateValidoLiblockCliente", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/ISDateValidoLiblockClienteResponse")]
        string ISDateValidoLiblockCliente(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/ISDateValidoLiblockCliente", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/ISDateValidoLiblockClienteResponse")]
        System.Threading.Tasks.Task<string> ISDateValidoLiblockClienteAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveLiberacion_Clientes", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveLiberacion_ClientesResponse")]
        void SaveLiberacion_Clientes(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveLiberacion_Clientes", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveLiberacion_ClientesResponse")]
        System.Threading.Tasks.Task SaveLiberacion_ClientesAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveLock_Clientes", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveLock_ClientesResponse")]
        void SaveLock_Clientes(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SaveLock_Clientes", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SaveLock_ClientesResponse")]
        System.Threading.Tasks.Task SaveLock_ClientesAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetLibClientesinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetLibClientesinfoResponse")]
        System.Data.DataSet GetLibClientesinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetLibClientesinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetLibClientesinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetLibClientesinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetLockClientesinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetLockClientesinfoResponse")]
        System.Data.DataSet GetLockClientesinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetLockClientesinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetLockClientesinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetLockClientesinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/RImpLibLockCliente", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/RImpLibLockClienteResponse")]
        System.Data.DataSet RImpLibLockCliente(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/RImpLibLockCliente", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/RImpLibLockClienteResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> RImpLibLockClienteAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetCatalogoinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetCatalogoinfoResponse")]
        System.Data.DataSet GetCatalogoinfo(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetCatalogoinfo", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetCatalogoinfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetCatalogoinfoAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetPNAsignacion", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetPNAsignacionResponse")]
        System.Data.DataSet GetPNAsignacion(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/GetPNAsignacion", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/GetPNAsignacionResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPNAsignacionAsync(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SavePNAsignacion", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SavePNAsignacionResponse")]
        void SavePNAsignacion(string xmlparameter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWFCPASEPUERTA/SavePNAsignacion", ReplyAction="http://tempuri.org/IWFCPASEPUERTA/SavePNAsignacionResponse")]
        System.Threading.Tasks.Task SavePNAsignacionAsync(string xmlparameter);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWFCPASEPUERTAChannel : CSLSite.WFCPASEPUERTA.IWFCPASEPUERTA, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WFCPASEPUERTAClient : System.ServiceModel.ClientBase<CSLSite.WFCPASEPUERTA.IWFCPASEPUERTA>, CSLSite.WFCPASEPUERTA.IWFCPASEPUERTA {
        
        public WFCPASEPUERTAClient() {
        }
        
        public WFCPASEPUERTAClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WFCPASEPUERTAClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WFCPASEPUERTAClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WFCPASEPUERTAClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet GetContainerinfo(string xmlparameter) {
            return base.Channel.GetContainerinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetContainerinfoAsync(string xmlparameter) {
            return base.Channel.GetContainerinfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetUserFacturainfo(string xmlparameter) {
            return base.Channel.GetUserFacturainfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetUserFacturainfoAsync(string xmlparameter) {
            return base.Channel.GetUserFacturainfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetCFSSubIteminfo(string xmlparameter) {
            return base.Channel.GetCFSSubIteminfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetCFSSubIteminfoAsync(string xmlparameter) {
            return base.Channel.GetCFSSubIteminfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetContainerN4info(string xmlparameter, string xmlparameter2) {
            return base.Channel.GetContainerN4info(xmlparameter, xmlparameter2);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetContainerN4infoAsync(string xmlparameter, string xmlparameter2) {
            return base.Channel.GetContainerN4infoAsync(xmlparameter, xmlparameter2);
        }
        
        public System.Data.DataSet GetChoferinfo() {
            return base.Channel.GetChoferinfo();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetChoferinfoAsync() {
            return base.Channel.GetChoferinfoAsync();
        }
        
        public System.Data.DataSet GetEmpresainfo(string xmlparameter) {
            return base.Channel.GetEmpresainfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetEmpresainfoAsync(string xmlparameter) {
            return base.Channel.GetEmpresainfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetPlacainfo() {
            return base.Channel.GetPlacainfo();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetPlacainfoAsync() {
            return base.Channel.GetPlacainfoAsync();
        }
        
        public System.Data.DataSet GetTurnoinfo(string xmlparameter, string wuser) {
            return base.Channel.GetTurnoinfo(xmlparameter, wuser);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetTurnoinfoAsync(string xmlparameter, string wuser) {
            return base.Channel.GetTurnoinfoAsync(xmlparameter, wuser);
        }
        
        public System.Data.DataSet GetFacturainfo(string[] xmlparameter) {
            return base.Channel.GetFacturainfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetFacturainfoAsync(string[] xmlparameter) {
            return base.Channel.GetFacturainfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet SavePasePuerta(string xmlparameter, string[] xmlparameterN4, string wtype) {
            return base.Channel.SavePasePuerta(xmlparameter, xmlparameterN4, wtype);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> SavePasePuertaAsync(string xmlparameter, string[] xmlparameterN4, string wtype) {
            return base.Channel.SavePasePuertaAsync(xmlparameter, xmlparameterN4, wtype);
        }
        
        public System.Data.DataSet RImprePasePuerta(string xmlparameter, string Wtype) {
            return base.Channel.RImprePasePuerta(xmlparameter, Wtype);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> RImprePasePuertaAsync(string xmlparameter, string Wtype) {
            return base.Channel.RImprePasePuertaAsync(xmlparameter, Wtype);
        }
        
        public System.Data.DataSet GetObserFactura(string xmlparameter) {
            return base.Channel.GetObserFactura(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetObserFacturaAsync(string xmlparameter) {
            return base.Channel.GetObserFacturaAsync(xmlparameter);
        }
        
        public System.Data.DataSet SaveFactura(string[] xmlparameter, string wuser, string Wtype, string WAgente, string WComentario) {
            return base.Channel.SaveFactura(xmlparameter, wuser, Wtype, WAgente, WComentario);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> SaveFacturaAsync(string[] xmlparameter, string wuser, string Wtype, string WAgente, string WComentario) {
            return base.Channel.SaveFacturaAsync(xmlparameter, wuser, Wtype, WAgente, WComentario);
        }
        
        public void SaveCancelarPasePuerta(string wxml, string[] xmlparameter, string[] xmlparameter2) {
            base.Channel.SaveCancelarPasePuerta(wxml, xmlparameter, xmlparameter2);
        }
        
        public System.Threading.Tasks.Task SaveCancelarPasePuertaAsync(string wxml, string[] xmlparameter, string[] xmlparameter2) {
            return base.Channel.SaveCancelarPasePuertaAsync(wxml, xmlparameter, xmlparameter2);
        }
        
        public void SaveTempTurno(string xmlparameter) {
            base.Channel.SaveTempTurno(xmlparameter);
        }
        
        public System.Threading.Tasks.Task SaveTempTurnoAsync(string xmlparameter) {
            return base.Channel.SaveTempTurnoAsync(xmlparameter);
        }
        
        public void SaveDAI(string[] xmlparameter) {
            base.Channel.SaveDAI(xmlparameter);
        }
        
        public System.Threading.Tasks.Task SaveDAIAsync(string[] xmlparameter) {
            return base.Channel.SaveDAIAsync(xmlparameter);
        }
        
        public string IsTempTurnoInfo(string xmlparameter) {
            return base.Channel.IsTempTurnoInfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<string> IsTempTurnoInfoAsync(string xmlparameter) {
            return base.Channel.IsTempTurnoInfoAsync(xmlparameter);
        }
        
        public string IsManifContenedorInfo(string xmlparameter) {
            return base.Channel.IsManifContenedorInfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<string> IsManifContenedorInfoAsync(string xmlparameter) {
            return base.Channel.IsManifContenedorInfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetRIDT(string xmlparameter) {
            return base.Channel.GetRIDT(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetRIDTAsync(string xmlparameter) {
            return base.Channel.GetRIDTAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetServicesinfo(string xmlparameter) {
            return base.Channel.GetServicesinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetServicesinfoAsync(string xmlparameter) {
            return base.Channel.GetServicesinfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetBookinginfo(string xmlparameter) {
            return base.Channel.GetBookinginfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetBookinginfoAsync(string xmlparameter) {
            return base.Channel.GetBookinginfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetSysproControlCreditoinfo(string xmlparameter) {
            return base.Channel.GetSysproControlCreditoinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetSysproControlCreditoinfoAsync(string xmlparameter) {
            return base.Channel.GetSysproControlCreditoinfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetBreakBulkN4info(string xmlparameter, string xmlparameter2) {
            return base.Channel.GetBreakBulkN4info(xmlparameter, xmlparameter2);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetBreakBulkN4infoAsync(string xmlparameter, string xmlparameter2) {
            return base.Channel.GetBreakBulkN4infoAsync(xmlparameter, xmlparameter2);
        }
        
        public void SaveRefeer(string[] xmlparameter) {
            base.Channel.SaveRefeer(xmlparameter);
        }
        
        public System.Threading.Tasks.Task SaveRefeerAsync(string[] xmlparameter) {
            return base.Channel.SaveRefeerAsync(xmlparameter);
        }
        
        public void EXECICU(string xmlparameter) {
            base.Channel.EXECICU(xmlparameter);
        }
        
        public System.Threading.Tasks.Task EXECICUAsync(string xmlparameter) {
            return base.Channel.EXECICUAsync(xmlparameter);
        }
        
        public void EXECICU_tmp(string xmlparameter, int tipo) {
            base.Channel.EXECICU_tmp(xmlparameter, tipo);
        }
        
        public System.Threading.Tasks.Task EXECICU_tmpAsync(string xmlparameter, int tipo) {
            return base.Channel.EXECICU_tmpAsync(xmlparameter, tipo);
        }
        
        public System.Data.DataSet GetPNinfo(string xmlparameter) {
            return base.Channel.GetPNinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetPNinfoAsync(string xmlparameter) {
            return base.Channel.GetPNinfoAsync(xmlparameter);
        }
        
        public string ISDateValidoLiblockCliente(string xmlparameter) {
            return base.Channel.ISDateValidoLiblockCliente(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<string> ISDateValidoLiblockClienteAsync(string xmlparameter) {
            return base.Channel.ISDateValidoLiblockClienteAsync(xmlparameter);
        }
        
        public void SaveLiberacion_Clientes(string xmlparameter) {
            base.Channel.SaveLiberacion_Clientes(xmlparameter);
        }
        
        public System.Threading.Tasks.Task SaveLiberacion_ClientesAsync(string xmlparameter) {
            return base.Channel.SaveLiberacion_ClientesAsync(xmlparameter);
        }
        
        public void SaveLock_Clientes(string xmlparameter) {
            base.Channel.SaveLock_Clientes(xmlparameter);
        }
        
        public System.Threading.Tasks.Task SaveLock_ClientesAsync(string xmlparameter) {
            return base.Channel.SaveLock_ClientesAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetLibClientesinfo(string xmlparameter) {
            return base.Channel.GetLibClientesinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetLibClientesinfoAsync(string xmlparameter) {
            return base.Channel.GetLibClientesinfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetLockClientesinfo(string xmlparameter) {
            return base.Channel.GetLockClientesinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetLockClientesinfoAsync(string xmlparameter) {
            return base.Channel.GetLockClientesinfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet RImpLibLockCliente(string xmlparameter) {
            return base.Channel.RImpLibLockCliente(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> RImpLibLockClienteAsync(string xmlparameter) {
            return base.Channel.RImpLibLockClienteAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetCatalogoinfo(string xmlparameter) {
            return base.Channel.GetCatalogoinfo(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetCatalogoinfoAsync(string xmlparameter) {
            return base.Channel.GetCatalogoinfoAsync(xmlparameter);
        }
        
        public System.Data.DataSet GetPNAsignacion(string xmlparameter) {
            return base.Channel.GetPNAsignacion(xmlparameter);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetPNAsignacionAsync(string xmlparameter) {
            return base.Channel.GetPNAsignacionAsync(xmlparameter);
        }
        
        public void SavePNAsignacion(string xmlparameter) {
            base.Channel.SavePNAsignacion(xmlparameter);
        }
        
        public System.Threading.Tasks.Task SavePNAsignacionAsync(string xmlparameter) {
            return base.Channel.SavePNAsignacionAsync(xmlparameter);
        }
    }
}
