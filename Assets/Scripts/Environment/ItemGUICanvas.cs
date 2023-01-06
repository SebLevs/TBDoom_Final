using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemGUICanvas : MonoBehaviour
{
    public static ItemGUICanvas instance;
    
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text price;
    [SerializeField] private TMP_Text damage;
    [SerializeField] private TMP_Text fireRate;
    [SerializeField] private TMP_Text reloadSpeed;
    [SerializeField] private TMP_Text clipSize;
    [SerializeField] private TMP_Text[] strategies;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image windowPanel;

    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject strategiesPanel;

    private TMP_Text[] myTexts;
    private Image[] myImages;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        myTexts = GetComponentsInChildren<TMP_Text>();
        myImages = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAndUpdate(Vector3 position, ItemSO item, bool _inShop)
    {
        transform.position = position;
        itemName.text = item.ItemName;
        description.text = item.ItemDescription;
        if (_inShop)
        {
            price.color = Color.white;
            price.text = item.ItemValue.ToString() + "$";
        }
        else
        {
            price.color = Color.clear;
        }
        weaponImage.sprite = item.ItemSprite;

        if (item is WeaponSO)
        {
            statsPanel.SetActive(true);
            strategiesPanel.SetActive(true);
            damage.text = "Damage : " + (item as WeaponSO).Damage.ToString();

            if ((item as WeaponSO).IsMelee)
            {
                fireRate.text = "Attack Speed : " + (item as WeaponSO).FiringRate.ToString();
                reloadSpeed.text = "Range : " + (item as WeaponSO).Range.ToString();
                clipSize.color = Color.clear;
            }
            else
            {
                fireRate.text = "Fire Rate : " + (item as WeaponSO).FiringRate.ToString();
                reloadSpeed.text = "Reload Speed : " + (item as WeaponSO).ReloadTime.ToString();
                clipSize.text = "Clip Size : " + (item as WeaponSO).MaxClip.ToString();
                clipSize.color = Color.white;
            }

            var tryAgain = true;
            while (tryAgain)
            {
                try
                {
                    //Do something with a gameObject
                    //If the Object was assigned
                    tryAgain = false;
                }
                catch (NullReferenceException e)
                {
                    //Assign gameObject
                }
            }
            

            for (int i = 0; i < strategies.Length; i++)
            {
                if (i < (item as WeaponSO).ProjectileStrategies.Count)
                {
                    strategies[i].color = Color.white;
                    strategies[i].text = (item as WeaponSO).ProjectileStrategies[i].StrategyDescription;
                }
                else
                {
                    strategies[i].color = Color.clear;
                    strategies[i].text = "Empty String";
                }
            }

            switch ((item as WeaponSO).ProjectileStrategies.Count)
            {
                case 2:
                    windowPanel.color = Color.blue;
                    break;
                case 3:
                    windowPanel.color = Color.yellow;
                    break;
                case 4:
                    windowPanel.color = Color.magenta;
                    break;
                default:
                    windowPanel.color = Color.white;
                    break;
            }
        }
        if (item is PickableSO)
        {
            statsPanel.SetActive(false);
            strategiesPanel.SetActive(false);
            // TODO: make the correct stuff appear
        }
    }

    public void FadeIn()
    {
        foreach (TMP_Text text in myTexts)
        {
            text.color = Color.white;
        }
        foreach (Image image in myImages)
        {
            image.color = Color.white;
        }
    }   
    
    public void FadeOut()
    {
        foreach (TMP_Text text in myTexts)
        {
            text.color = Color.clear;
        }
        foreach (Image image in myImages)
        {
            image.color = Color.clear;
        }
    }
}
