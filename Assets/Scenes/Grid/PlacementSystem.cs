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



    private void Start()
    {
        StopPlacement();
    }
    public void StartPlacement(int ID)
    {
        //Prevent a bug where we automatically start placing the item. 
        StopPlacement();
        //The index of the data is returned if the findIndex data matches with the ID param.
        _selectedObjIndex = _database.objectsData.FindIndex(data => data.ID == ID);
        if(_selectedObjIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        //if the item is in the database turn on the grid and allow placement.
        _gridVisualization.SetActive(true);
        _cellIndicator.SetActive(true);
        //Call the placeStructure method
        _selectionManager.OnClicked += PlaceStructure;
        _selectionManager.OnExit += StopPlacement;
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