using SCONV_Proyecto;

//Inicializar base de datos
FachadaBbdd.GetSingleton();

var a = FachadaBbdd.GetSingleton().GetEstablecimientos();

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
//Iniciar aplicaci�n
app.Run();
