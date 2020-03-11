using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Infraestructure
{
    public static class ICollectionExtensions
    {
        public static void RemoveRange<T>(this ICollection<T> source, params T[] objects)
        {
            foreach (var item in objects)
            {
                source.Remove(item);
            }
        }
    }
}
