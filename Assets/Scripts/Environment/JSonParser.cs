using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JSonParser : MonoBehaviour
{
    [SerializeField] private bool save = true;
    [SerializeField] private Room roomToSave;
    [SerializeField] private Room roomToLoadOn;

    

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    private void SaveRoom(RoomData roomData, bool debug = false)
    {
        if (debug) Debug.Log("Saving...");

        // int saveNumber = 1;
        
    }

    //private void LoadRoom(Room loadRoom, bool debug = false)
    //{
    //    DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);

    //    string saveString = File.ReadAllText(SAVE_FOLDER + "Room_Save" + "." + SAVE_EXTENSION);

    //    if (debug) Debug.Log("Loaded: " + saveString);

    //    RoomData roomData = JsonUtility.FromJson<RoomData>(saveString);
    //    PrepareLoadData(roomData, roomToLoadOn);

    //    if (debug) Debug.Log("No saved data!");
    //}
}
