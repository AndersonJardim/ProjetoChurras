using Microsoft.Extensions.DependencyInjection;
using ProjetoChurras.Extensions;
using ProjetoChurras.Repository;

var builder = WebApplication.CreateBuilder(args);

//CosmosDBExtensions.AddDataBase(builder.Configuration);
builder.Configuration.AddDataBase();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddTransient<CosmosDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

