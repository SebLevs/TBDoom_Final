using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RoomDataList
{
    public List<RoomData> bossRoom = new List<RoomData>();
    public List<RoomData> normalRoom = new List<RoomData>();
    public List<RoomData> secretRoom = new List<RoomData>();
    public List<RoomData> specialRoom = new List<RoomData>();
    public List<RoomData> startingRoom = new List<RoomData>();
    public List<RoomData> treasureRoom = new List<RoomData>();
}

public class RoomsList : MonoBehaviour
{
    [SerializeField] private Transform bossRoomParent;
    [SerializeField] private Transform normalRoomParent;
    [SerializeField] private Transform secretRoomParent;
    [SerializeField] private Transform specialRoomParent;
    [SerializeField] private Transform startingRoomParent;
    [SerializeField] private Transform treasureRoomParent;

    private static string SAVE_FOLDER;
    private const string SAVE_EXTENSION = "json";

    private List<Room> bossRoomList = new List<Room>();
    private List<Room> normalRoomList = new List<Room>();
    private List<Room> secretRoomList = new List<Room>();
    private List<Room> specialRoomList = new List<Room>();
    private List<Room> startingRoomList = new List<Room>();
    private List<Room> treasureRoomList = new List<Room>();

    public List<Room> BossRoomList { get => bossRoomList; set => bossRoomList = value; }
    public List<Room> NormalRoomList { get => normalRoomList; set => normalRoomList = value; }
    public List<Room> SecretRoomList { get => secretRoomList; set => secretRoomList = value; }
    public List<Room> SpecialRoomList { get => specialRoomList; set => specialRoomList = value; }
    public List<Room> StartingRoomList { get => startingRoomList; set => startingRoomList = value; }
    public List<Room> TreasureRoomList { get => treasureRoomList; set => treasureRoomList = value; }

    private void Awake()
    {
        SAVE_FOLDER = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        UpdateRoomList();
        SaveRoomList(PrepareRoomList());
    }

    public void UpdateRoomList()
    {
        foreach (Room room in bossRoomParent.GetComponentsInChildren<Room>())
        {
            bossRoomList.Add(room);
        }
        foreach (Room room in normalRoomParent.GetComponentsInChildren<Room>())
        {
            normalRoomList.Add(room);
        }
        foreach (Room room in secretRoomParent.GetComponentsInChildren<Room>())
        {
            secretRoomList.Add(room);
        }
        foreach (Room room in specialRoomParent.GetComponentsInChildren<Room>())
        {
            specialRoomList.Add(room);
        }
        foreach (Room room in startingRoomParent.GetComponentsInChildren<Room>())
        {
            startingRoomList.Add(room);
        }
        foreach (Room room in treasureRoomParent.GetComponentsInChildren<Room>())
        {
            treasureRoomList.Add(room);
        }
    }

    private RoomDataList PrepareRoomList()
    {
        RoomDataList roomList = new RoomDataList();
        foreach (Room room in bossRoomList)
        {
            roomList.bossRoom.Add(room.SaveRoomData());
            room.gameObject.SetActive(false);
        }
        foreach (Room room in normalRoomList)
        {
            roomList.normalRoom.Add(room.SaveRoomData());
            room.gameObject.SetActive(false);
        }
        foreach (Room room in secretRoomList)
        {
            roomList.secretRoom.Add(room.SaveRoomData());
            room.gameObject.SetActive(false);
        }
        foreach (Room room in specialRoomList)
        {
            roomList.specialRoom.Add(room.SaveRoomData());
            room.gameObject.SetActive(false);
        }
        foreach (Room room in startingRoomList)
        {
            roomList.startingRoom.Add(room.SaveRoomData());
            room.gameObject.SetActive(false);
        }
        foreach (Room room in treasureRoomList)
        {
            roomList.treasureRoom.Add(room.SaveRoomData());
            room.gameObject.SetActive(false);
        }
        return roomList;
    }

    private void SaveRoomList(RoomDataList roomList)
    {
        string json = JsonUtility.ToJson(roomList);

        if (json != null)
        {
            File.WriteAllText(SAVE_FOLDER + "Room_List3" + "." + SAVE_EXTENSION, json);
        }
    }
}
