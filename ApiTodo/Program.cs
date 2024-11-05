// http://localhost:5275/swagger/index.html
// dotnet run puis “Try it out” puis “Execute”

SeedData.Init();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// The aim of this is to add API controllers to the application
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// You as a developer can use Swagger to interact with the API through a web interface
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the database context to the application
// This will allow the application to interact with the database
builder.Services.AddDbContext<TodoContext>();

// Allows annotations on the Swagger UI documentation
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

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