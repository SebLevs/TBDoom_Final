using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPedestal : MonoBehaviour
{
    [SerializeField] private float spriteRotationSpeed = 0.5f;
    [SerializeField] private float spriteVerticalRange = 0.1f;
    [SerializeField] private float spriteVerticalSpeed = 1f;
    [SerializeField] private bool isOnPedestal = true;

    [SerializeField] private WeaponSO specificWeapon;
    [SerializeField] private bool isInHub = false;
    [SerializeField] private bool isInShop = false;
    [SerializeField] private bool weaponIsRandom = true;
    [SerializeField] private WeaponSO[] randomWeapons;
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private UnityEvent mainWeaponHasChanged;

    [SerializeField] private ArrayLinearWeaponSOSO hubWeaponArray_Main;
    [SerializeField] private ArrayLinearWeaponSOSO hubWeaponArray_Secondary;
    [SerializeField] private ArrayLinearWeaponSOSO hubWeaponArray_Melee;

    [SerializeField] private GameObject pedestalVisual;
    [SerializeField] private GameObject groundVisual;

    [SerializeField] private SpriteRenderer pedestalSpriteRenderer;
    [SerializeField] private Light pedestalLight;

    [SerializeField] private SpriteRenderer groundSpriteRenderer;
    [SerializeField] private ParticleSystem groundParticleSystem;
    [SerializeField] private Light groundLight;

    private ItemSO pedestalItem;
    private SpriteRenderer mySpriteRenderer;
    private Vector3 spriteInitialPosition;
    private RandomSeed roomGenerationRandom;

    int randomIndex;

    // Start is called before the first frame update
    void Start()
    {
        var room = GetComponentInParent<Room>();
        roomGenerationRandom = RandomManager.Instance.RoomGenerationRandom;
        if (weaponIsRandom)
        {
            if (isOnPedestal)
            {
                if (room.IsTreasureRoom)
                {
                    pedestalItem = ItemFactory.instance.CreateRandomLegendaryWeapon();
                }
                else
                {
                    pedestalItem = ItemFactory.instance.CreateRandomItem();
                }
            }
            else
            {
                pedestalItem = ItemFactory.instance.CreateRandomItem();
            }    
        }
        else
        {
            pedestalItem = Instantiate(specificWeapon);
        }

        if (isOnPedestal)
        {
            Debug.Log("Item is on Pedestal");
            mySpriteRenderer = pedestalSpriteRenderer;
            groundVisual.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Item is on the Ground");
            mySpriteRenderer = groundSpriteRenderer;
            mySpriteRenderer.transform.rotation = Quaternion.Euler(90, roomGenerationRandom.Random.Next(0, 360), 0);
            pedestalVisual.gameObject.SetActive(false);
        }

        spriteInitialPosition = mySpriteRenderer.transform.localPosition;
        Room merchantRoom = GetComponentInParent<Room>();
        if (!merchantRoom) merchantRoom = transform.parent.GetComponentInParent<Room>();
        isInShop = (merchantRoom != null) ? merchantRoom.IsMerchantRoom : false;
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnPedestal) return;

        if (mySpriteRenderer == null)
        {
            mySpriteRenderer = pedestalSpriteRenderer;
        }
        mySpriteRenderer.transform.Rotate(new Vector3(0, spriteRotationSpeed, 0));
        var verticalOffset = Mathf.Sin(Time.time * spriteVerticalSpeed) * spriteVerticalRange;
        mySpriteRenderer.transform.localPosition = spriteInitialPosition + new Vector3(0, verticalOffset, 0);
    }

    public void ActivatePedestal()
    {
        if (pedestalItem is WeaponSO)
        {
            var otherPedestals = transform.parent.transform.GetComponentsInChildren<ItemPedestal>();
            if (!isInHub && isOnPedestal)
            {
                //foreach (ItemPedestal pedestal in otherPedestals)
                //{
                //    if (pedestal != this && pedestal.isOnPedestal)
                //    {
                //        pedestal.EmptyPedestal();
                //    }
                //}
            }

            if (weaponInventory.CarriedMainWeapons.Count < weaponInventory.MaxMainWeapons && !isInHub)
            {
                WeaponSO temp = (pedestalItem as WeaponSO);

                /// <NOTE>
                /// 
                /// If conditional is a TEMPORARY DEBUGGER
                ///     - Get rid of conditional when merging of weaponPedestal.cs with ShootingRangePedestal.cs
                /// 
                /// </NOTE>
                if (!pedestalItem.name.Contains("Melee") && !pedestalItem.name.Contains("Grenade")) // !pedestalWeapon.HasPlayerUsedOnce && 
                {
                    hubWeaponArray_Main.AddUnique(randomWeapons[randomIndex]); // Hub linear array update
                }

                pedestalItem = null;
                weaponInventory.CarriedMainWeapons.Add(temp);
                weaponInventory.EquippedMainWeapon = temp;
                mainWeaponHasChanged.Invoke();
                EmptyPedestal();
                HideItemUI();
            }
            else if (weaponInventory.EquippedMainWeapon != weaponInventory.CarriedMainWeapons[0])
            {
                WeaponSO temp = (pedestalItem as WeaponSO);
                pedestalItem = weaponInventory.EquippedMainWeapon;
                var carriedWeaponIndex = weaponInventory.CarriedMainWeapons.IndexOf(weaponInventory.EquippedMainWeapon);
                weaponInventory.CarriedMainWeapons[carriedWeaponIndex] = temp;
                weaponInventory.EquippedMainWeapon = temp;
                mainWeaponHasChanged.Invoke();
                UpdateSprite();
                UpdateItemUI();
            }
        }
        else if (pedestalItem is PickableSO)
        {
            if (isInShop)
            {
                if (PickableManager.instance.BuyPickable(pedestalItem as PickableSO))
                {
                    EmptyPedestal();
                    HideItemUI();
                }
            }
            else
            {
                if (PickableManager.instance.PickPickable(pedestalItem as PickableSO))
                {
                    EmptyPedestal();
                    HideItemUI();
                }
            }
        }
        // mySpriteRenderer.enabled = false;
    }

    private void UpdateSprite()
    {
        mySpriteRenderer.sprite = pedestalItem.ItemSprite;

        if (pedestalItem is PickableSO) return;

        var psMain = groundParticleSystem.main;
        if (isOnPedestal)
        {
            switch ((pedestalItem as WeaponSO).ProjectileStrategies.Count)
            {
                case 2:
                    pedestalLight.color = Color.blue;
                    break;
                case 3:
                    pedestalLight.color = Color.yellow;
                    break;
                case 4:
                    pedestalLight.color = Color.magenta;
                    break;
                default:
                    pedestalLight.color = Color.white;
                    break;
            }
        }
        else if (!isOnPedestal)
        {
            switch ((pedestalItem as WeaponSO).ProjectileStrategies.Count)
            {
                case 2:
                    psMain.startColor = Color.blue;
                    groundLight.color = Color.blue;
                    break;
                case 3:
                    psMain.startColor = Color.yellow;
                    groundLight.color = Color.yellow;
                    break;
                case 4:
                    psMain.startColor = Color.magenta;
                    groundLight.color = Color.magenta;
                    break;
                default:
                    psMain.startColor = Color.white;
                    groundLight.color = Color.white;
                    break;
            }
        }
    }

    public void EmptyPedestal()
    {
        pedestalItem = null;
        if (isOnPedestal)
        {
            pedestalLight.enabled = false;
            mySpriteRenderer.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    public void OnInteractEnter()
    {
        if (pedestalItem == null) return;

        //if (other.gameObject.GetComponent<PlayerContext>() != null)
        //{
            ShowItemUI();
            UpdateItemUI();
        //}
    }

    public void OnInteractExit()
    {
        if (pedestalItem == null) return;

        //if (other.gameObject.GetComponent<PlayerContext>() != null)
        //{
            HideItemUI();
        //}
    }

    private void UpdateItemUI()
    {
        ItemGUICanvas.instance.MoveAndUpdate(transform.position, pedestalItem, isInShop);
    }

    private void ShowItemUI()
    {
        if (pedestalItem != null)
            mySpriteRenderer.enabled = false;
        ItemGUICanvas.instance.FadeIn();
    }

    private void HideItemUI()
    {
        if (pedestalItem != null)
            mySpriteRenderer.enabled = true;
        ItemGUICanvas.instance.FadeOut();
    }
}
