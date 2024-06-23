using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PROJETO_CLIMA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClimaTempoController : ControllerBase
    {
        public string Id_Cidade;
        public string xToken;



        [HttpGet("BuscaID_Cidade/{Nome_Cidade}/{Estado}")]
        public async Task BuscaID_CidadeAsync(string Nome_Cidade, string Estado)
        {
            ClimaTempo Clima = new ClimaTempo();
            xToken = Clima.iTOKEN;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://apiadvisor.climatempo.com.br/api/v1/locale/city?name="+ Nome_Cidade +"&state="+ Estado +"&token="+ xToken +"");

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            dynamic DeserializeJson = JsonConvert.DeserializeObject(responseBody);

            Console.WriteLine($"{DeserializeJson}");

            Id_Cidade = DeserializeJson[0].Value<string>("id"); 

            Console.WriteLine(Id_Cidade);


        }

        [HttpGet("Status_Pais")]

        public async Task BuscaStatus_Pais()
        {

            ClimaTempo Clima = new ClimaTempo();
            xToken = Clima.iTOKEN;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://apiadvisor.climatempo.com.br/api/v1/anl/synoptic/locale/BR?token="+ xToken +"");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            dynamic DeserializeJson = JsonConvert.DeserializeObject(responseBody);

            Console.WriteLine($"{DeserializeJson}");


        }

        [HttpPut("Cadastrar_Cidade/{Id_Cidade}")]
        public async Task Cadastrar_Cidade(string Id_Cidade)
        {
            ClimaTempo Clima = new ClimaTempo();
            xToken = Clima.iTOKEN;

            string data = "localeId[]="+ Id_Cidade;

            StringContent postData = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");


     
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PutAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/"+ xToken + "/locales", postData);
 

            response.EnsureSuccessStatusCode();

            Console.WriteLine(response);



        }


        [HttpGet("UserToken")]
        public async Task UserToken()
        {
            ClimaTempo Clima = new ClimaTempo();
            xToken = Clima.iTOKEN;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://apiadvisor.climatempo.com.br/api-manager/user-token/" + xToken + "/locales");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);

            dynamic DeserializeJson = JsonConvert.DeserializeObject(responseBody);

            Console.WriteLine($"{DeserializeJson}");



        }


        [HttpGet("Tempo_Na_Cidade/{Id_Cidade}")]
        public async Task<IActionResult> Clima_Atual(string Id_Cidade)
        {
            ClimaTempo Clima = new ClimaTempo();
            xToken = Clima.iTOKEN;

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://apiadvisor.climatempo.com.br/api/v1/weather/locale/" + Id_Cidade + "/current?token=" + xToken);

                // Verifique se o código de status é 200
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonString);
                    Item item = JsonConvert.DeserializeObject<Item>(jsonString);

                    return Ok(item);
                }
                else
                {
                    // Log detalhado do erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
            }
            catch (HttpRequestException e)
            {
                // Log detalhado do erro
                return StatusCode(500, $"Erro ao chamar a API externa: {e.Message}");
            }
        }







        [HttpGet("Tempo15D/{Id_Cidade}")]
        public async Task<IActionResult> Clima15D(string Id_Cidade)
        {
            ClimaTempo Clima = new ClimaTempo();
            xToken = Clima.iTOKEN;

            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync("http://apiadvisor.climatempo.com.br/api/v1/forecast/locale/" + Id_Cidade + "/days/15?token=" + xToken);



                // Verifique se o código de status é 200
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonString);
                    // Parse the JSON response
                    var jsonResponse = JObject.Parse(jsonString);
                    var data = jsonResponse["data"] as JArray;

                    var forecasts = data.Select(f => new ClimaItem
                    {
                        Date = (string)f["date"],
                        Temperature = new Temperature
                        {
                            Min = (double)f["temperature"]["min"],
                            Max = (double)f["temperature"]["max"]
                        },
                        Description = (string)f["text_icon"]["text"]["pt"] // Extraindo a descrição em "pt"
                    }).ToList();

                    return Ok(forecasts);
                }
                else
                {
                    // Log detalhado do erro
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
            }
            catch (HttpRequestException e)
            {
                // Log detalhado do erro
                return StatusCode(500, $"Erro ao chamar a API externa: {e.Message}");
            }
        }




    }
}
