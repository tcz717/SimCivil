using SimCivil.Net;

namespace SimCivil.Auth
{
    /// <summary>
    /// Represent a human player.
    /// </summary>
    public class Player
    {
        public Player(string username, object token, IServerConnection connection = null)
        {
            Username = username;
            Token = token;
            Connection = connection;
        }

        public Player() { }

        public string Username { get; set; }
        public object Token { get; set; }
        public IServerConnection Connection { get; set; }
    }
}