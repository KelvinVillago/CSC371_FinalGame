using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * The Inventory UI 
 * VertContainer starts with a HoriLayoutTemplate, 
 * HoriLayoutTemplate starts with a itemSlotTemplate
 * Both have dynamic layouts.
 * 
 * VertContainer  = |horiContainer| = max is none
 *                  |horiContainer|
 *                  |.............|
 * 
 * HoriContainer =  | ST ST ST ST ST| , max is 5
 */

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Transform _layoutContainerVert;
    [SerializeField] private Transform _layoutContainerHor;
    [SerializeField] private Transform _itemSlotTemplate;
    [SerializeField] private int _maxSlotCount;
    [SerializeField] private ItemsDatabaseSO _itemDB;
    [SerializeField] private Transform[] _hotKeySlots;
    [SerializeField] private Transform _scrollView;
    [SerializeField] private Sprite _defaultMissingSprite;
    private PreventClickDrag _scrollScript;
    private Item _item;
    private int _slotUsedCount = 0;
    //Get a refrence to the Start Placement function
    private Inventory _inventory;
    private PlacementSystem _placementSystem;
    private TextMeshProUGUI _amountText;

    private void Awake()
    {
        //Hide the templates on empty open.
        //_layoutContainerHor.gameObject.SetActive(false);
        _placementSystem = GameObject.Find("BuildingSystem").GetComponentInChildren<PlacementSystem>();
        _scrollScript = _scrollView.GetComponent<PreventClickDrag>();
    }
    private void Start()
    {
        disableScroll();
        /* Print everyting item in the database to the inventory.
        //Add the starter items, marked in the DB?
        CreateItemButton(_itemDB.AnimalDB[0]);
        foreach (var item in _itemDB.DefenseDB)
        {
            CreateItemButton(item);
        }
        foreach(var item in _itemDB.WeaponsDB)
        {
            CreateItemButton(item);
        }
        */
       
        //ItemWorld.SpawnItemWorld(new Item(_itemDB.MscItemsDB[0]), new Vector3(4, 1, 4));
        //ItemWorld.SpawnItemWorld(new Item(_itemDB.MscItemsDB[0]), new Vector3(10, 1, 4));
    }

    private void OnDisable()
    {
        _inventory.OnInventoryChanged -= OnInventoryChanged;
    }
    public void SetInventory(Inventory inventory)
    {
        //Starting inventory
        this._inventory = inventory;
        Item tempItem = new Item(_itemDB.DefenseDB[0]);
        inventory.AddItem(tempItem);
        inventory.AddItem(tempItem);
        inventory.AddItem(tempItem);
        inventory.AddItem(tempItem);
        inventory.AddItem(tempItem);
        inventory.AddItem(tempItem);
        //Build UI for starting inventory
        RefreshInventory();
        //Start listening for new changes
        inventory.OnInventoryChanged += OnInventoryChanged;
        
    }

    private void OnInventoryChanged(Item newItem, bool IsRemoving)
    {
        if (IsRemoving)
        {
            //RemoveButton
            print("Removing item" + newItem);
        }
        CreateItemButton(newItem);
    }

    private void RefreshInventory()
    {
        foreach (Item item in _inventory.GetItemsList())
        {
            CreateItemButton(item);
        }
    }

    private void CreateItemButton(Item item)
    {
        
        _item = item;
        int indexVert;
        Transform lastCol = null;
        Transform newItemSlot;
        int indexSlots;

        //Add weapon to weapon slots if avalible
        if (item.IsSameOrSubclass(typeof(WeaponItemSO)))
        {
            //Check if there is an open slot on the quickbar
            if (_slotUsedCount < _maxSlotCount)
            {
                for (indexSlots = 0; indexSlots < _maxSlotCount; indexSlots++)
                {
                    if (_hotKeySlots[indexSlots].Find("Icon").GetComponent<Image>().sprite == null)
                    {
                        print("Creating a new button in the quick bar");
                        //This slot is empty
                        CustomizeButton(_hotKeySlots[indexSlots]);
                        //Increase used count
                        _slotUsedCount++;
                        //stop creating
                        return;
                    }
                }
            }
        }

        print("Creating a button");
        //Check whats in the verticle layout container
        indexVert = _layoutContainerVert.childCount - 1;
        //Get the last column of the vert container

        
        //If there are no columns, If the last column couldent be found, or if last column is full
        if (indexVert >= 0)
        {
            lastCol = _layoutContainerVert.GetChild(indexVert);
        }
   
        if (indexVert < 0 || lastCol == null || (indexSlots = lastCol.childCount) >= _maxSlotCount)
        {
   
            print("vert index: " + indexVert + "creating new column");
            //Create a new column, which will be the hori container
            lastCol = Instantiate(_layoutContainerHor, _layoutContainerVert);
            if (indexVert > 1)
                enableScroll();
        }

        //Create the new item slot.
        newItemSlot = Instantiate(_itemSlotTemplate, lastCol);

        CustomizeButton(newItemSlot);
    }

    void CustomizeButton(Transform newItemSlot)
    {
        //Pull information from the SO
        ItemSO itemSO = _item.itemSO;
        Type itemType = _item.type;
        Sprite iconSprite = itemSO.IconSprite;
        int itemID = itemSO.ID;

        //Customize the new item slot.
        if (iconSprite == null)
        {
            newItemSlot.Find("Icon").GetComponent<Image>().sprite = _defaultMissingSprite;
        }
        else
        {
            newItemSlot.Find("Icon").GetComponent<Image>().sprite = itemSO.IconSprite;
        }
        //newItemSlot.Find("Icon").GetComponent<Image>().color = Color.white;

        //Set the text
        newItemSlot.Find("AmountText").GetComponent<TextMeshProUGUI>().text = _item.amount.ToString();

        //Create a new action to listen to the buttonclick.
        if (itemType == typeof(DefenseItemSO))
        {  
            //TODO: find out if I need to remove listener at some point is so find out how. 
            newItemSlot.GetComponent<Button>().onClick.AddListener(delegate { _placementSystem.StartPlacement(_item); });
        }
        //TODO: Make an action for weapons
    }
    
    void handelOnclick()
    {
        /*Testing with a delegate*/
        PlacementSystem placementSystem = new PlacementSystem();
        placementSystem.StartPlacement(_item);
    }
    private void disableScroll()
    {
        //_scrollScript.vertical = false;
        //_scrollScript.verticalScrollbar = null;
    }
    private void enableScroll()
    {
       // _scrollScript.vertical = true;;
    }
}
