using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
  public class PagedList<T> : List<T>
  {
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
      CurrentPage = pageNumber;
      // in case page has 12 items and pageSize is 10, then TotalPages will be 2
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      PageSize = pageSize;
      TotalCount = count;
      AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
      // This will be the first query to our DB to find out how many items are there
      var count = await source.CountAsync();
      var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
      return new PagedList<T>(items, count, pageNumber, pageSize);
    }
  }
}