using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_CheckList_Det : Cls_Bil_Base
    {

        #region "Variables"

        private Int32 _SECUENCIA;
        private Int64 _ID_CHECKLIST;
        private Int64 _ID_TIPO_EQUIPO;
        private string _NOMBRE_TIPO_EQUIPO;
      
        private Int64 _ID_EQUIPO;
        private string _NOMBRE_EQUIPO;
        private string _OPERADOR = string.Empty;
        private DateTime? _FECHA = null;
        private Int64 _ID_TURNO;
        private bool _ESTADO = false;
        private string _USUARIO_CREA = string.Empty;
        private DateTime? _FECHA_CREA = null;
        private Int64 _ID_NOVEDAD;
        private string _NOVEDAD = string.Empty;
        private string _MOTIVO = string.Empty;
        #endregion

        #region "Propiedades"
        public Int32 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public Int64 ID_CHECKLIST { get => _ID_CHECKLIST; set => _ID_CHECKLIST = value; }
        public Int64 ID_TIPO_EQUIPO { get => _ID_TIPO_EQUIPO; set => _ID_TIPO_EQUIPO = value; }
        public string NOMBRE_TIPO_EQUIPO { get => _NOMBRE_TIPO_EQUIPO; set => _NOMBRE_TIPO_EQUIPO = value; }
        public Int64 ID_EQUIPO { get => _ID_EQUIPO; set => _ID_EQUIPO = value; }
        public string NOMBRE_EQUIPO { get => _NOMBRE_EQUIPO; set => _NOMBRE_EQUIPO = value; }
        public string OPERADOR { get => _OPERADOR; set => _OPERADOR = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
        public Int64 ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }


        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }

        public Int64 ID_NOVEDAD { get => _ID_NOVEDAD; set => _ID_NOVEDAD = value; }
        public string NOVEDAD { get => _NOVEDAD; set => _NOVEDAD = value; }
        public string MOTIVO { get => _MOTIVO; set => _MOTIVO = value; }

        #endregion

        public Cls_CheckList_Det()
        {
            init();
        }


        public Cls_CheckList_Det(Int64 _ID_CHECKLIST, Int64 _ID_TIPO_EQUIPO, Int64 _ID_EQUIPO,
         string _OPERADOR , DateTime? _FECHA ,  Int64 _ID_TURNO, bool _ESTADO, string _USUARIO_CREA , DateTime? _FECHA_CREA,
          Int64 _ID_NOVEDAD, string _NOVEDAD ,   string _MOTIVO )
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

            this.ID_NOVEDAD = _ID_NOVEDAD;
            this.NOVEDAD = _NOVEDAD;
            this.MOTIVO = _MOTIVO;

        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();

            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }

     


        private int? PreValidations(out string msg)
        {

            if (this.ID_NOVEDAD <= 0)
            {
                msg = "Especifique el id de la novedad";
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

           // OnInit();

            parametros.Clear();
            parametros.Add("ID_CHECKLIST", this.ID_CHECKLIST);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("ID_NOVEDAD", this.ID_NOVEDAD);
            parametros.Add("NOVEDAD", this.NOVEDAD);
            parametros.Add("MOTIVO", this.MOTIVO);
            parametros.Add("USUARIO_CREA", this.@USUARIO_CREA);
          

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "checklist_inserta_det", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


    }
}
