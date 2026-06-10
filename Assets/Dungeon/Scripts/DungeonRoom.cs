using UnityEngine;

public enum RoomType { Normal, TypeA, TypeB, Treasure, Entrance }

public class DungeonRoom
{
    public RectInt bounds; // Guarda la posición X, Y, ancho y alto
    public RoomType type;

    public DungeonRoom(Vector2Int position, Vector2Int size)
    {
        bounds = new RectInt(position, size);
        type = RoomType.Normal;
    }

    // Propiedad súper útil para saber el punto central y trazar pasillos luego
    public Vector2Int Center => new Vector2Int(bounds.x + bounds.width / 2, bounds.y + bounds.height / 2);
}