﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.1022
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SysproCompanyC")]
	public partial class DBMSYSPRODataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Definiciones de métodos de extensibilidad
    partial void OnCreated();
    #endregion
		
		public DBMSYSPRODataContext() : 
				base(global::ClSPOLTOIS.Properties.Settings.Default.SysproCompanyCConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DBMSYSPRODataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBMSYSPRODataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBMSYSPRODataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBMSYSPRODataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SYP_PRO_CONTROL_CREDITO")]
		public int SYP_PRO_CONTROL_CREDITO([global::System.Data.Linq.Mapping.ParameterAttribute(Name="RUC_CLIENTE", DbType="VarChar(20)")] string rUC_CLIENTE, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TOTAL_FACT", DbType="Decimal(12,2)")] System.Nullable<decimal> tOTAL_FACT, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="FECHA_FACTURA", DbType="VarChar(10)")] ref string fECHA_FACTURA, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="F_MENSAJE", DbType="VarChar(1000)")] ref string f_MENSAJE)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), rUC_CLIENTE, tOTAL_FACT, fECHA_FACTURA, f_MENSAJE);
			fECHA_FACTURA = ((string)(result.GetParameterValue(2)));
			f_MENSAJE = ((string)(result.GetParameterValue(3)));
			return ((int)(result.ReturnValue));
		}
	}
}
#pragma warning restore 1591
