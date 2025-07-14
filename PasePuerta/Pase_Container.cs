using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Configuraciones;
using Respuesta;
using N4Ws;
using System.Data;

namespace PasePuerta
{
    public class Pase_Container : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public Pase_Container()
        {
            OnInstanceCreate();
            this.ESTADO = "GN";
            this.CANTIDAD_CARGA = 0;
            this.FECHA_REGISTRO = DateTime.Now;
            this.TIPO_CARGA = "CNTR";
        }
        public decimal ID_PASE { get; set; } //secuencil
        public decimal? ID_CARGA { get; set; } //gkey
        public string ESTADO { get; set; } //nuevo->GN
        public DateTime? FECHA_EXPIRACION { get; set; } // *
        public string ID_PLACA { get; set; }
        public string ID_CHOFER { get; set; }
        public string ID_EMPRESA { get; set; }
        public int? CANTIDAD_CARGA { get; set; }
        public string USUARIO_REGISTRO { get; set; }
        public DateTime? FECHA_REGISTRO { get; set; }
        public string USUARIO_ESTADO { get; set; }
        public DateTime? FECHA_ESTADO { get; set; }
        public Int64? ID_RESERVA { get; set; }
        public string TIPO_CARGA { get; set; }
        public string NUMERO_PASE_N4 { get; set; }
        public string MOTIVO_CANCELACION { get; set; }
        public bool? RESERVA { get; set; }
        public bool? ESTADO_RESERVA { get; set; }
        public Int64? PPW { get; set; }
        public string NUMERO_PASE_N4_ANTERIOR { get; set; }

        //nuevos campos
        public string CONSIGNATARIO_ID { get; set; }
        public string CONSIGNARIO_NOMBRE { get; set; }

        //NOMBRE DE EMPRESA
              public string TRANSPORTISTA_DESC { get; set; }
        //NOMBRE DE CHOFER
        public string CHOFER_DESC { get; set; }

                public DateTime? TINICIA { get; set; }
                public DateTime? TFIN { get; set; }
                public Int64? TID { get; set; }

