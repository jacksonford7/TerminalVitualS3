using System.Collections.Generic;
using Core.Cedi.Models;
using Infra.Cedi;

namespace Core.Cedi
{
    /// <summary>
    /// Implementación de la lógica de negocio para CEDI.
    /// </summary>
    public class CediDespachoService : ICediDespachoService
    {
        public const string ContextoModulo = "CEDI";

        private readonly CediDespachoRepository _repository;

        public CediDespachoService(CediDespachoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CediDespachoItem> Buscar(CediDespachoFiltro filtro)
        {
            return _repository.Buscar(filtro);
        }

        public CediDespachoItem ObtenerDetalle(int id)
        {
            return _repository.ObtenerDetalle(id);
        }

        public void Asignar(int id, string usuario)
        {
            _repository.Asignar(id, usuario, ContextoModulo);
        }

        public void Despachar(int id, string usuario)
        {
            _repository.Despachar(id, usuario, ContextoModulo);
        }

        public byte[] Exportar(CediDespachoFiltro filtro)
        {
            return _repository.Exportar(filtro);
        }
    }
}
