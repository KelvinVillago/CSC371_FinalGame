using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem.HID;
using static UnityEditor.Progress;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor;
using System.Reflection;

public enum Menu_Enum {Weapon = 0, Defense = 1, Animal = 2}
public class ShopManager : MonoBehaviour
{
    [Header("Shop Data Properties")]
    [SerializeField] private ItemsDatabaseSO _itemsDB;
    [SerializeField] private PlayerInventory _playerInventory;
    [Header("Shop UI Properties")]
    //[SerializeField] private GameObject _InventoryPanel;
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

    public event Action<bool> IsShopOpen;

    // Game UI canvas variables
    public GameObject gameCanvas;
    //public Button openShopBtn;
    public CoinCounter coinReference;
    public int coins;

    //public TMP_Text coinUI;

    // Weapon Shop Variables
    //public ShopItemSO[] shopItemsSO_Weapons;
    //public GameObject[] shopPanelsGO_Weapons;
    //public ShopTemplate[] shopPanels_Weapons;
    //public Button[] myPurchaseBtns_Weapons;
    //public TextMeshProUGUI[] buttonTexts_Weapons;
    //rivate bool[] isItemPurchased_Weapons;
    //private int curWeaponIndex = 0;

    // Defesne Shop Variables
    //public ShopItemSO[] shopItemsSO_Defenses;
    //public GameObject[] shopPanelsGO_Defenses;
    //public ShopTemplate[] shopPanels_Defenses;
    //public Button[] myPurchaseBtns_Defenses;
    //private bool[] isItemPurchased_Defenses;

    // Sheep Shop Variables
    //public ShopItemSO[] shopItemsSO_Sheeps;
    //public GameObject[] shopPanelsGO_Sheeps;
    //public ShopTemplate[] shopPanels_Sheeps;
    //public Button[] myPurchaseBtns_Sheeps;
    //private bool[] isItemPurchased_Sheeps;
    // Flags
    private bool initShop = true;
    //private bool IsLoaded_Animals, IsLoaded_Defenses, IsLoaded_Weapons = false;

    //List<Transform> shopCards = new List<Transform>();
    GameObject weaponMenu;
    GameObject animalMenu;
    GameObject defenseMenu;
    int weaponIndex = -1;
    int animalIndex = -1;
    int defenseIndex = -1;

    private void Awake()
    {
        _inventory = _playerInventory.Inventory;
    }
    
    private void initShopData()
    {
        //Turn off the inventory panels if they are open
        //_uiPanel.SetActive(false);
        //SetDeactive(_uiPanel);
        print("Starting Shop Manager");

        //Init the money
        coins = coinReference.Balance = 0;
        //coinUI.text = "Coins: " + coins.ToString();

        //Init the shop data
        _weaponItems = new List<Item>();
        _defenseItems = new List<Item>();
        _animalItems = new List<Item>();
        weaponMenu = _layoutHori_Weapons.gameObject;
        animalMenu = _layoutHori_Animals.gameObject;
        defenseMenu = _layoutHori_Defenses.gameObject;

        SetShopInventory();

        //Load the weapons and update the buttons for starting weapons.
        LoadPanels_Weapons();

        foreach (Transform child in _layoutHori_Weapons)
        {
            ShopTemplate shopCard = child.GetComponent<ShopTemplate>();
            print(shopCard.PositionInMenu);
            Item weaponItem = _weaponItems[shopCard.PositionInMenu];
            if (_inventory.getIndexByMatchItemSO(weaponItem) != -1)
            {
                shopCard.PurchasedButton();
                continue;
            }
        }

        //Load the panel into the menu
        SetButton_Purchasable();
        
        initShop = false;
    }

