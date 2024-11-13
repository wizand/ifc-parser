using System.Numerics;

using IfcCoordinateParser;

public class BeamElement : IPrintableElement
{
    /*
            //#457621= IFCBEAM('2KH4fGC_9AGeMZwtXY4KCm',#45,'LVL-PALKKI','225x590',$,#457620,#457617,'ID94444a50-33e2-4a42-85a3-eb7862114330');
            /*GlobalId: '2KH4fGC_9AGeMZwtXY4KCm'

            A globally unique identifier (UUID) for this beam instance, typically generated automatically.
            OwnerHistory: #45

            Refers to another IFC entity (likely an IFCOWNERHISTORY) that holds metadata about ownership and history, including information about the creation and last modification of this entity.

            Name: 'LVL-PALKKI'
            The name of the beam, "LVL-PALKKI", which could indicate the type of beam (in this case, "palkki" translates to "beam" in Finnish, and "LVL" refers to Laminated Veneer Lumber, a common engineered wood product).

            Description: '225x590'
            A description of the beam, here indicating its dimensions, 225x590 (likely in millimeters, referring to width and height respectively).

            ObjectType: $
            This field typically specifies a user-defined type for the object, but it’s marked with $, meaning it is not provided or left undefined.

            ObjectPlacement: #457620
            References an IFCLOCALPLACEMENT entity, defining the beam's location and orientation within the 3D space of the model.

            Representation: #457617
            References an IFCPRODUCTREPRESENTATION entity, which provides the geometric representation of the beam (e.g., shape, extrusion path, etc.).

            Tag: 'ID94444a50-33e2-4a42-85a3-eb7862114330'
            A unique identifier or tag specific to this beam, potentially used for tracking within a construction or building information model (BIM) system.
            */
    public string Id { get; set; }
    public LocalPlacementElement ObjectPlacement { get; set; }

    public BeamElement(IfcRow ifcRow)
    {
        this.Id = ifcRow.Id;

        foreach (IfcRow referencedRow in ifcRow.RowReferences)
        {
            if (referencedRow.Type == IfcTypes.IFCLOCALPLACEMENT_IDENTIFIER)
            {
                ObjectPlacement = new LocalPlacementElement(referencedRow.Row);
                break;
            }
        }
    }

    public string GetAsPrintableString()
    {
        return $"ID=[{Id}]";
    }
}

