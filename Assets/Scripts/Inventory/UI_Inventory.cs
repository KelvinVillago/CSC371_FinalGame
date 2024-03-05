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
    }

    private void OnDisable()
    {
        _inventory.OnInventoryChanged -= OnInventoryChanged;
    }
    public void SetInventory(Inventory inventory)
    {
        //Starting inventory
        this._inventory = inventory;
        //Set the same inventory for the grid placement system to check
        _placementSystem.SetInventory(inventory);

        //Test code
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));
        inventory.AddItem(new Item(_itemDB.AnimalDB[0]));
        inventory.AddItem(new Item(_itemDB.WeaponsDB[0]));
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));

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
                        CustomizeButton(_hotKeySlots[indexSlots], item);
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

        CustomizeButton(newItemSlot, item);
    }

    void CustomizeButton(Transform newItemSlot, Item item)
    {

        //Customize the new item slot.
        if (item.itemSO.IconSprite == null)
        {
            newItemSlot.Find("Icon").GetComponent<Image>().sprite = _defaultMissingSprite;
        }
        else
        {
            newItemSlot.Find("Icon").GetComponent<Image>().sprite = item.itemSO.IconSprite;
        }
        //newItemSlot.Find("Icon").GetComponent<Image>().color = Color.white;

        //Set the text
        newItemSlot.Find("AmountText").GetComponent<TextMeshProUGUI>().text = item.AmountSting();

        //Create a new action to listen to the buttonclick.
        if (item.type == typeof(DefenseItemSO))
        {  
            //TODO: find out if I need to remove listener at some point is so find out how. 
            newItemSlot.GetComponent<Button>().onClick.AddListener(delegate { _placementSystem.StartPlacement(item); });
        }
        //TODO: Make an action for weapons
    }
    



    /*USING FOR DEBUGGING*/
    void handelOnclick(Item item)
    {
        /*Testing with a delegate*/
        PlacementSystem placementSystem = new PlacementSystem();
        placementSystem.StartPlacement(item);
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
