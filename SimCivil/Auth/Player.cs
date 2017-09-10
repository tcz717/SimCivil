using Newtonsoft.Json;
using SimCivil.Model;
using SimCivil.Net;
using System;
using System.Collections.Generic;

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
        public List<Guid> Roles { get; set; }
        [JsonIgnore]
        public Entity CurrentRole { get; set; }
    }
}