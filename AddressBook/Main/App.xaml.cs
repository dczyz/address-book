using System.Windows;
using AddressBook.Mapper;
using AddressBook.Service;

namespace AddressBook.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var app = new ApplicationView();
            var applicationViewModel = ViewModelLocator.ApplicationViewModel;
            applicationViewModel.Init();
            app.DataContext = applicationViewModel;
            MapperConfiguration.Configure();
            ServiceLocator.PhotoService.CreatePhotoDirectoryPathIfNotExists();
            app.Show();
        }
    }
}
