using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBlock : MonoBehaviour
{
    [SerializeField] private float damage = 5f;

    private DamageOnContact myDamageOnContact;

    // Start is called before the first frame update
    void Start()
    {
        myDamageOnContact = GetComponentInChildren<DamageOnContact>();
        myDamageOnContact.Damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
