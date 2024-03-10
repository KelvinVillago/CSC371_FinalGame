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
    [Header("Inventory Properties")]
    [SerializeField] private Transform _layoutContainerVert;
    [SerializeField] private Transform _layoutContainerHor;
    [SerializeField] private Transform _itemSlotTemplate;
    [SerializeField] private int _maxSlotCount;
    [SerializeField] private Transform _scrollView;
    [SerializeField] private Sprite _defaultMissingSprite;
    [SerializeField] private Transform _defaultMissingPrefab;
    [Header("Quickbar Properties")]
    [SerializeField] private Transform[] _hotKeySlots;

    [Header("Testing Database")]
    [SerializeField] private ItemsDatabaseSO _itemDB;
    private int _slotIndex;



    private PreventClickDrag _scrollScript;
    //Get a refrence to the Start Placement function
    private Inventory _inventory;
    [SerializeField] private PlacementSystem _placementSystem;
    private TextMeshProUGUI _amountText;
    private PlayerInventory _player;

    private void Awake()
    {
        _scrollScript = _scrollView.GetComponent<PreventClickDrag>();
    }
    private void Start()
    {
        disableScroll();
    }
    public void SetPlayer(PlayerInventory player)
    {
        _player = player;
    }
    public void SetInventory(Inventory inventory)
    {
        //Starting inventory
        this._inventory = inventory;
        //Start listening to the equipts
        inventory.EquipItemAction += EquipItemHandler;

        //Set the same inventory for the grid placement system to check
        _placementSystem.SetInventory(inventory);

        //Test code
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));
        inventory.AddItem(new Item(_itemDB.DefenseDB[0]));
        inventory.AddItem(new Item(_itemDB.AnimalDB[0]));
        inventory.AddItem(new Item(_itemDB.WeaponsDB[0]));
        //Build UI for starting inventory
        RefreshInventory();
        
        //Start listening for new changes
        inventory.OnInventoryChanged += OnInventoryChanged;
        
    }

    private void OnInventoryChanged()
    {
        //Need to delet everything because we are using columns and rows it would be difficult to move each item up by 1.
        //Delete the items in the currenty inventory
        foreach (Transform child in _layoutContainerVert)
        {
            Destroy(child.gameObject);
        }

        //To update the inventory, deleted children might still be active,
        //to prevent destroying the new inventory start a new row. 
        Instantiate(_layoutContainerHor, _layoutContainerVert);
        
        //Display the updated inventory
        RefreshInventory();
    }

    private void RefreshInventory()
    {
        _slotIndex = 0;
        foreach (Item item in _inventory.GetItemsList())
        {
            CreateItemButton(item);
            _slotIndex++;
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
            for (indexSlots = 0; indexSlots < _maxSlotCount; indexSlots++)
            {
                EquiptSlot slot = _hotKeySlots[indexSlots].GetComponent<EquiptSlot>();

                if (slot.Item == item)
                {
                    //Item already in hot bar
                    return;
                }

                if (slot.Image.sprite == null)
                {
                    //This slot is empty
                    Transform newHotKey = CustomizeButton(_hotKeySlots[indexSlots], item);
                        
                    //Activate the hotkey if its the first one.
                    if(indexSlots == 0)
                    {
                        _inventory.EquipItem(newHotKey);
                    }

                    //stop creating
                    return;
                }
            }
            
        }
       
        
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
            //Create a new column, which will be the hori container
            lastCol = Instantiate(_layoutContainerHor, _layoutContainerVert);
            if (indexVert > 1)
                enableScroll();
        }

        //Create the new item slot.
        newItemSlot = Instantiate(_itemSlotTemplate, lastCol);

        CustomizeButton(newItemSlot, item);
    }

    Transform CustomizeButton(Transform newItemSlot, Item item)
    {
        EquiptSlot slot = newItemSlot.GetComponent<EquiptSlot>();
        //Update the slot index.
        slot.Item = item;
        item.SlotID = _slotIndex;

        //Customize the new item slot.
        if (item.itemSO.IconSprite == null)
        {
            //newItemSlot.Find("Icon").GetComponent<Image>().sprite = _defaultMissingSprite;
            slot.Image.sprite = _defaultMissingSprite;
        }
        else
        {
            //newItemSlot.Find("Icon").GetComponent<Image>().sprite = item.itemSO.IconSprite;
            slot.Image.sprite = item.itemSO.IconSprite;
        }
        //newItemSlot.Find("Icon").GetComponent<Image>().color = Color.white;

        //Set the text
        if (item.amount > 1)
        {
           // newItemSlot.Find("AmountText").GetComponent<TextMeshProUGUI>().text = item.AmountSting();
            slot.AmountText.text = item.AmountSting();
        }
        //Create a new action to listen to the buttonclick.
        if (item.type == typeof(DefenseItemSO))
        {  
            //TODO: find out if I need to remove listener at some point is so find out how. 
            //newItemSlot.GetComponent<Button>().onClick.AddListener(delegate { _placementSystem.StartPlacement(item);});
            slot.Button.onClick.AddListener(delegate { _placementSystem.StartPlacement(item); });
        }
        else if(item.itemSO.Prefab.GetComponent<ItemWorld>() != null)
        {
            //This is a drop-able world item.
            //Call without using delegate. https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Button-onClick.html
            //newItemSlot.GetComponent<Button>().onClick.AddListener(()=>DropItemHandler(item , newItemSlot));
            slot.Button.onClick.AddListener(() => DropItemHandler(item));
        }
        else if (item.IsSameOrSubclass(typeof(WeaponItemSO)))
        {
            //SWORDS do not live here?
            //newItemSlot.GetComponent<Button>().onClick.AddListener(()=> _inventory.EquipItem(newItemSlot));
            slot.Button.onClick.AddListener(() => _inventory.EquipItem(newItemSlot));
        }
        else
        {
            //button calls useitem from inventory. When inventory useItem is called the player is listening.
            //newItemSlot.GetComponent<Button>().onClick.AddListener(() => _inventory.UseItem(item));
            slot.Button.onClick.AddListener(() => _inventory.UseItem(item));
        }

        return newItemSlot;
    }
    
    private void EquipItemHandler(Transform slot)
    {
        //Update the UI for the Hotkey slots
        foreach (Transform hotKeySlot in _hotKeySlots)
        {
            //Remove color from other slots
            hotKeySlot.GetComponent<Image>().color = Color.white;
        }
        slot.gameObject.GetComponent<Image>().color = Color.yellow;
    }

    public ItemWorld DropItemHandler(Item item)
    {
        //Item removeOne = new Item(item.itemSO, 1 ,item.SlotID);
        _inventory.RemoveItem(item);
        return ItemWorld.DropItem(item, _player.GetPos());
    }

    private void OnDestroy()
    {
        _inventory.OnInventoryChanged -= OnInventoryChanged;
    }

    /*USING FOR DEBUGGING*/
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