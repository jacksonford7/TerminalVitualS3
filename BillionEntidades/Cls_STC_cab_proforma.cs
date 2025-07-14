using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_STC_cab_proforma : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _id_proforma;
        private string _numero_carga = string.Empty;
        private DateTime? _fecha = null;
        private string _id_consignatario = string.Empty;
        private string _consignatario = string.Empty;
        private string _buque = string.Empty;
        private string _naviera = string.Empty;
        private DateTime? _fecha_salida = null;
        private decimal? _peso_mani;
        private int? _bultos;
        private string _concepto = string.Empty;
        private string _usuario = string.Empty;
      

        #endregion

        #region "Propiedades"

        public Int64 id_proforma { get => _id_proforma; set => _id_proforma = value; }
        public string numero_carga { get => _numero_carga; set => _numero_carga = value; }
        public DateTime? fecha { get => _fecha; set => _fecha = value; }
        public DateTime? fecha_salida { get => _fecha_salida; set => _fecha_salida = value; }
        public string id_consignatario { get => _id_consignatario; set => _id_consignatario = value; }
        public string consignatario { get => _consignatario; set => _consignatario = value; }
        public string buque { get => _buque; set => _buque = value; }
        public string naviera { get => _naviera; set => _naviera = value; }
        public decimal? peso_mani { get => _peso_mani; set => _peso_mani = value; }
        public int? bultos { get => _bultos; set => _bultos = value; }
        public string concepto { get => _concepto; set => _concepto = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
       

        #endregion

        public List<Cls_STC_Informacion_Carga> Detalle_Contenedores { get; set; }
        public List<Cls_STC_det_proforma> Detalle_Proforma { get; set; }


        public Cls_STC_cab_proforma()
        {
            init();

            this.Detalle_Contenedores = new List<Cls_STC_Informacion_Carga>();
            this.Detalle_Proforma = new List<Cls_STC_det_proforma>();
        }

        public Cls_STC_cab_proforma(  Int64 _id_proforma,string _numero_carga,
         DateTime? _fecha,string _id_consignatario,string _consignatario ,string _buque ,
         string _naviera , DateTime? _fecha_salida ,decimal? _peso_mani,
         int? _bultos, string _concepto, string _usuario )
        {
            this.id_proforma = _id_proforma; 
            this.numero_carga= _numero_carga; 
            this.fecha=_fecha;
            this.fecha_salida= _fecha_salida; 
            this.id_consignatario= _id_consignatario;
            this.consignatario = _consignatario;
            this.buque= _buque; 
            this.naviera = _naviera; 
            this.peso_mani= _peso_mani; 
            this.bultos = _bultos;
            this.concepto = _concepto; 
            this.usuario = _usuario; 

            this.Detalle_Contenedores = new List<Cls_STC_Informacion_Carga>();
            this.Detalle_Proforma = new List<Cls_STC_det_proforma>();
        }


    }
}
