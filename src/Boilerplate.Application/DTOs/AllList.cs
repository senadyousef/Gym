using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Application.DTOs
{
    public class AllList<T>
    {
        public int CurrentPage { get; set; } 
        public int TotalItems { get; set; }

        public List<T> Result { get; set; } = new List<T>();

        public AllList(List<T> items, int count, int currentPage)
        {
            CurrentPage = currentPage; 
            TotalItems = count;
            Result.AddRange(items);
        }

        public AllList()
        {

        }
    }

    public static class AllListHelper
    {

        public const int DefaultPageSize = 15;
        public const int DefaultCurrentPage = 1;

        public static async Task<AllList<T>> ToAllListAsync<T>(this IQueryable<T> source, int currentPage)
        {
            currentPage = currentPage > 0 ? currentPage : DefaultCurrentPage; 
            var count = await source.CountAsync();
            var items = await source.Skip(currentPage - 1).ToListAsync();
            return new AllList<T>(items, count, currentPage);
        }
    }

    

    
}
