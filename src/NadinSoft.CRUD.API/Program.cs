using Microsoft.Extensions.Options;
using NadinSoft.CRUD.API.Extensions;
using NadinSoft.CRUD.API.Middleware;
using NadinSoft.CRUD.Application;
using NadinSoft.CRUD.Infrastructure;
using NadinSoft.CRUD.Infrastructure.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("LoadEnv"))
{
    DotNetEnv.Env.Load();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen()
    .AddCustomSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.MigrateDb();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();