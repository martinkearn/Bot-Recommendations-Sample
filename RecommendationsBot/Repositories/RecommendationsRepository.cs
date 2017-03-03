using Newtonsoft.Json;
using RecommendationsBot.Interfaces;
using RecommendationsBot.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace RecommendationsBot.Repositories
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private string _baseItemToItemApiUrl;
        private AppSettings _appSettings;

        public RecommendationsRepository()
        {
            var appSettings = new AppSettings()
            {
                RecommendationsApiBaseUrl = ConfigurationManager.AppSettings["RecommendationsApiBaseUrl"],
                RecommendationsApiKey = ConfigurationManager.AppSettings["RecommendationsApiKey"],
                RecommendationsApiModelId = ConfigurationManager.AppSettings["RecommendationsApiModelId"],
                RecommendationsApiITIBuildId = ConfigurationManager.AppSettings["RecommendationsApiITIBuildId"],
                RecommendationsApiFBTBuildId = ConfigurationManager.AppSettings["RecommendationsApiFBTBuildId"],
            };
            _appSettings = appSettings;
            _baseItemToItemApiUrl = _appSettings.RecommendationsApiBaseUrl.Replace("MODELID", _appSettings.RecommendationsApiModelId);
        }

        /// <summary>
        /// Helper function to call the Cognitive Recommendations API with an Item-to-Item build
        /// </summary>
        /// <param name="id">ItemId to seed recommendations on</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <param name="minimalScore">Minimal score for results to be included</param>
        /// <returns>RecomendedItems object - a list of RecommendItems</returns>
        public async Task<RecommendedItems> GetITIItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _appSettings.RecommendationsApiITIBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            return recomendedItems;
        }

        /// <summary>
        /// Helper function to call the Cognitive Recommendations API with an Frequently-Bought-Together build
        /// </summary>
        /// <param name="id">ItemId to seed recommendations on</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <param name="minimalScore">Minimal score for results to be included</param>
        /// <returns>RecomendedItems object - a list of RecommendItems</returns>
        public async Task<RecommendedItems> GetFBTItems(string id, string numberOfResults, string minimalScore)
        {
            var responseContent = await CallRecomendationsApi(id, numberOfResults, minimalScore, _appSettings.RecommendationsApiFBTBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);
            return recomendedItems;
        }

        private async Task<string> CallRecomendationsApi(string id, string numberOfResults, string minimalScore, string buildId)
        {
            //construct API parameters
            var parameters = new NameValueCollection {
                { "itemIds", id},
                { "numberOfResults", numberOfResults },
                { "minimalScore", minimalScore },
            };

            //Only add build ID if it is not empty. buildId is an optional field and API defaults to active build if it is ommited
            if (!String.IsNullOrEmpty(buildId))
            {
                parameters.Add("buildId", buildId);
            }

            //construct full API endpoint uri
            var apiUri = _baseItemToItemApiUrl + ToQueryString(parameters);

            //get item to item recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }
    }
}