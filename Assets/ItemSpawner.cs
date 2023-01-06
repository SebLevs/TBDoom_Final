using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemPedestal itemPrefab;

    private bool collisionHasBeenMade = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collisionHasBeenMade && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            collisionHasBeenMade = true;
            var newItem = Instantiate(itemPrefab);
            newItem.transform.position = new Vector3(collision.contacts[0].point.x, 0, collision.contacts[0].point.z);
            newItem.transform.SetParent(collision.gameObject.GetComponentInParent<Room>().transform);
            Destroy(gameObject);
        }
    }
}
