using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Proforma_Detalle : Cls_Bil_Base
    {


        #region "Variables"

        private Int64 _PF_ID;
        private Int64 _PF_GKEY;
        private string _PF_MRN = string.Empty;
        private string _PF_MSN = string.Empty;
        private string _PF_HSN = string.Empty;
        private string _PF_CONTENEDOR = string.Empty;
        private string _PF_REFERENCIA = string.Empty;
        private string _PF_TRAFICO = string.Empty;
        private string _PF_TAMANO = string.Empty;
        private string _PF_TIPO = string.Empty;
        private DateTime? _PF_CAS = null;
        private string _PF_BOOKING = string.Empty;
        private string _PF_IMDT = string.Empty;
        private bool _PF_BLOQUEO = false;
        private DateTime? _PF_FECHA_ULTIMA = null;
        private string _PF_IN_OUT = string.Empty;
        private string _PF_FULL_VACIO = string.Empty;
        private string _PF_AISV = string.Empty;
        private string _PF_REEFER = string.Empty;
        private bool _PF_VISTO = false;
        private string _PF_DOCUMENTO = string.Empty;
        private string _PF_DES_BLOQUEO = string.Empty;
        private string _PF_CONECTADO = string.Empty;
        private DateTime? _PF_FECHA_HASTA = null;

        private decimal _PF_CANTIDAD = 0;
        private decimal _PF_PESO = 0;
        private string _PF_OPERACION = string.Empty;
        private string _PF_DESCRIPCION = string.Empty;
        private string _PF_EXPORTADOR = string.Empty;
        private string _PF_AGENCIA = string.Empty;
        private string _PF_AUTORIZADO = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 PF_ID { get => _PF_ID; set => _PF_ID = value; }
        public Int64 PF_GKEY { get => _PF_GKEY; set => _PF_GKEY = value; }
        public string PF_MRN { get => _PF_MRN; set => _PF_MRN = value; }
        public string PF_MSN { get => _PF_MSN; set => _PF_MSN = value; }
        public string PF_HSN { get => _PF_HSN; set => _PF_HSN = value; }
        public string PF_CONTENEDOR { get => _PF_CONTENEDOR; set => _PF_CONTENEDOR = value; }
        public string PF_REFERENCIA { get => _PF_REFERENCIA; set => _PF_REFERENCIA = value; }
        public string PF_TRAFICO { get => _PF_TRAFICO; set => _PF_TRAFICO = value; }
        public string PF_TAMANO { get => _PF_TAMANO; set => _PF_TAMANO = value; }
        public string PF_TIPO { get => _PF_TIPO; set => _PF_TIPO = value; }
        public DateTime? PF_CAS { get => _PF_CAS; set => _PF_CAS = value; }
        public string PF_BOOKING { get => _PF_BOOKING; set => _PF_BOOKING = value; }
        public string PF_IMDT { get => _PF_IMDT; set => _PF_IMDT = value; }
        public bool PF_BLOQUEO { get => _PF_BLOQUEO; set => _PF_BLOQUEO = value; }
        public DateTime? PF_FECHA_ULTIMA { get => _PF_FECHA_ULTIMA; set => _PF_FECHA_ULTIMA = value; }
        public string PF_IN_OUT { get => _PF_IN_OUT; set => _PF_IN_OUT = value; }
        public string PF_FULL_VACIO { get => _PF_FULL_VACIO; set => _PF_FULL_VACIO = value; }
        public string PF_AISV { get => _PF_AISV; set => _PF_AISV = value; }
        public string PF_REEFER { get => _PF_REEFER; set => _PF_REEFER = value; }
        public bool PF_VISTO { get => _PF_VISTO; set => _PF_VISTO = value; }
        public string PF_DOCUMENTO { get => _PF_DOCUMENTO; set => _PF_DOCUMENTO = value; }
        public string PF_DES_BLOQUEO { get => _PF_DES_BLOQUEO; set => _PF_DES_BLOQUEO = value; }
        public string PF_CONECTADO { get => _PF_CONECTADO; set => _PF_CONECTADO = value; }
        public DateTime? PF_FECHA_HASTA { get => _PF_FECHA_HASTA; set => _PF_FECHA_HASTA = value; }

        public decimal PF_CANTIDAD { get => _PF_CANTIDAD; set => _PF_CANTIDAD = value; }
        public decimal PF_PESO { get => _PF_PESO; set => _PF_PESO = value; }
        public string PF_OPERACION { get => _PF_OPERACION; set => _PF_OPERACION = value; }
        public string PF_DESCRIPCION { get => _PF_DESCRIPCION; set => _PF_DESCRIPCION = value; }
        public string PF_EXPORTADOR { get => _PF_EXPORTADOR; set => _PF_EXPORTADOR = value; }
        public string PF_AGENCIA { get => _PF_AGENCIA; set => _PF_AGENCIA = value; }
        public string PF_AUTORIZADO { get => _PF_AUTORIZADO; set => _PF_AUTORIZADO = value; }

        #endregion

        public Cls_Bil_Proforma_Detalle()
        {
            init();
        }


        public Cls_Bil_Proforma_Detalle(Int64 _PF_ID, Int64 _PF_GKEY, string _PF_MRN, string _PF_MSN, string _PF_HSN, string _PF_CONTENEDOR,
        string _PF_REFERENCIA, string _PF_TRAFICO, string _PF_TAMANO, string _PF_TIPO, DateTime? _PF_CAS,
        string _PF_BOOKING, string _PF_IMDT, bool _PF_BLOQUEO, DateTime? _PF_FECHA_ULTIMA, string _PF_IN_OUT, string _PF_FULL_VACIO,
        string _PF_AISV, string _PF_REEFER, bool _PF_VISTO, string _PF_DOCUMENTO, string _PF_DES_BLOQUEO, string _PF_CONECTADO, DateTime? _PF_FECHA_HASTA)
        {
            this.PF_ID = _PF_ID;
            this.PF_GKEY = _PF_GKEY;
            this.PF_MRN = _PF_MRN;
            this.PF_MSN = _PF_MSN;
            this.PF_HSN = _PF_HSN;

            this.PF_CONTENEDOR = _PF_CONTENEDOR;
            this.PF_TRAFICO = _PF_TRAFICO;

            this.PF_TAMANO = _PF_TAMANO;
            this.PF_TIPO = _PF_TIPO;
            this.PF_CAS = _PF_CAS;
            this.PF_BOOKING = _PF_BOOKING;

            this.PF_IMDT = _PF_IMDT;
            this.PF_BLOQUEO = _PF_BLOQUEO;
            this.PF_FECHA_ULTIMA = _PF_FECHA_ULTIMA;
            this.PF_IN_OUT = _PF_IN_OUT;
            this.PF_FULL_VACIO = _PF_FULL_VACIO;
            this.PF_AISV = _PF_AISV;
            this.PF_REEFER = _PF_REEFER;
            this.PF_DOCUMENTO = _PF_DOCUMENTO;
            this.PF_DES_BLOQUEO = _PF_DES_BLOQUEO;
            this.PF_CONECTADO = _PF_CONECTADO;
            this.PF_FECHA_HASTA = _PF_FECHA_HASTA;
        }

        private int? PreValidations(out string msg)
        {

            if (this.PF_GKEY <= 0)
            {
                msg = "Especifique el id del contenedor o la carga";
                return 0;
            }

            if (this.PF_ID <= 0)
            {
                msg = "Especifique el ID de la cabecera de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.PF_MRN))
            {
                msg = "Especifique el MRN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.PF_MSN))
            {
                msg = "Especifique el MSN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.PF_HSN))
            {
                msg = "Especifique el HSN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_USUARIO_CREA))
            {
                msg = "Especifique el usuario que crea la transacción";
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
            //MM-dd-yyyy
            if (this.PF_CAS==null)
            {
                this.PF_CAS = DateTime.Parse("01/01/1900");
            }
            if (this.PF_FECHA_ULTIMA == null)
            {
                this.PF_FECHA_ULTIMA = DateTime.Parse("01/01/1900");
            }
            parametros.Clear();
            parametros.Add("PF_ID", this.PF_ID);
            parametros.Add("PF_MRN", this.PF_MRN);
            parametros.Add("PF_MSN", this.PF_MSN);
            parametros.Add("PF_HSN", this.PF_HSN);
            parametros.Add("PF_CONTENEDOR", this.PF_CONTENEDOR);
            parametros.Add("PF_GKEY", this.PF_GKEY);
            parametros.Add("PF_REFERENCIA", this.PF_REFERENCIA);
            parametros.Add("PF_TRAFICO", this.PF_TRAFICO);
            parametros.Add("PF_TAMANO", this.PF_TAMANO);
            parametros.Add("PF_TIPO", this.PF_TIPO);
            parametros.Add("PF_CAS", this.PF_CAS);
            parametros.Add("PF_BOOKING", this.PF_BOOKING);
            parametros.Add("PF_IMDT", this.PF_IMDT);
            parametros.Add("PF_BLOQUEO", this.PF_BLOQUEO);
            parametros.Add("PF_FECHA_ULTIMA", this.PF_FECHA_ULTIMA);
            parametros.Add("PF_IN_OUT", this.PF_IN_OUT);
            parametros.Add("PF_FULL_VACIO", this.PF_FULL_VACIO);
            parametros.Add("PF_AISV", this.PF_AISV);
            parametros.Add("PF_REEFER", this.PF_REEFER);
            parametros.Add("PF_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("PF_FECHA_CREA", this.IV_FECHA_CREA);
            parametros.Add("PF_FECHA_HASTA", this.PF_FECHA_HASTA);

            parametros.Add("PF_CANTIDAD", this.PF_CANTIDAD);
            parametros.Add("PF_PESO", this.PF_PESO);
            parametros.Add("PF_OPERACION", this.PF_OPERACION);
            parametros.Add("PF_DESCRIPCION", this.PF_DESCRIPCION);
            parametros.Add("PF_EXPORTADOR", this.PF_EXPORTADOR);
            parametros.Add("PF_AGENCIA", this.PF_AGENCIA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_inserta_proforma_det", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


    }
}
