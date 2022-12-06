using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net;

namespace SCONV_Proyecto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DialogflowController : ControllerBase
    {
        // A Protobuf JSON parser configured to ignore unknown fields. This makes
        // the action robust against new fields being introduced by Dialogflow.
        private static readonly JsonParser jsonParser =
            new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        [HttpPost]
        public async Task<ContentResult> DialogAction()
        {
            // Read the request JSON asynchronously, as the Google.Protobuf library
            // doesn't (yet) support asynchronous parsing.
            string requestJson;
            using (TextReader reader = new StreamReader(Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }

            // Parse the body of the request using the Protobuf JSON parser,
            // *not* Json.NET.
            WebhookRequest request = jsonParser.Parse<WebhookRequest>(requestJson);

            // Note: you should authenticate the request here.

            // Populate the response
            WebhookResponse response = new WebhookResponse
            {
                FulfillmentText= "Funciona webhook"
            };

            // Ask Protobuf to format the JSON to return.
            // Again, we don't want to use Json.NET - it doesn't know how to handle Struct
            // values etc.
            string responseJson = response.ToString();
            return Content(responseJson, "application/json");
        }
    }
}
