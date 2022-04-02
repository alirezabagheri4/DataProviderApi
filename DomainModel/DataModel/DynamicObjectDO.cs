using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DataModel
{
    public class DynamicObjectDO
    {
        public DynamicObjectDO(Tuple<string, int> classNameAndId, string propertyName, string propertyValue, string propertyType)
        {
            ClassNameAndId = classNameAndId;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            PropertyType = propertyType;
        }

        public Tuple<string, int> ClassNameAndId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyType { get; set; }
    }
}
