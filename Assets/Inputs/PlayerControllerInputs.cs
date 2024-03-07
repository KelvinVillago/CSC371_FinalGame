using System;
using UnityEngine;
using UnityEngine.InputSystem;


public enum ActionMapName_Enum {main, GridMap};

[RequireComponent(typeof(PlayerInput))]
public class PlayerControllerInputs : MonoBehaviour
{
    //Refrences to the input for other scripts to access. 
    public bool OpenInventoryInput { get; set; }
    public bool SelectInventoryInput { get; set; }
    public bool ExitInventoryInput { get; set; }
    public bool RotateInventoryInput { get; set; }
    
    [Header("READ ONLY - Debugging values")]
    [SerializeField] private string _currentMapName;

    //Actions to look for presses
    private InputAction selectInventoryAction; 
    private InputAction exitInventoryAction;
    private InputAction rotateInventoryAction;
    private InputAction openInventoryAction;

    private PlayerInput _playerInput;
    private InputActionAsset _actions;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _actions = _playerInput.actions;
        openInventoryAction = _actions["OpenInventory"];
        selectInventoryAction = _actions["ShopSelect"];
        exitInventoryAction = _actions["ExitShop"];
        rotateInventoryAction = _actions["RotateInventory"];
        _currentMapName = _playerInput.currentActionMap.name;
    }
   
 
    /*-----------------------Inputs for Main Map ------------------------------*/
    private void OnOpenInventory(InputValue value)
    {
        OpenInventoryInput = openInventoryAction.WasPressedThisFrame();
    }

    /*-----------------------Inputs for Grid Map ------------------------------*/
    private void OnRotateInventory(InputValue value)
    {
        RotateInventoryInput = rotateInventoryAction.WasPressedThisFrame();
       
    }
    private void OnShopSelect(InputValue value)
    {
        SelectInventoryInput = selectInventoryAction.WasPressedThisFrame();
    }

    private void OnExitShop()
    {
        ExitInventoryInput = exitInventoryAction.WasPressedThisFrame();
    }

    /*-----------------------Input Action Maps ------------------------------*/
    public void ToggleActionMap(ActionMapName_Enum actionMapName)
    {
        //get the action map using the enum index
        InputActionMap actionMap = _actions.actionMaps[(int)actionMapName];
        
        //if its already turned on stop.
        if (actionMap.enabled && _currentMapName == actionMap.name)
            return;

        //Disable all the maps
        DisableAllMaps();
        
        //Enable the correct map
        actionMap.Enable();
        _playerInput.currentActionMap = actionMap;
        _currentMapName = _playerInput.currentActionMap.name;
    }

    private void DisableAllMaps()
    {
        foreach (var map in _actions.actionMaps)
        {
            map.Disable();
        }
    }


    /*-----------------------Still testing these functions ------------------------------*/
    public void SwitchActionMap(InputActionMap actionMap)
    {
        _playerInput.SwitchCurrentActionMap(actionMap.name);
    }
    private void EnableAllMaps()
    {
        foreach(var map in _actions.actionMaps)
        {
            map.Enable();
        }
    }
   
    
}
