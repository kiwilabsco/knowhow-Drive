﻿using Google.Apis.Auth.OAuth2;
using GoogleDriveIntegration.Helper;
using GoogleDriveIntegration.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleDriveIntegration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Loading files...";
            button1.Enabled = false;
            var folderName = tbFolderName.Text;

            var responseRefresh = RefreshDriveData(folderName);

            lblStatus.Text = "Done: " + responseRefresh.Message;
            button1.Enabled = true;

        }


        private Response<bool> RefreshDriveData(string folderName)
        {
            #region Read files from Drive
            //Create a new list
            var allFiles = new List<Google.Apis.Drive.v3.Data.File>();
            //Get all files in the folder
            var folderID = getSpecificFolderFromName(folderName);
            if (!folderID.Success)
            {
                return new Response<bool>() { Success = false, Message = folderID.Message };
            }
            var files = getFilesFromDriveFolder(folderID.Data);
            //Check if file exists
            if (files != null && files.Data.Count > 0)
            {
                foreach (var file in files.Data)
                {
                    if (file.MimeType == "application/vnd.google-apps.folder") //Fetch contents of folder
                        allFiles.AddRange(getFilesFromDriveFolder(file.Id).Data);

                    else
                        allFiles.Add(file);
                }
            }
            else

                return new Response<bool>() { Success = false, Message = "No files are found." };

            #endregion


            // Truncate the drive index before inserting new data
            Helper.ElasticsearchRepository.deleteByQuery(ElasticsearchRepository.knowhowDriveIndex);

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
                Helper.ElasticsearchRepository.insertItem(driveItem, ElasticsearchRepository.knowhowDriveIndex, item.Id);

            }
            return new Response<bool>() { Success = true, Message = "All files are inserted to Elasticsearch." };
        }

        public Response<List<Google.Apis.Drive.v3.Data.File>> getFilesFromDriveFolder(String folderID)
        {
            return new GoogleDriveAPI().getFilesFromDriveFolder(folderID);


        }


        public Response<string> getSpecificFolderFromName(string folderName)
        {
            return new GoogleDriveAPI().getSpecificFolderFromName(folderName);


        }

        public string getContentsFromFile(string itemID, string mimetype)
        {
            return new GoogleDriveAPI().getContentsFromFile(itemID, mimetype);


        }

        private void btnDeleteToken_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\token.json"))
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\token.json", true);
                lblStatus.Text = "Directory is deleted.";
            }
            else
                lblStatus.Text = "Directory not found.";

        }

        private void tbFolderName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
