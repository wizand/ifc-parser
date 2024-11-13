using System.Numerics;

using IfcCoordinateParser;

public class ColumnElement : IPrintableElement
{
    //IFCCOLUMN('32SxG1TuHENPjvPbftkZZ9',#45,'LVL-PILARI','300x590',$,#500908,#500905,'IDc273b401-7784-4e5d-9b79-665a77ba38c9');
    /*#500909: Unique identifier for this IFCCOLUMN instance. Other entities in the IFC file can reference it using this ID.

    IFCCOLUMN: The type of entity, representing a structural column in the building model.

    Properties
    GlobalId: '32SxG1TuHENPjvPbftkZZ9'
    A globally unique identifier (UUID) for this column instance, typically generated automatically.

    OwnerHistory: #45
    Refers to an IFCOWNERHISTORY entity that contains metadata about the ownership and history, such as who created or modified this entity.

    Name: 'LVL-PILARI'
    The name of the column, "LVL-PILARI". Here, "PILARI" translates to "column" in Finnish, and "LVL" refers to Laminated Veneer Lumber, indicating the column material type.

    Description: '300x590'
    Describes the dimensions of the column, 300x590, likely in millimeters, with 300 possibly being the width and 590 the height.

    ObjectType: $
    This field would specify a user-defined type for the object, but here it’s marked with $, meaning it’s undefined or not provided.

    ObjectPlacement: #500908
    References an IFCLOCALPLACEMENT entity that specifies the column's location and orientation within the 3D space of the model.

    Representation: #500905
    References an IFCPRODUCTREPRESENTATION entity, which defines the column's geometric representation, such as its shape and dimensions.

    Tag: 'IDc273b401-7784-4e5d-9b79-665a77ba38c9'
    A unique identifier or tag specific to this column, used for tracking purposes within the building information model (BIM).
    */


    public string Id { get; set; }
    private Vector3 coordinates;
    public LocalPlacementElement ObjectPlacement { get; set; }

    public ColumnElement(string id)
    {
        this.Id = id;

        string[] parametersBetweenBrackers = Utils.SplitBetweenSingleBrackets(IfcFileReader.GetRowById(id));
        string[] parameters = parametersBetweenBrackers[1].Split(",");

        foreach(string parameter in parameters)
        {
            if( Utils.IfcRowTypeById(parameter).Equals(IfcTypes.IFCLOCALPLACEMENT_IDENTIFIER) )
            {
                string localPlacementId = Utils.ParseIdFromRow(parameter);
                ObjectPlacement = new LocalPlacementElement(IfcFileReader.GetRowById(localPlacementId));
                break;
            } else
            {
                continue;
            }
        }
    }

    public ColumnElement(IfcRow ifcRow)
    {
        this.Id = ifcRow.Id;

        foreach(IfcRow referencedRow in ifcRow.RowReferences)
        {
            if(referencedRow.Type == IfcTypes.IFCLOCALPLACEMENT_IDENTIFIER)
            {
                ObjectPlacement = new LocalPlacementElement(referencedRow.Row);
                break;
            }
        }
        //string[] parametersBetweenBrackers = Utils.SplitBetweenSingleBrackets(ifcRow.Row);
        //string[] parameters = parametersBetweenBrackers[1].Split(",");

        //foreach (string parameter in parameters)
        //{
        //    if (Utils.IfcRowTypeById(parameter).Equals(IfcTypes.IFCLOCALPLACEMENT_IDENTIFIER))
        //    {
        //        string localPlacementId = Utils.ParseIdFromRow(parameter);
        //        ObjectPlacement = new LocalPlacementElement(IfcFileReader.GetRowById(localPlacementId));
        //        break;
        //    }
        //    else
        //    {
        //        continue;
        //    }
        //}
    }

    public string GetAsPrintableString()
    {
        return $"ID=[{Id}]";
    }
}