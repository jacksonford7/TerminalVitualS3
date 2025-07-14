using AccesoDatos;
using BillionEntidades;
using ServicesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Banano
{
    public class BAN_TurnosCliente : Cls_Bil_Base
    {
        public BAN_TurnosDet TurnoDet { get; set; }
        public long secuencia { get; set; }
        public long idTurnoDet { get; set; }
        public string idcliente { get; set; }
        public string cliente { get; set; }
        public int cantidad { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }

        public BAN_TurnosCliente()
        {
            init();
        }

        public ResultadoOperacion<List<BAN_TurnosCliente>> Save_Update( out long v_secuencia)// (long secuencia, long idDetalle, string idcliente, int cantidad, bool estado, string usuario)
        {
            string OnError = string.Empty;
            parametros.Clear();
            parametros.Add("i_secuencia ", secuencia);
            parametros.Add("i_idTurnoDet ", idTurnoDet);
            parametros.Add("i_idcliente ", idcliente);
            parametros.Add("i_cliente ", cliente);
            parametros.Add("i_cantidad ", cantidad);
            parametros.Add("i_estado ", estado);
            parametros.Add("i_usuarioCrea ", usuarioCrea);
            parametros.Add("i_usuarioModifica ", usuarioModifica);
            //var rp = BDOpe.ComandoSelectALista<BAN_TurnosCliente>(sql_punteroVBS.Conexion_LocalVBS, "[BAN_INSERTAR_TURNO_CLIENTE]", parametros);
            //return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosCliente>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosCliente>>.CrearFalla(rp.MensajeProblema);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_INSERTAR_TURNO_CLIENTE", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                ResultadoOperacion<List<BAN_TurnosCliente>>.CrearFalla(OnError);
            }

            v_secuencia = db.Value;
            OnError = string.Empty;

            var rp = BAN_TurnosDet.GetTurnoDet(idTurnoDet);
            return ResultadoOperacion<List<BAN_TurnosCliente>>.CrearResultadoExitoso(rp.SubDetalle);
        }
    
        public ResultadoOperacion<List<BAN_TurnosCliente>> GetListaTurnosCliente(long secuencia, long idDetalle)
        {
            parametros.Clear();
            parametros.Add("i_secuencia", secuencia);
            parametros.Add("i_idTurnoDet", idDetalle);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosCliente>(sql_punteroVBS.Conexion_LocalVBS, "[BAN_CONSULTAR_TURNO_CLIENTE]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosCliente>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosCliente>>.CrearFalla(rp.MensajeProblema);
        }

        public static List<BAN_TurnosCliente> listadoTurnosCliente(long idDetalle, out string OnError)
        {
            OnError = string.Empty;
            parametros.Clear();
            parametros.Add("i_idTurnoDet", idDetalle);
            var obj = sql_puntero.ExecuteSelectControl<BAN_TurnosCliente>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNO_CLIENTE", parametros, out OnError);
            return obj;
        }

    }
}

