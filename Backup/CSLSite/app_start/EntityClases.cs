using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using csl_log;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using CSLSite.XmlTool;
using System.Globalization;


namespace CSLSite
{
    //representa el aisv puro
    public class jAisvContainer
    {
        #region "Propiedades"
        //aisv propios
        public string secuencia { get; set; }//numero como texto-RETURN
        public string idexport { get; set; }//numero como texto-O
        public string idline { get; set; } //id de la linea msc,msk-O
        public string tipo { get; set; }//tipo de catalogo aisv 1-full, 2-carga s, 3- consolidar.-O
        //booking
        public string bnumber { get; set; } //alfanumerico
        public string breferencia { get; set; } //año-linea-secuencia
        public string bnave { get; set; }//texto
        public string beta { get; set; }//fecha
        public string bcutOff { get; set; } //fecha
        public string buis { get; set; } //???
        public string bagencia { get; set; } //nombre de la línea
        public string bpod { get; set; } //puerto de descarga
        public string bpod1 { get; set; } //puerto de descarga final
        public string bcomodity { get; set; } //comodity
        public string bfk { get; set; } //freightkind
        //booking item
        public string bsizeu { get; set; } //tamaño de la unidad
        public string btipou { get; set; } // tipo de la unidad
        public string bimo { get; set; } // es imo, bool
        public string breefer { get; set; } // es reefer, bool
        public string bover { get; set; } // es oversize, bool
        public string bdeck { get; set; } // sobre o bajo cubierta
        public string bnota { get; set; } //texto
        //documento de aduana.
        public string aidagente { get; set; } //numero como texto
        public string adocnumero { get; set; } // numero como texto
        public string adoctipo { get; set; } // sufijo documento
        public string acupo { get; set; }// sujeto a cupos
        public string aidinst { get; set; } //idinstitucion
        public string aidrule { get; set; } // idregla
        //info de la unidad
        public string unumber { get; set; } //alfanumerico
        public string utara { get; set; } // numero
        public string umaxpay { get; set; } //numero
        public string upeso { get; set; } //numero
        //caracteri unidad
        public string uidrefri { get; set; } //idtipo de refrigeracion alfa
        public string utemp { get; set; } // temperatura numero
        public string uhumedad { get; set; }//humedad numero
        public string uidventila { get; set; }//id % ventilacion
        public string ubultos { get; set; }
        //sellos
        public string seal1 { get; set; } //sello de agencia
        public string seal2 { get; set; } //sello de reefer
        public string seal3 { get; set; }//sello adicional 1
        public string seal4 { get; set; }//sello adicional 2
        //exceso
        public string eHas { get; set; } //tiene exceso
        public string eleft { get; set; } //exceso izq.
        public string eright { get; set; }//exceso der.
        public string efront { get; set; }//exceso front.
        public string etop { get; set; }//exceso superior
        public string eback { get; set; }//exceso atras.
        //transporte
        public string tidubica { get; set; } //idciudad numero
        public string tidcanton { get; set; } //idcantont numero
        public string tfechadoc { get; set; }
        public string tviaje { get; set; }
        public string tconductor { get; set; }
        public string tdocument { get; set; }//cedula/pasaporte
        public string tplaca { get; set; } //placas
        //certificados
        public string tcabcert { get; set; } //certificado cabezal
        public string tcabcertfecha { get; set; } //año
        public string tchacert { get; set; }//certificado chasis
        public string tcabchafecha { get; set; }//año
        public string tespcert { get; set; }//certifcado especial
        public string thoras { get; set; }//horas de viaje
        public string udepo { get; set; }
        public string udepofecha { get; set; }
        public string remark { get; set; }
        public string gkey { get; set; }
        //mails->cadenas de string.
        public string mail1 { get; set; }
        public string mail2 { get; set; }
        public string mail3 { get; set; }
        public string mail4 { get; set; }
        public string mail5 { get; set; }
        //datos de trackin->datos de session actual.
        public string autor { get; set; }
        //datos de cargasuelta
        public string cimo { get; set; }
        public string cembalaje { get; set; }
        public string breserva { get; set; }
        public string nomexpo { get; set; }
        public string shipid { get; set; }
        public string shipname { get; set; }
        public string hzkey { get; set; }
        public string direccion { get; set; }
        public string vent_pc { get; set; }
        public string ventu { get; set; }
        //@aisv_detalle_direcc 
        public List<aisvDetalle> detalles { get; set; }
        //nuevos cambios 2015.

        //persona responsable del sello
        public string sellor { get; set; }
        //cedula de dicha persona
        public string selloid { get; set; }
        //compania responsable del transporte
        public string trancia { get; set; }
        //ruc de dicha compania
        public string tranruc { get; set; }
        //proformas
        public string hasprof { get; set; }
        public string prosequence { get; set; }

        //nuevo late arrival
        public string late { get; set; }

        //nuevo 2017
        public string consignatario { get; set; }
        //nuevo 2017
        public string diplomatico { get; set; }
       

        //propiedad para saber si acaso intento sobrescribir
        #endregion
        //variable de cadena de conexion
        private static string cadena = string.Empty;
        private static void initdataconf()
        {
            if (cadena.Trim().Length <= 0)
            {
                if (System.Configuration.ConfigurationManager.ConnectionStrings["service"] != null)
                {
                    cadena = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                    return;
                }
                throw new InvalidOperationException("Error al inicializar conexión [D01]");
            }
        }

