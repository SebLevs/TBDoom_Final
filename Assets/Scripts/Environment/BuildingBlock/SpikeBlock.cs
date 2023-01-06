using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    [SerializeField] private float damage = 5f;
    
    [SerializeField] private bool pressurePlate = false;
    [SerializeField] private float timeDelay = 1f;
    
    [SerializeField] private bool timedBased = false;
    [SerializeField] private float timeOffset = 0;

    private Animator myAnimator;
    private DamageOnContact myDamageOnContact;

    private void OnEnable()
    {
        if (isActive)
        {
            if (pressurePlate)
            {
                myAnimator.SetBool("SpikeOut", false);
            }
            else
            {
                myAnimator.SetBool("SpikeOut", true);
            }
        }
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myDamageOnContact = GetComponentInChildren<DamageOnContact>();
        myDamageOnContact.Damage = damage;
        ActivateSpikeBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (timedBased)
            {
                var sign = Mathf.Sin(Time.timeSinceLevelLoad + timeOffset * Mathf.PI);
                if (sign > 0)
                {
                    myAnimator.SetBool("SpikeOut", true);
                }
                else if (sign < 0)
                {
                    myAnimator.SetBool("SpikeOut", false);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            if (pressurePlate)
            {
                StartCoroutine(ActivatePressurePlate());
            }
        }
    }

    public void ActivateSpikeBlock()
    {
        isActive = true;
        if (pressurePlate)
        {
            myAnimator.SetBool("SpikeOut", false);
        }
        else
        {
            myAnimator.SetBool("SpikeOut", true);
        }
    }

    public void DeactivateSpikeBlock()
    {
        isActive = false;
        myAnimator.SetBool("SpikeOut", false);
    }

    private IEnumerator ActivatePressurePlate()
    {
        yield return new WaitForSeconds(timeDelay);

        myAnimator.SetBool("SpikeOut", true);

        yield return new WaitForSeconds(timeDelay);

        myAnimator.SetBool("SpikeOut", false);
    }
}
