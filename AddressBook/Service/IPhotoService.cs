using System.Windows.Media.Imaging;

namespace AddressBook.Service
{
    public interface IPhotoService
    {
        void CreatePhotoDirectoryPathIfNotExists();

        void DeletePhoto(int entryId);

        BitmapImage GetPhoto(int entryId);

        void SavePhoto(BitmapImage photo, int entryId);
    }
}