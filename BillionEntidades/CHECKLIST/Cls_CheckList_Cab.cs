using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_CheckList_Cab : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_CHECKLIST;
        private Int64 _ID_TIPO_EQUIPO;
        private Int64 _ID_EQUIPO;

        private string _OPERADOR = string.Empty;
        private DateTime? _FECHA = null;
        private Int64 _ID_TURNO;
        private bool _ESTADO ;

        private string _USUARIO_CREA = string.Empty;
        private DateTime? _FECHA_CREA;

     

        #endregion

        #region "Propiedades"

        public Int64 ID_CHECKLIST { get => _ID_CHECKLIST; set => _ID_CHECKLIST = value; }
        public Int64 ID_TIPO_EQUIPO { get => _ID_TIPO_EQUIPO; set => _ID_TIPO_EQUIPO = value; }
        public Int64 ID_EQUIPO { get => _ID_EQUIPO; set => _ID_EQUIPO = value; }

        public string OPERADOR { get => _OPERADOR; set => _OPERADOR = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
        public Int64 ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }

        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }

     
        private static String v_mensaje = string.Empty;

      
        #endregion



        public List<Cls_CheckList_Det> Detalle { get; set; }
        public List<Cls_CheckList_Det_Tarea> Detalle_Tareas { get; set; }

        public Cls_CheckList_Cab()
        {
            init();

            this.Detalle = new List<Cls_CheckList_Det>();
            this.Detalle_Tareas = new List<Cls_CheckList_Det_Tarea>();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();

            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }


        public Cls_CheckList_Cab(   Int64 _ID_CHECKLIST,
         Int64 _ID_TIPO_EQUIPO,
         Int64 _ID_EQUIPO,
         string _OPERADOR,
         DateTime? _FECHA,
         Int64 _ID_TURNO,
         bool _ESTADO,
         string _USUARIO_CREA ,
         DateTime? _FECHA_CREA)

        {
            this.ID_CHECKLIST = _ID_CHECKLIST;
            this.ID_TIPO_EQUIPO = _ID_TIPO_EQUIPO;
            this.ID_EQUIPO = _ID_EQUIPO;
            this.OPERADOR = _OPERADOR;
            this.FECHA = _FECHA;
            this.ID_TURNO = _ID_TURNO;
            this.ESTADO = _ESTADO;
            this.USUARIO_CREA = _USUARIO_CREA;
            this.FECHA_CREA = _FECHA_CREA;
          
            this.Detalle = new List<Cls_CheckList_Det>();
          
        }

        private int? PreValidationsTransaction(out string msg)
        {

            if (!this.FECHA.HasValue)
            {

                msg = "La fecha de la transacción no es valida";
                return 0;
            }



            if (string.IsNullOrEmpty(this.USUARIO_CREA))
            {
                msg = "Debe especificar el usuario que crea la transacción";
                return 0;
            }


            //cuenta solo los activos
            //var nRegistros = this.Detalle_Tareas.Where(d => d.ID_NOVEDAD != 0).Count();

            //if (this.Detalle == null || nRegistros <= 0)
            //{
            //    msg = "No se puede agregar el detalle de la transaccion, debe agregar las tareas";
            //    return 0;
            //}

            msg = string.Empty;
            return 1;
        }


        private Int64? Save(out string OnError)
        {
            //OnInit();

            parametros.Clear();
            parametros.Add("ID_TIPO_EQUIPO", this.ID_TIPO_EQUIPO);
            parametros.Add("ID_EQUIPO", this.ID_EQUIPO);
            parametros.Add("OPERADOR", this.OPERADOR);
            parametros.Add("FECHA", this.FECHA);
            parametros.Add("ID_TURNO", this.ID_TURNO);
            parametros.Add("USUARIO_CREA", (string.IsNullOrEmpty(this.USUARIO_CREA) ? "CGSA" : this.USUARIO_CREA));
          
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "checklist_inserta_cab", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction(out string OnError)
        {

            string resultado_otros = null;
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

                    this.ID_CHECKLIST = id.Value;
                    var nContador = 1;
                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Detalle)
                    {
                        i.ID_CHECKLIST = id.Value;
                        i.USUARIO_CREA = this.USUARIO_CREA;
                        i.IV_FECHA_CREA = this.IV_FECHA_CREA;

                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            return 0;
                        }

                        i.ID_CHECKLIST = IdRetorno.Value;
                        nContador = nContador + 1;
                    }

                    nContador = 1;
                    //graba segundo detalle DE TAREAS
                    foreach (var i in this.Detalle_Tareas)
                    {
                        i.ID_CHECKLIST = id.Value;
                        i.USUARIO_CREA = this.USUARIO_CREA;
                        i.IV_FECHA_CREA = this.IV_FECHA_CREA;
                        i.SECUENCIA = nContador;
                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            return 0;
                        }

                        i.ID_CHECKLIST = IdRetorno.Value;
                        nContador = nContador + 1;
                    }


                    //fin de la transaccion
                    scope.Complete();



                    return this.ID_CHECKLIST;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }



    }
}
