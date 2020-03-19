using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketNetWork
{
    public class Role
    {
        /// <summary>
        /// clientSocket
        /// </summary>
        public Client client;

        public Role(Client c)
        {
            this.client = c;
        }

    }
}
