using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem.HID;
using static UnityEditor.Progress;
using UnityEditor.ShaderGraph.Internal;

public enum Menu_Enum {Weapon = 0, Defense = 1, Animal = 2}
public class ShopManager : MonoBehaviour
{
    [Header("Shop Data Properties")]
    [SerializeField] private ItemsDatabaseSO _itemsDB;
    [SerializeField] private PlayerInventory _playerInventory;
    [Header("Shop UI Properties")]
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private Transform _layoutHori_Weapons;
    [SerializeField] private Transform _layoutHori_Defenses;
    [SerializeField] private Transform _layoutHori_Animals;
    [SerializeField] private Transform _itemTemplatePrefab;

    public SelectionManager selectionManager;
    private Inventory _inventory;
    private List<Item> _weaponItems;
    private List<Item> _defenseItems;
    private List<Item> _animalItems;
    private int _level = 1;
   
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
    private bool IsLoaded_Animals, IsLoaded_Defenses, IsLoaded_Weapons = false;

    List<Transform> shopCards = new List<Transform>();
    private void Awake()
    {
        _inventory = _playerInventory.Inventory;
    }
    void Start()
    {
        //Turn off the inventory panels if they are open
        _uiPanel.SetActive(false);
        coins = coinReference.num;
        /*
        isItemPurchased_Sheeps = new bool[9];
        isItemPurchased_Defenses = new bool[9];
        isItemPurchased_Weapons = new bool[9];
        isItemPurchased_Weapons[0] = true;

        Debug.Log("initailized");

        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanelsGO_Weapons[i].SetActive(true);
        }
        */
        coinUI.text = "Coins: " + coins.ToString();
        /*
        LoadPanels_Weapons();
        CheckPurchaseable_Weapons();
        */
        //Dreas Testing Database
        _weaponItems = new List<Item>();
        _defenseItems = new List<Item>();
        _animalItems = new List<Item>();
        SetUpShopInventory();
    }

    private void SetUpShopInventory()
    {
        foreach (WeaponItemSO weapon in _itemsDB.WeaponsDB)
        {
            if (weapon.AvailableLevel == 0 || weapon.AvailableLevel > _level)
            {
                //Item is not ready to be added to the shop skip it. 
                continue;
            }
            _weaponItems.Add(new Item(weapon, weapon.ShopQuantity));
        }

        foreach (DefenseItemSO defense in _itemsDB.DefenseDB)
        {
            if (defense.AvailableLevel == 0 || defense.AvailableLevel > _level)
            {
                //Item is not ready to be added to the shop skip it. 
                continue;
            }
            _defenseItems.Add(new Item(defense, defense.ShopQuantity));
        }

        foreach (AnimalItemSO animal in _itemsDB.AnimalDB)
        {
            if (animal.AvailableLevel == 0 || animal.AvailableLevel > _level)
            {
                //Item is not ready to be added to the shop skip it. 
                continue;
            }
            _animalItems.Add(new Item(animal, animal.ShopQuantity));
        }
    }

    public void OpenShop()
    {
        //Turn off the inventory panels if they are open
        _uiPanel.SetActive(false);

        if (!firstTimeOpenShop)
        {
            Start();
            firstTimeOpenShop = true;
        }

        coins = coinReference.num;
        coinUI.text = "Coins: " + coins.ToString();
        //Load shop icons
        LoadItems(Menu_Enum.Weapon);
        /*
        CheckPurchaseable_Weapons();
        CheckPurchaseable_Defenses();
        CheckPurchaseable_Sheeps();
        */
        gameObject.SetActive(true);
        Time.timeScale = 0;
        if (gameCanvas != null)
        {
            gameCanvas.SetActive(false);
        }
    }

    public void SelectMenu(int num)
    {
        //Button on click wont accept enums
        LoadItems((Menu_Enum)num);
    }

