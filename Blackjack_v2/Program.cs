using Blackjack_Server.bj;
using Blackjack_Server.SocketComm;

namespace Blackjack_Server
{
    static class Program
    {
        private static void Main()
        {
            Game game = new Game();

            string ipAddress = "192.168.1.132"; //Get from user-input or config
            AsyncServer server = new AsyncServer(ipAddress, game);
            server.StartListening();
        }
    }
}
