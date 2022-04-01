using DataAccess.Repository;
using DataAccess.Tools;
using DomainModel.Interface.DataProviderRepository;
using Facade;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataProviderFacade, DataProviderFacade>();
builder.Services.AddScoped<IDataProviderRepository, DataProviderRepository>();
builder.Services.AddScoped<ISqlDataAccessDapper, SqlDataAccessDapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(
        options =>
    {
        options.SerializeAsV2 = true;
    }
        );
    app.UseSwaggerUI(
    //    options =>
    //{
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //    options.RoutePrefix = string.Empty;
    //}
        );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
