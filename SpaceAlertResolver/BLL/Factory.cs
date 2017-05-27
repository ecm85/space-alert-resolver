using SimpleInjector;

namespace BLL
{
	public class Factory : IFactory
	{
		private Container Container { get; set; }

		public Factory(Container container)
		{
			Container = container;
		}

		public T Create<T>() where T : class
		{
			return Container.GetInstance<T>();
		}
	}
}
