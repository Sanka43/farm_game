using System.Collections.Generic;
using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>
    /// Grid settings. Create an asset at Resources/Configs/GridConfig to override the defaults
    /// (Create > Meadowbrook > Grid Config). If none exists, defaults below are used.
    /// </summary>
    [CreateAssetMenu(menuName = "Meadowbrook/Grid Config")]
    public sealed class GridConfig : ScriptableObject
    {
        public int Width = 24;
        public int Height = 24;
        public float CellSize = 1f;
        public string DefaultTileId = "grass";
        public string LockedTileId = "locked";
        public RectInt InitialUnlocked = new RectInt(8, 8, 8, 8);
        public List<TileTypeDefinition> TileTypes = new List<TileTypeDefinition>();

        public TileTypeDefinition GetTileType(string id)
        {
            foreach (var t in TileTypes)
                if (t != null && t.Id == id) return t;
            return null;
        }

        public static GridConfig CreateDefault()
        {
            var cfg = CreateInstance<GridConfig>();
            cfg.TileTypes.Add(TileTypeDefinition.Create("grass", "Grass", new Color(0.47f, 0.80f, 0.32f), true));
            cfg.TileTypes.Add(TileTypeDefinition.Create("soil", "Soil", new Color(0.55f, 0.38f, 0.22f), true));
            cfg.TileTypes.Add(TileTypeDefinition.Create("locked", "Locked", new Color(0.30f, 0.42f, 0.30f), false));
            return cfg;
        }
    }
}
