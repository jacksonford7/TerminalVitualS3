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
    public class P2D_Pase_CFS : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public P2D_Pase_CFS()
        {
            OnInstanceCreate();
            this.ESTADO = "GN";
            this.CANTIDAD_CARGA = 0;
            this.FECHA_REGISTRO = DateTime.Now;
            this.TIPO_CARGA = "CFS";
        }
        public decimal ID_PASE { get; set; } //secuencil
        public decimal? ID_CARGA { get; set; } //gkey
        public string ESTADO { get; set; } //nuevo->GN
        public DateTime? FECHA_EXPIRACION { get; set; } // *
        public string ID_PLACA { get; set; }
        public string ID_CHOFER { get; set; }
        public string CHOFER_DESC { get; set; }

        public string ID_EMPRESA { get; set; }
        public string CONSIGNATARIO_ID { get; set; }
        public string CONSIGNARIO_NOMBRE { get; set; }

        public string USUARIO_REGISTRO { get; set; }
        public DateTime? FECHA_REGISTRO { get; set; }
        public string NUMERO_PASE_N4 { get; set; }
        public string TRANSPORTISTA_DESC { get; set; }

        public int? CANTIDAD_CARGA { get; set; }

       
        public string USUARIO_ESTADO { get; set; }
        public DateTime? FECHA_ESTADO { get; set; }
        public Int64? ID_RESERVA { get; set; }
        public string TIPO_CARGA { get; set; }
      
        public string MOTIVO_CANCELACION { get; set; }
        public bool? RESERVA { get; set; }
        public bool? ESTADO_RESERVA { get; set; }
        public Int64? PPW { get; set; }



        public bool? SERVICIO { get; set; }
        public DateTime? FECHA_SERVICIO { get; set; }
        public string USUARIO_SERVICIO { get; set; }

        public Int64? ID_UNIDAD { get; set; } //gkey

        public Int64? ID_CIUDAD { get; set; }
        public Int64? ID_ZONA { get; set; }
        public string DIRECCION { get; set; }
        public string ORDER_ID { get; set; }
        public string TRACKING_NUMBER { get; set; }
        public string ORDER_FINAL { get; set; }
        public bool? P2D { get; set; }
        public bool? ENVIADO_LIFTIF { get; set; }
        public decimal? LATITUD { get; set; }
        public decimal? LONGITUD { get; set; }

        public string CONTACTO { get; set; }
        public string CLIENTE { get; set; }
        public string DIRECCION_CLIENTE { get; set; }
        public string TELEFONOS { get; set; }
        public string EMAIL { get; set; }
        public string ID_CLIENTE { get; set; }
        public string PRODUCTO { get; set; }
        public decimal? PESO { get; set; }
        public decimal? VOLUMEN { get; set; }

        public static ResultadoOperacion<bool> EsPaseSinTurno(string ID_USUARIO, string mrn, string msn, string hsn)
        {
            var p = new Pase_CFS();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            p.Parametros.Clear();
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (string.IsNullOrEmpty(mrn))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }

            if (string.IsNullOrEmpty(msn))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            p.Parametros.Add(nameof(mrn), mrn); p.Parametros.Add(nameof(msn), msn); p.Parametros.Add(nameof(hsn), hsn);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoSelectEscalar<bool>(bcon, "Select [Bill].[pase_sin_turno](@mrn,@msn,@hsn)", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, "Fallo la consulta de Pase sin turno");
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }
            return ResultadoOperacion<bool>.CrearResultadoExitoso(result.Resultado, "Exito al consultar");

        }
        //debe insertar, debe modificar, debe eliminar
        //Insertar pase a puerta
        public ResultadoOperacion<Int64> Insertar(
            Int64 ID_PLAN, 
             string CONTENEDOR, string SUBITEMS, int TOTAL_BULTOS, string MRN, string MSN, string HSN,   bool IsDD=false)
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

        
            if (string.IsNullOrEmpty(MRN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
             if (string.IsNullOrEmpty(MSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
              if (string.IsNullOrEmpty(HSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }


            if (string.IsNullOrEmpty(SUBITEMS))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(SUBITEMS)));
            }
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

                tt = SetMessage("NO_CERO", actualMetodo, USUARIO_REGISTRO);
                if (ID_PLAN <= 0)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN)));
                }
                ////if (ID_SECUENCIA <= 0)
                ////{
                ////    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_SECUENCIA)));
                ////}
            }
            else
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("PP es DD {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));
            }
            //AQUI OTRAS REGLAS DE VALIDACIÓN AL NUEVO!!
            //inicializa


            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSADeliveryOrderQty";
            code.location = "database";
            code.parameters.Add("agencia",ID_EMPRESA);
            code.parameters.Add("camion", ID_PLACA);
            //verficar
            code.parameters.Add("fecha", FECHA_REGISTRO.HasValue? FECHA_REGISTRO.Value.ToString("yyyy-MM-dd HH:mm"):DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            code.parameters.Add("referencia", ID_EMPRESA);
            code.parameters.Add("BLs",string.Format("{0}-{1}-{2}",MRN,MSN,HSN));
            code.parameters.Add("codsubitem", SUBITEMS.Trim());
            code.parameters.Add("placa", ID_PLACA);
            code.parameters.Add("tipo_carga", "CFS");
            code.parameters.Add("consecutivo", ID_CARGA.ToString());
            code.parameters.Add("usuer", USUARIO_REGISTRO);
            code.parameters.Add("QTY", CANTIDAD_CARGA.HasValue? CANTIDAD_CARGA.Value.ToString():"0");
            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtension(code, USUARIO_REGISTRO);

            if (n4r.status != 1)
            {
               
                var ex = new ApplicationException(n4r.status_id);
                var i =  this.LogError<ApplicationException>(ex,"InsertarPaseCFS",USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del pase, favor comuníquese con SAC",i.HasValue?i.Value:-1);
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(emsg);
            }
            this.NUMERO_PASE_N4 = n4r.messages.First()?.message_severity;
            //aca grabar el pase a puerta pues todo salio bien en n4.
            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PLAN), ID_PLAN);

            this.Parametros.Add(nameof(CONTENEDOR), CONTENEDOR);
            this.Parametros.Add(nameof(ID_CARGA), ID_CARGA);
            this.Parametros.Add(nameof(FECHA_EXPIRACION), FECHA_EXPIRACION);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(CONSIGNATARIO_ID), CONSIGNATARIO_ID);
            this.Parametros.Add(nameof(CONSIGNARIO_NOMBRE), CONSIGNARIO_NOMBRE);
            this.Parametros.Add(nameof(USUARIO_REGISTRO), USUARIO_REGISTRO);
            this.Parametros.Add(nameof(FECHA_REGISTRO), FECHA_REGISTRO);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);
            this.Parametros.Add(nameof(PPW), PPW);
            this.Parametros.Add(nameof(CANTIDAD_CARGA), CANTIDAD_CARGA);
            this.Parametros.Add(nameof(SUBITEMS), SUBITEMS);
            this.Parametros.Add(nameof(TOTAL_BULTOS), TOTAL_BULTOS);
            this.Parametros.Add(nameof(ID_UNIDAD), ID_UNIDAD);

            this.Parametros.Add(nameof(ID_CIUDAD), ID_CIUDAD);
            this.Parametros.Add(nameof(ID_ZONA), ID_ZONA);
            this.Parametros.Add(nameof(DIRECCION), DIRECCION);
            this.Parametros.Add(nameof(P2D), P2D);
            this.Parametros.Add(nameof(ENVIADO_LIFTIF), ENVIADO_LIFTIF);
            this.Parametros.Add(nameof(LATITUD), LATITUD);
            this.Parametros.Add(nameof(LONGITUD), LONGITUD);
            this.Parametros.Add(nameof(CONTACTO), CONTACTO);
            this.Parametros.Add(nameof(CLIENTE), CLIENTE);
            this.Parametros.Add(nameof(DIRECCION_CLIENTE), DIRECCION_CLIENTE);
            this.Parametros.Add(nameof(TELEFONOS), TELEFONOS);
            this.Parametros.Add(nameof(EMAIL), EMAIL);
            this.Parametros.Add(nameof(ID_CLIENTE), ID_CLIENTE);
            this.Parametros.Add(nameof(PRODUCTO), PRODUCTO);
            this.Parametros.Add(nameof(PESO), PESO);
            this.Parametros.Add(nameof(VOLUMEN), VOLUMEN);

            //this.Parametros.Add(nameof(TID), TID);
            //this.Parametros.Add(nameof(TINICIA), TINICIA);
            //this.Parametros.Add(nameof(TFIN), TFIN);
            //   this.Parametros.Add(nameof(HORA_TURNO), HORA_TURNO);
            //   this.Parametros.Add(nameof(FECHA_CAS), FECHA_CAS);

            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
             var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_pase_puerta_cfs]",this.Parametros);
            this.ID_PASE = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la inserción {0}/{1}/{2}", ID_CARGA, FECHA_EXPIRACION,result.MensajeProblema));
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Termina creación de PP {0}/{1}", ID_CARGA, FECHA_EXPIRACION));
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        //crear pase de puerta multidespachos
        public ResultadoOperacion<Int64> Insertar_MultiDespacho(Int64 IV_ID, Int64 ID_PLAN, string CONTENEDOR, string SUBITEMS, int TOTAL_BULTOS, string MRN, string MSN, string HSN, bool IsDD = false)
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
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Inicia creación de PP {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));


            if (string.IsNullOrEmpty(MRN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
            if (string.IsNullOrEmpty(MSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
            if (string.IsNullOrEmpty(HSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }


            if (string.IsNullOrEmpty(SUBITEMS))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(SUBITEMS)));
            }
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
            

            //si no es DD, debe validar todo esto.
            if (!IsDD)
            {

                tt = SetMessage("NO_CERO", actualMetodo, USUARIO_REGISTRO);
                if (ID_PLAN <= 0)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN)));
                }
               
            }
            else
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("PP es DD {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));
            }
            //AQUI OTRAS REGLAS DE VALIDACIÓN AL NUEVO!!
            //inicializa


            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSADeliveryOrderQty";
            code.location = "database";
            code.parameters.Add("agencia", ID_EMPRESA);
            code.parameters.Add("camion", ID_PLACA);
            //verficar
            code.parameters.Add("fecha", FECHA_REGISTRO.HasValue ? FECHA_REGISTRO.Value.ToString("yyyy-MM-dd HH:mm") : DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            code.parameters.Add("referencia", ID_EMPRESA);
            code.parameters.Add("BLs", string.Format("{0}-{1}-{2}", MRN, MSN, HSN));
            code.parameters.Add("codsubitem", SUBITEMS.Trim());
            code.parameters.Add("placa", ID_PLACA);
            code.parameters.Add("tipo_carga", "CFS");
            code.parameters.Add("consecutivo", ID_CARGA.ToString());
            code.parameters.Add("usuer", USUARIO_REGISTRO);
            code.parameters.Add("QTY", CANTIDAD_CARGA.HasValue ? CANTIDAD_CARGA.Value.ToString() : "0");
            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtension(code, USUARIO_REGISTRO);

            if (n4r.status != 1)
            {

                var ex = new ApplicationException(n4r.status_id);
                var i = this.LogError<ApplicationException>(ex, "InsertarPaseCFS", USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del pase, favor comuníquese con SAC", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(emsg);
            }
            this.NUMERO_PASE_N4 = n4r.messages.First()?.message_severity;
            //aca grabar el pase a puerta pues todo salio bien en n4.
            this.Parametros.Clear();
            this.Parametros.Add(nameof(IV_ID), IV_ID);
            this.Parametros.Add(nameof(ID_PLAN), ID_PLAN);
            this.Parametros.Add(nameof(CONTENEDOR), CONTENEDOR);
            this.Parametros.Add(nameof(ID_CARGA), ID_CARGA);
            this.Parametros.Add(nameof(FECHA_EXPIRACION), FECHA_EXPIRACION);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(CONSIGNATARIO_ID), CONSIGNATARIO_ID);
            this.Parametros.Add(nameof(CONSIGNARIO_NOMBRE), CONSIGNARIO_NOMBRE);
            this.Parametros.Add(nameof(USUARIO_REGISTRO), USUARIO_REGISTRO);
            this.Parametros.Add(nameof(FECHA_REGISTRO), FECHA_REGISTRO);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);
            this.Parametros.Add(nameof(PPW), PPW);
            this.Parametros.Add(nameof(CANTIDAD_CARGA), CANTIDAD_CARGA);
            this.Parametros.Add(nameof(SUBITEMS), SUBITEMS);
            this.Parametros.Add(nameof(TOTAL_BULTOS), TOTAL_BULTOS);
            this.Parametros.Add(nameof(ID_UNIDAD), ID_UNIDAD);

            this.Parametros.Add(nameof(ID_CIUDAD), ID_CIUDAD);
            this.Parametros.Add(nameof(ID_ZONA), ID_ZONA);
            this.Parametros.Add(nameof(DIRECCION), DIRECCION);
            this.Parametros.Add(nameof(P2D), P2D);
            this.Parametros.Add(nameof(ENVIADO_LIFTIF), ENVIADO_LIFTIF);
            this.Parametros.Add(nameof(LATITUD), LATITUD);
            this.Parametros.Add(nameof(LONGITUD), LONGITUD);
            this.Parametros.Add(nameof(CONTACTO), CONTACTO);
            this.Parametros.Add(nameof(CLIENTE), CLIENTE);
            this.Parametros.Add(nameof(DIRECCION_CLIENTE), DIRECCION_CLIENTE);
            this.Parametros.Add(nameof(TELEFONOS), TELEFONOS);
            this.Parametros.Add(nameof(EMAIL), EMAIL);
            this.Parametros.Add(nameof(ID_CLIENTE), ID_CLIENTE);
            this.Parametros.Add(nameof(PRODUCTO), PRODUCTO);
            this.Parametros.Add(nameof(PESO), PESO);
            this.Parametros.Add(nameof(VOLUMEN), VOLUMEN);

           

            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_pase_puerta_cfs_multidespacho]", this.Parametros);
            this.ID_PASE = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la inserción {0}/{1}/{2}", ID_CARGA, FECHA_EXPIRACION, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Termina creación de PP {0}/{1}", ID_CARGA, FECHA_EXPIRACION));
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        //crear pase de puerta multidespachos
        public ResultadoOperacion<Int64> Insertar_MultiDespacho_Transporte(Int64 IV_ID, Int64 ID_PLAN, string CONTENEDOR, string SUBITEMS, int TOTAL_BULTOS, string MRN, string MSN, string HSN, bool IsDD = false)
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
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Inicia creación de PP {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));


            if (string.IsNullOrEmpty(MRN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
            if (string.IsNullOrEmpty(MSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
            if (string.IsNullOrEmpty(HSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }


            if (string.IsNullOrEmpty(SUBITEMS))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(SUBITEMS)));
            }
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


            //si no es DD, debe validar todo esto.
            if (!IsDD)
            {

                tt = SetMessage("NO_CERO", actualMetodo, USUARIO_REGISTRO);
                if (ID_PLAN <= 0)
                {
                    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN)));
                }

            }
            else
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("PP es DD {0}/{1}", CONTENEDOR, FECHA_EXPIRACION));
            }
            //AQUI OTRAS REGLAS DE VALIDACIÓN AL NUEVO!!
            //inicializa


            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSADeliveryOrderQty";
            code.location = "database";
            code.parameters.Add("agencia", ID_EMPRESA);
            code.parameters.Add("camion", ID_PLACA);
            //verficar
            code.parameters.Add("fecha", FECHA_REGISTRO.HasValue ? FECHA_REGISTRO.Value.ToString("yyyy-MM-dd HH:mm") : DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            code.parameters.Add("referencia", ID_EMPRESA);
            code.parameters.Add("BLs", string.Format("{0}-{1}-{2}", MRN, MSN, HSN));
            code.parameters.Add("codsubitem", SUBITEMS.Trim());
            code.parameters.Add("placa", ID_PLACA);
            code.parameters.Add("tipo_carga", "CFS");
            code.parameters.Add("consecutivo", ID_CARGA.ToString());
            code.parameters.Add("usuer", USUARIO_REGISTRO);
            code.parameters.Add("QTY", CANTIDAD_CARGA.HasValue ? CANTIDAD_CARGA.Value.ToString() : "0");
            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtension(code, USUARIO_REGISTRO);

            if (n4r.status != 1)
            {

                var ex = new ApplicationException(n4r.status_id);
                var i = this.LogError<ApplicationException>(ex, "InsertarPaseCFS", USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del pase, favor comuníquese con SAC", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(emsg);
            }
            this.NUMERO_PASE_N4 = n4r.messages.First()?.message_severity;
            //aca grabar el pase a puerta pues todo salio bien en n4.
            this.Parametros.Clear();
            this.Parametros.Add(nameof(IV_ID), IV_ID);
            this.Parametros.Add(nameof(ID_PLAN), ID_PLAN);
            this.Parametros.Add(nameof(CONTENEDOR), CONTENEDOR);
            this.Parametros.Add(nameof(ID_CARGA), ID_CARGA);
            this.Parametros.Add(nameof(FECHA_EXPIRACION), FECHA_EXPIRACION);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(CONSIGNATARIO_ID), CONSIGNATARIO_ID);
            this.Parametros.Add(nameof(CONSIGNARIO_NOMBRE), CONSIGNARIO_NOMBRE);
            this.Parametros.Add(nameof(USUARIO_REGISTRO), USUARIO_REGISTRO);
            this.Parametros.Add(nameof(FECHA_REGISTRO), FECHA_REGISTRO);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);
            this.Parametros.Add(nameof(PPW), PPW);
            this.Parametros.Add(nameof(CANTIDAD_CARGA), CANTIDAD_CARGA);
            this.Parametros.Add(nameof(SUBITEMS), SUBITEMS);
            this.Parametros.Add(nameof(TOTAL_BULTOS), TOTAL_BULTOS);
            this.Parametros.Add(nameof(ID_UNIDAD), ID_UNIDAD);

            this.Parametros.Add(nameof(ID_CIUDAD), ID_CIUDAD);
            this.Parametros.Add(nameof(ID_ZONA), ID_ZONA);
            this.Parametros.Add(nameof(DIRECCION), DIRECCION);
            this.Parametros.Add(nameof(P2D), P2D);
            this.Parametros.Add(nameof(ENVIADO_LIFTIF), ENVIADO_LIFTIF);
            this.Parametros.Add(nameof(LATITUD), LATITUD);
            this.Parametros.Add(nameof(LONGITUD), LONGITUD);
            this.Parametros.Add(nameof(CONTACTO), CONTACTO);
            this.Parametros.Add(nameof(CLIENTE), CLIENTE);
            this.Parametros.Add(nameof(DIRECCION_CLIENTE), DIRECCION_CLIENTE);
            this.Parametros.Add(nameof(TELEFONOS), TELEFONOS);
            this.Parametros.Add(nameof(EMAIL), EMAIL);
            this.Parametros.Add(nameof(ID_CLIENTE), ID_CLIENTE);
            this.Parametros.Add(nameof(PRODUCTO), PRODUCTO);
            this.Parametros.Add(nameof(PESO), PESO);
            this.Parametros.Add(nameof(VOLUMEN), VOLUMEN);



            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "P2D_MULTI_nuevo_pase_cfs", this.Parametros);
            this.ID_PASE = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la inserción {0}/{1}/{2}", ID_CARGA, FECHA_EXPIRACION, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Termina creación de PP {0}/{1}", ID_CARGA, FECHA_EXPIRACION));
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }


        ////Cancelar pase de puerta
        public ResultadoOperacion<bool> Cancelar(string USUARIO_CANCELA)
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
            if (string.IsNullOrEmpty(  USUARIO_CANCELA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_CANCELA)));
            }
            if (string.IsNullOrEmpty(this.MOTIVO_CANCELACION))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(MOTIVO_CANCELACION)));
            }
            if (string.IsNullOrEmpty(this.NUMERO_PASE_N4))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
            }
            if (this.ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }
            if (!this.PPW.HasValue || this.PPW.Value <=0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(PPW)));
            }

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            //code.name = "CGSADeliveryOrderCancel";
            //code.location = "database";
            code.name = "CGSADeliveryOrderCancelM";
            code.location = "code-extension";
            code.parameters.Add("OrderNbr", NUMERO_PASE_N4);
            code.parameters.Add("Nota", MOTIVO_CANCELACION);
            //verficar
            code.parameters.Add("fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            code.parameters.Add("tipo_carga", "CFS");
            //crear el apointment en N4
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, USUARIO_REGISTRO);
            if (n4r.status != 1)
            {
                string Mensaje_N4 = string.Empty;

                foreach (var s in n4r.messages)
                {
                    Mensaje_N4 = s.message_detail;
                }

                var ex = new ApplicationException(n4r.status_id);
                var i =  this.LogError<ApplicationException>(ex,"CancelarPaseCFS",USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del pase, el mismo presenta los siguientes problemas:{1}...favor comuníquese con SAC.", i.HasValue?i.Value:-1, Mensaje_N4);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(MOTIVO_CANCELACION), MOTIVO_CANCELACION);
            this.Parametros.Add(nameof(USUARIO_CANCELA), USUARIO_CANCELA);
            this.Parametros.Add(nameof(PPW), PPW);
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[cancelar_pase_puerta_cfs]", this.Parametros);
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la cancelacion {0}/{1}", ID_PASE, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Operación correcta");
        }
        public static ResultadoOperacion<Int64?> ReservarTurno(string USUARIO, int ID_PLAN, int ID_PLAN_SECUENCIA, int? ID_CNTR = null)
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
        public ResultadoOperacion<bool> Actualizar( string USUARIO_MODIFICA,   int? ID_PLAN, string SUBITEMS, string MRN, string MSN, string HSN)
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

            if (string.IsNullOrEmpty(MRN))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
            if (string.IsNullOrEmpty(MSN))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
            if (string.IsNullOrEmpty(HSN))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }
            if (string.IsNullOrEmpty(SUBITEMS))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(SUBITEMS)));
            }
            if (!FECHA_EXPIRACION.HasValue)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(FECHA_EXPIRACION)));
            }

            if (this.ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }
            if (!this.ID_CARGA.HasValue || this.ID_CARGA <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_CARGA)));
            }
            if (!this.PPW.HasValue ||  this.PPW <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(PPW)));
            }
            if (!ID_PLAN.HasValue || ID_PLAN.Value <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN)));
            }
            if (!CANTIDAD_CARGA.HasValue || CANTIDAD_CARGA.Value <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(CANTIDAD_CARGA)));
            }

            if (string.IsNullOrEmpty(ID_EMPRESA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_EMPRESA)));
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
            if (ID_PLAN.HasValue )
            {
                if (ID_PLAN.Value <= 0)
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PLAN) + '/' + nameof(ID_CARGA)));
                }
                if (string.IsNullOrEmpty(this.NUMERO_PASE_N4))
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(NUMERO_PASE_N4)));
                }

                if (!ID_CARGA.HasValue ||  ID_CARGA.Value <= 0)
                {
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_CARGA)));
                }
                actualizaturno = true;
            }

            if (actualizaturno)
            {
                //CANCELAR L DLV_ORDER
                N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
                //code.name = "CGSADeliveryOrderCancel";
                //code.location = "database";
                code.name = "CGSADeliveryOrderCancelM";
                code.location = "code-extension";
                code.parameters.Add("OrderNbr", NUMERO_PASE_N4);
                code.parameters.Add("Nota", MOTIVO_CANCELACION);
                //verficar
                code.parameters.Add("fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                code.parameters.Add("tipo_carga", "CFS");
                //crear el apointment en N4
                var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, USUARIO_REGISTRO);
                if (n4r.status != 1)
                {
                    string Mensaje_N4 = string.Empty;

                    foreach (var s in n4r.messages)
                    {
                        Mensaje_N4 = s.message_detail;
                    }

                    var ex = new ApplicationException(n4r.status_id);
                    var i = this.LogError<ApplicationException>(ex, "ActualizarCFS", USUARIO_REGISTRO);
                    var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del pase, el mismo presenta los siguientes problemas:{1}...favor comuníquese con SAC.", i.HasValue ? i.Value : -1, Mensaje_N4);
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
                }

                //CREAR EL DL_ORDER
                N4Ws.Entidad.GroovyCodeExtension code1 = new N4Ws.Entidad.GroovyCodeExtension();
                code1.name = "CGSADeliveryOrderQty";
                code1.location = "database";
                code1.parameters.Add("agencia", ID_EMPRESA);
                code1.parameters.Add("camion", ID_PLACA);
                //verficar
                code1.parameters.Add("fecha", FECHA_EXPIRACION.HasValue ? FECHA_EXPIRACION.Value.ToString("yyyy-MM-dd HH:mm") : DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                code1.parameters.Add("referencia", ID_EMPRESA);
                code1.parameters.Add("BLs", string.Format("{0}-{1}-{2}", MRN, MSN, HSN));
                code1.parameters.Add("codsubitem", SUBITEMS.Trim());
                code1.parameters.Add("placa", ID_PLACA);
                code1.parameters.Add("tipo_carga", "CFS");
                code1.parameters.Add("consecutivo", ID_CARGA.ToString());
                code1.parameters.Add("usuer", USUARIO_REGISTRO);
                code1.parameters.Add("QTY", CANTIDAD_CARGA.HasValue ? CANTIDAD_CARGA.Value.ToString() : "0");
                //crear el apointment en N4
                var n4r1 = N4Ws.Entidad.Servicios.EjecutarCODEExtension(code1, USUARIO_REGISTRO);
                if (n4r1.status != 1)
                {
                    var ex = new ApplicationException(n4r1.status_id);
                    var i = this.LogError<ApplicationException>(ex, "InsertarPaseCFS", USUARIO_REGISTRO);
                    var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del pase, favor comuníquese con SAC", i.HasValue ? i.Value : -1);
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
                }
                this.NUMERO_PASE_N4 = n4r1.messages.First()?.message_severity;
            }

            //AHORA SI GRABAR TURNO NUEVO.

            this.Parametros.Clear();
            this.Parametros.Add(nameof(PPW), PPW);
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            if (ID_PLAN.HasValue) { this.Parametros.Add(nameof(ID_PLAN), ID_PLAN); }
            this.Parametros.Add(nameof(ID_CARGA), ID_CARGA);
            this.Parametros.Add(nameof(FECHA_EXPIRACION), FECHA_EXPIRACION); ;
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(CONSIGNARIO_NOMBRE), CONSIGNARIO_NOMBRE);
            this.Parametros.Add(nameof(CONSIGNATARIO_ID), CONSIGNATARIO_ID);
            this.Parametros.Add(nameof(USUARIO_MODIFICA), USUARIO_MODIFICA);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);

            this.Parametros.Add(nameof(ID_CIUDAD), ID_CIUDAD);
            this.Parametros.Add(nameof(ID_ZONA), ID_ZONA);
            this.Parametros.Add(nameof(DIRECCION), DIRECCION);
            this.Parametros.Add(nameof(LATITUD), LATITUD);
            this.Parametros.Add(nameof(LONGITUD), LONGITUD);
           

            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
             var result =  BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[modifica_pase_puerta_cfs]",this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la ACTUALIZACION {0}/{1}/{2}", ID_PASE, ID_CARGA,result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Actualizado con exito");
           
        }

        //PASO 1->CON PARTIDA BUSCAR LOS PASES DE PUERTA
        public ResultadoOperacion<bool> Actualizar_MultiDespacho(string USUARIO_MODIFICA)
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

           
            if (this.ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }
            if (!this.ID_CARGA.HasValue || this.ID_CARGA <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_CARGA)));
            }
            if (!this.PPW.HasValue || this.PPW <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(PPW)));
            }
           
            
            if (string.IsNullOrEmpty(ID_EMPRESA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_EMPRESA)));
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



            //AHORA SI GRABAR TURNO NUEVO.

            this.Parametros.Clear();
            this.Parametros.Add(nameof(PPW), PPW);
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(ID_CARGA), ID_CARGA);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(CONSIGNARIO_NOMBRE), CONSIGNARIO_NOMBRE);
            this.Parametros.Add(nameof(CONSIGNATARIO_ID), CONSIGNATARIO_ID);
            this.Parametros.Add(nameof(USUARIO_MODIFICA), USUARIO_MODIFICA);
            this.Parametros.Add(nameof(NUMERO_PASE_N4), NUMERO_PASE_N4);
            this.Parametros.Add(nameof(TRANSPORTISTA_DESC), TRANSPORTISTA_DESC);

 
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[modifica_pase_puerta_cfs_multidespacho]", this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la ACTUALIZACION {0}/{1}/{2}", ID_PASE, ID_CARGA, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Actualizado con exito");

        }


        //PASO 2--> CON LAS LISTA DE CONTENEDORES SELECCIONADOS IMPRIMIR PASES DE PUERTA.
        public static Respuesta.ResultadoOperacion<System.Data.DataSet> ImprimirPasesCFS(List<string> pases, string usuario)
        {
            var p = new Pase_CFS();
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
            if (pases == null || pases.Count < 0)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            p.Parametros.Clear();

            StringBuilder par = new StringBuilder();

            par.Append("<PASE>");
            foreach (var g in pases)
            {
                if (!string.IsNullOrEmpty(g))
                    par.AppendFormat("<VALOR ID_PASE=\"{0}\"/>", g);
            }
            par.Append("</PASE>");
            p.Parametros.Add("pases_all", par.ToString());

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[info_pase_cfs]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(dtw.MensajeProblema);
            }
            par.Clear();
        

            par.Append("<PASE>");
            foreach (DataRow c in dtw.Resultado.Rows)
            {
                if (!string.IsNullOrEmpty(c.Field<string>("MRN")))
                    par.AppendFormat("<VALOR CARGA=\"{0}-{1}-{2}\"/>", c.Field<string>("MRN"),c.Field<string>("MSN"),c.Field<string>("HSN"));
            }
            par.Append("</PASE>");


            p.Parametros.Clear();
           p.Parametros.Add("pases_all", par.ToString());


            //tabla de middlware
            var dt1 = dtw.Resultado;
            bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            var dtp = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[info_pase_cfs]", p.Parametros);
            if (!dtp.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(dtp.MensajeProblema);
            }
            //tabla de n4
            var dt2 = dtp.Resultado;

            /*
             Nuevos campos a considerar:
       CANTIDAD_CARGA  (sp N4Middleware)
       BRBK_QUANTITY (sp N5)
       BRBK_BKLO_BLOCK (sp N5)
       OPERATION (sp N5)
       BRBK_VOLUME (sp N5)
       BRBK_GROSS_WEIGHT (sp N5)
       BRBK_DESCRIPTION (sp N5)
       BRBK_MRN (sp N5)
       BRBK_MSN (sp N5)
       BRBK_HSN (sp N5)
       BRBK_CONSECUTIVO (sp N5)

       Unión de ambos Sp
       BRBK_CONSECUTIVO=GKEY AND
       BRBK_MRN=MRN  AND BRBK_MSN=MSN AND BRBK_HSN=HSN

                    */
            var resultsa = (from table1 in dt1.AsEnumerable() //middleware
                           join table2 in dt2.AsEnumerable() // n4
                           on 
                           new {
                                   GKEY = table1.Field<Int64>("GKEY"),
                                   MRN =table1.Field<string>("MRN"),
                                   MSN =table1.Field<string>("MSN"),
                                   HSN =table1.Field<string>("HSN")
                               } 
                           equals
                           new {
                                   GKEY = table2.Field<Int64>("BRBK_CONSECUTIVO"),
                                   MRN =table2.Field<string>("BRBK_MRN"),
                                   MSN =table2.Field<string>("BRBK_MSN"),
                                   HSN =table2.Field<string>("BRBK_HSN")
                               }
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
                                RUC = table1["RUC"] as string,
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
                                CANTIDAD_CARGA = table1["CANTIDAD_CARGA"] as Int32?,
                                ENTREGA = table1["ENTREGA"] as string,
                                EXPIRA = table1["EXPIRA"] as string,
                                PRIMERA = table1["PRIMERA"] as string,
                                MARCA = table1["MARCA"] as string,

                                BRBK_QUANTITY = table2["BRBK_QUANTITY"] as double?,
                                BRBK_BKLO_BLOCK = table2["BRBK_BKLO_BLOCK"] as string ,
                                OPERATION = table2["OPERATION"] as string,
                                BRBK_VOLUME = table2["BRBK_VOLUME"] as double?,


                                BRBK_GROSS_WEIGHT = table2["BRBK_GROSS_WEIGHT"] as decimal?,
                                BRBK_DESCRIPTION = table2["BRBK_DESCRIPTION"] as string,
                                BRBK_MRN = table2["BRBK_MRN"] as string,
                                BRBK_MSN = table2["BRBK_MSN"] as string,
                                BRBK_HSN = table2["BRBK_HSN"] as string,
                                BRBK_CONSECUTIVO = table2["BRBK_CONSECUTIVO"] as Int64?,
                                SELLO_GEO = table2["SELLO_GEO"] as string
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
            tab.Columns.Add("GKEY", typeof(System.Int64));//VBS_T
            tab.Columns.Add("TELEFONO", typeof(System.String)); //?
            tab.Columns.Add("CANTIDAD_CARGA", typeof(System.Int32));//VBS_T

            tab.Columns.Add("ENTREGA", typeof(System.String)); //N4
            tab.Columns.Add("EXPIRA", typeof(System.String)); //N4 CUSTOMER EGNCY
            tab.Columns.Add("PRIMERA", typeof(System.String)); //N4
            tab.Columns.Add("MARCA", typeof(System.String)); //N4

            tab.Columns.Add("BRBK_BKLO_BLOCK", typeof(System.String)); // N4
             tab.Columns.Add("SELLO_GEO", typeof(System.String)); // N4
            tab.Columns.Add("BRBK_QUANTITY", typeof(System.Double)); //N4
            tab.Columns.Add("OPERATION", typeof(System.String)); //N4 CUSTOMER EGNCY
            tab.Columns.Add("BRBK_VOLUME", typeof(System.Double)); //N4


            tab.Columns.Add("BRBK_GROSS_WEIGHT", typeof(System.Decimal)); //N4
            tab.Columns.Add("BRBK_DESCRIPTION", typeof(System.String)); //N4
            tab.Columns.Add("BRBK_MRN", typeof(System.String));//N4
            tab.Columns.Add("BRBK_MSN", typeof(System.String));//N4
            tab.Columns.Add("BRBK_HSN", typeof(System.String)); //N4
            tab.Columns.Add("BRBK_CONSECUTIVO", typeof(System.Int64)); //N4

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
                r[nameof(i.DOCUMENTO)] = i.DOCUMENTO;
                r[nameof(i.CANTIDAD_CARGA)] = i.CANTIDAD_CARGA;
                //--
                r[nameof(i.ENTREGA)] = i.ENTREGA;
                r[nameof(i.EXPIRA)] = i.EXPIRA;
                r[nameof(i.PRIMERA)] = i.PRIMERA;
                r[nameof(i.MARCA)] = i.MARCA;


                //N4
                r[nameof(i.BRBK_BKLO_BLOCK)] = i.BRBK_BKLO_BLOCK;
                r[nameof(i.BRBK_QUANTITY)] = i.BRBK_QUANTITY;
                r[nameof(i.BRBK_GROSS_WEIGHT)] = i.BRBK_GROSS_WEIGHT;
                r[nameof(i.SELLO_GEO)] = i.SELLO_GEO;
                r[nameof(i.OPERATION)] = i.OPERATION;
                 r[nameof(i.BRBK_VOLUME)] = i.BRBK_VOLUME;
                r[nameof(i.BRBK_DESCRIPTION)] = i.BRBK_DESCRIPTION;
                r[nameof(i.BRBK_MRN)] = i.BRBK_MRN;
                r[nameof(i.BRBK_MSN)] = i.BRBK_MSN;
                r[nameof(i.BRBK_HSN)] = i.BRBK_HSN;

                
               
                r[nameof(i.BRBK_CONSECUTIVO)] = i.BRBK_CONSECUTIVO;
           //      r[nameof(i.LINE)] = i.LINE;

                tab.Rows.Add(r);

            }
            dts.Tables.Add(tab);
    
           return ResultadoOperacion<System.Data.DataSet>.CrearResultadoExitoso(dts, string.Format("Filas enocntradas: {0}", tab.Rows.Count ));

        }
        public static Respuesta.ResultadoOperacion<System.Data.DataTable> ObtenerListaEditable(string MRN, string MSN, string HSN, string RUC, string USUARIO, DateTime? fecha = null)
        {
            /*
             [Bill].[lista_editar_pase] 
             */
            var p = new Pase_CFS();
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

            if (fecha.HasValue)
            {
                p.Parametros.Add(nameof(fecha), fecha);
            }


            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[lista_editar_pase_cfs]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(dtw.MensajeProblema);
            }
            return ResultadoOperacion<System.Data.DataTable>.CrearResultadoExitoso(dtw.Resultado);

        }
        
        public static ResultadoOperacion<bool> Marcar_Servicio(string ID_USUARIO,Int64 UNIDAD_ID, Dictionary<Int64,string> PASES)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (UNIDAD_ID<=0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(UNIDAD_ID)));
            }
            if (PASES == null || PASES.Count <= 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(PASES)));
            }


            ID_USUARIO = ID_USUARIO.Trim();
            //Lo primero es cancelar los Pases y 
            p.Parametros.Clear();
          //  p.Parametros.Add(nameof(ID_USUARIO), ID_USUARIO);
            StringBuilder ebp = new StringBuilder();
            ebp.Append("<PASES>");
            foreach (var pi in PASES)
            {

                N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
                //code.name = "CGSADeliveryOrderCancel";
                //code.location = "database";
                code.name = "CGSADeliveryOrderCancelM";
                code.location = "code-extension";

                code.parameters.Add("OrderNbr", pi.Value);
                code.parameters.Add("Nota", "REASIGNACION");
                //verficar
                code.parameters.Add("fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                code.parameters.Add("tipo_carga", "CFS");
                //crear el apointment en N4
                var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, ID_USUARIO);
                if (n4r.status != 1)
                {
                    string Mensaje_N4 = string.Empty;

                    foreach (var s in n4r.messages)
                    {
                        Mensaje_N4 = s.message_detail;
                    }

                    var ex = new ApplicationException(n4r.status_id);
                    var i = p.LogError<ApplicationException>(ex, "CancelarPaseCFS_REAC", ID_USUARIO);
                    var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de cancelación del pase, el mismo presenta el siguiente problema:{1}..., favor comuníquese con el personal de GATE.", i.HasValue ? i.Value : -1, Mensaje_N4);
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
                }

                ebp.AppendFormat("<PASE ID=\"{0}\" USUARIO=\"{1}\" />",pi.Key, ID_USUARIO.Trim());
            }
             ebp.Append("</PASES>");
             p.Parametros.Add(nameof(PASES), ebp.ToString());


            //poner el evento
            var n4 = N4Ws.Entidad.Servicios.PonerEventoPasePuertaCFS(UNIDAD_ID, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio PASE_CFS {0}, RAZON {1}", UNIDAD_ID, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[pase_carga_servicio_cfs]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la ACTUALIZACION DEL PASE_CFS {0} RAZON, {1}", UNIDAD_ID, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }
            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }
        public static Respuesta.ResultadoOperacion<System.Data.DataTable> ObtenerListaEditable(string MRN, string MSN, string HSN, string RUC, string USUARIO, bool servicio,DateTime? fecha = null )
        {
            /*
             [Bill].[lista_editar_pase_cfs] 
             */
            var p = new Pase_CFS();
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
            //OBTENER LA LISTA DE PASES PARA EDITAR
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[lista_editar_pase_servicio_cfs]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(dtw.MensajeProblema);
            }
            return ResultadoOperacion<System.Data.DataTable>.CrearResultadoExitoso(dtw.Resultado);

        }
        

        public static Respuesta.ResultadoOperacion<System.Data.DataSet> ImprimirPasesCFS_ALT(List<string> pases, string usuario)
        { 
            var p = new Pase_CFS();
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
            if (pases == null || pases.Count < 0)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            p.Parametros.Clear();

            StringBuilder par = new StringBuilder();

            par.Append("<PASE>");
            foreach (var g in pases)
            {
                if (!string.IsNullOrEmpty(g))
                    par.AppendFormat("<VALOR ID_PASE=\"{0}\"/>", g);
            }
            par.Append("</PASE>");
            p.Parametros.Add("pases_all", par.ToString());

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[info_pase_cfs]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(dtw.MensajeProblema);
            }
            //tabla de middlware
            var dt1 = dtw.Resultado;

            //Me entrega los GKEY DE LA TABLE DE PASES
            var unit_keys = dt1.AsEnumerable().Where(n => n.Field<Int64?>("GKEY").HasValue).Distinct().Select(a=>a.Field<Int64>("GKEY")).ToList();

            par.Clear();
            par.Append("<PASE>");
            foreach (var c in unit_keys)
            {
                    par.AppendFormat("<VALOR CARGA=\"{0}\"/>", c);
            }
            par.Append("</PASE>");
            p.Parametros.Clear();
             p.Parametros.Add("pases_all", par.ToString());

            bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
            var dtp = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[info_pase_cfs_unit]", p.Parametros);
            if (!dtp.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataSet>.CrearFalla(dtp.MensajeProblema);
            }
            //tabla de n4
            var dt2 = dtp.Resultado;


            var resultsa = (from table1 in dt1.AsEnumerable() //middleware
                           join table2 in dt2.AsEnumerable() // n4
                           on 
                           new {
                                   GKEY = table1.Field<Int64>("GKEY"),
                                   MRN =table1.Field<string>("MRN"),
                                   MSN =table1.Field<string>("MSN"),
                                   HSN =table1.Field<string>("HSN")
                               } 
                           equals
                           new {
                                   GKEY = table2.Field<Int64>("BRBK_CONSECUTIVO"),
                                   MRN =table2.Field<string>("BRBK_MRN"),
                                   MSN =table2.Field<string>("BRBK_MSN"),
                                   HSN =table2.Field<string>("BRBK_HSN")
                               }
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
                                RUC = table1["RUC"] as string,
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
                                CANTIDAD_CARGA = table1["CANTIDAD_CARGA"] as Int32?,
                                ENTREGA = table1["ENTREGA"] as string,
                                EXPIRA = table1["EXPIRA"] as string,
                                PRIMERA = table1["PRIMERA"] as string,
                                MARCA = table1["MARCA"] as string,

                                BRBK_QUANTITY = table2["BRBK_QUANTITY"] as double?,
                                BRBK_BKLO_BLOCK = table2["BRBK_BKLO_BLOCK"] as string ,
                                OPERATION = table2["OPERATION"] as string,
                                BRBK_VOLUME = table2["BRBK_VOLUME"] as double?,


                                BRBK_GROSS_WEIGHT = table2["BRBK_GROSS_WEIGHT"] as decimal?,
                                BRBK_DESCRIPTION = table2["BRBK_DESCRIPTION"] as string,
                                BRBK_MRN = table2["BRBK_MRN"] as string,
                                BRBK_MSN = table2["BRBK_MSN"] as string,
                                BRBK_HSN = table2["BRBK_HSN"] as string,
                                BRBK_CONSECUTIVO = table2["BRBK_CONSECUTIVO"] as Int64?,
                              //  SELLO_GEO = table2["SELLO_GEO"] as string
                                 SELLO_GEO = table1["SELLO_SAT"] as string
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
            tab.Columns.Add("GKEY", typeof(System.Int64));//VBS_T
            tab.Columns.Add("TELEFONO", typeof(System.String)); //?
            tab.Columns.Add("CANTIDAD_CARGA", typeof(System.Int32));//VBS_T

            tab.Columns.Add("ENTREGA", typeof(System.String)); //N4
            tab.Columns.Add("EXPIRA", typeof(System.String)); //N4 CUSTOMER EGNCY
            tab.Columns.Add("PRIMERA", typeof(System.String)); //N4
            tab.Columns.Add("MARCA", typeof(System.String)); //N4

            tab.Columns.Add("BRBK_BKLO_BLOCK", typeof(System.String)); // N4
             tab.Columns.Add("SELLO_GEO", typeof(System.String)); // N4
            tab.Columns.Add("BRBK_QUANTITY", typeof(System.Double)); //N4
            tab.Columns.Add("OPERATION", typeof(System.String)); //N4 CUSTOMER EGNCY
            tab.Columns.Add("BRBK_VOLUME", typeof(System.Double)); //N4


            tab.Columns.Add("BRBK_GROSS_WEIGHT", typeof(System.Decimal)); //N4
            tab.Columns.Add("BRBK_DESCRIPTION", typeof(System.String)); //N4
            tab.Columns.Add("BRBK_MRN", typeof(System.String));//N4
            tab.Columns.Add("BRBK_MSN", typeof(System.String));//N4
            tab.Columns.Add("BRBK_HSN", typeof(System.String)); //N4
            tab.Columns.Add("BRBK_CONSECUTIVO", typeof(System.Int64)); //N4

            foreach (var i in resultsa.Distinct())
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
                r[nameof(i.DOCUMENTO)] = i.DOCUMENTO;
                r[nameof(i.CANTIDAD_CARGA)] = i.CANTIDAD_CARGA;
                //--
                r[nameof(i.ENTREGA)] = i.ENTREGA;
                r[nameof(i.EXPIRA)] = i.EXPIRA;
                r[nameof(i.PRIMERA)] = i.PRIMERA;
                r[nameof(i.MARCA)] = i.MARCA;


                //N4
                r[nameof(i.BRBK_BKLO_BLOCK)] = i.BRBK_BKLO_BLOCK;
                r[nameof(i.BRBK_QUANTITY)] = i.BRBK_QUANTITY;
                r[nameof(i.BRBK_GROSS_WEIGHT)] = i.BRBK_GROSS_WEIGHT;
                r[nameof(i.SELLO_GEO)] = i.SELLO_GEO;
                r[nameof(i.OPERATION)] = i.OPERATION;
                 r[nameof(i.BRBK_VOLUME)] = i.BRBK_VOLUME;
                r[nameof(i.BRBK_DESCRIPTION)] = i.BRBK_DESCRIPTION;
                r[nameof(i.BRBK_MRN)] = i.BRBK_MRN;
                r[nameof(i.BRBK_MSN)] = i.BRBK_MSN;
                r[nameof(i.BRBK_HSN)] = i.BRBK_HSN;

                
               
                r[nameof(i.BRBK_CONSECUTIVO)] = i.BRBK_CONSECUTIVO;
                tab.Rows.Add(r);

            }
            dts.Tables.Add(tab);
    
           return ResultadoOperacion<System.Data.DataSet>.CrearResultadoExitoso(dts, string.Format("Filas enocntradas: {0}", tab.Rows.Count ));

        }




    }
}
