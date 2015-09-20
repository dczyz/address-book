using System.Windows;
using AddressBook.Application;
using AddressBook.Mapper;
using AddressBook.Service;

namespace AddressBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var app = new ApplicationView();
            var context = new ApplicationViewModel();
            app.DataContext = context;
            MapperConfiguration.Configure();
            ServiceLocator.PhotoService.CreatePhotoDirectoryPathIfNotExists();
            app.Show();
        }
    }
}
