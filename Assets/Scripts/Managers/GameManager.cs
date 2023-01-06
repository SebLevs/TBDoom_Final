using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static GameManager instance;

    [Header("Important Layer Masks")]
    public LayerMask groundMask;
    public LayerMask interactableMask;
    public LayerMask canBeShotByPlayerMask;
    public LayerMask respawnMask;
    public bool levelIsReady = false;

    [Header("Important Scenes")]
    [SerializeField] private string stringHUB = "HUB";

    // private Transform playerTransform;
    [SerializeField] private PositionRotationSO lastDamagePosition;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    // private Transform playerTransformRef;

    private PlayerContext player;

    private AsyncOperation asyncLoad;
    private float asyncLoadTimer;
    private bool asyncLoadReady = false;

    [SerializeField] private GameEvent mainWeaponHasChanged;
    [SerializeField] private GameEvent secondaryWeaponHasChanged;
    [SerializeField] private GameEvent meleeWeaponHasChanged;

    [SerializeField] private WeaponsInventorySO myWeaponsInventory;
    [SerializeField] private TransformSO menuCanvas;
    // [SerializeField] private SelectMenu menuCanvas;

    [SerializeField] private LoadingTube loadingTube;

    private Vector3 playerMapPosition = Vector3.zero;

    // SECTION - Property ===================================================================
    // public Transform PlayerTransformRef => playerTransformRef;
    // public Transform PlayerTransformRef => playerTransform.Transform;

    public AsyncOperation AsyncLoad { get => asyncLoad; }
    public string StringHUB { get => stringHUB; }
    public PositionRotationSO LastDamagePosition { get => lastDamagePosition; set => lastDamagePosition = value; }
    // public Transform PlayerTransform { get => player.transform; }


    // SECTION - Method - Unity Specific ===================================================================
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // playerTransformRef = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        instance.SetMouseCursor_LockedInvisible();

        RandomManager.Instance.Initialize();
        AIManager.instance.Initialize();
        if (ItemFactory.instance)
        {
            ItemFactory.instance.Initialize();
        }

        MapLayout _mapLayout = FindObjectOfType<MapLayout>();
        if (_mapLayout) { _mapLayout.Initialize(); }
    }

    private void Update()
    {
        // CheckPlayerPosition();
        if (asyncLoadReady)
        {
            if (asyncLoadTimer > 0)
            {
                asyncLoadTimer -= Time.deltaTime;
            }
            else
            {
                asyncLoad.allowSceneActivation = true;
            }
        }
    }


    // SECTION - Method - Utility ===================================================================
    #region REGION - Option & Mouse Cursor & Mouse Visible
    public void ToggleMouseCursor_ConfinedToLocked()
    {
        Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }

    public void SetMouseCursor_LockedInvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetMouseCursor_Manual(CursorLockMode lockMode, bool cursorVisible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = cursorVisible;
    }

    public void ToggleTimeScale()
    {
        if (Time.timeScale >= 1.0f)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
    #endregion

    public void ToggleSeed()
    {
        GameObject mySeed = menuCanvas.Transform.GetChild(menuCanvas.Transform.childCount-1).gameObject;
        mySeed.SetActive(!mySeed.activeSelf);

        if (mySeed.activeSelf)
            mySeed.GetComponentInChildren<TextMeshProUGUI>().text = RandomManager.Instance.RoomGenerationRandom.Seed.ToString();
    }

    public void UpdateSeedText()
    {
        GameObject mySeed = menuCanvas.Transform.GetChild(menuCanvas.Transform.childCount - 1).gameObject;

        if (mySeed.activeSelf)
            mySeed.GetComponentInChildren<TextMeshProUGUI>().text = RandomManager.Instance.RoomGenerationRandom.Seed.ToString();
    }

    public void ShowMenu()
    {
        menuCanvas.Transform.GetComponent<SelectMenu>().PanelToggle(0);
    }

    public void QuitMenu()
    {
        menuCanvas.Transform.GetComponent<SelectMenu>().QuitMenu();
    }

    public void FakeLoading()
    {
        if (PickableManager.instance)
            PickableManager.instance.ResetPickableValues();
        LoadSceneAsync("FakeLoading", true);
    }

    public void ReturnToHub()
    {
        // Destroy(FindObjectOfType<PlayerContext>().gameObject);

        LoadScene("Hub_Sandbox");
        // Time.timeScale = 1;
    }

    //public void CheckPlayerPosition()
    //{
    //    var playerPosition = playerTransform.Transform.position / 15;
    //    var playerEvaluation = new Vector3(Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y), Mathf.RoundToInt(playerPosition.z));
    //    if (playerEvaluation != playerMapPosition)
    //    {
    //        RecalculateRoomPosition(playerEvaluation);
    //    }
    //}

    //public void RecalculateRoomPosition(Vector3 playerEvaluation)
    //{
    //    var index = mapLayoutInformation.RoomPositions.IndexOf(playerEvaluation);
    //    mapLayoutInformation.Rooms[index].UpdateAdjacentRooms();
    //}

    #region REGION - Scene Load & Quit
    // Basic
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(scene).name);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartLoadLoadingTube()
    {
        levelIsReady = false;
        loadingTube.GetComponent<Collider>().enabled = true;
        StartCoroutine(LoadLoadingTube());
    }

    private IEnumerator LoadLoadingTube()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        var asynchScene = SceneManager.LoadSceneAsync("Loading_Sandbox", LoadSceneMode.Additive);
        asynchScene.allowSceneActivation = true;
        
        yield return new WaitForSeconds(2.5f);

        MusicManager.instance.StopMusic();
        if (currentScene == "Hub_Sandbox")
        {
            PickableManager.instance.ResetPickableValues();
            myWeaponsInventory.SetDefaultWeapons();
            mainWeaponHasChanged.Raise();
            secondaryWeaponHasChanged.Raise();
            meleeWeaponHasChanged.Raise();
        }
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Loading_Sandbox"));
        SceneManager.UnloadSceneAsync(currentScene);
        StartCoroutine(LoadMainLevel());
    }

    private IEnumerator LoadMainLevel()
    {
        var asynchScene = SceneManager.LoadSceneAsync("Level_Sandbox", LoadSceneMode.Additive);
        asynchScene.allowSceneActivation = true;
        MusicManager.instance.SwitchToOutOfCombat();

        yield return new WaitForSeconds(2.5f);

        SceneManager.UnloadSceneAsync("Loading_Sandbox");
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level_Sandbox"));

        loadingTube.GetComponent<Collider>().enabled = false;
    }

    //private IEnumerator UnloadLoadingTube()
    //{
    //    yield return new WaitForSeconds(3.0f);
    //    SceneManager.UnloadSceneAsync("Loading_Sandbox");
    //}


    // Async
    public void LoadSceneAsync(int scene, bool allowSceneActivation = false)
    {
        // Async load desired scene
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = allowSceneActivation;

        Debug.Log($"asyncload: {asyncLoad}");

        // Prevents unintentional inputs
        Input.ResetInputAxes();

        // Garbage collection - just in case -
        System.GC.Collect();
    }

    public void LoadSceneAsync(string scene, bool allowSceneActivation = false)
    {
        // Async load desired scene
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = allowSceneActivation;

        // Prevents unintentional inputs
        Input.ResetInputAxes();

        // Garbage collection - just in case -
        System.GC.Collect();
    }

    // Quit Game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
