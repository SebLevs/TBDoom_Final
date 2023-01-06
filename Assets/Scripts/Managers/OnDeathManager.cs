using UnityEngine;
using TMPro;

public class OnDeathManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private string loadingText = "Loading...";
    [SerializeField] private int hubSceneInt = 0;
    [SerializeField] private PlayerInputSO playerInputSO;
    private GameObject myLoadSceneEndCue;

    private bool hasAsyncEndedWithAnim = true;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        // Get Component
        myLoadSceneEndCue = transform.GetChild(0).gameObject;

        // Start Load Hub uppon entering [PlayerStateDead.cs]
        GameManager.instance.LoadSceneAsync(0); // TO BE CHANGED FOR : GameManager.instance.StringHUB & delete hubsceneint
    }


    void Update()
    {
        // PrintSimple textual cue for end of loading
        if (!myLoadSceneEndCue.gameObject.activeSelf)
            OnLoadAsyncEndShowCue();

        // On Any Key, load scene
        if (myLoadSceneEndCue.gameObject.activeSelf == true && playerInputSO.AnyKey)
        {
            myLoadSceneEndCue.GetComponent<TextMeshProUGUI>().text = loadingText;
            GameManager.instance.FakeLoading();
        }

        Debug.Log("LOAD SYNC PROGRESS: " + GameManager.instance.AsyncLoad.progress);
    }


    // SECTION - Method - Utility ===================================================================
    public void OnDeathAnimationEnd()
    {
        GameManager.instance.FakeLoading();

        /*
        //if (GameManager.instance.AsyncLoad == null)
        //{
            GameManager.instance.LoadSceneAsync(0); // TO BE CHANGED FOR : GameManager.instance.StringHUB & delete hubsceneint

        //}

        Debug.Log(GameManager.instance.AsyncLoad.progress);

        if (GameManager.instance.AsyncLoad.progress > 0.89f)
            OnLoadAsyncEndShowCue();
        else
            hasAsyncEndedWithAnim = false;
        */
        
    }

    public void OnLoadAsyncEndShowCue() // Allows for load scene uppon async progress >= 0.9f
    {
        if (GameManager.instance.AsyncLoad.progress >= 0.9f)
            myLoadSceneEndCue.gameObject.SetActive(true);
    }
}
