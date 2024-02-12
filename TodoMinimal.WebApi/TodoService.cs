public record TodoDTO(int? Order, string? Title, bool? Completed, string Url);

public class TodoService {
    private string baseUrl;
    private TodoRepo repo;
    public TodoService(string baseUrl, TodoRepo repo) {
        this.baseUrl = baseUrl;
        this.repo = repo;
    }

    public IEnumerable<TodoDTO> All() {
        return repo.All().Select(MapTodo);
    }

    public TodoDTO Find(Guid id) {
        return MapTodo(
            repo.Find(id)
        );
    }

    public TodoDTO Add(TodoDTO todo) {
        return MapTodo(
            repo.Add(new(
                Guid.NewGuid(),
                todo.Order ?? 0,
                todo.Title ?? "default",
                todo.Completed ?? false
            ))
        );
    }

    public void Clear() {
        repo.Clear();
    }

    public TodoDTO Update(Guid id, TodoDTO todo) {
        return MapTodo(
            repo.Update(id, todo.Title, todo.Order, todo.Completed)
        );
    }

    public void Delete(Guid id) {
        repo.Delete(id);
    }

    private TodoDTO MapTodo(Todo todo) {
        return new(
        todo.Order,
        todo.Title,
        todo.Completed,
        $"{baseUrl}/{todo.Id}"
    );
    }
}