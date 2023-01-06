using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding; // Path Finding
using UnityEngine.Tilemaps;
using System;

public enum RoomType { R1X1, R2X1, R2X2, RL }

[System.Serializable]
public class RoomData
{
    public string roomName;
    public RoomType roomType;
    public bool isMimicRoom;
    public bool locksOnEnter;
    public bool eastDoorIsBlocked;
    public bool westDoorIsBlocked;
    public bool southDoorIsBlocked;
    public bool northDoorIsBlocked;
    public bool layoutCanBeRotated;
    public bool layoutCanBeMirroredX;
    public bool layoutCanBeMirroredY;
    public bool hasCeiling;
    public bool isCompleted;
    public bool isVisibleOnMap;
    public bool isVisitedOnMap;
    public bool isTreasureRoom;
    public bool isSecretRoom;
    public bool isBossRoom;
    public bool isSpecialRoom;
    public bool isMerchantRoom;
    public Vector3[] extraSpace;
    // public BiomeInformation biomeInformation;

    public BoundsInt outsideFloorTileBounds;
    public BoundsInt outsideEnvironmentTileBounds;
    public BoundsInt outsideCeilingTileBounds;

    public List<TileData> floorTileDatas = new List<TileData>();
    public List<TileData> environmentTileDatas = new List<TileData>();
    public List<TileData> ceilingTileDatas = new List<TileData>();
}

[System.Serializable]
public class TileData
{
    public string tileName;
    public List<Vector3Int> positions = new List<Vector3Int>();
}

public class Room : MonoBehaviour
{
    [Tooltip("Use to specify a room with only mimics in it\n")]
    [SerializeField] private bool isMimicRoom = false;
    [SerializeField] private RoomEnemyManager myRoomEnemyManager;

    // Path Finding 
    private AstarPath myAstarPath;

    // CONFIGURATION
    [SerializeField] private int xDimension = 15;
    [SerializeField] private int zDimension = 15;
    [SerializeField] private Tilemap outsideTilemapFloor;
    [SerializeField] private Tilemap outsideTilemapEnvironment;
    [SerializeField] private Tilemap outsideTilemapCeiling;
    [SerializeField] private Tilemap tilemapFloor;
    [SerializeField] private Tilemap tilemapEnvironment;
    [SerializeField] private Tilemap tilemapCeiling;
    [SerializeField] private int yHeight = 1;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private bool locksOnEnter = true;

    [SerializeField] private bool eastDoorIsBlocked = false;
    [SerializeField] private bool westDoorIsBlocked = false;
    [SerializeField] private bool southDoorIsBlocked = false;
    [SerializeField] private bool northDoorIsBlocked = false;

    [SerializeField] private bool layoutCanBeRotated = false;
    [SerializeField] private bool layoutCanBeMirroredX = false;
    [SerializeField] private bool layoutCanBeMirroredY = false;

    [SerializeField] private RoomType roomType;
    [SerializeField] private bool hasCeiling = true;
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private bool isVisibleOnMap = false;
    [SerializeField] private bool isVisitedOnMap = false;
    [SerializeField] private bool isTreasureRoom = false;
    [SerializeField] private bool isSecretRoom = false;
    [SerializeField] private bool isBossRoom = false;
    [SerializeField] private bool isSpecialRoom = false;
    [SerializeField] private bool isMerchantRoom = false;
    [SerializeField] private bool isShootingRangeRoom = false;
    [SerializeField] private Vector3[] extraSpace;

    private bool placedOnMap = false;

    [SerializeField] private Transform roomInside;
    [SerializeField] private PositionRotationSO lastSpawnPositionRotation;

    [SerializeField] private UnityEvent mapHasChanged;

    private List<Room> myAdjacentRooms = new List<Room>();

    // PREFABS
    [SerializeField] private BiomeInformation biomeInformation;

    private Block floorPrefab;
    private Block wallPrefab;
    private Block ceilingPrefab;
    private DoorBlock doorPrefab;

    private List<DoorBlock> myRoomDoors = new List<DoorBlock>();

    private NextLevelBlock nextLevelBlock;

    public bool IsMimicRoom { get => isMimicRoom; set => isMimicRoom = value; }

    public bool EastDoorIsBlocked { get => eastDoorIsBlocked; set => eastDoorIsBlocked = value; }
    public bool WestDoorIsBlocked { get => westDoorIsBlocked; set => westDoorIsBlocked = value; }
    public bool SouthDoorIsBlocked { get => southDoorIsBlocked; set => southDoorIsBlocked = value; }
    public bool NorthDoorIsBlocked { get => northDoorIsBlocked; set => northDoorIsBlocked = value; }

