using System.Collections.Generic;
using UnityEngine;

public class DungeonTileBuilder : MonoBehaviour
{
    public DungeonGenerator generator;

    [Header("Prefabs de Construcción")]
    public GameObject floorPrefab;
    public GameObject wallPrefab; // ¡Nueva variable para la pared!
    public GameObject doorPrefab;
    
    [Header("Prefabs de Decoración")]
    public GameObject treasurePrefab;
    public GameObject typeA_Prefab;
    public GameObject typeB_Prefab;

    // Aquí guardaremos las coordenadas de todos los suelos para calcular los muros
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

    void Start()
    {
        if (generator != null && generator.rooms.Count > 0)
        {
            BuildDungeon();
        }
    }

    public void BuildDungeon()
    {
        floorPositions.Clear();

        // 1. Construir el suelo y la decoración de las habitaciones
        foreach (DungeonRoom room in generator.rooms)
        {
            BuildRoom(room);
        }

        // 2. Construir pasillos conectando las habitaciones
        BuildCorridors();

        // 3. ¡NUEVO! Levantar las paredes alrededor de los suelos
        BuildWalls();
    }

    void BuildRoom(DungeonRoom room)
    {
        for (int x = 0; x < room.bounds.width; x++)
        {
            for (int y = 0; y < room.bounds.height; y++)
            {
                Vector2Int tilePos = new Vector2Int(room.bounds.x + x, room.bounds.y + y);
                Vector3 position = new Vector3(tilePos.x, 0, tilePos.y);
                
                Instantiate(floorPrefab, position, Quaternion.identity, transform);
                floorPositions.Add(tilePos); // Guardamos la posición del suelo
            }
        }

        Vector3 centerPos = new Vector3(room.Center.x, 1, room.Center.y);
        
        if (room.type == RoomType.Treasure)
            Instantiate(treasurePrefab, centerPos, Quaternion.identity, transform);
        else if (room.type == RoomType.TypeA)
            Instantiate(typeA_Prefab, centerPos, Quaternion.identity, transform);
        else if (room.type == RoomType.TypeB)
            Instantiate(typeB_Prefab, centerPos, Quaternion.identity, transform);
        else if (room.type == RoomType.Entrance)
            Instantiate(doorPrefab, centerPos, Quaternion.identity, transform);
    }

    void BuildCorridors()
    {
        for (int i = 0; i < generator.rooms.Count - 1; i++)
        {
            Vector2Int start = generator.rooms[i].Center;
            Vector2Int end = generator.rooms[i + 1].Center;
            CreateLCorridor(start, end, generator.config.corridorWidth);
        }
    }

    void CreateLCorridor(Vector2Int start, Vector2Int end, int width)
    {
        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);
        for (int x = minX; x <= maxX; x++)
        {
            for (int w = 0; w < width; w++)
            {
                Vector2Int tilePos = new Vector2Int(x, start.y + w);
                Vector3 pos = new Vector3(tilePos.x, 0, tilePos.y);
                Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                floorPositions.Add(tilePos); // Guardamos el suelo del pasillo
            }
        }

        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);
        for (int y = minY; y <= maxY; y++)
        {
            for (int w = 0; w < width; w++)
            {
                Vector2Int tilePos = new Vector2Int(end.x + w, y);
                Vector3 pos = new Vector3(tilePos.x, 0, tilePos.y);
                Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                floorPositions.Add(tilePos); // Guardamos el suelo del pasillo
            }
        }
    }

    void BuildWalls()
    {
        // Posibles direcciones adyacentes (Arriba, Abajo, Izquierda, Derecha)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (Vector2Int pos in floorPositions)
        {
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = pos + dir;
                
                // Si al lado de este suelo NO hay otro suelo, y tampoco hemos puesto un muro ya ahí...
                if (!floorPositions.Contains(neighbor) && !wallPositions.Contains(neighbor))
                {
                    // Altura temporal de 0.5f suponiendo que el cubo del muro tiene escala Y=1
                    Vector3 wallPos = new Vector3(neighbor.x, 0.5f, neighbor.y);
                    Instantiate(wallPrefab, wallPos, Quaternion.identity, transform);
                    wallPositions.Add(neighbor); // Evitamos poner dos muros en el mismo sitio
                }
            }
        }
    }
}