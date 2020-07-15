using GoogleDriveIntegration.Models;

using System.Collections.Generic;

namespace GoogleDriveIntegration.Helper
{
    public interface IGoogleDriveAPI
    {
        string UploadFile(string path, string credPath);
        void DownloadFile(string blobId, string savePath, string credPath);
        Response<string> getSpecificFolderFromName(string folderName, string credPath);
        string getContentsFromFile(string itemID, string mimetype, string credPath);
        Response<List<Google.Apis.Drive.v3.Data.File>> getFilesFromDriveFolder(string folderID, string credPath);
    }
}