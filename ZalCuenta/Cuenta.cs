using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZalCuenta
{
    class Cuenta:BaseInit
    {
   
        /// <summary>
        /// demo
        /// </summary>
        public string ruc { get; set; }
        public string razonsocial { get; set; }
        public decimal saldo { get; set; }
        private bool isloaded = false;
        public List<Cuenta_Usuario> usuarios { get; set; }
        public List<Cuenta_Registro> movimientos { get; set; }
        public Cuenta()
        {
            init();
            this.usuarios = new List<Cuenta_Usuario>();
            this.movimientos = new List<Cuenta_Registro>();
            isloaded = false;
        }
        public Cuenta(string ruc)
        {
            init();
            LoadMyData();         
        }
        private static void initialize()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
           
        }
        /// <summary>
        /// Retorna el saldo de la cuenta o ruc
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        public static decimal? SaldoActual(string ruc)
        {
            initialize();
            if (string.IsNullOrEmpty(ruc))
            {
                return 0;
            }
            ruc = ruc.Trim();
            //obtener el saldo
            //ecuapass
            //select [dbo].[zec_saldo](@ruc)
            parametros.Clear();
            parametros.Add("ruc", ruc);
            var v_conexion = app_configurations.get_configuration("ecuapass");
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 8000, "select [dbo].[zec_saldo](@ruc)", parametros, out error_mensaje);
            return db as decimal?;

        }
        /// <summary>
        /// Carga los datos basicos, razon social, saldo actual en dolares, y mis usuarios
        /// </summary>
        public void LoadMyData()
        {
            if (!string.IsNullOrEmpty(this.ruc))
            {
                //mis usuarios
                this.usuarios = Cuenta_Usuario.UsuarioxRuc(ruc);
                var s = SaldoActual(ruc);
                this.razonsocial = obtener_razon(ruc);
                //mi saldo actual
                this.saldo = s.HasValue ? s.Value : 0;
                isloaded = true;
            }
        }
        /// <summary>
        /// Carga todos los datos de la entidad incluidos los movimientos en el corte de fechas
        /// </summary>
        /// <param name="desde">Inicio</param>
        /// <param name="hasta">Fin</param>
        public void LoadMyData(DateTime? desde= null, DateTime? hasta=null)
        {
            if (!string.IsNullOrEmpty(this.ruc))
            {
                //mis usuarios
                this.usuarios = Cuenta_Usuario.UsuarioxRuc(ruc);
                this.razonsocial = obtener_razon(ruc);
                this.movimientos = Cuenta_Registro.ObtenerMovimientos(desde, hasta, this.ruc);
                var s = SaldoActual(ruc);
                //mi saldo actual
                this.saldo = s.HasValue ? s.Value : 0;
                isloaded = true;
            }
        }
        /// <summary>
        /// Valida que un login este activo y forme parte de esta cuenta empresa o ruc
        /// </summary>
        /// <param name="loginname">el login a validar</param>
        /// <returns></returns>
        public bool IsValidUser(string loginname)
        {
            if (string.IsNullOrEmpty(loginname))
            {
                return false;
            }
            //NO TENGO MIS USUARIOS CARGUELOS
            if (!isloaded)
            {
                LoadMyData();
            }
            ruc = ruc.Trim();
            //ESTA ACTIVO Y ADEMAS ESTA EN MI LISTADO
            return this.usuarios.Where(u => u.loginname.ToLower().Equals(ruc) && u.estado.Equals("A")).Count() > 0 ? true : false;
        }


        private string obtener_razon(string ruc)
        {
            if (string.IsNullOrEmpty(ruc))
            {
                return null;
            }
            ruc = ruc.Trim();

            parametros.Clear();
            parametros.Add("ruc", ruc);
            var v_conexion = app_configurations.get_configuration("csl_services");
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 8000, "select [dbo].[zec_obtener_cuenta_razon]](@ruc)", parametros, out error_mensaje);
            return db as string;
        }


        

    }
}