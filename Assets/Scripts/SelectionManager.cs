using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/*Use player input to trigger the placement manager*/

public class SelectionManager : MonoBehaviour
{
    [Header("Grid Selection properties")]
    [SerializeField] private Camera _sceneCamera;
    [Tooltip("Which layer will the input detection of the mouse listens too")]
    [SerializeField] private LayerMask _placementLayerMask;
    [SerializeField] private GameObject _player;
   
    //PlayerInventory Information
    private Vector3 _lastPosition;
    private PlayerControllerInputs _playerInputs;
    private GameObject _inventoryPanel;

    //Properties for the placement system.
    [HideInInspector] public event Action OnClicked, OnExit, OnRotate;
    [HideInInspector] public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();


    private void Start()
    {
        _playerInputs = _player.GetComponent<PlayerControllerInputs>();
        _inventoryPanel = GameObject.Find("UI/UI_Inventory");
        _inventoryPanel.SetActive(false);
    }

    public void OpenHandler()
    {
        _playerInputs.OpenInventoryInput = true;
    }

    private void Update()
    {
        if (_playerInputs.OpenInventoryInput)
        {
            //Activate grid map
            _playerInputs.ToggleActionMap(ActionMapName_Enum.GridMap);
            _inventoryPanel.SetActive(true);

            if (_playerInputs.SelectInventoryInput)
            {
                //Reseting the value;
                _playerInputs.SelectInventoryInput = false;
                //Debug.Log("Shop select activated");
                OnClicked?.Invoke();
            }
            if (_playerInputs.RotateInventoryInput)
            {
                //Reseting the value;
                _playerInputs.RotateInventoryInput = false;
                //Debug.Log("Shop select activated");
                OnRotate?.Invoke();
            }
            if (_playerInputs.ExitInventoryInput)
            {
                _playerInputs.ExitInventoryInput = false;
                //Only turn time back on if the shop is closed
                //if (!_shopPanel.activeInHierarchy)
                //Time.timeScale = 1;
                //Debug.Log("ExitValue is true");
                //Close the inventory
                _playerInputs.OpenInventoryInput = false;
                _inventoryPanel.SetActive(false);
                _playerInputs.ToggleActionMap(ActionMapName_Enum.main);
                OnExit?.Invoke();
            }
        }
    }
   
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        //Only select objects rendered by the camera
        mousePos.z = _sceneCamera.nearClipPlane;
        //A ray from the mouse position
        Ray ray = _sceneCamera.ScreenPointToRay(mousePos);  
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200, _placementLayerMask))
        {
            //_debugTransform.position = hit.point;
            _lastPosition = hit.point;
        }
        return _lastPosition;
    }
}
