using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FirstSpawnData
{
    public int mapIdx;
    public Entity enemy;
}
[CreateAssetMenu(menuName = "SO/EnemyGroup")]
public class EnemyGroupSO : ScriptableObject
{
    public List<FirstSpawnData> firstSpawns = new(); 
    public List<Entity> enemies = new List<Entity>();
}
