using System;
using System.Xml.Linq;

namespace Authentication.System.API.Extensions
{
    public class TableAttribute:Attribute
    {
        private string TableName;

        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }
        public string GetName() => TableName;
    }
}
