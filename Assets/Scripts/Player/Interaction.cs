using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance = 3f;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private I_Interactable curInteractable;

    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            CheckForInteractable();
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                curInteractable = curInteractGameObject.GetComponent<I_Interactable>();
                
                if (curInteractable != null)
                {
                    string promptText = curInteractable.SetPrompt(); 
                    UIManager.Instance.ShowInteractionUI(promptText);
                }
                else
                {
                    UIManager.Instance.HideInteractionUI();
                }
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable = null;
            UIManager.Instance.HideInteractionUI();
        }
    }

    private void SetPromptText()
    {
        if (curInteractable != null) // 🎯 NULL 체크 추가
        {
            Debug.Log(curInteractable.SetPrompt());
            UIManager.Instance.HideInteractionUI();
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            UIManager.Instance.HideInteractionUI();
        }
    }
}
