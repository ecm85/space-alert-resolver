 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
