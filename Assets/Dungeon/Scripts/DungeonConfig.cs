using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDungeonConfig", menuName = "Dungeon/Config")]
public class DungeonConfig : ScriptableObject
{
    [Header("Dungeon Size")]
    public int dungeonWidth = 50;
    public int dungeonHeight = 50;
    public int corridorWidth = 2;

    [Header("Room Parameters")]
    public bool useRandomSeed = true;
    public int seed = 12345;
    public int minRoomSize = 8; // Regla: debe ser al menos 4 veces el corridorWidth
    public int maxRoomSize = 16;
    public int numberOfRooms = 6;

    [Header("Room Probabilities")]
    [Range(0f, 1f)] public float roomTypeA_Prob = 0.5f;
    [Range(0f, 1f)] public float roomTypeB_Prob = 0.5f;
}