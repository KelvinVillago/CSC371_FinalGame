using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopManager : MonoBehaviour
{
    // Purchase references
    public GameObject pistol;
    public GameObject AR;
    public GameObject SMG;
    public GameObject SR;
    public SelectionManager selectionManager;
    // Game UI canvas variables
    public GameObject gameCanvas;
    public Button openShopBtn;
    public CoinCounter coinReference;
    
    // Coin Variables
    public int coins;
    public TMP_Text coinUI;

    // Weapon Shop Variables
    public ShopItemSO[] shopItemsSO_Weapons;
    public GameObject[] shopPanelsGO_Weapons;
    public ShopTemplate[] shopPanels_Weapons;
    public Button[] myPurchaseBtns_Weapons;
    public TextMeshProUGUI[] buttonTexts_Weapons;
    private bool[] isItemPurchased_Weapons;
    private int curWeaponIndex = 0;

    // Defesne Shop Variables

    public ShopItemSO[] shopItemsSO_Defenses;
    public GameObject[] shopPanelsGO_Defenses;
    public ShopTemplate[] shopPanels_Defenses;
    public Button[] myPurchaseBtns_Defenses;
    private bool[] isItemPurchased_Defenses;

    // Sheep Shop Variables
    public ShopItemSO[] shopItemsSO_Sheeps;
    public GameObject[] shopPanelsGO_Sheeps;
    public ShopTemplate[] shopPanels_Sheeps;
    public Button[] myPurchaseBtns_Sheeps;
    private bool[] isItemPurchased_Sheeps;
    // Flags
    private bool firstTimeOpenShop = false;

    void Start()
    {
        coins = coinReference.num;
        isItemPurchased_Sheeps = new bool[9];
        isItemPurchased_Defenses = new bool[9];
        isItemPurchased_Weapons = new bool[9];
        isItemPurchased_Weapons[0] = true;
        Debug.Log("initailized");
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
            if (i == curWeaponIndex)
            {
                buttonTexts_Weapons[i].text = "Equipped";
                myPurchaseBtns_Weapons[i].interactable = false;
            }
            else if (isItemPurchased_Weapons[i])
            {
                buttonTexts_Weapons[i].text = "Equip";
                myPurchaseBtns_Weapons[i].interactable = true;
            }
            else if (coins >= shopItemsSO_Weapons[i].price)
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
        if (isItemPurchased_Weapons[btnNo] == true) //already bought item
        {
            curWeaponIndex = btnNo;
            CheckPurchaseable_Weapons();
        }
        else if (coins >= shopItemsSO_Weapons[btnNo].price) // buying item for first time now
        {
            coins -= shopItemsSO_Weapons[btnNo].price;
            coinUI.text = "Coins: " + coins.ToString();
            isItemPurchased_Weapons[btnNo] = true;
            curWeaponIndex = btnNo;
            CheckPurchaseable_Weapons();
        }
        //unlock item
        if (btnNo == 0)
        {
            DeactivateWeapons();
            pistol.SetActive(true);
            Debug.Log("Pistol active");
        }
        else if (btnNo == 1)
        {
            DeactivateWeapons();
            SMG.SetActive(true);
        }
        else if (btnNo == 2)
        {
            DeactivateWeapons();
            AR.SetActive(true);
        }
        else if (btnNo == 3)
        {
            DeactivateWeapons();
            SR.SetActive(true);
        }
    }

    public void DeactivateWeapons()
    {
        pistol.SetActive(false);
        AR.SetActive(false);
        SMG.SetActive(false);
    }
    
    public void PurchaseItem_Defenses(int btnNo)
    {
        if (coins >= shopItemsSO_Defenses[btnNo].price)
        {
            coins -= shopItemsSO_Defenses[btnNo].price;
            coinUI.text = "Coins: " + coins.ToString();
            CheckPurchaseable_Defenses();
            //unlock item
            if (btnNo == 0)
            {
                //unlock item 1
                selectionManager.fenceCount += 1;
            }
            else if (btnNo == 1)
            {
                selectionManager.smallTurretCount += 1;
                // OpenShop();
            }
            else if (btnNo == 2)
            {
                selectionManager.largeTurretCount += 1;
            }
        }
    }

    public void PurchaseItem_Sheeps(int btnNo)
    {
        if (coins >= shopItemsSO_Sheeps[btnNo].price)
        {
            coins -= shopItemsSO_Sheeps[btnNo].price;
            coinUI.text = "Coins: " + coins.ToString();
            GameManager.Instance.AddLives(1);
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
        if (gameCanvas != null) 
        {
            gameCanvas.SetActive(true); 
        }
    }

    public void OpenShop()
    {
        if (!firstTimeOpenShop)
        {
            Start();
            firstTimeOpenShop = true;
        }
        coins = coinReference.num;
        coinUI.text = "Coins: " + coins.ToString();
        CheckPurchaseable_Weapons();
        CheckPurchaseable_Defenses();
        CheckPurchaseable_Sheeps();
        gameObject.SetActive(true);
        Time.timeScale = 0;
        if (gameCanvas != null) 
        {
            gameCanvas.SetActive(false); 
        }
    }
}
