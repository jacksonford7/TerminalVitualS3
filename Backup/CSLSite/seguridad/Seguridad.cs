using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace CSLSite
{
    public class Seguridad
    {
        /// <summary>
        ///  Método que permite realizar la inserción o modificación de un servicio
        /// </summary>
        /// <param name="servicio">Objeto Servicio que posee toda la información del servicio a ser ingresado/modificado</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre de usuario que realiza la operación</param>
        /// <returns>Una cadena que informa el estado de la operación: ok=correcto</returns>
        public string GuardarModificarServicio(Servicio servicio, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_ServiciosInsercionModificacion", new Dictionary<string, string>() { { "idServicio", servicio.codigo.ToString() }, { "descripcion", servicio.descripcion }, { "estado", servicio.estado }, {"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos del servicio.";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que permite realizar la inserción o modificación de un grupo
        /// </summary>
        /// <param name="grupo">Objeto Grupo que posee toda la información del grupo a ser ingresado/modificado</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realiza la operación</param>
        /// <returns>Cadena que informa el estado de la operación: ok = correcto</returns>
        public string GuardarModificarGrupo(Grupo grupo, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_GruposInsercionModificacion", new Dictionary<string, string>() { { "idGrupo", grupo.codigo.ToString() }, { "descripcion", grupo.descripcion }, { "estado", grupo.estado }, {"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos del rol.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que permite realizar la inserción o modificación de una opción de servicio
        /// </summary>
        /// <param name="opcion">Objeto  OpcionesServicio que posee toda la información de la opción a ser ingresado/modificado</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realiza la operación</param>
        /// <returns>Cadena que informa el estado de la operación: ok = correcto</returns>
        public string GuardarModificarOpcionesServicio(OpcionesServicio opcion, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_OpcionesServiciosInsercionModificacion", new Dictionary<string, string>() { { "idOpcion", opcion.idOpcion.ToString() }, { "nombreOpcion", opcion.nombreOpcion }, { "formularioOpcion", opcion.formulario }, { "tituloOpcion", opcion.titulo }, { "descripcionOpcion", opcion.descripcion }, { "estado", opcion.estado }, { "idServicio", opcion.idServicio.ToString() }, {"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos de la opción.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que permite realizar la inserción o modificación de un área
        /// </summary>
        /// <param name="area">Objeto area que posee toda la información del área a ser ingresada/modificada</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realiza la operación</param>
        /// <returns>Cadena que informa el estado de la operación: ok = correcto</returns>
        public string GuardarModificarArea(Area area, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_AreasInsercionModificacion", new Dictionary<string, string>() { { "idServicio", area.idServicio.ToString() }, { "nombreArea", area.nombreArea }, { "iconoArea", area.icono }, { "tituloArea", area.titulo }, { "estado", area.estado }, {"areaAdministrativa",area.areaAdministrativa.ToString()},{"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos de la opción.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que permite realizar la inserción o modificación de un usuario
        /// </summary>
        /// <param name="usuario">Objeto Usuario que posee toda la información del usuario a ser ingresado/modificado</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realiza la operación</param>
        /// <returns>Cadena que informa el estado de la operación: ok = correcto</returns>
        public string GuardarModificarUsuario(UsuarioSeguridad usuario, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_UsuarioInsercionModificacion", new Dictionary<string, string>() { { "idUsuario", usuario.idUsuario.ToString() }, { "usuario", usuario.usuario }, { "password", usuario.password}, { "tipoUsuario", usuario.tipoUsuario }, { "estado", usuario.estado }, { "nombreUsuario", usuario.nombreUsuario.ToString() }, { "apellidoUsuario", usuario.apellidoUsuario }, { "identificacionEmpresa", usuario.codigoEmpresa }, { "ciudad", usuario.ciudad.ToString() }, { "pais", usuario.pais }, { "correoUsuario", usuario.usuarioCorreo }, { "direccionEmpresa", usuario.direccionEmpresa }, { "telefonoEmpresa", usuario.telefonoEmpresa }, { "faxEmpresa", usuario.faxEmpresa }, { "websiteEmpresa", usuario.websiteEmpresa }, { "nombreEmpresa", usuario.nombreEmpresa }, { "correoEmpresa", usuario.correoEmpresa }, { "registradoPor", usuario.registradoPor }, { "identificacionUsuario", usuario.usuarioIdentificacion }, { "ipCreacion", usuario.ipCreacion }, {"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos del usuario.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que permite realizar la inserción o modificación de permisos
        /// </summary>
        /// <param name="idGrupo">Id del grupo que se modificará los permisos</param>
        /// <param name="idServicio">Id del servicio que se modificará los permisos</param>
        /// <param name="permisos">Una cadena que especifica los permisos que se le dará a cada opción: 1=acceso; 0: no acceso</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realiza la operación</param>
        /// <returns>Cadena que informa el estado de la operación: ok = correcto</returns>
        public string GuardarModificarPermisos(int idGrupo, int idServicio, string permisos, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_InsercionModificacionPermisos", new Dictionary<string, string>() { { "idGrupo", idGrupo.ToString() }, { "idServicio", idServicio.ToString() }, { "permisos", permisos }, {"idUsuarioOperacion", idUsuarioOperacion.ToString()},{"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los permisos.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que devuelve la cantidad de areas que hay por un servicio
        /// </summary>
        /// <param name="idServicio">El id del servicio del cual quiere saber el área</param>
        /// <returns>Devuelve la cantidad de áreas asociadas a un servicio</returns>
        public int consultarAreaPorIdServicio(int idServicio)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_ConsultaAreasPorIdServicio", new Dictionary<string, string>() { { "idServicio", idServicio.ToString() }},tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado))
                {
                    return int.Parse(resultado);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que consulta los servicios según los filtros de búsqueda
        /// </summary>
        /// <param name="descripcion">Nombre del servicio que desea buscar</param>
        /// <param name="estado">Estado de los servicios que desea buscar</param>
        /// <returns>Lista de servicios</returns>
        public List<Servicio> consultarServicio(string descripcion, string estado)
        {
            try
            {
                string resultado = string.Empty;

                List<Servicio> servicioTemp = new List<Servicio>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaServicios", tComando.Procedure, new Dictionary<string, string>() { { "descripcion", descripcion }, { "estado", estado } }))
                {
                    Servicio s = new Servicio();
                    s.codigo = int.Parse(item[0].ToString().Trim());
                    s.descripcion = item[1].ToString().Trim();
                    s.estado = item[2].ToString().Trim();
                    servicioTemp.Add(s);
                }

                return servicioTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que consulta los grupos según los filtros de búsqueda
        /// </summary>
        /// <param name="descripcion">Nombre del grupo que desea buscar</param>
        /// <param name="estado">Estado de los grupos que desea buscar</param>
        /// <returns>Lista de grupos</returns>
        public List<Grupo> consultarGrupo(string descripcion, string estado)
        {
            try
            {
                string resultado = string.Empty;

                List<Grupo> grupoTemp = new List<Grupo>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaGrupos", tComando.Procedure, new Dictionary<string, string>() { { "descripcion", descripcion }, { "estado", estado } }))
                {
                    Grupo g = new Grupo();
                    g.codigo = int.Parse(item[0].ToString().Trim());
                    g.descripcion = item[1].ToString().Trim();
                    g.estado = item[2].ToString().Trim();
                    grupoTemp.Add(g);
                }

                return grupoTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que consulta los grupos que tiene una determinada identificación ( devuelve los grupos de una empresa)
        /// </summary>
        /// <param name="identificacion">Identificación sobre la cual quiere realizar la obtención de los grupos</param>
        /// <param name="idUsuario">Id del usuario sobre el que obtendra la identificación</param>
        /// <returns>Lista de grupos</returns>
        public List<Grupo> consultarGrupoPorIdentificacion(string identificacion, int idUsuario)
        {
            try
            {
                string resultado = string.Empty;

                List<Grupo> grupoTemp = new List<Grupo>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaGruposxIdentificacion", tComando.Procedure, new Dictionary<string, string>() { { "identificacion", identificacion }, { "idUsuario", idUsuario.ToString() } }))
                {
                    Grupo g = new Grupo();
                    g.codigo = int.Parse(item[0].ToString().Trim());
                    g.descripcion = item[1].ToString().Trim();
                    g.estado = item[2].ToString().Trim();
                    grupoTemp.Add(g);
                }

                return grupoTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que consulta las opciones según filtros de búsqueda
        /// </summary>
        /// <param name="nombreOpcion">Nombre de la opción que desea buscar</param>
        /// <param name="nombreServicio">Nombre del servicio asociado a la opción que desea buscar</param>
        /// <param name="estado">Estado de la opción que desea buscar</param>
        /// <returns>Listado de opciones</returns>
        public List<OpcionesServicio> consultarOpciones(string nombreOpcion, string nombreServicio, string estado)
        {
            try
            {
                string resultado = string.Empty;

                List<OpcionesServicio> opcionesTemp = new List<OpcionesServicio>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaOpcionesServicios", tComando.Procedure, new Dictionary<string, string>() { { "nombreOpcion", nombreOpcion }, { "nombreServicio", nombreServicio }, { "estado", estado } }))
                {
                    OpcionesServicio os = new OpcionesServicio();
                    os.idOpcion = int.Parse(item[0].ToString().Trim());
                    os.idServicio = int.Parse(item[1].ToString().Trim());
                    os.nombreOpcion = item[2].ToString().Trim();
                    os.nombreServicio = item[3].ToString().Trim();
                    os.estado = item[4].ToString().Trim();
                    os.descripcion = item[5].ToString().Trim();
                    os.formulario = item[6].ToString().Trim();
                    os.titulo = item[7].ToString().Trim();
                    opcionesTemp.Add(os);
                }

                return opcionesTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que consulta las áreas según filtros de búsqueda
        /// </summary>
        /// <param name="nombreOpcion">Nombre del área que desea consultar</param>
        /// <param name="nombreServicio">Nombre del servicio asociado al área que desea buscar</param>
        /// <param name="estado">Estado del área que desea buscar</param>
        /// <returns>Listado de áreas</returns>
        public List<Area> consultarAreas(string nombreOpcion, string nombreServicio, string estado)
        {
            try
            {
                
                string resultado = string.Empty;

                List<Area> areasTemp = new List<Area>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaAreas", tComando.Procedure, new Dictionary<string, string>() { { "nombreArea", nombreOpcion }, { "nombreServicio", nombreServicio }, { "estado", estado } }))
                {
                    Area a = new Area();
                    a.idServicio = int.Parse(item[0].ToString().Trim());
                    a.nombreArea = item[1].ToString().Trim();
                    a.nombreServicio = item[2].ToString().Trim();
                    a.estado = item[3].ToString().Trim();
                    a.icono = item[4].ToString().Trim();
                    a.titulo = item[5].ToString().Trim();
                    a.areaAdministrativa = bool.Parse(item[6].ToString().Trim());
                    areasTemp.Add(a);
                }

                return areasTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que consulta las áreas según filtros de búsqueda
        /// </summary>
        /// <param name="nombreOpcion">Nombre del área que desea consultar</param>
        /// <param name="nombreServicio">Nombre del servicio asociado al área que desea buscar</param>
        /// <param name="estado">Estado del área que desea buscar</param>
        /// <returns>Listado de áreas</returns>
        public List<Area> consultarAreasServicios(string nombreOpcion, string nombreServicio, string estado)
        {
            try
            {

                string resultado = string.Empty;

                List<Area> areasTemp = new List<Area>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaAreasServicios", tComando.Procedure, new Dictionary<string, string>() { { "nombreArea", nombreOpcion }, { "nombreServicio", nombreServicio }, { "estado", estado } }))
                {
                    Area a = new Area();
                    a.idServicio = int.Parse(item[0].ToString().Trim());
                    a.nombreArea = item[1].ToString().Trim();
                    a.nombreServicio = item[2].ToString().Trim();
                    a.estado = item[3].ToString().Trim();
                    a.icono = item[4].ToString().Trim();
                    a.titulo = item[5].ToString().Trim();
                    a.areaAdministrativa = bool.Parse(item[6].ToString().Trim());
                    areasTemp.Add(a);
                }

                return areasTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que retorna los detalles de un determinado catálogo
        /// </summary>
        /// <param name="tipoCatalogo">Nombre del catálogo que quiere consultar</param>
        /// <returns>Detalle del catálogo</returns>
        public static HashSet<Tuple<string, string>> getDetalleCatalogosSeguridad(string tipoCatalogo)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in CLSDataSeguridad.returnDetalleCatalogosSeguridad(tConexion.Master, tipoCatalogo))
            {
                xlista.Add(Tuple.Create(i.Item1.ToUpper(), i.Item2.ToUpper()));
            }
            
            return xlista;
        }

        /// <summary>
        /// Método que retorna el catálogo de paises completo
        /// </summary>
        /// <returns>Catálogo de países</returns>
        public static HashSet<Tuple<string, string>> getPaises()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in CLSDataSeguridad.returnPaises(tConexion.Service))
            {
                xlista.Add(Tuple.Create(i.Item1.ToUpper(), i.Item2.ToUpper()));
            }

            return xlista;
        }

        /// <summary>
        /// Método que retorna el catálogo de provincias de ECUADOR completo
        /// </summary>
        /// <returns>Catálogo de provincias</returns>
        public static HashSet<Tuple<string, string>> getProvincias()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in CLSDataSeguridad.returnProvincias(tConexion.Service))
            {
                xlista.Add(Tuple.Create(i.Item1.ToUpper(), i.Item2.ToUpper()));
            }

            return xlista;
        }


        /// <summary>
        /// Método que retorna el catálogo de ciudades según el Id de la provincia enviado
        /// </summary>
        /// <param name="idProvincia">Id de la provincia a enviar</param>
        /// <returns>Catálogo de ciudades</returns>
        public static HashSet<Tuple<string, string>> getCiudades(int idProvincia)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in CLSDataSeguridad.returnCiudades(tConexion.Service, idProvincia))
            {
                xlista.Add(Tuple.Create(i.Item1.ToUpper(), i.Item2.ToUpper()));
            }

            return xlista;
        }


        /// <summary>
        /// Método que retorna la información del usuario según el id consultado
        /// </summary>
        /// <param name="id">Id del usuario que desea consultar</param>
        /// <returns>Información completa del usuario</returns>
        public UsuarioSeguridad consultarUsuarioPorId(int id)
        {
            try
            {

                string resultado = string.Empty;

                UsuarioSeguridad u = null;
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaUsuarioPorId", tComando.Procedure, new Dictionary<string, string>() { { "idUsuario", id.ToString() } }))
                {
                    u = new UsuarioSeguridad();
                    u.idUsuario = int.Parse(item[0].ToString().Trim());
                    u.usuario = item[1].ToString().Trim();
                    u.nombres = item[2].ToString().Trim();
                    u.usuarioIdentificacion = item[3].ToString().Trim();
                    u.usuarioCorreo = item[4].ToString().Trim();
                    u.nombreEmpresa = item[5].ToString().Trim();
                    u.codigoEmpresa = item[6].ToString().Trim();
                    u.tipoUsuario = item[7].ToString().Trim();
                    u.desTipoUsuario = item[8].ToString().Trim();
                    u.estado = item[9].ToString().Trim();
                    u.correoEmpresa = item[10].ToString().Trim();
                    u.pais = item[11].ToString().Trim();
                    u.ciudad = int.Parse(item[12].ToString().Trim());
                    u.direccionEmpresa = item[13].ToString().Trim();
                    u.telefonoEmpresa = item[14].ToString().Trim();
                    u.faxEmpresa = item[15].ToString().Trim();
                    u.websiteEmpresa = item[16].ToString().Trim();
                    u.nombreUsuario = item[17].ToString().Trim();
                    u.apellidoUsuario = item[18].ToString().Trim();
                    u.idProvincia = int.Parse(item[20].ToString().Trim());
                    Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                    if (componente == null)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_id", u.usuario, u.usuario);
                        //  throw new Exception("Control1");
                        return null;
                    }
                    else
                    {
                        u.password = item[19].ToString().Trim();
                        dynamic instancia = Activator.CreateInstance(componente);
                        var dcpass = instancia.Decrypt(u.usuario.Trim(), u.password.Trim()) as string;
                        if (string.IsNullOrEmpty(dcpass))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_id", u.password, u.usuario);
                            // throw new Exception("Control2");
                            return null;
                        }
                        u.password = dcpass;
                        instancia = null;
                        componente = null;
                    }
                }

                return u;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que consulta usuarios según los filtros de búsqueda
        /// </summary>
        /// <param name="usuario">Nombre de usuario(username) a consultar</param>
        /// <param name="nombreUsuario">Nombre personal del usuario (Nombre o Apellido) a consultar</param>
        /// <param name="identificacionUsuario">Identificación del usuario a consultar</param>
        /// <param name="nombreEmpresa">Nombre de la empresa relacionada con el usuario que desea consultar</param>
        /// <param name="estado">Estado del usuario que desea consultar</param>
        /// <param name="tipoUsuario">Tipo de usuario que desea consultar</param>
        /// <param name="identificacionEmpresa">Identificación de la empresa relacionada con el usuario que desea consultar</param>
        /// <returns></returns>
        public List<UsuarioSeguridad> consultarUsuarios(string usuario, string nombreUsuario, string identificacionUsuario, string nombreEmpresa, string estado, string tipoUsuario, string identificacionEmpresa)
        {
            try
            {

                string resultado = string.Empty;

                List<UsuarioSeguridad> usuarios = new List<UsuarioSeguridad>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaUsuarios", tComando.Procedure, new Dictionary<string, string>() { { "usuario", usuario }, { "nombreUsuario", nombreUsuario }, { "identificacionUsuario", identificacionUsuario }, { "nombreEmpresa", nombreEmpresa }, { "estado", estado }, { "tipoUsuario", tipoUsuario },  { "identificacionEmpresa", identificacionEmpresa } }))
                {
                    UsuarioSeguridad u = new UsuarioSeguridad();
                    u.idUsuario = int.Parse(item[0].ToString().Trim());
                    u.usuario = item[1].ToString().Trim();
                    u.nombres = item[2].ToString().Trim();
                    u.usuarioIdentificacion = item[3].ToString().Trim();
                    u.usuarioCorreo = item[4].ToString().Trim();
                    u.nombreEmpresa = item[5].ToString().Trim();
                    u.codigoEmpresa = item[6].ToString().Trim();
                    u.tipoUsuario = item[7].ToString().Trim();
                    u.desTipoUsuario = item[8].ToString().Trim();
                    u.estado = item[9].ToString().Trim();
                    u.correoEmpresa = item[10].ToString().Trim();
                    usuarios.Add(u);
                }

                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que devuelve una lista de usuarios según los filtros de búsqueda y el id del grupo que desea asignar
        /// </summary>
        /// <param name="usuario">Nombre de usuario(username) a consultar</param>
        /// <param name="nombreUsuario">Nombre personal del usuario (Nombre o Apellido) a consultar</param>
        /// <param name="identificacionUsuario">Identificación del usuario a consultar</param>
        /// <param name="nombreEmpresa">Nombre de la empresa relacionada con el usuario que desea consultar</param>
        /// <param name="estado">Estado del usuario que desea consultar</param>
        /// <param name="tipoUsuario">Tipo de usuario que desea consultar</param>
        /// <param name="identificacionEmpresa">Identificación de la empresa relacionada con el usuario que desea consultar</param>
        /// <param name="idGrupo">Id del grupo sobre el cual desea consultar la asignación grupo - usuario</param>
        /// <returns>Lista de asignación grupo - usuario</returns>
        public List<UsuariosRolesSeguridad> consultarUsuariosRolesAdministradores(string usuario, string nombreUsuario, string identificacionUsuario, string nombreEmpresa, string estado, string tipoUsuario, string identificacionEmpresa, int idGrupo)
        {
            try
            {

                string resultado = string.Empty;

                List<UsuariosRolesSeguridad> usuarios = new List<UsuariosRolesSeguridad>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaUsuariosRolesAdministrador", tComando.Procedure, new Dictionary<string, string>() { { "usuario", usuario }, { "nombreUsuario", nombreUsuario }, { "identificacionUsuario", identificacionUsuario }, { "nombreEmpresa", nombreEmpresa }, { "estado", estado }, { "tipoUsuario", tipoUsuario }, { "identificacionEmpresa", identificacionEmpresa }, { "idGrupo", idGrupo.ToString() } }))
                {
                    UsuariosRolesSeguridad u = new UsuariosRolesSeguridad();
                    u.idUsuario = int.Parse(item[0].ToString().Trim());
                    u.usuario = item[1].ToString().Trim();
                    u.nombres = item[2].ToString().Trim();
                    u.usuarioIdentificacion = item[3].ToString().Trim();
                    u.usuarioCorreo = item[4].ToString().Trim();
                    u.nombreEmpresa = item[5].ToString().Trim();
                    u.codigoEmpresa = item[6].ToString().Trim();
                    u.tipoUsuario = item[7].ToString().Trim();
                    u.desTipoUsuario = item[8].ToString().Trim();
                    u.estado = item[9].ToString().Trim();
                    u.correoEmpresa = item[10].ToString().Trim();
                    u.roles = item[11].ToString().Trim();
                    u.rolGrupo = Convert.ToBoolean(int.Parse((item[12].ToString().Trim())));
                    u.idGrupo = int.Parse(item[13].ToString().Trim());
                    usuarios.Add(u);
                }

                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que consulta la asignación de grupos - usuarios (hijos)
        /// </summary>
        /// <param name="usuario">Nombre de usuario(username) a consultar</param>
        /// <param name="nombreUsuario">Nombre personal del usuario (Nombre o Apellido) a consultar</param>
        /// <param name="identificacionUsuario">Identificación del usuario a consultar</param>
        /// <param name="nombreEmpresa">Nombre de la empresa relacionada con el usuario que desea consultar</param>
        /// <param name="estado">Estado del usuario que desea consultar</param>
        /// <param name="tipoUsuario">Tipo de usuario que desea consultar</param>
        /// <param name="identificacionEmpresa">Identificación de la empresa relacionada con el usuario que desea consultar</param>
        /// <param name="idGrupo">Id del grupo que desea asignar</param>
        /// <returns>Lista de asignación grupo - usuario</returns>
        public List<UsuariosRolesSeguridad> consultarUsuariosRolesOperador(string usuario, string nombreUsuario, string identificacionUsuario, string nombreEmpresa, string estado, string tipoUsuario, string identificacionEmpresa, int idGrupo)
        {
            try
            {

                string resultado = string.Empty;

                List<UsuariosRolesSeguridad> usuarios = new List<UsuariosRolesSeguridad>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaUsuariosRolesOperador", tComando.Procedure, new Dictionary<string, string>() { { "usuario", usuario }, { "nombreUsuario", nombreUsuario }, { "identificacionUsuario", identificacionUsuario }, { "nombreEmpresa", nombreEmpresa }, { "estado", estado }, { "tipoUsuario", tipoUsuario }, { "identificacionEmpresa", identificacionEmpresa }, { "idGrupo", idGrupo.ToString() } }))
                {
                    UsuariosRolesSeguridad u = new UsuariosRolesSeguridad();
                    u.idUsuario = int.Parse(item[0].ToString().Trim());
                    u.usuario = item[1].ToString().Trim();
                    u.nombres = item[2].ToString().Trim();
                    u.usuarioIdentificacion = item[3].ToString().Trim();
                    u.usuarioCorreo = item[4].ToString().Trim();
                    u.nombreEmpresa = item[5].ToString().Trim();
                    u.codigoEmpresa = item[6].ToString().Trim();
                    u.tipoUsuario = item[7].ToString().Trim();
                    u.desTipoUsuario = item[8].ToString().Trim();
                    u.estado = item[9].ToString().Trim();
                    u.correoEmpresa = item[10].ToString().Trim();
                    u.roles = item[11].ToString().Trim();
                    u.rolGrupo = Convert.ToBoolean(int.Parse((item[12].ToString().Trim())));
                    u.idGrupo = int.Parse(item[13].ToString().Trim());
                    usuarios.Add(u);
                }

                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que consulta las empresas por su nombre e identificación
        /// </summary>
        /// <param name="nombreEmpresa">Nombre de la empresa que desea consultar</param>
        /// <param name="identificacionEmpresa">Identificación de la empresa que desea consultar</param>
        /// <returns>Listado de las empresas</returns>
        public List<Empresa> consultarEmpresas(string nombreEmpresa, string identificacionEmpresa)
        {
            try
            {

                string resultado = string.Empty;

                List<Empresa> empresas = new List<Empresa>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.N4Catalog, "MA_SE_ConsultaClientesEmpresa", tComando.Procedure, new Dictionary<string, string>() { { "nombreEmpresa", nombreEmpresa }, { "identificacionEmpresa", identificacionEmpresa } }))
                {
                    Empresa e = new Empresa();
                    e.identificacion = item[0].ToString().Trim();
                    e.nombre = item[1].ToString().Trim();
                    e.direccion = item[2].ToString().Trim();
                    e.fax = item[3].ToString().Trim();
                    e.website = item[4].ToString().Trim();
                    e.email1 = item[5].ToString().Trim();
                    e.telefono = item[6].ToString().Trim();
                    empresas.Add(e);
                }

                return empresas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método que devuelve la información completa del usuario buscandolo por username
        /// </summary>
        /// <param name="username">Username que desea buscar</param>
        /// <returns>Información del usuario</returns>
        public UsuarioSeguridad consultarUsuarioPorUserName(string username)
        {
            try
            {

                string resultado = string.Empty;

                UsuarioSeguridad u = null;
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaUsuarioPorUserName", tComando.Procedure, new Dictionary<string, string>() { { "usuario", username } }))
                {
                    u = new UsuarioSeguridad();
                    u.idUsuario = int.Parse(item[0].ToString().Trim());
                    u.usuario = item[1].ToString().Trim();
                    u.nombres = item[2].ToString().Trim();
                    u.usuarioIdentificacion = item[3].ToString().Trim();
                    u.usuarioCorreo = item[4].ToString().Trim();
                    u.nombreEmpresa = item[5].ToString().Trim();
                    u.codigoEmpresa = item[6].ToString().Trim();
                    u.tipoUsuario = item[7].ToString().Trim();
                    u.desTipoUsuario = item[8].ToString().Trim();
                    u.estado = item[9].ToString().Trim();
                    u.correoEmpresa = item[10].ToString().Trim();
                    u.pais = item[11].ToString().Trim();
                    u.ciudad = int.Parse(item[12].ToString().Trim());
                    u.direccionEmpresa = item[13].ToString().Trim();
                    u.telefonoEmpresa = item[14].ToString().Trim();
                    u.faxEmpresa = item[15].ToString().Trim();
                    u.websiteEmpresa = item[16].ToString().Trim();
                    u.nombreUsuario = item[17].ToString().Trim();
                    u.apellidoUsuario = item[18].ToString().Trim();
                    u.idProvincia = int.Parse(item[20].ToString().Trim());
                    Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                    if (componente == null)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_username", u.usuario, u.usuario);
                        //  throw new Exception("Control1");
                        return null;
                    }
                    else
                    {
                        u.password = item[19].ToString().Trim();
                        dynamic instancia = Activator.CreateInstance(componente);
                        var dcpass = instancia.Decrypt(u.usuario.Trim(), u.password.Trim()) as string;
                        if (string.IsNullOrEmpty(dcpass))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_username", u.password, u.usuario);
                            // throw new Exception("Control2");
                            return null;
                        }
                        u.password = dcpass;
                        instancia = null;
                        componente = null;
                    }
                }

                return u;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que devuelve la información completa del usuario buscandolo por la identificación de la empresa
        /// </summary>
        /// <param name="idUsuario">Id del usuario que desea buscar</param>
        /// <param name="identificacionEmpresa">Identificación del usuario que desea buscar</param>
        /// <returns>Información del usuario</returns>
        public UsuarioSeguridad consultarUsuarioPorCodigoEmpresa(int idUsuario, string identificacionEmpresa)
        {
            try
            {

                string resultado = string.Empty;

                UsuarioSeguridad u = null;
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaUsuarioPorIdentificacionEmpresa", tComando.Procedure, new Dictionary<string, string>() { { "idUsuario", idUsuario.ToString() }, { "codigoEmpresa", identificacionEmpresa } }))
                {
                    u = new UsuarioSeguridad();
                    u.idUsuario = int.Parse(item[0].ToString().Trim());
                    u.usuario = item[1].ToString().Trim();
                    u.nombres = item[2].ToString().Trim();
                    u.usuarioIdentificacion = item[3].ToString().Trim();
                    u.usuarioCorreo = item[4].ToString().Trim();
                    u.nombreEmpresa = item[5].ToString().Trim();
                    u.codigoEmpresa = item[6].ToString().Trim();
                    u.tipoUsuario = item[7].ToString().Trim();
                    u.desTipoUsuario = item[8].ToString().Trim();
                    u.estado = item[9].ToString().Trim();
                    u.correoEmpresa = item[10].ToString().Trim();
                    u.pais = item[11].ToString().Trim();
                    u.ciudad = int.Parse(item[12].ToString().Trim());
                    u.direccionEmpresa = item[13].ToString().Trim();
                    u.telefonoEmpresa = item[14].ToString().Trim();
                    u.faxEmpresa = item[15].ToString().Trim();
                    u.websiteEmpresa = item[16].ToString().Trim();
                    u.nombreUsuario = item[17].ToString().Trim();
                    u.apellidoUsuario = item[18].ToString().Trim();
                    u.idProvincia = int.Parse(item[20].ToString().Trim());
                    Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                    if (componente == null)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_identificacionEmpresa", u.usuario, u.usuario);
                        //  throw new Exception("Control1");
                        return null;
                    }
                    else
                    {
                        u.password = item[19].ToString().Trim();
                        dynamic instancia = Activator.CreateInstance(componente);
                        var dcpass = instancia.Decrypt(u.usuario.Trim(), u.password.Trim()) as string;
                        if (string.IsNullOrEmpty(dcpass))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_identificacionEmpresa", u.password, u.usuario);
                            // throw new Exception("Control2");
                            return null;
                        }
                        u.password = dcpass;
                        instancia = null;
                        componente = null;
                    }
                }

                return u;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que realiza el cambio de password de un determinado usuario
        /// </summary>
        /// <param name="password">Password nuevo del usuario</param>
        /// <param name="idUsuario">Id del usuario al cual se le realizará la actualización</param>
        /// <param name="cambiarPassword">Especifíca si requiere cambio de contraseña al iniciar sesión o no, S=SI; N=NO</param>
        /// <param name="operacion">Específica si se realiza el cambio de contraseña por recuperación o por cambio</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realiza la operación</param>
        /// <param name="loginNameOperacion">Nombre de usuario que realiza la operación</param>
        /// <returns>Cadena que identifica el estado de la operación</returns>
        public string CambioPasswordUsuario(string password, int idUsuario, string cambiarPassword, string operacion, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalar(tConexion.Master, "MA_SE_UsuarioCambioPassword", new Dictionary<string, string>() { { "idUsuario", idUsuario.ToString() }, { "password", password }, {"cambiarpassword",cambiarPassword}, {"operacion", operacion}, {"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos del usuario.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que permite consultar los permisos de un grupo y servicio específico
        /// </summary>
        /// <param name="idGrupo">Id del grupo a consultar</param>
        /// <param name="idServicio">Id del servicio a consultar</param>
        /// <returns>Lista de permisos</returns>
        public List<Permiso> consultarPermisos(int idGrupo, int idServicio)
        {
            try
            {

                string resultado = string.Empty;

                List<Permiso> permisosTemp = new List<Permiso>();
                foreach (var item in CLSDataSeguridad.ValorLecturas(tConexion.Master, "MA_SE_ConsultaPermisos", tComando.Procedure, new Dictionary<string, string>() { { "idServicio", idServicio.ToString() }, { "idGrupo", idGrupo.ToString() } }))
                {
                    Permiso p = new Permiso();
                    p.idGrupo = int.Parse(item[0].ToString().Trim());
                    p.idServicio = int.Parse(item[1].ToString().Trim());
                    p.nombreOpcion = item[2].ToString().Trim();
                    p.descripcion = item[3].ToString().Trim();
                    p.idOpcion = int.Parse(item[4].ToString().Trim());
                    p.permiso = Convert.ToBoolean(int.Parse(item[5].ToString().Trim()));
                    permisosTemp.Add(p);
                }

                return permisosTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que permite modificar e insertar las asignaciones grupo - usuario administrador
        /// </summary>
        /// <param name="idGrupo">Id del grupo de la asignación</param>
        /// <param name="dtUsuarios">Conjunto de usuarios a asignarle la relación</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realizó la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realizó la operación</param>
        /// <returns>Cadena que identifica el estado de la operación</returns>
        public string GuardarModificarRolesUsuarioAdministrador(int idGrupo, DataTable dtUsuarios, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalarConjunto(tConexion.Master, "MA_SE_RolUsuarioInsercionModificacion", new Dictionary<string, string>() { { "idGrupo", idGrupo.ToString() }, {"idUsuarioOperacion",idUsuarioOperacion.ToString()},{"loginNameOperacion", loginNameOperacion} }, "usuarioDatos", dtUsuarios, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos de asignación.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que permite modificar e insertar las asignaciones grupo - usuario operador
        /// 
        /// 
        /// </summary>
        /// <param name="idGrupo">Id del grupo de la asignación</param>
        /// <param name="dtUsuarios">Conjunto de usuarios a asignarle la relación</param>
        /// <param name="idUsuarioOperacion">Id del usuario que realizó la operación</param>
        /// <param name="loginNameOperacion">Nombre del usuario que realizó la operación</param>
        /// <returns>Cadena que identifica el estado de la operación</returns>
        public string GuardarModificarRolesUsuarioOperador(int idGrupo, DataTable dtUsuarios, int idUsuarioOperacion, string loginNameOperacion)
        {
            try
            {
                string resultado = string.Empty;
                resultado = CLSDataSeguridad.ValorEscalarConjunto(tConexion.Master, "MA_SE_RolUsuarioOperadorInsercionModificacion", new Dictionary<string, string>() { { "idGrupo", idGrupo.ToString() }, {"idUsuarioOperacion", idUsuarioOperacion.ToString()}, {"loginNameOperacion", loginNameOperacion} }, "usuarioDatos", dtUsuarios, tComando.Procedure);

                if (!string.IsNullOrEmpty(resultado) && resultado != "0")
                {
                    return "ok";
                }
                else
                {
                    return "Ocurrió un error al momento de guardar los datos de asignación.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}