    public bool LayoutCanBeRotated { get => layoutCanBeRotated; set => layoutCanBeRotated = value; }
    public bool LayoutCanBeMirroredX { get => layoutCanBeMirroredX; set => layoutCanBeMirroredX = value; }
    public bool LayoutCanBeMirroredY { get => layoutCanBeMirroredY; set => layoutCanBeMirroredY = value; }

    public int XDimension { get => xDimension; }
    public int ZDimension { get => zDimension; }

    public RoomType RoomType { get => roomType; set => roomType = value; }
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public bool IsVisibleOnMap { get => isVisibleOnMap; set => isVisibleOnMap = value; }
    public bool IsVisitedOnMap { get => isVisitedOnMap; set => isVisitedOnMap = value; }
    public bool IsTreasureRoom { get => isTreasureRoom; set => isTreasureRoom = value; }
    public bool IsSecretRoom { get => isSecretRoom; set => isSecretRoom = value; }
    public bool IsBossRoom { get => isBossRoom; set => isBossRoom = value; }
    public bool IsSpecialRoom { get => isSpecialRoom; set => isSpecialRoom = value; }
    public bool IsMerchantRoom { get => isMerchantRoom; set => isMerchantRoom = value; }
    public bool IsShootingRangeRoom { get => isShootingRangeRoom; }
    public Vector3[] ExtraSpace { get => extraSpace; set => extraSpace = value; }

    public Transform RoomInside { get => roomInside; set => roomInside = value; }
    public AstarPath MyAstarPath { get => myAstarPath; set => myAstarPath = value; }
    
    public List<Room> MyAdjacentRooms { get => myAdjacentRooms; set => myAdjacentRooms = value; }
    public List<DoorBlock> MyRoomDoors { get => myRoomDoors; set => myRoomDoors = value; }

    public Tilemap OutsideTilemapFloor { get => outsideTilemapFloor; set => outsideTilemapFloor = value; }
    public Tilemap OutsideTilemapEnvironment { get => outsideTilemapEnvironment; set => outsideTilemapEnvironment = value; }
    public Tilemap OutsideTilemapCeiling { get => outsideTilemapCeiling; set => outsideTilemapCeiling = value; }

    public Tilemap TilemapFloor { get => tilemapFloor; set => tilemapFloor = value; }
    public Tilemap TilemapEnvironment { get => tilemapEnvironment; set => tilemapEnvironment = value; }
    public Tilemap TilemapCeiling { get => tilemapCeiling; set => tilemapCeiling = value; }

    public bool LocksOnEnter { get => locksOnEnter; set => locksOnEnter = value; }
    public bool HasCeiling { get => hasCeiling; set => hasCeiling = value; }
    public BiomeInformation BiomeInformation { get => biomeInformation; set => biomeInformation = value; }
    public bool PlacedOnMap { get => placedOnMap; set => placedOnMap = value; }

    // Start is called before the first frame update
    public void GenerateRoom()
    {
        if (GetComponentInChildren<NextLevelBlock>() != null)
        {
            nextLevelBlock = GetComponentInChildren<NextLevelBlock>();
            // nextLevelBlock.gameObject.SetActive(false);
        }

        floorPrefab = biomeInformation.floorPrefab;
        wallPrefab = biomeInformation.wallPrefab;
        ceilingPrefab = biomeInformation.ceilingPrefab;
        doorPrefab = biomeInformation.doorPrefab;

        var baseRoomDoors = GetComponentsInChildren<DoorBlock>();
        foreach (DoorBlock baseRoomDoor in baseRoomDoors)
        {
            myRoomDoors.Add(baseRoomDoor);
            baseRoomDoor.CloseDoor();
        }

        if (hasCeiling)
        {
            for (int i = -xDimension / 2; i <= xDimension / 2; i++)
            {
                for (int j = -zDimension / 2; j <= zDimension / 2; j++)
                {
                    var newCeiling = Instantiate(ceilingPrefab, transform);
                    newCeiling.transform.localPosition = new Vector3(i, yHeight, j);
                }
            }
            foreach (Vector3 extraSpace in extraSpace)
            {
                for (int i = ((int)extraSpace.x * xDimension) - xDimension / 2; i <= ((int)extraSpace.x * xDimension) + xDimension / 2; i++)
                {
                    for (int j = ((int)extraSpace.z * xDimension) - zDimension / 2; j <= ((int)extraSpace.z * xDimension) + zDimension / 2; j++)
                    {
                        var newCeiling = Instantiate(ceilingPrefab, transform);
                        newCeiling.transform.localPosition = new Vector3(i, yHeight, j);
                    }
                }
            }
        }
    }

