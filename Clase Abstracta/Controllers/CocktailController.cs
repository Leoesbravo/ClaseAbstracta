using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Clase_Abstracta.Controllers
{
    public class CocktailController : Controller
    {
        private readonly BebidasAbstract _Bebidas; 
        public CocktailController(BebidasAbstract bebidas)
        {
            _Bebidas = bebidas;
        }

        public ActionResult Index()
        {
            Models.Drink drink = new Models.Drink();
            drink.Drinks = new List<Models.Drink>();
            return View(drink);
        }
        [HttpPost]
        public ActionResult Index(string nombre)
        {
            var result = "";
            Models.Drink drinks = new Models.Drink();

            var json = _Bebidas.ConsultarCocktail("search.php?s="+nombre);
            json.Wait();
            if(json.Result.Length < 20)
            {
                var jsonByIngrediente = _Bebidas.ConsultarCocktail("filter.php?i="+nombre);
                jsonByIngrediente.Wait();
                if (jsonByIngrediente.Result.Length > 20)
                {
                     result = jsonByIngrediente.Result;
                    ViewBag.Mensaje = "Lista de Cocteles que incluyen " + nombre;
                }
                else
                {
                    return View(drinks);
                }
            }
            else
            {
                ViewBag.Mensaje = "Lista de Cocteles con el nombre " + nombre;
                result = json.Result;
            }
            drinks.Drinks = new List<Models.Drink>();
            dynamic resultJSON = JObject.Parse(result);
            foreach (var resultItem in resultJSON.drinks)
            {
                Models.Drink drink = new Models.Drink();
                drink.IdDrink = resultItem.idDrink;
                drink.Nombre = resultItem.strDrink;
                drink.Categoria = resultItem.strCategory;
                drink.Instrucciones = resultItem.strInstructions;
                drink.Imagen = resultItem.strDrinkThumb;
                drink.Ingredientes = resultItem.strIngredient1 + ", " + resultItem.strIngredient2 + ", " + resultItem.strIngredient3 + ", " + resultItem.strIngredient4;

                drinks.Drinks.Add(drink);
            }
            return View(drinks);
        }
    }
}
