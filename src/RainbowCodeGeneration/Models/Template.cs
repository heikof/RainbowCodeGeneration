using System.Collections.Generic;
using Rainbow.Model;

namespace RainbowCodeGeneration.Models
{
    public class Template
    {
        public IItemData Item { get; private set; }
        public IEnumerable<IItemData> Fields { get; private set; }

        
        public Template(IItemData item, IEnumerable<IItemData> fields)
        {
            this.Item = item;
            this.Fields = fields;
        }
    }
}
