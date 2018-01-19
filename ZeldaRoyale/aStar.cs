using System.Collections.Generic;
using RoyT.AStar;
using Microsoft.Xna.Framework;

namespace ZeldaRoyale
{
    public class aStar
    {
        public aStar()
        {

        }

        public static List<Vector2> getPath(Vector2 initalPosition, Vector2 targetPosition)
        {
            var grid = new Grid((int)Game1.windowWidth, (int)Game1.windowHeight, 1.0f);
            var movementPattern = new[] { new Offset(-1, 0), new Offset(0, -1), new Offset(1, 0), new Offset(0, 1) };
            Position[] path = grid.GetPath(new Position((int)initalPosition.X, (int)initalPosition.Y), new Position((int)targetPosition.X, (int)targetPosition.Y));
            List<Vector2> finalPath = new List<Vector2>();
            for (int i = 0; i < path.Length; i++)
            {
                finalPath.Add(new Vector2(path[i].X, path[i].Y));
            }
            return finalPath;
        }
    }
}