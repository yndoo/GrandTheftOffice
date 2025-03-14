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

    public TextMeshProUGUI promptText;
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
                    SetPromptText();
                }
                else
                {
                    // promptText.gameObject.SetActive(false); // 인터랙션 불가능하면 UI 숨김
                }
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable = null;
            // promptText.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        if (curInteractable != null) 
        {
            Debug.Log(curInteractable.SetPrompt());
            // promptText.gameObject.SetActive(true);
            // promptText.text = curInteractable.SetPrompt();
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            // promptText.gameObject.SetActive(false);
        }
    }
}
