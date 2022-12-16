using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotPokemon
{
    /// <summary>
    /// Interaction logic for Lost.xaml
    /// </summary>
    public partial class Lost : Page
    {
        public Lost()
        {
            InitializeComponent();
        }

        private void playagain_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            this.NavigationService.Navigate(game);
        }

        private void backtomm_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }
    }
}
