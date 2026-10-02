using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>Runtime-generated placeholder sprites. Replace with real art later.</summary>
    public static class PlaceholderSprites
    {
        static Sprite square;
        static Sprite outline;

        /// <summary>A white 1x1 world-unit square (tint it with SpriteRenderer.color).</summary>
        public static Sprite Square
        {
            get
            {
                if (square == null)
                {
                    var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
                    var px = new Color32[16];
                    for (int i = 0; i < px.Length; i++) px[i] = new Color32(255, 255, 255, 255);
                    tex.SetPixels32(px);
                    tex.Apply();
                    square = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
                }
                return square;
            }
        }

        /// <summary>A hollow 1x1 world-unit square frame, used for selection.</summary>
        public static Sprite Outline
        {
            get
            {
                if (outline == null)
                {
                    const int size = 32, border = 3;
                    var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
                    var px = new Color32[size * size];
                    for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                    {
                        bool edge = x < border || y < border || x >= size - border || y >= size - border;
                        px[y * size + x] = edge ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 0);
                    }
                    tex.SetPixels32(px);
                    tex.Apply();
                    outline = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
                }
                return outline;
            }
        }
    }
}
