
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

public static class CoordinatesHelper
{
    public static Dictionary<string, Vector3> mapIdToCoordiantes = new Dictionary<string, Vector3>();
    public static Dictionary<string, Vector3> mapIdToDirections = new Dictionary<string, Vector3>();
    public static int BuildCoodrinatesMap()
    {
        
        if (IfcFileReader.IfcContentRows == null)
        {
            int ret = IfcFileReader.ReadIfcModel();
            if (ret == 0 || ret == -1)
            {
                return -1;
            }
        }


        for (int i = 0; i < IfcFileReader.IfcContentRows!.Length; i++)
        {
            string row = IfcFileReader.IfcContentRows[i];
            if (row.Contains("= IFCCARTESIANPOINT("))
            {
                string id = ParseCoordinateId(row);
                Vector3 coordinates = ParseCoordinates(row);
                mapIdToCoordiantes[id] = coordinates;
            }
            else if (row.Contains("= IFCDIRECTION("))
            {
                string id = Utils.ParseIdFromRow(row);
                Vector3 direction = ParseCoordinates(row);
                mapIdToDirections[id] = direction;
            }
        }
        Console.WriteLine("Coordinates parsed: " + mapIdToCoordiantes.Count+ " Directions parsed: " + mapIdToDirections.Count);
        return mapIdToCoordiantes.Count;
    }

    private static string ParseCoordinateId(string row)
    {
        return Utils.ParseIdFromRow(row);
    }
    private static Vector3 ParseCoordinates(string row)
    {
        //#4457= IFCDIRECTION((0.0537002650466456,0.273348529061069,-0.960414943237593));
        //#873774= IFCCARTESIANPOINT((109232.026907855,78403.3692441094,16129.));
        //RegExp the content between (( and )) out of the string \(\((.*?)\)\)
        //Regex regex = new Regex(@"\(\((.*?)\)\)");
        string[] betweenBrackets = Regex.Split(row, @"\(\((.*?)\)\)");
        string[] coordinateStrings = betweenBrackets[1].Split(",");
        for (int i = 0; i < coordinateStrings.Length; i++)
        {

            if (coordinateStrings[i].EndsWith("."))
            {
                coordinateStrings[i] = coordinateStrings[i] + "0";
            }
        }
        if (coordinateStrings.Length == 3)
        {
            return new Vector3(float.Parse(coordinateStrings[0], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[1], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[2], CultureInfo.InvariantCulture));
        }

        else // (coordinateStrings.Length == 2)
        {
            return new Vector3(float.Parse(coordinateStrings[0], CultureInfo.InvariantCulture), float.Parse(coordinateStrings[1], CultureInfo.InvariantCulture), 0);
        }

    }
}

