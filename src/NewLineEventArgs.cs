using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_WP8.src
{
    public class NewLineEventArgs : EventArgs
    {
        public String message;

        public NewLineEventArgs(String message)
        {
            this.message = message;
        }
    }
}
