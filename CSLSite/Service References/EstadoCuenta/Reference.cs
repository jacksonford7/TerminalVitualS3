﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSLSite.EstadoCuenta {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://localhost/Ws_Sap_EstadoDeCuenta", ConfigurationName="EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap")]
    public interface Ws_Sap_EstadoDeCuentaSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento EMPE_RUC del espacio de nombres http://localhost/Ws_Sap_EstadoDeCuenta no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost/Ws_Sap_EstadoDeCuenta/SI_Customer_Statement_NAVIS_CGSA", ReplyAction="*")]
        CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponse SI_Customer_Statement_NAVIS_CGSA(CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost/Ws_Sap_EstadoDeCuenta/SI_Customer_Statement_NAVIS_CGSA", ReplyAction="*")]
        System.Threading.Tasks.Task<CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponse> SI_Customer_Statement_NAVIS_CGSAAsync(CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_Customer_Statement_NAVIS_CGSARequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SI_Customer_Statement_NAVIS_CGSA", Namespace="http://localhost/Ws_Sap_EstadoDeCuenta", Order=0)]
        public CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequestBody Body;
        
        public SI_Customer_Statement_NAVIS_CGSARequest() {
        }
        
        public SI_Customer_Statement_NAVIS_CGSARequest(CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://localhost/Ws_Sap_EstadoDeCuenta")]
    public partial class SI_Customer_Statement_NAVIS_CGSARequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string EMPE_RUC;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string USER_NAME;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string PASSWORD;
        
        public SI_Customer_Statement_NAVIS_CGSARequestBody() {
        }
        
        public SI_Customer_Statement_NAVIS_CGSARequestBody(string EMPE_RUC, string USER_NAME, string PASSWORD) {
            this.EMPE_RUC = EMPE_RUC;
            this.USER_NAME = USER_NAME;
            this.PASSWORD = PASSWORD;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_Customer_Statement_NAVIS_CGSAResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SI_Customer_Statement_NAVIS_CGSAResponse", Namespace="http://localhost/Ws_Sap_EstadoDeCuenta", Order=0)]
        public CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponseBody Body;
        
        public SI_Customer_Statement_NAVIS_CGSAResponse() {
        }
        
        public SI_Customer_Statement_NAVIS_CGSAResponse(CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://localhost/Ws_Sap_EstadoDeCuenta")]
    public partial class SI_Customer_Statement_NAVIS_CGSAResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement SI_Customer_Statement_NAVIS_CGSAResult;
        
        public SI_Customer_Statement_NAVIS_CGSAResponseBody() {
        }
        
        public SI_Customer_Statement_NAVIS_CGSAResponseBody(System.Xml.Linq.XElement SI_Customer_Statement_NAVIS_CGSAResult) {
            this.SI_Customer_Statement_NAVIS_CGSAResult = SI_Customer_Statement_NAVIS_CGSAResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface Ws_Sap_EstadoDeCuentaSoapChannel : CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Ws_Sap_EstadoDeCuentaSoapClient : System.ServiceModel.ClientBase<CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap>, CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap {
        
        public Ws_Sap_EstadoDeCuentaSoapClient() {
        }
        
        public Ws_Sap_EstadoDeCuentaSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Ws_Sap_EstadoDeCuentaSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Ws_Sap_EstadoDeCuentaSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Ws_Sap_EstadoDeCuentaSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponse CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap.SI_Customer_Statement_NAVIS_CGSA(CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest request) {
            return base.Channel.SI_Customer_Statement_NAVIS_CGSA(request);
        }
        
        public System.Xml.Linq.XElement SI_Customer_Statement_NAVIS_CGSA(string EMPE_RUC, string USER_NAME, string PASSWORD) {
            CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest inValue = new CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest();
            inValue.Body = new CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequestBody();
            inValue.Body.EMPE_RUC = EMPE_RUC;
            inValue.Body.USER_NAME = USER_NAME;
            inValue.Body.PASSWORD = PASSWORD;
            CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponse retVal = ((CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap)(this)).SI_Customer_Statement_NAVIS_CGSA(inValue);
            return retVal.Body.SI_Customer_Statement_NAVIS_CGSAResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponse> CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap.SI_Customer_Statement_NAVIS_CGSAAsync(CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest request) {
            return base.Channel.SI_Customer_Statement_NAVIS_CGSAAsync(request);
        }
        
        public System.Threading.Tasks.Task<CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSAResponse> SI_Customer_Statement_NAVIS_CGSAAsync(string EMPE_RUC, string USER_NAME, string PASSWORD) {
            CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest inValue = new CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequest();
            inValue.Body = new CSLSite.EstadoCuenta.SI_Customer_Statement_NAVIS_CGSARequestBody();
            inValue.Body.EMPE_RUC = EMPE_RUC;
            inValue.Body.USER_NAME = USER_NAME;
            inValue.Body.PASSWORD = PASSWORD;
            return ((CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoap)(this)).SI_Customer_Statement_NAVIS_CGSAAsync(inValue);
        }
    }
}
