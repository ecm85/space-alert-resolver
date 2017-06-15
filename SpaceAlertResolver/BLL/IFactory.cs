namespace BLL
{
    public interface IFactory
    {
        T Create<T>() where T : class;
    }
}
