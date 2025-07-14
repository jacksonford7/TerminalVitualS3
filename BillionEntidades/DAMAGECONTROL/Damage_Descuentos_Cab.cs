using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Damage_Descuentos_Cab : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private Int64 _DESC_ID;
        private DateTime? _DESC_DESDE = null;
        private DateTime? _DESC_HASTA = null;
        private string _DESC_NOTA = string.Empty;
        private string _DESC_ESTADO = string.Empty;
        private string _DESC_USER_CREA = string.Empty;
        private DateTime? _DESC_DATE_CREA = null;
        private string _DESC_USER_UPD = string.Empty;
        private DateTime? _DESC_DATE_UPD = null;
        private string _DESC_USER_ANIVEL1 = string.Empty;
        private DateTime? _DESC_DATE_ANIVEL1 = null;
        private string _DESC_USER_ANIVEL2 = string.Empty;
        private DateTime? _DESC_DATE_ANIVEL2 = null;
        private string _DESC_USER_RECHAZA = string.Empty;
        private DateTime? _DESC_DATE_RECHAZA = null;
        private bool _DESC_DELETE = false;
        private string _LINEAS = string.Empty;


        #endregion

        #region "Propiedades"

        public Int64 DESC_ID { get => _DESC_ID; set => _DESC_ID = value; }
        public DateTime? DESC_DESDE { get => _DESC_DESDE; set => _DESC_DESDE = value; }
        public DateTime? DESC_HASTA { get => _DESC_HASTA; set => _DESC_HASTA = value; }
        public string DESC_NOTA { get => _DESC_NOTA; set => _DESC_NOTA = value; }
        public string DESC_ESTADO { get => _DESC_ESTADO; set => _DESC_ESTADO = value; }
        public string DESC_USER_CREA { get => _DESC_USER_CREA; set => _DESC_USER_CREA = value; }
        public DateTime? DESC_DATE_CREA { get => _DESC_DATE_CREA; set => _DESC_DATE_CREA = value; }
        public string DESC_USER_UPD { get => _DESC_USER_UPD; set => _DESC_USER_UPD = value; }
        public DateTime? DESC_DATE_UPD { get => _DESC_DATE_UPD; set => _DESC_DATE_UPD = value; }

        public string DESC_USER_ANIVEL1 { get => _DESC_USER_ANIVEL1; set => _DESC_USER_ANIVEL1 = value; }
        public DateTime? DESC_DATE_ANIVEL1 { get => _DESC_DATE_ANIVEL1; set => _DESC_DATE_ANIVEL1 = value; }
        public string DESC_USER_ANIVEL2 { get => _DESC_USER_ANIVEL2; set => _DESC_USER_ANIVEL2 = value; }
        public DateTime? DESC_DATE_ANIVEL2 { get => _DESC_DATE_ANIVEL2; set => _DESC_DATE_ANIVEL2 = value; }
        public string DESC_USER_RECHAZA { get => _DESC_USER_RECHAZA; set => _DESC_USER_RECHAZA = value; }
        public DateTime? DESC_DATE_RECHAZA { get => _DESC_DATE_RECHAZA; set => _DESC_DATE_RECHAZA = value; }

        public bool DESC_DELETE { get => _DESC_DELETE; set => _DESC_DELETE = value; }

        private static String v_mensaje = string.Empty;

        public string LINEAS { get => _LINEAS; set => _LINEAS = value; }

        #endregion



        public List<Damage_Descuentos_Det> Detalle { get; set; }
        public List<Damage_Detalle_Contenedor> Detalle_Contenedores { get; set; }

        public Damage_Descuentos_Cab()
        {
            init();

            this.Detalle = new List<Damage_Descuentos_Det>();
            this.Detalle_Contenedores = new List<Damage_Detalle_Contenedor>();
        }

     


        public Damage_Descuentos_Cab(  Int64 _DESC_ID,
         DateTime? _DESC_DESDE ,
         DateTime? _DESC_HASTA ,
         string _DESC_NOTA ,
         string _DESC_ESTADO ,
         string _DESC_USER_CREA,
         DateTime? _DESC_DATE_CREA,
         string _DESC_USER_UPD ,
         DateTime? _DESC_DATE_UPD ,
         string _DESC_USER_ANIVEL1 ,
         DateTime? _DESC_DATE_ANIVEL1 ,
         string _DESC_USER_ANIVEL2 ,
         DateTime? _DESC_DATE_ANIVEL2 ,
         string _DESC_USER_RECHAZA ,
         DateTime? _DESC_DATE_RECHAZA ,
         bool _DESC_DELETE
     )

        {
            this.DESC_ID = _DESC_ID;
            this.DESC_DESDE = _DESC_DESDE;
            this.DESC_HASTA = _DESC_HASTA;
            this.DESC_NOTA = _DESC_NOTA;
            this.DESC_ESTADO = _DESC_ESTADO;
            this.DESC_USER_CREA = _DESC_USER_CREA;
            this.DESC_DATE_CREA = _DESC_DATE_CREA;
            this.DESC_USER_UPD = _DESC_USER_UPD;
            this.DESC_DATE_UPD = _DESC_DATE_UPD;

            this.DESC_USER_ANIVEL1 = _DESC_USER_ANIVEL1;
            this.DESC_DATE_ANIVEL1 = _DESC_DATE_ANIVEL1;
            this.DESC_USER_ANIVEL2 = _DESC_USER_ANIVEL2;
            this.DESC_DATE_ANIVEL2 = _DESC_DATE_ANIVEL2;

            this.DESC_USER_RECHAZA = _DESC_USER_RECHAZA;
            this.DESC_DATE_RECHAZA = _DESC_DATE_RECHAZA;
            this.DESC_DELETE = _DESC_DELETE;

            this.Detalle = new List<Damage_Descuentos_Det>();
          
        }

        private int? PreValidationsTransaction(out string msg)
        {

            if (!this.DESC_DESDE.HasValue)
            {

                msg = "La fecha de inicio del descuento no es valida";
                return 0;
            }

            if (!this.DESC_HASTA.HasValue)
            {

                msg = "La fecha final del descuento no es valida";
                return 0;
            }

            if (string.IsNullOrEmpty(this.DESC_ESTADO))
            {
                msg = "Debe especificar el estado que crea la transacción";
                return 0;
            }


        
            msg = string.Empty;
            return 1;
        }


        private Int64? Save(out string OnError)
        {
         
            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);
            parametros.Add("DESC_DESDE", this.DESC_DESDE);
            parametros.Add("DESC_HASTA", this.DESC_HASTA);
            parametros.Add("DESC_NOTA", this.DESC_NOTA);
            parametros.Add("DESC_ESTADO", this.DESC_ESTADO);
            parametros.Add("DESC_USER_CREA", (string.IsNullOrEmpty(this.DESC_USER_CREA) ? "CGSA" : this.DESC_USER_CREA));
          
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "DAMAGE_DESCUENTO_CAB", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction(out string OnError)
        {

         
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    this.DESC_ID = id.Value;
                    var nContador = 1;
                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Detalle)
                    {
                        i.DESC_ID = id.Value;

                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            return 0;
                        }

                       
                        nContador = nContador + 1;
                    }

                    nContador = 1;

                    


                    //fin de la transaccion
                    scope.Complete();



                    return this.DESC_ID;
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Aprobar), "SaveTransaction_Aprobar", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #region "Listado pendientes"
        //listado de descuentos pendientes de aprobar en el nivel 1
        public static List<Damage_Descuentos_Cab> Descuentos_Pendientes_Aprobar(out string OnError)
        {


            return sql_puntero.ExecuteSelectControl<Damage_Descuentos_Cab>(sql_puntero.Conexion_Local, 6000, "DAMAGE_DESCUENTOS_PENDIENTES_N1", null, out OnError);

        }
        #endregion

        #region "Listado pendientes Nivel dos"
        //listado de descuentos pendientes de aprobar en el nivel 2
        public static List<Damage_Descuentos_Cab> Descuentos_Pendientes_Aprobar_Gerente(out string OnError)
        {

            return sql_puntero.ExecuteSelectControl<Damage_Descuentos_Cab>(sql_puntero.Conexion_Local, 6000, "DAMAGE_DESCUENTOS_PENDIENTES_N2", null, out OnError);

        }
        #endregion

        #region "Listado en base a consulta"
        //listado de descuentos pendientes de aprobar en el nivel 2
        public static List<Damage_Descuentos_Cab> Descuentos_Consultas(DateTime FECHA_DESDE,  DateTime FECHA_HASTA,  string ESTADO, out string OnError)
        {
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("ESTADO", ESTADO);
            return sql_puntero.ExecuteSelectControl<Damage_Descuentos_Cab>(sql_puntero.Conexion_Local, 6000, "DAMAGE_LISTADO_DESCUENTOS", parametros, out OnError);

        }
        #endregion

        #region "Aprobar descuentos en el nivel 1"


        private Int64? Aprobar_Update(out string OnError)
        {

            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);
            parametros.Add("DESC_USER_UPD", this.DESC_USER_UPD);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "DAMAGE_DESCUENTOS_PENDIENTES_N1_APROBAR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Aprobar(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Aprobar_Update(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al aprobar política de descuento ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Aprobar), "SaveTransaction_Aprobar", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion

        #region "Anular política "
      
        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);
            parametros.Add("DESC_USER_UPD", this.DESC_USER_UPD);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "DAMAGE_DESCUENTOS_PENDIENTES_N1_CANCELAR", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        #endregion

        #region "Cargar descuento nivel 1"
        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);

            var t = sql_puntero.ExecuteSelectOnly<Damage_Descuentos_Cab>(sql_puntero.Conexion_Local, 6000, "DAMAGE_CARGAR_CABECERA_DESCUENTO", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe políticas de descuentos disponible con el ID seleccionado: {0}", this.DESC_ID);
                return false;
            }

            this.DESC_ID = t.DESC_ID;
            this.DESC_DESDE = t.DESC_DESDE;
            this.DESC_HASTA = t.DESC_HASTA;
            this.DESC_NOTA = t.DESC_NOTA;
            this.DESC_ESTADO = t.DESC_ESTADO;
            this.DESC_USER_CREA = t.DESC_USER_CREA;
            this.DESC_DATE_CREA = t.DESC_DATE_CREA;
            this.DESC_USER_UPD = t.DESC_USER_UPD;
            this.DESC_DATE_UPD = t.DESC_DATE_UPD;
            this.DESC_USER_ANIVEL1 = t.DESC_USER_ANIVEL1;
            this.DESC_DATE_ANIVEL2 = t.DESC_DATE_ANIVEL2;
            this.DESC_USER_RECHAZA = t.DESC_USER_RECHAZA;
            this.DESC_DATE_RECHAZA = t.DESC_DATE_RECHAZA;
            this.DESC_DELETE = t.DESC_DELETE;

            this.Detalle = Damage_Descuentos_Det.Detalle_Descuentos(this.DESC_ID, out OnError);

            if (!string.IsNullOrEmpty(OnError))
            {
                OnError = string.Format("Error al cargar detalle de políticas de descuentos..{0}", OnError);
                return false;
            }

            OnError = string.Empty;
            return true;
        }
        #endregion


        #region "Aprobar descuentos en el nivel 2"


        private Int64? Aprobar_Update_N2(out string OnError)
        {

            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);
            parametros.Add("DESC_USER_UPD", this.DESC_USER_UPD);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "DAMAGE_DESCUENTOS_PENDIENTES_N2_APROBAR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Aprobar_N2(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Aprobar_Update_N2(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al aprobar política de descuento ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Aprobar_N2), "SaveTransaction_Aprobar_N2", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion

        #region "Rechazar política "

        public bool Rechazar(out string OnError)
        {
            parametros.Clear();
            parametros.Add("DESC_ID", this.DESC_ID);
            parametros.Add("DESC_USER_UPD", this.DESC_USER_UPD);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "DAMAGE_DESCUENTOS_PENDIENTES_N2_CANCELAR", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        #endregion
    }
}
