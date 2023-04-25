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
        // Base url for the API
        private readonly Uri BooksApiUrl = new Uri("https://openlibrary.org/");

        // The possible cover sizes
        private enum CoverSize { Large, Medium, Small };

        /// <summary>
        /// Gets data from the API
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="uri">The url for the data</param>
        /// <returns>Data</returns>
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

        /// <summary>
        /// Gets a set of books by book title
        /// </summary>
        /// <param name="title">Book's title</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="sortMethod">Sorting method</param>
        /// <returns>Books</returns>
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

        /// <summary>
        /// Gets a set of books by an Author
        /// </summary>
        /// <param name="author">Author's name</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="sortMethod">Sorting method</param>
        /// <returns>Books</returns>
        public async Task<SearchResult> getBookByAuthorAsynch(string author, int pageNumber = 1, SortCategories sortMethod = SortCategories.Title)
        {
            string relativeUri = String.Format("search.json?author={0}&page={1}&sort={2}", author.ToLower().Replace(" ", "+"), pageNumber, getSortCategoryValue(sortMethod));
            Uri authorSearchUri = new Uri(BooksApiUrl, relativeUri);
            SearchResult result = await GetAsync<SearchResult>(authorSearchUri);
            return result;
        }

        /// <summary>
        /// Gets a deailed information about a book
        /// </summary>
        /// <param name="key">The book's ID/Key</param>
        /// <returns>Data about the book</returns>
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

        /// <summary>
        /// Gets an author by the author ID/Key
        /// </summary>
        /// <param name="key">Author's key</param>
        /// <returns>Data about the author</returns>
        private async Task<AuthorDetail> getAuthor(string key)
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

        /// <summary>
        /// It will convert the enum to string witch will be used in the url
        /// </summary>
        /// <param name="coverID">The ID of the book</param>
        /// <param name="size">The size/resolution of the cover picture (S/M/L)</param>
        /// <returns>string for url</returns>
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

        /// <summary>
        /// It will convert the enum to string witch will be used in the url
        /// </summary>
        /// <param name="category">The selected category type</param>
        /// <returns>string for url</returns>
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
