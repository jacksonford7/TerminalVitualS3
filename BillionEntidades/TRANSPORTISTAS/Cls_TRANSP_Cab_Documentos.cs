using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_Cab_Documentos : Cls_Bil_Base
    {
        #region "Propiedades Colaborador"


        public string RUC_EMPRESA { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string NOMINA_COD { get; set; }
        public string COLABORADOR { get; set; }
        public string NOMBRES { get; set; }
        public string FECHA_CADUCIDAD { get; set; }
        public string ESTADO { get; set; }
        public string NOVEDAD { get; set; }
        public string ESTADO2 { get; set; }
        public int ORDEN { get; set; }
        public string APELLIDOS { get; set; }
        public string TIPOSANGRE { get; set; }
        public string DIRECCIONDOM { get; set; }
        public string TELFDOM { get; set; }
        public DateTime FECHANAC { get; set; }
        public string CARGO { get; set; }

        #endregion

        #region "Propiedades Vehículo"
       
        public string PLACA { get; set; }
        public string VE_POLIZA { get; set; }    
        public string CLASETIPO { get; set; }
        public string MARCA { get; set; }
        public string MODELO { get; set; }
        public string COLOR { get; set; }
        public string TIPOCERTIFICADO { get; set; }
        public string CERTIFICADO { get; set; }
        public string CATEGORIA { get; set; }
        public string DESCRIPCIONCATEGORIA { get; set; }

        public DateTime? FECHAPOLIZA { get; set; }
        public DateTime? FECHAMTOP { get; set; }
        public string TIPO { get; set; }
        public Int64 IDSOLICITUD { get; set; }
        public string MAIL { get; set; }
        #endregion

        #region "Propiedades Pase de Puerta"
        public string AISV { get; set; }
        public Int64 PASE { get; set; }
        public string EMPRESA { get; set; }
        public string CHOFER { get; set; }
        public string NUMERO_CARGA { get; set; }
        public string USUARIOING { get; set; }
        #endregion

        public List<Cls_TRANSP_Colaborador> Colaborador { get; set; }
        public List<Cls_TRANSP_Vehiculo> Vehiculo { get; set; }

        public List<Cls_TRANSP_Doc_Vehiculo> Documento_Vehiculo { get; set; }
        public List<Cls_TRANSP_Doc_Colaborador> Documento_Colaborador { get; set; }

        private static Int64? lm = -3;

        public Cls_TRANSP_Cab_Documentos()
        {
            init();

            this.Colaborador = new List<Cls_TRANSP_Colaborador>();
            this.Vehiculo = new List<Cls_TRANSP_Vehiculo>();

            this.Documento_Vehiculo = new List<Cls_TRANSP_Doc_Vehiculo>();
            this.Documento_Colaborador = new List<Cls_TRANSP_Doc_Colaborador>();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        #region "Grabar Colaborador"
        public Int64? Save_Colaborador(out string OnError)
        {
            OnInit("Portal_Sca");


            parametros.Clear();
            parametros.Add("RUCCIPAS", this.RUC_EMPRESA);
            parametros.Add("CIPAS", this.NOMINA_COD);
            parametros.Add("NOMBRES", this.NOMBRES);
            parametros.Add("APELLIDOS", this.APELLIDOS);
            parametros.Add("TIPOSANGRE", this.TIPOSANGRE);
            parametros.Add("DIRECCIONDOM", this.DIRECCIONDOM);
            parametros.Add("TELFDOM", this.TELFDOM);
            parametros.Add("FECHANAC", this.FECHANAC);
            parametros.Add("CARGO", this.CARGO);
            parametros.Add("USUARIOING", this.IV_USUARIO_CREA);



            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "TRANSP_REGISTRA_SOL_COLABORADOR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {

                return null;
            }

            OnError = string.Empty;

            return db.Value;


        }

        public Int64? Enviar_Mail_Colaborador(out string OnError)
        {
            OnInit("Portal_Sca");


            parametros.Clear();
            parametros.Add("IDSOLICITUD", this.IDSOLICITUD);
            parametros.Add("CIPAS", this.NOMINA_COD);
            parametros.Add("MAIL", this.MAIL);
            parametros.Add("USUARIOING", this.IV_USUARIO_CREA);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "TRANSP_EMIAL_SOL_COLABORADOR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {

                return null;
            }

            OnError = string.Empty;

            return db.Value;


        }


        public Int64? SaveTransaction_Colaborador(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Colaborador(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar solicitud de actualización de documentos de Colaborador ****";
                    return 0;
                }
                ID = id.Value;
                this.IDSOLICITUD = id.Value;
                var nContador = 1;


                foreach (var i in this.Documento_Colaborador)
                {
                    i.ID_SOLICITUD = ID;
                    i.IV_USUARIO_CREA = this.IV_USUARIO_CREA;

                    var IdRetorno = i.Save_Colaborador(out OnError);
                    if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                    {
                        OnError = string.Format("Error: al grabar detalle de actualización de documentos de la solicitud del colaborador. {0}", OnError);

                        return 0;
                    }

                    nContador++;
                }

                //enviar mail
                var id_co = this.Enviar_Mail_Colaborador(out OnError);
                if (!id_co.HasValue)
                {

                }

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Colaborador), "SaveTransaction_Colaborador", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Grabar Vehiculo"
        public Int64? Save_Vehiculo(out string OnError)
        {
            OnInit("Portal_Sca");


            parametros.Clear();
            parametros.Add("RUCCIPAS", this.RUC_EMPRESA);
            parametros.Add("PLACA", this.PLACA);
            parametros.Add("CLASETIPO", this.CLASETIPO);
            parametros.Add("MARCA", this.MARCA);
            parametros.Add("MODELO", this.MODELO);
            parametros.Add("COLOR", this.COLOR);
            parametros.Add("TIPOCERTIFICADO", this.TIPOCERTIFICADO);
            parametros.Add("CERTIFICADO", this.CERTIFICADO);
            parametros.Add("CATEGORIA", this.CATEGORIA);
            parametros.Add("DESCRIPCIONCATEGORIA", this.DESCRIPCIONCATEGORIA);
            parametros.Add("FECHAPOLIZA", this.FECHAPOLIZA);
            parametros.Add("FECHAMTOP", this.FECHAMTOP);
            parametros.Add("TIPO", this.TIPO);
            parametros.Add("USUARIOING", this.IV_USUARIO_CREA);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "TRANSP_REGISTRA_SOL_VEHICULO", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {

                return null;
            }

            OnError = string.Empty;

            return db.Value;


        }

        public Int64? Enviar_Mail_Vehiculo(out string OnError)
        {
            OnInit("Portal_Sca");


            parametros.Clear();
            parametros.Add("IDSOLICITUD", this.IDSOLICITUD);
            parametros.Add("PLACA", this.PLACA);
            parametros.Add("MAIL", this.MAIL);
            parametros.Add("USUARIOING", this.IV_USUARIO_CREA);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "TRANSP_EMIAL_SOL_VEHICULO", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {

                return null;
            }

            OnError = string.Empty;

            return db.Value;


        }

        public Int64? SaveTransaction_Vehiculo(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Vehiculo(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar solicitud de actualización de documentos de Vehículo ****";
                    return 0;
                }
                ID = id.Value;
                this.IDSOLICITUD = id.Value;

                var nContador = 1;


                foreach (var i in this.Documento_Vehiculo)
                {
                    i.ID_SOLICITUD = ID;
                    i.IV_USUARIO_CREA = this.IV_USUARIO_CREA;

                    var IdRetorno = i.Save_Vehiculo(out OnError);
                    if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                    {
                        OnError = string.Format("Error: al grabar detalle de actualización de documentos de la solicitud de Vehículo. {0}", OnError);

                        return 0;
                    }

                    nContador++;
                }

                //enviar mail
                var id_vh = this.Enviar_Mail_Vehiculo(out OnError);
                if (!id_vh.HasValue)
                {
                  
                }


                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Vehiculo), "SaveTransaction_Vehiculo", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Enviar mail pase de puerta y aisv"
        public Int64? Enviar_Mail_PasePuerta(out string OnError)
        {
            OnInit("Portal_Sca");


            parametros.Clear();
            parametros.Add("TIPO", this.TIPO);
            parametros.Add("PASE", this.PASE);
            parametros.Add("AISV", this.AISV);
            parametros.Add("EMPRESA", this.EMPRESA);
            parametros.Add("CHOFER", this.CHOFER);
            parametros.Add("PLACA", this.PLACA);
            parametros.Add("MAIL", this.MAIL);
            parametros.Add("NUMERO_CARGA", this.NUMERO_CARGA);
            parametros.Add("USUARIOING", this.USUARIOING);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "TRANSP_EMAIL_PASE_PUERTA", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {

                return null;
            }

            OnError = string.Empty;

            return db.Value;


        }

        #endregion

    }
}
