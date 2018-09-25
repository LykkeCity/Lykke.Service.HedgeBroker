using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lykke.Service.HedgeBroker.Models
{
    public class ErrorModel
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("modelErrors")]
        public Dictionary<string, List<string>> ModelErrors { get; set; }

        public ErrorModel(string message, Dictionary<string, List<string>> modelErrors = null)
        {
            ErrorMessage = message;
            ModelErrors = modelErrors;
        }
    }
}
