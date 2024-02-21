using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerControllerInputs : MonoBehaviour
{
    [SerializeField] private string _currentMapName;
    public bool openInventoryInput = false;
    public bool shopSelectInput = false;
    public bool exitShopInput = false;
    private PlayerInput _playerInput;
    [HideInInspector] public InputActionMap itemMap; 
    [HideInInspector] public InputActionMap mainMap;
    [HideInInspector] public InputAction selectInventoryAction; 
    [HideInInspector] public InputAction exitInventoryAction;
    [HideInInspector] public InputAction openInventoryAction;
    //public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        itemMap = _playerInput.actions.FindActionMap("GridTest");
        mainMap = _playerInput.actions.FindActionMap("main");
        openInventoryAction = mainMap.FindAction("OpenInventory");
        selectInventoryAction = itemMap.FindAction("ShopSelect");
        exitInventoryAction = itemMap.FindAction("ExitShop");
        _currentMapName = _playerInput.currentActionMap.name;
    }

    private void OnOpenInventory(InputValue value)
    {
        openInventoryInput = openInventoryAction.WasPressedThisFrame();
    }
    private void OnShopSelect(InputValue value)
    {
        shopSelectInput = selectInventoryAction.WasPressedThisFrame();
    }

    private void OnExitShop()
    {
        exitShopInput = exitInventoryAction.WasPressedThisFrame();
    }

    public void SwitchActionMap(InputActionMap actionMap)
    {
        _playerInput.SwitchCurrentActionMap(actionMap.name);
    }

    public void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled && _currentMapName == actionMap.name)
            return;
        //Disable all the maps
        DisableAllMaps();
        //actionMapChange?.Invoke(actionMap);
        //Enable the correct map
        actionMap.Enable();
        _playerInput.currentActionMap = actionMap;
        _currentMapName = _playerInput.currentActionMap.name;
    }

    private void EnableAllMaps()
    {
        itemMap.Enable();
        mainMap.Enable();
    }
    private void DisableAllMaps()
    {
        itemMap.Disable();
        mainMap.Disable();
    }
    
}
