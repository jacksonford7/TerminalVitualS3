using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Registra_Dae : Cls_Bil_Base
    {
        #region "Variables"

        private Int64? _Id;
        private string _dae;
        private string _ruc;
        private string _empresa;
        private int _qty;
        private string _tipo;
        private string _ruta_pdf;
        private string _usuario;
        private static Int64? lm = -3;

        #endregion

        #region "Propiedades"

        public Int64? Id { get => _Id; set => _Id = value; }
        public string dae { get => _dae; set => _dae = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public string empresa { get => _empresa; set => _empresa = value; }
        public int qty { get => _qty; set => _qty = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        public string ruta_pdf { get => _ruta_pdf; set => _ruta_pdf = value; }
        public string usuario { get => _usuario; set => _usuario = value; }
        #endregion


       
        public Cls_Registra_Dae()
        {
            init();
         

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

     

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.ruc))
            {
                msg = "Debe especificar el ruc";
                return 0;
            }
            if (string.IsNullOrEmpty(this.empresa))
            {
                msg = "Debe especificar el nombre";
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

            OnInit("ADUANA");

            parametros.Clear();
            parametros.Add("dae", this.dae);
            parametros.Add("ruc", this.ruc);
            parametros.Add("empresa", this.empresa);
            parametros.Add("qty", this.qty);
            parametros.Add("tipo", this.tipo);
            parametros.Add("ruta_pdf", this.ruta_pdf);
            parametros.Add("usuario", this.usuario);
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "tv_insertar_nueva_dae_manual", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

       
      
    }
}
