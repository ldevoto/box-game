using System.Collections.Generic;

namespace UnityTemplateProjects.Base
{
    public class MovementUtils
    {
        public static readonly Dictionary<Movement, Direction> DirectionByMovement = new Dictionary<Movement, Direction>()
        {
            {Movement.FORWARD, Direction.FORWARD},
            {Movement.BACKWARD, Direction.BACKWARD},
            {Movement.LEFT, Direction.LEFT},
            {Movement.RIGHT, Direction.RIGHT}
        };
    }
}