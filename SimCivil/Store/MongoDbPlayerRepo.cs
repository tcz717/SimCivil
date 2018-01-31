// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil - MongoDbPlayerRepo.cs
// Create Date: 2018/01/09
// Update Date: 2018/01/10

using System;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using SimCivil.Auth;

namespace SimCivil.Store
{
    /// <summary>
    /// MongoDb Player Repo
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Store.IPlayerRepository</cref>
    /// </seealso>
    public class MongoDbPlayerRepo : IPlayerRepository
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public IMongoDatabase Db { get; }

        /// <summary>
        /// Gets or sets the player collection.
        /// </summary>
        /// <value>
        /// The player collection.
        /// </value>
        public IMongoCollection<Player> PlayerCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbPlayerRepo"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public MongoDbPlayerRepo(IMongoDatabase db)
        {
            Db = db;
            PlayerCollection = db.GetCollection<Player>(nameof(Player));
        }

        /// <summary>
        /// Adds the player.
        /// </summary>
        /// <param name="player">The player.</param>
        public Task AddPlayerAsync(Player player)
        {
            return PlayerCollection.InsertOneAsync(player);
        }

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="player">The player.</param>
        public Task UpdatePlayerAsync(Player player)
        {
            return PlayerCollection.ReplaceOneAsync(
                p => p.Id == player.Id,
                player);
        }

        /// <summary>
        /// Determines whether the specified player name is exsist.
        /// </summary>
        /// <param name="name">The name.</param>
        public bool IsExsist(string name)
        {
            return PlayerCollection.Find(p => p.Username == name).Any();
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Player GetPlayer(string name)
        {
            return PlayerCollection.Find(p => p.Username == name).First();
        }
    }
}