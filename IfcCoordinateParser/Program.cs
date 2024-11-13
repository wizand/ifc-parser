

IfcFileReader.Init("WoodenOffice.ifc");
IfcFileReader.ReadIfcModel();
IfcFileReader.PopulateIfcRowReferences();


//CoordinatesHelper CoordinatesHelper = new(reader);
CoordinatesHelper.BuildCoodrinatesMap();

ElementHelper elementHelper = new();

elementHelper.BuildColumnElements();
elementHelper.BuildBeamElements();


bool dontQuit = true;
string cmd = "";


while (dontQuit)
{
    Console.WriteLine("Please enter command (? for list of commands):");
    cmd = Console.ReadLine();
    cmd = cmd.Trim().ToUpper();
    switch (cmd)
    {
        case "?":
            Console.WriteLine("BEAMS: Listing beams");
            Console.WriteLine("COLUMNS: Listing columns");
            Console.WriteLine("QUIT: Quit");
            break;
        case "BEAMS":
            Console.WriteLine("Listing beams");
            PrintElements(elements: elementHelper.BeamElements as List<IPrintableElement>, typeName: "BEAMS");
            break;
        case "COLUMNS":
            Console.WriteLine("Listing columns");
            break;
        case "QUIT":
            Console.WriteLine("Quitting..");
            dontQuit = false;
            break;
    }
}

void PrintElements(List<IPrintableElement> elements, string typeName)
{
    Console.WriteLine(elementHelper.PrintInRow(typeName, 6, elements));
}