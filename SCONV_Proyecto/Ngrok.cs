using System.Diagnostics;

namespace SCONV_Proyecto
{
    public class Ngrok
    {
        public static void IniciarNGrok()
        {
            ProcessStartInfo ngrok = new ProcessStartInfo();
            ngrok.FileName = "ngrok.exe";
            ngrok.Arguments = "http https://localhost:7228";
            ngrok.CreateNoWindow= false;
            ngrok.UseShellExecute= true;
            Process procesoNgrok = Process.Start(ngrok);
        }
    }
}
