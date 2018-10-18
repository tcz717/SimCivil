using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains {
    public class GameState
    {
        public Config Config { get; set; }
        public HashSet<IAccount> OnlineAccounts { get; set; } = new HashSet<IAccount>();
    }
}