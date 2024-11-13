

using System.Linq;
using System.Text.RegularExpressions;

internal class Utils
{
    public static string ParseIdFromRow(string row)
    {
        string[] parts = row.Split("=");
        return parts[0];
    }

    public static string[] SplitBetweenSingleBrackets(string rawRow)
    {
        string[] tokens = Regex.Split(rawRow, @"\((.*?)\)");
        return tokens;
    }

    private static Dictionary<string, string> _mapIdToType = new Dictionary<string, string>();

    public static string IfcRowTypeById(string id)
    {
        if ( id.StartsWith("#") == false )
        {
            return "";
        }

        if ( _mapIdToType.ContainsKey(id))
        {
            return _mapIdToType[id];
        }

        string row = IfcFileReader.GetRowById(id);
        string[] parts = row.Split("=");
        string type = parts[1].Split("(")[0];
        type = type.Trim();
        _mapIdToType[id] = type;
        return type;

    }

    public static string GetRowType(string row)
    {  
        return IfcRowTypeById(Utils.ParseIdFromRow(row));
    }


    public static string[] GetAllIdNumbersFromRowExceptFirst(string row)
    {
        row = row.Split("=")[1];
        Regex regex = new Regex(@"#[0-9]+");
        MatchCollection matches = regex.Matches(row);
        string[] ids = new string[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            ids[i] = matches[i].Value;
        }

        return ids;
        //if (ids.Length < 3) //There is only one id in the row
        //{
        //    return Array.Empty<string>();
        //}
        //Returns all the id numbers except the first one

        //return ids.Skip(1).ToArray();
    }
}