using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/RoomsList", fileName = "RoomsListSO")]
public class RoomsListSO : ScriptableObject
{
    [SerializeField] private List<Room> rooms;

    public List<Room> Rooms { get => rooms; set => rooms = value; }
}
