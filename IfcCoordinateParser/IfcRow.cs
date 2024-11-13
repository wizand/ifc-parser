using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfcCoordinateParser
{
    public class IfcRow
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Row { get; set; }

        public List<IfcRow> RowReferences { get; set;} 

        public IfcRow(string rawRow)
        {
            Id = Utils.ParseIdFromRow(rawRow);
            Type = Utils.GetRowType(Id);
            Row = rawRow;
            RowReferences = new List<IfcRow>();
        }

        public void PopulateRowReferences()
        {
            string[] referencedIds = Utils.GetAllIdNumbersFromRowExceptFirst(Row);
            foreach (string id in referencedIds)
            {
                IfcRow ifcRow = IfcFileReader.GetIfcRowById(id);
                RowReferences.Add(ifcRow);
            }

        }


    }
}
