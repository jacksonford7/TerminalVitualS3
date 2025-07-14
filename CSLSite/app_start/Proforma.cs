using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using csl_log;

namespace CSLSite.app_start
{
    public class Proforma
    {
        public string Secuencia { get; set; } //pedir
        public int IdCorporacion { get; set; }
        public int IdEmpresa { get; set; }
        public int IdServicio { get; set; }
        public int IdZona { get; set; }
        public int IdGrupo { get; set; } //pedir
        public string Email { get; set; }
        public int IdUsuario { get; set; } //pedir
        public string UsuarioIngreso { get; set; } //pedir
        public string Actividad { get; set; }
        public decimal SubTotal { get; set; } //pedir
        public decimal IVA { get; set; } //pedir
        public decimal IvaMas { get; set; } //pedir
       
        public decimal ReteIVA { get; set; } //pedir
        public decimal ReteFuente { get; set; } //pedir

        public decimal PctIVA { get; set; } //pedir
        public decimal PctFuente { get; set; } //pedir

        
        public decimal Total { get; set; } //pedir
        public string RUC { get; set; } //pedir
        public string Token { get; set; } //pedir
        public string Nave { get; set; } //pedir
        public DateTime FechaSalida { get; set; }
        public bool Estado { get; set; } 
        public Int64 Gkey { get; set; } //pedir
        public Int32 Cantidad  { get; set; }
        public string Bokingnbr { get; set; } //pedir
        public string Referencia { get; set; } //pedir
        public Int32 Reserva { get; set; }
        //Nuevo 2017------------------------>
        public string Liquidacion { get; set; }
        //----------------------------------
        public DateTime Etd { get; set; }
        public DateTime FechaCliente { get; set; }
        public DateTime Cutoff { get; set; }
        public bool Reefer { get; set; }
        public string Size { get; set; } //pedir

