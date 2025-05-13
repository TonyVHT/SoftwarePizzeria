namespace ItaliaPizza.Server.PlatilloModulo
{
    public class IngredienteDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;  
    }

}
