using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProgressusWebApi.DataContext;
using ProgressusWebApi.Services.AuthServices;
using ProgressusWebApi.Services.AuthServices.Interfaces;
using ProgressusWebApi.Services.EmailServices;
using ProgressusWebApi.Services.EmailServices.Interfaces;
using ProgressusWebApi.Utilities;
using Swashbuckle.AspNetCore.Filters;
using WebApiMercadoPago.Repositories.Interface;
using WebApiMercadoPago.Repositories;
using WebApiMercadoPago.Services.Interface;
using WebApiMercadoPago.Services;
using MercadoPago.Config;
using ProgressusWebApi.Repositories.EjercicioRepositories;
using ProgressusWebApi.Repositories.EjercicioRepositories.Interfaces;
using ProgressusWebApi.Services.EjercicioServices.Interfaces;
using ProgressusWebApi.Services.EjercicioServices;
using ProgressusWebApi.Repositories.Interfaces;
using ProgressusWebApi.Repositories.PlanEntrenamientoRepositories;
using ProgressusWebApi.Services.PlanEntrenamientoServices.Interfaces;
using ProgressusWebApi.Services.PlanEntrenamientoServices;
using ProgressusWebApi.Repositories.PlanEntrenamientoRepositories.Interfaces;
using ProgressusWebApi.Repositories.MembresiaRepositories.Interfaces;
using ProgressusWebApi.Repositories.MembresiaRepositories;
using ProgressusWebApi.Services.MembresiaServices.Interfaces;
using ProgressusWebApi.Services.MembresiaServices;
using ProgressusWebApi.Services.ReservaService.cs.interfaces;
using ProgressusWebApi.Services.ReservaService;
using ProgressusWebApi.Services.ReservaServices;
using ProgressusWebApi.Services.InventarioServices;
using ProgressusWebApi.Services.InventarioServices.Interfaces;
using ProgressusWebApi.Services.NotificacionesServices;
using ProgressusWebApi.Services.NotificacionesServices.Interfaces;
using ProgressusWebApi.Services.MerchServices;
using ProgressusWebApi.Services.MerchServices.Interfaces;
using ProgressusWebApi.Repositories.NotificacionesRepositories.Interfaces;
using ProgressusWebApi.Repositories.NotificacionesRepositories;
using ProgressusWebApi.Services.AlimentoServices;
using ProgressusWebApi.Services.CarritoServices;
using ProgressusWebApi.Services.PedidosServices.Interfaces;
using ProgressusWebApi.Services.PedidosServices;
using ProgressusWebApi.Repositories.QrCodeRepositories;
using ProgressusWebApi.Services.QrCodeServices;

var builder = WebApplication.CreateBuilder(args);
// Configuración de CORS para aceptar peticiones de cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Permite cualquier origen
              .AllowAnyHeader() // Permite cualquier encabezado
              .AllowAnyMethod(); // Permite cualquier método (GET, POST, etc.)
    });
});
// Agregar los servicios al contenedor
// Configuración para ignorar referencias cíclicas en la serialización JSON
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Inyección de repositorios y servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEjercicioRepository, EjercicioRepository>();
builder.Services.AddScoped<IEjercicioService, EjercicioService>();
builder.Services.AddScoped<IMusculoDeEjercicioRepository, MusculoDeEjercicioRepository>();
builder.Services.AddScoped<IMusculoDeEjercicioService, MusculoDeEjercicioService>();
builder.Services.AddScoped<IMusculoRepository, MusculoRepository>();
builder.Services.AddScoped<IMusculoService, MusculoService>();
builder.Services.AddScoped<IGrupoMuscularRepository, GrupoMuscularRepository>();
builder.Services.AddScoped<IGrupoMuscularService, GrupoMuscularService>();
builder.Services.AddScoped<IPlanDeEntrenamientoRepository, PlanDeEntrenamientoRepository>();
builder.Services.AddScoped<IPlanDeEntrenamientoService, PlanDeEntrenamientoService>();
builder.Services.AddScoped<IDiaDePlanRepository, DiaDePlanRepository>();
builder.Services.AddScoped<IEjercicioEnDiaDelPlanRepository, EjercicioEnDiaDelPlanRepository>();
builder.Services.AddScoped<IObjetivoDelPlanRepository, ObjetivoDelPlanRepository>();
builder.Services.AddScoped<ISerieDeEjercicioRepository, SerieDeEjercicioRepository>();
builder.Services.AddScoped<IObjetivoDelPlanService, ObjetivoDelPlanService>();
builder.Services.AddScoped<IAsignacionDePlanRepository, AsignacionDePlanRepository>();
builder.Services.AddScoped<IAsignacionDePlanService, AsignacionDePlanService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IMembresiaRepository, MembresiaRepository>();
builder.Services.AddScoped<IMembresiaService, MembresiaService>();
builder.Services.AddScoped<ISolicitudDePagoRepository, SolicitudDePagoRepository>();
builder.Services.AddScoped<ISolicitudDePagoService, SolicitudDePagoService>();
builder.Services.AddScoped<AlimentoCalculoService>();
builder.Services.AddScoped<QrCodeRepository>();
builder.Services.AddScoped<QrCodeService>();

builder.Services.AddMemoryCache();

// Permitir documentación y acceso de los endpoints con swagger
// Configuración con oauth2 para requerir autorización en la ejecución de los endpoints 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Registrar IHttpClientFactory
builder.Services.AddHttpClient();

// Conexión a la base de datos
builder.Services.AddDbContext<ProgressusDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticación y autorización con Identity (endpoints)
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ProgressusDataContext>();

// Configuración para envio de emails con servidor SMTP de Gmail
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.Configure<GmailSetter>(builder.Configuration.GetSection("GmailSettings"));

// Configuración para sistema de cobros con MercadoPago

builder.Services.AddScoped<IMercadoPagoRepository, MercadoPagoRepository>();
builder.Services.AddScoped<IMercadoPagoService, MercadoPagoService>(); MercadoPagoConfig.AccessToken = "APP_USR-2278733141716614-062815-583c9779901a7bbf32c8e8a73971e44c-1878150528";
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<INotificacionesService, NotificacionesService>();
builder.Services.AddScoped<IMerchService, MerchService>();
// Registrar servicios
builder.Services.AddScoped<ICarritoService, CarritoService>();

//Notificaciones services - repository
builder.Services.AddScoped<INotificacionesUsuariosService,NotificacionesUsuariosService>();
builder.Services.AddScoped<IEstadosNotificacionesService,EstadosNotificacionesService>();
builder.Services.AddScoped<IPlantillasService,PlantillasService>();
builder.Services.AddScoped<ITiposNotificacionesService,TiposNotificacionesService>();

builder.Services.AddScoped<IEstadoNotificacionRepository, EstadoNotificacionRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<IPlantillaRepository, PlantillaRepository>();
builder.Services.AddScoped<ITipoNotificacionRepository, TipoNotificacionRepository>();




// Construir la aplicación con todas las configuraciones y servicios definidos en el objeto builder
var app = builder.Build();

// Configuración de la pipeline del HTTP request
// Comentamos esta linea para poder deployar en produccion
//if (app.Environment.IsDevelopment())
//{
    //app.UseSwagger();
    //app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

// Aplicar CORS para permitir cualquier origen
app.UseCors("AllowAllOrigins");
// Mapear endpoints de authorization y authentication
app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
