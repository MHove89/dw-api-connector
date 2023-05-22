using DW.Api.Connector.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace DW.Api.Connector.Services
{
    public class CustomPropertyService : EnterspeedPropertyService
    {
        protected override void MapAdditionalMetaProperties(Dictionary<string, IEnterspeedProperty> data, Page page, string culture)
        {
            var dateAsString = page.Item.Fields.Find(f => f.SystemName == "Dato");

            if (DateTime.TryParse(dateAsString?.Value.ToString(), out DateTime parsedDate))
            {
                data.Add("filteringDate", new StringEnterspeedProperty(parsedDate.Year.ToString() + parsedDate.Month.ToString()));
            }
        }
    }
}
