using System;
using System.Collections.Generic;

namespace Meadowbrook.Farm
{
    public readonly struct GridCoord : IEquatable<GridCoord>
    {
        public readonly int X;
        public readonly int Y;

        public GridCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(GridCoord other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is GridCoord other && Equals(other);
        public override int GetHashCode() => (X * 397) ^ Y;
        public override string ToString() => "(" + X + ", " + Y + ")";
        public static bool operator ==(GridCoord a, GridCoord b) => a.Equals(b);
        public static bool operator !=(GridCoord a, GridCoord b) => !a.Equals(b);
    }

    /// <summary>One square of land. Plain data: no Unity types, easy to save later.</summary>
    public sealed class GridCell
    {
        public GridCoord Coord;
        public string TileTypeId;
        public bool IsUnlocked;
        /// <summary>Reserved for crops/buildings in later phases. Null = empty.</summary>
        public string OccupantId;
    }

    /// <summary>The farm grid model. Single source of truth for land state.</summary>
    public sealed class FarmGrid
    {
        readonly GridCell[] cells;

        public int Width { get; }
        public int Height { get; }

        public event Action<GridCell> CellChanged;

        public FarmGrid(int width, int height, string defaultTileId)
        {
            Width = width;
            Height = height;
            cells = new GridCell[width * height];
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                cells[y * width + x] = new GridCell
                {
                    Coord = new GridCoord(x, y),
                    TileTypeId = defaultTileId,
                    IsUnlocked = false,
                    OccupantId = null
                };
        }

        public IEnumerable<GridCell> Cells => cells;

        public bool InBounds(GridCoord c) => c.X >= 0 && c.Y >= 0 && c.X < Width && c.Y < Height;

        public GridCell GetCell(GridCoord c) => InBounds(c) ? cells[c.Y * Width + c.X] : null;

        public void SetTileType(GridCoord c, string tileTypeId)
        {
            var cell = GetCell(c);
            if (cell == null || cell.TileTypeId == tileTypeId) return;
            cell.TileTypeId = tileTypeId;
            CellChanged?.Invoke(cell);
        }

        public void SetUnlocked(GridCoord c, bool unlocked)
        {
            var cell = GetCell(c);
            if (cell == null || cell.IsUnlocked == unlocked) return;
            cell.IsUnlocked = unlocked;
            CellChanged?.Invoke(cell);
        }

        public void SetUnlockedRect(int x, int y, int w, int h, bool unlocked)
        {
            for (int yy = y; yy < y + h; yy++)
            for (int xx = x; xx < x + w; xx++)
                SetUnlocked(new GridCoord(xx, yy), unlocked);
        }
    }
}
