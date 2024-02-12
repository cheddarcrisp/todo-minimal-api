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

app.MapGet("/", () =>
{
    return repo.All().Select(x => MapTodo(x));
})
.WithName("GetTodos")
.WithOpenApi();

app.MapGet("/{id}", (Guid id) =>
{
    return MapTodo(repo.Find(id));
})
.WithName("GetTodo")
.WithOpenApi();

app.MapPost("/", (Todo todo) =>
{
    return MapTodo(repo.Add(todo));
})
.WithName("CreateTodo")
.WithOpenApi();

app.MapDelete("/", () => {
    repo.Clear();
})
.WithName("DeleteTodos")
.WithOpenApi();

app.MapPatch("/{id}", (Guid id, ApiTodo todo) =>
{
    return MapTodo(repo.Update(id, todo.Title, todo.Order, todo.Completed));
})
.WithName("UpdateTodo")
.WithOpenApi();

app.MapDelete("/{id}", (Guid id) => {
    repo.Delete(id);
})
.WithName("DeleteTodo")
.WithOpenApi();

app.Run(BASE_URL);

ApiTodo MapTodo(Todo todo) {
    return new(
        todo.Order,
        todo.Title,
        todo.Completed,
        $"{BASE_URL}/{todo.Id}"
    );
}

public record ApiTodo(int? Order, string? Title, bool? Completed, string Url);

