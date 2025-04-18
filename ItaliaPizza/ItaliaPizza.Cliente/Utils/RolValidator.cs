using ItaliaPizza.Cliente.Screens.Admin;
using ItaliaPizza.Cliente.Screens.Cook;
using ItaliaPizza.Cliente.Screens.Manager;
using ItaliaPizza.Cliente.Screens.Waiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizza.Cliente.Utils
{
    public static class RolValidator
    {
        public static void ValidateRol(String rol)
        {
            Window ventana;

            switch (rol.ToLower())
            {
                case "gerente":
                    ventana = new HomePageManager();
                   
                    break;

                case "mesero":
                   ventana = new HomePageWaiter();
                    
                    break;

                case "cajero":
                    ventana = new HomePageWaiter();
                    break;

                case "cocinero":
                    ventana = new HomePageCook();
                    break;

                case "administrador":
                    ventana = new HomePageAdmin();
                    break;

                default:
                    MessageBox.Show("Rol no reconocido");
                    return;
            }

            ventana.Show();

        }
    }
}
