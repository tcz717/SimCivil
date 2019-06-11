using System;
using System.Collections.Generic;

namespace SimCivil.Orleans.Interfaces.Item
{
    public class Element
    {
        public string Name { get; set; }

        public string FullName { get; set; }
    }

    public class JsonElementRepository : Dictionary<string, Element>, IRepository<string, Element>, IDataLoader
    {
        public static JsonElementRepository Instance { get; } = new JsonElementRepository();

        private JsonElementRepository() { }

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
