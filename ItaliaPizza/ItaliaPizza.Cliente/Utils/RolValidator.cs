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
                    //ventana = new VentanaGerente();
                    break;

                case "mesero":
                   // ventana = new VentanaMesero();
                    break;

                case "cajero":
                    //ventana = new VentanaCajero();
                    break;

                case "cocinero":
                    //ventana = new VentanaCocinero();
                    break;

                case "administrador":
                    //ventana = new VentanaAdministrador();
                    break;

                default:
                    MessageBox.Show("Rol no reconocido");
                    return;
            }

            //ventana.Show();

        }
    }
}
