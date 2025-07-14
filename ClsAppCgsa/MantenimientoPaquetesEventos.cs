using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace ClsAppCgsa
{
    public class MantenimientoPaquetesEventos : Base
    {
        #region "Variables"

       
        private Int64? _PackageId;
        private static Int64? lm = -3;
        #endregion

        #region "Propiedades"

        public Int64? PackageId { get => _PackageId; set => _PackageId = value; }
      
        #endregion

        public List<DetalleEventos> Detalle { get; set; }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("APPCGSA");
        }

        public MantenimientoPaquetesEventos()
        {
            init();
            this.Detalle = new List<DetalleEventos>();

        }

        private int? PreValidationsTransaction(out string msg)
        {

            if (!this.PackageId.HasValue)
            {

                msg = "Ingrese el paque a relacionar los eventos";
                return 0;
            }


            //cuenta solo los activos
            var nRegistros = this.Detalle.Where(d => d.Check).Count();

            if (this.Detalle == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de la transaccion, seleccione los eventos";
                return 0;
            }


           
            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
            //OnInit();

            parametros.Clear();
          
            parametros.Add("PackageId", this.PackageId);
            
            var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 4000, "APC_ELIMINA_PAQUETESEVENTOS", parametros, out OnError);
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
            Int64 ID = 0;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //elimina transaccion en caso de existir
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    
                    ID = id.Value;
                    var nContador = 1;

                   
                    foreach (var i in this.Detalle.Where(p=> p.Check==true))
                    {
                       
                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            OnError = "*** Error: al grabar detalle de asignación de eventos ****";
                            return 0;
                        }

                        nContador = nContador + 1;
                        ID = IdRetorno.Value;
                    }

                    nContador = 1;
                    
                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));


                return null;
            }
        }
    }
}
