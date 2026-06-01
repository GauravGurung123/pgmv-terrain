using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonConfig config; // Arrastraremos el archivo de configuración aquí
    public List<DungeonRoom> rooms = new List<DungeonRoom>();

    void Awake()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        // 1. Inicializar la semilla para el generador de números pseudoaleatorios [cite: 83]
        if (config.useRandomSeed)
        {
            config.seed = System.Environment.TickCount;
        }
        Random.InitState(config.seed);
        rooms.Clear();

        // 2. Bucle para colocar habitaciones en posiciones aleatorias
        int maxAttempts = 100; // Evita bucles infinitos si el mapa es muy pequeño
        int attempts = 0;

        // Intentamos generar salas hasta llegar al número deseado
        while (rooms.Count < config.numberOfRooms && attempts < maxAttempts)
        {
            // REGLA: El lado de la sala más pequeña debe ser al menos 4 veces el ancho del túnel 
            int minSize = Mathf.Max(config.minRoomSize, config.corridorWidth * 4); 
            
            // REGLA: Las salas deben ser rectangulares [cite: 71]
            int roomWidth = Random.Range(minSize, config.maxRoomSize + 1);
            int roomHeight = Random.Range(minSize, config.maxRoomSize + 1);

            int xPos = Random.Range(0, config.dungeonWidth - roomWidth);
            int yPos = Random.Range(0, config.dungeonHeight - roomHeight);

            DungeonRoom newRoom = new DungeonRoom(new Vector2Int(xPos, yPos), new Vector2Int(roomWidth, roomHeight));

            // Si la sala no choca con otra, la añadimos a la lista
            if (!DoesRoomIntersect(newRoom))
            {
                rooms.Add(newRoom);
            }
            attempts++;
        }

        // 3. Asignar los tipos de habitación requeridos [cite: 75]
        AssignRoomTypes();
    }

    bool DoesRoomIntersect(DungeonRoom room)
    {
        // Comprobamos si la nueva sala choca con alguna de las que ya hemos creado
        foreach (DungeonRoom existingRoom in rooms)
        {
            // Añadimos un margen de 1 casilla (agrandando temporalmente el RectInt) para que no compartan paredes
            if (room.bounds.Overlaps(new RectInt(existingRoom.bounds.x - 1, existingRoom.bounds.y - 1, existingRoom.bounds.width + 2, existingRoom.bounds.height + 2)))
            {
                return true;
            }
        }
        return false;
    }

    void AssignRoomTypes()
    {
        // El enunciado exige al menos 5 salas [cite: 77]
        if (rooms.Count < 5) 
        {
            Debug.LogWarning("No se generaron suficientes salas. Revisa el tamaño de la mazmorra o reduce el tamaño de las salas.");
            return; 
        }

        // La primera sala que generamos será la entrada
        rooms[0].type = RoomType.Entrance;

        // REGLA: La sala del tesoro debe estar lo más lejos posible de la entrada 
        float maxDistance = 0f;
        DungeonRoom treasureRoom = rooms[1];
        
        for (int i = 1; i < rooms.Count; i++)
        {
            float dist = Vector2Int.Distance(rooms[0].Center, rooms[i].Center);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                treasureRoom = rooms[i];
            }
        }
        treasureRoom.type = RoomType.Treasure;

        // REGLA: Al menos dos tipos de salas, usando las probabilidades [cite: 75, 83]
        foreach (DungeonRoom room in rooms)
        {
            if (room.type == RoomType.Normal)
            {
                float randomVal = Random.value; // Número entre 0.0 y 1.0
                if (randomVal <= config.roomTypeA_Prob)
                {
                    room.type = RoomType.TypeA;
                }
                else
                {
                    room.type = RoomType.TypeB;
                }
            }
        }
    }
}