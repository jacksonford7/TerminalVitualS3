﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace ServicioEcuapass.AccesoDatos
{
    #region Contextos
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    public partial class N4MiddlewareEntities : ObjectContext
    {
        #region Constructores
    
        /// <summary>
        /// Inicializa un nuevo objeto N4MiddlewareEntities usando la cadena de conexión encontrada en la sección 'N4MiddlewareEntities' del archivo de configuración de la aplicación.
        /// </summary>
        public N4MiddlewareEntities() : base("name=N4MiddlewareEntities", "N4MiddlewareEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto N4MiddlewareEntities.
        /// </summary>
        public N4MiddlewareEntities(string connectionString) : base(connectionString, "N4MiddlewareEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto N4MiddlewareEntities.
        /// </summary>
        public N4MiddlewareEntities(EntityConnection connection) : base(connection, "N4MiddlewareEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Métodos parciales
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Propiedades de ObjectSet
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<CLIENTS_BILL_SAP> CLIENTS_BILL_SAP
        {
            get
            {
                if ((_CLIENTS_BILL_SAP == null))
                {
                    _CLIENTS_BILL_SAP = base.CreateObjectSet<CLIENTS_BILL_SAP>("CLIENTS_BILL_SAP");
                }
                return _CLIENTS_BILL_SAP;
            }
        }
        private ObjectSet<CLIENTS_BILL_SAP> _CLIENTS_BILL_SAP;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<CLIENTS_BILL> CLIENTS_BILL
        {
            get
            {
                if ((_CLIENTS_BILL == null))
                {
                    _CLIENTS_BILL = base.CreateObjectSet<CLIENTS_BILL>("CLIENTS_BILL");
                }
                return _CLIENTS_BILL;
            }
        }
        private ObjectSet<CLIENTS_BILL> _CLIENTS_BILL;

        #endregion

        #region Métodos AddTo
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet CLIENTS_BILL_SAP. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToCLIENTS_BILL_SAP(CLIENTS_BILL_SAP cLIENTS_BILL_SAP)
        {
            base.AddObject("CLIENTS_BILL_SAP", cLIENTS_BILL_SAP);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet CLIENTS_BILL. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddToCLIENTS_BILL(CLIENTS_BILL cLIENTS_BILL)
        {
            base.AddObject("CLIENTS_BILL", cLIENTS_BILL);
        }

        #endregion

    }

    #endregion

    #region Entidades
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="N4MiddlewareModel", Name="CLIENTS_BILL")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CLIENTS_BILL : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto CLIENTS_BILL.
        /// </summary>
        /// <param name="cLNT_CUSTOMER">Valor inicial de la propiedad CLNT_CUSTOMER.</param>
        /// <param name="cLNT_EBILLING">Valor inicial de la propiedad CLNT_EBILLING.</param>
        /// <param name="cLNT_RFC">Valor inicial de la propiedad CLNT_RFC.</param>
        /// <param name="cLNT_ACTIVE">Valor inicial de la propiedad CLNT_ACTIVE.</param>
        /// <param name="rOLE">Valor inicial de la propiedad ROLE.</param>
        /// <param name="cODIGO_SAP">Valor inicial de la propiedad CODIGO_SAP.</param>
        /// <param name="cLNT_DIA_CREDITO">Valor inicial de la propiedad CLNT_DIA_CREDITO.</param>
        public static CLIENTS_BILL CreateCLIENTS_BILL(global::System.String cLNT_CUSTOMER, global::System.String cLNT_EBILLING, global::System.String cLNT_RFC, global::System.String cLNT_ACTIVE, global::System.String rOLE, global::System.String cODIGO_SAP, global::System.Int32 cLNT_DIA_CREDITO)
        {
            CLIENTS_BILL cLIENTS_BILL = new CLIENTS_BILL();
            cLIENTS_BILL.CLNT_CUSTOMER = cLNT_CUSTOMER;
            cLIENTS_BILL.CLNT_EBILLING = cLNT_EBILLING;
            cLIENTS_BILL.CLNT_RFC = cLNT_RFC;
            cLIENTS_BILL.CLNT_ACTIVE = cLNT_ACTIVE;
            cLIENTS_BILL.ROLE = rOLE;
            cLIENTS_BILL.CODIGO_SAP = cODIGO_SAP;
            cLIENTS_BILL.CLNT_DIA_CREDITO = cLNT_DIA_CREDITO;
            return cLIENTS_BILL;
        }

        #endregion

        #region Propiedades simples
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_CUSTOMER
        {
            get
            {
                return _CLNT_CUSTOMER;
            }
            set
            {
                if (_CLNT_CUSTOMER != value)
                {
                    OnCLNT_CUSTOMERChanging(value);
                    ReportPropertyChanging("CLNT_CUSTOMER");
                    _CLNT_CUSTOMER = StructuralObject.SetValidValue(value, false, "CLNT_CUSTOMER");
                    ReportPropertyChanged("CLNT_CUSTOMER");
                    OnCLNT_CUSTOMERChanged();
                }
            }
        }
        private global::System.String _CLNT_CUSTOMER;
        partial void OnCLNT_CUSTOMERChanging(global::System.String value);
        partial void OnCLNT_CUSTOMERChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_NAME
        {
            get
            {
                return _CLNT_NAME;
            }
            set
            {
                OnCLNT_NAMEChanging(value);
                ReportPropertyChanging("CLNT_NAME");
                _CLNT_NAME = StructuralObject.SetValidValue(value, true, "CLNT_NAME");
                ReportPropertyChanged("CLNT_NAME");
                OnCLNT_NAMEChanged();
            }
        }
        private global::System.String _CLNT_NAME;
        partial void OnCLNT_NAMEChanging(global::System.String value);
        partial void OnCLNT_NAMEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_CITY
        {
            get
            {
                return _CLNT_CITY;
            }
            set
            {
                OnCLNT_CITYChanging(value);
                ReportPropertyChanging("CLNT_CITY");
                _CLNT_CITY = StructuralObject.SetValidValue(value, true, "CLNT_CITY");
                ReportPropertyChanged("CLNT_CITY");
                OnCLNT_CITYChanged();
            }
        }
        private global::System.String _CLNT_CITY;
        partial void OnCLNT_CITYChanging(global::System.String value);
        partial void OnCLNT_CITYChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> CLNT_STATE
        {
            get
            {
                return _CLNT_STATE;
            }
            set
            {
                OnCLNT_STATEChanging(value);
                ReportPropertyChanging("CLNT_STATE");
                _CLNT_STATE = StructuralObject.SetValidValue(value, "CLNT_STATE");
                ReportPropertyChanged("CLNT_STATE");
                OnCLNT_STATEChanged();
            }
        }
        private Nullable<global::System.Int32> _CLNT_STATE;
        partial void OnCLNT_STATEChanging(Nullable<global::System.Int32> value);
        partial void OnCLNT_STATEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_ADRESS1
        {
            get
            {
                return _CLNT_ADRESS1;
            }
            set
            {
                OnCLNT_ADRESS1Changing(value);
                ReportPropertyChanging("CLNT_ADRESS1");
                _CLNT_ADRESS1 = StructuralObject.SetValidValue(value, true, "CLNT_ADRESS1");
                ReportPropertyChanged("CLNT_ADRESS1");
                OnCLNT_ADRESS1Changed();
            }
        }
        private global::System.String _CLNT_ADRESS1;
        partial void OnCLNT_ADRESS1Changing(global::System.String value);
        partial void OnCLNT_ADRESS1Changed();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> CLNT_TRANSACTION_DATE
        {
            get
            {
                return _CLNT_TRANSACTION_DATE;
            }
            set
            {
                OnCLNT_TRANSACTION_DATEChanging(value);
                ReportPropertyChanging("CLNT_TRANSACTION_DATE");
                _CLNT_TRANSACTION_DATE = StructuralObject.SetValidValue(value, "CLNT_TRANSACTION_DATE");
                ReportPropertyChanged("CLNT_TRANSACTION_DATE");
                OnCLNT_TRANSACTION_DATEChanged();
            }
        }
        private Nullable<global::System.DateTime> _CLNT_TRANSACTION_DATE;
        partial void OnCLNT_TRANSACTION_DATEChanging(Nullable<global::System.DateTime> value);
        partial void OnCLNT_TRANSACTION_DATEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_EMAIL
        {
            get
            {
                return _CLNT_EMAIL;
            }
            set
            {
                OnCLNT_EMAILChanging(value);
                ReportPropertyChanging("CLNT_EMAIL");
                _CLNT_EMAIL = StructuralObject.SetValidValue(value, true, "CLNT_EMAIL");
                ReportPropertyChanged("CLNT_EMAIL");
                OnCLNT_EMAILChanged();
            }
        }
        private global::System.String _CLNT_EMAIL;
        partial void OnCLNT_EMAILChanging(global::System.String value);
        partial void OnCLNT_EMAILChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_TYPE
        {
            get
            {
                return _CLNT_TYPE;
            }
            set
            {
                OnCLNT_TYPEChanging(value);
                ReportPropertyChanging("CLNT_TYPE");
                _CLNT_TYPE = StructuralObject.SetValidValue(value, true, "CLNT_TYPE");
                ReportPropertyChanged("CLNT_TYPE");
                OnCLNT_TYPEChanged();
            }
        }
        private global::System.String _CLNT_TYPE;
        partial void OnCLNT_TYPEChanging(global::System.String value);
        partial void OnCLNT_TYPEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_EBILLING
        {
            get
            {
                return _CLNT_EBILLING;
            }
            set
            {
                if (_CLNT_EBILLING != value)
                {
                    OnCLNT_EBILLINGChanging(value);
                    ReportPropertyChanging("CLNT_EBILLING");
                    _CLNT_EBILLING = StructuralObject.SetValidValue(value, false, "CLNT_EBILLING");
                    ReportPropertyChanged("CLNT_EBILLING");
                    OnCLNT_EBILLINGChanged();
                }
            }
        }
        private global::System.String _CLNT_EBILLING;
        partial void OnCLNT_EBILLINGChanging(global::System.String value);
        partial void OnCLNT_EBILLINGChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_FAX_INVC
        {
            get
            {
                return _CLNT_FAX_INVC;
            }
            set
            {
                OnCLNT_FAX_INVCChanging(value);
                ReportPropertyChanging("CLNT_FAX_INVC");
                _CLNT_FAX_INVC = StructuralObject.SetValidValue(value, true, "CLNT_FAX_INVC");
                ReportPropertyChanged("CLNT_FAX_INVC");
                OnCLNT_FAX_INVCChanged();
            }
        }
        private global::System.String _CLNT_FAX_INVC;
        partial void OnCLNT_FAX_INVCChanging(global::System.String value);
        partial void OnCLNT_FAX_INVCChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_RFC
        {
            get
            {
                return _CLNT_RFC;
            }
            set
            {
                if (_CLNT_RFC != value)
                {
                    OnCLNT_RFCChanging(value);
                    ReportPropertyChanging("CLNT_RFC");
                    _CLNT_RFC = StructuralObject.SetValidValue(value, false, "CLNT_RFC");
                    ReportPropertyChanged("CLNT_RFC");
                    OnCLNT_RFCChanged();
                }
            }
        }
        private global::System.String _CLNT_RFC;
        partial void OnCLNT_RFCChanging(global::System.String value);
        partial void OnCLNT_RFCChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_ACTIVE
        {
            get
            {
                return _CLNT_ACTIVE;
            }
            set
            {
                if (_CLNT_ACTIVE != value)
                {
                    OnCLNT_ACTIVEChanging(value);
                    ReportPropertyChanging("CLNT_ACTIVE");
                    _CLNT_ACTIVE = StructuralObject.SetValidValue(value, false, "CLNT_ACTIVE");
                    ReportPropertyChanged("CLNT_ACTIVE");
                    OnCLNT_ACTIVEChanged();
                }
            }
        }
        private global::System.String _CLNT_ACTIVE;
        partial void OnCLNT_ACTIVEChanging(global::System.String value);
        partial void OnCLNT_ACTIVEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> CLNT_CONT_CONSECUTIVO
        {
            get
            {
                return _CLNT_CONT_CONSECUTIVO;
            }
            set
            {
                OnCLNT_CONT_CONSECUTIVOChanging(value);
                ReportPropertyChanging("CLNT_CONT_CONSECUTIVO");
                _CLNT_CONT_CONSECUTIVO = StructuralObject.SetValidValue(value, "CLNT_CONT_CONSECUTIVO");
                ReportPropertyChanged("CLNT_CONT_CONSECUTIVO");
                OnCLNT_CONT_CONSECUTIVOChanged();
            }
        }
        private Nullable<global::System.Int32> _CLNT_CONT_CONSECUTIVO;
        partial void OnCLNT_CONT_CONSECUTIVOChanging(Nullable<global::System.Int32> value);
        partial void OnCLNT_CONT_CONSECUTIVOChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String ROLE
        {
            get
            {
                return _ROLE;
            }
            set
            {
                if (_ROLE != value)
                {
                    OnROLEChanging(value);
                    ReportPropertyChanging("ROLE");
                    _ROLE = StructuralObject.SetValidValue(value, false, "ROLE");
                    ReportPropertyChanged("ROLE");
                    OnROLEChanged();
                }
            }
        }
        private global::System.String _ROLE;
        partial void OnROLEChanging(global::System.String value);
        partial void OnROLEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CODIGO_SAP
        {
            get
            {
                return _CODIGO_SAP;
            }
            set
            {
                if (_CODIGO_SAP != value)
                {
                    OnCODIGO_SAPChanging(value);
                    ReportPropertyChanging("CODIGO_SAP");
                    _CODIGO_SAP = StructuralObject.SetValidValue(value, false, "CODIGO_SAP");
                    ReportPropertyChanged("CODIGO_SAP");
                    OnCODIGO_SAPChanged();
                }
            }
        }
        private global::System.String _CODIGO_SAP;
        partial void OnCODIGO_SAPChanging(global::System.String value);
        partial void OnCODIGO_SAPChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 CLNT_DIA_CREDITO
        {
            get
            {
                return _CLNT_DIA_CREDITO;
            }
            set
            {
                if (_CLNT_DIA_CREDITO != value)
                {
                    OnCLNT_DIA_CREDITOChanging(value);
                    ReportPropertyChanging("CLNT_DIA_CREDITO");
                    _CLNT_DIA_CREDITO = StructuralObject.SetValidValue(value, "CLNT_DIA_CREDITO");
                    ReportPropertyChanged("CLNT_DIA_CREDITO");
                    OnCLNT_DIA_CREDITOChanged();
                }
            }
        }
        private global::System.Int32 _CLNT_DIA_CREDITO;
        partial void OnCLNT_DIA_CREDITOChanging(global::System.Int32 value);
        partial void OnCLNT_DIA_CREDITOChanged();

        #endregion

    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="N4MiddlewareModel", Name="CLIENTS_BILL_SAP")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CLIENTS_BILL_SAP : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto CLIENTS_BILL_SAP.
        /// </summary>
        /// <param name="cLNT_CUSTOMER">Valor inicial de la propiedad CLNT_CUSTOMER.</param>
        /// <param name="cLNT_EBILLING">Valor inicial de la propiedad CLNT_EBILLING.</param>
        /// <param name="cLNT_RFC">Valor inicial de la propiedad CLNT_RFC.</param>
        /// <param name="cLNT_ACTIVE">Valor inicial de la propiedad CLNT_ACTIVE.</param>
        /// <param name="rOLE">Valor inicial de la propiedad ROLE.</param>
        /// <param name="cLNT_DIA_CREDITO">Valor inicial de la propiedad CLNT_DIA_CREDITO.</param>
        public static CLIENTS_BILL_SAP CreateCLIENTS_BILL_SAP(global::System.String cLNT_CUSTOMER, global::System.String cLNT_EBILLING, global::System.String cLNT_RFC, global::System.String cLNT_ACTIVE, global::System.String rOLE, global::System.Int64 cLNT_DIA_CREDITO)
        {
            CLIENTS_BILL_SAP cLIENTS_BILL_SAP = new CLIENTS_BILL_SAP();
            cLIENTS_BILL_SAP.CLNT_CUSTOMER = cLNT_CUSTOMER;
            cLIENTS_BILL_SAP.CLNT_EBILLING = cLNT_EBILLING;
            cLIENTS_BILL_SAP.CLNT_RFC = cLNT_RFC;
            cLIENTS_BILL_SAP.CLNT_ACTIVE = cLNT_ACTIVE;
            cLIENTS_BILL_SAP.ROLE = rOLE;
            cLIENTS_BILL_SAP.CLNT_DIA_CREDITO = cLNT_DIA_CREDITO;
            return cLIENTS_BILL_SAP;
        }

        #endregion

        #region Propiedades simples
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_CUSTOMER
        {
            get
            {
                return _CLNT_CUSTOMER;
            }
            set
            {
                if (_CLNT_CUSTOMER != value)
                {
                    OnCLNT_CUSTOMERChanging(value);
                    ReportPropertyChanging("CLNT_CUSTOMER");
                    _CLNT_CUSTOMER = StructuralObject.SetValidValue(value, false, "CLNT_CUSTOMER");
                    ReportPropertyChanged("CLNT_CUSTOMER");
                    OnCLNT_CUSTOMERChanged();
                }
            }
        }
        private global::System.String _CLNT_CUSTOMER;
        partial void OnCLNT_CUSTOMERChanging(global::System.String value);
        partial void OnCLNT_CUSTOMERChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_NAME
        {
            get
            {
                return _CLNT_NAME;
            }
            set
            {
                OnCLNT_NAMEChanging(value);
                ReportPropertyChanging("CLNT_NAME");
                _CLNT_NAME = StructuralObject.SetValidValue(value, true, "CLNT_NAME");
                ReportPropertyChanged("CLNT_NAME");
                OnCLNT_NAMEChanged();
            }
        }
        private global::System.String _CLNT_NAME;
        partial void OnCLNT_NAMEChanging(global::System.String value);
        partial void OnCLNT_NAMEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_CITY
        {
            get
            {
                return _CLNT_CITY;
            }
            set
            {
                OnCLNT_CITYChanging(value);
                ReportPropertyChanging("CLNT_CITY");
                _CLNT_CITY = StructuralObject.SetValidValue(value, true, "CLNT_CITY");
                ReportPropertyChanged("CLNT_CITY");
                OnCLNT_CITYChanged();
            }
        }
        private global::System.String _CLNT_CITY;
        partial void OnCLNT_CITYChanging(global::System.String value);
        partial void OnCLNT_CITYChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> CLNT_STATE
        {
            get
            {
                return _CLNT_STATE;
            }
            set
            {
                OnCLNT_STATEChanging(value);
                ReportPropertyChanging("CLNT_STATE");
                _CLNT_STATE = StructuralObject.SetValidValue(value, "CLNT_STATE");
                ReportPropertyChanged("CLNT_STATE");
                OnCLNT_STATEChanged();
            }
        }
        private Nullable<global::System.Int32> _CLNT_STATE;
        partial void OnCLNT_STATEChanging(Nullable<global::System.Int32> value);
        partial void OnCLNT_STATEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_ADRESS1
        {
            get
            {
                return _CLNT_ADRESS1;
            }
            set
            {
                OnCLNT_ADRESS1Changing(value);
                ReportPropertyChanging("CLNT_ADRESS1");
                _CLNT_ADRESS1 = StructuralObject.SetValidValue(value, true, "CLNT_ADRESS1");
                ReportPropertyChanged("CLNT_ADRESS1");
                OnCLNT_ADRESS1Changed();
            }
        }
        private global::System.String _CLNT_ADRESS1;
        partial void OnCLNT_ADRESS1Changing(global::System.String value);
        partial void OnCLNT_ADRESS1Changed();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> CLNT_TRANSACTION_DATE
        {
            get
            {
                return _CLNT_TRANSACTION_DATE;
            }
            set
            {
                OnCLNT_TRANSACTION_DATEChanging(value);
                ReportPropertyChanging("CLNT_TRANSACTION_DATE");
                _CLNT_TRANSACTION_DATE = StructuralObject.SetValidValue(value, "CLNT_TRANSACTION_DATE");
                ReportPropertyChanged("CLNT_TRANSACTION_DATE");
                OnCLNT_TRANSACTION_DATEChanged();
            }
        }
        private Nullable<global::System.DateTime> _CLNT_TRANSACTION_DATE;
        partial void OnCLNT_TRANSACTION_DATEChanging(Nullable<global::System.DateTime> value);
        partial void OnCLNT_TRANSACTION_DATEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_EMAIL
        {
            get
            {
                return _CLNT_EMAIL;
            }
            set
            {
                OnCLNT_EMAILChanging(value);
                ReportPropertyChanging("CLNT_EMAIL");
                _CLNT_EMAIL = StructuralObject.SetValidValue(value, true, "CLNT_EMAIL");
                ReportPropertyChanged("CLNT_EMAIL");
                OnCLNT_EMAILChanged();
            }
        }
        private global::System.String _CLNT_EMAIL;
        partial void OnCLNT_EMAILChanging(global::System.String value);
        partial void OnCLNT_EMAILChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_TYPE
        {
            get
            {
                return _CLNT_TYPE;
            }
            set
            {
                OnCLNT_TYPEChanging(value);
                ReportPropertyChanging("CLNT_TYPE");
                _CLNT_TYPE = StructuralObject.SetValidValue(value, true, "CLNT_TYPE");
                ReportPropertyChanged("CLNT_TYPE");
                OnCLNT_TYPEChanged();
            }
        }
        private global::System.String _CLNT_TYPE;
        partial void OnCLNT_TYPEChanging(global::System.String value);
        partial void OnCLNT_TYPEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_EBILLING
        {
            get
            {
                return _CLNT_EBILLING;
            }
            set
            {
                if (_CLNT_EBILLING != value)
                {
                    OnCLNT_EBILLINGChanging(value);
                    ReportPropertyChanging("CLNT_EBILLING");
                    _CLNT_EBILLING = StructuralObject.SetValidValue(value, false, "CLNT_EBILLING");
                    ReportPropertyChanged("CLNT_EBILLING");
                    OnCLNT_EBILLINGChanged();
                }
            }
        }
        private global::System.String _CLNT_EBILLING;
        partial void OnCLNT_EBILLINGChanging(global::System.String value);
        partial void OnCLNT_EBILLINGChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CLNT_FAX_INVC
        {
            get
            {
                return _CLNT_FAX_INVC;
            }
            set
            {
                OnCLNT_FAX_INVCChanging(value);
                ReportPropertyChanging("CLNT_FAX_INVC");
                _CLNT_FAX_INVC = StructuralObject.SetValidValue(value, true, "CLNT_FAX_INVC");
                ReportPropertyChanged("CLNT_FAX_INVC");
                OnCLNT_FAX_INVCChanged();
            }
        }
        private global::System.String _CLNT_FAX_INVC;
        partial void OnCLNT_FAX_INVCChanging(global::System.String value);
        partial void OnCLNT_FAX_INVCChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_RFC
        {
            get
            {
                return _CLNT_RFC;
            }
            set
            {
                if (_CLNT_RFC != value)
                {
                    OnCLNT_RFCChanging(value);
                    ReportPropertyChanging("CLNT_RFC");
                    _CLNT_RFC = StructuralObject.SetValidValue(value, false, "CLNT_RFC");
                    ReportPropertyChanged("CLNT_RFC");
                    OnCLNT_RFCChanged();
                }
            }
        }
        private global::System.String _CLNT_RFC;
        partial void OnCLNT_RFCChanging(global::System.String value);
        partial void OnCLNT_RFCChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CLNT_ACTIVE
        {
            get
            {
                return _CLNT_ACTIVE;
            }
            set
            {
                if (_CLNT_ACTIVE != value)
                {
                    OnCLNT_ACTIVEChanging(value);
                    ReportPropertyChanging("CLNT_ACTIVE");
                    _CLNT_ACTIVE = StructuralObject.SetValidValue(value, false, "CLNT_ACTIVE");
                    ReportPropertyChanged("CLNT_ACTIVE");
                    OnCLNT_ACTIVEChanged();
                }
            }
        }
        private global::System.String _CLNT_ACTIVE;
        partial void OnCLNT_ACTIVEChanging(global::System.String value);
        partial void OnCLNT_ACTIVEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> CLNT_CONT_CONSECUTIVO
        {
            get
            {
                return _CLNT_CONT_CONSECUTIVO;
            }
            set
            {
                OnCLNT_CONT_CONSECUTIVOChanging(value);
                ReportPropertyChanging("CLNT_CONT_CONSECUTIVO");
                _CLNT_CONT_CONSECUTIVO = StructuralObject.SetValidValue(value, "CLNT_CONT_CONSECUTIVO");
                ReportPropertyChanged("CLNT_CONT_CONSECUTIVO");
                OnCLNT_CONT_CONSECUTIVOChanged();
            }
        }
        private Nullable<global::System.Int32> _CLNT_CONT_CONSECUTIVO;
        partial void OnCLNT_CONT_CONSECUTIVOChanging(Nullable<global::System.Int32> value);
        partial void OnCLNT_CONT_CONSECUTIVOChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String ROLE
        {
            get
            {
                return _ROLE;
            }
            set
            {
                if (_ROLE != value)
                {
                    OnROLEChanging(value);
                    ReportPropertyChanging("ROLE");
                    _ROLE = StructuralObject.SetValidValue(value, false, "ROLE");
                    ReportPropertyChanged("ROLE");
                    OnROLEChanged();
                }
            }
        }
        private global::System.String _ROLE;
        partial void OnROLEChanging(global::System.String value);
        partial void OnROLEChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CODIGO_SAP
        {
            get
            {
                return _CODIGO_SAP;
            }
            set
            {
                OnCODIGO_SAPChanging(value);
                ReportPropertyChanging("CODIGO_SAP");
                _CODIGO_SAP = StructuralObject.SetValidValue(value, true, "CODIGO_SAP");
                ReportPropertyChanged("CODIGO_SAP");
                OnCODIGO_SAPChanged();
            }
        }
        private global::System.String _CODIGO_SAP;
        partial void OnCODIGO_SAPChanging(global::System.String value);
        partial void OnCODIGO_SAPChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 CLNT_DIA_CREDITO
        {
            get
            {
                return _CLNT_DIA_CREDITO;
            }
            set
            {
                if (_CLNT_DIA_CREDITO != value)
                {
                    OnCLNT_DIA_CREDITOChanging(value);
                    ReportPropertyChanging("CLNT_DIA_CREDITO");
                    _CLNT_DIA_CREDITO = StructuralObject.SetValidValue(value, "CLNT_DIA_CREDITO");
                    ReportPropertyChanged("CLNT_DIA_CREDITO");
                    OnCLNT_DIA_CREDITOChanged();
                }
            }
        }
        private global::System.Int64 _CLNT_DIA_CREDITO;
        partial void OnCLNT_DIA_CREDITOChanging(global::System.Int64 value);
        partial void OnCLNT_DIA_CREDITOChanged();

        #endregion

    }

    #endregion

}
