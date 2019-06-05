using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public class EquipmentType
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class JsonEquipmentTypeRepository : Dictionary<string, EquipmentType>, IRepository<string, EquipmentType>, IDataLoader
    {
        public static JsonEquipmentTypeRepository Instance { get; } = new JsonEquipmentTypeRepository();

        private JsonEquipmentTypeRepository() { }

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
