using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_WP8
{
    class TextEventArg: EventArgs
    {
        private String message;


        public String getMessage()
        {
            return this.message;
        }

        public void setMessage(String message)
        {
            this.message = message;
        }

    }
}
