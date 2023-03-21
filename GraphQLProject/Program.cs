using GraphiQl;
using GraphQL.NewtonsoftJson;
using GraphQL.Server;
using GraphQL.Types;
using GraphQLProject.Data;
using GraphQLProject.Interfaces;
using GraphQLProject.Mutation;
using GraphQLProject.Query;
using GraphQLProject.Schema;
using GraphQLProject.Services;
using GraphQLProject.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IProduct, ProductService>();

builder.Services.AddTransient<ProductType>();
builder.Services.AddTransient<ProductQuery>();
builder.Services.AddTransient<ProductMutation>();
builder.Services.AddTransient<ISchema, ProductSchema>();

builder.Services.AddGraphQL(options =>
{
    options.EnableMetrics = false;
}).AddSystemTextJson();

builder.Services.AddDbContext<GraphQLDbContext>(option => option.UseSqlServer(@"Data Source= (localdb)\MSSQLLocalDB;Initial Catalog=GraphQLDb;Integrated Security = True"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<GraphQLDbContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
   // app.UseSwaggerUI();
}

dbContext.Database.EnsureCreated();
app.UseGraphiQl("/graphql");
app.UseGraphQL<ISchema>();

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
