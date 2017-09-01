using Newtonsoft.Json;
using SimCivil.Model;
using SimCivil.Net;

namespace SimCivil.Auth
{
    /// <summary>
    /// Represent a human player.
    /// </summary>
    public class Player
    {
        public Player(string username, object token)
        {
            Username = username;
            Token = token;
        }

        public Player() { }

        public string Username { get; set; }
        public object Token { get; set; }
        public string PlayerName { get; set; }
        [JsonIgnore]
        public Entity Entity { get; set; }
    }
}