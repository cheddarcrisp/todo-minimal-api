public record Todo(Guid Id, int Order, string Title, bool Completed = false);

public class TodoRepo {
    private List<Todo> todos = new();

    public Todo Add(Todo todo) {
        var newTodo = todo with { Id = Guid.NewGuid() };
        todos.Add(newTodo);
        return newTodo;
    }

    public IEnumerable<Todo> All() {
        return todos;
    }

    public void Clear() {
        todos.Clear();
    }

    public Todo Find(Guid id) {
        return todos.Single(x => x.Id == id);
    }

    public Todo Update(Guid id, string? title, int? order, bool? completed) {
        var todoIndex = todos.FindIndex(x => x.Id == id);
        var oldTodo = todos[todoIndex];
        var newTodo = oldTodo with {
            Title = title ?? oldTodo.Title,
            Order = order ?? oldTodo.Order,
            Completed = completed ?? oldTodo.Completed
        };
        todos[todoIndex] = newTodo;
        return newTodo;
    }

    public void Delete(Guid id) {
        var todoIndex = todos.FindIndex(x => x.Id == id);
        todos.RemoveAt(todoIndex);
    }
}