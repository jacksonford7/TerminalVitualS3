using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_Bil_Stock_Pases_CFS : Cls_Bil_Base
    {
        #region "Variables"

        private int _TotalBultos =0;
        private Int64 _IdPlan = 0;
        private DateTime _Inicio;
        private DateTime _Fin;
        private int _Bultos = 0;

        private string _Turno = string.Empty;
        private int _Secuencia = 0;
        private int _TOTAL = 0;

        private Int64 _IDDISPONIBLEDET = 0;
        private DateTime _FECHA;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _BODEGA = string.Empty;
        private string _USUARIOING = string.Empty;
        private string _subitems = string.Empty;
        #endregion

        #region "Propiedades"

        public int TotalBultos { get => _TotalBultos; set => _TotalBultos = value; }
        public Int64 IdPlan { get => _IdPlan; set => _IdPlan = value; }
        public DateTime Inicio { get => _Inicio; set => _Inicio = value; }
        public DateTime Fin { get => _Fin; set => _Fin = value; }


        public int Bultos { get => _Bultos; set => _Bultos = value; }
        public string Turno { get => _Turno; set => _Turno = value; }
        public int Secuencia { get => _Secuencia; set => _Secuencia = value; }
        public int TOTAL { get => _TOTAL; set => _TOTAL = value; }
        private static String v_mensaje = string.Empty;

        public Int64 IDDISPONIBLEDET { get => _IDDISPONIBLEDET; set => _IDDISPONIBLEDET = value; }
        public DateTime FECHA { get => _FECHA; set => _FECHA = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string BODEGA { get => _BODEGA; set => _BODEGA = value; }
        public string USUARIOING { get => _USUARIOING; set => _USUARIOING = value; }
        public string subitems { get => _subitems; set => _subitems = value; }
        #endregion

        public Cls_Bil_Stock_Pases_CFS()
        {
            init();

        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }


        /*carga todas las facturas*/
        public static List<Cls_Bil_Stock_Pases_CFS> Carga_Stock_Turno(DateTime fecha, string subitems, Int64 IDDISPONIBLEDET, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("fecha", fecha);
            parametros.Add("subitems", subitems);
            parametros.Add("IDDISPONIBLEDET", IDDISPONIBLEDET);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Stock_Pases_CFS>(nueva_conexion, 6000, "[Bill].[cfs_turnos_disponibles_unico]", parametros, out OnError);

        }

        /*carga todas las facturas*/
        public static List<Cls_Bil_Stock_Pases_CFS> Carga_Cantidad_Bultos_Pase(string subitems, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("subitems", subitems);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Stock_Pases_CFS>(nueva_conexion, 6000, "[Bill].[cantidad_bultos_cfs]", parametros, out OnError);

        }

        private int? PreValidationsTransaction(out string msg)
        {

            if (this.IDDISPONIBLEDET == 0)
            {

                msg = "Debe seleccionar un plan/turno";
                return 0;
            }

            if (this.Bultos == 0)
            {

                msg = "No tiene bultos disponibles para ingresar";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }


        private Int64? Save(out string OnError)
        {
           // OnInit();

            parametros.Clear();
            parametros.Add("IDDISPONIBLEDET", this.IDDISPONIBLEDET);
            parametros.Add("BULTOS", this.Bultos);
            parametros.Add("FECHA", this.FECHA);
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("BODEGA", this.BODEGA);
            parametros.Add("subitems", this.subitems);
            parametros.Add("USUARIOING", this.USUARIOING);
            
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[Bill].[nuevo_turno_temporal_cfs]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction(out string OnError)
        {

            string resultado_otros = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al grabar detalle de reserva de turnos temporales ****";
                        return 0;
                    }

                    //fin de la transaccion
                    scope.Complete();

                    return id;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }

    }
}
