using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IfcCoordinateParser.Entities
{
    public class AxisToPlacement3d
    {

        /*
         * In the IFC format, the line #500907= IFCAXIS2PLACEMENT3D(#500906,#342190,#9); defines an IFCAXIS2PLACEMENT3D entity, which represents a three-dimensional coordinate system placement, commonly used to specify the exact position and orientation of objects within the model. Here’s a breakdown of each part:

           #500907: This is the unique identifier for this instance of IFCAXIS2PLACEMENT3D. Other entities, like IFCLOCALPLACEMENT, reference this ID to establish the precise position and orientation of objects.

           IFCAXIS2PLACEMENT3D: This entity type defines a three-dimensional coordinate system by specifying an origin point and directions for the primary axes. It helps establish the orientation of an element in the 3D model space.

           Properties

           Location: #500906
           This references an IFCCARTESIANPOINT (with ID #500906), which defines the origin point of the local coordinate system in the 3D space. This origin serves as the base point for the object’s placement.

           Axis: #342190
           This references an IFCDIRECTION entity (with ID #342190), which specifies the direction of the Z-axis of this coordinate system. The Z-axis is often used as the "up" or "height" direction in 3D placements. By defining the Z-axis, we set the orientation of the object vertically.

           RefDirection: #9
           This references another IFCDIRECTION entity (with ID #9), which specifies the direction of the X-axis of this coordinate system. The X-axis direction is important for defining the orientation of the object horizontally. The Y-axis is implicitly defined as perpendicular to both the X- and Z-axis directions.

           Summary
           This IFCAXIS2PLACEMENT3D entity (#500907) establishes a local 3D coordinate system with:

           A location (origin point) at #500906.
           An axis for the Z direction (#342190), setting the vertical orientation.
           A reference direction for the X-axis (#9), setting the horizontal orientation.
           Together, these properties fully define a coordinate system for positioning and orienting objects in the 3D space, allowing precise placement and rotation of elements within the IFC model.
        */

        public AxisToPlacement3d(string placementRow, bool isRelativePlacementRow = true)
        {

            if (isRelativePlacementRow)
            {
                Init( placementRow);
                return;
            }
            string[] betweenBrackets = Regex.Split(placementRow, @"\((.*?)\)");
            string[] placementIds = betweenBrackets[1].Split(",");
            string relativePlacementRow = IfcFileReader.GetRowById(placementIds[1]); //IFCAXIS2PLACEMENT3D , relativePlacementRow
            Init(relativePlacementRow);
        }     

        public void Init(string relativePlacementRow)
        {
            Id = Utils.ParseIdFromRow(relativePlacementRow);
            //#500907= IFCAXIS2PLACEMENT3D(#500906,#342190,#9);
            string[] betweenBrackets = Utils.SplitBetweenSingleBrackets(relativePlacementRow);
            betweenBrackets = betweenBrackets[1].Split(",");
            string locationId = betweenBrackets[0];
            string axisDirId = betweenBrackets[1];
            string refDirId = betweenBrackets[2];
            Location = CoordinatesHelper.mapIdToCoordiantes[locationId];
            AxisDirection = CoordinatesHelper.mapIdToDirections[axisDirId];
            RefDirection = CoordinatesHelper.mapIdToDirections[refDirId];
        }


        public string Id { get; set; }
        public Vector3 Location { get; set; }
        public Vector3 AxisDirection { get; set; }
        public Vector3 RefDirection { get; set; }
    }
}
