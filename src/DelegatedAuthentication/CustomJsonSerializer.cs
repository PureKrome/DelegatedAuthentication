using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WorldDomination.DelegatedAuthentication
{
    internal sealed class CustomJsonSerializer : JsonSerializer
    {
        internal CustomJsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.Indented;
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            NullValueHandling = NullValueHandling.Ignore;
            Converters.Add(new StringEnumConverter());
        }
    }
}