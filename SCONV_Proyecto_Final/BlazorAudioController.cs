using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SCONV_Proyecto_Final
{
    
    [ApiController]
    [Route("[controller]")]
    public class BlazorAudioController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Save(IFormFile file)
        {
            if (file.ContentType != "audio/wav")
            {
                return BadRequest("Wrong file type");
            }

            string ruta = "audios/";
            
            var filePath = Path.Combine(ruta, file.FileName + ".wav");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return Ok("File uploaded successfully");
        }

    }
}
