namespace Market.DAL;

public interface ICrud<T>
{
    public IEnumerable<T> GetAll();
    public bool Insert(T entity);
    public bool Update(T entity);
    public bool Delete(T entity);
}