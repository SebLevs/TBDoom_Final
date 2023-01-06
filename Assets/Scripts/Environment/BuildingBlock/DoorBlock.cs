using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlock : MonoBehaviour
{
    [SerializeField] private bool isDoor;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isSecret;
    [SerializeField] private bool isOutsideSecret;

    [SerializeField] private Block wallPrefab;
    [SerializeField] private GameObject door;
    [SerializeField] private BoxCollider doorCollider;
    private BoxCollider roomTrigger;
    private Room myRoom;
    private DoorBlock adjacentDoor;

    [SerializeField] private Block secretWall;

    private TorchBlock[] myTorchs;
    private bool isConnected = false;
    private Animator myAnimator;

    public bool IsDoor { get => isDoor; set => isDoor = value; }
    public bool IsOpen { get => isOpen; set => isOpen = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public bool IsSecret { get => isSecret; set => isSecret = value; }
    public bool IsConnected { get => isConnected; set => isConnected = value; }
    public bool IsOutsideSecret { get => isOutsideSecret; set => isOutsideSecret = value; }
    public BoxCollider RoomTrigger { get => roomTrigger; set => roomTrigger = value; }
    public DoorBlock AdjacentDoor { get => adjacentDoor; set => adjacentDoor = value; }

    private void Awake()
    {
        InitializeReferences();
    }

    private void OnEnable()
    {
        if (myAnimator == null)
        {
            InitializeReferences();
        }
        myAnimator.SetBool("ClosedDoor", !isOpen);
    }

    private void SetIsInteractable(bool setTo)
    {
        // Get
        Interactable myDoorInteractable;

        // Set
        myDoorInteractable = GetComponentInChildren<Interactable>();
        if (myDoorInteractable != null)
        {
            myDoorInteractable.SetIsInteractable(setTo);
        }
    }

    private void InitializeReferences()
    {
        myTorchs = GetComponentsInChildren<TorchBlock>();
        myAnimator = GetComponentInParent<Animator>();
        myRoom = GetComponentInParent<Room>();
        roomTrigger = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
        if (!isDoor)
        {
            Instantiate(wallPrefab, transform);
            door.SetActive(false);
        }
        else
        {
            roomTrigger.enabled = true;
        }
    }

    // INITIATE ROOM
    private void OnTriggerExit(Collider other)
    {
        if (myRoom == null)
        {
            InitializeReferences();
        }

        myRoom.UpdateAdjacentRooms();

        if (myRoom.IsCompleted) 
        { 
            return;
        }

        if (isOpen && !myRoom.IsShootingRangeRoom && other.CompareTag("Player"))
        {
            myRoom.InitiateRoom();
        }
    }

    // CALL BY THE INTERACT SCRIPT OF THE DOOR
    public void CloseAndLock_AdjacentCompletedRoomDoors()
    {
        if (myRoom == null)
        {
            InitializeReferences();
        }
            
        if (myRoom.LocksOnEnter)
        {
            adjacentDoor.CloseDoor();
            adjacentDoor.LockDoor();
        }

        myRoom.UpdateMap();
        OpenDoor();
    }

    public void OpenDoor()
    {
        if (!isLocked && !isOpen)
        {
            Debug.Log(name + " is opening");
            isOpen = true;
            if (myAnimator == null)
            {
                InitializeReferences();
            }
            myAnimator.SetBool("ClosedDoor", false);
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            Debug.Log(name + " is closing");
            isOpen = false;
            if (myAnimator == null)
            {
                InitializeReferences();
            }
            myAnimator.SetBool("ClosedDoor", true);
        }
    }

    public void LockDoor()
    {
        isLocked = true;
        SetIsInteractable(false);
        UnlitTorchs();
    }

    public void UnlockDoor()
    {
        isLocked = false;
        SetIsInteractable(true);
        LitTorchs();
    }

    public void InitiateRoom()
    {
        if (!isLocked)
        {
            if (myRoom == null)
            {
                InitializeReferences();
            }
            myRoom.InitiateRoom();
        }
    }

    public void CloseOffWall()
    {
        Instantiate(wallPrefab, transform);
        door.SetActive(false);
    }

    public void CloseOffSecretWall()
    {
        secretWall.gameObject.SetActive(true);
    }

    public void RemoveTorchs()
    {
        foreach (TorchBlock torch in myTorchs)
        {
            torch.gameObject.SetActive(false);
        }
    }

    public void LitTorchs()
    {
        foreach (TorchBlock torch in myTorchs)
        {
            torch.LitTorch();
        }
    }

    public void UnlitTorchs()
    {
        foreach (TorchBlock torch in myTorchs)
        {
            torch.UnlitTorch();
        }
    }
}