        //agrega el aisv
        public bool add(out string number)
        {
            try
            {
                var t1 = this.tipo.ToCharArray();
                initdataconf();
                //CREAR EL PREAVISO, O BL DEPENDIENDO DEL TIPO DE AISV.
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_add_aisv_2017";
                            comando.Parameters.AddWithValue("@aisv_number", this.secuencia);
                            comando.Parameters.AddWithValue("@aisv_tipo_mov", t1[0]);
                            comando.Parameters.AddWithValue("@referencia", this.breferencia);
                            comando.Parameters.AddWithValue("@aisv_codig_agnv", this.idline);
                            comando.Parameters.AddWithValue("@aisv_codig_buqu", this.bnave);
                            comando.Parameters.AddWithValue("@aisv_codig_puer ", this.bpod);
                            comando.Parameters.AddWithValue("@aisv_codig_puer_fin", this.bpod1);
                            comando.Parameters.AddWithValue("@aisv_codig_agen", this.aidagente);
                            comando.Parameters.AddWithValue("@aisv_desc_prod", this.bcomodity);
                            comando.Parameters.AddWithValue("@aisv_peso_carga", this.upeso);
                            comando.Parameters.AddWithValue("@aisv_codig_clte", this.idexport);
                            comando.Parameters.AddWithValue("@aisv_tipo_carga", t1[1]);
                            if (this.cimo != null && this.cimo.Trim().Length > 0)
                            {
                                comando.Parameters.AddWithValue("@aisv_code_imo", this.cimo);
                            }
                            else
                            {
                                comando.Parameters.AddWithValue("@aisv_code_imo", 0);
                            }
                            comando.Parameters.AddWithValue("@aisv_numero_booking", this.bnumber);
                            comando.Parameters.AddWithValue("@aisv_contenedor", this.unumber);
                            comando.Parameters.AddWithValue("@aisv_tara_cont", this.utara.Replace(",", "."));
                            comando.Parameters.AddWithValue("@aisv_temperatura_cont", this.utemp);
                            comando.Parameters.AddWithValue("@aisv_humedad_cont", this.uhumedad);
                            comando.Parameters.AddWithValue("@aisv_ventilacion_cont", this.uidventila);
                            comando.Parameters.AddWithValue("@aisv_tamano_cont", this.bsizeu);
                            comando.Parameters.AddWithValue("@aisv_exceso_izq", this.eleft);
                            comando.Parameters.AddWithValue("@aisv_exceso_der", this.eright);
                            comando.Parameters.AddWithValue("@aisv_exceso_frn", this.efront);
                            comando.Parameters.AddWithValue("@aisv_exceso_enc", this.etop);
                            comando.Parameters.AddWithValue("@aisv_cedul_chof", this.tdocument);
                            comando.Parameters.AddWithValue("@aisv_nombr_chof", this.tconductor);
                            comando.Parameters.AddWithValue("@aisv_placa_vehi", this.tplaca);
                            comando.Parameters.AddWithValue("@aisv_user_ing", this.autor);
                            comando.Parameters.AddWithValue("@aisv_max_payload", this.umaxpay);
                            comando.Parameters.AddWithValue("@aisv_email_user", this.mail1);
                            comando.Parameters.AddWithValue("@aisv_sello1", this.seal1);
                            comando.Parameters.AddWithValue("@aisv_sello2", this.seal2);
                            comando.Parameters.AddWithValue("@aisv_sello3", this.seal3);
                            comando.Parameters.AddWithValue("@aisv_sello4", this.seal4);
                            comando.Parameters.AddWithValue("@aisv_embalaje", this.cembalaje);
                            comando.Parameters.AddWithValue("@aisv_cant_bult", this.ubultos);
                            comando.Parameters.AddWithValue("@aisv_tipo_cont", t1[1] == 'C' ? this.btipou : null);
                            //nuevos campos-->PROBAR
                            comando.Parameters.AddWithValue("@aisv_agencia_n", this.bagencia);
                            comando.Parameters.AddWithValue("@aisv_tipo_cntr", this.btipou);
                            comando.Parameters.AddWithValue("@aisv_nota", this.remark);
                            comando.Parameters.AddWithValue("@aisv_adu_doc", this.adocnumero);
                            comando.Parameters.AddWithValue("@aisv_adu_doc_tipo", this.adoctipo);
                            comando.Parameters.AddWithValue("@aisv_cu_id_inst", this.aidinst);
                            comando.Parameters.AddWithValue("@aisv_cu_id_rule", this.aidrule);
                            comando.Parameters.AddWithValue("@aisv_id_refri", this.uidrefri);
                            comando.Parameters.AddWithValue("@aisv_ex_back", this.eback);
                            comando.Parameters.AddWithValue("@aisv_id_prov", this.tidubica);
                            comando.Parameters.AddWithValue("@aisv_fk", this.bfk);
                            comando.Parameters.AddWithValue("@aisv_id_cant", this.tidcanton);
                            comando.Parameters.AddWithValue("@aisv_mtop_cab_cert", this.tcabcert);
                            comando.Parameters.AddWithValue("@aisv_mtop_cha_cert", this.tchacert);
                            comando.Parameters.AddWithValue("@aisv_mtop_espe_cert", this.tespcert);
                            comando.Parameters.AddWithValue("@aisv_h_viaje", this.thoras);
                            comando.Parameters.AddWithValue("@aisv_mail2", this.mail2);
                            comando.Parameters.AddWithValue("@aisv_mail3", this.mail3);
                            comando.Parameters.AddWithValue("@aisv_mail4", this.mail4);
                            comando.Parameters.AddWithValue("@aisv_mail5", this.mail5);
                            comando.Parameters.AddWithValue("@aisv_id_depo", this.udepo);
                            comando.Parameters.AddWithValue("@aisv_nom_expor", this.nomexpo);
                            //aisv_b_reserva
                            comando.Parameters.AddWithValue("@aisv_b_reserva", this.breserva);
                            comando.Parameters.AddWithValue("@aisv_ship_id", this.shipid);
                            comando.Parameters.AddWithValue("@aisv_ship_name", this.shipname);
                            comando.Parameters.AddWithValue("@aisv_hzkey", this.hzkey);
                            //CAMPOS FECHA
                            DateTime fecha;
                            if (!DateTime.TryParse(this.tfechadoc, out fecha))
                            {
                                number = "Error al convertir la fecha de salida de finca";
                                return false;
                            }
                            comando.Parameters.AddWithValue("@aisv_fecha_plant", fecha);

                            if (DateTime.TryParse(this.udepofecha, out fecha))
                            {
                                comando.Parameters.AddWithValue("@aisv_depo_date", fecha);
                            }

                            if (!DateTime.TryParse(this.beta, out fecha))
                            {
                                number = "Error al convertir la fecha de ETA";
                                return false;
                            }

                            comando.Parameters.AddWithValue("@aisv_eta", fecha);
                            if (!DateTime.TryParse(this.bcutOff, out fecha))
                            {
                                number = "Error al convertir la fecha de CutOff";
                                return false;
                            }
                            comando.Parameters.AddWithValue("@aisv_cutoff", fecha);
                            if (!DateTime.TryParse(this.buis, out fecha))
                            {
                                fecha = DateTime.Now;
                            }
                            comando.Parameters.AddWithValue("@aisv_uis", fecha);

                            if (!string.IsNullOrEmpty(this.tcabcertfecha) && DateTime.TryParse(this.tcabcertfecha.Trim(), out fecha))
                            {
                                comando.Parameters.AddWithValue("@aisv_mtop_cab_date", fecha);
                            }

                            if (!string.IsNullOrEmpty(this.tcabchafecha) && DateTime.TryParse(this.tcabchafecha.Trim(), out fecha))
                            {
                                comando.Parameters.AddWithValue("@aisv_mtop_cha_date", fecha);
                            }
                            //CAMPOS BIT
                            comando.Parameters.AddWithValue("@aisv_exceso_tam", !string.IsNullOrEmpty(this.eHas) && this.eHas.ToLower() == "true" ? 'S' : 'N');
                            comando.Parameters.AddWithValue("@aisv_bo_imo", !string.IsNullOrEmpty(this.bimo) && this.bimo.ToLower() == "true" ? 1 : 0);
                            comando.Parameters.AddWithValue("@aisv_bo_reef", !string.IsNullOrEmpty(this.breefer) && this.breefer.ToLower() == "true" ? 1 : 0);
                            comando.Parameters.AddWithValue("@aisv_bo_over", !string.IsNullOrEmpty(this.bover) && this.bover.ToLower() == "true" ? 1 : 0);
                            comando.Parameters.AddWithValue("@aisv_deck", !string.IsNullOrEmpty(this.bdeck) && this.bdeck.ToLower() == "true" ? 1 : 0);
                            comando.Parameters.AddWithValue("@aisv_cupo", !string.IsNullOrEmpty(this.acupo) && this.acupo.ToLower() == "true" ? 1 : 0);
                            comando.Parameters.AddWithValue("@aisv_gkey", this.gkey);


                            if (!string.IsNullOrEmpty(this.selloid))
                            {
                                comando.Parameters.AddWithValue("@aisv_sellado", this.sellor);
                                comando.Parameters.AddWithValue("@aisv_sellado_ci", this.selloid);
                            }

                            //TRANSPORTE
                            comando.Parameters.AddWithValue("@aisv_tran_cia", this.trancia);
                            comando.Parameters.AddWithValue("@aisv_tran_ruc", this.tranruc);

                            //ARRIBO TARDÍO AÑADIR EL CAMPO LATE ARRIVAL
                            comando.Parameters.AddWithValue("@aisv_late", !string.IsNullOrEmpty(this.late) && this.late.ToLower() == "true" ? 1 : 0);

                            //@aisv_proforma 
                            if (!string.IsNullOrEmpty(this.prosequence))
                            {
                                comando.Parameters.AddWithValue("@aisv_proforma", this.prosequence);
                            }

                            //nuevo 2017->campos adicionales
                            comando.Parameters.AddWithValue("@aisv_diplomatico", !string.IsNullOrEmpty(this.diplomatico) && this.diplomatico.ToLower() == "true" ? 1 : 0);
                            comando.Parameters.AddWithValue("@aisv_consignatario", this.consignatario);
                            //->Nuevo-->

                            var xdt = false;
                            var dtcomand = conexion.CreateCommand();
                            if (this.detalles != null && this.detalles.Count > 1)
                            {
                                xdt = true;
                                StringBuilder xml = new StringBuilder();
                                dtcomand.CommandType = CommandType.StoredProcedure;
                                dtcomand.CommandText = "sp_add_aisv_detalle";
                                dtcomand.CommandTimeout = 60;
                                xml.Append("<detalle>");
                                foreach (var t in this.detalles)
                                {
                                    xml.AppendFormat("<item  dae=\"{0}\" embalaje=\"{1}\" bultos=\"{2}\"  peso=\"{3}\"  tipo=\"{4}\"  aisv=\"{5}\" usuario=\"{6}\"  />", t.adudoc, t.embalaje, t.bultos, t.peso, t.tipodoc, this.secuencia, this.autor);
                                }
                                xml.Append("</detalle>");
                                dtcomand.Parameters.AddWithValue("@xml", xml.ToString());

                            }
                            //aqui viene lo nuevo de la inserción.
                            conexion.Open();
                            if (xdt)
                            {
                                var t = dtcomand.ExecuteScalar();
                                var intres = -2;
                                if (t != null && t.GetType() != typeof(DBNull))
                                {
                                    intres = int.Parse(t.ToString());
                                }
                                if (intres < 0)
                                {
                                    number = "La inserción de los detalles ha fallado";
                                    return false;
                                }
                                comando.Parameters.AddWithValue("@aisv_detalle", true);
                                comando.Parameters.AddWithValue("@aisv_detalle_direcc", this.direccion);
                            }
                            comando.ExecuteNonQuery();
                            conexion.Close();
                            number = this.secuencia;
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
                        var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "add", this.secuencia, this.autor);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "add", this.secuencia, this.autor);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

         public static bool S3Solicitud(string login, string secuencia, string booking, string referencia,string dae,string contenedor, string mail, DateTime cutOff, DateTime maxCutOff,    out string number)
        {
            try
            {
                initdataconf();
                //CREAR EL PREAVISO, O BL DEPENDIENDO DEL TIPO DE AISV.
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sca_inserta_sol_la";
                            comando.Parameters.AddWithValue("@codserv", "LA");
                            comando.Parameters.AddWithValue("@login", login);
                            comando.Parameters.AddWithValue("@aisv", secuencia);
                            comando.Parameters.AddWithValue("@boking", booking);
                            comando.Parameters.AddWithValue("@referencia", referencia);
                            comando.Parameters.AddWithValue("@dae ", dae);
                            comando.Parameters.AddWithValue("@fechaCutoff", cutOff);
                            comando.Parameters.AddWithValue("@MaxfechaCutoff ", maxCutOff);
                            comando.Parameters.AddWithValue("@contenedor", contenedor);
                            comando.Parameters.AddWithValue("@mail ", mail);

                            //aqui viene lo nuevo de la inserción.
                            conexion.Open();
                            var oexec=  comando.ExecuteReader( CommandBehavior.CloseConnection);
                            number = string.Empty;
                            while(oexec.Read())
                            {
                                number = oexec[0] != null ? oexec[0].ToString():string.Empty;
                            }
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
                        var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "S3Solicitud", secuencia, login);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "S3Solicitud", secuencia, login);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }

