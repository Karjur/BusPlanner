namespace Web.Helpers
{
    using Domain.PriceListAggregate;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class FetchApi : IFetchApi
    {
        private readonly string _apiUrl;
        private readonly string _username;
        private readonly string _password; 

        public FetchApi()
        {
            _apiUrl = "https://assignments.novater.com/v1/bus/schedule";
            _username = "karmo";
            _password = "71b4b3253c40c6f59cc4af5b9005d105";
        }

        public async Task<PriceList> FetchPriceListAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string base64Credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_username}:{_password}"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");

                    HttpResponseMessage response = await client.GetAsync(_apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        PriceList priceList = JsonConvert.DeserializeObject<PriceList>(json);

                        
                        return priceList;
                    }
                    else
                    {
                        // Handle non-successful response
                        throw new Exception($"API request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}