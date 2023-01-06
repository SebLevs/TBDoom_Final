using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDoDamage : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Damage Output")]
    [SerializeField] private WeaponSO myWeapon;
    [SerializeField] private float additionalDamage = 0;

    [Header("Target Management")]
    [SerializeField] private string[] ignoreDamageForTags;

    private float GetTotalDamageOutput => (myWeapon) ? myWeapon.Damage + additionalDamage : additionalDamage;


    // SECTION - Method - Unity Specific ===================================================================
    private void OnCollisionEnter(Collision other)
    {
        foreach (string tag in ignoreDamageForTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                Destroy(gameObject);
                return;
            }

            LivingEntityContext otherLEC = other.transform.GetComponent<LivingEntityContext>();

            if (otherLEC != null)
                otherLEC.TakeDamage(GetTotalDamageOutput, transform.position);
        }
    }
}
