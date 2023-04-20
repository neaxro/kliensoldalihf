using Konyvkereso.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Konyvkereso.Services
{
    public class BookService
    {
        private readonly Uri BooksApiUrl = new Uri("https://openlibrary.org/");

        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        public async Task<SearchResult> getBookByTitleAsynch(string title)
        {
            Uri titleSearchUri = new Uri(BooksApiUrl, $"/search.json?title={title}");
            SearchResult result = await GetAsync<SearchResult>(titleSearchUri);
            return result;
        }

        public async Task<SearchResult> getBookByAuthorAsynch(string author)
        {
            Uri authorSearchUri = new Uri(BooksApiUrl, $"/search.json?author={author}");
            SearchResult result = await GetAsync<SearchResult>(authorSearchUri);
            return result;
        }
    }
}
