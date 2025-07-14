using NadinSoft.CRUD.Infrastructure;
using NadinSoft.CRUD.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddCustomService();
builder.Services.AddRepositories();
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();