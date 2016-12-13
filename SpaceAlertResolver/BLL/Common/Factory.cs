namespace BLL.Common
{
	public class Factory
	{
		public virtual T Create<T>() where T: class, new()
		{
			return new T();
		}
	}
}
