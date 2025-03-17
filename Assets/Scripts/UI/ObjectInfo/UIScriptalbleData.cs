using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewInteractableObject", menuName = "Interactable Object")]
public class InteractableObjectData : ScriptableObject
{
    public string title;
    [TextArea(2, 5)]
    public string description;
    public Sprite icon;
    public string interactionText = "상호작용";
}