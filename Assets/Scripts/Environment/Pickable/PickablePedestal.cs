using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PickablePedestal : MonoBehaviour
{
    [SerializeField] private float spriteRotationSpeed = 0.5f;
    [SerializeField] private float spriteVerticalRange = 0.1f;
    [SerializeField] private float spriteVerticalSpeed = 1f;

    [SerializeField] private PickablesListSO buyablePickablesList;
    [SerializeField] private PickablesListSO pickablePickablesList;
    [SerializeField] private TextMeshPro myPrice;

    // [SerializeField] private WeaponsInventorySO weaponInventory;
    // [SerializeField] private UnityEvent mainWeaponHasChanged;

    private bool isInShop = false;
    private PickableSO pedestalPickable;
    private SpriteRenderer mySpriteRenderer;
    private Vector3 spriteInitialPosition;
    private System.Random roomGenerationRandom;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteInitialPosition = mySpriteRenderer.transform.localPosition;

        Room merchantRoom = GetComponentInParent<Room>();
        if (!merchantRoom) merchantRoom = transform.parent.GetComponentInParent<Room>();
        isInShop = (merchantRoom != null)? merchantRoom.IsMerchantRoom : false;

        roomGenerationRandom = RandomManager.Instance.RoomGenerationRandom.Random;
        if (isInShop)
        {
            pedestalPickable = Instantiate(buyablePickablesList.PickableSO[roomGenerationRandom.Next(0, buyablePickablesList.PickableSO.Count)]);
        }
        else
        {
            pedestalPickable = Instantiate(pickablePickablesList.PickableSO[roomGenerationRandom.Next(0, pickablePickablesList.PickableSO.Count)]);
        }
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        mySpriteRenderer.transform.Rotate(new Vector3(0, spriteRotationSpeed, 0));
        var verticalOffset = Mathf.Sin(Time.time * spriteVerticalSpeed) * spriteVerticalRange;
        mySpriteRenderer.transform.localPosition = spriteInitialPosition + new Vector3(0, verticalOffset, 0);
    }

    public void ActivatePedestal()
    {
        if (isInShop)
        {
            if (PickableManager.instance.BuyPickable(pedestalPickable))
            {
                EmptyPedestal();
            }
        }
        else
        {
            if (PickableManager.instance.PickPickable(pedestalPickable))
            {
                var otherPedestals = transform.parent.transform.GetComponentsInChildren<PickablePedestal>();
                foreach (PickablePedestal pedestal in otherPedestals)
                {
                    pedestal.EmptyPedestal();
                }
            }
        }
    }

    private void UpdateSprite()
    {
        mySpriteRenderer.sprite = pedestalPickable.PickableUISprite;
        if (isInShop)
        {
            myPrice.text = pedestalPickable.MerchantPrice + "$";
        }
        else
        {
            myPrice.enabled = false;
        }
    }

    public void EmptyPedestal()
    {
        mySpriteRenderer.enabled = false;
        myPrice.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
    }
}
