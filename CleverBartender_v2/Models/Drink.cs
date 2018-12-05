 
using System.Collections.Generic;
using System.Net.Sockets;

namespace CleverBartender.Models
{
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    
    public static class GlobalVariables
    {
        public static string MyGlobalString { get; set; }

        //Socket
        public static TcpListener server { get; set; }
        public static TcpClient client { get; set; }
        public static NetworkStream stream { get; set; }
        public static bool socketStarted { get; set; }
    }
}
