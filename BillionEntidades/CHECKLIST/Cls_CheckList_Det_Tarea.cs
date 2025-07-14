using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_CheckList_Det_Tarea : Cls_Bil_Base
    {

        #region "Variables"


        private Int32 _SECUENCIA;
        private Int64 _ID_CHECKLIST;
        private Int64 _ID_TAREA;
        private string _TAREA;
        private Int64 _ID_TIPO_EQUIPO;
        private string _NOMBRE_TIPO_EQUIPO;
      

     
        private bool _SELECCION = false;
        private string _USUARIO_CREA = string.Empty;
        private DateTime? _FECHA_CREA = null;
        private Int64 _ID_NOVEDAD;
        private string _NOVEDAD = string.Empty;
        private string _COMENTARIO = string.Empty;
 
        #endregion

        #region "Propiedades"
        public Int32 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public Int64 ID_CHECKLIST { get => _ID_CHECKLIST; set => _ID_CHECKLIST = value; }
        public Int64 ID_TAREA { get => _ID_TAREA; set => _ID_TAREA = value; }
        public string TAREA { get => _TAREA; set => _TAREA = value; }
        public Int64 ID_TIPO_EQUIPO { get => _ID_TIPO_EQUIPO; set => _ID_TIPO_EQUIPO = value; }
        public string NOMBRE_TIPO_EQUIPO { get => _NOMBRE_TIPO_EQUIPO; set => _NOMBRE_TIPO_EQUIPO = value; }
       

        public bool SELECCION { get => _SELECCION; set => _SELECCION = value; }


        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }

        public Int64 ID_NOVEDAD { get => _ID_NOVEDAD; set => _ID_NOVEDAD = value; }
        public string NOVEDAD { get => _NOVEDAD; set => _NOVEDAD = value; }
        public string COMENTARIO { get => _COMENTARIO; set => _COMENTARIO = value; }

        #endregion

        public Cls_CheckList_Det_Tarea()
        {
            init();
        }


        
        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();

            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }



        public static List<Cls_CheckList_Det_Tarea> Lista_Tareas(Int64 ID_TIPO_EQUIPO, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("ID_TIPO_EQUIPO", ID_TIPO_EQUIPO);
            return sql_puntero.ExecuteSelectControl<Cls_CheckList_Det_Tarea>(nueva_conexion, 6000, "listado_tareas", parametros, out OnError);

        }


        private int? PreValidations(out string msg)
        {

            if (this.ID_TAREA <= 0)
            {
                msg = "Especifique el id de la tarea";
                return 0;
            }

          

            msg = string.Empty;
            return 1;
        }

        public Int64? Save(out string OnError)
        {

            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }



            parametros.Clear();
            parametros.Add("ID_CHECKLIST", this.ID_CHECKLIST);
            parametros.Add("ID_TAREA", this.ID_TAREA);
            parametros.Add("ID_TIPO_EQUIPO", this.ID_TIPO_EQUIPO);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("SELECCION", this.SELECCION);
            parametros.Add("ID_NOVEDAD", this.ID_NOVEDAD);
            parametros.Add("NOVEDAD", this.NOVEDAD);
            parametros.Add("COMENTARIO", this.COMENTARIO);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
          

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "checklist_inserta_det_tareas", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


    }
}
