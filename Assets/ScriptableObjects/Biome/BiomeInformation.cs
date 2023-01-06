using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BiomeInformation : ScriptableObject
{
    public Block floorPrefab;
    public Block wallPrefab;
    public Block ceilingPrefab;
    public DoorBlock doorPrefab;
}
