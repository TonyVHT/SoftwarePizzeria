using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.Cliente.Utils
{
    public interface IModalPage
    {
        Action OnClose { get; set; }
    }
}
