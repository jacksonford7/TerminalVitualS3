using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace ClsAppCgsa
{
    public class DetalleEventos : Base
    {
        #region "Variables"

        private Int64? _Id = 0;
        private string _Name = string.Empty;
        private bool? _isPhoto = false;
        private Int64? _PackageId;
        private Int64? _EventsId;
        private string _NamePackages = string.Empty;
        private string _IdEventsN4 = string.Empty;
        private string _EventoN4 = string.Empty;
        private bool _Check;
        #endregion

        #region "Propiedades"
        public Int64? Id { get => _Id; set => _Id = value; }
        public string Name { get => _Name; set => _Name = value; }
        public bool? isPhoto { get => _isPhoto; set => _isPhoto = value; }
        public Int64? PackageId { get => _PackageId; set => _PackageId = value; }
        public Int64? EventsId { get => _EventsId; set => _EventsId = value; }
        public string NamePackages { get => _NamePackages; set => _NamePackages = value; }
        public string IdEventsN4 { get => _IdEventsN4; set => _IdEventsN4 = value; }
        public string EventoN4 { get => _EventoN4; set => _EventoN4 = value; }
        public bool Check { get => _Check; set => _Check = value; }
        #endregion


        public DetalleEventos()
        {
            init();
        }

        public DetalleEventos(Int64? _Id , string _Name ,bool? _isPhoto, Int64? _PackageId,
         Int64? _EventsId, string _NamePackages, string _IdEventsN4 , string _EventoN4 ,  bool _Check)
        {
            this.Id = _Id;
            this.Name = _Name;
            this.isPhoto = _isPhoto;
            this.PackageId = _PackageId;
            this.EventsId = _EventsId;
            this.NamePackages = _NamePackages;
            this.IdEventsN4 = _IdEventsN4;
            this.EventoN4 = _EventoN4;
            this.Check = _Check;
            
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("APPCGSA");
        }

        private int? PreValidations(out string msg)
        {

            if (!this.EventsId.HasValue)
            {
                msg = "Especifique el id del evento";
                return 0;
            }

            if (!this.PackageId.HasValue)
            {
                msg = "Especifique el id del paqueta a grabar";
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
            parametros.Add("Id", this.Id);
            parametros.Add("PackageId", this.PackageId);
            parametros.Add("EventsId", this.EventsId);
            parametros.Add("Create_user", this.Create_user);
           

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "APC_MANTENIMIENTO_PAQUETESEVENTOS", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }
    }
}
