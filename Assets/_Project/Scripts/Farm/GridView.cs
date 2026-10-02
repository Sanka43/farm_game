using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>Renders the grid. Holds no state: it mirrors FarmGrid and reacts to CellChanged.</summary>
    public sealed class GridView : MonoBehaviour
    {
        FarmGrid grid;
        GridConfig config;
        SpriteRenderer[] tiles;

        public void Init(FarmGrid farmGrid, GridConfig gridConfig)
        {
            grid = farmGrid;
            config = gridConfig;
            tiles = new SpriteRenderer[grid.Width * grid.Height];

            foreach (var cell in grid.Cells)
            {
                var go = new GameObject("Tile_" + cell.Coord.X + "_" + cell.Coord.Y);
                go.transform.SetParent(transform, false);
                go.transform.position = GridCoordUtility.CellToWorld(cell.Coord, config.CellSize);
                go.transform.localScale = Vector3.one * config.CellSize;

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = PlaceholderSprites.Square;
                sr.sortingOrder = 0;
                tiles[cell.Coord.Y * grid.Width + cell.Coord.X] = sr;
                Refresh(cell);
            }

            grid.CellChanged += Refresh;
        }

        void OnDestroy()
        {
            if (grid != null) grid.CellChanged -= Refresh;
        }

        void Refresh(GridCell cell)
        {
            var def = config.GetTileType(cell.IsUnlocked ? cell.TileTypeId : config.LockedTileId);
            var color = def != null ? def.Color : Color.magenta;
            // Subtle checkerboard so individual cells are easy to see.
            float shade = ((cell.Coord.X + cell.Coord.Y) & 1) == 0 ? 1f : 0.93f;
            tiles[cell.Coord.Y * grid.Width + cell.Coord.X].color = new Color(color.r * shade, color.g * shade, color.b * shade, 1f);
        }
    }
}
