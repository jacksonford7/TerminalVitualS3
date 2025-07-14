using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using csl_log;

namespace CSLSite.app_start
{
    public class PagoEnLineaData
    {
        public string IngresoAnticipo(string idCliente, string nombreCliente, string monto, string usuario, string numeroBooking, string rol)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaIngreso(idCliente, nombreCliente, monto, usuario, numeroBooking, rol), conexion);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                return "";
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Add", idCliente, usuario);
                return "Ha ocurrido un error.";
            }
        }
        private string ArmarSentenciaIngreso(string idCliente, string nombreCliente, string monto, string usuario, string numeroBooking, string rol)
        {
            return string.Format("EXEC [ecuapass].[dbo].[ECU_INGRESO_ANTICIPO] '{0}','{1}',{2},'{3}','{4}',{5}", nombreCliente, idCliente, monto, usuario, string.IsNullOrWhiteSpace(numeroBooking) ? "" : numeroBooking,rol);
        }

        public DataTable ConsultaAnticipo(string idCliente, DateTime fechaDesde, DateTime fechaHasta, string numeroLiquidacion, string estado)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaConsulta(idCliente, fechaDesde, fechaHasta, numeroLiquidacion, estado), conexion);
                    conexion.Open();
                    var lector = comando.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(lector);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Query", idCliente, "");
                return null;
            }
        }
        private string ArmarSentenciaConsulta(string idCliente, DateTime fechaDesde, DateTime fechaHasta, string numeroLiquidacion, string estado)
        {
            if (string.IsNullOrWhiteSpace(numeroLiquidacion) && string.IsNullOrWhiteSpace(estado))
            {
                return string.Format("SELECT F.CODIGO_UNICO AS CODIGO_ANTICIPO, rank() OVER (ORDER BY F.NUMERO_LIQUIDACION DESC) as ITEM, CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,F.NUMERO_LIQUIDACION,F.FECHA_REGISTRO,ESTADO = CASE WHEN P.CODIGO_PAGO IS NULL THEN 'PENDIENTE' WHEN F.MONTO_TOTAL = (SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA_ANTICIPO = F.CODIGO_UNICO) THEN 'APLICADO' ELSE 'CONFIRMADO' END  FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F LEFT JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P ON F.CODIGO_UNICO = P.CODIGO_FACTURA WHERE F.CODIGO_UNICO <= 0 AND F.ESTADO != 'Z' AND F.NUMERO_IDENTIFICACION = '{0}' AND F.FECHA_REGISTRO BETWEEN '{1}-{2}-{3} 00:00:00' AND '{4}-{5}-{6} 23:59:59' order by ITEM "
                                        , idCliente, fechaDesde.Year, fechaDesde.Month.ToString("D2"), fechaDesde.Day.ToString("D2"),
                                        fechaHasta.Year, fechaHasta.Month.ToString("D2"), fechaHasta.Day.ToString("D2"));
            }
            if (string.IsNullOrWhiteSpace(numeroLiquidacion) && !string.IsNullOrWhiteSpace(estado))
            {
                return string.Format("SELECT * FROM (SELECT F.CODIGO_UNICO AS CODIGO_ANTICIPO, rank() OVER (ORDER BY F.NUMERO_LIQUIDACION DESC) as ITEM, CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,F.NUMERO_LIQUIDACION,F.FECHA_REGISTRO,ESTADO = CASE WHEN P.CODIGO_PAGO IS NULL THEN 'PENDIENTE' WHEN F.MONTO_TOTAL = (SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA_ANTICIPO = F.CODIGO_UNICO) THEN 'APLICADO' ELSE 'CONFIRMADO' END  FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F LEFT JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P ON F.CODIGO_UNICO = P.CODIGO_FACTURA WHERE F.CODIGO_UNICO <= 0 AND F.ESTADO != 'Z' AND F.NUMERO_IDENTIFICACION = '{0}' AND F.FECHA_REGISTRO BETWEEN '{1}-{2}-{3} 00:00:00' AND '{4}-{5}-{6} 23:59:59') A WHERE A.ESTADO = '{7}' order by A.ITEM"
                                        , idCliente, fechaDesde.Year, fechaDesde.Month.ToString("D2"), fechaDesde.Day.ToString("D2"),
                                        fechaHasta.Year, fechaHasta.Month.ToString("D2"), fechaHasta.Day.ToString("D2"),estado);
            }
            if (!string.IsNullOrWhiteSpace(numeroLiquidacion) && string.IsNullOrWhiteSpace(estado))
            {
                return string.Format("SELECT F.CODIGO_UNICO AS CODIGO_ANTICIPO, rank() OVER (ORDER BY F.NUMERO_LIQUIDACION DESC) as ITEM, CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,F.NUMERO_LIQUIDACION,F.FECHA_REGISTRO,ESTADO = CASE WHEN P.CODIGO_PAGO IS NULL THEN 'PENDIENTE' WHEN F.MONTO_TOTAL = (SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA_ANTICIPO = F.CODIGO_UNICO) THEN 'APLICADO' ELSE 'CONFIRMADO' END  FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F LEFT JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P ON F.CODIGO_UNICO = P.CODIGO_FACTURA WHERE F.CODIGO_UNICO <= 0 AND F.ESTADO != 'Z' AND F.NUMERO_IDENTIFICACION = '{0}' AND F.FECHA_REGISTRO BETWEEN '{1}-{2}-{3} 00:00:00' AND '{4}-{5}-{6} 23:59:59' AND F.NUMERO_LIQUIDACION = '{7}' order by ITEM"
                                        , idCliente, fechaDesde.Year, fechaDesde.Month.ToString("D2"), fechaDesde.Day.ToString("D2"),
                                        fechaHasta.Year, fechaHasta.Month.ToString("D2"), fechaHasta.Day.ToString("D2"), numeroLiquidacion);
            }
            if (!string.IsNullOrWhiteSpace(numeroLiquidacion) && !string.IsNullOrWhiteSpace(estado))
            {
                return string.Format("SELECT * FROM (SELECT F.CODIGO_UNICO AS CODIGO_ANTICIPO, rank() OVER (ORDER BY F.NUMERO_LIQUIDACION DESC) as ITEM, CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,F.NUMERO_LIQUIDACION,F.FECHA_REGISTRO,ESTADO = CASE WHEN P.CODIGO_PAGO IS NULL THEN 'PENDIENTE' WHEN F.MONTO_TOTAL = (SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA_ANTICIPO = F.CODIGO_UNICO) THEN 'APLICADO' ELSE 'CONFIRMADO' END  FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F LEFT JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P ON F.CODIGO_UNICO = P.CODIGO_FACTURA  WHERE F.CODIGO_UNICO <= 0 AND F.ESTADO != 'Z' AND F.NUMERO_IDENTIFICACION = '{0}' AND F.FECHA_REGISTRO BETWEEN '{1}-{2}-{3} 00:00:00' AND '{4}-{5}-{6} 23:59:59' AND F.NUMERO_LIQUIDACION = '{7}') A WHERE A.ESTADO = '{8}' order by A.ITEM"
                                        , idCliente, fechaDesde.Year, fechaDesde.Month.ToString("D2"), fechaDesde.Day.ToString("D2"),
                                        fechaHasta.Year, fechaHasta.Month.ToString("D2"), fechaHasta.Day.ToString("D2"), numeroLiquidacion,estado);
            }
            return "";
        }

        public DataTable ConsultaAnticipoPorNumeroLiquidacion(string numeroLiquidacion)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaConsultaPorNumeroLiquidacion(numeroLiquidacion), conexion);
                    conexion.Open();
                    var lector = comando.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(lector);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Query", numeroLiquidacion, "");
                return null;
            }
        }
        private string ArmarSentenciaConsultaPorNumeroLiquidacion(string numeroLiquidacion)
        {
            return string.Format("SELECT NUMERO_IDENTIFICACION,RAZON_SOCIAL,CAST(MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,NUMERO_LIQUIDACION,FORMAT(FECHA_REGISTRO,'dd/MM/yyyy HH:mm') AS FECHA_REGISTRO,NUMERO_BOOKING FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] WHERE NUMERO_LIQUIDACION = '{0}'"
                , numeroLiquidacion);
        }

        public DataTable ConsultaAnticipoConfirmadosParaCompensacion(string idCliente, DateTime fechaDesde, DateTime fechaHasta, string numeroLiquidacion)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaConsultaAnticipoConfirmadosParaCompensacion(idCliente, fechaDesde, fechaHasta, numeroLiquidacion), conexion);
                    conexion.Open();
                    var lector = comando.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(lector);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Query", numeroLiquidacion, "");
                return null;
            }
        }
        private string ArmarSentenciaConsultaAnticipoConfirmadosParaCompensacion(string idCliente, DateTime fechaDesde, DateTime fechaHasta, string numeroLiquidacion)
        {
            if (string.IsNullOrWhiteSpace(numeroLiquidacion))
            {
                return string.Format("SELECT * FROM (SELECT F.NUMERO_LIQUIDACION,F.FECHA_REGISTRO,CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,CAST(F.MONTO_TOTAL - ISNULL((SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA_ANTICIPO = F.CODIGO_UNICO),0) as numeric(18,2)) AS SALDO FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F  INNER JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P1 ON P1.CODIGO_FACTURA = F.CODIGO_UNICO  WHERE F.CODIGO_UNICO <= 0 AND  F.NUMERO_IDENTIFICACION = '{0}' AND F.FECHA_REGISTRO BETWEEN '{1}-{2}-{3} 00:00:00' AND '{4}-{5}-{6} 23:59:59') A WHERE A.SALDO > 0 order by A.FECHA_REGISTRO DESC "
                                        , idCliente, fechaDesde.Year, fechaDesde.Month.ToString("D2"), fechaDesde.Day.ToString("D2"),
                                        fechaHasta.Year, fechaHasta.Month.ToString("D2"), fechaHasta.Day.ToString("D2"));
            }
            return string.Format("SELECT * FROM (SELECT F.NUMERO_LIQUIDACION,F.FECHA_REGISTRO,CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,CAST(F.MONTO_TOTAL - ISNULL((SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA_ANTICIPO = F.CODIGO_UNICO),0) as numeric(18,2)) AS SALDO FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F  INNER JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P1 ON P1.CODIGO_FACTURA = F.CODIGO_UNICO  WHERE F.CODIGO_UNICO <= 0 AND  F.NUMERO_IDENTIFICACION = '{0}' AND F.FECHA_REGISTRO BETWEEN '{1}-{2}-{3} 00:00:00' AND '{4}-{5}-{6} 23:59:59' AND F.NUMERO_LIQUIDACION = '{7}') A WHERE A.SALDO > 0 order by A.FECHA_REGISTRO DESC "
                                    , idCliente, fechaDesde.Year, fechaDesde.Month.ToString("D2"), fechaDesde.Day.ToString("D2"),
                                    fechaHasta.Year, fechaHasta.Month.ToString("D2"), fechaHasta.Day.ToString("D2"), numeroLiquidacion);
        }

        public DataTable ConsultaFacturasNoCanceladasPorClientes(string idCliente)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaConsultaFacturasNoCanceladasPorClientes(idCliente), conexion);
                    conexion.Open();
                    var lector = comando.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(lector);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Query", idCliente, "");
                return null;
            }
        }
        private string ArmarSentenciaConsultaFacturasNoCanceladasPorClientes(string idCliente)
        {
            return string.Format("SELECT * FROM(SELECT F.FECHA_REGISTRO,F.NUMERO_LIQUIDACION,CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,CAST(F.MONTO_TOTAL - ISNULL((SELECT SUM(X.MONTO_RECAUDADO) FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] X WHERE X.CODIGO_FACTURA = F.CODIGO_UNICO),0) - ISNULL((SELECT SUM(D.VALOR) FROM [N4Middleware].[dbo].[INV_RETENCION_CLIENTE_DET] D INNER JOIN [N4Middleware].[dbo].[INV_RETENCION_CLIENTE] C ON D.CODIGO = C.CODIGO AND C.ESTADO_SAP = 'S' WHERE D.FACTURA = RIGHT(F.NUMERO_LIQUIDACION,15)) ,0) as numeric(18,2)) AS SALDO FROM [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F WHERE F.CODIGO_UNICO > 0 AND  F.NUMERO_IDENTIFICACION = '{0}' AND F.ESTADO = 'A' ) A WHERE A.SALDO > 0 ORDER BY A.NUMERO_LIQUIDACION"
                , idCliente);
        }

        public void IngresarCompensacion(string liquidacionAnticipo, string facturas)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaCompensacion(liquidacionAnticipo,facturas), conexion);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "PAGO", "Compensacion", liquidacionAnticipo, "");
                return;
            }
        }
        private string ArmarSentenciaCompensacion(string liquidacionAnticipo, string facturas)
        {
            return string.Format("EXEC [ecuapass].[dbo].[ECU_INGRESO_COMPENSACION] '{0}','{1}'", liquidacionAnticipo, facturas);
        }

        public DataTable ConsultarFacturasPagadasPorAnticipo(string codigoAnticipo)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaFacturasPagadasPorAnticipo(codigoAnticipo), conexion);
                    conexion.Open();
                    var lector = comando.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(lector);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "PAGO", "Compensacion", codigoAnticipo, "");
                return null;
            }
        }
        private string ArmarSentenciaFacturasPagadasPorAnticipo(string codigoAnticipo)
        {
            return string.Format("SELECT P.NUMERO_LIQUIDACION,CAST(L.MONTO_TOTAL as numeric(18,2)) AS MONTO_FACTURA,CAST(P.MONTO_RECAUDADO as numeric(18,2)) AS MONTO_PAGADO,L.NUMERO_IDENTIFICACION,L.RAZON_SOCIAL FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P INNER JOIN [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] L ON P.CODIGO_FACTURA = L.CODIGO_UNICO WHERE CODIGO_FACTURA_ANTICIPO ={0}", codigoAnticipo);
        }

        public DataTable ConsultarFacturaPorCodigoAnticipo(string codigoAnticipo)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaConsultarFacturaPorCodigoAnticipo(codigoAnticipo), conexion);
                    conexion.Open();
                    var lector = comando.ExecuteReader();
                    var dataTable = new DataTable();
                    dataTable.Load(lector);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Query", codigoAnticipo, "");
                return null;
            }
        }
        private string ArmarSentenciaConsultarFacturaPorCodigoAnticipo(string codigoAnticipo)
        {
            return string.Format("SELECT F.NUMERO_IDENTIFICACION,F.RAZON_SOCIAL,CAST(F.MONTO_TOTAL as numeric(18,2)) AS MONTO_TOTAL,F.NUMERO_LIQUIDACION FROM [ecuapass].[dbo].[ECU_LIQUIDACION_PAGO_SENAE] P INNER JOIN  [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] F ON P.NUMERO_LIQUIDACION = F.NUMERO_LIQUIDACION WHERE CODIGO_FACTURA = {0}", codigoAnticipo);
        }

        public void AnularAnticipo(string codigoAnticipo)
        {
            try
            {
                using (var conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                {
                    var comando = new SqlCommand(ArmarSentenciaAnularAnticipo(codigoAnticipo), conexion);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "PAGO", "ANTICIPO", codigoAnticipo, "");
                return;
            }
        }
        private string ArmarSentenciaAnularAnticipo(string codigoAnticipo)
        {
            return string.Format("UPDATE [ecuapass].[dbo].[ECU_LIQUIDACION_SENAE] SET TRAMITE = 'M',ESTADO = 'Z',ESTADO_PROCESO = 'A' WHERE CODIGO_UNICO = {0}", codigoAnticipo);
        }

        public static DataTable ConsultaLiquidacionSaldo(string idCliente, int anio, int mes)
        {
            var d = new DataTable();
            try
            {
                using (var c = new SqlConnection(ConfigurationManager.ConnectionStrings["ecuapass"].ConnectionString))
                {
                    var coman = c.CreateCommand();
                    coman.CommandType = CommandType.StoredProcedure;
                    coman.CommandText = "[ECU_SP_LIQUIDACION_SALDO_RPT]";
                    coman.Parameters.AddWithValue("@CLIENTE", idCliente);
                    coman.Parameters.AddWithValue("@anio", anio);
                    coman.Parameters.AddWithValue("@mes", mes);

                    try
                    {
                        c.Open();
                        d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                    }
                    catch (SqlException ex)
                    {
                        csl_log.log_csl.save_log<SqlException>(ex, "PagoEnLineaData", "ConsultaLiquidacionSaldo", idCliente, "gs");
                    }
                    finally
                    {
                        if (c.State == ConnectionState.Open)
                        {
                            c.Close();
                        }
                        c.Dispose();
                    }
                }
                return d;
            
            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "Anticipo", "Query", idCliente, "");
                return null;
            }
        }


        public static DataSet ComboFechas()
        {
            var d = new DataSet();
            try
            {
                using (var c = new SqlConnection(ConfigurationManager.ConnectionStrings["ecuapass"].ConnectionString))
                {
                    var coman = c.CreateCommand();
                    coman.CommandType = CommandType.StoredProcedure;
                    coman.CommandText = "ECU_SP_LIQUIDACION_RPT_PARAM";

                    try
                    {
                        //c.Open();
                        //d.(coman.ExecuteReader(CommandBehavior.CloseConnection));
                        SqlDataAdapter adapter = new SqlDataAdapter(coman);
                        adapter.Fill(d);
                    }
                    catch (SqlException ex)
                    {
                        csl_log.log_csl.save_log<SqlException>(ex, "PagoEnLineaData", "ComboFechas", "ECU_SP_LIQUIDACION_RPT_PARAM", "gs");
                    }
                    finally
                    {
                        if (c.State == ConnectionState.Open)
                        {
                            c.Close();
                        }
                        c.Dispose();
                    }
                }
                return d;

            }
            catch (Exception ex)
            {
                log_csl.save_log(ex, "DataSet ComboFechas", "ComboFechas", "ECU_SP_LIQUIDACION_RPT_PARAM", "");
                return null;
            }
        }

    }
}