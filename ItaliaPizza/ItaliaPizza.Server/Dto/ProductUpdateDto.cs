namespace ItaliaPizza.Server.Dto
{
    public class ProductUpdateDto
    {
        public int Id { get; set; } 

        public string Nombre { get; set; } = string.Empty;

        public int CategoriaId { get; set; }

        public string UnidadMedida { get; set; } = string.Empty;

        public decimal CantidadMinima { get; set; }

        public decimal Precio { get; set; }

        public string? ObservacionesInventario { get; set; }

        public bool Estatus { get; set; } = true;

        public bool EsIngrediente { get; set; } = false;
    }
}
