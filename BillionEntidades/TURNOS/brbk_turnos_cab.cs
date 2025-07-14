using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_turnos_cab : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID_TURNO;
        private DateTime? _FECHA_DESDE = null;
        private DateTime? _FECHA_HASTA = null;
        private Int64? _ID_TIPO_PRODUCTO;
        private string _DESC_TIPO_PRODUCTO = string.Empty;
        private string _ID_BODEGA = string.Empty;
        private string _DESC_BODEGA = string.Empty;
        private Int64? _FRECUENCIA;
        private Int64? _CAPACIDAD;
        private string _NOMBRE_CABECERA = string.Empty;
        private string _CONCEPTO = string.Empty;
        private string _ESTADO = string.Empty;
       
        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;

        private string _xmlTurnos = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public DateTime? FECHA_DESDE { get => _FECHA_DESDE; set => _FECHA_DESDE = value; }
        public DateTime? FECHA_HASTA { get => _FECHA_HASTA; set => _FECHA_HASTA = value; }
        public Int64? ID_TIPO_PRODUCTO { get => _ID_TIPO_PRODUCTO; set => _ID_TIPO_PRODUCTO = value; }

        public string DESC_TIPO_PRODUCTO { get => _DESC_TIPO_PRODUCTO; set => _DESC_TIPO_PRODUCTO = value; }
        public string ID_BODEGA { get => _ID_BODEGA; set => _ID_BODEGA = value; }
        public string DESC_BODEGA { get => _DESC_BODEGA; set => _DESC_BODEGA = value; }
        public string NOMBRE_CABECERA { get => _NOMBRE_CABECERA; set => _NOMBRE_CABECERA = value; }

        public Int64? FRECUENCIA { get => _FRECUENCIA; set => _FRECUENCIA = value; }
        public Int64? CAPACIDAD { get => _CAPACIDAD; set => _CAPACIDAD = value; }

        public string CONCEPTO { get => _CONCEPTO; set => _CONCEPTO = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }

        public string xmlTurnos { get => _xmlTurnos; set => _xmlTurnos = value; }

        #endregion



        public List<brbk_turnos_det> Detalle { get; set; }
      

        public brbk_turnos_cab()
        {
            init();

            this.Detalle = new List<brbk_turnos_det>();
         
        }

        public brbk_turnos_cab(   Int64 _ID_TURNO,
         DateTime? _FECHA_DESDE,
         DateTime? _FECHA_HASTA ,
         Int64? _ID_TIPO_PRODUCTO,
         string _DESC_TIPO_PRODUCTO,
         string _ID_BODEGA ,
         string _DESC_BODEGA,
         int? _FRECUENCIA,
         int? _CAPACIDAD,

         string _CONCEPTO,
         string _ESTADO,

         string _USUARIO_CREA,
         string _USUARIO_MOD)

        {
            this.ID_TURNO = _ID_TURNO;
            this.FECHA_DESDE = _FECHA_DESDE;
            this.FECHA_HASTA = _FECHA_HASTA;
            this.ID_TIPO_PRODUCTO = _ID_TIPO_PRODUCTO;
            this.DESC_TIPO_PRODUCTO = _DESC_TIPO_PRODUCTO;
            this.ID_BODEGA = _ID_BODEGA;
            this.DESC_BODEGA = _DESC_BODEGA;
            this.FRECUENCIA = _FRECUENCIA;
            this.CAPACIDAD = _CAPACIDAD;
            this.CONCEPTO = _CONCEPTO;
            this.ESTADO = _ESTADO;
            this.USUARIO_CREA = _USUARIO_CREA;
            this.USUARIO_MOD = _USUARIO_MOD;
           


            this.Detalle = new List<brbk_turnos_det>();

        }


        #region "Graba Turnos"
        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("FECHA_DESDE", this.FECHA_DESDE);
            parametros.Add("FECHA_HASTA", this.FECHA_HASTA);
            parametros.Add("ID_TIPO_PRODUCTO", this.ID_TIPO_PRODUCTO);
            parametros.Add("ID_BODEGA", this.ID_BODEGA);
            parametros.Add("FRECUENCIA", this.FRECUENCIA);
            parametros.Add("CAPACIDAD", this.CAPACIDAD);
            parametros.Add("CONCEPTO", this.CONCEPTO);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            parametros.Add("xmlTurnos", this.xmlTurnos);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[GRABAR_TURNOS]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Actualizar Turnos"

        private Int64? Save_Update(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_TURNO", this.ID_TURNO);
            parametros.Add("CAPACIDAD", this.CAPACIDAD);
            parametros.Add("CONCEPTO", this.CONCEPTO);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            parametros.Add("xmlTurnos", this.xmlTurnos);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[ACTUALIZAR_TURNOS]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Update(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Update(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al actualizar turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Update), "SaveTransaction_Update", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Aprobar Turno"

        private Int64? Aprobar_Update(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_TURNO", this.ID_TURNO);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[APROBAR_TURNOS]", parametros, out OnError);
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
                    OnError = "*** Error: al Aprobar turnos ****";
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

        #region "Eliminar un turno"
        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_TURNO", this.ID_TURNO);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "[Brbk].[ELIMINA_TURNOS]", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }
        #endregion


        #region "Listados"
        public static List<brbk_turnos_cab> Turnos_Pendientes_Aprobar( out string OnError)
        {

         
            return sql_puntero.ExecuteSelectControl<brbk_turnos_cab>(sql_puntero.Conexion_Local, 6000, "[Brbk].[TURNOS_PENDIENTES]", null, out OnError);

        }
        #endregion

        #region "Poblar Turnos Pendientes de Aprobar"
        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_TURNO", this.ID_TURNO);

            var t = sql_puntero.ExecuteSelectOnly<brbk_turnos_cab>(sql_puntero.Conexion_Local, 6000, "[Brbk].[CARGA_CABECERA_TURNOS]", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe turnos disponible con el ID seleccionado: {0}", this.ID_TURNO);
                return false;
            }

            this.ID_TURNO = t.ID_TURNO;
            this.FECHA_DESDE = t.FECHA_DESDE;
            this.FECHA_HASTA = t.FECHA_HASTA;
            this.ID_TIPO_PRODUCTO = t.ID_TIPO_PRODUCTO;
            this.DESC_TIPO_PRODUCTO = t.DESC_TIPO_PRODUCTO;
            this.ID_BODEGA = t.ID_BODEGA;
            this.DESC_BODEGA = t.DESC_BODEGA;
            this.CAPACIDAD = t.CAPACIDAD;
            this.FRECUENCIA = t.FRECUENCIA;
            this.CONCEPTO = t.CONCEPTO;
            this.ESTADO = t.ESTADO;
            this.USUARIO_CREA = t.USUARIO_CREA;


            this.Detalle = brbk_turnos_det.Detalle_Turnos(this.ID_TURNO, out OnError);

            if (!string.IsNullOrEmpty(OnError))
            {
                OnError = string.Format("Error al cargar detalle de turnos..{0}", OnError);
                return false;
            }

            OnError = string.Empty;
            return true;
        }
        #endregion


        #region "Poblar Turnos Pendientes de Aprobar"
        public bool Valida_Existe_Turnos(out string OnError)
        {

            parametros.Clear();
            parametros.Add("FECHA_DESDE", this.FECHA_DESDE);
            parametros.Add("FECHA_HASTA", this.FECHA_HASTA);
            parametros.Add("ID_TIPO_PRODUCTO", this.ID_TIPO_PRODUCTO);
            parametros.Add("ID_BODEGA", this.ID_BODEGA);

            var t = sql_puntero.ExecuteSelectOnly<brbk_turnos_cab>(sql_puntero.Conexion_Local, 6000, "[Brbk].[VALIDA_TURNOS_EXISTENTE]", parametros);
            if (t == null)
            {
                // OnError = string.Format("No existe turnos disponible con el ID seleccionado: {0}", this.ID_TURNO);
                OnError = string.Empty;
                return false;
            }
            else
            {
                this.ID_TURNO = t.ID_TURNO;
                this.FECHA_DESDE = t.FECHA_DESDE;
                this.FECHA_HASTA = t.FECHA_HASTA;
                this.ID_TIPO_PRODUCTO = t.ID_TIPO_PRODUCTO;
                this.DESC_TIPO_PRODUCTO = t.DESC_TIPO_PRODUCTO;
                this.ID_BODEGA = t.ID_BODEGA;
                this.DESC_BODEGA = t.DESC_BODEGA;
                this.CAPACIDAD = t.CAPACIDAD;
                this.FRECUENCIA = t.FRECUENCIA;
                this.CONCEPTO = t.CONCEPTO;
                this.ESTADO = t.ESTADO;
                this.USUARIO_CREA = t.USUARIO_CREA;

                OnError = string.Format("Ya existe un turno activo con los criterios seleccionados, turno: {0}, fecha desde: {1}, fecha hasta: {2}, tipo producto: {3}, bodega: {4}", this.ID_TURNO, this.FECHA_DESDE.Value.ToString("dd/MM/yyyy HH:mm"),
                     this.FECHA_HASTA.Value.ToString("dd/MM/yyyy HH:mm"), this.DESC_TIPO_PRODUCTO, this.DESC_BODEGA
                    );

                return true;
            }

  
        }
        #endregion


        #region "Mantenimiento de Turnos"

        private Int64? Save_Update_Mante(out string OnError)
        {

            parametros.Clear();   
            parametros.Add("CONCEPTO", this.CONCEPTO);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            parametros.Add("xmlTurnos", this.xmlTurnos);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[MANTENIMIENTO_TURNOS]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Update_Mante(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Update_Mante(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al actualizar capacidad turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Update_Mante), "SaveTransaction_Update_Mante", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

    }
}
