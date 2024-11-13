// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

using IfcCoordinateParser;

public static class IfcFileReader
{
    public static void Init(string fileName, string path = "IfcModels\\")
    {
        FileName = fileName;
        FolderPath = path;
    }

    public static string FileName { get; set; }
    public static string FolderPath { get; set; }


    public static Dictionary<string, string> mapIdToRow;

    private static string[]? _rows = null;
    public static string[]? IfcContentRows
    {
        get
        {
            if (_rows == null)
            {
                int ret = ReadIfcModel();
                if (ret == 0 || ret == -1)
                {
                    return null;
                }
            }
            return _rows;
        }
        set => _rows = value;
    }

    
    

    //public CoordinateHandler CoordinateHandler { get; set; }

    public static int ReadIfcModel()
    {
        Console.WriteLine("Trying to parse " + FileName);
        try
        {
            // Read the IFC file and return the number of objects read
            IfcContentRows = File.ReadAllLines(FolderPath + FileName);
            mapIdToRow = new Dictionary<string, string>();
            _mapIdToIfcRow = new();
            for (int i = 0; i < IfcContentRows.Length; i++)
            {
                string row = IfcContentRows[i];
                string id = Utils.ParseIdFromRow(row);
                if ( id.StartsWith("#") )
                {
                    mapIdToRow[id] = row;
                    IfcRow ifcRow = new IfcRow(row);
                    _mapIdToIfcRow[id] = ifcRow;
                }
            }
            Console.WriteLine("Data rows read " + IfcContentRows.Length + " ids stored in map " + mapIdToRow.Count);
            return IfcContentRows.Length;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error reading file: " + e.Message);
            return -1;
        }
    }

    public static string GetRowById(string id)
    {
        return mapIdToRow[id];
    }


    private static Dictionary<string, IfcRow> _mapIdToIfcRow = new Dictionary<string, IfcRow>();

    internal static IfcRow GetIfcRowById(string id)
    {
        return _mapIdToIfcRow[id];
    }

    public static void PopulateIfcRowReferences()
    {
        foreach (var ifcRow in _mapIdToIfcRow.Values)
        {
            ifcRow.PopulateRowReferences();
        }
    }

    internal static IfcRow[] GetIfcRowsByType(string type)
    {
        List<IfcRow> ifcRows = new List<IfcRow>();
        foreach (var ifcRow in _mapIdToIfcRow.Values)
        {
            if (ifcRow.Type == type)
            {
                ifcRows.Add(ifcRow);
            }
        }
        Console.WriteLine($"{ifcRows.Count} IfcRows for type {type} found.");
        return ifcRows.ToArray();
    }
}