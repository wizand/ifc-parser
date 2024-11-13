using System.Numerics;
using System.Text.RegularExpressions;

using IfcCoordinateParser.Entities;


/*In the IFC format, the line #500908= IFCLOCALPLACEMENT(#237408,#500907); defines an IFCLOCALPLACEMENT entity, which specifies the placement and orientation of an element within the 3D model. This entity is often used to position objects like columns, beams, or other structural components in the building model. Here’s a breakdown of each part:

    #500908: This is the unique identifier for this instance of IFCLOCALPLACEMENT. Other entities, such as IFCCOLUMN or IFCBEAM, may reference it to define their location and orientation within the model space.

    IFCLOCALPLACEMENT: The type of entity, representing a coordinate system or placement in the IFC schema. It defines both the position and orientation of an object in relation to another placement or a global coordinate system.

    Properties
    PlacementRelTo: #237408
    This property references another IFCLOCALPLACEMENT entity (in this case, with ID #237408). By referencing another placement, IFCLOCALPLACEMENT can be defined relative to a parent coordinate system. This hierarchical relationship allows objects to be positioned based on other objects or levels, helping to establish a spatial hierarchy within the model.
    
    RelativePlacement: #500907
    This property references an IFCAXIS2PLACEMENT3D entity (in this case, with ID #500907), which defines the exact position and orientation of the object within the referenced coordinate system. IFCAXIS2PLACEMENT3D specifies the local origin, as well as the direction of the X, Y, and Z axes, establishing a local coordinate system for the object.
    
    Summary
    This IFCLOCALPLACEMENT entity (#500908) establishes the location and orientation of an element in the 3D space by positioning it relative to another placement (#237408) and specifying the exact coordinates and orientation through IFCAXIS2PLACEMENT3D (#500907). This setup provides a flexible way to position objects within a model, allowing for relative placements that adapt to higher-level coordinate systems.
    */

public class LocalPlacementElement
{
    public string Id { get; set; }
    //public LocalPlacementElement PlacementRelTo { get; set; }
    public AxisToPlacement3d Axis2Placement3d { get; set; }

    public LocalPlacementElement(string row)
    {
        Id = Utils.ParseIdFromRow(row);
        //#500908= IFCLOCALPLACEMENT(#237408,#500907);
        string[] betweenBrackets =Utils.SplitBetweenSingleBrackets(row);
        string[] placementIds = betweenBrackets[1].Split(",");
        string relativePlacementRow = IfcFileReader.GetRowById(placementIds[1]); //IFCAXIS2PLACEMENT3D row
        Axis2Placement3d = new AxisToPlacement3d(relativePlacementRow, isRelativePlacementRow: true);
    }

}

