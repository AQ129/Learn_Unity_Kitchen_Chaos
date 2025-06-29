using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour, IKitchenObjectParent
{

    private static NewBehaviourScript instance;
    public static NewBehaviourScript Instance
    {
        get { return instance; }
        set
        {
            instance = value;
        }
    }

    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint ;
    private KitchenObject kitchenObject;
    Vector3 inputVector;
    private bool isWalking;
    private new CapsuleCollider collider;
    private float moveDis;
    private Vector3 lastInteracDir;
    private BaseCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternate += GameInput_OnInteractAlternate;
    }

    private void GameInput_OnInteractAlternate(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputVector = gameInput.GetMovermentVectorNomalized();
        moveDis = Time.deltaTime * moveSpeed;
        HandleMovement(moveDis);
        HandleInteractions();
    }

    public bool IsWalking() { return isWalking; }

    private void HandleMovement(float moveDis)
    {
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * collider.height,
                        collider.radius, inputVector, moveDis);
        if (canMove)
        {
            transform.position += inputVector * moveDis;
        }
        else if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * collider.height,
                        collider.radius, new Vector3(inputVector.x, 0, 0), moveDis))
        {
            transform.position += new Vector3(inputVector.x, 0, 0).normalized * moveDis;
        }
        else if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * collider.height,
                        collider.radius, new Vector3(0, 0, inputVector.z), moveDis))
        {
            transform.position += new Vector3(0, 0, inputVector.z).normalized * moveDis;
        }
        transform.forward = Vector3.Slerp(transform.forward, inputVector, Time.deltaTime * rotateSpeed);

        isWalking = inputVector != Vector3.zero;
    }

    private void HandleInteractions()
    {
        //Debug.Log(selectedCounter);
        if (inputVector != Vector3.zero)
        {
            lastInteracDir = inputVector;
        }
        if (Physics.Raycast(transform.position, lastInteracDir, out RaycastHit raycastHit, collider.height, layerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this.selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            selectedCounter = baseCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public KitchenObject KitchenObject 
    {
        get 
        { 
            return kitchenObject;
        }
        set 
        { 
            kitchenObject = value;
            if(kitchenObject != null)
            {
                OnPickedSomething?.Invoke(this, EventArgs.Empty);
            }
        } 
    }

    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }

    public bool HasKitchenObject() { return KitchenObject != null; }
}
