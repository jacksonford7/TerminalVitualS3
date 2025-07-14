using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Libera_Pases_Zal : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _id_ppzal = 0;
        private string _booking = string.Empty;
        private string _referencia = string.Empty;
        private string _fecha_salida = string.Empty;
        private string _turno = string.Empty;
        private string _placa = string.Empty;
        private string _chofer = string.Empty;
        private string _liquidacion = string.Empty;
        private string _estado_pago = string.Empty;
        private string _deposito = string.Empty;
        private decimal _valor = 0;
        private string _comentario = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 id_ppzal { get => _id_ppzal; set => _id_ppzal = value; }
        public string booking { get => _booking; set => _booking = value; }
        public string referencia { get => _referencia; set => _referencia = value; }
        public string fecha_salida { get => _fecha_salida; set => _fecha_salida = value; }
        public string turno { get => _turno; set => _turno = value; }
        public string placa { get => _placa; set => _placa = value; }
        public string chofer { get => _chofer; set => _chofer = value; }
        public string liquidacion { get => _liquidacion; set => _liquidacion = value; }
        public string estado_pago { get => _estado_pago; set => _estado_pago = value; }
        public string deposito { get => _deposito; set => _deposito = value; }
        public decimal valor { get => _valor; set => _valor = value; }
        public string comentario { get => _comentario; set => _comentario = value; }


        #endregion

        public Cls_Bil_Libera_Pases_Zal()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }

        /*lista pase puerta zal a liberar*/
        public static List<Cls_Bil_Libera_Pases_Zal> Listado_Pagos_Terceros(Int64? ID_PPZAL, string liquidacion,  out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("ID_PPZAL", ID_PPZAL);
            parametros.Add("liquidacion", liquidacion);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Libera_Pases_Zal>(nueva_conexion, 6000, "SP_GET_PASE_ZAL_LIBERAR", parametros, out OnError);

        }

        public bool Update(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ID_PPZAL", this.id_ppzal);
            parametros.Add("USR_MOD", this.IV_USUARIO_CREA);
            parametros.Add("COMENTARIO", this.comentario);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "SP_ACTUALIZA_PASE_ZAL_LIBERAR", parametros, out OnError);
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
