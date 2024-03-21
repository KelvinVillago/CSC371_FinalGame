using System.Collections.Generic;
using UnityEngine;
using System;

public enum Menu_Enum {Weapon, Defense, Animal}
public class ShopManager : MonoBehaviour
{
    //Events for other scripts to listen to
    [HideInInspector] public event Action<bool> IsShopOpen;

    [Header("Shop Data Properties")]
    [SerializeField] private ItemsDatabaseSO _itemsDB;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private CoinCounter _coinReference;

    [Header("Shop UI Properties")]
    [SerializeField] private Transform _layoutHori_Weapons;
    [SerializeField] private Transform _layoutHori_Defenses;
    [SerializeField] private Transform _layoutHori_Animals;
    [SerializeField] private Transform _itemTemplatePrefab;
    [SerializeField] private GameObject _gameCanvas;

    //Data
    private Inventory _inventory;
    //Weapon vars
    private List<Item> _weaponItems;
    private GameObject _weaponMenu;
    private int _weaponIndex;
    //Defense vars
    private List<Item> _defenseItems;
    private GameObject _defenseMenu;
    private int _defenseIndex;
    //Animal vars
    private List<Item> _animalItems;
    private GameObject _animalMenu;
    private int _animalIndex;
    //Shop vars
    private bool _initShop = true;
    private int _level = 1;
    private int _coins;

    private void Awake()
    {
        _inventory = _playerInventory.Inventory;
    }

    private void InitShopData()
    {
        //Init the money
        _coins = _coinReference.GetBalance();
        print("Coins: " + _coins.ToString());

        //Init the shop data
        _weaponItems = new List<Item>();
        _defenseItems = new List<Item>();
        _animalItems = new List<Item>();
        _weaponMenu = _layoutHori_Weapons.gameObject;
        _animalMenu = _layoutHori_Animals.gameObject;
        _defenseMenu = _layoutHori_Defenses.gameObject;

        SetShopInventory();

        //Load the weapons and update the buttons for starting weapons.
        LoadPanels_Weapons();

        foreach (Transform child in _layoutHori_Weapons)
        {
            ShopTemplate shopCard = child.GetComponent<ShopTemplate>();
            Item weaponItem = _weaponItems[shopCard.PositionInMenu];
            if (_inventory.getIndexByMatchItemSO(weaponItem) != -1)
            {
                shopCard.PurchasedButton();
                continue;
            }
        }

        //Load the panel into the menu
        SetButton_Purchasable();

        _initShop = false;
    }

    private void SetShopInventory( )
    {
        _weaponIndex = _weaponItems.Count;
        foreach (WeaponItemSO weapon in _itemsDB.WeaponsDB)
        {
            if (weapon.AvailableLevel == _level)
            {
                _weaponItems.Add(new Item(weapon, weapon.ShopQuantity));
            }
        }

        _defenseIndex = _defenseItems.Count;
        foreach (DefenseItemSO defense in _itemsDB.DefenseDB)
        {
            if (defense.AvailableLevel == _level)
            {
                _defenseItems.Add(new Item(defense, defense.ShopQuantity));
            }
        }

        _animalIndex = _animalItems.Count;
        foreach (AnimalItemSO animal in _itemsDB.AnimalDB)
        {
            if (animal.AvailableLevel == _level)
            {
                _animalItems.Add(new Item(animal, animal.ShopQuantity));
            }
        }
    }

    public void LoadPanels_Weapons()
    {
        //Load the card for each item
        for (; _weaponIndex < _weaponItems.Count; _weaponIndex++)
        {
            CustomizeCard(_weaponItems[_weaponIndex], _layoutHori_Weapons);
        }
    }

    public void LoadPanels_Defenses()
    {
        //Load the card for each item
        for (; _defenseIndex < _defenseItems.Count; _defenseIndex++)
        {
            CustomizeCard(_defenseItems[_defenseIndex], _layoutHori_Defenses);
        }
    }

    public void LoadPanels_Animals()
    {
        //Load the card for each item
        for (; _animalIndex < _animalItems.Count; _animalIndex++)
        {
            CustomizeCard(_animalItems[_animalIndex], _layoutHori_Animals);
        }
    }

