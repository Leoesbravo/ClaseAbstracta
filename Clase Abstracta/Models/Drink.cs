namespace Clase_Abstracta.Models
{
    public class Drink
    {
        public string IdDrink { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Instrucciones { get; set; }
        public string Imagen { get; set; }
        public string Ingredientes { get; set; }
        public List<Drink> Drinks { get; set; }
    }
}
