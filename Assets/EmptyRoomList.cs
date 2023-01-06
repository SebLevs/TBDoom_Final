using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoomList : MonoBehaviour
{
    [SerializeField] private Transform empty1x1RoomsParent;
    [SerializeField] private Transform empty2x1RoomsParent;
    [SerializeField] private Transform empty2x2RoomsParent;
    [SerializeField] private Transform emptyLRoomsParent;

    private List<Room> empty1x1Rooms = new List<Room>();
    private List<Room> empty2x1Rooms = new List<Room>();
    private List<Room> empty2x2Rooms = new List<Room>();
    private List<Room> emptyLRooms = new List<Room>();

    public List<Room> Empty1x1Rooms { get => empty1x1Rooms; set => empty1x1Rooms = value; }
    public List<Room> Empty2x1Rooms { get => empty2x1Rooms; set => empty2x1Rooms = value; }
    public List<Room> Empty2x2Rooms { get => empty2x2Rooms; set => empty2x2Rooms = value; }
    public List<Room> EmptyLRooms { get => emptyLRooms; set => emptyLRooms = value; }

    public void UpdateEmptyRooms()
    {
        var empty1x1RoomListTemp = empty1x1RoomsParent.GetComponentsInChildren<Room>();
        foreach (Room room in empty1x1RoomListTemp)
        {
            empty1x1Rooms.Add(room);
            room.gameObject.SetActive(false);
        }
        var empty2x1RoomListTemp = empty2x1RoomsParent.GetComponentsInChildren<Room>();
        foreach (Room room in empty2x1RoomListTemp)
        {
            empty2x1Rooms.Add(room);
            room.gameObject.SetActive(false);
        }
        var empty2x2RoomListTemp = empty2x2RoomsParent.GetComponentsInChildren<Room>();
        foreach (Room room in empty2x2RoomListTemp)
        {
            empty2x2Rooms.Add(room);
            room.gameObject.SetActive(false);
        }
        var emptyLRoomListTemp = emptyLRoomsParent.GetComponentsInChildren<Room>();
        foreach (Room room in emptyLRoomListTemp)
        {
            emptyLRooms.Add(room);
            room.gameObject.SetActive(false);
        }
    }
}
