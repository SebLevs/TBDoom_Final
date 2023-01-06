using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    [SerializeField] private PickableSO pickableSO;
    [SerializeField] private float spawnForce = 10f;

    private SpriteRenderer mySpriteRenderer;
    private Rigidbody myRigidboby;

    private void Awake()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.sprite = pickableSO.Sprite;
        mySpriteRenderer.color = pickableSO.Color;
        myRigidboby = GetComponent<Rigidbody>();
        myRigidboby.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)).normalized * spawnForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivatePickable(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<PickableManager>().PickPickable(pickableSO))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePickable(other);
        }
    }
}
