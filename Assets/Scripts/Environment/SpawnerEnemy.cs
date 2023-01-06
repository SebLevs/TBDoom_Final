using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <TODO>
/// Need implementing Observer pattern after implementation of task 62.01 (refactoring of room.cs -enemies- into roomenemymanager)
/// </TODO>
public class SpawnerEnemy : MonoBehaviour
{
    // SECTION - Field =========================================================
    [SerializeField] private Transform playerTransform;
    // [SerializeField] private SpriteRenderer m_temporarySpriteRenderer;

    [Header("SpawnerEnemy")]
    [SerializeField] private LayerMask anySurfaceMask;
    [SerializeField] private SpawnerOrientation spawnerOrientation = 0;
                     private const string isSpawningTrigger = "isSpawning";

    [Header("Animator Modifiers")]
    [SerializeField] private bool isAnimationSpeedRandom = false;
    [Range(minAnimSpeed, maxAnimSpeed)]
    [SerializeField] private float specificAnimationSpeedModifier = 1f;
                     private const float minAnimSpeed = 0.75f;
                     private const float maxAnimSpeed = 1f;
                     private Animator myAnimationEntityAnimator = null;

    [Header("Entity")]
    [SerializeField] private GameObject[] myDesiredPrefabs; // TODO: Could be changed to ArrayLinear scriptable ref if need arises for modularity
                     private GameObject myDesiredPrefab;

                     private GameObject myPrefabSpawnedEntity;
                     private SpriteRenderer AnimationEntitySprtRdr;

                     private const float AdjustedHalfUnit = 0.49f; // 0,49f == half a unit - 0.01f | Avoid sprite clipping through environment


    // SECTION - Property =========================================================
    private bool HasAnimationEntityAnimation => myAnimationEntityAnimator.runtimeAnimatorController != null;
    private Transform GetEntityAnimationTransform => myAnimationEntityAnimator.transform;
    public Sprite GetDesiredPrefabSprite => myDesiredPrefabs[0].GetComponentInChildren<SpriteRenderer>().sprite;
    public int GetMyDesiredPrefabsCount { get => myDesiredPrefabs.Length; }


    // SECTION - Method =========================================================
    #region UNITY
    private void Start()
    {
        playerTransform = PlayerContext.instance.transform;

        if (spawnerOrientation == SpawnerOrientation.FLOOR)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        myAnimationEntityAnimator = transform.GetChild(1).gameObject.GetComponent<Animator>();

        SetPositionRotation(isSpawner: true);
        //SetEntityPrefab(GetEntityPrefab());
    }

    private void FixedUpdate()
    {
        if (spawnerOrientation == SpawnerOrientation.CENTER)
        {
            transform.LookAt(playerTransform);
        }
        // AnimationEntitySprtRdr.transform.LookAt(playerTransform);
        // m_temporarySpriteRenderer.transform.LookAt(playerTransform);
    }
    #endregion

    #region Getter
    private GameObject GetEntityPrefab()
    {
        if (myDesiredPrefabs == null) return null;

        GameObject myEntity = null;

        if (myDesiredPrefabs.Length == 1)
        {
            myEntity = myDesiredPrefabs[0];
        }
        else
        {
            int randIndex = Random.Range(0, myDesiredPrefabs.Length);
            myEntity = myDesiredPrefabs[randIndex];
        }

        return myEntity;
    }

    private int GetRandomOrientation()
    {
        List<int> surfaces = new List<int>();
        int lastCheck = -1;

        // Center
        surfaces.Add(1);

        // Y Axis
        lastCheck = (StaticRayCaster.IsLineCastTouching(transform.position, Vector3.down, 1.0f, anySurfaceMask).transform) ? 2 : -1;
        if (lastCheck != -1)
        {
            surfaces.Add(lastCheck);
        }

        lastCheck = (StaticRayCaster.IsLineCastTouching(transform.position, Vector3.up, 1.0f, anySurfaceMask).transform) ? 3 : -1;
        if (lastCheck != -1)
        {
            surfaces.Add(lastCheck);
        }

        // Sides
        lastCheck = (StaticRayCaster.IsLineCastTouching(transform.position, Vector3.left, 1.0f, anySurfaceMask).transform) ? 4 : -1;
        if (lastCheck != -1)
        {
            surfaces.Add(lastCheck);
        }
        lastCheck = (StaticRayCaster.IsLineCastTouching(transform.position, Vector3.right, 1.0f, anySurfaceMask).transform) ? 5 : -1;
        if (lastCheck != -1)
        {
            surfaces.Add(lastCheck);
        }
        lastCheck = (StaticRayCaster.IsLineCastTouching(transform.position, Vector3.forward, 1.0f, anySurfaceMask).transform) ? 6 : -1;
        if (lastCheck != -1)
        {
            surfaces.Add(lastCheck);
        }
        lastCheck = (StaticRayCaster.IsLineCastTouching(transform.position, Vector3.back, 1.0f, anySurfaceMask).transform) ? 7 : -1;
        if (lastCheck != -1)
        {
            surfaces.Add(lastCheck);
        }

        return surfaces[Random.Range(0, surfaces.Count)];
    }
    #endregion

