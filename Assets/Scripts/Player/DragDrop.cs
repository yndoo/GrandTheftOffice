using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;


public class DragDrop : MonoBehaviour

{
    public Camera camera;
    public bool isHolding = false;
    public LayerMask oblayer;
    public LayerMask cantDragLayer;
    public Rigidbody heldItem;
    public Transform holdPositon;

    public float pickupRange = 5f;
    private float direction = 0f;
    private float mouseDirection = 0f;
    private float heldDistance;

    private Coroutine RotateCoroutine;
    private Coroutine RotatfrontCoroutine;

    void Start()
    {
        camera = Camera.main;
    }

    public void Pickup()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //상호작용레이어감지
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,pickupRange, oblayer))
        {
            Rigidbody itemRB = hit.collider.GetComponent<Rigidbody>();
            if (itemRB != null)
            {
                heldItem = itemRB;
                holdPositon.position = hit.transform.position;
                heldDistance = Vector3.Distance(transform.position, heldItem.position);
                heldItem.useGravity = false;
                isHolding = true;
            }
        }
    }

    public void OnPickupInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started )
        {

            if(isHolding)
            {
                Drop();              
            }
            else
            {
                Pickup();
            }
        }
    }
 
    public void Drop()
    {
        heldItem.useGravity = true ;
        heldItem.includeLayers = oblayer;
        isHolding = false ;
        heldItem = null ;  
    }


    private void FixedUpdate()
    {
        if(isHolding == true && heldItem != null)
        {
            Vector3 targetPosition = holdPositon.position;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 벽뚫 방지
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, heldDistance, cantDragLayer))
            {
                targetPosition = hit.point;
            }

            heldItem.transform.position = targetPosition;
        }
    }

    public void OnHoldingRotate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            mouseDirection = context.ReadValue<float>();
            mouseDirection = Mathf.Clamp(mouseDirection, -1f, 1f);
            if (RotatfrontCoroutine == null)
            {
                RotatfrontCoroutine = StartCoroutine(RotatefronteItem());
            }
           
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            if(RotatfrontCoroutine != null)
            {
                StopCoroutine(RotatfrontCoroutine);
                RotatfrontCoroutine = null;
            }
        }
    }

    public void OnRotateMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            direction = context.ReadValue<float>();
            direction = Mathf.Clamp(direction, -1f, 1f);
            if (RotateCoroutine == null)
           {
                RotateCoroutine = StartCoroutine(RotateItem());
            }

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (RotateCoroutine != null)
            {
                StopCoroutine(RotateCoroutine);
                RotateCoroutine = null;
            }
        }

   }
    IEnumerator RotatefronteItem()
    {
        while (true)
        {        
                heldItem.gameObject.transform.Rotate(Vector3.right * 50 * Time.deltaTime);
                yield return null;
        }
    }

    IEnumerator RotateItem()
    {
        while(true)
        {
            if(direction <  0)
            {
                heldItem.gameObject.transform.Rotate(Vector3.up * 50 * Time.deltaTime);
            }
            else if(direction > 0)
            {
                heldItem.gameObject.transform.Rotate(Vector3.up * -50 * Time.deltaTime);         
            }
            yield return null;
        }
    }
}