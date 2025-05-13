namespace ItaliaPizza.Server.PlatilloModulo
{
    public class RecetaDto
    {
        public int PlatilloId { get; set; }
        public string Instrucciones { get; set; } = string.Empty;
        public List<IngredienteRecetaDto> Ingredientes { get; set; } = new();
    }
}
