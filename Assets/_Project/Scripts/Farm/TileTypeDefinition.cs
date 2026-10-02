using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>Data definition of a terrain type. Add new ones without touching code.</summary>
    [CreateAssetMenu(menuName = "Meadowbrook/Tile Type")]
    public sealed class TileTypeDefinition : ScriptableObject
    {
        public string Id = "grass";
        public string DisplayName = "Grass";
        public Color Color = Color.green;
        public bool Buildable = true;

        public static TileTypeDefinition Create(string id, string displayName, Color color, bool buildable)
        {
            var def = CreateInstance<TileTypeDefinition>();
            def.Id = id;
            def.DisplayName = displayName;
            def.Color = color;
            def.Buildable = buildable;
            return def;
        }
    }
}
