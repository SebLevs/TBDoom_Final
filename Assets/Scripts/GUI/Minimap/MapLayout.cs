using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class MapLayout : MonoBehaviour
{
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private int minRooms = 5;
    [SerializeField] private int maxRooms = 12;
    [SerializeField] private int roomSize = 15;
    [SerializeField] private int secretRooms = 1;
    [SerializeField] private int bossRooms = 1;
    [SerializeField] private int treasureRooms = 1;
    [SerializeField] private int specialRooms = 2;
    [SerializeField] private int maxXGridSize = 4;
    [SerializeField] private int maxYGridSize = 3;
    [SerializeField] private RoomsListSO startingRoomsList;
    [SerializeField] private RoomsListSO normalRoomsList;
    [SerializeField] private RoomsListSO bossRoomsList;
    [SerializeField] private RoomsListSO secretRoomsList;
    [SerializeField] private RoomsListSO specialRoomsList;
    [SerializeField] private RoomsListSO treasureRoomsList;
    [SerializeField] private EmptyRoomList emptyRoomList;

    private float restartMapGenerationTime = 5f;
    private float restartMapGenerationTimer = 0;
    private RandomSeed roomGenerationRandom;

    [SerializeField] private TextAsset myJSON;

    private static string SAVE_FOLDER;
    private const string SAVE_EXTENSION = "json";
    private RoomDataList roomDataList;
    private Dictionary<string, TileBase> guidToTileBase = new Dictionary<string, TileBase>();

    // Start is called before the first frame update
    public void Initialize()
    {
        Debug.Log("Initializing MapLayout");
        roomGenerationRandom = RandomManager.Instance.RoomGenerationRandom;
        // roomGenerationRandom.UpdateSeed();
        LoadRoomList();
        // roomsList.UpdateRoomList();
        emptyRoomList.UpdateEmptyRooms();
        StartCoroutine(InitializeMapLayout());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.instance.levelIsReady)
        {
            if (restartMapGenerationTimer > 0)
            {
                // Debug.Log("AML");
                restartMapGenerationTimer -= Time.deltaTime;
            }
            else
            {
                // Debug.Log("AAML");
                //StopCoroutine(InitializeMapLayout());
                //StartCoroutine(InitializeMapLayout());
            }
        }
    }

    private void LoadRoomList()
    {
        // SAVE_FOLDER = Application.dataPath + "/Saves/";
        // string saveString = File.ReadAllText(SAVE_FOLDER + "Room_List" + "." + SAVE_EXTENSION);
        TileBase[] tileBases = Resources.LoadAll<TileBase>("");
        foreach (TileBase tileBase in tileBases)
        {
            if (!guidToTileBase.ContainsKey(tileBase.name))
            {
                guidToTileBase.Add(tileBase.name, tileBase);
            }
            else
            {
                Debug.LogError("There's already a tile like that you dingus");
            }
        }
        roomDataList = JsonUtility.FromJson<RoomDataList>(myJSON.text);
    }

    private IEnumerator InitializeMapLayout()
    {
        restartMapGenerationTimer = restartMapGenerationTime;
        GenerateMapLayout();
        AddBigRoomToLayout();
        PlaceSpecialRooms();
        GetNearbyRooms();
        InitializeStartingRoom();
        CloseOffWalls();
        mapLayoutInformation.Rooms[0].OpenAllDoors();
        mapLayoutInformation.Rooms[0].IsCompleted = true;
        yield return new WaitForSeconds(1f);
        GameManager.instance.levelIsReady = true;
        FindObjectOfType<Minimap>().Initialize();
    }

    public void GenerateMapLayout()
    {
        mapLayoutInformation.RoomPositions.Clear();
        mapLayoutInformation.Rooms.Clear();

        mapLayoutInformation.RoomPositions.Add(new Vector3Int(0, 0, 0));
        var numberOfRooms = roomGenerationRandom.Random.Next(minRooms, maxRooms + 1);
        Debug.Log(numberOfRooms);

        Vector3Int startPoint;
        Vector3Int offset;
        Vector3Int endPoint;
        for (int i = 1; i < numberOfRooms - secretRooms - bossRooms - treasureRooms - specialRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[roomGenerationRandom.Random.Next(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3Int(roomGenerationRandom.Random.Next(-1, 2), 0, roomGenerationRandom.Random.Next(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) ||
                     Mathf.Abs(endPoint.x) > maxXGridSize ||
                     Mathf.Abs(endPoint.z) > maxYGridSize ||
                     mapLayoutInformation.RoomPositions.Contains(endPoint) ||
                     CheckMaxRoomsAround(endPoint, 1));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        PlaceNormalRooms();
        for (int i = 0; i < secretRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[roomGenerationRandom.Random.Next(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3Int(roomGenerationRandom.Random.Next(-1, 2), 0, roomGenerationRandom.Random.Next(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) ||
                     Mathf.Abs(endPoint.x) > maxXGridSize ||
                     Mathf.Abs(endPoint.z) > maxYGridSize ||
                     mapLayoutInformation.RoomPositions.Contains(endPoint) ||
                     CheckMinRoomsAround(endPoint, 2));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        for (int i = 0; i < bossRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[roomGenerationRandom.Random.Next(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3Int(roomGenerationRandom.Random.Next(-1, 2), 0, roomGenerationRandom.Random.Next(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) ||
                     Mathf.Abs(endPoint.x) > maxXGridSize ||
                     Mathf.Abs(endPoint.z) > maxYGridSize ||
                     mapLayoutInformation.RoomPositions.Contains(endPoint) ||
                     CheckMaxRoomsAround(endPoint, 1) ||
                     IsTouchingLastXRooms(endPoint, secretRooms + bossRooms));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        for(int i = 0; i < treasureRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[roomGenerationRandom.Random.Next(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3Int(roomGenerationRandom.Random.Next(-1, 2), 0, roomGenerationRandom.Random.Next(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) ||
                     Mathf.Abs(endPoint.x) > maxXGridSize ||
                     Mathf.Abs(endPoint.z) > maxYGridSize ||
                     mapLayoutInformation.RoomPositions.Contains(endPoint) ||
                     CheckMaxRoomsAround(endPoint, 1) ||
                     IsTouchingLastXRooms(endPoint, secretRooms + bossRooms + treasureRooms));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        for (int i = 0; i < specialRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[roomGenerationRandom.Random.Next(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3Int(roomGenerationRandom.Random.Next(-1, 2), 0, roomGenerationRandom.Random.Next(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) ||
                     Mathf.Abs(endPoint.x) > maxXGridSize ||
                     Mathf.Abs(endPoint.z) > maxYGridSize ||
                     mapLayoutInformation.RoomPositions.Contains(endPoint) ||
                     CheckMaxRoomsAround(endPoint, 1) ||
                     IsTouchingLastXRooms(endPoint, secretRooms + bossRooms + treasureRooms + specialRooms));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
    }

    bool IsTouchingLastXRooms(Vector3Int endPoint, int limit)
    {
        for (int i = mapLayoutInformation.RoomPositions.Count - 1; i > mapLayoutInformation.RoomPositions.Count - 1 - limit; i--)
        {
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3Int(1, 0, 0))
            {
                return true;
            };
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3Int(-1, 0, 0))
            {
                return true;
            };
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3Int(0, 0, 1))
            {
                return true;
            };
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3Int(0, 0, -1))
            {
                return true;
            };
        }
        return false;
    }

    bool CheckMaxRoomsAround(Vector3Int endPoint, int countLimit)
    {
        var count = 0;
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(1, 0, 0)))
        {
            count++;
        };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(-1, 0, 0)))
        {
            count++;
        };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(0, 0, 1)))
        {
            count++;
        };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(0, 0, -1)))
        {
            count++;
        };
        
        if (count > countLimit)
        {
            return true;
        }
        return false;
    }

    bool CheckMinRoomsAround(Vector3Int endPoint, int countLimit)
    {
        var count = 0;
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(1, 0, 0)))
        { 
            count++;
        };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(-1, 0, 0)))
        {
            count++;
        };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(0, 0, 1)))
        {
            count++;
        };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3Int(0, 0, -1)))
        {
            count++;
        };

        if (count < countLimit)
        { 
            return true;
        }
        return false;
    }

    public void PlaceNormalRooms()
    {
        AstarPath myAstarPathRef = AIManager.instance.AstarPath;

        // Place Starting Room
        var newStartingRoom = RotateAndPlaceRoom(roomDataList.startingRoom, mapLayoutInformation.RoomPositions[0]);
        // Set AStar reference
        newStartingRoom.MyAstarPath = myAstarPathRef;

        // Place Normal Rooms
        var start = 1;
        var end = mapLayoutInformation.RoomPositions.Count;
        for (int i = start; i < end; i++)
        {
            var newNormalRoom = RotateAndPlaceRoom(roomDataList.normalRoom, mapLayoutInformation.RoomPositions[i]);
            // Set AStar reference
            newNormalRoom.MyAstarPath = myAstarPathRef;
        }
    }

    private void PlaceSpecialRooms()
    {
        AstarPath myAstarPathRef = AIManager.instance.AstarPath;

        // Place SecretRooms
        var start = mapLayoutInformation.RoomPositions.Count - secretRooms - bossRooms - treasureRooms - specialRooms;
        var end = mapLayoutInformation.RoomPositions.Count - bossRooms - treasureRooms - specialRooms;
        for (int i = start; i < end; i++)
        {
            var newBossRoom = RotateAndPlaceRoom(roomDataList.secretRoom, mapLayoutInformation.RoomPositions[i]);
            newBossRoom.IsSecretRoom = true;
            newBossRoom.MyAstarPath = myAstarPathRef;
        }
        // Place BossRooms
        start = mapLayoutInformation.RoomPositions.Count - bossRooms - treasureRooms - specialRooms;
        end = mapLayoutInformation.RoomPositions.Count - treasureRooms - specialRooms;
        for (int i = start; i < end; i++)
        {
            var newBossRoom = RotateAndPlaceRoom(roomDataList.bossRoom, mapLayoutInformation.RoomPositions[i]);
            newBossRoom.IsBossRoom = true;
            newBossRoom.MyAstarPath = myAstarPathRef;
        }
        // Place TreasureRooms
        start = mapLayoutInformation.RoomPositions.Count - treasureRooms - specialRooms;
        end = mapLayoutInformation.RoomPositions.Count - specialRooms;
        for (int i = start; i < end; i++)
        {
            var newTreasureRoom = RotateAndPlaceRoom(roomDataList.treasureRoom, mapLayoutInformation.RoomPositions[i]);
            newTreasureRoom.IsTreasureRoom = true;
            newTreasureRoom.MyAstarPath = myAstarPathRef;
        }
        // Place SpecialRooms
        start = mapLayoutInformation.RoomPositions.Count - specialRooms;
        end = mapLayoutInformation.RoomPositions.Count;
        for (int i = start; i < end; i++)
        {
            var newSpecialRoom = RotateAndPlaceRoom(roomDataList.specialRoom, mapLayoutInformation.RoomPositions[i]);
            newSpecialRoom.IsSpecialRoom = true;
            newSpecialRoom.MyAstarPath = myAstarPathRef;
        }
    }

    // private Room RotateAndPlaceRoom(RoomsListSO roomsList, Vector3 roomPosition)
    private Room RotateAndPlaceRoom(List<RoomData> roomDataList, Vector3Int roomPosition)
    {
        var eastHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3Int(1, 0, 0));
        var westHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3Int(-1, 0, 0));
        var northHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3Int(0, 0, 1));
        var southHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3Int(0, 0, -1));

        RoomData newRoomData;
        float roomRotation;
        bool tempEastHasNoRoom;
        bool tempWestHasNoRoom;
        bool tempNorthHasNoRoom;
        bool tempSouthHasNoRoom;
        var check = false;

        do
        {
            newRoomData = null;
            newRoomData = roomDataList[roomGenerationRandom.Random.Next(0, roomDataList.Count)];
            roomRotation = 0;

            tempEastHasNoRoom = eastHasNoRoom;
            tempWestHasNoRoom = westHasNoRoom;
            tempNorthHasNoRoom = northHasNoRoom;
            tempSouthHasNoRoom = southHasNoRoom;

            if (newRoomData.roomType == RoomType.R1X1)
            {
                do
                {
                    if ((newRoomData.eastDoorIsBlocked == tempEastHasNoRoom || !newRoomData.eastDoorIsBlocked) &&
                     (newRoomData.westDoorIsBlocked == tempWestHasNoRoom || !newRoomData.westDoorIsBlocked) &&
                     (newRoomData.northDoorIsBlocked == tempNorthHasNoRoom || !newRoomData.northDoorIsBlocked) &&
                     (newRoomData.southDoorIsBlocked == tempSouthHasNoRoom || !newRoomData.southDoorIsBlocked))
                    {
                        break;
                    }
                    else
                    {
                        if (newRoomData.layoutCanBeRotated)
                        {
                            roomRotation += 90;
                            var temp = tempEastHasNoRoom;
                            tempEastHasNoRoom = tempSouthHasNoRoom;
                            tempSouthHasNoRoom = tempWestHasNoRoom;
                            tempWestHasNoRoom = tempNorthHasNoRoom;
                            tempNorthHasNoRoom = temp;
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (roomRotation < 360);
            }
            else
            {
                do
                {
                    Debug.Log("Trying to place a bigger room at position " + roomPosition);
                    check = false;
                    foreach (Vector3 extraSpace in newRoomData.extraSpace)
                    {
                        Debug.Log(roomPosition + (Quaternion.AngleAxis(roomRotation, Vector3.up) * extraSpace));
                        var rotatedExtraSpacePosition = Quaternion.AngleAxis(roomRotation, Vector3.up) * extraSpace;
                        var newExtraSpacePosition = roomPosition + new Vector3Int(Mathf.RoundToInt(rotatedExtraSpacePosition.x), 0, Mathf.RoundToInt(rotatedExtraSpacePosition.z));
                        
                        if (mapLayoutInformation.RoomPositions.Contains(newExtraSpacePosition) ||
                            Mathf.Abs(newExtraSpacePosition.x) > maxXGridSize ||
                            Mathf.Abs(newExtraSpacePosition.z) > maxYGridSize)
                        {
                            check = true;
                        }
                    }
                    if (!check)
                    {
                        break;
                    }
                    else
                    {
                        roomRotation += 90;
                    }
                } while (roomRotation < 360);
            } 
        } while ((newRoomData.roomType == RoomType.R1X1 &&
                 ((newRoomData.eastDoorIsBlocked != tempEastHasNoRoom && newRoomData.eastDoorIsBlocked) ||
                 (newRoomData.westDoorIsBlocked != tempWestHasNoRoom && newRoomData.westDoorIsBlocked) ||
                 (newRoomData.northDoorIsBlocked != tempNorthHasNoRoom && newRoomData.northDoorIsBlocked) ||
                 (newRoomData.southDoorIsBlocked != tempSouthHasNoRoom && newRoomData.southDoorIsBlocked))) ||
                 (newRoomData.roomType != RoomType.R1X1 &&
                 check));

        switch (newRoomData.roomType)
        {
            case RoomType.R1X1:
                var newRoom0 = emptyRoomList.Empty1x1Rooms[0];
                newRoom0.gameObject.SetActive(true);
                newRoom0.LoadRoomData(newRoomData, guidToTileBase);
                newRoom0.GenerateRoom();
                newRoom0.transform.parent = transform;
                newRoom0.transform.localPosition = roomPosition * roomSize;
                newRoom0.transform.localScale = Vector3.one;
                emptyRoomList.Empty1x1Rooms.Remove(newRoom0);

                // newRoom.RoomInside.transform.rotation = Quaternion.Euler(newRoom.transform.rotation.eulerAngles + new Vector3(0, roomRotation, 0));
                newRoom0.RoomInside.transform.localRotation = Quaternion.Euler(new Vector3(0, roomRotation, 0));
                while (roomRotation > 0)
                {
                    var temp = tempNorthHasNoRoom;
                    tempNorthHasNoRoom = tempWestHasNoRoom;
                    tempWestHasNoRoom = tempSouthHasNoRoom;
                    tempSouthHasNoRoom = tempEastHasNoRoom;
                    tempEastHasNoRoom = temp;
                    roomRotation -= 90;
                }
                newRoom0.EastDoorIsBlocked = tempEastHasNoRoom;
                newRoom0.WestDoorIsBlocked = tempWestHasNoRoom;
                newRoom0.NorthDoorIsBlocked = tempNorthHasNoRoom;
                newRoom0.SouthDoorIsBlocked = tempSouthHasNoRoom;

                mapLayoutInformation.Rooms.Add(newRoom0);
                if (!newRoom0.IsSecretRoom)
                {
                    newRoom0.gameObject.SetActive(false);
                }
                return newRoom0;
            case RoomType.R2X1:
                var newRoom1 = emptyRoomList.Empty2x1Rooms[0];
                newRoom1.gameObject.SetActive(true);
                newRoom1.LoadRoomData(newRoomData, guidToTileBase);
                newRoom1.GenerateRoom();
                newRoom1.transform.parent = transform;
                newRoom1.transform.localPosition = roomPosition * roomSize;
                newRoom1.transform.localScale = Vector3.one;
                emptyRoomList.Empty2x1Rooms.Remove(newRoom1);
                newRoom1.transform.localRotation = Quaternion.Euler(new Vector3(0, roomRotation, 0));
                mapLayoutInformation.Rooms.Add(newRoom1);
                foreach (Vector3 extraSpace in newRoom1.ExtraSpace)
                {
                    var rotatedExtraSpacePosition = Quaternion.AngleAxis(roomRotation, Vector3.up) * extraSpace;
                    var newExtraSpacePosition = new Vector3Int(Mathf.RoundToInt(rotatedExtraSpacePosition.x), 0, Mathf.RoundToInt(rotatedExtraSpacePosition.z));
                    mapLayoutInformation.RoomPositions.Add(roomPosition + newExtraSpacePosition);
                    Debug.Log("Room position added for " + newRoom1.name);
                }
                newRoom1.gameObject.SetActive(false);
                return newRoom1;
            case RoomType.R2X2:
                var newRoom2 = emptyRoomList.Empty2x2Rooms[0];
                newRoom2.gameObject.SetActive(true);
                newRoom2.LoadRoomData(newRoomData, guidToTileBase);
                newRoom2.GenerateRoom();
                newRoom2.transform.parent = transform;
                newRoom2.transform.localPosition = roomPosition * roomSize;
                newRoom2.transform.localScale = Vector3.one;
                emptyRoomList.Empty2x2Rooms.Remove(newRoom2);
                newRoom2.transform.localRotation = Quaternion.Euler(new Vector3(0, roomRotation, 0));
                mapLayoutInformation.Rooms.Add(newRoom2);
                foreach (Vector3 extraSpace in newRoom2.ExtraSpace)
                {
                    var rotatedExtraSpacePosition = Quaternion.AngleAxis(roomRotation, Vector3.up) * extraSpace;
                    var newExtraSpacePosition = new Vector3Int(Mathf.RoundToInt(rotatedExtraSpacePosition.x), 0, Mathf.RoundToInt(rotatedExtraSpacePosition.z));
                    mapLayoutInformation.RoomPositions.Add(roomPosition + newExtraSpacePosition);
                    Debug.Log("Room position added for " + newRoom2.name);
                }
                newRoom2.gameObject.SetActive(false);
                return newRoom2;
            case RoomType.RL:
                var newRoom3 = emptyRoomList.EmptyLRooms[0];
                newRoom3.gameObject.SetActive(true);
                newRoom3.LoadRoomData(newRoomData, guidToTileBase);
                newRoom3.GenerateRoom();
                newRoom3.transform.parent = transform;
                newRoom3.transform.localPosition = roomPosition * roomSize;
                newRoom3.transform.localScale = Vector3.one;
                emptyRoomList.EmptyLRooms.Remove(newRoom3);
                newRoom3.transform.localRotation = Quaternion.Euler(new Vector3(0, roomRotation, 0));
                mapLayoutInformation.Rooms.Add(newRoom3);
                foreach (Vector3 extraSpace in newRoom3.ExtraSpace)
                {
                    var rotatedExtraSpacePosition = Quaternion.AngleAxis(roomRotation, Vector3.up) * extraSpace;
                    var newExtraSpacePosition = new Vector3Int(Mathf.RoundToInt(rotatedExtraSpacePosition.x), 0, Mathf.RoundToInt(rotatedExtraSpacePosition.z));
                    mapLayoutInformation.RoomPositions.Add(roomPosition + newExtraSpacePosition);
                    Debug.Log("Room position added for " + newRoom3.name);
                }
                newRoom3.gameObject.SetActive(false);
                return newRoom3;
            default:
                Debug.Log("Room EnemyType Wasn't found");
                return null;
        }
    }

    private void AddBigRoomToLayout()
    {
        var end = mapLayoutInformation.Rooms.Count;
        for (int i = 0; i < end; i++)
        {
            if (mapLayoutInformation.Rooms[i].RoomType != RoomType.R1X1)
            {
                for (int j = 0; j < mapLayoutInformation.Rooms[i].ExtraSpace.Length; j++)
                {
                    mapLayoutInformation.Rooms.Add(mapLayoutInformation.Rooms[i]);
                }
            }
        }
    }

    private void GetNearbyRooms()
    {
        for (int i = 0; i < mapLayoutInformation.RoomPositions.Count; i++)
        {
            var eastHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3Int(1, 0, 0));
            var westHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3Int(-1, 0, 0));
            var northHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3Int(0, 0, 1));
            var southHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3Int(0, 0, -1));

            if (!eastHasNoRoom)
            {
                Room eastRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3Int(1, 0, 0))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(eastRoom);
            }
            if (!westHasNoRoom)
            {
                Room westRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3Int(-1, 0, 0))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(westRoom);
            }
            if (!northHasNoRoom)
            {
                Room northRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3Int(0, 0, 1))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(northRoom);
            }
            if (!southHasNoRoom)
            {
                Room southRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3Int(0, 0, -1))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(southRoom);
            }
        }
    }

    private void InitializeStartingRoom()
    {
        mapLayoutInformation.Rooms[0].gameObject.SetActive(true);
        mapLayoutInformation.Rooms[0].IsVisitedOnMap = true;
        mapLayoutInformation.Rooms[0].OpenAllDoors();
        foreach (Room room in mapLayoutInformation.Rooms[0].MyAdjacentRooms)
        {
            if (!room.IsSecretRoom)
            {
                room.IsVisibleOnMap = true;
            }
            room.gameObject.SetActive(true);
        }
    }

    private void CloseOffWalls()
    {
        foreach (Room room in mapLayoutInformation.Rooms)
        {
            foreach (Room adjacentRoom in room.MyAdjacentRooms)
            {
                if (room != adjacentRoom)
                {
                    foreach (DoorBlock adjacentDoorBlock in adjacentRoom.MyRoomDoors)
                    {
                        if (!adjacentDoorBlock.IsConnected)
                        {
                            foreach (DoorBlock doorBlock in room.MyRoomDoors)
                            {
                                if (!doorBlock.IsConnected)
                                {
                                    if ((adjacentDoorBlock.transform.position - doorBlock.transform.position).magnitude < 5)
                                    {
                                        adjacentDoorBlock.IsConnected = true;
                                        doorBlock.IsConnected = true;
                                        adjacentDoorBlock.AdjacentDoor = doorBlock;
                                        doorBlock.AdjacentDoor = adjacentDoorBlock;
                                        if (adjacentRoom.IsSecretRoom)
                                        {
                                            doorBlock.IsOutsideSecret = true;
                                        }
                                        else if (room.IsSecretRoom)
                                        {
                                            adjacentDoorBlock.IsOutsideSecret = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } 
        }

        foreach (Room room in mapLayoutInformation.Rooms)
        {
            foreach (DoorBlock doorBlock in room.MyRoomDoors)
            {
                if (!doorBlock.IsConnected)
                {
                    doorBlock.CloseOffWall();
                    doorBlock.RemoveTorchs();
                    //room.CloseOffWalls(mapLayoutInformation.RoomPositions);
                    //if (room.IsSecretRoom)
                    //    room.CloseOffSecretWalls();
                }
                else if (doorBlock.IsOutsideSecret)
                {
                    doorBlock.RemoveTorchs();
                }
                else if (room.IsSecretRoom)
                {
                    doorBlock.CloseOffSecretWall();
                }
            }
        }
    }
}