    private void CustomizeCard(Item item, Transform layout)
    {
        //Create the card
        Transform shopCardUI = Instantiate(_itemTemplatePrefab, layout);

        //Get the fields to customize
        ShopTemplate shopCard = shopCardUI.GetComponent<ShopTemplate>();

        //Save the location of this new object
        shopCard.PositionInMenu = layout.childCount - 1;
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

    private void SetButton_Purchasable()
    {
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

        foreach (Transform child in layout)
        {
            ShopTemplate shopCard = child.GetComponent<ShopTemplate>();

            //Dont activate the weapon if its already puruchased.
            if (menu == Menu_Enum.Weapon)
            {
                if(shopCard.ButtonText.text == "Purchased")
                {
                    //Already purchased.
                    continue;
                }
            }
            
            print("Coins: " + _coins.ToString());
            //Check the ShopItem Buy price for the item at the index
            if (_coins >= list[shopCard.PositionInMenu].GetItemSO<ShopItemSO2>().BuyPrice)
            {
                shopCard.Button.interactable = true;
            }
            else
            {
                shopCard.Button.interactable = false;
            }
        }
    }

    private void HideAllMenus()
    {
        SetDeactive(_weaponMenu);
        SetDeactive(_defenseMenu);
        SetDeactive(_animalMenu);
    }

    private Menu_Enum GetActiveMenuName()
    {
        if (_weaponMenu.activeInHierarchy)
            return Menu_Enum.Weapon;
        if (_animalMenu.activeInHierarchy)
            return Menu_Enum.Animal;
        if (_defenseMenu.activeInHierarchy)
            return Menu_Enum.Defense;
        //Default menu is WeaponMenu
        return 0;
    }

    //-------------------------------Methods for checking----------------------//
    public void SetDeactive(GameObject obj)
    {
        if (obj.activeInHierarchy)
            obj.SetActive(false);
    }

    public void SetActive(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(true);
        else
            print($"{obj.name} == null");
    }


    //------------------------------------Called Through Buttons-------------//
    public void UpdateCoins(int amount)
    {
        //Update the coins
        _coins = _coinReference.ChangeBalance(amount);

        //Update the buttons
        SetButton_Purchasable();
    }

    private void Weapon_BuyButttonHandler(ShopTemplate shopCard, Item item)
    {
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;
        if (_coins >= buyPrice)
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
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;
        //Check if it can be purchased.
        if (_coins >= buyPrice)
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
        int buyPrice = item.GetItemSO<ShopItemSO2>().BuyPrice;

        //Check if it can be purchased.
        if (_coins >= buyPrice)
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
        _coins = _coinReference.GetBalance();
        IsShopOpen?.Invoke(true);

        //Hide all the menus to prevent overlapping
        HideAllMenus();

        //Set the layout
        switch (menu)
        {
            case (int)Menu_Enum.Weapon:
                _weaponMenu.SetActive(true);
                LoadPanels_Weapons();
                break;
            case (int)Menu_Enum.Defense:
                _defenseMenu.SetActive(true);
                LoadPanels_Defenses();
                break;
            case (int)Menu_Enum.Animal:
                _animalMenu.SetActive(true);
                LoadPanels_Animals();
                break;
        }

        //Load the panels into the menu
        SetButton_Purchasable();
    }

    public void OpenShop_Handler()
    {
        //Pause the game
        Time.timeScale = 0;

        //Activate Shop window
        SetActive(gameObject);

        //Init the data if needed
        if (_initShop == true)
        {
            InitShopData();
            return;
        }

        //Load menu
        OpenMenu_Handler((int)Menu_Enum.Weapon);
    }

    public void CloseShop_Handler()
    {
        //Hide all panels - Does not distroy the menu items...
        HideAllMenus();
        SetDeactive(gameObject);

        //UnPause the game
        Time.timeScale = 1;

        //Sends an alert, if a script is listening it can do stuff.
        IsShopOpen?.Invoke(false);
    }
}
