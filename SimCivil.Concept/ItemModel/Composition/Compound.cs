using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public class Compound
    {
        public string Name { get; set; }
        public double Density { get; set; }
        public double MeltPoint { get; set; }
        public double BoilPoint { get; set; }
        public double FlashPoint { get; set; }
        public IReadOnlyDictionary<string, double> Elements { get; set; }
    }

    public class JsonCompoundRepository : Dictionary<string, Compound>, IRepository<string, Compound>, IDataLoader
    {
        public static JsonCompoundRepository Instance { get; } = new JsonCompoundRepository();

        private JsonCompoundRepository() { }

        public void LoadData()
        {
            throw new NotImplementedException("Load data into properties.");
        }

        public void InitRepo()
        {
            LoadData();
        }
    }
}
