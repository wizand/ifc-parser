
using System.Numerics;
using System.Text;

using IfcCoordinateParser;

public class ElementHelper
{

    public List<IPrintableElement> BeamElements { get; set; } = null;
    public List<IPrintableElement> ColumnElements { get; set; } = null;

    private const string BEAM_IDENTIFIER= "IFCBEAM";
    private const string COLUMN_IDENTIFIER = "IFCCOLUMN";

    public int BuildBeamElements()
    {
        if (IfcFileReader.IfcContentRows == null)
        {
            int ret = IfcFileReader.ReadIfcModel();
            if (ret == 0 || ret == -1)
            {
                return -1;
            }
        }

        BeamElements = new List<IPrintableElement>();

        IfcRow[] beamTypeIfcRows = IfcFileReader.GetIfcRowsByType(BEAM_IDENTIFIER);
        for ( int i = 0; i < beamTypeIfcRows.Length; i++)
        {
            BeamElements.Add(new BeamElement(beamTypeIfcRows[i]));
        }
    
        Console.WriteLine("Beams identified: " + BeamElements.Count);
        return BeamElements.Count;
    }

    public int BuildColumnElements()
    {
        if (IfcFileReader.IfcContentRows == null)
        {
            int ret = IfcFileReader.ReadIfcModel();
            if (ret == 0 || ret == -1)
            {
                return -1;
            }
        }

        ColumnElements = new();


        IfcRow[] columnTypeIfcRows = IfcFileReader.GetIfcRowsByType(COLUMN_IDENTIFIER);
        for (int i = 0; i < columnTypeIfcRows.Length; i++)
        {
            ColumnElements.Add(new ColumnElement(columnTypeIfcRows[i]));
        }

        //for (int i = 0; i < IfcFileReader.IfcContentRows!.Length; i++)
        //{
        //    string row = IfcFileReader.IfcContentRows[i];



        //    if (row.Contains("= IFCCOLUMN("))
        //    {
        //        string id = ParseElementId(row);

        //        ColumnElements.Add(new ColumnElement(id));
        //    }
        //    else
        //    {
        //        continue;
        //    }
        //}

        Console.WriteLine("Columns identified: " + ColumnElements.Count);
    return ColumnElements.Count;
    }

    private string ParseElementId(string row)
    {
        return Utils.ParseIdFromRow(row);
    }

    public string PrintInRow(string elementName, int numberOfElementsInRow, List<IPrintableElement> printableElements) 
    {
        StringBuilder sb = new StringBuilder(elementName);

        int index = 0;
        for ( int i = 0; i < printableElements.Count ; i++)
        {
            if (index < numberOfElementsInRow)
            {
                sb.Append(printableElements[i].GetAsPrintableString());
                sb.Append("\t");
                index = index + 1;
            }
            else
            {
                sb.AppendLine("");
                index = 0;
            }
        }
        
        return sb.ToString();
        //TODO: Change to yield
    }
}
