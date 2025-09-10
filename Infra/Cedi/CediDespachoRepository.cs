using System.Collections.Generic;
using Core.Cedi.Models;

namespace Infra.Cedi
{
    /// <summary>
    /// Repositorio ADO.NET/EF para acceder a datos del módulo CEDI.
    /// Los métodos están listos para invocar procedimientos almacenados en el esquema cedi.*
    /// </summary>
    public class CediDespachoRepository
    {
        private readonly string _connectionString;

        public CediDespachoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<CediDespachoItem> Buscar(CediDespachoFiltro filtro)
        {
            // TODO: Implementar llamada a cedi.buscar_despacho
            return new List<CediDespachoItem>();
        }

        public CediDespachoItem ObtenerDetalle(int id)
        {
            // TODO: Implementar llamada a cedi.obtener_despacho
            return new CediDespachoItem();
        }

        public void Asignar(int id, string usuario, string contexto)
        {
            // TODO: Implementar llamada a cedi.asignar_despacho
        }

        public void Despachar(int id, string usuario, string contexto)
        {
            // TODO: Implementar llamada a cedi.despachar
        }

        public byte[] Exportar(CediDespachoFiltro filtro)
        {
            // TODO: Implementar exportación cedi.exportar_despacho
            return new byte[0];
        }
    }
}
