using System;
using System.IO;
using System.Windows.Media.Imaging;
using static System.IO.FileMode;

namespace AddressBook.Service
{
    class PhotoService : IPhotoService
    {
        private static readonly string PhotosDirectoryPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\AddressBook\\Photos";

        public BitmapImage GetPhoto(int entryId)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            var photoPath = GetPhotoPath(entryId);
            if (!File.Exists(photoPath))
            {
                return null;
            }
            image.UriSource = new Uri(photoPath);
            image.EndInit();
            return image;
        }

        public void SavePhoto(BitmapImage photo, int entryId)
        {
            var photoPath = GetPhotoPath(entryId);
            DeleteFileIfExists(photoPath);
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(photo));
            using (var filestream = new FileStream(photoPath, Create))
            {
                encoder.Save(filestream);
            }
        }

        private static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void DeletePhoto(int entryId)
        {
            DeleteFileIfExists(GetPhotoPath(entryId));     
        }

        public void CreatePhotoDirectoryPathIfNotExists()
        {
            if (!Directory.Exists(PhotosDirectoryPath))
            {
                Directory.CreateDirectory(PhotosDirectoryPath);
            }
        }

        private static string GetPhotoPath(int entryId)
        {
            return $"{PhotosDirectoryPath}\\{entryId}.jpg";
        }
    }
}
