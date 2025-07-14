using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SqlConexion;
using System.Web;

namespace BillionAutenticacion
{
    public class Cls_Token : Cls_Base
    {
        #region "Variables"

     
        private string _Token = string.Empty;
        private static String Mensaje = string.Empty;
        private DateTime? _fecha_creacion;
        private DateTime? _fecha_vigencia;
        private string _Usuario = string.Empty;
        private int? _IdUsuario = 0;
        private Int64? _Id = 0;
        #endregion

        #region "Propiedades"

        public string Token { get => _Token; set => _Token = value; }
        public int? IdUsuario { get => _IdUsuario; set => _IdUsuario = value; }
        public Int64? Id { get => _Id; set => _Id = value; }
        public DateTime? fecha_creacion { get => _fecha_creacion; set => _fecha_creacion = value; }
        public DateTime? fecha_vigencia { get => _fecha_vigencia; set => _fecha_vigencia = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }

        #endregion


        public  Cls_Token()
        {
            init();
        }

        

       /* private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_MASTER");
        }*/


         private static Cls_Token Genera_token(out string OnError)
        {
           // OnInit();

            Cls_Token ClsToken = new Cls_Token();

            ClsToken = sql_puntero.ExecuteSelectOnly<Cls_Token>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Genera_Token", null);
            if (ClsToken == null)
            {
                OnError = "No Se pudo generar token";
                return ClsToken;
            }
         
            OnError = string.Empty;
            return ClsToken;
        }

        private static int? PreValidations(string Token, out string msg)
        {

            if (string.IsNullOrEmpty(Token) )
            {
                msg = "No existe el token";
                return 0;
            }

            msg = string.Empty;
            return 1;

        }

        public static Int64? Save(Cls_Usuario ClsUsuario, string Token, out string OnError)
        {
            if (PreValidations(Token, out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("IdUsuario", ClsUsuario.IdUsuario);
            parametros.Add("Usuario", ClsUsuario.Usuario);
            parametros.Add("Token", Token);
           
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_Inserta_Token", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }


        //siembra el token
        public static Cls_Usuario Tracker(System.Web.UI.Page page)
        {
            
            Cls_Usuario ClsUsuario = new Cls_Usuario();

            if (!page.IsPostBack)
            {
                ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;

                if (ClsUsuario == null)
                {
                    HttpContext.Current.Response.Redirect("~/login.aspx", true);
                    return ClsUsuario;
                }

                //creo la cokie token
                var ck = new HttpCookie("token");
                // la expiro en 45 minutos
                ck.Expires = DateTime.Now.AddMinutes(30);
                //le digo que solo es usable via http
                ck.HttpOnly = true;

                //HABILITAR Y PROBAR VIA HTTP->COKIE
                var desa = System.Configuration.ConfigurationManager.AppSettings["DESARROLLO"];
                if (desa == null || desa.Contains("0"))
                {
                    ck.Secure = true;
                }

                var ClsToken = Genera_token(out Mensaje);
                if (ClsToken !=null)
                {
                    ck.Value = ClsToken.Token;
                    var Grabar = Save(ClsUsuario, ck.Value, out Mensaje);
                    if (Mensaje != string.Empty)
                    {
                        HttpContext.Current.Response.Redirect("~/login.aspx", true);
                        return ClsUsuario;
                    }
                }
 
                HttpContext.Current.Response.Cookies.Add(ck);
                //AÑADO EL TOKEN
                HttpContext.Current.Session["tokenID"] = ck;

                //EXISTE TOKEN
                //bool ExisteToken = false;
                //ExisteToken = ValidateToken(ck.Value);

            }
            else
            {
                ClsUsuario = null;
            }
            return ClsUsuario;

        }


        public static bool ValidateToken(string Token)
        {
            if (Token == null)
            {
                return false;
            }
            
            //OnInit();

            parametros.Clear();
            parametros.Add("token", Token);
            var ClsToken = sql_puntero.ExecuteSelectOnly<Cls_Token>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Validate_Token", parametros);

            if (ClsToken == null)
             {
               return false;
            }
  
            return true;
        }

        /*listado general de token*/
        public static List<Cls_Token> List_Token()
        {
            //OnBillion();
          
            return sql_puntero.ExecuteSelectControl<Cls_Token>(sql_puntero.Conexion_Local, 2000, "sp_Bil_Listado_token", null, out Mensaje);
        }


    }
}