        public Proforma() { this.Cantidad = 0; this.Estado = true; this.Detalle = new HashSet<ProformaDetalle>(); this.IdCorporacion = 1; this.IdEmpresa = 1; this.IdServicio = 33; this.Actividad = "E"; this.FechaSalida = DateTime.Now; }
        public HashSet<ProformaDetalle> Detalle { get;set;}
        public string Guardar(out string error)
        {
            if (this.Detalle.Count <= 0)
            {
                error = "La proforma no presenta detalles, imposible insertar";
                return null;
            }
            if (Total <= 0 || SubTotal <= 0)
            {
                error = "El total y/o subtotal debe ser mayor que cero";
                return null;
            }
            if (string.IsNullOrEmpty(RUC))
            {
                error = "Agregue el No. de RUC";
                return null;
            }
            if (string.IsNullOrEmpty(Token))
            {
                error = "Token Expirado";
                return null;
            }
            if (string.IsNullOrEmpty(UsuarioIngreso))
            {
                error = "Agregue el nombre del usuario que crea la proforma";
                return null;
            }
            if (IdUsuario <= 0)
            {
                error = "Agregue el Id de usuario (#)";
                return null;
            }
            //validar todos los detalles
            foreach (var item in this.Detalle)
            {
                if (!item.IsValid(out error))
                {
                    return null;
                }
            }
       


            StringBuilder email = new StringBuilder();
            email.AppendFormat("Estimado {0}","Cliente");

            using (var con = new SqlConnection())
            {
                var conx = System.Configuration.ConfigurationManager.ConnectionStrings["service"];
                if (conx == null)
                {
                    error = "Cadena de conexión service NO EXISTE";
                    return null;
                }
                con.ConnectionString = conx.ConnectionString;
                var comandoCabecera = con.CreateCommand();
                comandoCabecera.CommandText = "pc_proforma_insertar_2017";
                comandoCabecera.CommandTimeout = 6000;
                comandoCabecera.CommandType = CommandType.StoredProcedure;
                comandoCabecera.Parameters.AddWithValue("@IdCorporacion", IdCorporacion);
                comandoCabecera.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                comandoCabecera.Parameters.AddWithValue("@IdServicio", IdServicio);
                comandoCabecera.Parameters.AddWithValue("@IdZona", IdZona);
                comandoCabecera.Parameters.AddWithValue("@IdGrupo", IdGrupo);
                comandoCabecera.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                comandoCabecera.Parameters.AddWithValue("@UsuarioIngreso", UsuarioIngreso);
                comandoCabecera.Parameters.AddWithValue("@actividad", Actividad);
                comandoCabecera.Parameters.AddWithValue("@RUC", RUC);
                comandoCabecera.Parameters.AddWithValue("@FechaSalida", FechaSalida);
                comandoCabecera.Parameters.AddWithValue("@SubTotal", SubTotal);
                comandoCabecera.Parameters.AddWithValue("@IVA", IVA);
                comandoCabecera.Parameters.AddWithValue("@ReteIVA", ReteIVA);
                comandoCabecera.Parameters.AddWithValue("@ReteFuente", ReteFuente);
                comandoCabecera.Parameters.AddWithValue("@Total", Total);
                comandoCabecera.Parameters.AddWithValue("@Token", Token);
                comandoCabecera.Parameters.AddWithValue("@Nave", Nave);
                comandoCabecera.Parameters.AddWithValue("@Gkey", Gkey);
                comandoCabecera.Parameters.AddWithValue("@Estado", Estado);

                comandoCabecera.Parameters.AddWithValue("@Cantidad", Cantidad);
                comandoCabecera.Parameters.AddWithValue("@Bokingnbr", Bokingnbr);
                comandoCabecera.Parameters.AddWithValue("@Referencia", Referencia);
                comandoCabecera.Parameters.AddWithValue("@Reserva", Reserva);

                comandoCabecera.Parameters.AddWithValue("@Etd", Etd.ToString("yyyy-MM-dd HH:mm"));
                comandoCabecera.Parameters.AddWithValue("@CutOff", Cutoff.ToString("yyyy-MM-dd HH:mm"));
                comandoCabecera.Parameters.AddWithValue("@FechaCliente", FechaCliente.ToString("yyyy-MM-dd HH:mm"));
                comandoCabecera.Parameters.AddWithValue("@Reefer", Reefer);
                comandoCabecera.Parameters.AddWithValue("@Size", Size);
                comandoCabecera.Parameters.AddWithValue("@IvaMas", IvaMas);


                //-->Nuevos Campos 2017-->Abril->19
                comandoCabecera.Parameters.AddWithValue("@PctIVA", PctIVA);
                comandoCabecera.Parameters.AddWithValue("@PctFuente", PctFuente);


                var sale = new SqlParameter();
                sale.Direction = ParameterDirection.Output;
                sale.SqlDbType = SqlDbType.BigInt;
                sale.ParameterName = "@IdContenido";

                var secuencia = new SqlParameter();
                secuencia.Direction = ParameterDirection.Output;
                secuencia.SqlDbType = SqlDbType.NVarChar;
                secuencia.Size = 100;
                secuencia.ParameterName = "@Secuencia";

                comandoCabecera.Parameters.Add(sale);
                comandoCabecera.Parameters.Add(secuencia);
       
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    try
                    {
                        comandoCabecera.Transaction = tx;
                        comandoCabecera.ExecuteNonQuery();
                        var secIdentity = Int64.Parse(sale.Value.ToString());
                       
                        foreach (var ti in this.Detalle)
                        {
                            ti.IdProforma = secIdentity;
                            var comandoBody = con.CreateCommand();
                            comandoBody.Transaction = tx;
                            comandoBody.CommandText = "pc_proforma_insertar_det";
                            comandoBody.CommandTimeout = 6000;
                            comandoBody.CommandType = CommandType.StoredProcedure;
                            comandoBody.Parameters.AddWithValue("@IdProforma", ti.IdProforma);
                            comandoBody.Parameters.AddWithValue("@Item", ti.Item);
                            comandoBody.Parameters.AddWithValue("@CodigoServicio", ti.CodigoServicio);
                            comandoBody.Parameters.AddWithValue("@DescServicio", ti.DescServicio);
                            comandoBody.Parameters.AddWithValue("@IdContenedor", ti.IdContenedor);
                            comandoBody.Parameters.AddWithValue("@Contenedor", ti.Contenedor);
                            comandoBody.Parameters.AddWithValue("@FechaAlmacenaje", ti.FechaAlmacenaje);
                            comandoBody.Parameters.AddWithValue("@Referencia", ti.Referencia);
                            comandoBody.Parameters.AddWithValue("@TarifaControl", ti.TarifaControl);
                            comandoBody.Parameters.AddWithValue("@ValorUnitario", ti.ValorUnitario);
                            comandoBody.Parameters.AddWithValue("@Tipo", ti.Tipo);
                            comandoBody.Parameters.AddWithValue("@Cantidad", ti.Cantidad);

                            comandoBody.Parameters.AddWithValue("@ValorTotal", ti.ValorTotal);
                            comandoBody.Parameters.AddWithValue("@BL", ti.BL);
                            comandoBody.ExecuteNonQuery();
                        }
                        email.AppendFormat("<h2>Se ha generado exitosamente la proforma No. {0}</h2>", secuencia.Value.ToString());

                        email.Append("<h3>A continuación el detalle:</h3>");
                        email.AppendFormat("<h4>Booking Number.{0}</h4>", Bokingnbr);
                        email.AppendFormat("<p>Fecha de estimada de Ingreso:{0}</p>", FechaCliente.ToString("dd/MM/yyyy HH:mm"));
                        email.AppendFormat("<p>Fecha estimada de zarpe:{0}</p>", Etd.ToString("dd/MM/yyyy HH:mm"));
                        email.AppendFormat("<p>Fecha de CutOff:{0}</p>", Cutoff.ToString("dd/MM/yyyy HH:mm"));
                        email.Append("<table cellpading='2' cellspacing='2' border='1'>");
                        email.Append("<tr><th>Código</th><th>Descripción</th><th>Cant.</th><th>V.Unit</th><th>V.Total</th></tr>");
                        foreach (var ti in this.Detalle)
                        {
                            email.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", ti.CodigoServicio, ti.DescServicio, ti.Cantidad, ti.ValorUnitario.ToString("C"), ti.ValorTotal.ToString("C"));
                        }
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>Subtotal</strong></td><td>{0:c}</td></tr>", SubTotal);
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>IVA {0}% (+)</strong></td><td>{0:c}</td></tr>", IvaMas, IVA);
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>Ret.Fte {0}% (-)</strong></td><td>{1:c}</td></tr>", PctFuente, ReteFuente);
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>Ret.IVA {0}% (-)</strong></td><td>{1:c}</td></tr>", PctIVA, ReteIVA);

                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>NETO A PAGAR</strong></td><td>{0:c}</td></tr>", Total);
                        email.Append("</table>");
                        email.AppendFormat("<br/>Atentamente,<br/><strong>Terminal Virtual</strong><br/>Contecon Guayaquil S.A. CGSA<br/>An ICTSI Group Company.");
                        email.AppendFormat("<br><p style='font-size:x-small; text-align:right'>Fecha de creación:{0}</p>", DateTime.Now.ToString("dd/MM/YYYY HH:mm"));
  
                        var mcom = con.CreateCommand();
                        mcom.Transaction = tx;
                        mcom.CommandType = CommandType.StoredProcedure;
                        mcom.CommandText = "sp_insert_mail_log_pan";
                        mcom.Parameters.AddWithValue("@htmlmsg", email.ToString());
                        mcom.Parameters.AddWithValue("@mailpara", Email);
                        mcom.Parameters.AddWithValue("@copiaspara", Email);
                        mcom.Parameters.AddWithValue("@usuario", UsuarioIngreso);
                        mcom.Parameters.AddWithValue("@asunto", string.Format("SNA:  Servicio de Notificaciones Automáticas -  Proforma de Exportación No. {0}", secuencia.Value));
                        mcom.Parameters.AddWithValue("@moduloID", 0);
                        mcom.Parameters.AddWithValue("@codigo_evento", 1);
                        mcom.Parameters.AddWithValue("@sistema", "CSL");
                        mcom.ExecuteNonQuery();
                        tx.Commit();
                        this.Secuencia = secuencia.Value != null ? secuencia.Value.ToString():null;
                        con.Dispose();
                        error = string.Empty;
                        return secIdentity.ToString();
                    }
                    catch (SqlException ex)
                    {
                        tx.Rollback();
                        con.Dispose();
                        var  ng = csl_log.log_csl.save_log<SqlException>(ex, "Proforma", "Guardar", "No disponible", this.UsuarioIngreso);
                        error = string.Format("Estimado Cliente, hubo un problema durante el almacenamiento de datos, por favor reporte este código de servicio BD00-{0}",ng);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        con.Dispose();
                        var ng = csl_log.log_csl.save_log<Exception>(ex, "Proforma", "Guardar", "No disponible", this.UsuarioIngreso);
                        error = string.Format("Estimado Cliente, hubo un problema durante el almacenamiento de datos, por favor reporte este código de servicio EX00-{0}", ng);
                        return null;
                    }
                
                }
                

            }
        }

