﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSLSite.WebRef_IngresoIEE {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://argo.navis.com/webservice/external", ConfigurationName="WebRef_IngresoIEE.n4ServiceSoap")]
    public interface n4ServiceSoap {
        
        // CODEGEN: Se está generando un contrato de mensaje, ya que el nombre de elemento scopeCoordinateIds del espacio de nombres http://argo.navis.com/webservice/external no está marcado para aceptar valores nil.
        [System.ServiceModel.OperationContractAttribute(Action="http://argo.navis.com/webservice/external/basicInvoke", ReplyAction="*")]
        CSLSite.WebRef_IngresoIEE.basicInvokeResponse basicInvoke(CSLSite.WebRef_IngresoIEE.basicInvokeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://argo.navis.com/webservice/external/basicInvoke", ReplyAction="*")]
        System.Threading.Tasks.Task<CSLSite.WebRef_IngresoIEE.basicInvokeResponse> basicInvokeAsync(CSLSite.WebRef_IngresoIEE.basicInvokeRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class basicInvokeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="basicInvoke", Namespace="http://argo.navis.com/webservice/external", Order=0)]
        public CSLSite.WebRef_IngresoIEE.basicInvokeRequestBody Body;
        
        public basicInvokeRequest() {
        }
        
        public basicInvokeRequest(CSLSite.WebRef_IngresoIEE.basicInvokeRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://argo.navis.com/webservice/external")]
    public partial class basicInvokeRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string scopeCoordinateIds;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string xmlDoc;
        
        public basicInvokeRequestBody() {
        }
        
        public basicInvokeRequestBody(string scopeCoordinateIds, string xmlDoc) {
            this.scopeCoordinateIds = scopeCoordinateIds;
            this.xmlDoc = xmlDoc;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class basicInvokeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="basicInvokeResponse", Namespace="http://argo.navis.com/webservice/external", Order=0)]
        public CSLSite.WebRef_IngresoIEE.basicInvokeResponseBody Body;
        
        public basicInvokeResponse() {
        }
        
        public basicInvokeResponse(CSLSite.WebRef_IngresoIEE.basicInvokeResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://argo.navis.com/webservice/external")]
    public partial class basicInvokeResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string basicInvokeResult;
        
        public basicInvokeResponseBody() {
        }
        
        public basicInvokeResponseBody(string basicInvokeResult) {
            this.basicInvokeResult = basicInvokeResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface n4ServiceSoapChannel : CSLSite.WebRef_IngresoIEE.n4ServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class n4ServiceSoapClient : System.ServiceModel.ClientBase<CSLSite.WebRef_IngresoIEE.n4ServiceSoap>, CSLSite.WebRef_IngresoIEE.n4ServiceSoap {
        
        public n4ServiceSoapClient() {
        }
        
        public n4ServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public n4ServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public n4ServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public n4ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CSLSite.WebRef_IngresoIEE.basicInvokeResponse CSLSite.WebRef_IngresoIEE.n4ServiceSoap.basicInvoke(CSLSite.WebRef_IngresoIEE.basicInvokeRequest request) {
            return base.Channel.basicInvoke(request);
        }
        
        public string basicInvoke(string scopeCoordinateIds, string xmlDoc) {
            CSLSite.WebRef_IngresoIEE.basicInvokeRequest inValue = new CSLSite.WebRef_IngresoIEE.basicInvokeRequest();
            inValue.Body = new CSLSite.WebRef_IngresoIEE.basicInvokeRequestBody();
            inValue.Body.scopeCoordinateIds = scopeCoordinateIds;
            inValue.Body.xmlDoc = xmlDoc;
            CSLSite.WebRef_IngresoIEE.basicInvokeResponse retVal = ((CSLSite.WebRef_IngresoIEE.n4ServiceSoap)(this)).basicInvoke(inValue);
            return retVal.Body.basicInvokeResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CSLSite.WebRef_IngresoIEE.basicInvokeResponse> CSLSite.WebRef_IngresoIEE.n4ServiceSoap.basicInvokeAsync(CSLSite.WebRef_IngresoIEE.basicInvokeRequest request) {
            return base.Channel.basicInvokeAsync(request);
        }
        
        public System.Threading.Tasks.Task<CSLSite.WebRef_IngresoIEE.basicInvokeResponse> basicInvokeAsync(string scopeCoordinateIds, string xmlDoc) {
            CSLSite.WebRef_IngresoIEE.basicInvokeRequest inValue = new CSLSite.WebRef_IngresoIEE.basicInvokeRequest();
            inValue.Body = new CSLSite.WebRef_IngresoIEE.basicInvokeRequestBody();
            inValue.Body.scopeCoordinateIds = scopeCoordinateIds;
            inValue.Body.xmlDoc = xmlDoc;
            return ((CSLSite.WebRef_IngresoIEE.n4ServiceSoap)(this)).basicInvokeAsync(inValue);
        }
    }
}