    private void SetShopInventory( )
    {
        weaponIndex = _weaponItems.Count;
        foreach (WeaponItemSO weapon in _itemsDB.WeaponsDB)
        {
            if (weapon.AvailableLevel == _level)
            {
                _weaponItems.Add(new Item(weapon, weapon.ShopQuantity));
            }
        }

        defenseIndex = _defenseItems.Count;
        foreach (DefenseItemSO defense in _itemsDB.DefenseDB)
        {
            if (defense.AvailableLevel == _level)
            {
                _defenseItems.Add(new Item(defense, defense.ShopQuantity));
            }
        }

        animalIndex = _animalItems.Count;
        foreach (AnimalItemSO animal in _itemsDB.AnimalDB)
        {
            if (animal.AvailableLevel == _level)
            {
                _animalItems.Add(new Item(animal, animal.ShopQuantity));
            }
        }
    }

    private void LoadItems()
    {
       LoadPanels_Weapons();
       LoadPanels_Defenses();
       LoadPanels_Animals();
    }

    public void LoadPanels_Weapons()
    {
        print("LoadPanels_Weapons");
        if(_layoutHori_Weapons == null)
        {
            print("layout is null");
        }
        //Load the card for each item
        for (; weaponIndex < _weaponItems.Count; weaponIndex++)
        {
            CustomizeCard(_weaponItems[weaponIndex], _layoutHori_Weapons);
        }
    }

    public void LoadPanels_Defenses()
    {
        //Load the card for each item
        for (; defenseIndex < _defenseItems.Count; defenseIndex++)
        {
            CustomizeCard(_defenseItems[defenseIndex], _layoutHori_Defenses);
        }
    }

    public void LoadPanels_Animals()
    {
        //Load the card for each item
        for (; animalIndex < _animalItems.Count; animalIndex++)
        {
            CustomizeCard(_animalItems[animalIndex], _layoutHori_Animals);
        }
    }

    private void CustomizeCard(Item item, Transform layout)
    {
        print("CustomizeCard");
        if (item == null)
            print("item is null");
        if (layout == null)
            print("layout is null");

        Transform shopCardUI = Instantiate(_itemTemplatePrefab, layout);
        if (shopCardUI == null)
            print("CardUI is null");
        ShopTemplate shopCard = shopCardUI.GetComponent<ShopTemplate>();
        if (shopCard == null)
            print("shopCard is null");

        //Save the location of this new object
        shopCard.PositionInMenu = layout.childCount - 1;
       
        //Title Txt
        shopCard.TitleText.text = item.itemSO.Name;
        //shopCard.TitleText.text ="word";
        //Description Txt
        shopCard.DescriptionText.text = item.itemSO.Description;
        //Cost
        shopCard.PriceText.text = item.GetItemSO<ShopItemSO2>().BuyPrice.ToString();
        //shopCard.SetPrice(item.GetItemSO<ShopItemSO2>().BuyPrice.ToString());
        //Button
        shopCard.ButtonText.text = "Buy";
        //Add button Listeners
        if (layout == _layoutHori_Weapons)
        {
            shopCard.Button.onClick.AddListener(() => Weapon_BuyButttonHandler(shopCard, item));
            print($"{shopCard.TitleText}{shopCard.PositionInMenu}{shopCard.PriceText}");
            print($"list{layout.name}: item:{_weaponItems[shopCard.PositionInMenu].itemSO.Name}");
        }
        else if (layout == _layoutHori_Defenses)
        {
            shopCard.Button.onClick.AddListener(() => Defense_BuyButttonHandler(item));
            print($"{shopCard.TitleText}{shopCard.PositionInMenu}{shopCard.PriceText}");
            print($"list{layout.name}: item:{_weaponItems[shopCard.PositionInMenu].itemSO.Name}");
        }
        else
        {
            shopCard.Button.onClick.AddListener(() => Animal_BuyButttonHandler(item));
            print($"{shopCard.TitleText}{shopCard.PositionInMenu}{shopCard.PriceText}");
            print($"list{layout.name}: item:{_weaponItems[shopCard.PositionInMenu].itemSO.Name}");
        }
        
    }
   
