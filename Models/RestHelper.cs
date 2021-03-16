using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
	public class RestHelper // <T> Detta kan vidareutvecklas
	{
		// Skapar en statisk httpclient som kan användas genom hela projektet.
		private static readonly HttpClient client = new HttpClient();
		// Connectionstring till api.

		private static readonly string api = "http://informatik12.ei.hv.se/grupp5v2/api/";

		//private static readonly string api = "http://informatik12.ei.hv.se/grupp5/api/"; DEPRECATED

		
		// API / GET
		public static async Task<List<T>> ApiGet<T>(string apiPath)
		{
			// Generisk lista som kan spara olika typer av klasser.
			var returnList = new List<T>();
			try
			{
				var res = await client.GetAsync(api + apiPath);
				
				var jRes = await res.Content.ReadAsStringAsync();

				returnList = JsonConvert.DeserializeObject<List<T>>(jRes);
			
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Something went wrong!!!: \n \t " + e.ToString());
				throw;
			}

			return returnList;
		}
	
		// API / GET / ID - Överladdad get-metod för att hämta enskilt objekt.
		public static async Task<T> ApiGet<T>(string apiPath, int? id)
		{
			var res = await client.GetAsync(api + apiPath + id);

			var jRes = await res.Content.ReadAsStringAsync();
			
			var returnObj = JsonConvert.DeserializeObject<T>(jRes);

			return returnObj;
			 
		} 
		// API / POST
		public static async Task ApiCreate<T>(string apiPath, T newObj)
		{
			await client.PostAsJsonAsync(api + apiPath, newObj);
		}

		// API / PUT
		public static async Task ApiEdit<T>(string apiPath, T oldObj)
		{
			await client.PutAsJsonAsync(api + apiPath, oldObj);
		}

		// API / DELETE
		public static async Task ApiDelete<T>(string apiPath, int? id)
		{
			await client.DeleteAsync(api + apiPath + id);
		}
	/*	public static async Task<T> ApiGetChildren<T>(string apiPath, int? fk)
		{

			


			return childObj;
		}*/

		



	}
}
