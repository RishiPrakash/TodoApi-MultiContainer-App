using TodoApi;

interface IService {
    public Task<Todo> AddItemAysnc(Todo item);

    public  Task<List<Todo>> GetAllAsync();
}