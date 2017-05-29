using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_1617_G03
{
   public class infoClient
    {
        public Client client { set; get; }
        public Show show { set; get; }
        

        public infoClient(Client cl , Show sh)
        {
            client = cl;
            show = sh;
           

        }


    }
}
