using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDetail : MonoBehaviour
{
    [SerializeField] private Sprite[] mySprites;
    [SerializeField] private int height = 0;

    // Start is called before the first frame update
    void Start()
    {
        var mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.sprite = mySprites[Random.Range(0, mySprites.Length)];
        mySpriteRenderer.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), height, Random.Range(-0.4f, 0.4f));
        mySpriteRenderer.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        var random = Random.Range(0, 4);
        if (random != 0)
        {
            mySpriteRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
