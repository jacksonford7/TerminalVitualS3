using System.Collections.Generic;
using Core.Cedi.Models;

namespace Core.Cedi
{
    /// <summary>
    /// Operaciones de negocio para el módulo de despacho CEDI.
    /// </summary>
    public interface ICediDespachoService
    {
        IEnumerable<CediDespachoItem> Buscar(CediDespachoFiltro filtro);
        CediDespachoItem ObtenerDetalle(int id);
        void Asignar(int id, string usuario);
        void Despachar(int id, string usuario);
        byte[] Exportar(CediDespachoFiltro filtro);
    }
}
