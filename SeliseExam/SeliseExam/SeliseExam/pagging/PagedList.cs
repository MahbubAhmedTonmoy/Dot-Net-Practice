using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.pagging
{
    public class PagedList<T>: List<T>
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PagedList(List<T> items, int totalPage, int pageNumber, int pageSize)
        {
            TotalPage = totalPage;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(totalPage / (double)pageNumber);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreteAsync(IQueryable<T> sourceData, int pageNumber, int pageSize)
        {
            var count = await sourceData.CountAsync();
            var items = await sourceData.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
