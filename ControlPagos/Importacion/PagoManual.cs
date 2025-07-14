using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuraciones;
using N4;
using Aduanas.Entidades;
using AccesoDatos;
using System.Reflection;

namespace ControlPagos.Importacion
{


    public class PagoAsignado:ModuloBase
    {
        public Int64? id_asignacion { get; set; }

        public string consecutivo { get; set; }
        public string ruc_asumido { get; set; }
        public string nombre_asumido { get; set; }
        public string container { get; set; }

        public Int64? id_depot { get; set; }
        public string booking { get; set; }

        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }

        public int? total { get;  set; }
        public DateTime? fecha_manifiesto { get;  set; }

        public DateTime? fecha_asignado { get; set; }
        public string login_asigna { get;  set; }
        public bool activo { get; set; }

        public DateTime? fecha_modifica { get; set; }
        public string login_modifica { get; set; }

        public PagoAsignado(string _mrn, string _msn, string _hsn, string _ruc, string _login)
        {
            this.mrn = _mrn?.Trim().ToUpper();
            this.msn = _msn?.Trim().ToUpper();
            this.hsn = _hsn?.Trim().ToUpper(); 
            this.ruc = _ruc?.Trim().ToUpper(); 
            this.id_asignacion = -1;
            this.fecha_asignado = DateTime.Now;
            this.login_asigna = _login?.Trim();
            OnInstanceCreate();
        }

        public PagoAsignado(long _consecutivo, string _contenedor, string _ruc_asumir, string _nombre_asumir, string _ruc, string _login)
        {
            this.consecutivo = _consecutivo.ToString();
            this.container = _contenedor?.Trim().ToUpper();
            this.ruc_asumido = _ruc_asumir;
            this.nombre_asumido = _nombre_asumir;
            this.ruc = _ruc?.Trim().ToUpper();
            this.id_asignacion = -1;
            this.fecha_asignado = DateTime.Now;
            this.login_asigna = _login?.Trim();
            OnInstanceCreate();
        }


        public PagoAsignado( long idDepot, string _booking, string _ruc_asumir, string _nombre_asumir, string _ruc, string _nombre, string _login)
        {
            this.id_depot = idDepot;
            this.booking = _booking?.Trim().ToUpper();
            this.ruc_asumido = _ruc_asumir;
            this.nombre_asumido = _nombre_asumir;
            this.ruc = _ruc?.Trim().ToUpper();
            this.id_asignacion = -1;
            this.fecha_asignado = DateTime.Now;
            this.login_asigna = _login?.Trim();
            OnInstanceCreate();
        }

        public PagoAsignado():base()
        {
           
        }
        public PagoAsignado(Int64 idpago, string modifica) : base()
        {
            this.id_asignacion = idpago;
            this.login_modifica = modifica;
        }

        public Respuesta.ResultadoOperacion<Int64> NuevaAsignacion()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }


            var tt = SetMessage("NO_NULO", actualMetodo, login_asigna);
            if (string.IsNullOrEmpty(this.mrn))
            {
               return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1,nameof(mrn)));
            }
            if (string.IsNullOrEmpty(this.msn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(this.hsn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
             if (string.IsNullOrEmpty(this.ruc))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ruc)));
            }
            if (string.IsNullOrEmpty(this.login_asigna))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            this.fecha_asignado = fecha_asignado.HasValue ? fecha_asignado.Value : DateTime.Now;
            this.id_asignacion = 0;
            //LLame a N4 traiga el cliente.


            var resp_n4 = N4.Entidades.Cliente.ObtenerCliente(this.login_asigna, ruc);
           
            
            //NO_CLIENTE
            tt = SetMessage("NO_CLIENTE", actualMetodo, login_asigna);
            if (!resp_n4.Exitoso)
            {
                //No es un cliente N4
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1,ruc),tt.Item2);
            }
            //NO_CREDITO

