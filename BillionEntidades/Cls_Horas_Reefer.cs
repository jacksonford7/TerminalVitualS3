using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Horas_Reefer : Cls_Bil_Base
    {

        #region "Variables"

  
        private string _linea = string.Empty;
        private string _referencia = string.Empty;
        private string _trafico = string.Empty;
        private string _id = string.Empty;
        private double _horas = 0;
        private Int64 _gkey = 0;
        private bool _valido = false;
        private string _novedad = string.Empty;
        private float _horas2 = 0;
        #endregion

        #region "Propiedades"

        public string linea { get => _linea; set => _linea = value; }
        public string referencia { get => _referencia; set => _referencia = value; }
        public string trafico { get => _trafico; set => _trafico = value; }
        public string id { get => _id; set => _id = value; }
        public double horas { get => _horas; set => _horas = value; }
        public Int64 gkey { get => _gkey; set => _gkey = value; }
        public bool valido { get => _valido; set => _valido = value; }
        public string novedad { get => _novedad; set => _novedad = value; }
        public float horas2 { get => _horas2; set => _horas2 = value; }
        #endregion

        public Cls_Horas_Reefer()
        {
            init();
        }

        public Cls_Horas_Reefer( string _linea ,string _referencia,string _trafico ,string _id , double _horas ,Int64 _gkey , bool _valido,string _novedad, float _horas2)

        {
            this.linea = _linea;
            this.referencia = _referencia;
            this.trafico = _trafico;
            this.id = _id;
            this.horas = _horas;
            this.gkey = _gkey;
            this.horas2 = _horas2;
            this.valido = _valido;
            this.novedad = _novedad;
         
        }



    }
}
