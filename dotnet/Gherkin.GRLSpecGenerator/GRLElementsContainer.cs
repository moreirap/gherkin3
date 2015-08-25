using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gherkin.GRLCatalogueGenerator
{
    class GRLElementsContainer
    {
        private Dictionary<int, IElementWithIdentity> registry;
        private int lastAssignedId = 0;

        public GRLElementsContainer()
        {
            registry = new Dictionary<int, IElementWithIdentity>();
        }

        public int Add<T>(T value) where T : class,IElementWithIdentity
        {
            if (value == null)
                throw new ApplicationException("Cannot register null object in GRLContainer");
            if (registry.Any(keyval => keyval.Value.name == value.name))
                throw new ApplicationException("Cannot register value in GRLContainer as it already exists");
            registry.Add(++lastAssignedId, value);
            return lastAssignedId;
        }

        public int AddOrUpdate<T>(T value) where T : class,IElementWithIdentity
        {
            if (value == null)
                throw new ApplicationException("Cannot register null object in GRLContainer");
            if (registry.Any(keyval => keyval.Value.name == value.name))
                registry.Remove(Convert.ToInt32(value.id));
            return Add<T>(value);
        }


        public T GetValue<T>(int key) where T : class,IElementWithIdentity
        {
            return registry[key] as T;
        }

        public T GetElementByName<T>(string name) where T : class,IElementWithIdentity
        {
            var capitalisedName = String.IsNullOrEmpty(name) ? "" : char.ToUpper(name[0]) + name.Substring(1);
            if (registry.Any(keyval => keyval.Value.name == name || keyval.Value.name == capitalisedName))
                return registry.First(keyval => keyval.Value.name == name || keyval.Value.name == capitalisedName).Value as T;
            return null;
        }

        public int LastAddedElementId()
        {
            return lastAssignedId;
        }

        public T RegisterElement<T>(string name, out bool exists) where T : class,IElementWithIdentity, new()
        {
            T intElement = new T();
            exists = false;
            var existingElement = GetElementByName<T>(name);
            if (existingElement != null)
            {
                exists = true;
                return existingElement;
            }

            var id = Add<IElementWithIdentity>(intElement);
            intElement.id = id.ToString();
            // Capitalise first letter of name (if non empty)
            intElement.name = String.IsNullOrEmpty(name) ? "" : char.ToUpper(name[0]) + name.Substring(1);
            intElement.description = "";
            return intElement;
        }
    }

    interface IElementWithIdentity
    {
        string id { get; set; }
        string name { get; set; }
        string description { get; set; }
    }
}
