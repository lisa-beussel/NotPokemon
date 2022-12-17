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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        //goes to the game
        private void gotogame_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            this.NavigationService.Navigate(game);
        }

        //goes to the database
        private void gotodb_Click(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            this.NavigationService.Navigate(db);
        }

        //quits the app
        private void quit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
