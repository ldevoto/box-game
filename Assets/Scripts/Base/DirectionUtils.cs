using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects.Base
{
    public class DirectionUtils
    {
        public static readonly Dictionary<Direction, Vector3> normalByDirection = new Dictionary<Direction, Vector3>
        {
            {Direction.UP, Vector3.up},
            {Direction.DOWN, Vector3.down},
            {Direction.LEFT, Vector3.left},
            {Direction.RIGHT, Vector3.right},
            {Direction.FORWARD, Vector3.forward},
            {Direction.BACKWARD, Vector3.back}
        };
        public static readonly Dictionary<Vector3, Direction> DirectionByNormal = new Dictionary<Vector3, Direction>
        {
            {Vector3.up, Direction.UP},
            {Vector3.down, Direction.DOWN},
            {Vector3.left, Direction.LEFT},
            {Vector3.right, Direction.RIGHT},
            {Vector3.forward, Direction.FORWARD},
            {Vector3.back, Direction.BACKWARD}
        };
        public static readonly Dictionary<Direction, Vector3> RotationAxisByDirection = new Dictionary<Direction, Vector3>
        {
            {Direction.UP, Vector3.up},
            {Direction.DOWN, Vector3.down},
            {Direction.LEFT, Vector3.forward},
            {Direction.RIGHT, Vector3.back},
            {Direction.FORWARD, Vector3.right},
            {Direction.BACKWARD, Vector3.left}
        };

        public static Vector3 GetPivotForMovement(Direction moveTo, Vector3 currentPosition)
        {
            switch (moveTo)
            {
                case Direction.FORWARD:
                    return currentPosition + new Vector3(0, - WorldConstants.halfUnit, WorldConstants.halfUnit);
                case Direction.BACKWARD:
                    return currentPosition + new Vector3(0, - WorldConstants.halfUnit, -WorldConstants.halfUnit);
                case Direction.RIGHT:
                    return currentPosition + new Vector3(WorldConstants.halfUnit, - WorldConstants.halfUnit, 0);
                case Direction.LEFT:
                    return currentPosition + new Vector3(-WorldConstants.halfUnit, - WorldConstants.halfUnit, 0);
                default:
                    return currentPosition;
            }
        }
    }
}