        public bool? SERVICIO { get; set; }
          public DateTime? FECHA_SERVICIO { get; set; }
         public string USUARIO_SERVICIO { get; set; }
       //debe insertar, debe modificar, debe eliminar
        //Insertar pase a puerta
        public ResultadoOperacion<Int64> Insertar(Int64 ID_PLAN, Int64 ID_SECUENCIA, string CONTENEDOR, string HORA_TURNO, DateTime FECHA_CAS, bool IsDD=false)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_REGISTRO);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(this.USUARIO_REGISTRO))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_REGISTRO)));
            }
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format( "Inicia creación de PP {0}/{1}",CONTENEDOR, FECHA_EXPIRACION));

        
            if (string.IsNullOrEmpty(CONTENEDOR))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(CONTENEDOR)));
            }


            //if (string.IsNullOrEmpty(this.NUMERO_PASE_N4))
            //{
            //    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
            //}
            if (!ID_CARGA.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_CARGA)));
            }
      
            if (string.IsNullOrEmpty(this.ID_EMPRESA))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_EMPRESA)));
            }


            if (string.IsNullOrEmpty(this.TRANSPORTISTA_DESC))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(TRANSPORTISTA_DESC)));
            }
            //if (string.IsNullOrEmpty(this.CONSIGNARIO_NOMBRE))
            //{
            //    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(CONSIGNARIO_NOMBRE)));
            //}
            //if (string.IsNullOrEmpty(this.CONSIGNATARIO_ID))
            //{
            //    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(CONSIGNATARIO_ID)));
            //}

            //si no es DD, debe validar todo esto.
            if (!IsDD)
            {
                //@HORA_TURNO
                if (string.IsNullOrEmpty(HORA_TURNO))
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(HORA_TURNO)));
                }
                if (!this.TINICIA.HasValue)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(TINICIA)));
                }
                if (!this.TFIN.HasValue)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(TFIN)));
                }
                if (!TID.HasValue)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(TID)));
                }
                tt = SetMessage("RANGO_INVALIDO", actualMetodo, USUARIO_REGISTRO);
                if (this.TINICIA >= TFIN)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(TINICIA), nameof(TFIN)));
                }
                tt = SetMessage("NO_CERO", actualMetodo, USUARIO_REGISTRO);
                if (ID_PLAN <= 0)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN)));
                }
                if (ID_SECUENCIA <= 0)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_SECUENCIA)));
                }
            }
            else
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("PP es DD {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));
            }
            //AQUI OTRAS REGLAS DE VALIDACIÓN AL NUEVO!!
                //inicializa
                     



            N4Ws.Entidad.gate gt = new N4Ws.Entidad.gate();
            gt.appointment = new N4Ws.Entidad.appointment();
            gt.appointment.appointment_date = FECHA_EXPIRACION;

            var cita = this.Accesorio.ObtenerConfiguracion("HORACITA");
            if (cita!=null && !string.IsNullOrEmpty(cita.valor) && cita.valor.ToUpper().Equals("SI"))
            {
                gt.appointment.appointment_time = FECHA_EXPIRACION;
            }


            gt.appointment.container_id = CONTENEDOR;
            gt.appointment.driver_id = string.IsNullOrEmpty(this.ID_CHOFER) ? null : ID_CHOFER;
            gt.appointment.truck_id = string.IsNullOrEmpty(this.ID_PLACA) ? null : ID_PLACA;

            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.CrearNuevoAppointment(gt, USUARIO_REGISTRO);

            if (n4r.status != 0)
            {
               
                var ex = new ApplicationException(n4r.status_id);
                var i =  this.LogError<ApplicationException>(ex,"InsertarPase",USUARIO_REGISTRO);
                var emsg = string.Format("Novedad #{0} el turno ya no se encuentra disponible por favor reintente con otro.",i.HasValue?i.Value:-1);
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(emsg);
            }
            this.NUMERO_PASE_N4 = n4r.messages.First()?.message_severity;
            //aca grabar el pase a puerta pues todo salio bien en n4.
            this.Parametros.Clear();
             this.Parametros.Add(nameof(ID_PLAN), ID_PLAN);
             this.Parametros.Add(nameof(ID_SECUENCIA), ID_SECUENCIA);
             this.Parametros.Add(nameof(CONTENEDOR), CONTENEDOR);
            this.Parametros.Add(nameof(ID_CARGA), ID_CARGA);
            this.Parametros.Add(nameof(FECHA_EXPIRACION), FECHA_EXPIRACION);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(USUARIO_REGISTRO), USUARIO_REGISTRO);
            this.Parametros.Add(nameof(FECHA_REGISTRO), FECHA_REGISTRO);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(CONSIGNATARIO_ID), CONSIGNATARIO_ID);
            this.Parametros.Add(nameof(CONSIGNARIO_NOMBRE), CONSIGNARIO_NOMBRE);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(TID), TID);
            this.Parametros.Add(nameof(TINICIA), TINICIA);
            this.Parametros.Add(nameof(TFIN), TFIN);
            this.Parametros.Add(nameof(HORA_TURNO), HORA_TURNO);
            this.Parametros.Add(nameof(FECHA_CAS), FECHA_CAS);
            this.Parametros.Add(nameof(PPW), PPW);

             this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);

            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
             var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_pase_puerta_v1]",this.Parametros);
            this.ID_PASE = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la inserción {0}/{1}/{2}", CONTENEDOR, FECHA_EXPIRACION,result.MensajeProblema));
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Termina creación de PP {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }
        ////Cancelar pase de puerta
        public ResultadoOperacion<bool> Cancelar(string USUARIO_CANCELA, string MOTIVO_CANCELACION=null)
        {
            //pone pase a puerta CA, pone reserva a X, VBS_web a CA
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_CANCELA);
            if (string.IsNullOrEmpty(USUARIO_CANCELA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_CANCELA)));
            }
   
               if (string.IsNullOrEmpty(NUMERO_PASE_N4))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
            }
            if (ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }


            N4Ws.Entidad.gate gt = new N4Ws.Entidad.gate();
            var AP = new N4Ws.Entidad.appointment();
            AP.nbr = NUMERO_PASE_N4;
            gt.appointments.Add(AP);
            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.CancelarAppointments(gt, USUARIO_REGISTRO);
            if (n4r.status != 0)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = this.LogError<ApplicationException>(ex, string.Format("Traza n4:{0}",n4r.trace), USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de cancelación del pase, favor comuníquese con SAC", n4r.trace.HasValue ? n4r.trace.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(MOTIVO_CANCELACION), MOTIVO_CANCELACION);
            this.Parametros.Add(nameof(USUARIO_CANCELA), USUARIO_CANCELA);
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[cancelar_pase_puerta]", this.Parametros);
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la cancelacion {0}/{1}", ID_PASE, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Operación correcta");
        }

        public static ResultadoOperacion<Int64?> ReservarTurno(string USUARIO, int ID_PLAN, int ID_PLAN_SECUENCIA, int? ID_CNTR = null, DateTime? FECHA_SALIDA = null)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Int64?>.CrearFalla(pv);
            }

            p.Parametros.Clear();
            p.Parametros.Add(nameof(USUARIO), USUARIO);
            p.Parametros.Add(nameof(ID_PLAN), ID_PLAN);
            p.Parametros.Add(nameof(ID_PLAN_SECUENCIA), ID_PLAN_SECUENCIA);

            p.Parametros.Add(nameof(FECHA_SALIDA), FECHA_SALIDA);

            //NUEVO CODIGO--VALIDAR PLAN-SECUENCIA--PRIMARY KEY
            var rs = p.ExisteReserva(USUARIO, ID_PLAN, ID_PLAN_SECUENCIA);
            if (!rs.Exitoso)
            {
               
                return Respuesta.ResultadoOperacion<Int64?>.CrearFalla(rs.MensajeProblema);
            }
            //si la reserva existe.
            if (rs.Resultado)
            {
                 p.LogEvent(USUARIO, p.actualMetodo, string.Format("RESERVA USADA ID_PLAN:{0}/ID_PLAN_SECUENCIA:{1}", ID_PLAN, ID_PLAN_SECUENCIA));
                return Respuesta.ResultadoOperacion<Int64?>.CrearFalla(string.Format("El horario seleccionado ya fué asignado, por favor seleccione otro."));
            }


            if (ID_CNTR.HasValue)
            p.Parametros.Add(nameof(ID_CNTR), ID_CNTR);
            //todo HACER EL CAMBIO PARA SABER SI EXISTE

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nueva_reserva_termporal]", p.Parametros);
            var r= result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                p.LogEvent(USUARIO, p.actualMetodo, string.Format("Falló la inserción reserva ID_PLAN:{0}/SECUENCIA:{1}/{2}", ID_PLAN, ID_PLAN_SECUENCIA, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<Int64?>.CrearFalla(result.MensajeProblema);
            }

            return ResultadoOperacion<Int64?>.CrearResultadoExitoso(r, "Exito ak insertar");

        }
        //PASO 1->CON PARTIDA BUSCAR LOS PASES DE PUERTA
        public ResultadoOperacion<bool> Actualizar( string USUARIO_MODIFICA, string CONTENEDOR, DateTime? FECHA_TURNO , string HORA_TURNO, int? ID_PLAN, int? ID_SECUENCIA)
        {
            //inicializar
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            
            string pv = string.Empty;
           
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_MODIFICA);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(USUARIO_MODIFICA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_MODIFICA)));
            }
            if (string.IsNullOrEmpty(CONTENEDOR))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(CONTENEDOR)));
            }
            if (string.IsNullOrEmpty(NUMERO_PASE_N4))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
            }
            if (string.IsNullOrEmpty(NUMERO_PASE_N4_ANTERIOR))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4_ANTERIOR)));
            }

            //OBLIGATORIO
            if (ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }

    

            //SI PASA EMPRESA ENTONCES US DESCRIPCION ES OBLIGATORIO
            if (!string.IsNullOrEmpty(ID_EMPRESA))
            {
                if (string.IsNullOrEmpty(TRANSPORTISTA_DESC))
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(TRANSPORTISTA_DESC)));
            }
            //SI PASA LICENCIA ENTONCES NOMBRE OBLIGATORIO
            if (!string.IsNullOrEmpty(ID_CHOFER))
            {
                if (string.IsNullOrEmpty(CHOFER_DESC))
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(CHOFER_DESC)));
            }
            


            bool actualizaturno = false;
            //si alguno tiene valor, ambos deben tener valor
            if (ID_PLAN.HasValue || ID_SECUENCIA.HasValue)
            {
                if (!ID_PLAN.HasValue || !ID_SECUENCIA.HasValue)
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN)+'/'+nameof(ID_SECUENCIA)));
                }

                if (ID_PLAN.Value <= 0 || ID_SECUENCIA.Value <= 0)
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN) + '/' + nameof(ID_SECUENCIA)));
                }

                if (string.IsNullOrEmpty(this.NUMERO_PASE_N4))
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
                }

                if (!ID_CARGA.HasValue ||  ID_CARGA.Value <= 0)
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_CARGA)));
                }

                 if (!FECHA_TURNO.HasValue )
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(FECHA_TURNO)));
                }

                actualizaturno = true;
            }

            if (actualizaturno)
            {
                //ahora crear nuevo appointment, y obtener el numero, para la actualizacion
                N4Ws.Entidad.gate gt = new N4Ws.Entidad.gate();
                gt.appointment = new N4Ws.Entidad.appointment();
                gt.appointment.appointment_date = FECHA_EXPIRACION;
                //25-01-2021 (agregado)
                var cita = this.Accesorio.ObtenerConfiguracion("HORACITA");
                if (cita != null && !string.IsNullOrEmpty(cita.valor) && cita.valor.ToUpper().Equals("SI"))
                {
                    gt.appointment.appointment_time = FECHA_EXPIRACION;
                }


                gt.appointment.container_id = CONTENEDOR;
                gt.appointment.driver_id = string.IsNullOrEmpty(this.ID_CHOFER) ? null : ID_CHOFER;
                gt.appointment.truck_id = string.IsNullOrEmpty(this.ID_PLACA) ? null : ID_PLACA;

                //crear el apointment en N4
                var n4r = N4Ws.Entidad.Servicios.CrearNuevoAppointment(gt, USUARIO_REGISTRO);

                if (n4r.status != 0)
                {
                    string MsgError = string.Empty;

                    foreach (var s in n4r.messages)
                    {
                        MsgError = s.message_id;
                    }

                    if (MsgError.Equals("ERROR_NO_SLOTS_FOR_DATE_MESSAGE"))
                    {
                        MsgError = "turno ya fue se encuentra utilizado, por favor intentar con otro horario";
                    }

                    this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló creacion en N4 {0}/{1}/{2}", CONTENEDOR, FECHA_EXPIRACION, MsgError));
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format("{0}....{1}",n4r.status_id, MsgError));
                }

                this.NUMERO_PASE_N4 = n4r.messages.First()?.message_severity;


                //cancela el pase anterior
                //aqui debe hacer lo de N4, 
                N4Ws.Entidad.gate gta = new N4Ws.Entidad.gate();

                gta.appointments.Add(new N4Ws.Entidad.appointment() { nbr = this.NUMERO_PASE_N4_ANTERIOR });
                //intenta cancelar, si falla solo grabo log
                N4Ws.Entidad.N4_BasicResponse ca = N4Ws.Entidad.Servicios.CancelarAppointments(gta, USUARIO_MODIFICA);
                if (ca.status > 1)
                {
                    var ex = new ApplicationException(ca.status_id);
                    var i = this.LogError<ApplicationException>(ex, "Cancelar", USUARIO_REGISTRO);
                    var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de cancelación del pase, favor comuníquese con SAC", ca.trace.HasValue?ca.trace:-1);
                    //return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
                }

            }

            //AHORA SI GRABAR TURNO NUEVO.

            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(HORA_TURNO), HORA_TURNO);
            if (ID_PLAN.HasValue) { this.Parametros.Add(nameof(ID_PLAN), ID_PLAN); }
            if (ID_SECUENCIA.HasValue) { this.Parametros.Add(nameof(ID_SECUENCIA), ID_SECUENCIA); }
            this.Parametros.Add(nameof(FECHA_EXPIRACION), FECHA_EXPIRACION);
            this.Parametros.Add(nameof(TID), TID);
            this.Parametros.Add(nameof(TINICIA), TINICIA);
            this.Parametros.Add(nameof(TFIN), TFIN);
            this.Parametros.Add(nameof(USUARIO_MODIFICA), USUARIO_MODIFICA);


             var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
             var result =  BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[modifica_pase_puerta]",this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la ACTUALIZACION {0}/{1}/{2}", ID_PASE, CONTENEDOR,result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Actualizado con exito");
           
        }
        //PASO 2--> CON LAS LISTA DE CONTENEDORES SELECCIONADOS IMPRIMIR PASES DE PUERTA.
        public static Respuesta.ResultadoOperacion<System.Data.DataSet> ImprimirPasesContenedor(List<Int64> gkeys, string usuario)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            tt = p.SetMessage("SIN_REGISTROS", p.actualMetodo, usuario);
            if (gkeys == null || gkeys.Count < 0)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            p.Parametros.Clear();

            StringBuilder par = new StringBuilder();

            par.Append("<UNIT>");
            foreach (var g in gkeys)
            {
                if (g > 0)
                    par.AppendFormat("<VALOR ID_GKEY=\"{0}\"/>", g);
            }
            par.Append("</UNIT>");
            p.Parametros.Add("unit_all", par.ToString());

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[container_info_pase]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(dtw.MensajeProblema);
            }
            //tabla de middlware
            var dt1 = dtw.Resultado;
            bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            var dtp = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[container_info_pase]", p.Parametros);
            if (!dtp.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(dtp.MensajeProblema);
            }
            //tabla de n4
            var dt2 = dtp.Resultado;

     
            var resultsa = (from table1 in dt1.AsEnumerable()
                           join table2 in dt2.AsEnumerable() on table1.Field<Int64>("GKEY") equals table2.Field<Int64>("CNTR_CONSECUTIVO")
                            select   new
                            {
                                CONTAINER = table1["CONTENEDOR"] as string,
                                MRN =  table1["MRN"] as string,
                                MSN = table1["MSN"] as string,
                                HSN = table1["HSN"] as string,
                                BL = table1["BL"] as string,
                                DOCUMENTO = table1["DOCUMENTO"] as string,
                                PASE = table1["PASE"] as string,
                                TTURNO = table1["TTURNO"] as string,
                                TINICIO = table1["TINICIO"] as string,
                                TFIN = table1["TFIN"] as string,
                                IMPORTADOR = table1["IMPORTADOR"] as string,
                                SN = table1["SN"] as string,
                                RUC = table1["RUC"] as string
                                ,
                                EMPRESA = table1["EMPRESA"] as string,
                                PLACA = table1["PLACA"] as string,
                                LICENCIA = table1["LICENCIA"] as string,
                                CONDUCTOR = table1["CONDUCTOR"] as string,
                                ITEM = table1["ITEM"] as string,
                                PROVINCIA = table1["PROVINCIA"] as string,
                                GKEY = table1["GKEY"] as Int64?,
                                TELEFONO = table1["TELEFONO"] as string,
                                NETO = table1["NETO"] as decimal?,
                                BRUTO = table1["NETO"] as decimal?,

                                SELLO1 = table2["SELLO1"] as string,
                                SELLO2 = table2["SELLO2"] as string ,
                                SELLO3 = table2["SELLO3"] as string,
                                SELLO4 = table2["SELLO4"] as string,
                                SELLOCGSA = table2["SELLO_GEO"] as string,
                                BUQUE =table2["CNTR_VEPR_VSSL_NAME"] as string,
                                ADUANA = table2["CNTR_CLNT_CUSTOMER_AGENCY"] as string,
                                ISO = table2["CNTR_TYSZ_ISO"] as string,
                                LINE = table2["CNTR_CLNT_CUSTOMER_LINE"] as string,
                                UBICACION = table2["CNTR_POSITION"] as string,
                            }
                           ).ToList();



            var dts =new System.Data.DataSet();
            var tab = new System.Data.DataTable();
            tab.Columns.Add("CONTAINER", typeof(System.String)); //VBS_W
            tab.Columns.Add("MRN", typeof(System.String));//VBS_W
            tab.Columns.Add("MSN", typeof(System.String));//VBS_W
            tab.Columns.Add("HSN", typeof(System.String));//VBS_W
            tab.Columns.Add("BL", typeof(System.String));//VBS_W
            tab.Columns.Add("DOCUMENTO", typeof(System.String)); //VBS_W

            tab.Columns.Add("PASE", typeof(System.String)); //VBS_T (N4)
            tab.Columns.Add("TTURNO", typeof(System.String));//VBS_T +PAR
            tab.Columns.Add("TINICIO", typeof(System.String));//VBS_T +PAR
            tab.Columns.Add("TFIN", typeof(System.String));//VBS_T
         
            tab.Columns.Add("IMPORTADOR", typeof(System.String));//VBS_T
            tab.Columns.Add("SN", typeof(System.String)); // VBS_T->ID_PASE
            tab.Columns.Add("RUC", typeof(System.String)); //VBS_T
            tab.Columns.Add("EMPRESA", typeof(System.String)); //VBS_T
            tab.Columns.Add("PLACA", typeof(System.String));//VBS_t
            tab.Columns.Add("LICENCIA", typeof(System.String));//VBS_T
            tab.Columns.Add("CONDUCTOR", typeof(System.String));//VBS_T


            tab.Columns.Add("ITEM", typeof(System.String)); //?
            tab.Columns.Add("PROVINCIA", typeof(System.String));//?
            tab.Columns.Add("NETO", typeof(System.Decimal)); //? TARA N4
            tab.Columns.Add("BRUTO", typeof(System.Decimal));//?
            tab.Columns.Add("TELEFONO", typeof(System.String)); //?

            tab.Columns.Add("UBICACION", typeof(System.String)); // N4
            tab.Columns.Add("BUQUE", typeof(System.String)); //N4
            tab.Columns.Add("ADUANA", typeof(System.String)); //N4 CUSTOMER EGNCY
            tab.Columns.Add("SELLO1", typeof(System.String)); //N4
            tab.Columns.Add("SELLO2", typeof(System.String)); //N4
            tab.Columns.Add("SELLO3", typeof(System.String)); //N4
            tab.Columns.Add("SELLO4", typeof(System.String));//N4
             tab.Columns.Add("SELLOCGSA", typeof(System.String));//N4
            tab.Columns.Add("ISO", typeof(System.String)); //N4
            tab.Columns.Add("LINE", typeof(System.String)); //N4

            foreach (var i in resultsa)
            {
                var r = tab.NewRow();
                r[nameof(i.CONTAINER)] = i.CONTAINER;
                r[nameof(i.MRN)] = i.MRN;
                r[nameof(i.MSN)] = i.MSN;
                r[nameof(i.HSN)] = i.HSN;
                r[nameof(i.BL)] = i.BL;
                r[nameof(i.PASE)] = i.PASE;
                r[nameof(i.TTURNO)] = i.TTURNO;
                r[nameof(i.TINICIO)] = i.TINICIO;
                r[nameof(i.TFIN)] = i.TFIN;
                r[nameof(i.IMPORTADOR)] = i.IMPORTADOR;
                r[nameof(i.SN)] = i.SN;
                r[nameof(i.RUC)] = i.RUC;
                r[nameof(i.EMPRESA)] = i.EMPRESA;
                r[nameof(i.PLACA)] = i.PLACA;
                r[nameof(i.LICENCIA)] = i.LICENCIA;
                r[nameof(i.CONDUCTOR)] = i.CONDUCTOR;

                r[nameof(i.ITEM)] = i.ITEM;
                r[nameof(i.PROVINCIA)] = i.PROVINCIA;
                r[nameof(i.NETO)] = i.NETO;
                r[nameof(i.BRUTO)] = i.BRUTO;
                r[nameof(i.TELEFONO)] = i.TELEFONO;

                r[nameof(i.UBICACION)] = i.UBICACION;
                r[nameof(i.BUQUE)] = i.BUQUE;

                 r[nameof(i.ADUANA)] = i.ADUANA;
                r[nameof(i.SELLO1)] = i.SELLO1;
                  r[nameof(i.SELLO2)] = i.SELLO2;
                  r[nameof(i.SELLO3)] = i.SELLO3;
                  r[nameof(i.SELLO4)] = i.SELLO4;

                   r[nameof(i.DOCUMENTO)] = i.DOCUMENTO;
                r[nameof(i.SELLOCGSA)] = i.SELLOCGSA;
                r[nameof(i.ISO)] = i.ISO;
                 r[nameof(i.LINE)] = i.LINE;

                tab.Rows.Add(r);

            }
            dts.Tables.Add(tab);
    
           return ResultadoOperacion<System.Data.DataSet>.CrearResultadoExitoso(dts, string.Format("Filas enocntradas: {0}", tab.Rows.Count ));

        }
        public static Respuesta.ResultadoOperacion<System.Data.DataTable> ObtenerListaEditable(string MRN, string MSN, string HSN, string RUC,string USUARIO, DateTime? fecha = null)
        {
            /*
             [Bill].[lista_editar_pase] 
             */
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, USUARIO);
            if (string.IsNullOrEmpty(USUARIO))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO)));
            }
             if (string.IsNullOrEmpty(MRN))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
              if (string.IsNullOrEmpty(MSN))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
               if (string.IsNullOrEmpty(HSN))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }
            if (string.IsNullOrEmpty(RUC))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(RUC)));
            }
            /*hacer log de eventos*/

             p.Parametros.Clear();
            p.Parametros.Add(nameof(MRN),MRN);
             p.Parametros.Add(nameof(MSN),MSN);
             p.Parametros.Add(nameof(HSN),HSN);
            p.Parametros.Add(nameof(RUC), RUC);

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[lista_editar_pase]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(dtw.MensajeProblema);
            }
            return ResultadoOperacion<System.Data.DataTable>.CrearResultadoExitoso(dtw.Resultado);

        }
        public static ResultadoOperacion<bool> Marcar_Servicio(string ID_USUARIO, string CONTENEDOR, Int64 ID_PASE)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            p.Parametros.Clear();
            p.Parametros.Add(nameof(ID_USUARIO), ID_USUARIO);
            p.Parametros.Add(nameof(ID_PASE), ID_PASE);

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (string.IsNullOrEmpty(CONTENEDOR))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(CONTENEDOR)));
            }
            if (ID_PASE <= 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }

            var n4 = N4Ws.Entidad.Servicios.PonerEventoPasePuerta(CONTENEDOR, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio PASE {0}, RAZON {1}", ID_PASE, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[pase_carga_servicio]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la ACTUALIZACION DEL PASE {0} SERVICIO, {1}", ID_PASE, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }
            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }
        public static Respuesta.ResultadoOperacion<System.Data.DataTable> ObtenerListaEditable(string MRN, string MSN, string HSN, string RUC, string USUARIO, bool servicio,DateTime? fecha = null )
        {
            /*
             [Bill].[lista_editar_pase] 
             */
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, USUARIO);
            if (string.IsNullOrEmpty(USUARIO))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO)));
            }
            if (string.IsNullOrEmpty(MRN))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
            if (string.IsNullOrEmpty(MSN))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
            if (string.IsNullOrEmpty(HSN))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }
            if (string.IsNullOrEmpty(RUC))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(RUC)));
            }
            /*hacer log de eventos*/

            p.Parametros.Clear();
            p.Parametros.Add(nameof(MRN), MRN);
            p.Parametros.Add(nameof(MSN), MSN);
            p.Parametros.Add(nameof(HSN), HSN);
            p.Parametros.Add(nameof(RUC), RUC);
            p.Parametros.Add(nameof(servicio), servicio);

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[lista_editar_pase_servicio]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(dtw.MensajeProblema);
            }
            return ResultadoOperacion<System.Data.DataTable>.CrearResultadoExitoso(dtw.Resultado);

        }


        //PASO 1->CON PARTIDA BUSCAR LOS PASES DE PUERTA
        public ResultadoOperacion<bool> Autorizar(string USUARIO, string TIPO, string COMENTARIO, Int64 ID_PPWEB, string CONTENEDOR)
        {
            //inicializar
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            string pv = string.Empty;

            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(USUARIO))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO)));
            }
          

            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PPWEB), ID_PPWEB);
            this.Parametros.Add(nameof(TIPO), TIPO);
            this.Parametros.Add(nameof(COMENTARIO), COMENTARIO);
            this.Parametros.Add(nameof(USUARIO), USUARIO);
           
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[autoriza_pase_puerta]", this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO, actualMetodo, string.Format("Falló la AUTORIZACION {0}/{1}/{2}", ID_PPWEB, CONTENEDOR, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Autorizado con exito");

        }

        public ResultadoOperacion<bool> Cancelar_PorRetorno(string USUARIO_CANCELA, string MOTIVO_CANCELACION = null)
        {
            //pone pase a puerta CA, pone reserva a X, VBS_web a CA
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_CANCELA);
            if (string.IsNullOrEmpty(USUARIO_CANCELA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_CANCELA)));
            }

            if (string.IsNullOrEmpty(NUMERO_PASE_N4))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
            }
            if (ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }


            N4Ws.Entidad.gate gt = new N4Ws.Entidad.gate();
            var AP = new N4Ws.Entidad.appointment();
            AP.nbr = NUMERO_PASE_N4;
            gt.appointments.Add(AP);
            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.CancelarAppointments(gt, USUARIO_REGISTRO);
            if (n4r.status != 0)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = this.LogError<ApplicationException>(ex, string.Format("Traza n4:{0}", n4r.trace), USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de cancelación del pase, favor comuníquese con SAC", n4r.trace.HasValue ? n4r.trace.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(MOTIVO_CANCELACION), MOTIVO_CANCELACION);
            this.Parametros.Add(nameof(USUARIO_CANCELA), USUARIO_CANCELA);
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[cancelar_pase_puerta_retorno]", this.Parametros);
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la cancelacion {0}/{1}", ID_PASE, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Operación correcta");
        }


        public ResultadoOperacion<Int64> InsertarAppoimentEXPO(string CONTENEDOR,  bool IsDD = false)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_REGISTRO);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(this.USUARIO_REGISTRO))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_REGISTRO)));
            }
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Inicia creación de PP EXPO {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));


            if (string.IsNullOrEmpty(CONTENEDOR))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(CONTENEDOR)));
            }

            N4Ws.Entidad.gate gt = new N4Ws.Entidad.gate();
            gt.appointment = new N4Ws.Entidad.appointment();
            gt.appointment.appointment_date = FECHA_EXPIRACION;

            var cita = this.Accesorio.ObtenerConfiguracion("HORACITA");
            if (cita != null && !string.IsNullOrEmpty(cita.valor) && cita.valor.ToUpper().Equals("SI"))
            {
                gt.appointment.appointment_time = FECHA_EXPIRACION;
            }

            gt.appointment.container_id = CONTENEDOR;
            gt.appointment.driver_id = string.IsNullOrEmpty(this.ID_CHOFER) ? null : ID_CHOFER;
            gt.appointment.truck_id = string.IsNullOrEmpty(this.ID_PLACA) ? null : ID_PLACA;
            gt.appointment.gate_id = "RECEPTIO";
            gt.appointment.tran_type = "RE";
            //gt.appointment.appointment_date = FECHA_CAS;
            //gt.appointment.appointment_time = DateTime.Parse(HORA_TURNO);

            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.CrearNuevoAppointment(gt, USUARIO_REGISTRO);

            if (n4r.status != 0)
            {

                var ex = new ApplicationException(n4r.status_id);
                var i = this.LogError<ApplicationException>(ex, "InsertarPase", USUARIO_REGISTRO);
                var emsg = string.Format("Novedad #{0} el turno ya no se encuentra disponible por favor reintente con otro.", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(emsg);
            }

            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Termina creación de PP EXPO {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(n4r.status);
        }
    }
}
