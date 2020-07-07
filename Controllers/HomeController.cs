using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KnowledgeDrive.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using KnowledgeDrive.Helper;
using Nest;
using System.Configuration;
using Newtonsoft.Json;

namespace KnowledgeDrive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Drive API .NET Quickstart";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string refresh)
        {
            ViewBag.Message = "";
            if (refresh != null && refresh.Equals("1"))
            {
                var responseRefresh = RefreshDriveData("KiwiLab");
                ViewBag.Message = responseRefresh.Message + "\n";
            }

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

        private Response<bool> RefreshDriveData(string folderName)
        {
            #region drive'dan oku
            //yeni liste oluştur
            var allFiles = new List<Google.Apis.Drive.v3.Data.File>();
            //ilgili klasördeki tüm dosyaları al

            var folderID = getSpecificFolderFromName(folderName);
            if (!folderID.Success)
            {
                return new Response<bool>() { Success = false, Message = folderID.Message };
            }
            var files = getFilesFromDriveFolder(folderID.Data);
            //dosya var mı bak
            if (files != null && files.Data.Count > 0)
            {
                foreach (var file in files.Data)
                {
                    if (file.MimeType == "application/vnd.google-apps.folder") //Get contents of folder
                        allFiles.AddRange(getFilesFromDriveFolder(file.Id).Data);

                    else
                        allFiles.Add(file);
                }
            }
            else

                return new Response<bool>() { Success = false, Message = "No files found." };

            #endregion


            //truncate the drive index before inserting new data
            Helper.ElasticsearchRepository.deleteByQuery(ElasticsearchRepository.knowledgeDriveIndex);

            //Insert file data to ES
            foreach (var item in allFiles)
            {
                string fileContents = "";
                try
                {
                    fileContents = getContentsFromFile(item.Id, item.MimeType);
                }
                catch (Exception) { }

                //insert file to Elasticsearch
                var driveItem = new Models.DriveItemModel();
                driveItem.createTime = DateTime.UtcNow;
                driveItem.updateTime = DateTime.UtcNow;
                driveItem.content = fileContents;
                driveItem.title = item.Name;
                driveItem.sharableLink = item.WebViewLink;
                driveItem.fileId = item.Id;
                Helper.ElasticsearchRepository.insertItem(driveItem, ElasticsearchRepository.knowledgeDriveIndex, item.Id);

            }
            return new Response<bool>() { Success = true, Message = "All files saved succesfully to ElasticSearch." };
        }

        public Response<List<Google.Apis.Drive.v3.Data.File>> getFilesFromDriveFolder(String folderID)
        {
            return new GoogleDriveAPI().getFilesFromDriveFolder(folderID);
            try
            {
                UserCredential credential;
                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.Q = "'" + folderID + "' in parents"; //Get contents of folder using file ID. 
                listRequest.PageSize = 100;
                listRequest.Fields = "nextPageToken, files(id, name, webViewLink, size, mimeType)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                    .Files;
                if (files?.Count > 0)

                    return new Response<List<Google.Apis.Drive.v3.Data.File>>() { Success = true, Message = "Success", Data = files.ToList() };
                else
                    return new Response<List<Google.Apis.Drive.v3.Data.File>>() { Success = false, Message = "No files found" };


            }
            catch (Exception ex)
            {

                return new Response<List<Google.Apis.Drive.v3.Data.File>>() { Success = false, Message = ex?.Message + " | " + ex?.StackTrace };

            }

        }


        public Response<string> getSpecificFolderFromName(string folderName)
        {
            return new GoogleDriveAPI().getSpecificFolderFromName(folderName);

            try
            {
                UserCredential credential;
                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 100;
                listRequest.Fields = "nextPageToken, files(id, name, webViewLink, size, mimeType)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                    .Files;


                foreach (var item in files)
                {
                    if (item.MimeType.Equals("application/vnd.google-apps.folder") && item.Name.Equals(folderName))
                    {
                        return new Response<string>() { Success = true, Message = "Success", Data = item.Id };
                    }
                }
                return new Response<string>() { Success = false, Message = "Folder name not found!" };
            }
            catch (Exception ex)
            {

                return new Response<string>()
                {
                    Success = false,
                    Message = ex?.Message + " | " + ex?.StackTrace
                };

            }
        }

        public string getContentsFromFile(string itemID, string mimetype)
        {
            return new GoogleDriveAPI().getContentsFromFile(itemID, mimetype);


            UserCredential credential;
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            FilesResource.ExportRequest exportRequest = service.Files.Export(itemID, "text/plain");

            var result = exportRequest.Execute();

            return result;
        }

        [HttpGet]
        public IActionResult SearchKeyword(string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
                return Json(Helper.ElasticsearchRepository.getDriveFiles());


            return Json(Helper.ElasticsearchRepository.SearchKeyword(keyword));
        }


    }
}


