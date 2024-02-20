using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;
    private Vector3 _lastPosition;
    [Tooltip("Which layer will the input detection of the mouse listens too")]
    [SerializeField] LayerMask _placementLayerMask;
    public event Action OnClicked, OnExit;

    private InputAction _exitShop;
    private InputAction _selectShop;
    //Will return true or false if we are above the UI
    //This is a labda to set true or false. 
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    private void Start()
    {
        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _exitShop = _playerInput.actions["ExitShop"];
        _selectShop = _playerInput.actions["ShopSelect"];
    }

    private void Update()
    {
        //Input.GetMouseButtonDown(0)
        //Mouse.current.leftButton.wasPressedThisFrame
        if (_selectShop.WasPerformedThisFrame())
        {
            Debug.Log("Shop select activated");
            OnClicked?.Invoke();
        }
        //Input.GetKeyDown(KeyCode.Escape)
        if (_exitShop.WasPressedThisFrame())
        {
            Debug.Log("ExitValue is true");
            OnExit?.Invoke();
        }
    }


    public Vector3 GetSelectedMapPosition()
    {
        //Ising the old input syste, 
        //Vector3 mousePos = Input.mousePosition;
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
        //else
        //{
        //    _lastPosition = ray.GetPoint(100) - transform.position;
        //}
        return _lastPosition;
    }
}
