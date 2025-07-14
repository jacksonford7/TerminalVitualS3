using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Bil_DetalleMsc : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _id = 0;
        private int _fila = 0;
        private string _contenedor = string.Empty;
        private DateTime _fecha_despacho;
        private bool _estado;
        private DateTime _fecha;
        private string _linea = string.Empty;
        private string _bl = string.Empty;
        private string _cliente = string.Empty;
        private string _tipo = string.Empty;
        private string _nave = string.Empty;
        private string _viaje = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64 id { get => _id; set => _id = value; }
        public int fila { get => _fila; set => _fila = value; }
        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public DateTime fecha_despacho { get => _fecha_despacho; set => _fecha_despacho = value; }
      
        public bool estado { get => _estado; set => _estado = value; }
        public DateTime fecha { get => _fecha; set => _fecha = value; }
        public string linea { get => _linea; set => _linea = value; }

        public string bl { get => _bl; set => _bl = value; }
        public string cliente { get => _cliente; set => _cliente = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        public string nave { get => _nave; set => _nave = value; }
        public string viaje { get => _viaje; set => _viaje = value; }

        #endregion

        public Cls_Bil_DetalleMsc()
        {
            init();
        }

        public Cls_Bil_DetalleMsc(  Int64 _id,
         int _fila ,
         string _contenedor,
         DateTime _fecha_despacho,
         bool _estado,
         DateTime _fecha,
         string _linea
         )

        {
            this.id = _id;
            this.fila = _fila;
            this.contenedor = _contenedor;
            this.fecha_despacho = _fecha_despacho;
            this.estado = _estado;
            this.fecha = _fecha;
            this.linea = _linea;

        }



    }
}