    private void SetButton_Purchasable()
    {
        print(MethodBase.GetCurrentMethod());
        Transform layout = null;
        List<Item> list = null;

        //Get the active menu
        Menu_Enum menu = GetActiveMenuName();
        switch (menu)
        {
            case Menu_Enum.Weapon:
                layout = _layoutHori_Weapons;
                list = _weaponItems;
                break;
            case Menu_Enum.Animal:
                layout = _layoutHori_Animals;
                list = _animalItems;
                break;
            case Menu_Enum.Defense:
                layout = _layoutHori_Defenses;
                list = _defenseItems;
                break;
        }

        ;
        //Update the buttons for the active menu
        foreach (Transform child in layout)
        {
            ShopTemplate shopCard = child.GetComponent<ShopTemplate>();
            Item item = list[shopCard.PositionInMenu];
            ShopItemSO2 shopItem = item.GetItemSO<ShopItemSO2>();

            print($"layout: {layout.name}, Child: {child.name},\n" +
                    $"ShopCard: {shopCard.name}, Position: {shopCard.PositionInMenu}\n" +
                    $"Item at Index {item} ItemsSO: {shopItem.Name} ItemPrice: {shopItem.BuyPrice}");

            if (menu == Menu_Enum.Weapon)
            {
                if(shopCard.ButtonText.text == "Purchased")
                {
                    //Already purchased.
                    continue;
                }
            }
            if (coins >= shopItem.BuyPrice)
            {
                print($"Coins: {coins}, BuyPrice: {shopItem.BuyPrice}");
                shopCard.Button.interactable = true;
            }
            else
            {
                shopCard.Button.interactable = false;
            }
        }
    }

    public void UpdateCoins(int amount)
    {
        //Update the coins
        //coins += amount;
        //coinUI.text = "Coins: " + coins.ToString();
        coins = coinReference.ChangeBalance(amount);
        print($"UpdateCoins: Balance = {coins}");
        //Update the buttons
        SetButton_Purchasable();
    }

    private void HideAllMenus()
    {
        print("HideAllMenus");
        print("deactivating: weapon");
        SetDeactive(weaponMenu);
        print("deactivating: defense");
        SetDeactive(defenseMenu);
        print("deactivating: animal");
        SetDeactive(animalMenu);
    }

    private Menu_Enum GetActiveMenuName()
    {
        print("GetActiveMenuName");
        if (weaponMenu.activeInHierarchy)
            return Menu_Enum.Weapon;
        if (animalMenu.activeInHierarchy)
            return Menu_Enum.Animal;
        if (defenseMenu.activeInHierarchy)
            return Menu_Enum.Defense;
        Debug.LogError("No menu was active: Using Default");
        return 0;
    }

    //Methods for checking
    public void SetDeactive(GameObject obj)
    {
        if (obj == null)
            print("null obj");
        if (!gameObject.activeInHierarchy)
            print($"{obj}not active in scene");
        if (obj.activeInHierarchy)
            obj.SetActive(false);
    }

    public void SetActive(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(true);
        else
            print("null obj");
    }


    //------------------------------------Called Through Buttons-------------//
    private void Weapon_BuyButttonHandler(ShopTemplate shopCard, Item item)
    {
        print($"Buy button clicked for item {item.itemSO.Name}");
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;
        if (coins >= buyPrice)
        {
            //Subtract the cost from the coins.
            UpdateCoins(-buyPrice);
        }

        //Remove one from the shops inventory
        item.amount--;

        //Buying adds one to inventory.
        Item boughtItem = new Item(item.itemSO, 1);

        //inventory and UI take care of updating the inventory.
        _inventory.AddItem(boughtItem);

        //Update shop button
        shopCard.PurchasedButton();
    }

    private void Defense_BuyButttonHandler(Item item)
    {
        print($"Buy button clicked for item {item.itemSO.Name}");
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;
        //Check if it can be purchased.
        if (coins >= buyPrice)
        {
            //Subtract the cost from the coins.
            UpdateCoins(-buyPrice);
        }

        //Remove one from the shops inventory
        item.amount--;

        //Buying adds one to inventory.
        Item boughtItem = new Item(item.itemSO, 1);

        //inventory and UI take care of updating the inventory.
        _inventory.AddItem(boughtItem);
    }

