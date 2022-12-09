using SCONV_Proyecto;
using System.Runtime.CompilerServices;

//Inicializar base de datos
FachadaBbdd.GetSingleton();

//Iniciar Ngrok
//Ngrok.IniciarNGrok();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//Iniciar aplicación
app.Run();
