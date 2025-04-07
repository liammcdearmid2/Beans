using Beans.Controllers;
using Beans.Services;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(BeansController).Assembly);

//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register the service
builder.Services.AddScoped<IBeanService, BeanService>();

//Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Register IBeanRepository with the connection string
builder.Services.AddScoped<IBeanRepository>(provider => new BeanRepository(connectionString));
var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
