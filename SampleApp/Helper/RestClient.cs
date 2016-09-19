using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Helper
{
    public static class RestClient
    {
        
        public static readonly string BaseUri = ConfigurationManager.AppSettings["BaseUri"].ToString();
        //public static readonly string Username = ConfigurationManager.AppSettings["Username"].ToString();
        public static readonly string Key = ConfigurationManager.AppSettings["API-KEY"].ToString();
        public static readonly string playerkey = "Basic N2NjN2JlNjgtNWMyMC00YTZiLTgxYWUtYTQ0ODkxMWExZTVk";

        public static async Task<T> GetResponse<T>(T model, string endPoint)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model));

                //client.DefaultRequestHeaders.Add("Username", Username);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", Key);
                var result = await client.GetAsync(BaseUri + endPoint);
                var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
               

                return response;
            }
        }


        public static async Task<T> PostResponse<T>(T model, string endPoint)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(model));
                    //client.DefaultRequestHeaders.Add("Username", Username);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    client.DefaultRequestHeaders.Add("Authorization", Key);
                    var result = await client.PostAsync(BaseUri + endPoint, content);
                   /// var res = result;

                   
                    var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
                    return response;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public static async Task<T> PutResponse<T>(T model, string endPoint)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(model));
                    //client.DefaultRequestHeaders.Add("Username", Username);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    client.DefaultRequestHeaders.Add("Authorization", Key);
                    var result = await client.PutAsync(BaseUri + endPoint, content);
                    var response = JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
                    return response;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}