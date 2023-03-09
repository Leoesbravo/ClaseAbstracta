using Clase_Abstracta.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Clase_Abstracta.Controllers
{
    public  abstract class BebidasAbstract 
    {    
        public readonly string _baseUrl = "https://www.thecocktaildb.com/api/json/v1/1/";
        public  async Task<string> ConsultarCocktail(string name)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(_baseUrl + name);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al realizar la petición al endpoint: {response.ReasonPhrase}");
            }
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
