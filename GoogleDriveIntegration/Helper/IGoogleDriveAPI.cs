using GoogleDriveIntegration.Models;

using System.Collections.Generic;

namespace GoogleDriveIntegration.Helper
{
    public interface IGoogleDriveAPI
    {
        string UploadFile(string path);
        void DownloadFile(string blobId, string savePath);
        Response<string> getSpecificFolderFromName(string folderName);
        string getContentsFromFile(string itemID, string mimetype);
        Response<List<Google.Apis.Drive.v3.Data.File>> getFilesFromDriveFolder(string folderID);
    }
}