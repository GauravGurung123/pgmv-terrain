using System.Collections.Generic;
using UnityEngine;


public class DungeonTileBuilder : MonoBehaviour
{
    public DungeonGenerator generator;

    [Header("Prefabs de Construcción")]
    public GameObject floorPrefab;
    public GameObject wallPrefab; 
    public GameObject wallLightPrefab;
    public GameObject pillarPrefab;
    public GameObject doorPrefab;
    public GameObject ceilingPrefab;

    [Header("Ajustes de Construcción")]
    public float ceilingHeight = 4.5f;
    
    [Header("Prefabs de Decoración")]
    public GameObject treasurePrefab;
    public GameObject typeA_Prefab;
    public GameObject typeB_Prefab;

    
    [Header("Player Settings")]
    public GameObject playerAvatar;

    // Aquí guardaremos las coordenadas de todos los suelos para calcular los muros
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

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
        wallPositions.Clear();

        // 1. Construir el suelo y la decoración de las habitaciones
        foreach (DungeonRoom room in generator.rooms)
        {
            BuildRoom(room);
        }

        // 2. Construir pasillos conectando las habitaciones
        BuildCorridors();

        // 3. ¡NUEVO! Levantar las paredes alrededor de los suelos
        BuildWalls();

        BuildPillars();

        BuildCeilings();

        if (playerAvatar != null && generator.rooms.Count > 0)
        {
            Vector3 startPos = new Vector3(generator.rooms[0].Center.x, 1f, generator.rooms[0].Center.y);
            
            // Truco de Unity: Para mover un CharacterController instantáneamente, hay que apagarlo y encenderlo
            CharacterController cc = playerAvatar.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            
            playerAvatar.transform.position = startPos;
            
            if (cc != null) cc.enabled = true;
        }
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
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        int wallCounter = 0; // <--- ¡NUESTRO CONTADOR DE PAREDES!

        foreach (Vector2Int pos in floorPositions)
        {
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = pos + dir;
                
                if (!floorPositions.Contains(neighbor) && !wallPositions.Contains(neighbor))
                {
                    Vector3 wallPos = new Vector3(neighbor.x, 0f, neighbor.y);
                    Quaternion wallRotation = Quaternion.identity;

                    if (dir == Vector2Int.up) wallRotation = Quaternion.Euler(0, 0, 0); 
                    else if (dir == Vector2Int.down) wallRotation = Quaternion.Euler(0, 180, 0);
                    else if (dir == Vector2Int.left) wallRotation = Quaternion.Euler(0, -90, 0);
                    else if (dir == Vector2Int.right) wallRotation = Quaternion.Euler(0, 90, 0);

                    wallCounter++; 
                    GameObject selectedWallPrefab = wallPrefab;

                 
                    if (wallCounter % 6 == 0)
                    {
                        selectedWallPrefab = wallLightPrefab;
                    }

                    Instantiate(selectedWallPrefab, wallPos, wallRotation, transform);
                    wallPositions.Add(neighbor);
                }
            }
        }
    }

    void BuildCeilings()
    {
        // Giramos el techo 180 grados para que la cara "visible" o texturizada mire hacia abajo (hacia el jugador)
        Quaternion ceilingRotation = Quaternion.Euler(180f, 0f, 0f);

        foreach (Vector2Int pos in floorPositions)
        {
            // Misma posición X y Z que el suelo, pero elevado a la altura de 'ceilingHeight'
            Vector3 ceilingPos = new Vector3(pos.x, ceilingHeight, pos.y);
            Instantiate(ceilingPrefab, ceilingPos, ceilingRotation, transform);
        }
    }

    void BuildPillars()
    {
        HashSet<Vector2Int> pillarPositions = new HashSet<Vector2Int>();

        foreach (DungeonRoom room in generator.rooms)
        {
            // Calculamos las 4 esquinas exteriores exactas de la habitación
            Vector2Int[] corners = new Vector2Int[]
            {
                new Vector2Int(room.bounds.x - 1, room.bounds.y - 1), // Esquina inferior izquierda
                new Vector2Int(room.bounds.x + room.bounds.width, room.bounds.y - 1), // Esquina inferior derecha
                new Vector2Int(room.bounds.x - 1, room.bounds.y + room.bounds.height), // Esquina superior izquierda
                new Vector2Int(room.bounds.x + room.bounds.width, room.bounds.y + room.bounds.height) // Esquina superior derecha
            };

            foreach (Vector2Int corner in corners)
            {
                // Solo ponemos el pilar si en esa esquina NO hay un suelo/pasillo y si no hemos puesto ya otro pilar
                if (!floorPositions.Contains(corner) && !pillarPositions.Contains(corner))
                {
                    // Altura Y = 0. Asegúrate de que el modelo de tu pilar tiene el pivote en la base
                    Vector3 pos = new Vector3(corner.x, 0f, corner.y);
                    Instantiate(pillarPrefab, pos, Quaternion.identity, transform);
                    pillarPositions.Add(corner);
                }
            }
        }
    }
}