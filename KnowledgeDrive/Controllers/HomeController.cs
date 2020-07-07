using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KnowhowDrive.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using KnowhowDrive.Helper;
using Nest;
using System.Configuration;
using Newtonsoft.Json;

namespace KnowhowDrive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "knowhow Drive";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "";
            
            var allFiles = Helper.ElasticsearchRepository.getDriveFiles();
            var view = new HomeViewModel();
            if (allFiles.Success)
            {
                ViewBag.Message += allFiles.Message + ". " + allFiles.Data.Count() + " files listed.";
                view.files = allFiles.Data;
            }
            else
            {
                ViewBag.Message += allFiles.Message;
            }
            return View(view);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpGet]
        public IActionResult SearchKeyword(string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
                return Json(Helper.ElasticsearchRepository.getDriveFiles());

            Response<List<KnowhowDrive.Models.DriveItemModel>> results = Helper.ElasticsearchRepository.SearchKeyword(keyword);
            foreach (var item in results.Data)
            {
                item.content = "";
                item.fileId = "";
            }
            return Json(results);
        }


    }
}