    private void LoadItems(Menu_Enum menu)
    {
        List<Item> itemList = null;
        Transform layout = null;
        Type menuType = null;

        switch (menu)
        {
            case Menu_Enum.Weapon:
                _layoutHori_Weapons.gameObject.SetActive(true);
                _layoutHori_Animals.gameObject.SetActive(false);
                _layoutHori_Defenses.gameObject.SetActive(false);
                //Ignore if its alerady loaded.
                if (IsLoaded_Weapons)
                {
                    //Turn off the button if it is to broke
                    SetButton_Purchasable(menu);
                    return;
                }
                itemList = _weaponItems;
                layout = _layoutHori_Weapons;
                menuType = typeof(WeaponItemSO);
                IsLoaded_Weapons = true;
                break;
            case Menu_Enum.Animal:
                _layoutHori_Animals.gameObject.SetActive(true);
                _layoutHori_Weapons.gameObject.SetActive(false);
                _layoutHori_Defenses.gameObject.SetActive(false);
                if (IsLoaded_Animals)
                {
                    //Turn off the button if it is to broke
                    SetButton_Purchasable(menu);
                    return;
                }
                itemList = _animalItems;
                layout = _layoutHori_Animals;
                menuType = typeof(AnimalItemSO);
                IsLoaded_Animals = true;
                break;
            case Menu_Enum.Defense:
                _layoutHori_Defenses.gameObject.SetActive(true);
                _layoutHori_Animals.gameObject.SetActive(false);
                _layoutHori_Weapons.gameObject.SetActive(false);
                if (IsLoaded_Defenses)
                {
                    //Turn off the button if it is to broke
                    SetButton_Purchasable(menu);
                    return;
                }
                itemList = _defenseItems;
                layout = _layoutHori_Defenses;
                menuType = typeof(DefenseItemSO);
                IsLoaded_Defenses = true;
                break;
        }

        if (itemList == null || layout == null)
        {
            Debug.LogError("itemlist or layout was not set correctly");
        }

        //Build the card for each item
        foreach (Item item in itemList)
        {
            CustomizeCard(item, layout);
        }
    }

    private void CustomizeCard(Item item, Transform layout)
    {
        Transform shopCardUI = Instantiate(_itemTemplatePrefab, layout);
        ShopTemplate shopCard = shopCardUI.GetComponent<ShopTemplate>();

        //Save the location of this new object
        shopCard.PositionInMenu = layout.childCount;
        //Title Txt
        shopCard.TitleText.text = item.itemSO.Name;
        //Description Txt
        shopCard.DescriptionText.text = item.itemSO.Description;
        //Cost
        shopCard.PriceText.text = item.GetItemSO<ShopItemSO2>().BuyPrice.ToString();
        //Button
        shopCard.ButtonText.text = "Buy";
        //Add button Listeners
        if (layout == _layoutHori_Weapons)
        {
            shopCard.Button.onClick.AddListener(() => Weapon_BuyButttonHandler(shopCard, item));
        }
        else if (layout == _layoutHori_Defenses)
        {
            shopCard.Button.onClick.AddListener(() => Defense_BuyButttonHandler(item));
        }
        else
        {
            shopCard.Button.onClick.AddListener(() => Animal_BuyButttonHandler(item));
        }
    }

    private int CheckPlayerInventory(Item item)
    {
        int index = -1;
        //Get the players Inventory
        List<Item> inventory = _inventory.GetItemsList();

        //Look for the item
        index = _inventory.GetItemsList().FindIndex(data => data.itemSO == item.itemSO);
        if (index < 0)
        {
            Debug.LogError($"No matching SO found: {item.itemSO}");
        }
        return index;
    }

    private void Weapon_BuyButttonHandler(ShopTemplate shopCard, Item item)
    {
        print($"Buy button clicked for item {item.itemSO.Name}");
        //Remove one from the shops inventory
        item.amount--;

        //Buying adds one to inventory.
        Item boughtItem = new Item(item.itemSO, 1);

        //inventory and UI take care of updating the inventory.
        _inventory.AddItem(item);

        //Update shop button
        shopCard.PurchasedButton();
    }

    private void Defense_BuyButttonHandler(Item item)
    {
        print($"Buy button clicked for item {item.itemSO.Name}");
        //Remove one from the shops inventory
        item.amount--;

        //Buying adds one to inventory.
        Item boughtItem = new Item(item.itemSO, 1);
        
        //inventory and UI take care of updating the inventory.
        _inventory.AddItem(item);
    }

