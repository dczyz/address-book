using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace AddressBook.Service
{
    class PhotoService
    {
        public BitmapImage GetPhoto(int entryId)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            if (
                !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                            $"\\AddressBook\\Photos\\{entryId}.jpg"))
            {
                return null;
            }
            image.UriSource = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\AddressBook\\Photos\\{entryId}.jpg");
            image.EndInit();
            return image;
        }

        public void SavePhoto(BitmapImage photo, int entryId)
        {
            if (
                !Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                           "\\AddressBook\\Photos"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                           "\\AddressBook\\Photos");
            }
            if (
                File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                            $"\\AddressBook\\Photos\\{entryId}.jpg"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\AddressBook\\Photos\\{entryId}.jpg");
            }
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(photo));
            using (
                var filestream =
                    new FileStream(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                        $"\\AddressBook\\Photos\\{entryId}.jpg", FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }

        public void DeletePhoto(int entryId)
        {
            if (
                File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                            $"\\AddressBook\\Photos\\{entryId}.jpg"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                            $"\\AddressBook\\Photos\\{entryId}.jpg");
            }         
        }
    }
}