    public void CloseOffSecretWalls()
    {
        myRoomDoors.ForEach(door => door.CloseOffSecretWall());
    }

    public void SetAllDoorsRoomTrigger(bool setAs)
    {
        myRoomDoors.ForEach(door => door.RoomTrigger.enabled = setAs);
    }

    public void OpenAllDoors()
    {
        myRoomDoors.ForEach(door => door.OpenDoor());
    }

    public void CloseAllDoors()
    {
        myRoomDoors.ForEach(door => door.CloseDoor());
    }

    public void LockAllDoors()
    {
        myRoomDoors.ForEach(door => door.LockDoor());
    }

    public void UnlockAllDoors()
    {
        myRoomDoors.ForEach(door => door.UnlockDoor());
    }

    public void UpdateMap()
    {
        foreach (Room room in MyAdjacentRooms)
        {
            if (!room.isSecretRoom)
            {
                room.isVisibleOnMap = true;
            }
            room.gameObject.SetActive(true);
        }

        isVisitedOnMap = true;
        mapHasChanged.Invoke();
    }

    public void InitiateRoom()
    {
        UpdateMap();

        if (locksOnEnter && !isCompleted)
        {
            if (isBossRoom)
            {
                MusicManager.instance.SwitchToBoss();
            }
            else
            {
                MusicManager.instance.SwitchToInCombat();
            }
            CloseAllDoors();
            LockAllDoors();
            myRoomEnemyManager.SetSpawnersPrefab();
            SetGridGraphsToRoom();
            myAstarPath.Scan();
        }
        else
        {
            IsCompleted = true;
            OpenAllDoors();
        }
    }

    public void CloseAndLock_AdjacentCompletedRoomDoors()
    {
        foreach (Room room in myAdjacentRooms)
        {
            if (room.IsCompleted && room.LocksOnEnter)
            {
                room.CloseAllDoors();
                room.LockAllDoors();
            }
        }
    }

    /// <summary>
    /// Set ALL available GRIDGRAPHS to desired settings
    /// </summary>
    public void SetGridGraphsToRoom()
    {
        for (int index = 0; index < myAstarPath.data.graphs.Length; index++)
        {
            GridGraph gg = myAstarPath.data.graphs[index] as GridGraph;
            gg.center = gameObject.transform.localPosition;
            gg.center.y = -1.0f; // Must be at ground level
            gg.SetDimensions(XDimension * 4 + 3, ZDimension * 4 + 3, myAstarPath.data.gridGraph.nodeSize);
        }
    }

    public void FinishRoom()
    {
        MusicManager.instance.SwitchToOutOfCombat();
        IsCompleted = true;
        if (nextLevelBlock != null)
        {
            nextLevelBlock.ShowButton();
        }
        UnlockAllDoors();
        OpenAllDoors();
        foreach (Room room in myAdjacentRooms)
        {
            if (room.IsCompleted)
            {
                room.UnlockAllDoors();

                foreach (DoorBlock doorBlock in myRoomDoors)
                {
                    SetIsInteractable(doorBlock, false);
                }

                room.OpenAllDoors();
            }
        }
    }

    public void UpdateAdjacentRooms()
    {
        Debug.Log("Adjacent Rooms are being Updated");
        foreach (Room room in mapLayoutInformation.Rooms)
        {
            if (room != this && !room.IsSecretRoom && !myAdjacentRooms.Contains(room))
            {
                room.gameObject.SetActive(false);
            }
        }
        myAdjacentRooms.ForEach(room => room.gameObject.SetActive(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LivingEntityContext>() != null)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<LivingEntityContext>().TakeDamage(5, Vector3.zero);
                RespawnPlayer(other.gameObject);
            }
            else
            {
                other.GetComponent<LivingEntityContext>().TakeDamage(float.MaxValue, Vector3.zero);
            }
        }
        else
        {
            Destroy(other);
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        // Debug.Log("Applying Spawn Position " + lastSpawnPositionRotation.Transform.position);
        player.GetComponent<PlayerContext>().Respawn();
    }

    private void SetIsInteractable(DoorBlock doorBlock, bool setTo)
    {
        // Get
        Interactable myDoorInteractable;

        // Set
        myDoorInteractable = doorBlock.GetComponentInChildren<Interactable>();
        if (myDoorInteractable != null) myDoorInteractable.SetIsInteractable(setTo);
    }

