using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    private ItemSO _itemSO;
    //Get a refrence to the Start Placement function
    PlacementSystem _placementSystem;
    private int _slotUsedCount = 0;


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
    }

    private void CreateItemButton(ItemSO itemSO)
    {
        _itemSO = itemSO;
        int indexVert;
        Transform lastCol = null;
        Transform newItemSlot;
        int indexSlots;

        //Check if there is an open slot on the quickbar
        if(_slotUsedCount < _maxSlotCount)
        {
            for (indexSlots = 0;  indexSlots < _maxSlotCount; indexSlots++)
            {
                if (_hotKeySlots[indexSlots].Find("Icon").GetComponent<Image>().sprite == null)
                {
                    //This slot is empty
                    customizeSlot(_hotKeySlots[indexSlots], itemSO);
                    //Increase used count
                    _slotUsedCount++;
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
            print(lastCol);
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

        customizeSlot(newItemSlot, itemSO);
    }

    void customizeSlot(Transform newItemSlot, ItemSO itemSO)
    {
        //Customize the new item slot.
        Sprite iconSprite = itemSO.IconSprite;

        if (iconSprite == null)
        {
            newItemSlot.Find("Icon").GetComponent<Image>().sprite = _defaultMissingSprite;
        }
        else
        {
            newItemSlot.Find("Icon").GetComponent<Image>().sprite = itemSO.IconSprite;
        }
        //newItemSlot.Find("Icon").GetComponent<Image>().color = Color.white;

        //Create a new action to listen to the buttonclick.
        //TODO: find out if I need to remove listener at some point is so find out how. 
        newItemSlot.GetComponent<Button>().onClick.AddListener(delegate { _placementSystem.StartPlacement(itemSO); });
    }
    /*Testing with a delegate*/
    void handelOnclick()
    {
        PlacementSystem placementSystem = new PlacementSystem();
        placementSystem.StartPlacement(_itemSO);
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