    #region Setter
    /// <summary>
    /// Set passed entity GameObject's position and rotation<br />
    /// if HasAnimation: Calls MoveEntityThroughSpawner() IEnumerator
    /// </summary>
    public void SetEntityPrefab(GameObject entity = null)
    {
        GameObject trueEntity = (entity) ? entity : GetEntityPrefab();

        if (!trueEntity)
        {
            DestroyImmediate(gameObject);
        }

        myDesiredPrefab = trueEntity;

        myAnimationEntityAnimator.transform.forward = transform.forward;
        myAnimationEntityAnimator.transform.localPosition = Vector3.zero;
        myAnimationEntityAnimator.GetComponent<SpriteRenderer>().sprite = trueEntity.GetComponentInChildren<SpriteRenderer>().sprite;

        SetPositionRotation(isSpawner: false);

        if (HasAnimationEntityAnimation)
        {
            myAnimationEntityAnimator.speed = (isAnimationSpeedRandom)? Random.Range(minAnimSpeed, maxAnimSpeed): specificAnimationSpeedModifier;
            //Debug.Log("Animator controller was not null, an animation should be played and [myDesiredPrefab] should be instanciated afterward at Animation.position");
        }
        else
        {
            StartCoroutine(MoveEntityThroughSpawner(myAnimationEntityAnimator.gameObject));
        }
    }

    /// <summary>
    /// If isSpawner == false: [Animation] gameObject will be set instead
    /// </summary>
    private void SetPositionRotation(bool isSpawner)
    {
        GameObject entity = (isSpawner) ? gameObject : myAnimationEntityAnimator.gameObject;

        AnimationEntitySprtRdr = entity.GetComponentInChildren<SpriteRenderer>();

        float entitySpriteSize = (isSpawner) ? AdjustedHalfUnit : AnimationEntitySprtRdr.size.x;

        float x = entity.transform.position.x;
        float y = entity.transform.position.y;
        float z = entity.transform.position.z;

        Vector3 eulers = entity.transform.rotation.eulerAngles;

        switch (spawnerOrientation)
        {
            case SpawnerOrientation.ANYSURFACE:
                spawnerOrientation = (SpawnerOrientation)GetRandomOrientation();
                SetPositionRotation(isSpawner);
                break;

            case SpawnerOrientation.CENTER:
                if (!isSpawner)
                {
                    z -= entitySpriteSize;
                }
                break;

            case SpawnerOrientation.FLOOR:
                y -= entitySpriteSize;

                if (isSpawner)
                {
                    eulers.x = -90f;
                }
                break;

            case SpawnerOrientation.CEILING:
                y += entitySpriteSize;

                if (isSpawner)
                {
                    eulers.x = 90f;
                }
                break;

            case SpawnerOrientation.LEFT: // Get eulers with raycast to wall at .49 distance?
                x -= entitySpriteSize;

                if (isSpawner)
                {
                    eulers.y = 90f;
                }
                break;

            case SpawnerOrientation.RIGHT:
                x += entitySpriteSize;

                if (isSpawner)
                {
                    eulers.y = -90f;
                }
                break;

            case SpawnerOrientation.FORWARD:
                z += entitySpriteSize;

                if (isSpawner)
                {
                    eulers.y = 0f;
                }
                break;

            case SpawnerOrientation.BACKWARD:
                z -= entitySpriteSize;

                if (isSpawner)
                {
                    eulers.y = 180f;
                }
                break;

            default: Debug.Log("ERROR @ [SetEntityBehindSpawner()] of [SpawnerEnemy.cs]"); break;
        }

        entity.transform.position = new Vector3(x, y, z);
        entity.transform.rotation = Quaternion.Euler(eulers);
    }
    #endregion

    #region Utility
    private IEnumerator MoveEntityThroughSpawner(GameObject entity)
    {
        Animator spawnerAnim = GetComponent<Animator>();
        spawnerAnim.SetTrigger(isSpawningTrigger);

        //float animLength = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        float animLength = spawnerAnim.GetCurrentAnimatorStateInfo(0).length;
        int deltaIteration = Mathf.FloorToInt(animLength / Time.fixedDeltaTime);

        float spriteWidth = (spawnerOrientation == SpawnerOrientation.CENTER) ? AnimationEntitySprtRdr.size.x * 1.15f : AnimationEntitySprtRdr.size.x * 1.5f;

        float addDistance = spriteWidth / deltaIteration;

        for (int count = 0; count < deltaIteration; count++)
        {
            Vector3 newPosition = entity.transform.position + transform.forward * addDistance;

            entity.transform.position = newPosition;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        RoomEnemyManager myRoomEnemyManager = GetComponentInParent<RoomEnemyManager>();
        myPrefabSpawnedEntity = Instantiate(myDesiredPrefab, myRoomEnemyManager.transform);
        myPrefabSpawnedEntity.transform.position = GetEntityAnimationTransform.position;
        myPrefabSpawnedEntity.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        DestroyImmediate(gameObject);
    }
    #endregion
}
