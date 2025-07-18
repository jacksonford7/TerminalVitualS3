﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.1008
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClSPOLTOIS
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="ecuapass")]
	public partial class DBMECUAPASSDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Definiciones de métodos de extensibilidad
    partial void OnCreated();
    partial void InsertECU_RIDT(ECU_RIDT instance);
    partial void UpdateECU_RIDT(ECU_RIDT instance);
    partial void DeleteECU_RIDT(ECU_RIDT instance);
    #endregion
		
		public DBMECUAPASSDataContext() : 
				base(global::ClSPOLTOIS.Properties.Settings.Default.ecuapassConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DBMECUAPASSDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBMECUAPASSDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBMECUAPASSDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBMECUAPASSDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ECU_RIDT> ECU_RIDT
		{
			get
			{
				return this.GetTable<ECU_RIDT>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ECU_RIDT")]
	public partial class ECU_RIDT : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _CODIGO_RIDT;
		
		private long _CODIGO_RESPUESTA;
		
		private System.Nullable<System.DateTime> _ACCEPT_DATE_TIME;
		
		private string _MRN;
		
		private string _MSN;
		
		private string _HSN;
		
		private string _STATUS_CODE;
		
		private string _NOMBRE_IMPORTADOR;
		
		private string _ID_IMPORTADOR;
		
		private string _NOMBRE_AGENTE;
		
		private string _ID_AGENTE;
		
		private string _CARRY_OUT_TYPE_CODE;
		
		private string _BL;
		
		private string _NUMERO_DECLARACION;
		
		private string _USUARIO_REGISTRA;
		
		private System.Nullable<System.DateTime> _FECHA_PROCESAMIENTO;
		
		private System.Nullable<char> _ESTADO;
		
		private string _COMENTARIOS;
		
		private System.Nullable<System.DateTime> _FECHA_REGISTRO;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCODIGO_RIDTChanging(long value);
    partial void OnCODIGO_RIDTChanged();
    partial void OnCODIGO_RESPUESTAChanging(long value);
    partial void OnCODIGO_RESPUESTAChanged();
    partial void OnACCEPT_DATE_TIMEChanging(System.Nullable<System.DateTime> value);
    partial void OnACCEPT_DATE_TIMEChanged();
    partial void OnMRNChanging(string value);
    partial void OnMRNChanged();
    partial void OnMSNChanging(string value);
    partial void OnMSNChanged();
    partial void OnHSNChanging(string value);
    partial void OnHSNChanged();
    partial void OnSTATUS_CODEChanging(string value);
    partial void OnSTATUS_CODEChanged();
    partial void OnNOMBRE_IMPORTADORChanging(string value);
    partial void OnNOMBRE_IMPORTADORChanged();
    partial void OnID_IMPORTADORChanging(string value);
    partial void OnID_IMPORTADORChanged();
    partial void OnNOMBRE_AGENTEChanging(string value);
    partial void OnNOMBRE_AGENTEChanged();
    partial void OnID_AGENTEChanging(string value);
    partial void OnID_AGENTEChanged();
    partial void OnCARRY_OUT_TYPE_CODEChanging(string value);
    partial void OnCARRY_OUT_TYPE_CODEChanged();
    partial void OnBLChanging(string value);
    partial void OnBLChanged();
    partial void OnNUMERO_DECLARACIONChanging(string value);
    partial void OnNUMERO_DECLARACIONChanged();
    partial void OnUSUARIO_REGISTRAChanging(string value);
    partial void OnUSUARIO_REGISTRAChanged();
    partial void OnFECHA_PROCESAMIENTOChanging(System.Nullable<System.DateTime> value);
    partial void OnFECHA_PROCESAMIENTOChanged();
    partial void OnESTADOChanging(System.Nullable<char> value);
    partial void OnESTADOChanged();
    partial void OnCOMENTARIOSChanging(string value);
    partial void OnCOMENTARIOSChanged();
    partial void OnFECHA_REGISTROChanging(System.Nullable<System.DateTime> value);
    partial void OnFECHA_REGISTROChanged();
    #endregion
		
		public ECU_RIDT()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CODIGO_RIDT", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long CODIGO_RIDT
		{
			get
			{
				return this._CODIGO_RIDT;
			}
			set
			{
				if ((this._CODIGO_RIDT != value))
				{
					this.OnCODIGO_RIDTChanging(value);
					this.SendPropertyChanging();
					this._CODIGO_RIDT = value;
					this.SendPropertyChanged("CODIGO_RIDT");
					this.OnCODIGO_RIDTChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CODIGO_RESPUESTA", DbType="BigInt NOT NULL")]
		public long CODIGO_RESPUESTA
		{
			get
			{
				return this._CODIGO_RESPUESTA;
			}
			set
			{
				if ((this._CODIGO_RESPUESTA != value))
				{
					this.OnCODIGO_RESPUESTAChanging(value);
					this.SendPropertyChanging();
					this._CODIGO_RESPUESTA = value;
					this.SendPropertyChanged("CODIGO_RESPUESTA");
					this.OnCODIGO_RESPUESTAChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ACCEPT_DATE_TIME", DbType="DateTime2")]
		public System.Nullable<System.DateTime> ACCEPT_DATE_TIME
		{
			get
			{
				return this._ACCEPT_DATE_TIME;
			}
			set
			{
				if ((this._ACCEPT_DATE_TIME != value))
				{
					this.OnACCEPT_DATE_TIMEChanging(value);
					this.SendPropertyChanging();
					this._ACCEPT_DATE_TIME = value;
					this.SendPropertyChanged("ACCEPT_DATE_TIME");
					this.OnACCEPT_DATE_TIMEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MRN", DbType="VarChar(30)")]
		public string MRN
		{
			get
			{
				return this._MRN;
			}
			set
			{
				if ((this._MRN != value))
				{
					this.OnMRNChanging(value);
					this.SendPropertyChanging();
					this._MRN = value;
					this.SendPropertyChanged("MRN");
					this.OnMRNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MSN", DbType="VarChar(30)")]
		public string MSN
		{
			get
			{
				return this._MSN;
			}
			set
			{
				if ((this._MSN != value))
				{
					this.OnMSNChanging(value);
					this.SendPropertyChanging();
					this._MSN = value;
					this.SendPropertyChanged("MSN");
					this.OnMSNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HSN", DbType="VarChar(30)")]
		public string HSN
		{
			get
			{
				return this._HSN;
			}
			set
			{
				if ((this._HSN != value))
				{
					this.OnHSNChanging(value);
					this.SendPropertyChanging();
					this._HSN = value;
					this.SendPropertyChanged("HSN");
					this.OnHSNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_STATUS_CODE", DbType="VarChar(5)")]
		public string STATUS_CODE
		{
			get
			{
				return this._STATUS_CODE;
			}
			set
			{
				if ((this._STATUS_CODE != value))
				{
					this.OnSTATUS_CODEChanging(value);
					this.SendPropertyChanging();
					this._STATUS_CODE = value;
					this.SendPropertyChanged("STATUS_CODE");
					this.OnSTATUS_CODEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NOMBRE_IMPORTADOR", DbType="VarChar(400)")]
		public string NOMBRE_IMPORTADOR
		{
			get
			{
				return this._NOMBRE_IMPORTADOR;
			}
			set
			{
				if ((this._NOMBRE_IMPORTADOR != value))
				{
					this.OnNOMBRE_IMPORTADORChanging(value);
					this.SendPropertyChanging();
					this._NOMBRE_IMPORTADOR = value;
					this.SendPropertyChanged("NOMBRE_IMPORTADOR");
					this.OnNOMBRE_IMPORTADORChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID_IMPORTADOR", DbType="VarChar(20)")]
		public string ID_IMPORTADOR
		{
			get
			{
				return this._ID_IMPORTADOR;
			}
			set
			{
				if ((this._ID_IMPORTADOR != value))
				{
					this.OnID_IMPORTADORChanging(value);
					this.SendPropertyChanging();
					this._ID_IMPORTADOR = value;
					this.SendPropertyChanged("ID_IMPORTADOR");
					this.OnID_IMPORTADORChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NOMBRE_AGENTE", DbType="VarChar(400)")]
		public string NOMBRE_AGENTE
		{
			get
			{
				return this._NOMBRE_AGENTE;
			}
			set
			{
				if ((this._NOMBRE_AGENTE != value))
				{
					this.OnNOMBRE_AGENTEChanging(value);
					this.SendPropertyChanging();
					this._NOMBRE_AGENTE = value;
					this.SendPropertyChanged("NOMBRE_AGENTE");
					this.OnNOMBRE_AGENTEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID_AGENTE", DbType="VarChar(20)")]
		public string ID_AGENTE
		{
			get
			{
				return this._ID_AGENTE;
			}
			set
			{
				if ((this._ID_AGENTE != value))
				{
					this.OnID_AGENTEChanging(value);
					this.SendPropertyChanging();
					this._ID_AGENTE = value;
					this.SendPropertyChanged("ID_AGENTE");
					this.OnID_AGENTEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CARRY_OUT_TYPE_CODE", DbType="VarChar(5)")]
		public string CARRY_OUT_TYPE_CODE
		{
			get
			{
				return this._CARRY_OUT_TYPE_CODE;
			}
			set
			{
				if ((this._CARRY_OUT_TYPE_CODE != value))
				{
					this.OnCARRY_OUT_TYPE_CODEChanging(value);
					this.SendPropertyChanging();
					this._CARRY_OUT_TYPE_CODE = value;
					this.SendPropertyChanged("CARRY_OUT_TYPE_CODE");
					this.OnCARRY_OUT_TYPE_CODEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BL", DbType="VarChar(30)")]
		public string BL
		{
			get
			{
				return this._BL;
			}
			set
			{
				if ((this._BL != value))
				{
					this.OnBLChanging(value);
					this.SendPropertyChanging();
					this._BL = value;
					this.SendPropertyChanged("BL");
					this.OnBLChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NUMERO_DECLARACION", DbType="VarChar(30)")]
		public string NUMERO_DECLARACION
		{
			get
			{
				return this._NUMERO_DECLARACION;
			}
			set
			{
				if ((this._NUMERO_DECLARACION != value))
				{
					this.OnNUMERO_DECLARACIONChanging(value);
					this.SendPropertyChanging();
					this._NUMERO_DECLARACION = value;
					this.SendPropertyChanged("NUMERO_DECLARACION");
					this.OnNUMERO_DECLARACIONChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_USUARIO_REGISTRA", DbType="VarChar(20)")]
		public string USUARIO_REGISTRA
		{
			get
			{
				return this._USUARIO_REGISTRA;
			}
			set
			{
				if ((this._USUARIO_REGISTRA != value))
				{
					this.OnUSUARIO_REGISTRAChanging(value);
					this.SendPropertyChanging();
					this._USUARIO_REGISTRA = value;
					this.SendPropertyChanged("USUARIO_REGISTRA");
					this.OnUSUARIO_REGISTRAChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FECHA_PROCESAMIENTO", DbType="DateTime2")]
		public System.Nullable<System.DateTime> FECHA_PROCESAMIENTO
		{
			get
			{
				return this._FECHA_PROCESAMIENTO;
			}
			set
			{
				if ((this._FECHA_PROCESAMIENTO != value))
				{
					this.OnFECHA_PROCESAMIENTOChanging(value);
					this.SendPropertyChanging();
					this._FECHA_PROCESAMIENTO = value;
					this.SendPropertyChanged("FECHA_PROCESAMIENTO");
					this.OnFECHA_PROCESAMIENTOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ESTADO", DbType="Char(1)")]
		public System.Nullable<char> ESTADO
		{
			get
			{
				return this._ESTADO;
			}
			set
			{
				if ((this._ESTADO != value))
				{
					this.OnESTADOChanging(value);
					this.SendPropertyChanging();
					this._ESTADO = value;
					this.SendPropertyChanged("ESTADO");
					this.OnESTADOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_COMENTARIOS", DbType="VarChar(2000)")]
		public string COMENTARIOS
		{
			get
			{
				return this._COMENTARIOS;
			}
			set
			{
				if ((this._COMENTARIOS != value))
				{
					this.OnCOMENTARIOSChanging(value);
					this.SendPropertyChanging();
					this._COMENTARIOS = value;
					this.SendPropertyChanged("COMENTARIOS");
					this.OnCOMENTARIOSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FECHA_REGISTRO", DbType="DateTime2")]
		public System.Nullable<System.DateTime> FECHA_REGISTRO
		{
			get
			{
				return this._FECHA_REGISTRO;
			}
			set
			{
				if ((this._FECHA_REGISTRO != value))
				{
					this.OnFECHA_REGISTROChanging(value);
					this.SendPropertyChanging();
					this._FECHA_REGISTRO = value;
					this.SendPropertyChanged("FECHA_REGISTRO");
					this.OnFECHA_REGISTROChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