    private void Animal_BuyButttonHandler(Item item)
    {
        print($"Buy button clicked for item {item.itemSO.Name}");
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;

        //Check if it can be purchased.
        if (coins >= buyPrice)
        {
            coins -= buyPrice;
            coinUI.text = "Coins: " + coins.ToString();
            GameManager.Instance.AddLives(1);
            SetButton_Purchasable(Menu_Enum.Animal);
        }

        //Remove one from the shops inventory
        item.amount--;

        //Buying adds one to inventory.
        Item boughtItem = new Item(item.itemSO, 1);

        //inventory and UI take care of updating the inventory.
        _inventory.AddItem(item);
    }



    private void SetButton_Purchasable(Menu_Enum menu)
    {
        Transform layout = null;
        List<Item> list = null;

        switch (menu)
        {
            case Menu_Enum.Weapon:
                layout = _layoutHori_Weapons;
                list = _weaponItems;
                break;
            case Menu_Enum.Animal:
                layout = _layoutHori_Animals;
                list = _weaponItems;
                break;
            case Menu_Enum.Defense:
                layout = _layoutHori_Defenses;
                list = _weaponItems;
                break;
        }

        foreach(Transform child in layout)
        {
            ShopTemplate shopCard = child.GetComponent<ShopTemplate>();
            Item item = list[shopCard.PositionInMenu];
            ShopItemSO2 shopItem = item.GetItemSO<ShopItemSO2>();

            if (menu == Menu_Enum.Weapon)
            {
                if(shopCard.Button.interactable == false)
                {
                    //Already purchased.
                    continue;
                }

                //if the weapon is already in the inventory change the button
                if (CheckPlayerInventory(list[shopCard.PositionInMenu]) != -1)
                {
                    shopCard.PurchasedButton();
                    continue;
                }
            }
            if (coins >= shopItem.BuyPrice)
            {
                shopCard.Button.interactable = true;
            }
            else
            {
                shopCard.Button.interactable = true;
            }
        }
    }

    public void AddCoins()
    {
        coins += 50;
        coinUI.text = "Coins: " + coins.ToString();
        SetButton_Purchasable(Menu_Enum.Weapon);
        SetButton_Purchasable(Menu_Enum.Defense);
        SetButton_Purchasable(Menu_Enum.Animal);
    }



    public void LoadPanels_Weapons()
    {
        for (int i = 0; i < shopItemsSO_Weapons.Length; i++)
        {
            shopPanels_Weapons[i].TitleText.text = shopItemsSO_Weapons[i].title;
            shopPanels_Weapons[i].DescriptionText.text = shopItemsSO_Weapons[i].description;
            shopPanels_Weapons[i].PriceText.text = "Coins: " + shopItemsSO_Weapons[i].price.ToString();
        }
    }

    public void LoadPanels_Defenses()
    {
        for (int i = 0; i < shopItemsSO_Defenses.Length; i++)
        {
            shopPanels_Defenses[i].TitleText.text = shopItemsSO_Defenses[i].title;
            shopPanels_Defenses[i].DescriptionText.text = shopItemsSO_Defenses[i].description;
            shopPanels_Defenses[i].PriceText.text = "Coins: " + shopItemsSO_Defenses[i].price.ToString();
        }
    }

    public void LoadPanels_Sheeps()
    {
        for (int i = 0; i < shopItemsSO_Sheeps.Length; i++)
        {
            shopPanels_Sheeps[i].TitleText.text = shopItemsSO_Sheeps[i].title;
            shopPanels_Sheeps[i].DescriptionText.text = shopItemsSO_Sheeps[i].description;
            shopPanels_Sheeps[i].PriceText.text = "Coins: " + shopItemsSO_Sheeps[i].price.ToString();
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

    public void OpenAnimalMenu()
    {
        // Hide Weapons

        // Hide Defenses

        // Show Defenses

        LoadPanels_Sheeps();
        SetButton_Purchasable();
    }

    public void CloseShop()
    {
        //Desolve the shop
        DestroyMenus();
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if (gameCanvas != null) 
        {
            gameCanvas.SetActive(true); 
        }
        _uiPanel.SetActive(true);
    }
    private void DestroyMenus()
    {
        //Reset Everything
        foreach (Transform child in _layoutHori_Weapons)
        {
            Destroy(child.gameObject);
        }
       
        foreach (Transform child in _layoutHori_Defenses)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _layoutHori_Animals)
        {
            Destroy(child.gameObject);
        }
        IsLoaded_Animals = false;
        IsLoaded_Defenses = false;
        IsLoaded_Weapons = false;
    }

    
}
