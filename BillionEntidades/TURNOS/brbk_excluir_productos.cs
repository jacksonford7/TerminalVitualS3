using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_excluir_productos : Cls_Bil_Base
    {


        #region "Variables"

        private int _SECUENCIA;
        private Int64 _ID;
        private string _DESCRIPCION;
        private string _USUARIO_CREA;
        private DateTime? _FECHA_CREA;
        private string _USUARIO_MOD;
        private DateTime? _FECHA_MOD;
        private bool _ESTADO;

      
        private static Int64? lm = -3;

        #endregion

        #region "Propiedades"
        public int SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public Int64 ID { get => _ID; set => _ID = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public DateTime? FECHA_MOD { get => _FECHA_MOD; set => _FECHA_MOD = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        
        #endregion



        public brbk_excluir_productos()
        {
            init();


        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.USUARIO_CREA))
            {
                msg = "Debe especificar el usuario";
                return 0;
            }
            if (this.ID == 0)
            {
                msg = "Debe especificar el ID del tipo de producto";
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
            parametros.Add("ID", this.ID);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
            
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[BRBK].[REGISTRA_EXCLUIR_PRODUCTO]", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        public static List<brbk_excluir_productos> brbk_Excluir_Producto(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<brbk_excluir_productos>(sql_puntero.Conexion_Local, 6000, "[BRBK].[LISTADO_EXCLUIR_TIPO_PRODUCTO]", null, out OnError);
        }

        public bool Existe_Producto(out string OnError)
        {

         
            parametros.Clear();
            parametros.Add("ID", this.ID);
        

            var t = sql_puntero.ExecuteSelectOnly<brbk_excluir_productos>(sql_puntero.Conexion_Local, 6000, "[BRBK].[VALIDA_TIPO_PRODUCTO]", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }

            this.ID = t.ID;
            this.DESCRIPCION = t.DESCRIPCION;
           

            OnError = string.Empty;
            return true;
        }

        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID", this.ID);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "[Bill].[brbk_elimina_tipo_producto]", parametros, out OnError);
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
