namespace Core.Cedi.Models
{
    /// <summary>
    /// Filtros de búsqueda para el despacho CEDI.
    /// </summary>
    public class CediDespachoFiltro
    {
        public string Mrn { get; set; }
        public string Msn { get; set; }
        public string Hsn { get; set; }
    }
}
