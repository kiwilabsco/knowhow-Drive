using KnowledgeDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeDrive.Helper
{
    interface IGoogleDriveAPI
    {
        string UploadFile(string path);
        void DownloadFile(string blobId, string savePath);
        Response<string> getSpecificFolderFromName(string folderName);
        string getContentsFromFile(string itemID, string mimetype);
        Response<List<Google.Apis.Drive.v3.Data.File>> getFilesFromDriveFolder(String folderID);
    }
}