    private void Animal_BuyButttonHandler(Item item)
    {
        print($"Buy button clicked for item {item.itemSO.Name}");
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;

        //Check if it can be purchased.
        if (coins >= buyPrice)
        {
            //Subtract the cost from the coins.
            UpdateCoins(-buyPrice);
            GameManager.Instance.AddLives(1);
        }

        //Remove one from the shops inventory
        item.amount--;

        //Buying adds one to inventory.
        Item boughtItem = new Item(item.itemSO, 1);

        //inventory and UI take care of updating the inventory.
        _inventory.AddItem(boughtItem);
    }

    public void OpenMenu_Handler(int menu)
    {
        IsShopOpen?.Invoke(true);
        print("Open menu Handler");
        //Hide all the menus to prevent overlapping
        HideAllMenus();
        //Set the layout
        switch (menu)
        {
            case (int)Menu_Enum.Weapon:
                print("activating: weapon");
                weaponMenu.SetActive(true);
                LoadPanels_Weapons();
                break;
            case (int)Menu_Enum.Defense:
                print("activating: defense");
                defenseMenu.SetActive(true);
                LoadPanels_Defenses();
                break;
            case (int)Menu_Enum.Animal:
                print("activating: animal");
                animalMenu.SetActive(true);
                LoadPanels_Animals();
                break;
        }

        //Load the panels into the menu
        SetButton_Purchasable();
    }

    public void OpenShop_Handler()
    {
        print("Opening Shop");
   
        //Pause the game
        Time.timeScale = 0;

        //Turn off the inventory & Game panels if they are open
        //_uiPanel.SetActive(false);
        //print("deactavating: ui");
        //SetDeactive(_InventoryPanel);
        //print("deactavating: gameCanvas");
        //SetDeactive(gameCanvas);

        //Turn on the ShopUI
        //gameObject.SetActive(true);

        //Start script only runs once, when its activated for the first time.
        //But after openshop since its a button handler Start runs after. :/
        print("activating: gameObj");
        SetActive(gameObject);
        if (initShop == true)
        {
            initShopData();
            return;
        }

        //Load Items into the menu
        OpenMenu_Handler((int)Menu_Enum.Weapon);
     
        //if (gameCanvas != null)
        //{
        //    gameCanvas.SetActive(false);
        //}
    }

    public void CloseShop_Handler()
    {
        print("Closing shop");
        HideAllMenus();
        //gameObject.SetActive(false);
        //print("deactavating: gameObject");
        SetDeactive(gameObject);
        //UnPause the game
        Time.timeScale = 1;
        IsShopOpen?.Invoke(false);
        //Activate UI
        //print("activating: gameCanvas");
        //SetActive(gameCanvas);
        //gameCanvas.SetActive(true);
        //print("activating:  _uiPanel");
        //SetActive(_InventoryPanel);
        //_uiPanel.SetActive(true);
        //if (gameCanvas != null) 
        //{
        //    gameCanvas.SetActive(true); 
        //}
    }


    /*Not using these functions rn ------------------------------------------------------*/
    /*
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

    public void OpenWeaponMenu()
    {
        // Hide Defenses
        _layoutHori_Defenses.gameObject.SetActive(false);
        // Show Animal
        _layoutHori_Animals.gameObject.SetActive(false);
        // Show Weapons
        _layoutHori_Weapons.gameObject.SetActive(true);

        LoadPanels_Weapons();
        SetButton_Purchasable(Menu_Enum.Animal);
    }

    public void OpenDefenseMenu()
    {
        // Hide Weapons
        _layoutHori_Weapons.gameObject.SetActive(true);
        // Hide Animal
        _layoutHori_Animals.gameObject.SetActive(false);
        // Show Defenses
        _layoutHori_Defenses.gameObject.SetActive(false);
        LoadPanels_Defenses();
        SetButton_Purchasable(Menu_Enum.Animal);
    }

    public void OpenAnimalMenu()
    {
        // Hide Weapons
        _layoutHori_Weapons.gameObject.SetActive(true);

        // Hide Defenses
        _layoutHori_Defenses.gameObject.SetActive(false);

        // Show Animal
        _layoutHori_Animals.gameObject.SetActive(false);

        LoadPanels_Animals();
        SetButton_Purchasable(Menu_Enum.Animal);
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
    */


}