    public RoomData SaveRoomData()
    {
        RoomData roomData = new()
        {
            roomName = gameObject.name,
            roomType = roomType,
            isMimicRoom = IsMimicRoom,
            locksOnEnter = LocksOnEnter,
            eastDoorIsBlocked = EastDoorIsBlocked,
            westDoorIsBlocked = WestDoorIsBlocked,
            southDoorIsBlocked = SouthDoorIsBlocked,
            northDoorIsBlocked = NorthDoorIsBlocked,
            layoutCanBeRotated = LayoutCanBeRotated,
            layoutCanBeMirroredX = LayoutCanBeMirroredX,
            layoutCanBeMirroredY = LayoutCanBeMirroredY,
            hasCeiling = HasCeiling,
            isCompleted = IsCompleted,
            isVisibleOnMap = IsVisibleOnMap,
            isVisitedOnMap = IsVisitedOnMap,
            isTreasureRoom = IsTreasureRoom,
            isSecretRoom = IsSecretRoom,
            isBossRoom = IsBossRoom,
            isSpecialRoom = IsSpecialRoom,
            isMerchantRoom = IsMerchantRoom,
            extraSpace = extraSpace,

            outsideFloorTileBounds = outsideTilemapFloor.cellBounds,
            outsideEnvironmentTileBounds = outsideTilemapEnvironment.cellBounds,
            outsideCeilingTileBounds = outsideTilemapCeiling.cellBounds,
            // biomeInformation = BiomeInformation,
        };

        ProcessTilemapRoomData(tilemapFloor, roomData.floorTileDatas, roomData.outsideFloorTileBounds);
        ProcessTilemapRoomData(tilemapEnvironment, roomData.environmentTileDatas, roomData.outsideEnvironmentTileBounds);
        ProcessTilemapRoomData(tilemapCeiling, roomData.ceilingTileDatas, roomData.outsideCeilingTileBounds);

        return roomData;
    }

    private void ProcessTilemapRoomData(Tilemap tilemap, List<TileData> tileDatas, BoundsInt bounds)
    {
        for (int i = tilemap.origin.x; i < (tilemap.origin.x + bounds.size.x - 1); i++)
        {
            for (int j = tilemap.origin.y; j < (tilemap.origin.y + bounds.size.y - 1); j++)
            {
                var position = new Vector3Int(i, j, 0);
                TileBase newTile = tilemap.GetTile(position);
                bool check = false;
                if (newTile != null && newTile.name != "PaletteIcons_0" && newTile.name != "")
                {
                    foreach (TileData tileData in tileDatas)
                    {
                        if (tileData.tileName == newTile.name)
                        {
                            tileData.positions.Add(position);
                            check = true;
                            break;
                        }
                    }
                    if (!check)
                    {
                        TileData newTileData = new TileData();
                        newTileData.tileName = newTile.name;
                        newTileData.positions.Add(position);
                        tileDatas.Add(newTileData);
                    }
                }
            }
        }
    }

    public void LoadRoomData(RoomData roomData, Dictionary<string, TileBase> guidToTileBase)
    {
        gameObject.name = roomData.roomName;
        roomType = roomData.roomType;
        isMimicRoom = roomData.isMimicRoom;
        locksOnEnter = roomData.locksOnEnter;
        eastDoorIsBlocked = roomData.eastDoorIsBlocked;
        westDoorIsBlocked = roomData.westDoorIsBlocked;
        southDoorIsBlocked = roomData.southDoorIsBlocked;
        northDoorIsBlocked = roomData.northDoorIsBlocked;
        layoutCanBeRotated = roomData.layoutCanBeRotated;
        layoutCanBeMirroredX = roomData.layoutCanBeMirroredX;
        layoutCanBeMirroredY = roomData.layoutCanBeMirroredY;
        hasCeiling = roomData.hasCeiling;
        isCompleted = roomData.isCompleted;
        isVisibleOnMap = roomData.isVisibleOnMap;
        isVisitedOnMap = roomData.isVisitedOnMap;
        isTreasureRoom = roomData.isTreasureRoom;
        isSecretRoom = roomData.isSecretRoom;
        isBossRoom = roomData.isBossRoom;
        isSpecialRoom = roomData.isSpecialRoom;
        isMerchantRoom = roomData.isMerchantRoom;
        extraSpace = roomData.extraSpace;
        // biomeInformation = roomData.biomeInformation;

        DrawTilemap(tilemapFloor, roomData.floorTileDatas, guidToTileBase);
        DrawTilemap(tilemapEnvironment, roomData.environmentTileDatas, guidToTileBase);
        DrawTilemap(tilemapCeiling, roomData.ceilingTileDatas, guidToTileBase);
    }

    private void DrawTilemap(Tilemap tilemap, List<TileData> tileDatas, Dictionary<string, TileBase> guidToTileBase)
    {
        foreach (TileData tileData in tileDatas)
        {
            foreach (Vector3Int position in tileData.positions)
            {
                tilemap.SetTile(position, guidToTileBase[tileData.tileName]);
            }
        }
    }
}
