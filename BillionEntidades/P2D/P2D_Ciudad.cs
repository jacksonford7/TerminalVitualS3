using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Ciudad : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_CIUDAD = 0;
        private string _DESC_CIUDAD = string.Empty;
        private string _USUARIO_CREA = string.Empty;
        private DateTime? _FECHA_CREA;
        private string _ACCION = string.Empty;
        private bool _ESTADO;
        private string _ESTADO_TEXTO;
        #endregion

        #region "Propiedades"
        public Int64 ID_CIUDAD { get => _ID_CIUDAD; set => _ID_CIUDAD = value; }
        public string DESC_CIUDAD { get => _DESC_CIUDAD; set => _DESC_CIUDAD = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public string ACCION { get => _ACCION; set => _ACCION = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string ESTADO_TEXTO { get => _ESTADO_TEXTO; set => _ESTADO_TEXTO = value; }
        #endregion


        public P2D_Ciudad()
        {
            init();
        }

        public static List<P2D_Ciudad> CboCiudad(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<P2D_Ciudad>(sql_puntero.Conexion_Local, 6000, "P2D_CBO_CIUDAD", null, out OnError);
        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.DESC_CIUDAD))
            {
                msg = "Debe especificar la descripción de la ciudad";
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



            parametros.Clear();
            parametros.Add("ID_CIUDAD", this.ID_CIUDAD);
            parametros.Add("DESC_CIUDAD", this.DESC_CIUDAD);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            parametros.Add("ACCION", this.ACCION);


            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "P2D_MANTENIMIENTO_CIUDADES", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }


        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_CIUDAD", this.ID_CIUDAD);

            var t = sql_puntero.ExecuteSelectOnly<P2D_Ciudad>(sql_puntero.Conexion_Local, 6000, "P2D_BUSCA_CIUDAD", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos de la ciudad";
                return false;
            }

            this.ID_CIUDAD = t.ID_CIUDAD;
            this.DESC_CIUDAD = t.DESC_CIUDAD;
            this.ESTADO = t.ESTADO;
            this.USUARIO_CREA = t.USUARIO_CREA;
            this.FECHA_CREA = t.FECHA_CREA;




            OnError = string.Empty;
            return true;
        }

        public bool Delete(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_CIUDAD", this.ID_CIUDAD);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "P2D_ELIMINA_CIUDAD", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public static List<P2D_Ciudad> Listado_Ciudades(out string OnError)
        {


            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<P2D_Ciudad>(sql_puntero.Conexion_Local, 5000, "P2D_LISTA_CIUDADES", null, out OnError);

        }

    }
}
