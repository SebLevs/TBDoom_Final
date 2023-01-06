using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    //[SerializeField] private Color bossRoomColor;
    //[SerializeField] private Color specialRoomColor;
    //[SerializeField] private Color secretRoomColor;
    //[SerializeField] private Color treasureRoomColor;

    [SerializeField] private TransformSO playerTransform;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private MinimapRoom minimapRoomPrefab;
    [SerializeField] private Transform minimapRoomsParent;

    [SerializeField] private float roomScale = 15.0f;
    [SerializeField] private float minimapScale = 5.0f;

    [SerializeField] private Sprite normalRoomSprite;
    [SerializeField] private Sprite bossRoomSprite;
    [SerializeField] private Sprite treasureRoomSprite;
    [SerializeField] private Sprite secretRoomSprite;
    [SerializeField] private Sprite specialRoomSprite;

    [SerializeField] private Image circleImage;
    [SerializeField] private Image squareImage;
    [SerializeField] private Image center;

    private bool fullView = false;
    private RectTransform myRectTransform;

    [SerializeField] private Vector3 circleMapPosition;
    [SerializeField] private Vector3 circleMapSize;
    [SerializeField] private Vector3 squareMapPosition;
    [SerializeField] private Vector3 squareMapSize;

    private List<MinimapRoom> minimapRooms = new List<MinimapRoom>();

    public float MinimapScale { get => minimapScale; set => minimapScale = value; }

    // Start is called before the first frame update
    public void Initialize()
    {
        myRectTransform = GetComponent<RectTransform>();
        fullView = false;
        squareImage.enabled = false;
        myRectTransform.anchoredPosition = circleMapPosition;
        myRectTransform.sizeDelta = circleMapSize;
        
        minimapRooms.Clear();
        transform.rotation = Quaternion.Euler(0, 0, 180);
        for (int i = 0; i < mapLayoutInformation.RoomPositions.Count; i++)
        {
            if (!mapLayoutInformation.Rooms[i].PlacedOnMap)
            {
                var newMinimapRoom = Instantiate(minimapRoomPrefab);
                newMinimapRoom.name = "Minimap Room " + mapLayoutInformation.Rooms[i].name;
                newMinimapRoom.transform.SetParent(minimapRoomsParent);
                var tempPosition = (Vector3)mapLayoutInformation.RoomPositions[i] * minimapScale;
                newMinimapRoom.transform.localPosition = new Vector3(tempPosition.x, tempPosition.z);
                newMinimapRoom.transform.localScale = Vector3.one;
                newMinimapRoom.transform.localRotation = Quaternion.identity;
                newMinimapRoom.PlaceDoors(mapLayoutInformation.Rooms[i]);
                if (mapLayoutInformation.Rooms[i].IsBossRoom)
                {
                    newMinimapRoom.UpdateSprite(bossRoomSprite);
                }
                else if (mapLayoutInformation.Rooms[i].IsTreasureRoom)
                {
                    newMinimapRoom.UpdateSprite(treasureRoomSprite);
                }
                else if (mapLayoutInformation.Rooms[i].IsSecretRoom)
                {
                    newMinimapRoom.UpdateSprite(secretRoomSprite);
                }
                else if (mapLayoutInformation.Rooms[i].IsSpecialRoom)
                {
                    newMinimapRoom.UpdateSprite(specialRoomSprite);
                }
                else
                {
                    newMinimapRoom.UpdateSprite(normalRoomSprite);
                }
                newMinimapRoom.UpdateSpriteColor(Color.black);
                newMinimapRoom.MyRoom = mapLayoutInformation.Rooms[i];
                newMinimapRoom.gameObject.SetActive(false);
                minimapRooms.Add(newMinimapRoom);
                mapLayoutInformation.Rooms[i].PlacedOnMap = true;
            }
        }
        UpdateMinimap();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fullView)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.Transform.rotation.eulerAngles.y));
            var tempPosition = playerTransform.Transform.position / roomScale * minimapScale;
            minimapRoomsParent.localPosition = new Vector3(-tempPosition.x, -tempPosition.z);
        }
        else
        {
            var tempPosition = playerTransform.Transform.position / roomScale * minimapScale;
            center.transform.localPosition = new Vector3(tempPosition.x, tempPosition.z);
        }    
    }

    public void ToggleMap()
    {
        fullView = !fullView;
        if (fullView)
        {
            circleImage.enabled = false;
            squareImage.enabled = true;
            myRectTransform.anchoredPosition = squareMapPosition;
            myRectTransform.sizeDelta = squareMapSize;
            transform.rotation = Quaternion.identity;
            minimapRoomsParent.localPosition = Vector3.zero;
        }
        else
        {
            circleImage.enabled = true;
            squareImage.enabled = false;
            myRectTransform.anchoredPosition = circleMapPosition;
            myRectTransform.sizeDelta = circleMapSize;
            center.transform.localPosition = Vector3.zero;
        }
    }

    public void UpdateMinimap()
    {
        for (int i = 0; i < minimapRooms.Count; i++)
        {
            if (minimapRooms[i].MyRoom.IsVisibleOnMap)
            {
                minimapRooms[i].gameObject.SetActive(true);
                minimapRooms[i].UpdateSpriteColor(new Color(0.25f, 0.25f, 0.25f, 1));
            }
            if (minimapRooms[i].MyRoom.IsVisitedOnMap)
            {
                minimapRooms[i].gameObject.SetActive(true);
                minimapRooms[i].ShowDoors();
                minimapRooms[i].UpdateSpriteColor(Color.white);
            }
        }
    }
}
