using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Pickables/Pickable", fileName = "PickableSO")]
public class PickableSO : ItemSO
{
    [SerializeField] private string pickableName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color color;
    [SerializeField] private bool healthValueIsPercent;
    [SerializeField] private float healthValue;
    [SerializeField] private bool armorValueIsPercent;
    [SerializeField] private float armorValue;
    [SerializeField] private bool ammoValueIsPercent;
    [SerializeField] private float ammoValue;
    [SerializeField] private bool secondaryValueIsPercent;
    [SerializeField] private float secondaryValue;
    [SerializeField] private float currencyValue;
    [SerializeField] private Sprite pickableUISprite;
    [SerializeField] private float merchantPrice;
    [TextArea(1, 100)][SerializeField] private string description;

    public string PickableName { get => pickableName; }
    public Sprite Sprite { get => sprite; }
    public Color Color { get => color; }
    public bool HealthValueIsPercent { get => healthValueIsPercent; }
    public float HealthValue { get => healthValue; }
    public bool ArmorValueIsPercent { get => armorValueIsPercent; }
    public float ArmorValue { get => armorValue; }
    public bool AmmoValueIsPercent { get => ammoValueIsPercent; }
    public float AmmoValue { get => ammoValue; }
    public bool SecondaryValueIsPercent { get => secondaryValueIsPercent; }
    public float SecondaryValue { get => secondaryValue; }
    public float CurrencyValue { get => currencyValue; }
    public Sprite PickableUISprite { get => pickableUISprite; }
    public float MerchantPrice { get => merchantPrice; }
    public string Description { get => description; }

    public PickableSO GetCopy() // WeaponSO copyFrom)
    {
        PickableSO copy = Instantiate(this);

        copy.pickableName = pickableName;
        copy.sprite = sprite;
        copy.color = color;
        copy.healthValueIsPercent = healthValueIsPercent;
        copy.healthValue = healthValue;
        copy.armorValueIsPercent = armorValueIsPercent;
        copy.armorValue = armorValue;
        copy.ammoValueIsPercent = ammoValueIsPercent;
        copy.ammoValue = ammoValue;
        copy.secondaryValueIsPercent = secondaryValueIsPercent;
        copy.secondaryValue = secondaryValue;
        copy.currencyValue = currencyValue;
        copy.pickableUISprite = pickableUISprite;
        copy.merchantPrice = merchantPrice;

        return copy;
    }
}
