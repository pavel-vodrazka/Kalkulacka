using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kalkulacka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly KalkulackaController controller = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = controller;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controller.ZpracujKlavesu(((Button)sender).Content.ToString());
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            controller.ZpracujKlavesu(e.Text);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                controller.ZpracujKlavesu("=");
        }
    }
}