/*
          //HABILITADO SOLO PAR CLIENTES DE CREDITO

            tt = SetMessage("NO_CREDITO", actualMetodo, login_asigna);
            if (!resp_n4.Resultado.DIAS_CREDITO.HasValue || resp_n4.Resultado.DIAS_CREDITO <= 0)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, ruc), tt.Item2);
            }

*/
            //LLame a ecuapas traiga el Bl

            tt = SetMessage("NO_MRN", actualMetodo, login_asigna);
            var ecu_resp = Manifiesto.ObtenerManifiesto(this.login_asigna, this.mrn, this.msn, this.hsn);
            if (!ecu_resp.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, mrn+msn+hsn), tt.Item2);
            }

            this.total = ecu_resp.Resultado.equipos;
            this.fecha_manifiesto = ecu_resp.Resultado.fecha;
            this.nombre = string.IsNullOrEmpty(this.nombre) ? resp_n4.Resultado.CLNT_NAME : this.nombre;
            //ok preparar la entidad


            this.Parametros.Clear();
            this.Parametros.Add("mrn",mrn);
            this.Parametros.Add("msn", msn);
            this.Parametros.Add("hsn", hsn);
            this.Parametros.Add("ruc", ruc);

            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO

            tt = SetMessage("YA_ASIGNADO", actualMetodo, login_asigna);
            var exresult = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[existe_asignacion](@mrn,@msn,@hsn,@ruc);", Parametros);
            if (exresult.Resultado)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, mrn+msn+hsn), tt.Item2);
            }

            
            this.Parametros.Add("nombre", nombre);
            this.Parametros.Add("fecha_asignado", fecha_asignado);
            this.Parametros.Add("login_asigna", login_asigna);
            this.Parametros.Add("fecha_manifiesto", fecha_manifiesto);
            this.Parametros.Add("total", total);
#if DEBUG
            this.LogEvent(login_asigna,this.actualMetodo,"Traza");
#endif
            var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[upsert_pago_asignado]",this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }

            this.id_asignacion = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue?result.Resultado.Value:-1);
        }

        public Respuesta.ResultadoOperacion<Int64> NuevaAsignacionSAV()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }


            var tt = SetMessage("NO_NULO", actualMetodo, login_asigna);
            if (string.IsNullOrEmpty(this.container))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(container)));
            }

            if (string.IsNullOrEmpty(this.ruc_asumido))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ruc_asumido)));
            }

            if (string.IsNullOrEmpty(this.nombre_asumido))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(nombre_asumido)));
            }

            if (string.IsNullOrEmpty(this.ruc))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ruc)));
            }
            if (string.IsNullOrEmpty(this.login_asigna))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            this.fecha_asignado = fecha_asignado.HasValue ? fecha_asignado.Value : DateTime.Now;
            this.id_asignacion = 0;
            //LLame a N4 traiga el cliente.

            var resp_n4 = N4.Entidades.Cliente.ObtenerClienteSAV(this.login_asigna, ruc);

            //NO_CLIENTE
            tt = SetMessage("NO_CLIENTE", actualMetodo, login_asigna);
            if (!resp_n4.Exitoso)
            {
                //No es un cliente N4
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, ruc), tt.Item2);
            }
            //NO_CREDITO

            
            //HABILITADO SOLO PAR CLIENTES DE CREDITO

            tt = SetMessage("NO_CREDITO", actualMetodo, login_asigna);
            if (!resp_n4.Resultado.DIAS_CREDITO.HasValue || resp_n4.Resultado.DIAS_CREDITO <= 0)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, ruc), tt.Item2);
            }

            if (resp_n4.Resultado.DIAS_CREDITO <= 0)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format("No se puede asumir el pago, Cliente no tiene crédito", nameof(login_asigna)));
            }

            //LLame a ecuapas traiga el Bl
