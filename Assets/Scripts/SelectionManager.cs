using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;
    [Tooltip("Which layer will the input detection of the mouse listens too")]
    [SerializeField] LayerMask _placementLayerMask;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject _shopPanel;
    //Refrences
    private PlayerControllerInputs _inputs;
    private PlayerInventory _playerInventory;
    private Inventory _inventory;
    private GameObject _inventoryUIPanel;
    private Vector3 _lastPosition;
    public event Action OnClicked, OnExit, OnRotate;
    public PlayerInventory PlayerInventory { get { return _playerInventory; } }

    //Will return true or false if we are above the UI
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    // Buttons and Text
    public Button[] invBtns;
    public TextMeshProUGUI[] invBtnTxts;
    public int fenceCount = 0;
    public int smallTurretCount = 0;
    public int largeTurretCount = 0;
    private void Awake()
    {
        _inputs = player.GetComponent<PlayerControllerInputs>();
        //Save the refrence to the targetObjects inventory
        _playerInventory = player.GetComponent<PlayerInventory>();
        //Get access to the UI the targetObject is using.
        _inventoryUIPanel = _playerInventory.GetInventoryPanel();
    }
    private void Start()
    {
        //Clear the screen
        _inventoryUIPanel.SetActive(false);
        //Get the UI_inventory
        _inventory = _playerInventory.Inventory;
        
    }

    private void Update()
    {
        
        if (_inputs.openInventoryInput) {
            //Time.timeScale = 0;

            //UpdateInventoryUI();

            _inputs.ToggleActionMap(_inputs.itemMap); //Activate grid map
            _inventoryUIPanel.SetActive(true);

            if(_inputs.shopSelectInput)
            {
                //Reseting the value;
                _inputs.shopSelectInput = false;
                //Debug.Log("Shop select activated");
                OnClicked?.Invoke();
            }
            if (_inputs.rotateInventoryInput)
            {
                //Reseting the value;
                _inputs.rotateInventoryInput = false;
                //Debug.Log("Shop select activated");
                OnRotate?.Invoke();
            }
            if (_inputs.exitShopInput)
            {
                _inputs.exitShopInput = false;
                //Only turn time back on if the shop is closed
                if (!_shopPanel.activeInHierarchy)
                    //Time.timeScale = 1;
                //Debug.Log("ExitValue is true");
                //Close the inventory
                _inputs.openInventoryInput = false;
                _inventoryUIPanel.SetActive(false);
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

    public void UpdateInventoryUI()
    {
        /*
        //We should update this to be the size of the list and generate the inventory cards.
        ObjectData[] invItems = _itemDB.objectsData.FindAll(data => data.IsPlaceable && data.InventoryQuantity > 0).ToArray();
        int buttonAmt = 3;
        for (int i = 0; i < buttonAmt ; i++)
        {
            if(invItems.Length < buttonAmt)
            {

            }
            //Remove this is we get generation
            ObjectData item = invItems[i];
            invBtnTxts[i].text = item.Name + " " + item.InventoryQuantity;
        }
        */
        
        invBtnTxts[0].text = "Fence: " + fenceCount.ToString();
        invBtnTxts[1].text = "Small Turrets: " + smallTurretCount.ToString();
        invBtnTxts[2].text = "Large Turrets: " + largeTurretCount.ToString();

        if (fenceCount == 0)
        {
            invBtns[0].interactable = false;
        }
        else
        {
            invBtns[0].interactable = true;
        }
        if (smallTurretCount == 0)
        {
            invBtns[1].interactable = false;
        }
        else
        {
            invBtns[1].interactable = true;
        }
        if (largeTurretCount == 0)
        {
            invBtns[2].interactable = false;
        }
        else
        {
            invBtns[2].interactable = true;
        }
    }
}
