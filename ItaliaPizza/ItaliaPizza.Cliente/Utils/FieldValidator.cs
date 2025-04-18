using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Utils
{
    public static class FieldValidator
    {
        public static bool HayCamposVacios(params Control[] controles)
        {
            foreach (var control in controles)
            {
                switch (control)
                {
                    case TextBox txt when string.IsNullOrWhiteSpace(txt.Text):
                        return true;

                    case PasswordBox pwd when string.IsNullOrWhiteSpace(pwd.Password):
                        return true;

                    case ComboBox cmb when cmb.SelectedItem == null:
                        return true;
                }
            }

            return false;
        }


    }
}
