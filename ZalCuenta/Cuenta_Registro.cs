using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZalCuenta
{
    public enum operacion
    {
        ingreso, egreso
    }

   public class Cuenta_Registro :BaseInit
    {
        private static void initialize()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

        }

        public Cuenta_Registro()
        {
            this.fecharegistro = DateTime.Now;
            this.cantidad = 0;
        }
        public operacion tipo { get; set; }
        public string ref_n4 { get; set; }
        public string ref_ecu { get; set; }
        public string ruc { get; set; }
        public string usuario { get; set; }
        public int cantidad { get; set; }
        public DateTime fecharegistro { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public Int64 id { get; set; }
        public decimal valorunitario { get; set; }

        public static bool? liquidacionPagada(string liquidacion)
        {
            initialize();
            if (string.IsNullOrEmpty(liquidacion))
            {
                return false;
            }
            liquidacion = liquidacion.Trim();
            parametros.Clear();
            parametros.Add("numero", liquidacion);
            var v_conexion = app_configurations.get_configuration("ecuapass");
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 8000, "select [dbo].[zec_liquidacion_pagada](@numero)", parametros, out error_mensaje);
            return db as bool?;
            //null: hubo error, true=>pagada/false=impaga
        }

        public static decimal? preciounitario_liquidacion(string liquidacion)
        {
            initialize();
            if (string.IsNullOrEmpty(liquidacion))
            {
                return 0;
            }
            liquidacion = liquidacion.Trim();
            parametros.Clear();
            parametros.Add("numero", liquidacion);
            var v_conexion = app_configurations.get_configuration("ecuapass");
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 8000, "select [dbo].[zec_preciounitario_liquidacion](@numero)", parametros, out error_mensaje);
            return db as decimal?;
            //null: hubo error, true=>pagada/false=impaga
        }

        /// <summary>
        /// Inserta los movimientos a la tabla de estado de cuenta
        /// </summary>
        /// <param name="fila">Objeto registro de tabla</param>
        /// <param name="novedad">Si retorna negativo o nulo, mensaje de error o novedad</param>
        /// <returns>Null= Hubo un error, Negativo no paso una validacion, positivo numero de registro</returns>
        public static Int64? InsertarMovimiento(Cuenta_Registro fila, out string novedad )
        {
            //proceso de base de datos para grabar el movimiento, tabla registros
            initialize();
            if (!validaciones(fila, out novedad))
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("ref_ecu", fila.ref_ecu);
            parametros.Add("ref_n4", fila.ref_n4);
            parametros.Add("ruc", fila.ruc);
            parametros.Add("usuario", fila.usuario);
            parametros.Add("cantidad", fila.cantidad);
            parametros.Add("fecharegistro", fila.fecharegistro);
            if (fila.tipo == operacion.ingreso)
            {
                parametros.Add("debe", fila.monto);
                parametros.Add("haber", 0);
            }
            else
            {
                parametros.Add("haber", fila.monto);
                parametros.Add("debe", 0);
            }
            parametros.Add("concepto", fila.concepto);
            parametros.Add("valorunitario", fila.valorunitario);

            var v_conexion = app_configurations.get_configuration("ecuapass");
            if (string.IsNullOrEmpty(v_conexion?.value))
            {
                novedad= "Cadena de conexion a datos no establecida (Cta_Registro)";
                return -1;
            }
            var rs = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion.value, 4000, "zec_agrega_registro", parametros, out novedad);
            if (!rs.HasValue)
            {
                return null;
            }
            //agregar la fila a la tabla.
            novedad = string.Empty;
            return rs.Value;
        }
        private static bool validaciones(Cuenta_Registro fila, out string novedad)
        {
            if (fila == null)
            {
                novedad = "El registro completo no debe ser un valor nulo";
                return false;
            }
            if (!checkstring(fila.ruc,"RUC",out novedad))
            {
                return false;
            }
            if (!checkstring(fila.usuario, "Usuario/Login", out novedad))
            {
                return false;
            }
            if (!checkstring(fila.ref_ecu, "Número Liquidación", out novedad))
            {
                return false;
            }

            //ingreso->Debe, Haber->gasto
            if (fila.monto<=0)
            {
                novedad = "Para operaciones de Ingreso/Egreso debe agregar el monto";
                return false;
            }
            novedad = string.Empty;
            return true;
        }
        private static bool checkstring(string cadena, string fieldName, out string mensaje)
        {
            if (string.IsNullOrEmpty(cadena))
            {
                mensaje = string.Format("El campo {0} no puede contener un valor vacío o nulo",fieldName);
                return false;
            }
            mensaje = string.Empty;
            return true;
        }

        /// <summary>
        /// Obtiene la lista de movimientos de un ruc, en un rango de fechas, si fechas es null, retorna los ultimos 5 dias
        /// </summary>
        /// <param name="desde">Inicio del corte</param>
        /// <param name="hasta">Fin del corte</param>
        /// <param name="ruc">Ruc de la cuenta</param>
        /// <returns>Lista de registros</returns>
        internal static List<Cuenta_Registro> ObtenerMovimientos(DateTime? desde, DateTime? hasta, string ruc )
        {
            initialize();
            var v_conexion = app_configurations.get_configuration("ecuapass");
            parametros.Clear();
            parametros.Add("ruc",ruc);
            if (desde.HasValue) { parametros.Add("desde",desde.Value); }
            if (hasta.HasValue) { parametros.Add("hasta", hasta.Value); }

            var salida = new List<Cuenta_Registro>();
            var registros= sql_pointer.ExecuteSelect(v_conexion.value, 2000, "zec_obtener_movimientos", parametros, null);
            if (registros == null || registros.Count <= 0)
            {
                return null;
            }
            Cuenta_Registro ri = null;
            foreach (var r in registros)
            {
                ri= new Cuenta_Registro();
                /* 
                 * 
                 * ref_ecu->0,
                 * ref_n4->1,
                 * ruc->2,
                 * usuario->3,
                 * cantidad->4,
                 * fecharegistro->5,
                 * debe->6,
                 * haber->7,
                 * concepto->8,
                 * id->9
                  */
                ri.ref_ecu = r.GetString(0);
                ri.ref_n4 = r.GetString(1);
                ri.ruc = r.GetString(2);
                ri.usuario = r.GetString(3);
                ri.cantidad = r.GetInt32(4);
                ri.fecharegistro = r.GetDateTime(5);
                //columna debe
                var debe = r.GetDecimal(6);
                ri.tipo = debe > 0 ? operacion.ingreso : operacion.egreso;
                ri.monto = debe > 0 ? r.GetDecimal(6) : r.GetDecimal(7);
                ri.concepto = r.GetString(8);
                ri.id = r.GetInt32(9);
                salida.Add(ri);
            }
            return salida;

        }

    }
}
