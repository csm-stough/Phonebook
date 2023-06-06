using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csm_stough.Phonebook
{
    public class MenuOption
    {
        public string text { get; set; }
        public Action action { get; set; }
    }
}
