using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{
    [SerializeField] private Image doorImagePrefab;
    [SerializeField] private Image room1x1ImagePrefab;
    [SerializeField] private Image room2x1ImagePrefab;
    [SerializeField] private Image room2x2ImagePrefab;
    [SerializeField] private Image roomLImagePrefab;
    private List<Image> myDoors = new List<Image>();
    private Image roomImage;
    private Room myRoom;

    public Image RoomImage { get => roomImage; set => roomImage = value; }
    public Room MyRoom { get => myRoom; set => myRoom = value; }

    public void PlaceDoors(Room room)
    {
        transform.localRotation = Quaternion.Euler(0, 0, -room.transform.localRotation.eulerAngles.y);
        foreach (DoorBlock door in room.MyRoomDoors)
        {
            if (door.IsConnected && !door.IsOutsideSecret)
            {
                var newDoor = Instantiate(this.doorImagePrefab);
                newDoor.color = Color.black;
                newDoor.transform.SetParent(transform);
                // var tempPosition = door.transform.parent.localPosition + new Vector3(0.5f, 0, 0.5f);
                var tempPosition = (door.transform.parent.localPosition + new Vector3(0.5f, 0, 0.5f)) / 15 * 5;
                var tempX = tempPosition.x;
                var tempY = tempPosition.z;
                if (tempX > 0)
                    tempX = Mathf.FloorToInt(tempX);
                else
                    tempX = Mathf.CeilToInt(tempX);
                if (tempY > 0)
                    tempY = Mathf.FloorToInt(tempY);
                else
                    tempY = Mathf.CeilToInt(tempY);
                newDoor.transform.localPosition = new Vector3(tempX, tempY);
                newDoor.transform.localRotation = Quaternion.Euler(0, 0, door.transform.localRotation.eulerAngles.y);
                newDoor.transform.localScale = Vector3.one;
                myDoors.Add(newDoor);
                newDoor.gameObject.SetActive(false);
            }
        }
        Image newRoom;
        switch (room.RoomType)
        {
            case RoomType.R1X1:
                newRoom = Instantiate(this.room1x1ImagePrefab);
                break;
            case RoomType.R2X1:
                newRoom = Instantiate(this.room2x1ImagePrefab);
                break;
            case RoomType.R2X2:
                newRoom = Instantiate(this.room2x2ImagePrefab);
                break;
            case RoomType.RL:
                newRoom = Instantiate(this.roomLImagePrefab);
                break;
            default:
                Debug.Log("Room EnemyType not found, generating 1x1 room on minimap.");
                newRoom = Instantiate(this.room1x1ImagePrefab);
                break;
        }
        newRoom.transform.SetParent(transform);
        newRoom.transform.localPosition = Vector3.zero;
        newRoom.transform.localRotation = Quaternion.identity;
        newRoom.transform.localScale = Vector3.one;
        UpdateSpriteColor(Color.black);
        roomImage = newRoom;
    }

    public void UpdateSprite(Sprite sprite)
    {
        var images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (!myDoors.Contains(image))
                image.sprite = sprite;
        }
    }   
    
    public void UpdateSpriteColor(Color color)
    {
        var images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (!myDoors.Contains(image))
                image.color = color;
        }
    }

    public void ShowDoors()
    {
        foreach (Image door in myDoors)
        {
            door.gameObject.SetActive(true);
        }
    }
}
