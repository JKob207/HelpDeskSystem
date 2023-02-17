using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HelpDeskApp.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Blog()
        {
            var newsApiClient = new NewsApiClient("4f6a212b2c7445c89362153dc646f6b7");
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "Hardware",
                SortBy = SortBys.Popularity,
                Language = Languages.EN,
                PageSize = 5,
                From = new DateTime(2023, 1, 17)
            });
            if (articlesResponse.Status == Statuses.Ok)
            {
                List<Article> articles = new List<Article>();
                foreach (var article in articlesResponse.Articles)
                {
                    Article a = new Article();
                    a.Title = article.Title;
                    a.Author = article.Author;
                    a.Description = article.Description;
                    a.Url = article.Url;
                    a.PublishedAt = article.PublishedAt;
                    articles.Add(a);
                }
                return View(articles);
            }
            else
            {
                Debug.WriteLine(articlesResponse.Status);
                Debug.WriteLine(articlesResponse.Error.Message);
                return RedirectToAction("Index", "Users");
            }
        }
    }
}
