namespace Web.Helpers
{
    using Domain.PriceListAggregate;
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Security.AccessControl;

    public class FetchApi : IFetchApi
    {
        private readonly string _apiUrl;
        private readonly string _username;
        private readonly string _password; 

        public FetchApi(string apiUrl, string username, string password)
        {
            _apiUrl = apiUrl;
            _username = username;
            _password = password;
        }

        public async Task<PriceList> FetchPriceListAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set up basic authentication
                    string base64Credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_username}:{_password}"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");

                    // Send a GET request to the API
                    HttpResponseMessage response = await client.GetAsync(_apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the JSON response into a PriceList object
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
                // Handle exceptions, e.g., log the error or throw an exception.
                throw ex;
            }
        }
    }

}
