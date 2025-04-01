using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
  public static class HttpExtensions
  {
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
      // We are using anonymous object, without any type
      var paginationHeader = new
      {
        currentPage,
        itemsPerPage,
        totalItems,
        totalPages
      };

      response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
    }
  }
}