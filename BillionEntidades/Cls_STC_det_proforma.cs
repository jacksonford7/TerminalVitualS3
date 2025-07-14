using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_STC_det_proforma : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _id_proforma;
        private int _fila;
        private string _sere_codigo = string.Empty;
        private string _sere_descripcion = string.Empty;

        private decimal? _cantidad;
        private decimal? _costo;
        private decimal? _subtotal;


        #endregion

        #region "Propiedades"

        public Int64 id_proforma { get => _id_proforma; set => _id_proforma = value; }
        public int fila { get => _fila; set => _fila = value; }
        public string sere_codigo { get => _sere_codigo; set => _sere_codigo = value; }
        public string sere_descripcion { get => _sere_descripcion; set => _sere_descripcion = value; }
       
        public decimal? cantidad { get => _cantidad; set => _cantidad = value; }
        public decimal? costo { get => _costo; set => _costo = value; }
        public decimal? subtotal { get => _subtotal; set => _subtotal = value; }



        #endregion

        public Cls_STC_det_proforma()
        {
            init();
        }

        public Cls_STC_det_proforma( Int64 _id_proforma,
         int _fila,
         string _sere_codigo ,
         string _sere_descripcion,
         decimal? _cantidad,
         decimal? _costo,
         decimal? _subtotal)
        {
            this.id_proforma = _id_proforma;
            this.fila = _fila;
            this.sere_codigo = _sere_codigo;
            this.sere_descripcion = _sere_descripcion;
            this.cantidad = _cantidad;
            this.costo = _costo;
            this.subtotal = _subtotal;
          
        }


    }
}
