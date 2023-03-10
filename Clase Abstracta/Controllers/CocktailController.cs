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
            Models.Drink drinks = new Models.Drink();
            drinks.Drinks = new List<Models.Drink>();
            var json = _Bebidas.ConsultarCocktail("search.php?s=" + nombre);
            json.Wait();
            if(json.Result.Length > 15)
            {
                drinks.Drinks = GenerarListas(json.Result);
            }
            var jsonByIngrediente = _Bebidas.ConsultarCocktail("filter.php?i=" + nombre);
            jsonByIngrediente.Wait();
            if(jsonByIngrediente.Result.Length > 15)
            {
                drinks.Drinks.AddRange(GenerarListas(jsonByIngrediente.Result));
            }

            return View(drinks);
        }

        public List<Models.Drink> GenerarListas(string data)
        {

            Models.Drink drinks = new Models.Drink();
            drinks.Drinks = new List<Models.Drink>();
            dynamic resultJSON = JObject.Parse(data);
            foreach (var result in resultJSON.drinks)
            {
                Models.Drink drink = new Models.Drink();
                drink.IdDrink = result.idDrink;
                drink.Nombre = result.strDrink;
                drink.Imagen = result.strDrinkThumb;

                drinks.Drinks.Add(drink);
            }
            return drinks.Drinks;
        }

        public JsonResult ConsultarDetalle(string idCoctel)
        {
            var json = _Bebidas.ConsultarCocktail("lookup.php?i=" + idCoctel);
            json.Wait();

            return Json(json.Result);
        }
    }
}
