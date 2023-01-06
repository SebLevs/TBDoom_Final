using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected int itemValue;
    [SerializeField] protected Sprite itemSprite;

    public string ItemName { get => itemName; set => itemName = value; }
    public string ItemDescription { get => itemDescription; set => itemDescription = value; }
    public int ItemValue { get => itemValue; set => itemValue = value; }
    public Sprite ItemSprite { get => itemSprite; set => itemSprite = value; }
}
