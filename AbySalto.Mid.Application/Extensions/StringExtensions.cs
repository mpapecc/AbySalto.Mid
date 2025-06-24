using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbySalto.Mid.Domain.Entites;

namespace AbySalto.Mid.Application.Extensions
{
    public static class StringExtensions
    {
        public static IList<int> IdsStringToList(this string? str) 
        {
            return (str ?? "")
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();
        }
    }
}
