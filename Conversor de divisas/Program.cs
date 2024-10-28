using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class ConversorDivisas
{
    private static readonly HttpClient client = new HttpClient();
    static async Task Main(string[] args)
    {
        //Lista de divisas disponibles
        List<string> currencyPrefixes = new List<string>
            {
                "USD - Dólar estadounidense",
                "EUR - Euro",
                "JPY - Yen japonés",
                "GBP - Libra esterlina",
                "AUD - Dólar australiano",
                "CAD - Dólar canadiense",
                "CHF - Franco suizo",
                "CNY - Yuan chino",
                "SEK - Corona sueca",
                "NZD - Dólar neozelandés",
                "MXN - Peso mexicano",
                "SGD - Dólar de Singapur",
                "HKD - Dólar de Hong Kong",
                "NOK - Corona noruega",
                "KRW - Won surcoreano",
                "TRY - Lira turca",
                "RUB - Rublo ruso",
                "INR - Rupia india",
                "BRL - Real brasileño",
                "ZAR - Rand sudafricano",
                "ARS - Peso argentino",
                "CLP - Peso chileno",
                "PEN - Sol peruano",
                "COP - Peso colombiano",
                "UYU - Peso uruguayo"
            };

        Console.WriteLine(":::::::Lista de prefijos de monedas disponibles:::::::");
        foreach (var currency in currencyPrefixes)
        {
            Console.WriteLine(currency);
        }

        //Entrada de datos
        Console.WriteLine("\nIngrese la divisa origen:");
        string divisaOrigen = Console.ReadLine().ToUpper();

        Console.WriteLine("\nIngrese la divisa destino:");
        string divisaDestino = Console.ReadLine().ToUpper();

        //Bloque de validacion
        while (true)
        {
            try
            {
                Console.WriteLine($"\n\nIngrese la cantidad a cambiar:[{divisaOrigen}]");
                decimal cantidad = Convert.ToDecimal(Console.ReadLine());
                if (cantidad <= 0)
                {
                    throw new Exception("Ingrese un numero mayor a 0");
                }
                decimal resultado = await convertirDivisa(divisaOrigen, divisaDestino, cantidad);

                Console.WriteLine($"\n{cantidad} {divisaOrigen} son equivalentes a {resultado}{divisaDestino}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            break; // Si el monto ingresado es mayor a 0 puede seguir el programa
        }      
    }

    private static async Task<decimal> convertirDivisa(string divisaOrigen, string divisaDestino, decimal cantidad)
    {
        //URL  de la API
        string url = $"https://api.currencyapi.com/v3/latest?apikey=cur_live_RAAfsWfD4Vadxnv3I0egMHuy7nWoWAInbr1lGD74&currencies={divisaDestino}&base_currency={divisaOrigen}";

  
        HttpResponseMessage respuesta = await client.GetAsync(url);
        respuesta.EnsureSuccessStatusCode();

        string cuerpoRespuesta = await respuesta.Content.ReadAsStringAsync();

        //Le damos formato a la respuesta parseando a JSON
        JObject json = JObject.Parse(cuerpoRespuesta);

        decimal tasaCambio = (decimal)json["data"][divisaDestino]["value"]; //Obtenemos los datos que buscamos

        Console.WriteLine($"\n:::Tasa de cambio: {tasaCambio}:::");

        return cantidad * tasaCambio; //retornamos el resultado de la operacion
    }
}