using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecommendationsBot.Models
{
    public class AppSettings
    {
        public string RecommendationsApiBaseUrl { get; set; }

        public string RecommendationsApiKey { get; set; }

        public string RecommendationsApiModelId { get; set; }

        public string RecommendationsApiITIBuildId { get; set; }

        public string RecommendationsApiFBTBuildId { get; set; }
    }
}