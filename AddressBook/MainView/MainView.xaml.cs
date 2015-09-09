using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AddressBook.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Keyboard.FocusedElement is TextBox)
                Keyboard.FocusedElement.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
        }
    }
}
