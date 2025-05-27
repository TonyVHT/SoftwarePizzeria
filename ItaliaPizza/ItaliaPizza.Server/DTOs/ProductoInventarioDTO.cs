namespace ItaliaPizza.Server.DTOs
{
    public class ProductoInventarioDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal CantidadActual { get; set; }
        public decimal CantidadMinima { get; set; }
        public decimal Precio { get; set; }
        public string? ObservacionesInventario { get; set; }
        public bool EsIngrediente { get; set; }
        public bool Estatus { get; set; }
    }
}
