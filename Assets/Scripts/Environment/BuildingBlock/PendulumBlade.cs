using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumBlade : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxAngle = 90;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * speed) * maxAngle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<LivingEntityContext>() != null)
        {
            collision.gameObject.GetComponent<LivingEntityContext>().TakeDamage(5, transform.position);
        }
    }
}