/*
            tt = SetMessage("NO_MRN", actualMetodo, login_asigna);
            var ecu_resp = Manifiesto.ObtenerManifiesto(this.login_asigna, this.mrn, this.msn, this.hsn);
            if (!ecu_resp.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, mrn + msn + hsn), tt.Item2);
            }
            
            this.total = ecu_resp.Resultado.equipos;
            this.fecha_manifiesto = ecu_resp.Resultado.fecha;
            */
            this.nombre = string.IsNullOrEmpty(this.nombre) ? resp_n4.Resultado.CLNT_NAME : this.nombre;
            //ok preparar la entidad


            this.Parametros.Clear();
            this.Parametros.Add("ruc", ruc);
            this.Parametros.Add("container", container);
            this.Parametros.Add("ruc_asumido", ruc_asumido);

            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO

            tt = SetMessage("YA_ASIGNADO", actualMetodo, login_asigna);
            var exresult = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[existe_asignacion_sav](@ruc,@container,@ruc_asumido);", Parametros);
            if (exresult.Resultado)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, mrn + msn + hsn), tt.Item2);
            }

            this.Parametros.Add("nombre_asumido", nombre_asumido);
            this.Parametros.Add("nombre", nombre);
            this.Parametros.Add("fecha_asignado", fecha_asignado);
            this.Parametros.Add("login_asigna", login_asigna);
            //this.Parametros.Add("fecha_manifiesto", fecha_manifiesto);
            //this.Parametros.Add("total", total);
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[upsert_pago_asignado_sav]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }

            this.id_asignacion = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        public Respuesta.ResultadoOperacion<Int64> NuevaAsignacionZAL()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }


            var tt = SetMessage("NO_NULO", actualMetodo, login_asigna);
            if (string.IsNullOrEmpty(this.booking))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(booking)));
            }

            if (string.IsNullOrEmpty(this.ruc_asumido))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ruc_asumido)));
            }

            if (string.IsNullOrEmpty(this.nombre_asumido))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(nombre_asumido)));
            }

            if (string.IsNullOrEmpty(this.ruc))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ruc)));
            }
            if (string.IsNullOrEmpty(this.login_asigna))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            this.fecha_asignado = fecha_asignado.HasValue ? fecha_asignado.Value : DateTime.Now;
            this.id_asignacion = 0;
            //LLame a N4 traiga el cliente.

            var resp_n4 = N4.Entidades.Cliente.ObtenerClienteZAL(this.login_asigna, ruc);

            //NO_CLIENTE
            tt = SetMessage("NO_CLIENTE", actualMetodo, login_asigna);
            if (!resp_n4.Exitoso)
            {
                //No es un cliente N4
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, ruc), tt.Item2);
            }
            //NO_CREDITO


            //HABILITADO SOLO PAR CLIENTES DE CREDITO

            tt = SetMessage("NO_CREDITO", actualMetodo, login_asigna);
            if (!resp_n4.Resultado.DIAS_CREDITO.HasValue || resp_n4.Resultado.DIAS_CREDITO <= 0)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, ruc), tt.Item2);
            }

            if (resp_n4.Resultado.DIAS_CREDITO <= 0)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format("No se puede asumir el pago, Cliente no tiene crédito", nameof(login_asigna)));
            }

            this.nombre = string.IsNullOrEmpty(this.nombre) ? resp_n4.Resultado.CLNT_NAME : this.nombre;

            this.Parametros.Clear();
            this.Parametros.Add("ruc", ruc);
            this.Parametros.Add("ruc_asumido", ruc_asumido);
            this.Parametros.Add("booking", booking);
            this.Parametros.Add("id_depot", id_depot);

            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO

            tt = SetMessage("YA_ASIGNADO", actualMetodo, login_asigna);
            var exresult = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[existe_asignacion_zal](@ruc,@booking,@ruc_asumido);", Parametros);
            if (exresult.Resultado)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1,"booking: " + booking), tt.Item2);
            }

            this.Parametros.Add("nombre_asumido", nombre_asumido);
            this.Parametros.Add("nombre", nombre);
            this.Parametros.Add("fecha_asignado", fecha_asignado);
            this.Parametros.Add("login_asigna", login_asigna);
          
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[upsert_pago_asignado_zal]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }

            this.id_asignacion = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        public  Respuesta.ResultadoOperacion<PagoAsignado> ExisteAsumeTercero()
        {
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<PagoAsignado>.CrearFalla(pv);
            }


            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            this.Parametros.Clear();
            this.Parametros.Add("container", container);
            this.Parametros.Add("ruc_asumido", ruc_asumido); 

            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO

            var tt = SetMessage("YA_ASIGNADO", actualMetodo, login_asigna);
            var exresult = BDOpe.ComandoSelectAEntidad<PagoAsignado>(bcon, "[Bill].[obtener_pago_asignacion_sav]", Parametros);
            if (!exresult.Exitoso)
            {
                return Respuesta.ResultadoOperacion<PagoAsignado>.CrearFalla(string.Format(tt.Item1, container), tt.Item2);
            }
            return Respuesta.ResultadoOperacion<PagoAsignado>.CrearResultadoExitoso(exresult.Resultado);
        }

        public Respuesta.ResultadoOperacion<PagoAsignado> ExisteAsumeTerceroZAL()
        {
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<PagoAsignado>.CrearFalla(pv);
            }

            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            this.Parametros.Clear();
            this.Parametros.Add("booking", booking);
            this.Parametros.Add("ruc_asumido", ruc_asumido);

            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO

            var tt = SetMessage("YA_ASIGNADO", actualMetodo, login_asigna);
            var exresult = BDOpe.ComandoSelectAEntidad<PagoAsignado>(bcon, "[Bill].[obtener_pago_asignacion_zal]", Parametros);
            if (!exresult.Exitoso)
            {
                return Respuesta.ResultadoOperacion<PagoAsignado>.CrearFalla(string.Format(tt.Item1, container), tt.Item2);
            }
            return Respuesta.ResultadoOperacion<PagoAsignado>.CrearResultadoExitoso(exresult.Resultado);
        }

        public override void OnInstanceCreate()
        {
            this.alterClase = "PAGOASIGNADO";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public static bool? ExistePago(string _mrn, string _msn, string _hsn)
        {
            
            if (string.IsNullOrEmpty(_mrn))
            {
                return true;
            }
            if (string.IsNullOrEmpty(_msn))
            {
                return true;
            }
            if (string.IsNullOrEmpty(_hsn))
            {
                return true;
            }
            var p = new PagoAsignado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            p.Parametros.Clear();
            p.Parametros.Add("mrn", _mrn);
            p.Parametros.Add("msn", _msn);
            p.Parametros.Add("hsn", _hsn);

    
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[existe_asignacion](@mrn,@msn,@hsn);", p.Parametros);

            //quiere decir que TODO SALIO BIEN
            if (!result.Exitoso)
            {
                return null;
            }
            p = null;
            return result.Resultado;

        }

        public static bool? ExistePagoSAV(string _ruc, string _container)
        {

            if (string.IsNullOrEmpty(_ruc))
            {
                return true;
            }
            if (string.IsNullOrEmpty(_container))
            {
                return true;
            }
            
            var p = new PagoAsignado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            p.Parametros.Clear();
            p.Parametros.Add("ruc", _ruc);
            p.Parametros.Add("container", _container);

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[existe_asignacion_sav](@ruc,@container);", p.Parametros);

            //quiere decir que TODO SALIO BIEN
            if (!result.Exitoso)
            {
                return null;
            }
            p = null;
            return result.Resultado;

        }


        public Respuesta.ResultadoOperacion<int> BorrarAsignacion()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            var tt = SetMessage("NO_NULO", actualMetodo, login_modifica);
            if (string.IsNullOrEmpty(this.login_modifica))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            if (this.id_asignacion <=0  )
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(id_asignacion)));
            }
            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("modifica", login_modifica);
            this.Parametros.Add("id", id_asignacion);
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[borrar_pago_asignado]", this.Parametros);
            return Respuesta.ResultadoOperacion<int>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }

        public Respuesta.ResultadoOperacion<int> BorrarAsignacionSAV()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            var tt = SetMessage("NO_NULO", actualMetodo, login_modifica);
            if (string.IsNullOrEmpty(this.login_modifica))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            if (this.id_asignacion <= 0)
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(id_asignacion)));
            }
            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("modifica", login_modifica);
            this.Parametros.Add("id", id_asignacion);
            this.Parametros.Add("consecutivo", consecutivo);
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[borrar_pago_asignado_sav]", this.Parametros);
            return Respuesta.ResultadoOperacion<int>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }

        public Respuesta.ResultadoOperacion<int> BorrarAsignacionZAL()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            var tt = SetMessage("NO_NULO", actualMetodo, login_modifica);
            if (string.IsNullOrEmpty(this.login_modifica))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            if (this.id_asignacion <= 0)
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(id_asignacion)));
            }
            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("modifica", login_modifica);
            this.Parametros.Add("id", id_asignacion);
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[borrar_pago_asignado_zal]", this.Parametros);
            return Respuesta.ResultadoOperacion<int>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }

        public Respuesta.ResultadoOperacion<int> ActivarAsignacionSAV()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            var tt = SetMessage("NO_NULO", actualMetodo, login_modifica);
            if (string.IsNullOrEmpty(this.login_modifica))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            if (this.id_asignacion <= 0)
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(id_asignacion)));
            }
            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("modifica", login_modifica);
            this.Parametros.Add("id", id_asignacion);
           
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[activar_pago_asignado_sav]", this.Parametros);
            return Respuesta.ResultadoOperacion<int>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }

        public Respuesta.ResultadoOperacion<int> ActivarAsignacionZAL()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            var tt = SetMessage("NO_NULO", actualMetodo, login_modifica);
            if (string.IsNullOrEmpty(this.login_modifica))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(login_asigna)));
            }
            if (this.id_asignacion <= 0)
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(id_asignacion)));
            }
            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("modifica", login_modifica);
            this.Parametros.Add("id", id_asignacion);

            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[activar_pago_asignado_zal]", this.Parametros);
            return Respuesta.ResultadoOperacion<int>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }

        public static Respuesta.ResultadoOperacion<List<PagoAsignado>> ListaAsignacion(string usuario, string ruc, DateTime? desde=null, DateTime? hasta=null )
        {
            var p = new PagoAsignado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
           var tt = p.SetMessage("NO_NULO", p.actualMetodo, ruc);
            p.Parametros.Clear();
            p.Parametros.Add("ruc",ruc);
            if (desde.HasValue)
            {
                p.Parametros.Add("desde", desde.Value);
            }
            if (hasta.HasValue)
            {
                p.Parametros.Add("hasta", hasta.Value);
            }
            if (string.IsNullOrEmpty(ruc))
            {
                  return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(string.Format(tt.Item1, nameof(ruc)),tt.Item2);
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<PagoAsignado>(bcon, "[Bill].[listar_pago_asignado]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

        public static Respuesta.ResultadoOperacion<List<PagoAsignado>> ListaAsignacionSAV(string usuario, string ruc, DateTime? desde = null, DateTime? hasta = null)
        {
            var p = new PagoAsignado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ruc);
            p.Parametros.Clear();
            p.Parametros.Add("ruc", ruc);
            if (desde.HasValue)
            {
                p.Parametros.Add("desde", desde.Value);
            }
            if (hasta.HasValue)
            {
                p.Parametros.Add("hasta", hasta.Value);
            }
            if (string.IsNullOrEmpty(ruc))
            {
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(string.Format(tt.Item1, nameof(ruc)), tt.Item2);
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<PagoAsignado>(bcon, "[Bill].[listar_pago_asignado_sav]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

        

        public static Respuesta.ResultadoOperacion<List<PagoAsignado>> ListaAsignacionPartida(string usuario, string mrn,string msn, string hsn)
        {
            var p = new PagoAsignado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn); p.Parametros.Add("msn", msn); p.Parametros.Add("hsn", hsn); 

            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(string.Format(tt.Item1, nameof(msn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<PagoAsignado>(bcon, "[Bill].[listar_partida_asignados]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

        public Respuesta.ResultadoOperacion<long> obtener_preaviso_id_tos(string _contenedor)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<long>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("cntr", _contenedor);
           
            var bcon = this.Accesorio.ObtenerConfiguracion("N5")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoSelectEscalar<long>(bcon, " exec sp_obtener_preaviso_id_tos @cntr", this.Parametros);
            return Respuesta.ResultadoOperacion<long>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }

        public Respuesta.ResultadoOperacion<long> obtener_IdAsignado(string _turno_id, string unidad_id)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<long>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("turno_id", _turno_id);
            this.Parametros.Add("unidad_id", unidad_id);

            var bcon = this.Accesorio.ObtenerConfiguracion("CSL_SERVICE")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(login_asigna, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoSelectEscalar<long>(bcon, " exec consultar_idAsignado_preaviso @turno_id, @unidad_id", this.Parametros);
            return Respuesta.ResultadoOperacion<long>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }


        public static Respuesta.ResultadoOperacion<List<PagoAsignado>> ListaAsignacionZAL(string usuario, string ruc, DateTime? desde = null, DateTime? hasta = null)
        {
            var p = new PagoAsignado();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ruc);
            p.Parametros.Clear();
            p.Parametros.Add("ruc", ruc);
            if (desde.HasValue)
            {
                p.Parametros.Add("desde", desde.Value);
            }
            if (hasta.HasValue)
            {
                p.Parametros.Add("hasta", hasta.Value);
            }
            if (string.IsNullOrEmpty(ruc))
            {
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(string.Format(tt.Item1, nameof(ruc)), tt.Item2);
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<PagoAsignado>(bcon, "[Bill].[listar_pago_asignado_zal]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<PagoAsignado>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }
    }

   
}
