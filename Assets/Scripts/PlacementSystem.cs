using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject _mouseIndicator;
    [SerializeField] private SelectionManager _selectionManager;
    [SerializeField] private GameObject _cellIndicator;
    [SerializeField] private Grid _grid;
    [Tooltip("Database refrence")]
    [SerializeField] private ObjectsDatabaseSO _database;
    private int _selectedObjIndex = -1;
    [Tooltip("Toggle to turn off the grid")]
    [SerializeField] private GameObject _gridVisualization;
    [SerializeField] private float _rotationAngle = 0;

    [SerializeField] private ItemsDatabaseSO _invDB;

    private void Start()
    {
        StopPlacement();
    }
    public void StartPlacement(ItemSO itemSO)
    {
        
        int ID = itemSO.ID;
        Type itemType = itemSO.GetType();

        //Trying to place an animal?
        if(itemType == typeof(AnimalItemSO))
        {
            print(itemSO.name +" :Animals cant be placed at night");
        }
        else if(itemType == typeof(DefenseItemSO))
        {
            print(itemSO.name + " Placing a defense, depending on the inventory keep going...");
        }
        else
        {
            print(itemSO.name + " Weapon, equipt it?");
        }


        //Set the start rotation of the cursor indicator.
        _rotationAngle = 0;
        _cellIndicator.transform.rotation = Quaternion.Euler(0f, _rotationAngle, 0f);
        //Prevent a bug where we automatically start placing the item. 
        StopPlacement();
        //The index of the data is returned if the findIndex data matches with the ID param.
        _selectedObjIndex = _database.objectsData.FindIndex(data => data.ID == ID);
        if(_selectedObjIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        ObjectData objectData = _database.objectsData[_selectedObjIndex];
        //if the item is in the database turn on the grid and allow placement.
        _gridVisualization.SetActive(true);
        _cellIndicator.SetActive(true);
        //Update the cell indicator to be the size of the selected item.
        _cellIndicator.transform.localScale = new Vector3 (objectData.Size.x, 1, objectData.Size.y);
        //Call the placeStructure method
        _selectionManager.OnClicked += PlaceStructure;
        _selectionManager.OnExit += StopPlacement;
        _selectionManager.OnRotate += RotateItem;
    }
    private void RotateItem()
    {
        _rotationAngle = (_rotationAngle == 270) ? 0f : _rotationAngle + 90.0f;
        _cellIndicator.transform.rotation = Quaternion.Euler(0f, _rotationAngle, 0f);
    }
    private void PlaceStructure()
    {
        //Place the object if we are over the grid.
        if (_selectionManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = _selectionManager.GetSelectedMapPosition();
        Vector3Int _gridPosition = _grid.WorldToCell(mousePosition);
        //Create the game object 
        GameObject newObj = Instantiate(_database.objectsData[_selectedObjIndex].Prefab);
        newObj.transform.position = _grid.CellToWorld(_gridPosition);
        //Change the rotation
        if (_rotationAngle != 0)
            newObj.transform.rotation = Quaternion.Euler(0f, _rotationAngle, 0f);
        //Done using the rotation so reset it
        _rotationAngle = 0;

        // Decrement the count based on the selected object
        if (_selectedObjIndex >= 0)
        {
            Debug.Log("Selected ID: " + _database.objectsData[_selectedObjIndex].ID);
            int selectedID = _database.objectsData[_selectedObjIndex].ID;
            if (selectedID == 0) // Fence ID
            {
                _selectionManager.fenceCount--;
            }
            else if (selectedID == 1) // Small Turret ID
            {
                _selectionManager.smallTurretCount--;
            }
            else if (selectedID == 2) // Large Turret ID
            {
                _selectionManager.largeTurretCount--;
            }
        }
        // Update the inventory UI
        _selectionManager.UpdateInventoryUI();

        // Close the grid visualization
        StopPlacement();
    }

    private void StopPlacement()
    {
        //Rest the selected item
        _selectedObjIndex = -1;
        //Trun off the grid, placement is no longer allowed. 
        _gridVisualization.SetActive(false);
        _cellIndicator.SetActive(false);
        //Stop listening to the events. 
        _selectionManager.OnClicked -= PlaceStructure;
        _selectionManager.OnRotate -= RotateItem;
        _selectionManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        //If we are not in the placement mode, dont do anything!
        if (_selectedObjIndex < 0)
            return;
        Vector3 mousePosition = _selectionManager.GetSelectedMapPosition();
        Vector3Int _gridPosition = _grid.WorldToCell(mousePosition);
        _mouseIndicator.transform.position = mousePosition;
        //Debug.Log("Mouse Position" + mousePosition);
        _cellIndicator.transform.position = _grid.CellToWorld(_gridPosition);
    }
}
/**
 * Note:
 * On the Grid Object the cell size is 1,1,1.
 * If you want two cells per unity unit (box size), change the cell size to (0.5, 0.50, 0.50)
 * Then change the  Grid shader Material (called shader Graph) size value to (2,2)
 */