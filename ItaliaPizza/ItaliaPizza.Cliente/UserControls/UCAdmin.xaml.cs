using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ItaliaPizza.Cliente.Screens.Admin;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ItaliaPizza.Cliente.Utils;
using ItaliaPizza.Cliente.Screens;

namespace ItaliaPizza.Cliente.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCManager.xaml
    /// </summary>
    public partial class UCAdmin : UserControl
    {
        public UCAdmin()
        {
            InitializeComponent();
        }


        

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SessionManagerHelper.CerrarSesionUniversal();
            
            
        }

        private void GoToUserOptions(object sender, RoutedEventArgs e)
        {
            var userOptions = new UserOptions();
            userOptions.Show();
            Window.GetWindow(this)?.Close();
        }

        private void GoToProductsOptions(object sender, RoutedEventArgs e)
        {
            var productOptions = new SearchProduct();
            productOptions.Show();
            Window.GetWindow(this)?.Close();
        }

        private void GoToProviderOptions(object sender, RoutedEventArgs e)
        {
            var providerOptions = new ProviderOptions();
            providerOptions.Show();
            Window.GetWindow(this)?.Close();
        }


        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            var homePage = new HomePageAdmin();
            homePage.Show();
            Window.GetWindow(this)?.Close();
        }

        private void GoToRecipeOptions(object sender, RoutedEventArgs e)
        {
            var recipeOptions = new RecipeOptions();
            recipeOptions.Show();
            Window.GetWindow(this)?.Close();
        }

        private void GoToCustomerOptions(object sender, RoutedEventArgs e)
        {
            var customerOptions = new CustomerOptiones();
            customerOptions.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
