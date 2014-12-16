using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace retailer.Handler
{
    //call web api || Get the result as JsonString
    public static class WebApiHelper
    {
        private const string baseUrl = "http://118.139.162.161:800/";

        public static string GetWebApi( string apiName)
        {
            string jsonStr = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(baseUrl);//("http://localhost/yourwebapi");
                HttpResponseMessage response = client.GetAsync(apiName).Result;
                if (response.IsSuccessStatusCode)
                {
                    jsonStr = JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result);

                }
            }
            return jsonStr;
        }

        public static string PostWebApi<T>(string apiName, T data) where T : class
        {
            string jsonStr = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(baseUrl);//("http://localhost/yourwebapi");
              //  MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

                // Use the JSON formatter to create the content of the request body.
                using (HttpContent content = new ObjectContent<T>(data, new JsonMediaTypeFormatter()))
                {

                    // Send the Post request.
                    HttpResponseMessage response = client.PostAsync(apiName, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonStr = JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result);

                    }
                }
            }
            return jsonStr;
        }
    }
}