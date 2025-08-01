﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N4Ws.N4Esquema
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ArgoServiceSoapBinding", Namespace = "http://www.navis.com/services/argoservice")]
    public abstract class ArgoService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        private System.Threading.SendOrPostCallback genericInvokeOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public ArgoService()
        {
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event genericInvokeCompletedEventHandler genericInvokeCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("genericInvokeResponse", Namespace = "http://www.navis.com/services/argoservice")]
        public genericInvokeResponse genericInvoke([System.Xml.Serialization.XmlElementAttribute("genericInvoke", Namespace = "http://www.navis.com/services/argoservice")] genericInvoke genericInvoke1)
        {
            object[] results = this.Invoke("genericInvoke", new object[] {
                        genericInvoke1});
            return ((genericInvokeResponse)(results[0]));
        }

        /// <remarks/>
        public void genericInvokeAsync(genericInvoke genericInvoke1)
        {
            this.genericInvokeAsync(genericInvoke1, null);
        }

        /// <remarks/>
        public void genericInvokeAsync(genericInvoke genericInvoke1, object userState)
        {
            if ((this.genericInvokeOperationCompleted == null))
            {
                this.genericInvokeOperationCompleted = new System.Threading.SendOrPostCallback(this.OngenericInvokeOperationCompleted);
            }
            this.InvokeAsync("genericInvoke", new object[] {
                        genericInvoke1}, this.genericInvokeOperationCompleted, userState);
        }

        private void OngenericInvokeOperationCompleted(object arg)
        {
            if ((this.genericInvokeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.genericInvokeCompleted(this, new genericInvokeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.navis.com/services/argoservice")]
    public partial class genericInvoke
    {

        private ScopeCoordinateIdsWsType scopeCoordinateIdsWsTypeField;

        private string xmlDocField;

        /// <comentarios/>
        public ScopeCoordinateIdsWsType scopeCoordinateIdsWsType
        {
            get
            {
                return this.scopeCoordinateIdsWsTypeField;
            }
            set
            {
                this.scopeCoordinateIdsWsTypeField = value;
            }
        }

        /// <comentarios/>
        public string xmlDoc
        {
            get
            {
                return this.xmlDocField;
            }
            set
            {
                this.xmlDocField = value;
            }
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://types.webservice.argo.navis.com/v1.0")]
    public partial class ScopeCoordinateIdsWsType
    {

        private string operatorIdField;

        private string complexIdField;

        private string facilityIdField;

        private string yardIdField;

        private string externalUserIdField;

        /// <comentarios/>
        public string operatorId
        {
            get
            {
                return this.operatorIdField;
            }
            set
            {
                this.operatorIdField = value;
            }
        }

        /// <comentarios/>
        public string complexId
        {
            get
            {
                return this.complexIdField;
            }
            set
            {
                this.complexIdField = value;
            }
        }

        /// <comentarios/>
        public string facilityId
        {
            get
            {
                return this.facilityIdField;
            }
            set
            {
                this.facilityIdField = value;
            }
        }

        /// <comentarios/>
        public string yardId
        {
            get
            {
                return this.yardIdField;
            }
            set
            {
                this.yardIdField = value;
            }
        }

        /// <comentarios/>
        public string externalUserId
        {
            get
            {
                return this.externalUserIdField;
            }
            set
            {
                this.externalUserIdField = value;
            }
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://types.webservice.argo.navis.com/v1.0")]
    public partial class QueryResultType
    {

        private string resultField;

        /// <comentarios/>
        public string Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://types.webservice.argo.navis.com/v1.0")]
    public partial class MessageType
    {

        private string messageField;

        private string severityLevelField;

        /// <comentarios/>
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <comentarios/>
        public string SeverityLevel
        {
            get
            {
                return this.severityLevelField;
            }
            set
            {
                this.severityLevelField = value;
            }
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://types.webservice.argo.navis.com/v1.0")]
    public partial class ResponseType
    {

        private string statusField;

        private string statusDescriptionField;

        private MessageType[] messageCollectorField;

        private QueryResultType[] queryResultsField;

        /// <comentarios/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <comentarios/>
        public string StatusDescription
        {
            get
            {
                return this.statusDescriptionField;
            }
            set
            {
                this.statusDescriptionField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Messages", IsNullable = false)]
        public MessageType[] MessageCollector
        {
            get
            {
                return this.messageCollectorField;
            }
            set
            {
                this.messageCollectorField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("QueryResults")]
        public QueryResultType[] QueryResults
        {
            get
            {
                return this.queryResultsField;
            }
            set
            {
                this.queryResultsField = value;
            }
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://types.webservice.argo.navis.com/v1.0")]
    public partial class GenericInvokeResponseWsType
    {

        private ResponseType commonResponseField;

        private string statusField;

        private string responsePayLoadField;

        /// <comentarios/>
        public ResponseType commonResponse
        {
            get
            {
                return this.commonResponseField;
            }
            set
            {
                this.commonResponseField = value;
            }
        }

        /// <comentarios/>
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <comentarios/>
        public string responsePayLoad
        {
            get
            {
                return this.responsePayLoadField;
            }
            set
            {
                this.responsePayLoadField = value;
            }
        }
    }

    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.navis.com/services/argoservice")]
    public partial class genericInvokeResponse
    {

        private GenericInvokeResponseWsType genericInvokeResponse1Field;

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("genericInvokeResponse")]
        public GenericInvokeResponseWsType genericInvokeResponse1
        {
            get
            {
                return this.genericInvokeResponse1Field;
            }
            set
            {
                this.genericInvokeResponse1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void genericInvokeCompletedEventHandler(object sender, genericInvokeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class genericInvokeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal genericInvokeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public genericInvokeResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((genericInvokeResponse)(this.results[0]));
            }
        }
    }

}
