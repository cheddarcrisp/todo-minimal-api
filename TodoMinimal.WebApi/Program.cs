const string BASE_URL = "http://localhost:5143";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

var repo = new TodoRepo();
var service = new TodoService(BASE_URL, repo);

app.MapGet("/", service.All)
.WithName("GetTodos")
.WithOpenApi();

app.MapGet("/{id}", service.Find)
.WithName("GetTodo")
.WithOpenApi();

app.MapPost("/", service.Add)
.WithName("CreateTodo")
.WithOpenApi();

app.MapDelete("/", service.Clear)
.WithName("DeleteTodos")
.WithOpenApi();

app.MapPatch("/{id}", service.Update)
.WithName("UpdateTodo")
.WithOpenApi();

app.MapDelete("/{id}", service.Delete)
.WithName("DeleteTodo")
.WithOpenApi();

app.Run(BASE_URL);
