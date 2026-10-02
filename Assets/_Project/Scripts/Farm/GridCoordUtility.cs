using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>
    /// The ONLY place that converts between grid cells and world positions.
    /// Cell (0,0) is the bottom-left; the grid origin is world (0,0).
    /// </summary>
    public static class GridCoordUtility
    {
        public static Vector3 CellToWorld(GridCoord c, float cellSize)
        {
            return new Vector3((c.X + 0.5f) * cellSize, (c.Y + 0.5f) * cellSize, 0f);
        }

        public static GridCoord WorldToCell(Vector3 world, float cellSize)
        {
            return new GridCoord(Mathf.FloorToInt(world.x / cellSize), Mathf.FloorToInt(world.y / cellSize));
        }
    }
}
