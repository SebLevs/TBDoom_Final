using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerBlock : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private float damage;
    [SerializeField] private float flameLength = 1f;
    [SerializeField] private float flameInterval = 0.25f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private int branchNumber = 2;
    [SerializeField] private int rotationDirection = 1;

    private GameObject flamesParent;

    private void Awake()
    {
        // *** Temporary Testing ***********
        var random = Random.Range(0, 2);
        if (random == 1)
        {
            rotationDirection = -1;
            branchNumber = 2;
        }
        // *********************************

        flamesParent = new GameObject("FlamesParent");
        flamesParent.transform.SetParent(transform);
        flamesParent.transform.localScale = Vector3.one * (1 / transform.localScale.x);
        for (int i = (int)(-flameLength / flameInterval); i <= (int)(flameLength / flameInterval); i++)
        {
            var newFlame = Instantiate(flamePrefab);
            newFlame.transform.SetParent(transform);
            newFlame.transform.localPosition = new Vector3(i * flameInterval * (1 / transform.localScale.x), 0, 0);
            // newFlame.GetComponent<PhysicalProjectile>().Damage = damage;
            newFlame.GetComponent<PhysicalProjectile>().HasLifespanTimer = false;
            newFlame.GetComponent<PhysicalProjectile>().MyRigidbody.useGravity = false;
        }
        if (branchNumber == 4)
        {
            for (int i = (int)(-flameLength / flameInterval); i <= (int)(flameLength / flameInterval); i++)
            {
                var newFlame = Instantiate(flamePrefab);
                newFlame.transform.SetParent(transform);
                newFlame.transform.localPosition = new Vector3(0, 0, i * flameInterval * (1 / transform.localScale.x));
                // newFlame.GetComponent<PhysicalProjectile>().Damage = damage;
                newFlame.GetComponent<PhysicalProjectile>().HasLifespanTimer = false;
                newFlame.GetComponent<PhysicalProjectile>().MyRigidbody.useGravity = false;
            }
        }
        ActivateFlameThrowerBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Rotate(new Vector3(0, rotationSpeed * rotationDirection * Time.deltaTime, 0));
        }
    }

    public void ActivateFlameThrowerBlock()
    {
        isActive = true;
        flamesParent.SetActive(true);
    }

    public void DeactivateFlameThrowerBlock()
    {
        isActive = false;
        flamesParent.SetActive(false);
    }
}
