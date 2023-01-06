using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class ShootingRangeManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Shooting Range Variables")]
    [SerializeField] private int rangeX;
    [SerializeField] private int rangeY;
    [SerializeField] private int rangeZ;
                     private Transform shootingRangeCenter;
                     private BoxCollider shootingRangeCenterCollider;

                     private GameObject[,] myGrid;
                     private Vector3Int pZero = new Vector3Int();

    [Header("Practice Targets")]
    [SerializeField] private ArrayLinearGameObjectSO myDefaultPracticeTargetPrefabsSO;
    [SerializeField] private ArrayLinearGameObjectSO myPracticeTargetPrefabsSO;
    private RoomEnemyManager myRoomEnemyManager;
    //[SerializeField] private List<GameObject> myPracticeTargetInstances = new List<GameObject>();

    [Header("Preview")]
    [SerializeField] private int quantityPerLine = 6;
    [SerializeField] private GameObject myImagesPrefab;
    [SerializeField] private Sprite mySpriteWhenMultipleEnemyFromSpawner;
                     private Transform horizontalLayoutTransform;
                     private List<GameObject> myPreviewList = new List<GameObject>();
                     private float horizontalLayoutOffset = 0.1f;



    private AstarPath myAstarPath; // Width and Depth are : (grid.N * 2) + 4

    //[Header("Shooting Range Ground")]
    // Note:
    //      - Tiled : X & Y :  0.32 * ((grid.N + 1) * 2)
                     private Animator outlineAnimator;
                     private const string outlineAnimString = "isShootingRangeActive";
                     private SpriteRenderer shootingRange_GroundSprite;
                     private SpriteRenderer shootingRange_OutlineSprite;
                     private const float spritesY = -0.49f;


    // SECTION - Method
    #region Unity
    private void Start()
    {
        GetComponentsAtStart();

        // Note
        //      - If even number, left side will be 1 square shorter than right side
        pZero.x = -(rangeX >> 1);
        pZero.z = -(rangeZ >> 1);

        SetAstarPathAndScan();

        InstantiateSpecificArrayLinear(myDefaultPracticeTargetPrefabsSO);

        SetArrayLinearGenerics();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerPresent = true;
            myRoomEnemyManager.PlayerhasEnteredRoom = true;

            //GetComponent<Collider>().enabled = false;

            //if (myPracticeTargetInstances.Count != 0)
            if (myRoomEnemyManager.MyEntities.Count != 0)
            {
                outlineAnimator.SetBool(outlineAnimString, true);
                myRoomEnemyManager.SetSpawnersPrefab();

                myRoomEnemyManager.MyEntities.ForEach(entity => 
                {
                    AIBrain _brain = entity.GetComponent<AIBrain>();
                    if (_brain)
                    {
                        _brain.enabled = true;
                    }
                });
            }

            MusicManager.instance.SwitchToInCombat();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset shooting range if player gets outisde sumo ring
        //      - Prevents abuse by shooting from outside enemies' range
        if (other.CompareTag("Player"))
        {
            //isPlayerPresent = false;
            myRoomEnemyManager.PlayerhasEnteredRoom = false;

            //GetComponent<Collider>().enabled = true;

            other.GetComponent<LivingEntityContext>().FullHeal(); // Heal player

            outlineAnimator.SetBool(outlineAnimString, false);

            ResetShootingRange();

            InstantiateSpecificArrayLinear(myDefaultPracticeTargetPrefabsSO);
            MusicManager.instance.PlayHubTheme();
        }
        else if (other.gameObject.layer == 8) // Layer int # for LIVING ENTITY
        {

            // Manage untagged entites leaving the shooting range
            if (CompareTag("Untagged"))
            {
                /* // In case of need, Use this to kill off entitites instead of spawning them in the middle of the shooting range
                LivingEntityContext otherLEC = other.GetComponent<LivingEntityContext>();
                if (otherLEC)
                    otherLEC.InstantDeath();
                else
                    Destroy(otherLEC);

                return;
                */
            }

            // TODO: Should be placed at its original position on grid?
            //      - Other solution?
            other.transform.position = shootingRangeCenter.position;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    #endregion

    #region Getter
    private void GetComponentsAtStart()
    {
        myRoomEnemyManager = GetComponentInChildren<RoomEnemyManager>();
        if (!myRoomEnemyManager)
            myRoomEnemyManager = GetComponentInParent<RoomEnemyManager>();

        // Shooting Range Center
        shootingRangeCenter = transform.GetChild(1).transform;
        myGrid = new GameObject[rangeX, rangeZ];
        shootingRangeCenterCollider = shootingRangeCenter.GetComponent<BoxCollider>();
        shootingRangeCenterCollider.enabled = true;
        shootingRangeCenterCollider.size = new Vector3(rangeX + 1, rangeY + 1, rangeZ + 1);

        myAstarPath = AIManager.instance.AstarPath;

        // Sprite Renderer
        shootingRange_GroundSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        shootingRange_OutlineSprite = transform.GetChild(3).GetComponent<SpriteRenderer>();
        horizontalLayoutTransform = transform.GetChild(transform.childCount - 1).transform;

        // Animator
        outlineAnimator = shootingRange_OutlineSprite.GetComponent<Animator>();
    }
    #endregion

    #region Setter
    private void SetPreviewsImage(GameObject[] myPracticeTargets = null)
    {
        if (myPracticeTargets == null)
            return;

        if (myPreviewList != null && myPreviewList.Count > 0)
            foreach (GameObject obj in myPreviewList)
            {
                Destroy(obj);
            }

        myPreviewList.Clear();

        // Set Bell's enemy's sprite previews
        foreach (GameObject obj in myPracticeTargets)
        {
            myPreviewList.Add(Instantiate(myImagesPrefab, horizontalLayoutTransform));

            if (obj.CompareTag("Enemy"))
                myPreviewList[^1].GetComponent<Image>().sprite = obj.GetComponentInChildren<SpriteRenderer>().sprite;
            else
            {
                SpawnerEnemy myObjectSpawner = obj.GetComponentInChildren<SpawnerEnemy>();
                if (myObjectSpawner.GetMyDesiredPrefabsCount > 1)
                    myPreviewList[^1].GetComponent<Image>().sprite = mySpriteWhenMultipleEnemyFromSpawner;
                else
                    myPreviewList[^1].GetComponent<Image>().sprite = myObjectSpawner.GetDesiredPrefabSprite;
            }

            // Move up the sprite's preview to prevent clipping
            if (myPreviewList.Count % quantityPerLine == 0)
            {
                RectTransform myCanvasRect = horizontalLayoutTransform.GetComponent<RectTransform>();

                float x = myCanvasRect.position.x;
                float y = myCanvasRect.position.y + horizontalLayoutOffset;
                float z = myCanvasRect.position.z;

                myCanvasRect.position = new Vector3(x, y, z);
            }
        }
    }

    private void SetArrayLinearGenerics()
    {
        if (myPracticeTargetPrefabsSO.IsEmpty)
        {
            myPracticeTargetPrefabsSO.Copy(myDefaultPracticeTargetPrefabsSO.GetArray);
        }

        SetPreviewsImage(myPracticeTargetPrefabsSO.GetArray);
    }

    private void SetAstarPathAndScan()
    {
        // Pathfinding
        //astartPath = GameObject.Find(myAStarString).GetComponent<AstarPath>();

        // astartPath.data.gridGraph.center = shootingRangeCenter.position;
        // astartPath.data.gridGraph.center.y = -1.0f; // Must be at ground level

        // astartPath.data.gridGraph.SetDimensions((rangeX * 2) + 4, (rangeZ * 2) + 4, astartPath.data.gridGraph.nodeSize);

        //astartPath.Scan();


        // Set ALL GRIDGRAPHS available to desired settings
        for (int index = 0; index < myAstarPath.data.graphs.Length; index++)
        {
            GridGraph gg = myAstarPath.data.graphs[index] as GridGraph;
            gg.center = shootingRangeCenter.position;
            gg.center.y = -1.0f; // Must be at ground level
            gg.SetDimensions((rangeX * 4) + 4, (rangeZ * 4) + 4, myAstarPath.data.gridGraph.nodeSize);
        }

        myAstarPath.Scan();

        // Shooting range's ground
        float x = shootingRangeCenter.position.x;
        float z = shootingRangeCenter.position.z;
        Vector3 spritesPos = new Vector3(x, spritesY, z);

        shootingRange_GroundSprite.transform.position = spritesPos;
        shootingRange_OutlineSprite.transform.position = spritesPos;

        shootingRange_GroundSprite.size = new Vector2((myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.width), (myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.depth));
        shootingRange_OutlineSprite.size = new Vector2((myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.width), (myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.depth));
    }

    #endregion

    #region Utility
    public void InstantiateShootingRange(bool instantiateDefaults = false)
    {
        if (!myRoomEnemyManager.PlayerhasEnteredRoom)
        {
            if (!instantiateDefaults) // Instantiate prefabs
                InstantiateSpecificArrayLinear(myPracticeTargetPrefabsSO);
            else                      // Instantiate defaults
                InstantiateSpecificArrayLinear(myDefaultPracticeTargetPrefabsSO);
        }
    }

    public void InstantiateSpecificArrayLinear(ArrayLinearGameObjectSO myDesiredArrayLinear)
    {
        if (!myDesiredArrayLinear.IsEmpty)
        {
            for (int index = 0; index < myDesiredArrayLinear.Count; index++)
                if (myDesiredArrayLinear.GetArray[index] != null)
                    OnGridInstantiate(myDesiredArrayLinear.GetElement(index));
        }
    }

    private void OnGridInstantiate(GameObject practiceTarget)
    {
        if (practiceTarget == null)
            return;

        int x = Random.Range(0, rangeX);
        int z = Random.Range(0, rangeZ);

        // Recursive Instantiate Check
        if (myGrid[x, z] != null)
        {
            OnGridInstantiate(practiceTarget);
            return;
        }

        int posX = pZero.x + x;
        int posZ = pZero.z + z;

        myGrid[x, z] = Instantiate(practiceTarget, shootingRangeCenter.transform);
        myGrid[x, z].transform.localPosition = new Vector3(posX, 0.0f, posZ);
    }

    public void ResetShootingRange()
    {
        myRoomEnemyManager.SetBossHealthCanvas(false);
        myRoomEnemyManager.ClearMyEntities();
    }
    #endregion
}
