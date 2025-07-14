using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAisvSav
{
    public class SaldoProforma : Base
    {
        #region "Variables"

        private int _id_preaviso = 0;
        private Int64 _idproforma = 0;
        private string _ruc = string.Empty;
        private string _liquidacion = string.Empty;
        private decimal _total = 0;
        private decimal _valorcruzado =0;
        private decimal _valorsaldo = 0;
        private string _usuario = string.Empty;

        private string _leyenda = string.Empty;
        private decimal _saldo_final = 0;
        #endregion

        #region "Propiedades"
        public int id_preaviso { get => _id_preaviso; set => _id_preaviso = value; }
        public Int64 idproforma { get => _idproforma; set => _idproforma = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public string liquidacion { get => _liquidacion; set => _liquidacion = value; }
        public decimal total { get => _total; set => _total = value; }
        public decimal valorcruzado { get => _valorcruzado; set => _valorcruzado = value; }
        public decimal valorsaldo { get => _valorsaldo; set => _valorsaldo = value; }
        public string usuario { get => _usuario; set => _usuario = value; }

        public string leyenda { get => _leyenda; set => _leyenda = value; }
        public decimal saldo_final { get => _saldo_final; set => _saldo_final = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public SaldoProforma()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("SERVICES");
        }

        private int? PreValidations(out string msg)
        {

            if (this.id_preaviso==0)
            {
                msg = "Debe especificar id del preaviso";
                return 0;
            }

            if (this.idproforma==0)
            {
                msg = "Debe especificar el id de la proforma";
                return 0;
            }

            if (this.total == 0)
            {
                msg = "Debe especificar el valor de la proforma";
                return 0;
            }


            if (string.IsNullOrEmpty(this.ruc))
            {
                msg = "Debe especificar el ruc del cliente";
                return 0;
            }
            if (string.IsNullOrEmpty(this.usuario))
            {
                msg = "Especifique el usuario creador del registro";
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

            OnInit();

            parametros.Clear();
            parametros.Add("id_preaviso", this.id_preaviso);
            parametros.Add("ruc", this.ruc);
            parametros.Add("liquidacion", this.liquidacion);
            parametros.Add("idproforma", this.idproforma);
            parametros.Add("total", this.total);
            parametros.Add("valorcruzado", this.valorcruzado);
            parametros.Add("valorsaldo", this.valorsaldo);
            parametros.Add("usuario", this.usuario);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "far_graba_saldo_proforma", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<SaldoProforma> Get_Saldo(string _id_ruc)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_ruc", _id_ruc);
            return sql_pointer.ExecuteSelectControl<SaldoProforma>(v_conexion, 6000, "sav_saldo_cliente", parametros, out msg);
        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<SaldoProforma> Get_Saldo_Repcontver(string _id_ruc, Int64 deposito_id)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_ruc", _id_ruc);
            parametros.Add("deposito_id", deposito_id);
            return sql_pointer.ExecuteSelectControl<SaldoProforma>(v_conexion, 6000, "sav_saldo_cliente_repcontver", parametros, out msg);
        }

        public static string Actualiza_Saldo(Int64 _id)
        {
            OnInit();
            string OnError;
            parametros.Clear();
            parametros.Add("id", _id);

            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 6000, "sav_actualiza_saldo", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al actualizar saldo de pase: {0}", _id);
                return null;
            }
            else
            {
                if (db.code == 1)
                {
                    OnError = db.message;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<SaldoProforma> Lista_Saldo(Int64 id, out string mensaje)
        {
            OnInit();
           // string msg;
            parametros.Clear();
            parametros.Add("id", id);
            return sql_pointer.ExecuteSelectControl<SaldoProforma>(v_conexion, 6000, "sav_busca_saldo", parametros, out mensaje);
        }

        public bool Reversar(Int64 id, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("id", id); 
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "sav_reversa_saldo_proforma", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

    }
}
