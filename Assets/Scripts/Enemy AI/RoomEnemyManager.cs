using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyManager : MonoBehaviour
{
    // SECTION - Field
    [Tooltip("Indicates Mimic only room to prevent sudden activation of mimics\n")]
    [SerializeField] private bool isMimicRoom = false;
                     private const string mimicNameID = "mimic";

    [SerializeField] private TransformSO bossHealthBarCanvas;

    [SerializeField]
    private string[] doNotManageTheseTags =
    {
        "Practice Target"
    };

    private bool playerhasEnteredRoom = false;

    private BoxCollider myTriggerZone;

    [SerializeField] private List<GameObject> myEntities = new List<GameObject>();
    [SerializeField] private List<GameObject> myMimics = new List<GameObject>();


    private Room myRoom;

    // SECTION - Property
    public List<GameObject> MyEntities { get => myEntities; set => myEntities = value; }
    public List<GameObject> MyMimics { get => myMimics; set => myMimics = value; }
    public bool PlayerhasEnteredRoom { get => playerhasEnteredRoom; set => playerhasEnteredRoom = value; }



    // SECTION - Method
    #region Unity
    private void Start()
    {
        SetReferences();

        //SetAllEntityActive(false);

        SetRoomTriggerZone();
        SetLivingEntityGameObjects(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (isRoomShootingRange)
        //ShootingRangeHandler(other);

        if (!myRoom) return;
        /*
        if (!myRoom.IsShootingRangeRoom && other.CompareTag("Player") && !myRoom.IsCompleted)
        {
            //myRoom.CloseAllDoors();
            SetSpawnersPrefab();

            // myRoom.UpdateAdjacentRooms();
        }
        else if (!myRoom.IsShootingRangeRoom && other.CompareTag("Player") && myRoom.IsCompleted)
        {
            // myRoom.UpdateAdjacentRooms();
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        playerhasEnteredRoom = false;

        //if (myRoom && !myRoom.IsShootingRangeRoom)
           // ShootingRangeHandler(other);
    }

    #endregion

    #region Setter
    private void SetReferences()
    {
        myRoom = GetComponentInParent<Room>();
    }

    public void SetSpawnersPrefab()
    {
        foreach (GameObject entity in MyEntities)
        {
            SpawnerEnemy entitySpawner = entity.GetComponent<SpawnerEnemy>();
            if (entitySpawner)
                entitySpawner.SetEntityPrefab();
        }
    }

    private void SetRoomTriggerZone()
    {
        if (!myRoom) return;

        myTriggerZone = GetComponent<BoxCollider>();

        // Shooting range room size == manually setted
        if (!myRoom.IsShootingRangeRoom)
            myTriggerZone.enabled = false;
            //myTriggerZone.size = new Vector3(myRoom.XDimension - 2, myRoom.XDimension - 1, myRoom.ZDimension - 2);
    }

    public void SetBossHealthCanvas(bool setAs)
    {
        // TODO
        //      - Should destroy instantiated prefab of health bar inside a horizontal layout so that...
        //        multiple boss can easily destack (with a check for boss qty to not delete in the middle of a fight)
        bossHealthBarCanvas.Transform.GetChild(0).gameObject.SetActive(setAs);
        bossHealthBarCanvas.Transform.GetChild(1).gameObject.SetActive(setAs);
    }

    public void SetLivingEntityGameObjects(bool setAs)
    {
        foreach (GameObject livingEntity in myEntities)
        {
            livingEntity.SetActive(setAs);
        }
    }


    public void SetAllEntityActive(bool setAs)
    {
        foreach (GameObject entity in myEntities)
            SetEntityActive(entity, setAs);
    }

    public void SetEntityActive(GameObject other, bool setAs)
    {
        Animator otherAnimator = GetComponent<Animator>();
        if (otherAnimator)
            otherAnimator.enabled = setAs;

        SpawnerEnemy otherSpawner = other.GetComponent<SpawnerEnemy>();
        if (otherSpawner)
            otherSpawner.enabled = setAs;
        else
        {
            /*
            LivingEntityContext otherLEC = other.GetComponent<LivingEntityContext>();
            if (otherLEC)
                otherLEC.enabled = setAs;

            BasicEnemyContext otherBasicEnemyContext = other.GetComponent<BasicEnemyContext>();
            if (otherBasicEnemyContext)
                otherBasicEnemyContext.enabled = setAs;

            other.transform.GetChild(other.transform.childCount - 1).gameObject.SetActive(setAs);
            */
        }
    }
    #endregion

    #region Utility
    public void ClearMyEntities()
    {
        foreach (GameObject entity in MyEntities)
            Destroy(entity.gameObject);

        myEntities.Clear();
    }

    private void ShootingRangeHandler(Collider other)
    {
        if (other.gameObject.layer == 8 && !playerhasEnteredRoom) // Layer int # for LIVING ENTITY
        {
            // Quit Check
            foreach (string tag in doNotManageTheseTags)
            {
                if (other.CompareTag(tag))
                    return;
            }

            LivingEntityContext otherLEC = other.GetComponent<LivingEntityContext>();
            if (otherLEC != null && !otherLEC.ActivateEnemyOnTriggerEnter)
                SetActive(other.gameObject);
        }
        else if (other.CompareTag("Player") && !playerhasEnteredRoom)
        {
            playerhasEnteredRoom = true;

            foreach (GameObject otherEntity in myEntities)
                SetActive(otherEntity, true);
        }
    }

    public void SetActive(GameObject other, bool forceActivation = false)
    {
        LivingEntityContext otherLivingEntityContext = other.GetComponent<LivingEntityContext>();
        if (forceActivation)
        {
            if (otherLivingEntityContext)
                otherLivingEntityContext.ActivateEnemyOnTriggerEnter = true;

            SetActive(other);
        }
        else if (!CompareTag("Player"))
        {
            if (otherLivingEntityContext && otherLivingEntityContext.ActivateEnemyOnTriggerEnter)
            {
                SetEntityActive(other, true);
            }
            else
            {
                SetEntityActive(other, false);
            }

            if (otherLivingEntityContext)
                otherLivingEntityContext.ActivateEnemyOnTriggerEnter = !otherLivingEntityContext.ActivateEnemyOnTriggerEnter;
        }
    }

    public void CheckLivingEntities()
    {
        StaticDebugger.SimpleDebugger(false, $"Testing Living Entities in the room: {gameObject.name}");
        StartCoroutine(StartCheckLivingEntities());
        // Invoke("TestLivingEntities", 0.1f);
    }

    private IEnumerator StartCheckLivingEntities()
    {
        yield return new WaitForEndOfFrame();
        // yield return new WaitForSeconds(0.1f);
        // var livingEntitiesInsideRoom = GetComponentsInChildren<LivingEntityContext>();
        if (myEntities.Count <= 0)
        {
            myRoom.FinishRoom();
        }
        else if (myEntities.Count == myMimics.Count && !isMimicRoom)
        {
            ActivateAllMimic();
        }
    }

    /// <summary>
    /// Uses [SetStateNearTarget()] of [Passive] child object
    /// </summary>
    private void ActivateAllMimic()
    {
        SetStateNearTarget specificBehaviour_SetState;

        foreach (GameObject myMimicLEC in myMimics)
        {
            int lastChild = myMimicLEC.transform.childCount - 1;
            specificBehaviour_SetState = myMimicLEC.transform.GetChild(lastChild).GetComponent<SetStateNearTarget>();

            if (specificBehaviour_SetState)
                specificBehaviour_SetState.Behaviour();
        }
    }
    #endregion

    #region Observer Pattern
    public void Subscribe(GameObject entity)
    {
        myEntities.Add(entity);

        if (entity.name.ToLower().Contains(mimicNameID))
            myMimics.Add(entity);
        //else // Deactivate only the normal mobs
        //myRoomEnemyManager.SetEntityActive(gameObject, false);
    }

    public void Unsubscribe(GameObject entity)
    {
        myEntities.Remove(entity);
        MyEntities.TrimExcess();

        if (entity.name.ToLower().Contains(mimicNameID))
        {
            myMimics.Remove(entity);
            MyMimics.TrimExcess();
        }
    }

    public void Notify()
    {

    }
    #endregion
}
