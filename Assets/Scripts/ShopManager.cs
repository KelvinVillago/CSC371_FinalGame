using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopManager : MonoBehaviour
{
    // Open Shop Button
    public Button openShopBtn;
    
    // Coin Variables
    public int coins;
    public TMP_Text coinUI;

    // Weapon Shop Variables
    public ShopItemSO[] shopItemsSO_Weapons;
    public GameObject[] shopPanelsGO_Weapons;
    public ShopTemplate[] shopPanels_Weapons;
    public Button[] myPurchaseBtns_Weapons;

    // Defesne Shop Variables

    public ShopItemSO[] shopItemsSO_Defenses;
    public GameObject[] shopPanelsGO_Defenses;
    public ShopTemplate[] shopPanels_Defenses;
    public Button[] myPurchaseBtns_Defenses;

    // Sheep Shop Variables
    public ShopItemSO[] shopItemsSO_Sheeps;
    public GameObject[] shopPanelsGO_Sheeps;
    public ShopTemplate[] shopPanels_Sheeps;
    public Button[] myPurchaseBtns_Sheeps;

    void Start()
    {
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanelsGO_Weapons[i].SetActive(true);
        }
        coinUI.text = "Coins: " + coins.ToString();
        LoadPanels_Weapons();
        CheckPurchaseable_Weapons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins()
    {
        coins += 50;
        coinUI.text = "Coins: " + coins.ToString();
        CheckPurchaseable_Weapons();
        CheckPurchaseable_Defenses();
        CheckPurchaseable_Sheeps();
    }

    public void CheckPurchaseable_Weapons()
    {
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            if (coins >= shopItemsSO_Weapons[i].price)
            {
                myPurchaseBtns_Weapons[i].interactable = true;
            }
            else
            {
                myPurchaseBtns_Weapons[i].interactable = false;
            }
        }
    }

    public void CheckPurchaseable_Defenses()
    {
        for (int i = 0; i < shopItemsSO_Defenses.Length; i++)
        {
            if (coins >= shopItemsSO_Defenses[i].price)
            {
                myPurchaseBtns_Defenses[i].interactable = true;
            }
            else
            {
                myPurchaseBtns_Defenses[i].interactable = false;
            }
        }
    }

    public void CheckPurchaseable_Sheeps()
    {
        for (int i = 0; i < shopItemsSO_Sheeps.Length; i++)
        {
            if (coins >= shopItemsSO_Sheeps[i].price)
            {
                myPurchaseBtns_Sheeps[i].interactable = true;
            }
            else
            {
                myPurchaseBtns_Sheeps[i].interactable = false;
            }
        }
    }

    public void PurchaseItem_Weapons(int btnNo)
    {
        if (coins >= shopItemsSO_Weapons[btnNo].price)
        {
            coins -= shopItemsSO_Weapons[btnNo].price;
            coinUI.text = "Coins: " + coins.ToString();
            CheckPurchaseable_Weapons();
            //unlock item
        }
    }
    
    public void PurchaseItem_Defenses(int btnNo)
    {
        if (coins >= shopItemsSO_Defenses[btnNo].price)
        {
            coins -= shopItemsSO_Defenses[btnNo].price;
            coinUI.text = "Coins: " + coins.ToString();
            CheckPurchaseable_Defenses();
            //unlock item
        }
    }

    public void PurchaseItem_Sheeps(int btnNo)
    {
        if (coins >= shopItemsSO_Sheeps[btnNo].price)
        {
            coins -= shopItemsSO_Sheeps[btnNo].price;
            coinUI.text = "Coins: " + coins.ToString();
            CheckPurchaseable_Sheeps();
            //unlock item
        }
    }

    public void LoadPanels_Weapons()
    {
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanels_Weapons[i].titleText.text = shopItemsSO_Weapons[i].title;
            shopPanels_Weapons[i].descriptionText.text = shopItemsSO_Weapons[i].description;
            shopPanels_Weapons[i].priceText.text = "Coins: " + shopItemsSO_Weapons[i].price.ToString();
        }
    }

    public void LoadPanels_Defenses()
    {
        for (int i = 0; i < shopItemsSO_Defenses.Length; i++)
        {
            shopPanels_Defenses[i].titleText.text = shopItemsSO_Defenses[i].title;
            shopPanels_Defenses[i].descriptionText.text = shopItemsSO_Defenses[i].description;
            shopPanels_Defenses[i].priceText.text = "Coins: " + shopItemsSO_Defenses[i].price.ToString();
        }
    }

    public void LoadPanels_Sheeps()
    {
        for (int i = 0; i < shopItemsSO_Sheeps.Length; i++)
        {
            shopPanels_Sheeps[i].titleText.text = shopItemsSO_Sheeps[i].title;
            shopPanels_Sheeps[i].descriptionText.text = shopItemsSO_Sheeps[i].description;
            shopPanels_Sheeps[i].priceText.text = "Coins: " + shopItemsSO_Sheeps[i].price.ToString();
        }
    }

    public void OpenWeaponShop()
    {
        // Hide Defenses
        for (int i = 0; i < shopItemsSO_Defenses.Length; i++)
        {
            shopPanelsGO_Defenses[i].SetActive(false);
        }
        // Hide Sheep
        for (int i = 0; i < shopItemsSO_Sheeps.Length; i++)
        {
            shopPanelsGO_Sheeps[i].SetActive(false);
        }
        // Show Weapons
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanelsGO_Weapons[i].SetActive(true);
        }
        LoadPanels_Weapons();
        CheckPurchaseable_Weapons();
    }

    public void OpenDefenseShop()
    {
        // Hide Weapons
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanelsGO_Weapons[i].SetActive(false);
        }
        // Hide Sheep
        for (int i = 0; i < shopItemsSO_Sheeps.Length; i++)
        {
            shopPanelsGO_Sheeps[i].SetActive(false);
        }
        // Show Defenses
        for (int i = 0; i < shopItemsSO_Defenses.Length; i++)
        {
            shopPanelsGO_Defenses[i].SetActive(true);
        }
        LoadPanels_Defenses();
        CheckPurchaseable_Defenses();
    }

    public void OpenSheepShop()
    {
        // Hide Weapons
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanelsGO_Weapons[i].SetActive(false);
        }
        // Hide Defenses
        for (int i = 0; i < shopItemsSO_Defenses.Length; i++)
        {
            shopPanelsGO_Defenses[i].SetActive(false);
        }
        // Show Defenses
        for (int i = 0; i < shopItemsSO_Sheeps.Length; i++)
        {
            shopPanelsGO_Sheeps[i].SetActive(true);
        }
        LoadPanels_Sheeps();
        CheckPurchaseable_Sheeps();
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