        //anula aisv
        public static bool delete(string aisv, string usuario, out string mensaje)
        {
            try
            {

                initdataconf();
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_del_aisv";
                            comando.Parameters.AddWithValue("@aisv", aisv);
                            comando.Parameters.AddWithValue("@usuario", usuario);
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
                        var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "delete", aisv, "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "delete", aisv, "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        //establecer aisv
        public bool TransaportToN4(ObjectSesion user, out string mensaje)
        {
            //indica si la unidad deberíia revisar disponibilidad
            bool checkUnit = false;
            var t1 = this.tipo.Trim().ToCharArray();
            //es full de golpe y porrazo a revisar
            if (t1[1] == 'C')
            {
                checkUnit = true;
            }
            //si tiene numero de container y ademas es de acopio verificar todo
            if (!string.IsNullOrEmpty(this.unumber) && !string.IsNullOrEmpty(this.breferencia) && this.breferencia.Trim().ToUpper().Contains("CGS2008001"))
            {
                checkUnit = true;
            }
            //1.-OBTENER LA SECUENCIA-----> aqui secuencia
            this.secuencia = GetSecuence();
            //NUEVO OBTENER LA PROFORMA A LA QUE PERTENECE


            //2.- f Es la variable que obtenedrá el serializado del objeto
            var f = string.Empty;
            Int64 errorNumber = 0;

            mensaje = this.secuencia;
            user.transaccion = this.secuencia;
            if (string.IsNullOrEmpty(this.secuencia))
            {
                mensaje = "*Lo sentimos, algo salió mal.*\nEstamos trabajando para solucionarlo lo más pronto posible, fué imposible generar la secuencia";
                return false;
            }
            //si es container y no tiene unidad chao...
            if (checkUnit & string.IsNullOrEmpty(this.unumber))
            {
                mensaje = "*Datos del contenedor*\nDebe escribir el número de la unidad a procesar";
                return false;
            }
            //Si es container/ACOPI0 validar disponibilidad de items en el BOOKIG
            if (checkUnit)
            {
                if (!Inventarios(this.bnumber, this.idline, this.gkey))
                {
                    mensaje = "*Datos del Booking:*\nEl item de booking seleccionado, ya no tiene disponibilidad, por favor elija otro item o booking";
                    return false;
                }
            }
            //intentar envío de la unidad.
            var unidadok = true; //indica que la unidad se creó en N4
            var preaviso = new UnitAdvice();
            //crear la unidad unicamente si container 
            var webService = new n4WebService();
            if (checkUnit)
            {
                preaviso.createdby = user.usuario;
                preaviso.category = category.EXPORT;
                preaviso.transitstate = transitstate.INBOUND;
                preaviso.freightkind = tipo.Contains("EC") ? freightkind.FCL : freightkind.LCL;
                preaviso.line = this.idline;
                preaviso.id = unumber.Trim().ToUpper();
                var noexiste = UnitIsCreated(preaviso.id);
                //caso->no existe la unidad se va a crear pero resulta que no hay iso del booking
                if (noexiste == false && string.IsNullOrEmpty(this.btipou.Trim()))
                {
                    mensaje = "*Datos del contenedor*\nEl Booking no tiene especificado el tipo/iso del contenedor";
                    return false;
                }
                preaviso.equipment = null;
                //si no existe crear el equipo.
                if (noexiste == false)
                {
                    preaviso.equipment = new equipment();
                    preaviso.equipment.clase = "CTR";
                    preaviso.equipment.role = "PRIMARY";
                    preaviso.equipment.eqid = this.unumber;
                    preaviso.equipment.tipo = this.btipou;
                    preaviso.equipment.ownership.owner = this.idline;
                    preaviso.equipment.ownership.operador = this.idline; 
                }
                //si es reefer añadir todo lo que sea de reefer
                if (this.breefer.ToLower() == "true")
                {
                    //refeer equipo si es que existe el equipo efectivamente.
                    if (noexiste && preaviso.equipment != null)
                    {
                        preaviso.equipment.reefer.rfrtype = refertype.INTEG_AIR;
                        preaviso.equipment.reefer.iscontrolledatmosphere = "N";
                        preaviso.equipment.reefer.isstarvent = "N";
                        preaviso.equipment.reefer.issuperfreeze = "N";
                        preaviso.equipment.reefer.istemperaturecontrolled = "Y";
                    }
                    //refeer de unidad, esto si vienen datos...
                    if (!string.IsNullOrEmpty(this.utemp))
                    {
                        preaviso.reefer.tempreqdc = this.utemp;
                        preaviso.reefer.tempmaxc = this.utemp;
                        preaviso.reefer.tempdisplayunit = "C";
                        preaviso.reefer.extendedtimemonitors = "N";
                        preaviso.reefer.tempminc = this.utemp;
                        preaviso.reefer.ispower = "N";
                    }
                    if (!string.IsNullOrEmpty(this.ventu))
                    {
                       preaviso.reefer.ventrequiredunit = this.ventu;
                       preaviso.reefer.ventrequiredvalue = this.vent_pc;
                    }
                    if (!string.IsNullOrEmpty(this.uhumedad))
                    {
                        preaviso.reefer.humiditypct = this.uhumedad;
          
                    }
                    preaviso.reefer.wantedispower = "Y";
                }
                else
                {
                    if (preaviso.equipment != null)
                    {
                        preaviso.equipment.reefer = null;
                    }
                    preaviso.reefer = null;
                }

                //SPECIAL STOW 3-->matha ramos desactiva stow/ Pablo triviño sube stow 04/02/2015
                //if (!string.IsNullOrEmpty(this.bdeck) && this.bdeck.ToLower() == "true")
                //{
                //    var st = GetStow(this.bnumber);
                //    if (!string.IsNullOrEmpty(st))
                //    {
                //        var stw = new stow();
                //        stw.id = st;
                //        preaviso.specialstow.Add(stw);
                //    }
     
                //}
                //si es contenedor de exportación se añade el IMO de boking y el stow.

                if (tipo.Contains("EC"))
                {
                    var hz = 0;
                    if (!string.IsNullOrEmpty(this.hzkey) && int.TryParse(this.hzkey, out hz))
                    {
                        //obtener la lista de Imos/unum
                        foreach (var item in CslHelper.GetHzList(hz))
                        {
                            var haz = new hazard();
                            haz.imdg = item.Item1;
                            haz.un = item.Item2;
                            preaviso.hazards.Add(haz);
                        }
                    }
                    //esto es del stow lo unico nuevo.
                    var st = GetStow(this.bnumber);
                    if (!string.IsNullOrEmpty(st))
                    {
                        var stw = new stow();
                        stw.id = st;
                        preaviso.specialstow.Add(stw);
                    }

                    //si no es de consolidaciones
                    if (this.detalles == null || this.detalles.Count <= 0)
                    {
                        this.prosequence = ObtenerProforma(this.bnumber);
                    }
                }

                
                preaviso.routing.pol = "ECGYE";
                preaviso.routing.pod1 = (this.bpod != null && !this.bpod.Contains("NA")) ? this.bpod : null;
                preaviso.routing.pod2 = (this.bpod1 != null && !this.bpod1.Contains("NA")) ? this.bpod1 : null;

                if (!string.IsNullOrEmpty(this.late) && this.late.ToLower().Contains("true"))
                {
                    preaviso.routing.group = "LA";
                }

                //routing
                var car = new carrier();
                car.direction = "IB"; car.qualifier = "DECLARED"; car.mode = "TRUCK";
                preaviso.routing.carrier.Add(car);
                car = new carrier();
                car.direction = "IB"; car.qualifier = "ACTUAL"; car.mode = "TRUCK";
                preaviso.routing.carrier.Add(car);
                car = new carrier();
                car.direction = "OB"; car.qualifier = "DECLARED"; car.mode = "VESSEL"; car.id = this.breferencia;
                preaviso.routing.carrier.Add(car);
                car = new carrier();
                car.direction = "OB"; car.qualifier = "ACTUAL"; car.mode = "VESSEL"; car.id = this.breferencia;
                preaviso.routing.carrier.Add(car);
                //booking
                preaviso.booking.id = this.bnumber;
                //excesos
                if (!string.IsNullOrEmpty(this.eHas) && this.eHas.ToLower() == "true")
                {
                    preaviso.oog.backcm = this.eback;
                    preaviso.oog.topcm = this.etop;
                    preaviso.oog.frontcm = this.efront;
                    preaviso.oog.rightcm = this.eright;
                    preaviso.oog.leftcm = this.eleft;
                }
                else
                {
                    preaviso.oog = null;
                }


                //notas de la creacion de la unidad.
                preaviso.handling.remark = string.Format("Unidad preavisada por el usuario {0}, desde CSL", this.autor);
                //pesos
                decimal tx = 0;
                decimal tara = 0;
                CultureInfo enUS = new CultureInfo("en-US");
                //el stylo de numero normal
                NumberStyles style = NumberStyles.Number;

                string pesos = upeso.Replace(",", ".");
                string taras = utara.Replace(",", ".");

                preaviso.contents.weightkg = "0";


                if (!string.IsNullOrEmpty(taras))
                {
                    if (decimal.TryParse(taras, style, enUS, out tara))
                    {
                        tara = tara * 1000;
                    }
                    else
                    {
                        mensaje = "La conversión de la tara ha fallado";
                        return false;
                    }
                }
                //reemmplazar el punto por coma
                if (!string.IsNullOrEmpty(pesos) && decimal.TryParse(pesos, style, enUS, out tx))
                {
                    //si es container el peso x 1000, si es otra carga el peso normal, le reemplazo , x .
                    if (this.detalles != null && this.detalles.Count > 0)
                    {
                        tx = 0;
                        foreach (var p in detalles)
                        {
                            tx += p.peso != null ? decimal.Parse(p.peso, enUS) : 0;
                        }
                        tx += tara;
                        preaviso.contents.weightkg = tx.ToString("G").Replace(",", ".");
                        this.upeso = preaviso.contents.weightkg;
                    }
                    else
                    {
                        if (!tipo.Contains("EC"))
                        {
                            tx = tx / 1000;
                        }
                        preaviso.contents.weightkg = (tara + (tx * 1000)).ToString("G").Replace(",", ".");
                    }
                }
                //mensaje = string.Format("PESO: {0}| TARA: {1} | SUMA={2}| N4={3}", tx * 1000, tara, (tx*1000) + tara, preaviso.contents.weightkg);
                //return false;
                preaviso.contents.shipperid = this.idexport;
                //sellos de la unidad
                preaviso.sello.seal1 = this.seal1;
                preaviso.sello.seal2 = !string.IsNullOrEmpty(this.seal3) ? this.seal3 : null;
                preaviso.sello.seal3 = !string.IsNullOrEmpty(this.seal4) ? this.seal4 : null;
                preaviso.sello.seal4 = !string.IsNullOrEmpty(this.seal2) ? this.seal2 : null;
                preaviso.unitflex.flex2 = this.secuencia;


                //nuevo--->@aisv_proforma 
                if (!string.IsNullOrEmpty(this.prosequence))
                {
                    preaviso.unitflex.flex14 = this.prosequence;
                }
                

                //aqui la dae de la unidad
                preaviso.unitflex.flex1 = !string.IsNullOrEmpty(this.adocnumero) ? this.adocnumero : "Sin DAE";


                preaviso.unitflex.flex11 = tipo.Contains("EC") ? this.nomexpo : string.Empty;

                //Esto para consolidación--->NO LE PONGO DAE NI EXPORTADOR
                if (this.breferencia.Contains("CGS2008001"))
                {
                    preaviso.unitflex.flex1 = string.Empty;
                    preaviso.contents.shipperid = string.Empty;
                    preaviso.unitflex.flex11 = string.Empty;
                }

                //Validación 6 -> Serialización de la unidad
                f = xmlHelper.SerializeAsString<UnitAdvice>(preaviso, out errorNumber);

                if (string.IsNullOrEmpty(f) && tipo.Contains("EC"))
                {
                    mensaje = string.Format("*Lo sentimos, algo salió mal.*\nEstamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio X00-{0}", errorNumber);
                    return false;
                }

                //Validación del pre-aviso->3 chao
                var n4estado = webService.InvokeN4Service(user, f, ref mensaje, this.secuencia);
                //verificar la unidad
                //si dio Ok confirmelo->falsos positivos
                if (n4estado == 0)
                {
                    if (!ConfirmacionPreaviso(this.unumber))
                    {
                        mensaje = string.Format("La unidad [{0}] no pudo ser preavisada debido a una demora en la solicitud al sistema interno\n, por favor espere 1 minuto y vuelva intentarlo \no consulte con el depto. de planificación", this.unumber);
                        return false;
                    }

                    //AQUI DESPUES QUE LA UNIDAD SE AVISÓ, ademas debe venir marcado el AISV
                    bool latear = false;
                    if (!string.IsNullOrEmpty(this.late) && this.late.ToLower().Contains("true"))
                    {
                        latear = true;
                    }
                    if (this.bfk.Contains("FCL") && latear)
                    {
                        DateTime fecha;
                        if (!DateTime.TryParse(this.bcutOff, out fecha))
                        {
                            mensaje = string.Format("La unidad [{0}] no pudo ser preavisada debido a un error en la fecha de CutfOff (Late Arrival) \n,consulte con el depto. de planificación antes de reintentarlo", this.unumber);
                            return false;
                        }
                        var cutof = fecha;
                        string solnum = string.Empty;                                                 //numeroHorasMaxima
                        var fmax= fecha.AddHours(System.Configuration.ConfigurationManager.AppSettings["numeroHorasMaxima"]!=null? 
                            double.Parse(System.Configuration.ConfigurationManager.AppSettings["numeroHorasMaxima"]):0);
                        var sm = groovyLA(unumber.Trim().ToUpper(), breferencia.Trim().ToUpper(), fmax);
                        //INSERTAR LAS SOLICITUD DE S3--> ENVIAR MAIL?
                        if (!S3Solicitud(this.autor, this.secuencia, this.bnumber, this.breferencia, this.adocnumero, this.unumber.Trim().ToUpper(), this.mail1, cutof, fmax , out solnum))
                        { 
                          //aqui fallo la insercion con solnum
                            log_csl.save_log<ApplicationException>(new ApplicationException(sm), "jAisv", "S3-ADSOL", this.secuencia, this.autor);
                        }
                        n4estado = webService.InvokeN4Service(user, sm, ref mensaje, this.secuencia);
                    }
                }
                //si es advertencia
                if (n4estado == 2)
                {
                    if (!ConfirmacionPreaviso(this.unumber))
                    {
                        mensaje = string.Format("La unidad [{0}] no pudo ser preavisada por la siguiente advertencia:[{1}]\n, el AISV no será generado, intente con otro contenedor \no consulte con el depto. de planificación", this.unumber, mensaje);
                        return false;
                    }

                }
                //si es 3->Chao
                if (n4estado > 2)
                {
                    unidadok = false;
                }
            }
            //No hay detalles es container FULL
            var isdtnull = detalles == null || detalles.Count <= 0;
            if (tipo.Contains("EC") && isdtnull)
            {
                return unidadok;
            }
            //caso->El contenedor no pudo preavisarce, asi que para que crear el BL
            if (checkUnit && unidadok == false)
            {
                mensaje = string.Format("El contenedor no pudo ser preavisado razón:[{0}]\n, el AISV no será generado, intente con otro contenedor \no consulte con el depto. de planificación", mensaje);
                return false;
            }
            //si no es container del golpe y porrazo el BL
            if (!tipo.Contains("EC"))
            {
                // es carga de consolidación.
                var billcarga = new billoflading();
                billcarga.nbr = this.secuencia;
                billcarga.category = category.EXPORT;
                billcarga.line = idline;
                billcarga.pol = "ECGYE";
                billcarga.pod1 = this.bpod;
                billcarga.pod2 = this.bpod1;
                billcarga.carriervisit = this.breferencia.Trim().ToUpper();
                billcarga.shipperid = this.idexport;
                billcarga.shippername = this.nomexpo;
                billcarga.enteredquantity = this.ubultos;
                billcarga.releasedquantity = this.ubultos;
                billcarga.notes = this.adocnumero;

                //blitem
                var item = new blitem();
                item.nbr = this.bnumber;
                item.isbulk = "N";
                item.quantity = this.ubultos;
                item.commodityid = this.bcomodity;
                item.marksandnumbers = this.adocnumero + "[" + this.adoctipo + "]";

                //si solo si es de consolidaciones, entonces multiplico el peso por el valor de unidad carga.
                if (tipo.Contains("CS"))
                {
                    int dy = 0; decimal xxp = 0;
                    if (int.TryParse(this.ubultos, out dy) && decimal.TryParse(this.upeso,out xxp ))
                    {
                        this.upeso = (dy * xxp).ToString();
                    }
                }
                //cambio por el peso unitario


                item.weighttotalkg = this.upeso.Replace(",", ".");
                //cargolote
                var cargolote = new cargolot();
                cargolote.lotid = this.secuencia;

                cargolote.quantity = this.ubultos;
                cargolote.quantitymanifiested = cargolote.quantity;
                //bl.->Peso mas 
                cargolote.lotweighttotalkg = this.upeso.Replace(",", ".");

                // cargolote.isdefaultlot = "Y";
                if (this.breferencia.Trim().ToUpper().Contains("CGS2008001") && !string.IsNullOrEmpty(this.unumber))
                {
                    cargolote.isdefaultlot = "N";
                    cargolote.position.loctype = locacion.CONTAINER;
                    cargolote.position.location = this.unumber;
                }
                else
                {
                    cargolote.isdefaultlot = "Y";
                    cargolote.position.loctype = locacion.TRUCK;
                    cargolote.position.location = this.breferencia;
                }
                //añadir
                item.cargolots.Add(cargolote);
                item.notes = string.Format("Creado en csl, por usuario:{0}", this.autor);
                //si no es containers, entonces si la unidad se preavisó...(existe/se creo)
                if (unidadok && checkUnit)
                {
                    goodsbl gl = new goodsbl();
                    gl.unitid = this.unumber;
                    // gl.unitkey = this.unumber;
                    billcarga.goodsbl.Add(gl);
                }
                billcarga.items.Add(item);
                //Validación 6 -> Serialización
                f = xmlHelper.SerializeAsString<billoflading>(billcarga, out errorNumber);
                //comprobar si la serialización salió bien.
                if (string.IsNullOrEmpty(f))
                {
                    mensaje = string.Format("*Lo sentimos, algo salió mal.*\nEstamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio X00-{0}", errorNumber);
                    return false;
                }
                //de una enviar el bl sino para que seguir?
                if (!string.IsNullOrEmpty(f) && f.Trim().Length > 0)
                {
                    var nr = webService.InvokeN4Service(user, f, ref mensaje, this.secuencia);
                    if (nr >= 2)
                    {
                        //si no se creo para qe enviar el grooovy
                        return false;
                    }
                    //aplicar groovy de pesos, si es que el bl's se creó
                    webService.InvokeN4Service(user, setPesos(this.upeso, this.secuencia), ref mensaje, this.secuencia);
                }
            }
            return true;
        }
        //confirma con un ICU el numero de AISV
        public bool ConfirmUnitAISV(ObjectSesion user, string aisvnumber, out string mensaje)
        {
            var webService = new n4WebService();
            var f = XmlTool.xmlHelper.ICUSetProperty(this.breferencia, this.unumber, "UnitFlexString02", aisvnumber);
            mensaje = string.Empty;
            if (webService.InvokeN4Service(user, f, ref mensaje, "ICU N4") != 0)
            {
                return false;
            }
            return true;
        }
        //valida toda la infromación de un aisv
        public static bool ValidateAisvData(jAisvContainer aisv, out string validacionError)
        {
            decimal peso = 0; decimal tarx = 0; bool checkUnit = false;
            //si el tipo no vino
            if (string.IsNullOrEmpty(aisv.tipo) || aisv.tipo.Trim().Length != 2)
            {
                validacionError = "AISV:\nTipo de aisv inválido";
                return false;
            }

            var t1 = aisv.tipo.Trim().ToCharArray();
            //es container lleno, verificar todos los candados
            if (t1[1] == 'C')
            {
                checkUnit = true;
                if (!aisv.bfk.Contains("FCL"))
                {
                    validacionError = "*Datos del booking*\nEl tipo de booking no es coherente con el tipo de aisv";
                    return false;
                }
            }
            //solo si es acopio y no hay container-->Obligar el contenedor
            if (aisv.breferencia.Trim().ToUpper().Contains("CGS2008001") && string.IsNullOrEmpty(aisv.unumber))
            {
                validacionError = "*Datos del booking*\nDebe agregar el contenedor de acopio";
                return false;
            }

            //si tiene numero de container y ademas es de acopio verificar todo
            if (!string.IsNullOrEmpty(aisv.unumber) && !string.IsNullOrEmpty(aisv.breferencia) && aisv.breferencia.Trim().ToUpper().Contains("CGS2008001"))
            {
                checkUnit = true;
            }

            //bnumber*
            if (string.IsNullOrEmpty(aisv.bnumber))
            {
                validacionError = "*Datos del booking*\nNo ha seleccionado el Booking";
                return false;
            }
            //idexport*
            if (string.IsNullOrEmpty(aisv.idexport))
            {
                validacionError = "*Datos del booking*\nSeleccione el exportador asociado";
                return false;
            }

           
            //nomexpo
            if (string.IsNullOrEmpty(aisv.nomexpo))
            {
                validacionError = "*Datos del booking*\nSeleccione el exportador asociado";
                return false;
            }

            //nomexpo
            if (string.IsNullOrEmpty(aisv.consignatario))
            {
                validacionError = "*Datos de Aduana*\nAgregue el nombre del consginatario de la carga";
                return false;
            }

            //la cultura del server
            CultureInfo enUS = new CultureInfo("en-US");
            //el stylo de numero normal
            NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
            DateTime fecha;
            decimal numero = 0;
            bool booleano = false;
            //breserva
            //aisv_b_reserva
            if (string.IsNullOrEmpty(aisv.breserva) || !decimal.TryParse(aisv.breserva, out  numero))
            {
                aisv.breserva = "0";
            }
            //gkey*
            if (string.IsNullOrEmpty(aisv.gkey) || !Decimal.TryParse(aisv.gkey, out numero) || numero <= 0)
            {
                validacionError = "*Datos del booking*\nSeleccione un item de booking";
                return false;
            }
            //Eta*
            if (!DateTime.TryParseExact(aisv.beta.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
            {
                validacionError = "*Datos del booking*\nLa fecha de ETA no tiene el formato correcto";
                return false;
            }
            aisv.beta = fecha.ToString("yyyy/MM/dd HH:mm");
            //CutOff*
            if (DateTime.TryParseExact(aisv.bcutOff.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
            {
                if (DateTime.Now > fecha)
                {
                    validacionError = "*Datos del booking*\nYa no es posible usar este booking, la fecha de límite (CutOff) se ha cumplido";
                    return false;
                }
                aisv.bcutOff = fecha.ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                validacionError = "*Datos del booking*\nLa fecha de CutOff no tiene el formato correcto";
                return false;
            }
            //UIS->si no vino yo la agrego
            if (!DateTime.TryParseExact(aisv.buis.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
            {
                aisv.buis = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            }
            aisv.buis = fecha.ToString("yyyy/MM/dd HH:mm");
            //Agente->sino vino le pongo no aplica, añadirlo a cátalogo
            if (string.IsNullOrEmpty(aisv.aidagente))
            {
                aisv.aidagente = "-1";

            }
            //idubicacion*
            if (string.IsNullOrEmpty(aisv.tidubica) || !Decimal.TryParse(aisv.tidubica, out numero) || numero <= 0)
            {
                validacionError = "*Datos del transporte*\nSeleccione la provincia";
                return false;
            }
            //horas*
            if (string.IsNullOrEmpty(aisv.thoras) || !Decimal.TryParse(aisv.thoras, out numero))
            {
                validacionError = "*Datos del transporte*\nEscriba la cantidad de horas de viaje";
                return false;
            }
            //GUARDAR EN BASE DE DATOS -> hzkey*
            if (!Decimal.TryParse(aisv.hzkey, out numero))
            {
                aisv.hzkey = "0";
            }
            //idcanton*
            if (string.IsNullOrEmpty(aisv.tidcanton) || !Decimal.TryParse(aisv.tidcanton, out numero) || numero <= 0)
            {
                validacionError = "*Datos del transporte*\nSeleccione el cantón";
                return false;
            }
            //upeso*->dit
            if (string.IsNullOrEmpty(aisv.upeso) || !Decimal.TryParse(aisv.upeso, style, enUS, out peso) || peso <= 0)
            {
                validacionError = "*Unidad/Cargas*\nEscriba el peso de la unidad de carga";
                return false;
            }
            ////Documento de aduana*
            //if (string.IsNullOrEmpty(aisv.adocnumero) || aisv.adocnumero.Trim().Length <= 10)
            //{
            //    validacionError = "*Datos de aduana*\nRevise el número de documento";
            //    return false;
            //}
            //documento de aduana nuevo---->
            var msa = string.Empty;
            if (aisv.detalles == null || aisv.detalles.Count <= 0)
            {
                if (!DataTransformHelper.ValidarAduDoc(aisv.adocnumero, aisv.adoctipo, out msa))
                {
                    validacionError = string.Format("*Datos de aduana*\n{0}", msa);
                    return false;
                }
                aisv.adocnumero = aisv.adocnumero.Trim().Replace("-", string.Empty);
                aisv.adocnumero = aisv.adocnumero.Replace(" ", string.Empty);
            }
            //Deck-Undeck
            if (!string.IsNullOrEmpty(aisv.bdeck) && !Boolean.TryParse(aisv.bdeck, out booleano))
            {
                validacionError = "La propiedad cubierta no tiene un valor válido";
                return false;
            }
            //Usa Cupos?
            if (!string.IsNullOrEmpty(aisv.acupo))
            {
                //Se pudo convertir el booleno
                if (Boolean.TryParse(aisv.acupo, out booleano))
                {
                    //ok si usa cupos
                    if (booleano)
                    {
                        //->id institución se debe validar si usa cupos
                        if (string.IsNullOrEmpty(aisv.aidinst) || !Decimal.TryParse(aisv.aidinst, out numero) || numero <= 0)
                        {
                            validacionError = "*Datos de aduana*\nEstablezca la entidad que controla los cupos";
                            return false;
                        }
                        //->id regla se debe validar si usa cupos
                        if (string.IsNullOrEmpty(aisv.aidrule) || !Decimal.TryParse(aisv.aidrule, out numero) || numero <= 0)
                        {
                            validacionError = "*Datos de aduana*\nEscoja la regla o porcentaje que aplica";
                            return false;
                        }
                    }
                }
                else
                {
                    validacionError = "La propiedad cupos no tiene un valor válido";
                    return false;
                }
            }
            //fecha* anio mes día
            if (!DateTime.TryParseExact(aisv.tfechadoc.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
            {
                validacionError = "*Datos del transporte*\nLa fecha de salida de planta no tiene un formato válido";
                return false;
            }
            aisv.tfechadoc = fecha.ToString("yyyy/MM/dd HH:mm");
            //fecha anio mes->Cabecera
            if (!string.IsNullOrEmpty(aisv.tcabcertfecha))
            {
                if (!DateTime.TryParseExact(aisv.tcabcertfecha.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
                {
                    validacionError = "*Datos del transporte*\nLa propiedad fecha de expiración de certificado cabezal no tiene un formato válido dd/MM/aaaa";
                    return false;
                }
                aisv.tcabcertfecha = fecha.ToString("yyyy/MM/dd");
            }
            //fecha anio mes->Chasis
            if (!string.IsNullOrEmpty(aisv.tcabchafecha))
            {
                if (!DateTime.TryParseExact(aisv.tcabchafecha.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
                {
                    validacionError = "*Datos del transporte*\nLa propiedad fecha de expiración de certificado chasis no tiene un formato válido";
                    return false;
                }
                aisv.tcabchafecha = fecha.ToString("yyyy/MM/dd");
            }
            //Nombre del conductor *
            if (string.IsNullOrEmpty(aisv.tconductor))
            {
                validacionError = "*Datos del transporte*\nEscriba el nombre del conductor";
                return false;
            }
            //documento del conductor *
            if (string.IsNullOrEmpty(aisv.tdocument))
            {
                validacionError = "*Datos del transporte*\nEscriba el No. documento del conductor";
                return false;
            }
            //numero de placa del carro *

            if (string.IsNullOrEmpty(aisv.tplaca))
            {
                validacionError = "*Datos del transporte*\nEscriba el número de placa del vehículo";
                return false;
            }
            else
            {
                var ar = aisv.tplaca.Trim().ToCharArray();
                if (aisv.tplaca.Length <= 6)
                {
                    validacionError = "*Datos del transporte*\nEl número de placa del vehículo debe contener al menos 3 letras y 4 numeros";
                    return false;
                }
                var con = 1;
                foreach (var c in ar)
                {
                    if (con >= 1 & con < 4)
                    {
                        if (!Char.IsLetter(c))
                        {
                            validacionError = "*Datos del transporte*\nEl número de placa del vehículo debe contener al menos 3 letras";
                            return false;
                        }
                    }
                    if (con >= 4 & con < 8)
                    {
                        if (!Char.IsNumber(c))
                        {
                            validacionError = "*Datos del transporte*\nEl número de placa del vehículo debe contener al menos 4 dígitos, si falta un dígito anteponga el cero";
                            return false;
                        }
                    }
                    con++;
                }
            }

            //correo electronico *
            if (string.IsNullOrEmpty(aisv.mail1))
            {
                validacionError = "*Datos para notificación*\nEscriba al menos un email válido";
                return false;
            }
            #region "Validar Carga suelta"
            if (t1[1] == 'S')
            {

                if (string.IsNullOrEmpty(aisv.bcomodity))
                {
                    validacionError = "*Datos del Booking *\nPor favor comuníquese con planificación, el booking no tiene configurado un comodity ";
                    return false;
                }
                //Carga suelta/consolidar obligado->cantidad de bultos
                if (string.IsNullOrEmpty(aisv.ubultos) || !Decimal.TryParse(aisv.ubultos, style, enUS, out numero))
                {
                    validacionError = "*Carga*\nEscriba la cantidad de bultos";
                    return false;
                }
                //Carga suelta/consolidar obligado->imo
                if (string.IsNullOrEmpty(aisv.cimo))
                {
                    validacionError = "*Carga*\nSeleccione el imo de la lista";
                    return false;
                }

                //Carga suelta/consolidar obligado->embalaje
                if (string.IsNullOrEmpty(aisv.cembalaje))
                {
                    validacionError = "*Carga*\nSeleccione el tipo de embalaje";
                    return false;
                }

            }
            #endregion
            #region "Validar Unidad"
            if (checkUnit)
            {
               //nuevo sello de sticker
                if (aisv.idline.Trim().ToLower().Contains("msc") && string.IsNullOrEmpty(aisv.seal3))
                {
                    validacionError = "*Detalle de sellos*\nAgregue el sello de STICKER, en caso de que no exista escriba \"sin sello\"";
                    return false;
                }

                //Container obligado-> numero
                if (string.IsNullOrEmpty(aisv.unumber))
                {
                    validacionError = "*Datos del contenedor*\nEscriba el número del contenedor";
                    return false;
                }
                //container number
                if (!DataTransformHelper.ContainerBasicValidate(aisv.unumber))
                {
                    validacionError = "*Datos del contenedor*\nEl número del contenedor no cumple formato estándar XXXX0000000";
                    return false;
                }

                //Container obligado-> numero
                if (string.IsNullOrEmpty(aisv.seal1))
                {
                    validacionError = "*Datos del contenedor*\nEscriba el sello de agencia";
                    return false;
                }

                //---Nuevas validaciones de sellado
                //responsable del los sellos de la unidad... nuevo 2015
                if (aisv.tipo.Contains("EC"))
                {
                    if (string.IsNullOrEmpty(aisv.sellor) || string.IsNullOrEmpty(aisv.selloid) || aisv.selloid.Trim().Length <= 5)
                    {
                        validacionError = string.Format("*Datos del contenedor*\nEscriba el nombre y el no. de identificación de la persona\n responsable por el sellado de la unidad {0}, sello {1} ", aisv.unumber, aisv.seal1);
                        return false;
                    }
                }
                //-------------------------------
                //fecha deposito
                if (string.IsNullOrEmpty(aisv.udepo) || !Decimal.TryParse(aisv.udepo, out numero) || numero <= 0)
                {
                    validacionError = "*Datos del contenedor*\nEscoja un depósito de la lista, si no está, escoja otros";
                    return false;
                }
                //fecha deposito
                if (string.IsNullOrEmpty(aisv.udepofecha) || !DateTime.TryParseExact(aisv.udepofecha.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
                {
                    validacionError = "*Datos del contenedor*\nEscriba una fecha correcta para la salida del depósito";
                    return false;
                }
                aisv.udepofecha = fecha.ToString("yyyy/MM/dd HH:mm");
                //aqui se valida el intento de pisotón
                var xmens = string.Empty;

                if (UnidadEnDeposito(aisv.unumber) == false)
                {
                    validacionError = "*Datos del contenedor*\nEl contenedor ya se encuentra en los patios o sobreaviso, consulte con planificación";
                    return false;
                }
                if (!CslHelper.Unitproceed(aisv.unumber, aisv.bnumber, out xmens))
                {
                    validacionError = string.Format("*Datos del contenedor*\n{0}\n", xmens);
                    //intento de pisoton!!!
                    var car = new CSLSite.unitService.mailserviceSoapClient();
                    car.catchOverwrite(aisv.unumber.Trim(), aisv.bnumber.Trim(), aisv.autor, aisv.mail1, "notokenrequest");
                    return false;
                }
                //Container opcional-> imo de booking
                if (!string.IsNullOrEmpty(aisv.bimo) && !Boolean.TryParse(aisv.bimo, out booleano))
                {
                    validacionError = "*Datos del booking*\nLa propiedad IMO no tiene un valor válido";
                    return false;
                }
                //Container opcional-> oversize de booking
                if (!string.IsNullOrEmpty(aisv.bover) && !Boolean.TryParse(aisv.bover, out booleano))
                {
                    validacionError = "*Datos del booking*\nLa propiedad Oversize no tiene un valor válido";
                    return false;
                }
                //Container opcional-> reefer de booking
                if (!string.IsNullOrEmpty(aisv.breefer))
                {
                    //si se puede convertir su contenido
                    if (Boolean.TryParse(aisv.breefer, out booleano))
                    {
                        // si dice que si
                        if (booleano)
                        {
                            //Sello*
                            //si es msc y el sello de ventilación es vacío retorne el mensaje.
                            if (aisv.idline.Trim().ToLower().Contains("msc") && string.IsNullOrEmpty(aisv.seal2))
                            {
                                validacionError = "*Detalle de sellos*\nAgregue el sello de ventilación, en caso de que no exista escriba \"sin sello\"";
                                return false;
                            }
                            //refrigeración*
                            if (string.IsNullOrEmpty(aisv.uidrefri) || !Decimal.TryParse(aisv.uidrefri, out numero))
                            {
                                validacionError = "*Datos de refrigeración*\nEstablezca el tipo de refrigeración";
                                return false;
                            }
                            //Temperatura*
                            if (string.IsNullOrEmpty(aisv.utemp) || !Decimal.TryParse(aisv.utemp, style, enUS, out numero))
                            {
                                validacionError = "*Datos de refrigeración*\nEstablezca la temperatura";
                                return false;
                            }
                            //Humedad*
                            if (!string.IsNullOrEmpty(aisv.uhumedad) && !Decimal.TryParse(aisv.uhumedad, out numero))
                            {
                                validacionError = "*Datos de refrigeración*\nEstablezca los CBM de humedad";
                                return false;
                            }
                            //Ventilacion*
                            if (!string.IsNullOrEmpty(aisv.uidventila) && !Decimal.TryParse(aisv.uidventila, out numero) && numero <= 0)
                            {
                                validacionError = "*Datos de refrigeración*\nEstablezca el porcentaje de ventilación";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        validacionError = "La propiedad Reefer no tiene un valor válido";
                        return false;
                    }
                }
                //Tiene Exceso
                if (!string.IsNullOrEmpty(aisv.eHas))
                {
                    if (Boolean.TryParse(aisv.eHas, out booleano))
                    {
                        if (booleano)
                        {
                            //left
                            if (string.IsNullOrEmpty(aisv.eleft) || !Decimal.TryParse(aisv.eleft, out numero))
                            {
                                validacionError = "*Detalle de excesos*\nEl valor de exceso a la izquierda, no es válido";
                                return false;
                            }
                            //right
                            if (string.IsNullOrEmpty(aisv.eright) || !Decimal.TryParse(aisv.eright, out numero))
                            {
                                validacionError = "*Detalle de excesos*\nEl valor de exceso a la derecha, no es válido";
                                return false;
                            }
                            //top
                            if (string.IsNullOrEmpty(aisv.etop) || !Decimal.TryParse(aisv.etop, out numero))
                            {
                                validacionError = "*Detalle de excesos*\nEl valor de exceso superior, no es válido";
                                return false;
                            }
                            //frontal
                            if (string.IsNullOrEmpty(aisv.efront) || !Decimal.TryParse(aisv.efront, out numero))
                            {
                                validacionError = "*Detalle de excesos*\nEl valor de exceso al frente, no es válido";
                                return false;
                            }
                            //back
                            if (!string.IsNullOrEmpty(aisv.eback) && !Decimal.TryParse(aisv.eback, out numero))
                            {
                                validacionError = "*Detalle de excesos*\nEl valor de exceso posterior, no es válido";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        validacionError = "La propiedad Excesos no tiene un valor válido";
                        return false;
                    }
                }
                //aqui
                //vaidaciones numéricas
                //->Tara*
                if (!string.IsNullOrEmpty(aisv.utara) && !Decimal.TryParse(aisv.utara, style, enUS, out tarx))
                {
                    validacionError = "*Datos del contenedor*\nLa propiedad Tara no tiene un valor numérico válido";
                    return false;
                }
                //->MaxPayLoad* ->Numero
                if (!string.IsNullOrEmpty(aisv.umaxpay) && !Decimal.TryParse(aisv.umaxpay, style, enUS, out numero))
                {
                    validacionError = "*Datos del contenedor*\nLa propiedad MaxPayLoad no tiene un valor numérico válido";
                    return false;
                }
                if (numero <= 0 || numero > 60)
                {
                    validacionError = "*Datos del contenedor*\nEl valor del MaxPayLoad debe ser mayor que 0 y menor que 60 ";
                    return false;
                }
                var convertido = aisv.tipo.Contains("EC") ? peso : peso / 1000;
                if (convertido > numero)
                {
                    validacionError = "*Datos del contenedor*\nEl valor del peso debe ser menor al MaxPayLoad";
                    return false;
                }
                //if (convertido < tarx)
                //{
                //    validacionError = "*Datos del contenedor*\nEl valor del peso del contenedor debe ser mayor a la tara (Ton)";
                //    return false;
                //}

                //NUEVA VALIDACIÓN DEL BOOKING
               // if (aisv.tipo.Contains("EC") && aisv.detalles== null)
                if (aisv.tipo.Contains("EC") )
                {
                    //es contenedor lleno?
                    if (!BookingTieneProforma(aisv.bnumber))
                    {
                        validacionError = "Estimado Cliente\nDebe realizar la proforma, por servicios de exportación, para el booking " + aisv.bnumber;
                        aisv.hasprof = "N";
                        return false;
                    }

                    //cantidad de aisv's activos
                    var cnt_aisv = app_start.ProformaHelper.AisvActivos(aisv.bnumber) + 1;
                    //cantidad proformada
                    var proformado = app_start.ProformaHelper.Totalproformas(aisv.bnumber);
                    if (cnt_aisv > proformado)
                    {
                        validacionError = "*Proformas*\n Para realizar este AISV necesita generar una nueva proforma.\nTotal en proforma:  " + proformado.ToString() + "\nAISV generados: " + (cnt_aisv - 1).ToString() + " (Activos)";
                        aisv.hasprof = "N";
                        return false;
                    }

                }
            }
            #endregion
            #region "ValidarDetalles"
            if (aisv.detalles != null)
            {
                if (string.IsNullOrEmpty(aisv.direccion))
                {
                    validacionError = "*Datos del Transporte*\nEscriba la dirección del área de consolidación";
                    return false;
                }

                if (aisv.detalles.Count < 2 || aisv.detalles.Count > 50)
                {
                    validacionError = "*Detalle documentos*\nSe requieren de 2 a 50 documentos de exportación";
                    return false;
                }
                foreach (var it in aisv.detalles)
                {
                    //valida cada item de información.
                    if (!decimal.TryParse(it.bultos, out numero) || numero <= 0)
                    {
                        validacionError = string.Format("*Detalle documentos*\nDocumento No.[{0}], la cantidad bultos debe ser un número mayor a 0", it.adudoc);
                        return false;
                    }
                    if (!decimal.TryParse(it.peso, out numero) || numero <= 0)
                    {
                        validacionError = string.Format("*Detalle documentos*\nDocumento No.[{0}], el peso total de los bultos debe ser un número mayor a 0", it.adudoc);
                        return false;
                    }
                    if (string.IsNullOrEmpty(it.embalaje))
                    {
                        validacionError = string.Format("*Detalle documentos*\nDocumento No.[{0}], debe seleccionar el tipo de embalaje", it.adudoc);
                        return false;
                    }
                    var xxmen = string.Empty;
                    if (!DataTransformHelper.ValidarAduDoc(it.adudoc, "DAE", out xxmen))
                    {
                        validacionError = string.Format("*Detalle documentos*\nDocumento No.[{0}], {1}", it.adudoc, xxmen);
                        return false;
                    }
                    it.adudoc = it.adudoc.Trim().Replace("-", string.Empty);
                    it.adudoc = it.adudoc.Replace(" ", string.Empty);
                }
            }
            #endregion

            //valida al chofer por su CI
            if (!string.IsNullOrEmpty(aisv.tdocument))
            {
                if (IsDriverLock(aisv.tdocument.Trim()))
                {
                    validacionError = string.Format("*Datos del transporte*\nEl conductor {0}, se encuentra bloqueado para acceder a la terminal, comuníquese con CGSA en horarios de oficina.", aisv.tconductor);
                    return false;
                }
            }
            //Validar la placa -> Marzo 2015
            if (!string.IsNullOrEmpty(aisv.tplaca))
            {
                if (IsTruckLock(aisv.tplaca.Trim()))
                {
                    validacionError = string.Format("*Datos del transporte*\nEl camión con placas {0}, se encuentra bloqueado para acceder a la terminal, comuníquese con CGSA en horarios de oficina.", aisv.tplaca);
                    return false;
                }
            }
            //nuevo compañia de trasnporte, se valida para todos
            if (string.IsNullOrEmpty(aisv.trancia) || string.IsNullOrEmpty(aisv.tranruc) || aisv.tranruc.Trim().Length<=5)
            {
                    validacionError = string.Format("*Datos del transporte*\nEscriba el nombre y RUC de la compañía de transporte a la que pertenece el vehículo placa: {0}", aisv.tplaca);
                    return false;
            }

            validacionError = string.Empty;
            return true;
        }
        //metodo para comprobar el item antes de guradar solo para containers
        public static bool Inventarios(string booking, string line, string gkey)
        {
            long key = 0;
            try
            {
                if (string.IsNullOrEmpty(gkey) || !long.TryParse(gkey, out key))
                {
                    var ex = new ApplicationException(string.Format("El parametro Gkey vino NULL: Booking:{0}|Linea:{1}|Key:{2}", booking, line, gkey));
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "Inventario", booking, "Portal");
                    return false;
                }
                using (var con = new System.Data.SqlClient.SqlConnection())
                {
                    con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"] != null) ? System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString : cadena;
                    var comando = con.CreateCommand();
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.Connection = con;
                    comando.CommandText = "select [dbo].[fxCountItem](@gkey)";
                    comando.Parameters.AddWithValue("@gkey", gkey);
                    try
                    {
                        con.Open();
                        var valor = comando.ExecuteScalar();
                        con.Close();
                        int counte = 0;
                        if (valor != null && valor.GetType() != typeof(DBNull))
                        {
                            counte = int.Parse(valor.ToString());
                        }
                        else
                        {
                            log_csl.save_log<Exception>(new ApplicationException("El valor de dispone era NULL"), "jAisvContainer", "Inventario", gkey, "Portal");
                            return false;
                        }
                        return counte > 0;
                    }
                    catch (Exception ex)
                    {
                        log_csl.save_log<Exception>(ex, "jAisvContainer", "Inventarios", booking, "N4");
                        return false;
                    }
                    finally
                    {
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        con.Dispose();
                        comando.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "jAisvContainer", "Inventario", booking, "Portal");
                return false;
            }
        }
        //metodo para saber si la unidad ya esta en contecon
        public static bool UnidadEnDeposito(string cntr)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["N5"] != null) ? System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[N5].[dbo].[sp_cntr_estate]";
                comando.Parameters.AddWithValue("@cntr", cntr.Trim().ToUpper());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString()) != 1;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "UnidadEnDeposito", cntr, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        //metodo para saber si la unidad ya esta en contecon
        public static int UnidadEstado(string cntr)
        {
                /*
                 *-1 No existe
                 *0 Advice
                 *1 Inbound
                 *2 Yard
                 *3 Ec/In
                 *4 Ec/Out
                 *5 Impo Loaded
                 *6 Retired
                 *7 Departed
                 *8 Expo Loaded
                 */
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["N5"] != null) ? System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[N5].[dbo].[sp_contenedor_estado]";
                comando.Parameters.AddWithValue("@cntr", cntr.Trim().ToUpper());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                   // log_csl.save_log<Exception>(ex, "jAisvContainer", "UnidadEstado", cntr, "N4");
                    return 0;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }

        public static bool ConfirmacionPreaviso(string cntr)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                var delay = 2000;
                if (System.Configuration.ConfigurationManager.AppSettings["retraso"] != null)
                {
                    delay = int.Parse(System.Configuration.ConfigurationManager.AppSettings["retraso"]);
                }
                if (delay <= 0)
                {
                    return true;
                }
                System.Threading.Thread.Sleep(delay);

                con.ConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["N5"] != null) ? System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString : cadena;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[N5].[dbo].[sp_cntr_estate_ex]";
                comando.Parameters.AddWithValue("@cntr", cntr.Trim().ToUpper());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        //si es 0 entonces esta INBOUND/PREADVICE
                        return int.Parse(sale.ToString()) == 0;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "ConfirmacionPreaviso", cntr, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        //metodo para saber si la unidad es nueva o ya esta presente en N4
        public static bool UnitIsCreated(string cntr)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [n5].[dbo].[fx_cntr_exists](@cntr)";
                comando.Parameters.AddWithValue("@cntr", cntr);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return bool.Parse(sale.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "UnitIsCreated", cntr, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static bool IsDriverLock(string cedula)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_is_lock_driver](@cedula)";
                comando.Parameters.AddWithValue("@cedula", cedula);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return bool.Parse(sale.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "IsDriverLock", cedula, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        //nuevo validacionde la placa
        public static bool IsTruckLock(string placa)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_is_lock_truck](@placa)";
                comando.Parameters.AddWithValue("@placa", placa);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return bool.Parse(sale.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "IsTruckLock", placa, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        //metodo para obtner los stow del booking
        public static string GetStow(string booking)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_get_stow](@booking)";
                comando.Parameters.AddWithValue("@booking", booking);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return sale.ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "GetStow", booking, "N4");
                    return null;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        //obtiene el numero de secuencia
        private static string GetSecuence()
        {
            initdataconf();
            using (var con = new SqlConnection(cadena))
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 120;
                com.CommandText = "select next value for [csl_services].[dbo].[aisvSQ]";
                try
                {
                    con.Open();
                    var result = Int64.Parse(com.ExecuteScalar().ToString());
                    con.Close();
                    return string.Format("3{0}{1}", DateTime.Now.Year, result.ToString("D7"));
                }
                catch (SqlException ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "GetSecuence", "null", "N4");
                    return null;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }

        }
        public static bool cancelAdvice(ObjectSesion user, string unit, string referencia, out string mensaje)
        {
            if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(referencia))
            {
                mensaje = "Falló un parametro unit/reference";
                return false;
            }
            //comprobar que este PRE

            try
            {

                StringBuilder ad = new StringBuilder();
                ad.Append("<argo:snx xmlns:argo=\"http://www.navis.com/argo\" ><unit-cancel-preadvise facility=\"GYE\">");
                ad.AppendFormat("<unit-identity id=\"{0}\" type=\"CONTAINERIZED\"><carrier direction=\"OB\" qualifier=\"DECLARED\" mode=\"VESSEL\" id=\"{1}\"/>", unit, referencia);
                ad.Append("</unit-identity></unit-cancel-preadvise></argo:snx> ");

                //ahora el web services.
                var webService = new n4WebService();
                mensaje = string.Empty;
                //mayor que cero la unidad no se pudo crear
                if (webService.InvokeN4Service(user, ad.ToString(), ref mensaje, String.Format("Unidad:{0},Referencia:{1}", unit, referencia)) > 2)
                {
                    return false;
                }
                mensaje = string.Empty;
                return true;

            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return false;
            }
        }
        //metodo para aplicar pesos al bl.
        public string setPesos(string peso, string blnumber)
        {
            decimal pesox = 0;
            if (!decimal.TryParse(peso, out pesox))
            {
                return "0";
            }
            var peso_temp = Decimal.Round(pesox);
            var peso_temp_c = Convert.ToString(peso_temp);
            var webService = new n4WebService();
            var t = string.Format
                (
                "<groovy class-location=\"database\" class-name=\"CGSABLCondition\"><parameters><parameter id=\"peso\" value=\"{0}\"/><parameter id=\"volumen\" value=\"{0}\"/><parameter id=\"pesable\" value=\"N\"/><parameter id=\"BLs\" value=\"{1}\"/></parameters> </groovy>", peso_temp_c, blnumber
                );
            return t;
        }
        //metodo para validar unidades advice
        public static List<unidadAdvice> ValidatePreadvices(string xml)
        {
            try
            {
                var salida = new List<unidadAdvice>();
                using (var conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString))
                {
                    var xx = conexion.ConnectionTimeout;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                          
                            comando.CommandText = "[dbo].[CSL_P_Valida_Vacios1]";
                            comando.Parameters.AddWithValue("@xmlDatos", xml);
                            conexion.Open();
                          
                            using (var result = comando.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    var tx = new unidadAdvice();
                                    tx.id = result[0] as string;
                                    tx.status = result.IsDBNull(3) ? "1" : result.GetBoolean(3) ? "1" : "0";
                                    tx.data = result[2] as string;
                                    salida.Add(tx);
                                }
                            }
                            return salida;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "jAisvContainer", "ValidatePreadvices", xml, "N4");
                        return null;
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "ValidatePreadvices", xml, "N4");
                return null;
            }
        }
        //metodo para validar unidades advice
        public static List<unidadAdvice> ValidateCancelAdvice(string xml)
        {
            try
            {
                var salida = new List<unidadAdvice>();
                using (var conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString))
                {
                    var xx = conexion.ConnectionTimeout;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "[DBO].[CSL_P_Valida_Vacios_Cancel]";
                            comando.Parameters.AddWithValue("@xmlDatos", xml);
                            conexion.Open();
                            using (var result = comando.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    var tx = new unidadAdvice();
                                    tx.id = result[0] as string;
                                    tx.status = result.IsDBNull(4) ? "1" : result.GetBoolean(4) ? "1" : "0";
                                    tx.data = result[3] as string;
                                    salida.Add(tx);
                                }
                            }
                            return salida;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "jAisvContainer", "ValidateCancelAdvice", xml, "N4");
                        return null;
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "ValidateCancelAdvice", xml, "N4");
                return null;
            }
        }
        public static HashSet<aisvDetalle> getMyDetails(string code)
        {
            try
            {
                var salida = new HashSet<aisvDetalle>();
                using (var conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString))
                {
                    var xx = conexion.ConnectionTimeout;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_get_aisv_detalles";
                            comando.Parameters.AddWithValue("@aisvcode", code);
                            conexion.Open();
                            using (var result = comando.ExecuteReader())
                            {
                                var c = 1;
                                while (result.Read())
                                {
                                    var tx = new aisvDetalle();
                                    tx.numero = c.ToString();
                                    tx.adudoc = result[0] as string;
                                    tx.tipodoc = result[1] as string;
                                    tx.bultos = result[2] as string;
                                    tx.peso = result[3] as string;
                                    tx.embalaje = !result.IsDBNull(4) ? result[4] as string : string.Empty;
                                    tx.entrega = !result.IsDBNull(5) ? result[5] as string : string.Empty;
                                    tx.estado = !result.IsDBNull(6) ? result[6] as string : string.Empty;
                                    salida.Add(tx);
                                    c++;
                                }
                            }
                            return salida;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "jAisvContainer", "getMyDetails", code, "N4");
                        return null;
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "getMyDetails", code, "N4");
                return null;
            }
        }
        public static string isValidNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return "El número de contenedor está vacío";
            }
            var comprobador = number.Trim().ToUpper();
            if (comprobador.Length < 8)
            {
                return "El número de contenedor debe tener al menos 8 caracteres";
            }
            try
            {
                var sinultimo = comprobador.Substring(0, comprobador.Length - 1);
                var verificador = comprobador.Substring(comprobador.Length - 1, comprobador.Length);
                var newverfic = fCalDig(sinultimo);
                if (newverfic != verificador)
                {
                    return String.Format("Contenedor con dígito verificador incorrecto [{0}->{1}]", newverfic, verificador);
                }
            }
            catch (Exception e)
            {
                return string.Format("Error:[{0}]", e.Message);
            }
            return string.Empty;
        }
        public static string fCalDig(string codigo)
        {
            var refc = "0123456789A_BCDEFGHIJK_LMNOPQRSTU_VWXYZ";
            int nValor; 
            int nTotal = 0; 
            int nPow2 = 1;
            if (codigo.Length != 10)
            {
                return string.Empty;
            }
            for (var n = 0; n < 10; n++)
            {
                nValor = refc.IndexOf(codigo.Substring(n, 1));
                if (nValor < 0)
                {
                    return string.Empty;
                }
                nTotal += nValor * nPow2;
                nPow2 *= 2;
            }
            nTotal = nTotal % 11;
            if (nTotal >= 10)
            {
                nTotal = 0;
            }
            return nTotal.ToString();
        }
        public static bool BookingTieneProforma(string booking)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_bookin_proforma](@boking)";
                comando.Parameters.AddWithValue("@boking", booking.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return sale.ToString().Contains("1")?true:false;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "BookingTieneProforma", booking, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static string ObtenerProforma(string booking)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_proforma_secuencia](@boking)";
                comando.Parameters.AddWithValue("@boking", booking.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                       return sale.ToString().Trim();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "BookingTieneProforma", booking, "N4");
                    return null;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static Dictionary<string, string> UnitRealInfo(string aisv, string cntr)
        {

            var lista = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString))
            {
                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spp_real_unit_info";
                    command.Parameters.AddWithValue("@aisv", aisv);
                    command.Parameters.AddWithValue("@cntr", cntr);
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
                    }
                    connection.Close();
                    command.Dispose();
                    return lista;
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "UnitRealInfo", aisv, "N4");
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
        }

        public static string groovyLA(string cntr, string referencia, DateTime dateOffMax)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<groovy class-location=\"code-extension\" class-name=\"CGSALateArrivalWS\"><parameters><parameter id=\"UNIT\" value=\"{0}\"/><parameter id=\"VESSELVISIT\" value=\"{1}\"/><parameter id=\"CUTOFF\" value=\"{2}\"/></parameters></groovy>", cntr, referencia, dateOffMax.ToString("yyyy-MM-dd HH:mm:ss"));
            return sb.ToString();
        }

        //metodo para validar unidades advice
        public static List<unidadAdvice> ValidarRetornos(string xml)
        {
            try
            {
                var salida = new List<unidadAdvice>();
                using (var conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString))
                {
                    var xx = conexion.ConnectionTimeout;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "[dbo].[pc_validar_retorno]";
                            comando.Parameters.AddWithValue("@xml", xml);
                            conexion.Open();
                            using (var result = comando.ExecuteReader())
                            {
                                while (result.Read())
                                {
                                    var tx = new unidadAdvice();
                                    tx.id = result[0] as string;
                                    tx.status = result.IsDBNull(1) ? "1" : result.GetBoolean(1) ? "1" : "0";
                                    tx.data = result[2] as string;
                                    salida.Add(tx);
                                }
                            }
                            return salida;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "jAisvContainer", "ValidarRetornos", xml, "N4");
                        return null;
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "ValidarRetornos", xml, "N4");
                return null;
            }
        }
        // var webService = new n4WebService();
        public static void procesar_retorno(List<unidadAdvice> cntrs, string mail_destino,string linea, ObjectSesion usuario)
        {
            var webService = new n4WebService();
            StringBuilder html = new StringBuilder();
            html.Append("Estimado Cliente:<br/>A continuación el resultado del proceso de los siguientes contenedores:<br/>");
            html.Append("<table border='1'><tr><th>Contenedor<th></th><th>Resultado</th></tr>");
            var msf =string.Empty;
            //insertar y marcar las unidades en campo
            StringBuilder n4 = new StringBuilder();
            foreach (var c in cntrs)
            {
                n4.Clear();
                c.status = "1";
                n4.AppendFormat("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\" /></units>",c.id);
                n4.Append("<properties><property tag=\"UfvFlexString10\" value=\"CONTECON\" /></properties></icu>");
                var i =  webService.InvokeN4Service(usuario, n4.ToString(), ref msf, usuario.usuario);
                if (i < 0 || i > 1)
                {
                    c.data = msf;
                    c.status = "0";
                }
                //insertar el registro.
                inserta_nuevo_cas(c,usuario.usuario,linea,  mail_destino);
                html.AppendFormat("<tr> <td>{0}</td> <td>{1}</td> </tr>",c.id,string.IsNullOrEmpty(c.data)?"OK":c.data);
            }
            html.Append("</table><br/>Para mas detalles del resultado del proceso comuniquese con Planificación.<br/><br/>Sistema de Solicitud de Servicios <br/>S3 - Contecon Guayaquil");
            dbconfig.addMail(mail_destino, "Preaviso de contenedores por retorno de vacíos", html.ToString(), "contecon.s3@gmail.com", usuario.usuario);
          }
        private static bool inserta_nuevo_cas(unidadAdvice unidad,string usuario, string linea, string destino)
        {
            if (unidad == null)
            {
                return false;
            }
            if(string.IsNullOrEmpty( unidad.id))
            {
                return false;
            }
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Connection = con;
                comando.CommandText = "[dbo].[pc_inserta_retorno_cas]";
                comando.Parameters.AddWithValue("@cntr", unidad.id);
                comando.Parameters.AddWithValue("@usuario",usuario) ;
                comando.Parameters.AddWithValue("@codigoempresa", linea);
                comando.Parameters.AddWithValue("@estado", unidad.status.Contains("0")?false:true);
                if (unidad.status.Contains("0"))
                {
                    comando.Parameters.AddWithValue("@problema", unidad.data);
                }
                 try
                {
                    con.Open();
                    int sale = comando.ExecuteNonQuery();
                    con.Close();
                    if (sale > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jAisvContainer", "inserta_nuevo_cas", usuario, "INSERTA-CAS-IMPO");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
     
        }


        
    }
    //clase para pasarse mensajes 
    //entre los pagemethods y formularios
    public class jMessage
    {
        //resultado de la operación proscrita
        public bool resultado { get; set; }
        //le indica si la data debe ser ejecutada en el cliente
        public bool fluir { get; set; }
        //datos o funcionamientos adicionales en el cliente
        public string data { get; set; }
        //mensaje que se mostrará en pantalla
        public string mensaje { get; set; }
    }
    public class aisvDetalle
    {
        public string numero { get; set; }
        public string peso { get; set; }
        public string bultos { get; set; }
        public string embalaje { get; set; }
        public string adudoc { get; set; }
        public string tipodoc { get; set; }
        public string entrega { get; set; }
        public string estado { get; set; }
    }


}