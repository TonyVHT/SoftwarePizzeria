using ItaliaPizza.Cliente.Screens;
using ItaliaPizza.Cliente.UserControls; // donde están los UserControls
using ItaliaPizza.Cliente.Singleton;
using System;
using System.Windows;
using System.Net;
using ItaliaPizza.Cliente.UserControls;

namespace ItaliaPizza.Cliente.Utils
{
    public static class RolValidator
    {
        public static void ValidateRol(string rol)
        {
            var main = new MainWindow();

            switch (rol.ToLower())
            {
                case "gerente":
                    main.SetVista(new UCManager());
                    break;

                case "mesero":
                    main.SetVista(new UCWaiter());
                    break;

                case "cajero":
                    main.SetVista(new UCCashier());
                    break;

                case "cocinero":
                    main.SetVista(new UCCook());
                    break;

                case "administrador":
                    main.SetVista(new UCAdmin());
                    break;

                default:
                    MessageBox.Show("Rol no reconocido");
                    return;
            }

            main.Show();
        }
    }
}
