using UnityEngine;

public class DropBehaviour : MonoBehaviour
{
    [Header("Drops")]
    [SerializeField] private GameObject[] myDrops;
    [SerializeField] private float dropChance = -1f;
    [SerializeField] private int dropQty = 1;
    [SerializeField] private float impulseForce = 1f;
    [SerializeField] private int minDrop = 5;
    [SerializeField] private int maxDrop = 10;

    private Room myRoom;

    // Start is called before the first frame update
    void Start()
    {
        myRoom = GetComponentInParent<Room>();
    }

    public void Drop()
    {
        if (dropChance == -1)
        {
            var limit = Random.Range(minDrop, maxDrop + 1);
            for (int i = 0; i < limit; i++)
            {
                var newDrop = Instantiate(myDrops[Random.Range(0, myDrops.Length)], transform);

                // Temporary solution for the spawning of object in a room.
                // Would need to be fixed for turret or other physical objects that sapwns from the player.
                if (myRoom != null)
                    newDrop.transform.SetParent(myRoom.transform);
                else
                    newDrop.transform.SetParent(null);
                // **********************************************************

                newDrop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0, 0.5f), Random.Range(-1f, 1f)).normalized * impulseForce, ForceMode.Impulse);
            }
        }
        else
        {
            if (Random.Range(0, 100) < dropChance)
            {
                for (int i = 0; i < dropQty; i++)
                {
                    var newDrop = Instantiate(myDrops[Random.Range(0, myDrops.Length)], transform);

                    // Temporary solution for the spawning of object in a room.
                    // Would need to be fixed for turret or other physical objects that sapwns from the player.
                    if (myRoom != null)
                        newDrop.transform.SetParent(myRoom.transform);
                    else
                        newDrop.transform.SetParent(null);
                    // **********************************************************

                    newDrop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0, 0.5f), Random.Range(-1f, 1f)).normalized * impulseForce, ForceMode.Impulse);
                }
            }
        }    
    }
}
