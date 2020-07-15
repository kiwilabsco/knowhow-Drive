using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleDriveIntegration.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleDriveIntegration.Helper
{
    public class GoogleDriveAPI : IGoogleDriveAPI
    {
        private string[] Scopes = { DriveService.Scope.Drive };
        private string ApplicationName = "knowhow Drive";

        public void DownloadFile(string blobId, string savePath, string credPath)
        {
            var service = GetDriveServiceInstance(credPath);
            var request = service.Files.Get(blobId);
            var stream = new MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            SaveStream(stream, savePath);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
        }

        public string getContentsFromFile(string itemID, string mimetype, string credPath)
        {

            FilesResource.ExportRequest exportRequest = GetDriveServiceInstance(credPath).Files.Export(itemID, "text/plain");

            var result = exportRequest.Execute();

            return result;
        }

        public Response<List<Google.Apis.Drive.v3.Data.File>> getFilesFromDriveFolder(string folderID, string credPath)
        {
            try
            {

                FilesResource.ListRequest listRequest = GetDriveServiceInstance(credPath).Files.List();
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

        public string UploadFile(string path, string credPath)
        {
            var service = GetDriveServiceInstance(credPath);
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = "image/jpeg";
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            return file.Id;
        }
        private DriveService GetService()
        {
            UserCredential credential;
            using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, "./credentials/credentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        private DriveService GetDriveServiceInstance(string credPath)
        {
            UserCredential credential;

            using (var stream =
                new FileStream(credPath, FileMode.Open, FileAccess.Read))
            {
                string credPath1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal)+ "\\token.json";
                //string credPath = AppDomain.CurrentDomain.BaseDirectory+"\\token.json";
                //credPath = Path.Combine(credPath, "./credentials/credentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath1, true)).Result;
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        private static void SaveStream(MemoryStream stream, string saveTo)
        {
            using (FileStream file = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }

        public Response<string> getSpecificFolderFromName(string folderName, string credPath)
        {
            try
            {

                FilesResource.ListRequest listRequest = GetDriveServiceInstance(credPath).Files.List();
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

    }
}
