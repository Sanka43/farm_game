using Meadowbrook.Core;
using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>Draws a frame around the selected cell.</summary>
    public sealed class SelectionHighlight : MonoBehaviour
    {
        SpriteRenderer sr;
        float cellSize = 1f;

        public void Init(float size)
        {
            cellSize = size;
            transform.localScale = Vector3.one * cellSize;
        }

        void Awake()
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = PlaceholderSprites.Outline;
            sr.color = new Color(1f, 0.92f, 0.2f, 1f);
            sr.sortingOrder = 10;
            sr.enabled = false;
        }

        void OnEnable() => EventBus.Subscribe<CellSelectedEvent>(OnSelected);
        void OnDisable() => EventBus.Unsubscribe<CellSelectedEvent>(OnSelected);

        void OnSelected(CellSelectedEvent e)
        {
            transform.position = GridCoordUtility.CellToWorld(e.Coord, cellSize);
            sr.enabled = true;
        }
    }
}
