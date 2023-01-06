using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FakeLoading : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private TextMeshProUGUI myText;

    private int loadingDot = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectOfType<PlayerContext>() != null)
        {
            Destroy(FindObjectOfType<PlayerContext>().gameObject);
        }
        if (FindObjectOfType<MusicManager>() != null)
        {
            Destroy(FindObjectOfType<MusicManager>().gameObject);
        }
        Time.timeScale = 1;
        StartCoroutine(ReturnToHub());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ReturnToHub()
    {
        StartCoroutine(LoadingDot());
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.ReturnToHub();
    }

    private IEnumerator LoadingDot()
    {
        loadingDot++;
        if (loadingDot > 3)
        {
            loadingDot = 0;
        }
        myText.text = "Loading";
        for (int i = 0; i < loadingDot; i++)
        {
            myText.text += ".";
        }
        yield return new WaitForSeconds(waitTime / 10);
        StartCoroutine(LoadingDot());
    }
}
