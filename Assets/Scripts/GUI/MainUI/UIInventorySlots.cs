using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlots : MonoBehaviour
{
    [SerializeField] private Image inventorySlotImage;
    [SerializeField] private WeaponsInventorySO weaponsInventory;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVisual()
    {
        var inventorySlots = GetComponentsInChildren<Image>();
        foreach (Image inventorySlot in inventorySlots)
        {
            Destroy(inventorySlot.gameObject);
        }
        for (int i = 0; i < weaponsInventory.MaxMainWeapons; i++)
        {
            var newInventorySlot = Instantiate(inventorySlotImage);
            newInventorySlot.transform.SetParent(gameObject.transform);
            newInventorySlot.transform.localScale = Vector3.one;
            if (i < weaponsInventory.CarriedMainWeapons.Count)
            {
                if (weaponsInventory.CarriedMainWeapons[i] == weaponsInventory.EquippedMainWeapon)
                {
                    newInventorySlot.color = Color.green;
                }
                else
                {
                    newInventorySlot.color = Color.white;
                }
            }
            else
            {
                newInventorySlot.color = Color.black;
            }
        }
    }
}
