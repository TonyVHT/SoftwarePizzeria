using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Helpers
{
    public class ButtonSelectionHelper
    {

       

        // Método para desmarcar todos los botones de un UserControl
        public static void DesmarcarBotones(UserControl menuControl)
        {
            if (menuControl is UCManager ucManager)
            {
                ucManager.buttonInicio.Tag = "Inactive";
                ucManager.buttonOrders.Tag = "Inactive";
                ucManager.buttonMenu.Tag = "Inactive";
                ucManager.buttonCustomers.Tag = "Inactive";
            }
            else if (menuControl is UCWaiter ucWaiter)
            {
                ucWaiter.buttonInicio.Tag = "Inactive";
                ucWaiter.buttonOrders.Tag = "Inactive";

            }
            else if (menuControl is UCCook ucCook)
            {
                ucCook.buttonInicio.Tag = "Inactive";
                //ucCook.buttonOrders.Tag = "Inactive";
                ucCook.buttonMenu.Tag = "Inactive";
                ucCook.buttonCustomers.Tag = "Inactive";

            }
            else if (menuControl is UCAdmin ucAdmin)
            {
                ucAdmin.buttonInicio.Tag = "Inactive";
                ucAdmin.buttonUsuarios.Tag = "Inactive";
                ucAdmin.buttonMenu.Tag = "Inactive";
                ucAdmin.buttonCustomers.Tag = "Inactive";
                ucAdmin.buttonProductos.Tag = "Inactive";
                ucAdmin.buttonProveedores.Tag = "Inactive";
                ucAdmin.buttonAnalytics.Tag = "Inactive";

            }
            else if (menuControl is UCCashier uCCashier)
            {
                uCCashier.buttonInicio.Tag = "Inactive";
                uCCashier.buttonOrders.Tag = "Inactive";
                uCCashier.buttonMenu.Tag = "Inactive";
                uCCashier.buttonCustomers.Tag = "Inactive";
            }
        }

        // Método para marcar el botón seleccionado
        public static void MarcarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            switch (botonSeleccionado)
            {
                case "Inicio":
                    if (menuControl is UCManager manager)
                        manager.buttonInicio.Tag = "Active";
                    else if (menuControl is UCWaiter waiter1)
                        waiter1.buttonInicio.Tag = "Active";
                    else if (menuControl is UCCook cook1)
                        cook1.buttonInicio.Tag = "Active";
                    else if (menuControl is UCAdmin admin1)
                        admin1.buttonInicio.Tag = "Active";
                    else if (menuControl is UCCashier cashier)
                        cashier.buttonInicio.Tag = "Active";
                    break;

                case "Orders":
                    if (menuControl is UCWaiter waiter)
                        waiter.buttonOrders.Tag = "Active";
                    else if (menuControl is UCCashier cashier)
                        cashier.buttonOrders.Tag = "Active";
                    break;

                case "Menu":
                    if (menuControl is UCCook cook)
                        cook.buttonMenu.Tag = "Active";
                    else if (menuControl is UCAdmin admin2)
                        admin2.buttonMenu.Tag = "Active";
                    break;

                case "Customers":
                    if (menuControl is UCManager manager2)
                        manager2.buttonCustomers.Tag = "Active";
                    else if (menuControl is UCAdmin admin2)
                        admin2.buttonCustomers.Tag = "Active";
                    break;

                case "Analytics":
                    if (menuControl is UCAdmin admin)
                        admin.buttonAnalytics.Tag = "Active";
                    break;

                case "Productos":
                    if (menuControl is UCAdmin admin3)
                        admin3.buttonProductos.Tag = "Active";
                    break;

                case "Proveedores":
                    if (menuControl is UCAdmin admin4)
                        admin4.buttonProveedores.Tag = "Active";
                    break;

                case "Usuarios":
                    if (menuControl is UCAdmin admin5)
                        admin5.buttonUsuarios.Tag = "Active";
                    break;
            }
        }
    }

}
