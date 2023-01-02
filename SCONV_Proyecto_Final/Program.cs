using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SCONV_Proyecto_Final.Data;

namespace SCONV_Proyecto_Final
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.MapControllers();
            BorrarAudiosAntiguos();
            app.Run();
        }

        private static void BorrarAudiosAntiguos()
        {
            DirectoryInfo directorioServidor = new DirectoryInfo("audios");
            foreach (FileInfo file in directorioServidor.GetFiles())
            {
                file.Delete();
            }
            DirectoryInfo directorioJavascript = new DirectoryInfo("wwwroot/audios");
            foreach (FileInfo file in directorioJavascript.GetFiles())
            {
                file.Delete();
            }
        }
    }
}