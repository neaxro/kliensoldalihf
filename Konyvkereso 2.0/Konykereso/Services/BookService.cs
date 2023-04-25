using Konyvkereso.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using static Konyvkereso.ViewModels.MainPageViewModel;

namespace Konyvkereso.Services
{
    public class BookService
    {
        private readonly Uri BooksApiUrl = new Uri("https://openlibrary.org/");
        private enum CoverSize { Large, Medium, Small };

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

        public async Task<SearchResult> getBookByTitleAsynch(string title, int pageNumber = 1, SortCategories sortMethod = SortCategories.Title)
        {
            string relativeUri = String.Format("search.json?title={0}&page={1}&sort={2}", title.ToLower().Replace(" ", "+"), pageNumber, getSortCategoryValue(sortMethod));
            Uri titleSearchUri = new Uri(BooksApiUrl, relativeUri);
            SearchResult result = await GetAsync<SearchResult>(titleSearchUri);

            foreach (var book in result.Docs)
            {
                book.CoverUrl = getCoverUrl(book.Cover_i, CoverSize.Medium);
            }

            return result;
        }

        public async Task<SearchResult> getBookByAuthorAsynch(string author, int pageNumber = 1, SortCategories sortMethod = SortCategories.Title)
        {
            string relativeUri = String.Format("search.json?author={0}&page={1}&sort={2}", author.ToLower().Replace(" ", "+"), pageNumber, getSortCategoryValue(sortMethod));
            Uri authorSearchUri = new Uri(BooksApiUrl, relativeUri);
            SearchResult result = await GetAsync<SearchResult>(authorSearchUri);
            return result;
        }

        public async Task<BookDetail> getDetailedBookInfo(string key)
        {
            try
            {
                Uri detailedBookUri = new Uri(BooksApiUrl, String.Format("{0}.json", key));
                BookDetail result = await GetAsync<BookDetail>(detailedBookUri);
                
                // Set coverUrl from coverId for Image
                if(result.covers != null)
                {
                    result.coverUrl = getCoverUrl(result.covers[0], CoverSize.Large);
                }

                // Load all author details
                List<AuthorDetail> authorList = new List<AuthorDetail>();
                if(result.authors != null)
                {
                    foreach (var author in result.authors)
                    {
                        if (author == null) continue;
                        var authorResult = await getAuthor(author.author.key);
                        if (authorResult != null)
                        {
                            authorList.Add(authorResult);
                        }
                    }
                }
                result.authorDetails = authorList;

                return result;
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new InvalidBookDetail("Invalid UTF character(s)!");
            }
        }

        public async Task<AuthorDetail> getAuthor(string key)
        {
            try
            {
                Uri detailedAuthorUri = new Uri(BooksApiUrl, String.Format("{0}.json", key));
                AuthorDetail result = await GetAsync<AuthorDetail>(detailedAuthorUri);

                // Get the author photo url for Image
                if(result.photos != null)
                {
                    result.coverUrl = getCoverUrl(result.photos[0], CoverSize.Medium);
                }

                return result;
            } catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private string getCoverUrl(int coverID, CoverSize size)
        {
            switch (size)
            {
                case CoverSize.Small:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-S.jpg", coverID);
                case CoverSize.Medium:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-M.jpg", coverID);
                case CoverSize.Large:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-L.jpg", coverID);
                default:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-M.jpg", coverID);
            }
        }

        private string getSortCategoryValue(SortCategories category)
        {
            switch (category)
            {
                case SortCategories.New: return "new";
                case SortCategories.Old: return "old";
                case SortCategories.Random: return "random";
                case SortCategories.Title: return "title";
                default: return "title";
            }
        }
    }
}
