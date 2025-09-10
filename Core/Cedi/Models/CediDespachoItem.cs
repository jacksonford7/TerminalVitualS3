namespace Core.Cedi.Models
{
    /// <summary>
    /// Representa un registro de despacho CEDI.
    /// </summary>
    public class CediDespachoItem
    {
        public int Id { get; set; }
        public int Secuencia { get; set; }
        public string Contenedor { get; set; }
    }
}
