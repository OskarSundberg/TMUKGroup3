using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        public static void Main(string[] args)
        {
            ServerStartUp serverStartUp = new ServerStartUp();
            serverStartUp.StartServer();
            return;
        }
    }
}
