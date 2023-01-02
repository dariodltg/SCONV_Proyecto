using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SCONV_Proyecto_Final
{
    
    [ApiController]
    [Route("[controller]")]
    public class BlazorAudioController : ControllerBase
    {

        /// <summary>
        /// Recibe un audio grabado en el frontend y lo guarda en el servidor.
        /// </summary>
        /// <param name="file">El audio en formato. wav</param>
        [HttpPost]
        public async Task<IActionResult> Save(IFormFile file)
        {
            if (file.ContentType != "audio/wav")
            {
                return BadRequest("Wrong file type");
            }

            string ruta = "audios/";
            int etapaInteraccion = Utilidades.etapaInteraccion;
            var filePath = Path.Combine(ruta, file.FileName + "_"+etapaInteraccion+".wav");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return Ok("File uploaded successfully");
        }

    }
}
