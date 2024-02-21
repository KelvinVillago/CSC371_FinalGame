using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;
    private Vector3 _lastPosition;
    [Tooltip("Which layer will the input detection of the mouse listens too")]
    [SerializeField] LayerMask _placementLayerMask;
    [SerializeField] GameObject _inventoryCanvas;
    public event Action OnClicked, OnExit;
    private PlayerControllerInputs _inputs;
    [SerializeField] private GameObject player;
  
    //Will return true or false if we are above the UI
    //This is a labda to set true or false. 
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    private void Start()
    {
        //Clear the screen
        _inputs = player.GetComponent<PlayerControllerInputs>();
        _inventoryCanvas.SetActive(false);
    }

    private void Update()
    {
        
        if (_inputs.openInventoryInput) {
            Time.timeScale = 0;

            _inputs.ToggleActionMap(_inputs.itemMap);
            _inventoryCanvas.SetActive(true);

            if(_inputs.shopSelectInput)
            {
                //Reseting the value;
                _inputs.shopSelectInput = false;
                //Debug.Log("Shop select activated");
                OnClicked?.Invoke();
            }
        
            if(_inputs.exitShopInput)
            {
                _inputs.exitShopInput = false;
                Time.timeScale = 1;
                //Debug.Log("ExitValue is true");
                //Close the inventory
                _inputs.openInventoryInput = false;
                _inventoryCanvas.SetActive(false);
                _inputs.ToggleActionMap(_inputs.mainMap);
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
