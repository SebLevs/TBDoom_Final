using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    [SerializeField] private GameObject normalPlaform;
    [SerializeField] private GameObject breakablePlatform;

    // Start is called before the first frame update
    void Start()
    {
        var random = Random.Range(0, 2);
        if (random == 0)
        {
            normalPlaform.transform.localPosition = new Vector3(-1, 0, 0);
            breakablePlatform.transform.localPosition = new Vector3(1, 0, 0);
        }
        else
        {
            normalPlaform.transform.localPosition = new Vector3(1, 0, 0);
            breakablePlatform.transform.localPosition = new Vector3(-1, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
