using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net;

namespace SCONV_Proyecto.Controladores
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
            string requestJson;
            using (TextReader reader = new StreamReader(Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }

            WebhookRequest request = jsonParser.Parse<WebhookRequest>(requestJson);

            WebhookResponse respuesta = GestorDialogo.GenerarRespuesta(request);

            string respuestaJson = respuesta.ToString();
            return Content(respuestaJson, "application/json");
        }
    }
}
