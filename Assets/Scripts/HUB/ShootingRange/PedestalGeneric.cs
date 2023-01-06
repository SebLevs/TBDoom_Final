using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PedestalGeneric : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Weapons Inventory Scriptable")]
    [SerializeField] private WeaponsInventorySO playerWeaponInventorySO;

    [Header("Player Currency")]
    [SerializeField] private FloatReference playerCurrentCurrency;
    [SerializeField] private UnityEvent currencyHasChanged;

    [Header("Weapon Rack")]
    [SerializeField] private Sprite[] AddChangeCue;

    [Header("Text Bubble - Single Unit")]
    [SerializeField] private Transform tbRectPrice;
                     private TextBubble tbPrice;
    [SerializeField] private Transform tbRectName;
                     private TextBubble tbName;
    [SerializeField] private Transform tbRectDescription;
                     private TextBubble tbDescription;

    [Space(10)]
    [SerializeField] private WeaponSO pedestalWeapon = null;
    [SerializeField] private PickableSO pedestalPickable = null;

    [Space(10)]
    [Tooltip("Available Scriptables\nWill be copied if not in hub to avoid changing base scriptable")]
    [SerializeField] private ArrayLinearWeaponSOSO availableWeaponsSO = null;
    [Tooltip("Available Scriptables\nWill be copied if not in hub to avoid changing base scriptable")]
    [SerializeField] private ArrayLinearPickableSOSO availablePickablesSO = null;

    [SerializeField] private bool isInHub = false;
                     private bool isInShop = false;
                     private bool isAddToInventory = false; // TODO: Set add or change based on isHub or not
                     private Image mySpriteRenderer;
                     private SpriteRenderer addOrChangeSprite;
                     private Interactable myInteractable;

                     private System.Random roomGenerationRandom;

    public int PedestalWeaponCurrentCount => availableWeaponsSO.CurrentIndex;
    // SECTION - Property ===================================================================
    #region Property
    public IArrayLinear AvailableItemsSO { get => (availableWeaponsSO != null) ? availableWeaponsSO : availablePickablesSO; }
    public int GetCountDebug => (availableWeaponsSO != null) ? availableWeaponsSO.Count : availablePickablesSO.Count;
    #endregion


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        mySpriteRenderer = transform.GetChild(2).GetComponentInChildren<Image>();
        mySpriteRenderer.SetNativeSize();
        addOrChangeSprite = transform.GetChild(4).GetComponent<SpriteRenderer>();
        myInteractable = GetComponent<Interactable>();

        // Text Bubble
        tbPrice = tbRectPrice.GetComponentInChildren<TextBubble>();
        tbName = tbRectName.GetComponentInChildren<TextBubble>();
        tbDescription = tbRectDescription.GetComponentInChildren<TextBubble>();

        SetRoomReferences();

        // Copy to avoid overriding base scriptable
        availableWeaponsSO = (availableWeaponsSO == null) ? null : Instantiate(availableWeaponsSO);
        availablePickablesSO = (availablePickablesSO == null) ? null : Instantiate(availablePickablesSO); 

        SetPedestal();

        SetInteractability();
        SetPedestalItemTexts();

        SetAddOrChange();
        SetPedestalSprite();
    }

    // SECTION - Method - Utility Specific ===================================================================
    #region Getter
    public IArrayLinear GetArrayLinear()
    {
        if (availableWeaponsSO != null) return availableWeaponsSO;
        else if (availablePickablesSO != null) return availablePickablesSO;
        else return null;
    }

    private float GetItemValue()
    {
        float currencyValue;

        if (pedestalWeapon)
            currencyValue = pedestalWeapon.CurrencyValue;
        else
            currencyValue = pedestalPickable.CurrencyValue;

        return currencyValue;
    }

    #endregion

    #region Setter
    private void SetPedestal()
    {
        int randomIndex;
        roomGenerationRandom = RandomManager.Instance.RoomGenerationRandom.Random;

        // Set PedestalWeapon/Pickable
        if (pedestalWeapon == null) // && availableWeaponsSO != null)
        {
            if (!isInHub)
            {
                // randomIndex = roomGenerationRandom.Next(0, availableWeaponsSO.Length);
                pedestalWeapon = ItemFactory.instance.CreateRandomWeapon(); // Instantiate(availableWeaponsSO.GetElement(randomIndex));
            }
            else
                pedestalWeapon = availableWeaponsSO.GetElement(0);
        }
        else if (pedestalPickable == null && availablePickablesSO != null)
        {
            if (!isInHub)
            {
                randomIndex = roomGenerationRandom.Next(0, availablePickablesSO.Length);
                pedestalPickable = Instantiate(availablePickablesSO.GetElement(randomIndex));
            }
            else
                pedestalPickable = availablePickablesSO.GetElement(0);
        }

        // Disable unwanted children outside hub
        if (!isInHub)
            DisableChildrenBehaviour();
    }

    private void SetRoomReferences()
    {
        // TEMP NOTE: Comment out next line if testing in hub ******************************************************************************************
        isInHub = SceneManager.GetActiveScene().name.ToLower().Contains("hub"); // NOTE: To be uncommented after testings

        Room myRoom = GetComponentInParent<Room>();
        isInShop = (myRoom != null) ? myRoom.IsMerchantRoom : false;
    }

    private void SetInteractability()
    {
        if (isInShop)
        {
            if (playerCurrentCurrency.Value < GetItemValue())
                myInteractable.SetIsInteractable(false);
        }
    }

    private void SetPedestalSprite()
    {
        if (pedestalWeapon != null)
            mySpriteRenderer.sprite = pedestalWeapon.WeaponUISprite;
        else if (pedestalPickable != null)
            mySpriteRenderer.sprite = pedestalPickable.PickableUISprite;
    }

    private void SetPedestalItemTexts() // TODO: May need refactoring inside of SetPedestalSprite() after implementation of Text bubbles task
    {
        if (!tbPrice || !tbDescription) return;

        SetAddOrChange();

        if (pedestalWeapon != null)
        {
            tbName.SetText(pedestalWeapon.WeaponName);
            tbDescription.SetText(pedestalWeapon.WeaponDescription);
        }
        else if (pedestalPickable != null)
        {
            tbName.SetText(pedestalPickable.PickableName);
            tbDescription.SetText(pedestalPickable.Description);
        }

        if (isInShop)
        {
            if (pedestalWeapon != null)
                tbPrice.SetText(pedestalWeapon.CurrencyValue + "$");
            else if (pedestalPickable != null)
                tbPrice.SetText(pedestalPickable.MerchantPrice + "$");
        }
        else if (!pedestalWeapon && !pedestalPickable)
        {
            tbRectPrice.gameObject.SetActive(false);
            tbRectName.gameObject.SetActive(false);
            tbRectDescription.gameObject.SetActive(false);
        }
    }
    private void SetAddOrChange()
    {
        if (addOrChangeSprite.gameObject.activeSelf)
            return;

        if (pedestalWeapon != null)
        {
            if (pedestalWeapon.IsMain)
            {
                isAddToInventory = !playerWeaponInventorySO.IsMainFull;
                addOrChangeSprite.sprite = AddChangeCue[(playerWeaponInventorySO.IsMainFull) ? 1 : 0];
            }
            else if (pedestalWeapon.IsSecondary)
            {
                isAddToInventory = !playerWeaponInventorySO.IsSecondaryFull;
                addOrChangeSprite.sprite = AddChangeCue[(playerWeaponInventorySO.IsSecondaryFull) ? 1 : 0];
            }
            else if (pedestalWeapon.IsMelee)
            {
                isAddToInventory = !playerWeaponInventorySO.IsMeleeFull;
                addOrChangeSprite.sprite = AddChangeCue[(playerWeaponInventorySO.IsMeleeFull) ? 1 : 0];
            }
        }
    }
    #endregion

    #region Handler
    private void HandlerAdd()
    {
        //if (!isInHub)
        //{
        //    EmptyPedestal();

        //    HandlerAfterIsInShop();
        //}
    }

    private void HandlerChangeWeapon(WeaponSO lastEquipedWeapon)
    {
        if (!isInHub)
        {
            pedestalWeapon = lastEquipedWeapon;
            availableWeaponsSO.ChangeAt(lastEquipedWeapon, AvailableItemsSO.CurrentIndex);
            SetPedestalSprite();

            HandlerAfterIsInShop();
        }
        else
            pedestalWeapon.RefillAmmoAndClip();
    }

    private void HandlerAfterIsInShop()
    {
        if (!isInShop)
            EmptyRoomPedestals(isAlsoEmptyThisPedestal: false);
        else if (pedestalWeapon != null)
        {
            playerCurrentCurrency.Value -= GetItemValue();
            currencyHasChanged.Invoke();
        }
    }
    #endregion

    #region Utility
    public void ToggleAddOrChange()
    {
        if (pedestalWeapon != null)
        {
            isAddToInventory = !isAddToInventory;
            addOrChangeSprite.sprite = AddChangeCue[(isAddToInventory) ? 0 : 1];
        }
        else
            addOrChangeSprite.sprite = AddChangeCue[0];

        //SetAddOrChange();
    }


    public void ChangePedestalItem_LeftOrRight(bool getNext = false)
    {
        if (pedestalWeapon != null)
        {
            if (getNext)
                pedestalWeapon = availableWeaponsSO.GetNext();
            else if (!getNext)
                pedestalWeapon = availableWeaponsSO.GetPrevious();
        }
        else if (pedestalPickable != null)
        {
            if (getNext)
                pedestalPickable = availablePickablesSO.GetNext();
            else if (!getNext)
                pedestalPickable = availablePickablesSO.GetPrevious();
        }

        SetPedestalSprite();
        SetPedestalItemTexts();
    }

    public void ChangePedestalWeapon_LeftOrRight(bool getNext = false)
    {
        if (getNext)
            pedestalWeapon = availableWeaponsSO.GetNext();
        else if (!getNext)
            pedestalWeapon = availableWeaponsSO.GetPrevious();

        SetPedestalSprite();
        SetPedestalItemTexts();
    }

    public void ChangePedestalPickable_LeftOrRight(bool getNext = false) // TODO: May need refactoring/merge with [ChangePedestalWeapon_LeftOrRight();]
    {
        if (getNext)
            pedestalPickable = availablePickablesSO.GetNext();
        else if (!getNext)
            pedestalPickable = availablePickablesSO.GetPrevious();

        SetPedestalSprite();
        SetPedestalItemTexts();
    }

    private void EmptyRoomPedestals(bool isAlsoEmptyThisPedestal = false)
    {
        PedestalGeneric[] pedestals = transform.parent.transform.GetComponentsInChildren<PedestalGeneric>();

        if (pedestals.Length > 0)
        {
            foreach (PedestalGeneric pedestal in pedestals)
            {
                if (pedestal == this && !isAlsoEmptyThisPedestal)
                    continue;

                pedestal.EmptyPedestal();
            }
        }
    }

    private void EmptyPedestal()
    {
        mySpriteRenderer.gameObject.SetActive(false);
        SetPedestal();
        gameObject.layer = LayerMask.NameToLayer("Obstacle");

        // Disable Texts & trigger
        tbRectPrice.parent.gameObject.SetActive(false);
        tbRectName.parent.gameObject.SetActive(false);
        tbRectDescription.parent.gameObject.SetActive(false);
    }

    private void DisableChildrenBehaviour()
    {
        addOrChangeSprite.gameObject.SetActive(false);
        transform.GetChild(transform.childCount - 2).gameObject.SetActive(false);
    }
    #endregion


    public void AddOrChange_AutoCheck()
    {
        SetInteractability();

        if (pedestalWeapon != null)
        {
            if (pedestalWeapon.IsMain)
                AddOrChange_MainWeapon();
            else if (pedestalWeapon.IsSecondary)
                AddOrChange_SecondaryWeapon();
            else if (pedestalWeapon.IsMelee)
                AddOrChange_MeleeWeapon();
        }
        else
            AddOrChange_Pickable();

        SetPedestalItemTexts();
        SetAddOrChange();
    }

    // Main weapon
    #region Main Weapon
    public void AddOrChange_MainWeapon()
    {
        if (isAddToInventory)
            AddMainWeapon();
        else
            ChangeMainWeapon();
    }

    private void AddMainWeapon()
    {
        isAddToInventory = playerWeaponInventorySO.AddWeapon_Main(pedestalWeapon);

        HandlerAdd();
    }

    private void ChangeMainWeapon()
    {
        WeaponSO lastEquipedWeapon = playerWeaponInventorySO.ChangeWeapon_Main(pedestalWeapon);

        HandlerChangeWeapon(lastEquipedWeapon);
    }
    #endregion

    // Secondary Weapon
    #region Secondary Weapon
    public void AddOrChange_SecondaryWeapon()
    {
        if (isAddToInventory)
            AddSecondaryWeapon();
        else
            ChangeSecondaryWeapon();
    }

    private void AddSecondaryWeapon()
    {
        isAddToInventory = playerWeaponInventorySO.AddWeapon_Secondary(pedestalWeapon);

        HandlerAdd();
    }

    private void ChangeSecondaryWeapon()
    {
        WeaponSO lastEquipedWeapon = playerWeaponInventorySO.ChangeWeapon_Secondary(pedestalWeapon);

        HandlerChangeWeapon(lastEquipedWeapon);
    }
    #endregion

    // Melee Weapon
    #region Melee Weapon
    public void AddOrChange_MeleeWeapon()
    {
        if (isAddToInventory)
            AddMeleeWeapon();
        else
            ChangeMeleeWeapon();
    }

    private void AddMeleeWeapon()
    {
        isAddToInventory = playerWeaponInventorySO.AddWeapon_Melee(pedestalWeapon);

        HandlerAdd();
    }

    private void ChangeMeleeWeapon()
    {
        playerWeaponInventorySO.ChangeWeapon_Melee(pedestalWeapon);
    }
    #endregion

    // Pickable
    #region Pickable
    public void AddOrChange_Pickable()
    {
        AddPickable();

        HandlerAdd();
    }

    private void AddPickable()
    {
        if (isInShop)
            PickableManager.instance.BuyPickable(pedestalPickable);
        else
            PickableManager.instance.PickPickable(pedestalPickable);
    }
    #endregion
}