        //nuevo 2017-->Graba los clientes de contado, la liquidacion
        public string GuardarContado(usuario user, out string error)
        {

            if (user.IsCredito)
            {
                error = string.Format("El cliente con ruc: {0} no es un cliente de contado, el proceso no continuará",user.ruc);
                return null;
            }
            
            if (this.Detalle.Count <= 0)
            {
                error = "La proforma no presenta detalles, imposible insertar";
                return null;
            }
            if (Total <= 0 || SubTotal <= 0)
            {
                error = "El total y/o subtotal debe ser mayor que cero";
                return null;
            }
            if (string.IsNullOrEmpty(RUC))
            {
                error = "Agregue el No. de RUC";
                return null;
            }
            if (string.IsNullOrEmpty(Token))
            {
                error = "Token Expirado";
                return null;
            }
            if (string.IsNullOrEmpty(UsuarioIngreso))
            {
                error = "Agregue el nombre del usuario que crea la proforma";
                return null;
            }
            if (IdUsuario <= 0)
            {
                error = "Agregue el Id de usuario (#)";
                return null;
            }
            //validar todos los detalles
            foreach (var item in this.Detalle)
            {
                if (!item.IsValid(out error))
                {
                    return null;
                }
            }
           
         

            StringBuilder email = new StringBuilder();
            email.AppendFormat("Estimado {0}", "Cliente");

            using (var con = new SqlConnection())
            {
                var conx = System.Configuration.ConfigurationManager.ConnectionStrings["service"];
                if (conx == null)
                {
                    error = "Cadena de conexión service NO EXISTE";
                    return null;
                }
                con.ConnectionString = conx.ConnectionString;
                var comandoCabecera = con.CreateCommand();
                comandoCabecera.CommandText = "pc_proforma_insertar_contado";
                comandoCabecera.CommandTimeout = 6000;
                comandoCabecera.CommandType = CommandType.StoredProcedure;
                comandoCabecera.Parameters.AddWithValue("@IdCorporacion", IdCorporacion);
                comandoCabecera.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                comandoCabecera.Parameters.AddWithValue("@IdServicio", IdServicio);
                comandoCabecera.Parameters.AddWithValue("@IdZona", IdZona);
                comandoCabecera.Parameters.AddWithValue("@IdGrupo", IdGrupo);
                comandoCabecera.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                comandoCabecera.Parameters.AddWithValue("@UsuarioIngreso", UsuarioIngreso);
                comandoCabecera.Parameters.AddWithValue("@actividad", Actividad);
                comandoCabecera.Parameters.AddWithValue("@RUC", RUC);
                comandoCabecera.Parameters.AddWithValue("@FechaSalida", FechaSalida);
                comandoCabecera.Parameters.AddWithValue("@SubTotal", SubTotal);
                comandoCabecera.Parameters.AddWithValue("@IVA", IVA);
                comandoCabecera.Parameters.AddWithValue("@ReteIVA", ReteIVA);
                comandoCabecera.Parameters.AddWithValue("@ReteFuente", ReteFuente);
                comandoCabecera.Parameters.AddWithValue("@Total", Total);
                comandoCabecera.Parameters.AddWithValue("@Token", Token);
                comandoCabecera.Parameters.AddWithValue("@Nave", Nave);
                comandoCabecera.Parameters.AddWithValue("@Gkey", Gkey);
                comandoCabecera.Parameters.AddWithValue("@Estado", Estado);

                comandoCabecera.Parameters.AddWithValue("@Cantidad", Cantidad);
                comandoCabecera.Parameters.AddWithValue("@Bokingnbr", Bokingnbr);
                comandoCabecera.Parameters.AddWithValue("@Referencia", Referencia);
                comandoCabecera.Parameters.AddWithValue("@Reserva", Reserva);

                comandoCabecera.Parameters.AddWithValue("@Etd", Etd.ToString("yyyy-MM-dd HH:mm"));
                comandoCabecera.Parameters.AddWithValue("@CutOff", Cutoff.ToString("yyyy-MM-dd HH:mm"));
                comandoCabecera.Parameters.AddWithValue("@FechaCliente", FechaCliente.ToString("yyyy-MM-dd HH:mm"));
                comandoCabecera.Parameters.AddWithValue("@Reefer", Reefer);
                comandoCabecera.Parameters.AddWithValue("@Size", Size);

                comandoCabecera.Parameters.AddWithValue("@IvaMas", IvaMas);

                //Nuevo 2017---------------------------------------->
                comandoCabecera.Parameters.AddWithValue("@Liquidacion", Liquidacion);
                //--------------------------------------------------//

                //-->Nuevos Campos 2017-->Abril->19
                comandoCabecera.Parameters.AddWithValue("@PctIVA", PctIVA);
                comandoCabecera.Parameters.AddWithValue("@PctFuente", PctFuente);

                var sale = new SqlParameter();
                sale.Direction = ParameterDirection.Output;
                sale.SqlDbType = SqlDbType.BigInt;
                sale.ParameterName = "@IdContenido";

                var secuencia = new SqlParameter();
                secuencia.Direction = ParameterDirection.Output;
                secuencia.SqlDbType = SqlDbType.NVarChar;
                secuencia.Size = 100;
                secuencia.ParameterName = "@Secuencia";

                comandoCabecera.Parameters.Add(sale);
                comandoCabecera.Parameters.Add(secuencia);

                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    try
                    {
                        comandoCabecera.Transaction = tx;
                        comandoCabecera.ExecuteNonQuery();
                        var secIdentity = Int64.Parse(sale.Value.ToString());

                        foreach (var ti in this.Detalle)
                        {
                            ti.IdProforma = secIdentity;
                            var comandoBody = con.CreateCommand();
                            comandoBody.Transaction = tx;
                            comandoBody.CommandText = "pc_proforma_insertar_det";
                            comandoBody.CommandTimeout = 6000;
                            comandoBody.CommandType = CommandType.StoredProcedure;
                            comandoBody.Parameters.AddWithValue("@IdProforma", ti.IdProforma);
                            comandoBody.Parameters.AddWithValue("@Item", ti.Item);
                            comandoBody.Parameters.AddWithValue("@CodigoServicio", ti.CodigoServicio);
                            comandoBody.Parameters.AddWithValue("@DescServicio", ti.DescServicio);
                            comandoBody.Parameters.AddWithValue("@IdContenedor", ti.IdContenedor);
                            comandoBody.Parameters.AddWithValue("@Contenedor", ti.Contenedor);
                            comandoBody.Parameters.AddWithValue("@FechaAlmacenaje", ti.FechaAlmacenaje);
                            comandoBody.Parameters.AddWithValue("@Referencia", ti.Referencia);
                            comandoBody.Parameters.AddWithValue("@TarifaControl", ti.TarifaControl);
                            comandoBody.Parameters.AddWithValue("@ValorUnitario", ti.ValorUnitario);
                            comandoBody.Parameters.AddWithValue("@Tipo", ti.Tipo);
                            comandoBody.Parameters.AddWithValue("@Cantidad", ti.Cantidad);

                            comandoBody.Parameters.AddWithValue("@ValorTotal", ti.ValorTotal);
                            comandoBody.Parameters.AddWithValue("@BL", ti.BL);
                            comandoBody.ExecuteNonQuery();
                        }
                        this.Secuencia = secuencia.Value != null ? secuencia.Value.ToString() : null;
                        //--------------------->2017--------------------->Solo si cliente es contado
                        string secuenciaContado;
                        if (!guardarLiquidacion(user, out secuenciaContado))
                        {
                            tx.Rollback();
                            con.Dispose();
                            error = "Fue imposible guardar la liquidación de esta proforma comuniquese con CGSA";
                            return null;
                        }
                        this.Liquidacion = secuenciaContado;
                        //---------------------------------------------------------------------

                        if (user.IsCredito)
                        {
                            email.AppendFormat("<h2>Se ha generado exitosamente la proforma No. {0}</h2>", secuencia.Value);
                        }
                        else
                        {
                            email.AppendFormat("<h2>Se ha generado exitosamente la proforma No. {0}</h2>", secuenciaContado);
                        }
                        email.Append("<h3>A continuación el detalle:</h3>");
                        email.AppendFormat("<h4>Booking Number.{0}</h4>", Bokingnbr);
                        email.AppendFormat("<p>Fecha de estimada de Ingreso:{0}</p>", FechaCliente.ToString("dd/MM/yyyy HH:mm"));
                        email.AppendFormat("<p>Fecha estimada de zarpe:{0}</p>", Etd.ToString("dd/MM/yyyy HH:mm"));
                        email.AppendFormat("<p>Fecha de CutOff:{0}</p>", Cutoff.ToString("dd/MM/yyyy HH:mm"));
                        email.Append("<table cellpading='2' cellspacing='2' border='1'>");
                        email.Append("<tr><th>Código</th><th>Descripción</th><th>Cant.</th><th>V.Unit</th><th>V.Total</th></tr>");
                        foreach (var ti in this.Detalle)
                        {
                            email.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", ti.CodigoServicio, ti.DescServicio, ti.Cantidad, ti.ValorUnitario.ToString("C"), ti.ValorTotal.ToString("C"));
                        }
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>Subtotal</strong></td><td>{0:c}</td></tr>", SubTotal);
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>IVA {0}% (+)</strong></td><td>{1:c}</td></tr>",IvaMas, IVA);
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>Ret.Fte {0}% (-)</strong></td><td>{1:c}</td></tr>",PctFuente, ReteFuente);
                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>Ret.IVA {0}% (-)</strong></td><td>{1:c}</td></tr>",PctIVA, ReteIVA);

                        email.AppendFormat("<tr><td colspan='4' style='text-aling:right;'><strong>NETO A PAGAR</strong></td><td>{0:c}</td></tr>", Total);
                        email.Append("</table>");
                        email.AppendFormat("<br/>Atentamente,<br/><strong>Terminal Virtual</strong><br/>Contecon Guayaquil S.A. CGSA<br/>An ICTSI Group Company.");
                        email.AppendFormat("<br><p style='font-size:x-small; text-align:right'>Fecha de creación:{0}</p>",DateTime.Now.ToString("dd/MM/YYYY HH:mm"));
                       
                        
                        //el correo electronico
                        var mcom = con.CreateCommand();
                        mcom.Transaction = tx;
                        mcom.CommandType = CommandType.StoredProcedure;
                        mcom.CommandText = "sp_insert_mail_log_pan";
                        mcom.Parameters.AddWithValue("@htmlmsg", email.ToString());
                        mcom.Parameters.AddWithValue("@mailpara", Email);
                        mcom.Parameters.AddWithValue("@copiaspara", Email);
                        mcom.Parameters.AddWithValue("@usuario", UsuarioIngreso);
                        mcom.Parameters.AddWithValue("@asunto", string.Format("SNA:  Servicio de Notificaciones Automáticas -  Proforma de Exportación No. {0}", this.Liquidacion));
                        mcom.Parameters.AddWithValue("@moduloID", 0);
                        mcom.Parameters.AddWithValue("@codigo_evento", 1);
                        mcom.Parameters.AddWithValue("@sistema", "CSL");
                        mcom.ExecuteNonQuery();
                        tx.Commit();
                        


                        con.Dispose();
                        //UPDATE LA PROFORMA ACTUAL CON EL VALOR DE LA SECUENCIA CONTADO
                        this.UpdateLiquidacion();
                        error = string.Empty;

    
                        return secIdentity.ToString();
                    }
                    catch (SqlException ex)
                    {
                        tx.Rollback();
                        con.Dispose();
                        var ng = csl_log.log_csl.save_log<SqlException>(ex, "Proforma", "Guardar", "No disponible", this.UsuarioIngreso);
                        error = string.Format("Estimado Cliente, hubo un problema durante el almacenamiento de datos, por favor reporte este código de servicio BD00-{0}", ng);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        con.Dispose();
                        var ng = csl_log.log_csl.save_log<Exception>(ex, "Proforma", "Guardar", "No disponible", this.UsuarioIngreso);
                        error = string.Format("Estimado Cliente, hubo un problema durante el almacenamiento de datos, por favor reporte este código de servicio EX00-{0}", ng);
                        return null;
                    }

                }


            }
        }

        public static bool Borrar(Int64 proforma, string usuario, out string mensaje)
        {
            try
            {
                using (var conexion = new SqlConnection())
                {
                    var conx = System.Configuration.ConfigurationManager.ConnectionStrings["service"];
                    if (conx == null)
                    {
                        mensaje = "Cadena de conexión service NO EXISTE";
                        return false;
                    }
                    conexion.ConnectionString = conx.ConnectionString;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_borra_proforma";
                            comando.Parameters.AddWithValue("@IdProforma", proforma);
                            comando.Parameters.AddWithValue("@usuario", usuario);
                            comando.Parameters.AddWithValue("@fecha", DateTime.Now);
                            conexion.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conexion.Close();
                            mensaje = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "Proforma", "Borrar", proforma.ToString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "Proforma", "Borrar", proforma.ToString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static bool BorrarLiquidacion(Int64 proforma, string usuario, string liquidacion, out string mensaje)
        {
            try
            {
                using (var conexion = new SqlConnection())
                {
                    var conx = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"];
                    if (conx == null)
                    {
                        mensaje = "Cadena de conexión service NO EXISTE";
                        return false;
                    }
                    if (string.IsNullOrEmpty(liquidacion))
                    {
                        mensaje = "No es posible eliminar la liquidación ya que su número no es coherente o no existe";
                        return false;
                    }
                    conexion.ConnectionString = conx.ConnectionString;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_anula_liquidacion";
                            comando.Parameters.AddWithValue("@liquidacion", liquidacion);
                            comando.Parameters.AddWithValue("@proforma", proforma);
                            comando.Parameters.AddWithValue("@modifica", usuario);
                            conexion.Open();
                            var valor = comando.ExecuteScalar();
                            int afecta = 0;
                            conexion.Close();
                            if (valor != null && valor.GetType() != typeof(DBNull))
                            {
                                var s = valor.ToString();
                                if (!int.TryParse(s, out afecta))
                                {
                                    mensaje = string.Format("Hubo un problema para eliminar la liquidación {0}, es posible que la misma no exista en nuestros registros", liquidacion);
                                    return false;
                                }
                                if (afecta <= 0)
                                {
                                    mensaje = string.Format("Hubo un problema para eliminar la liquidación {0}, es posible que la misma no exista, o que la misma haya sido pagada", liquidacion);
                                    return false;
                                }
                                mensaje = string.Empty;
                                return true;
                            }
                            mensaje = string.Format("Hubo un problema para eliminar la liquidación {0}, la misma no pudo ser encontrada en nuestros registros.", liquidacion);
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "Proforma", "BorrarLiquidacion", proforma.ToString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "Proforma", "BorrarLiquidacionSQL", proforma.ToString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        public static  Dictionary<string, string> GetClientInfo(string RUC)
        {
            var conx = System.Configuration.ConfigurationManager.ConnectionStrings["N5"];
            if (conx == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(RUC))
            {
                return null;
            }
            using (SqlConnection connection = new SqlConnection(conx.ConnectionString))
            {
                try
                {
                    Dictionary<string, string> lista = new Dictionary<string, string>();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "pc_proforma_clientes";
                    command.Parameters.AddWithValue("@ruc", RUC);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        for (int i = 0; i <= (reader.FieldCount - 1); i++)
                        {
                            if (!reader.IsDBNull(i))
                            {
                                lista.Add(reader.GetName(i), reader.GetString(i));
                            }
                        }
                        return lista;
                    }
                    connection.Close();
                    command.Dispose();
                }
                catch (Exception exception)
                {
                    csl_log.log_csl.save_log<Exception>(exception, "Proforma", "GetClientInfo", "No disponible", RUC);
                    return null;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            return null;
        }
        
        //Nuevo 2017 obtener numero de liquidaciones, solo si cliente es contado
        public bool guardarLiquidacion( usuario user, out string secuencia)
        {
            //ECU_INGRESO_ANTICIPO_S3
            //Obtener el nombre 
            string razon_social = string.Empty;
            razon_social = CslHelper.getShiperName(string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa);
            try
            {
                using (var conexion = new SqlConnection())
                {
                    var conx = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"];
                    if (conx == null)
                    {
                        secuencia = "Cadena de conexión service NO EXISTE";
                        return false;
                    }
                    conexion.ConnectionString = conx.ConnectionString;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "ECU_INGRESO_ANTICIPO_S3";
                            comando.Parameters.AddWithValue("@razonSocial", razon_social);
                            comando.Parameters.AddWithValue("@identificacion", user.ruc);
                            comando.Parameters.AddWithValue("@monto", this.Total);
                            comando.Parameters.AddWithValue("@usuario", user.loginname);
                            comando.Parameters.AddWithValue("@numeroBooking", this.Bokingnbr);
                            comando.Parameters.AddWithValue("@idRol", user.grupo);
                            comando.Parameters.AddWithValue("@proformaID", this.Gkey);
                            comando.Parameters.AddWithValue("@proformaSQ", this.Secuencia);
           
                            conexion.Open();
                            var result = comando.ExecuteScalar().ToString();
                            conexion.Close();
                            secuencia = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "Proforma", "guardarLiquidacion",user.ruc , user.loginname);
                        secuencia = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "Proforma", "guardarLiquidacion", this.Secuencia, "sistema");
                secuencia = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
       

        }
        public bool UpdateLiquidacion( )
        {
            try
            {
                using (var conexion = new SqlConnection())
                {
                    var conx = System.Configuration.ConfigurationManager.ConnectionStrings["service"];
                    if (conx == null)
                    {
                        
                        return false;
                    }
                    conexion.ConnectionString = conx.ConnectionString;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_update_proforma_liquidacion";
                            comando.Parameters.AddWithValue("@IdProforma", this.Secuencia);
                            comando.Parameters.AddWithValue("@Liquidacion", this.Liquidacion);
                            conexion.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conexion.Close();
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "Proforma", "UpdateLiquidacion", this.Secuencia, "sistema");
                         return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "Proforma", "UpdateLiquidacion", this.Secuencia, "sistema");
                return false;
            }
        }
        //----->
    }
    public class ProformaDetalle
    {
        public Int64 IdProforma { get; set; } //si
        public int Item { get; set; }//si
        public string CodigoServicio { get; set; }//si
        public string DescServicio { get; set; }//si
        public int IdContenedor { get; set; }
        public string Contenedor { get; set; }
        public DateTime FechaAlmacenaje { get; set; }
        public string Referencia { get; set; } //si
        public decimal TarifaControl { get; set; }
        public decimal ValorUnitario { get; set; } //si
        public string Tipo { get; set; }
        public decimal Cantidad { get; set; }//si
        public decimal ValorTotal { get; set; }//si
        public string BL { get; set; }//si
        public ProformaDetalle() { this.TarifaControl = 1; this.IdContenedor = 1; this.Tipo = "PO"; }
        public bool IsValid(out string error)
        {
            //if (IdProforma <= 0)
            //{
            //    error = "No existe número de secuencia";
            //    return false;
            //}
            if (Item <= 0)
            {
                error = "No existe no. de item";
                return false;
            }
            if (string.IsNullOrEmpty(CodigoServicio))
            {
                error = "Agregue el codigo de servicio";
                return false;
            }
            if (string.IsNullOrEmpty(DescServicio))
            {
                error = "Agregue la descripcion del servicio";
                return false;
            }
            if (string.IsNullOrEmpty(Referencia))
            {
                error = "Agregue la referencia de la nave";
                return false;
            }
            
            //nuevo 2017--> Los valores de detalles pueden ser cero, debido al nuevo calculo
            //if (ValorUnitario <= 0)
            //{
            //    error = "El valor unitario debe ser mayor que cero";
            //    return false;
            //}
            //if (Cantidad <= 0)
            //{
            //    error = "La cantidad debe ser mayor que cero";
            //    return false;
            //}
            //if (ValorTotal <= 0)
            //{
            //    error = "El valor total debe ser mayor que cero";
            //    return false;
            //}

            if (string.IsNullOrEmpty(BL))
            {
                error = "Agregue el número de Booking/BL";
                return false;
            }
            error = string.Empty;
            return true;
          
        }
    }
}