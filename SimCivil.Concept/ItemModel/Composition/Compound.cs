using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public struct Compound
    {
        public string Name;
        public double Density;
        public double MeltPoint;
        public double BoilPoint;
        public double FlashPoint;
        public IReadOnlyDictionary<Element, double> Elements;
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
