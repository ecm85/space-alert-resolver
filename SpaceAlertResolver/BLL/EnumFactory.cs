using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class EnumFactory
    {
        public static IList<TEnum> All<TEnum>()
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("T must be an enum type.");
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }
    }
}
