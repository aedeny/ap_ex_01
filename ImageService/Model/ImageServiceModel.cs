﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageService.Model
{
    public class ImageServiceModel : IImageServiceModel
    {
        public ImageServiceModel(string outputFolder, int thumbnailSize)
        {
            _outputFolder = outputFolder;
            _thumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out EventLogEntryType result)
        {
            result = EventLogEntryType.Error;

            try
            {
                if (!File.Exists(path)) return "File does not exist.";

                // Tries to get date taken from image. If property doesn't exist, gets date created.
                DateTime dateTime;
                try
                {
                    dateTime = GetDateTakenFromImage(path);
                }
                catch (Exception)
                {
                    dateTime = File.GetCreationTime(path);
                }

                CreateDirectoriesStructure(dateTime);

                string pathSuffix = dateTime.Year + "\\" + dateTime.Month + "\\" +
                                    Path.GetFileNameWithoutExtension(path);

                string outputFilePath = _outputFolder + "\\" + pathSuffix;
                string extension = Path.GetExtension(path);

                int i = 0;
                string copyNumber = "";

                /* If a file named 'name.image' already exists,
                 * a file named 'name(1).image' will be created */
                while (File.Exists(outputFilePath + copyNumber + extension))
                {
                    i++;
                    copyNumber = "(" + i + ")";
                }

                outputFilePath += copyNumber + extension;

                string thumbnailPath = _outputFolder + "\\thumbnails\\" + pathSuffix + copyNumber + extension;

                // Creates thumbnail
                using (Image image = Image.FromFile(path))
                using (Image thumb = image.GetThumbnailImage(_thumbnailSize, _thumbnailSize, () => false, IntPtr.Zero))
                {
                    thumb.Save(Path.ChangeExtension(thumbnailPath, "thumb"));
                }

                // Copies file to output folder
                File.Copy(path, outputFilePath, true);

                result = EventLogEntryType.Information;
                return outputFilePath;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static DateTime GetDateTakenFromImage(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(0x9004);
                string dateTaken = Regex.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        private void CreateDirectoriesStructure(DateTime dateTime)
        {
            // Creates image folder
            Directory.CreateDirectory(_outputFolder + "\\" + dateTime.Year + "\\" + dateTime.Month);

            // Creates thumbnail folder
            Directory.CreateDirectory(_outputFolder + "\\thumbnails\\" + dateTime.Year + "\\" + dateTime.Month);
        }

        #region Members

        private readonly string _outputFolder;
        private readonly int _thumbnailSize;
        private static readonly Regex Regex = new Regex(":");

        #endregion
    }
}