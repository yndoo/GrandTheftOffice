
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NodeType { Normal, Start, Target }

public class NodeUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button button; 
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI idText;

    public System.Action<int> OnNodeClicked;

    private int nodeId;
    private NodeType nodeType = NodeType.Normal;

    private Color activeColor = new Color(0.2f, 0.7f, 1f);
    private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f);
    private Color startNodeColor = new Color(0f, 0.8f, 0.2f);
    private Color targetNodeColor = new Color(1f, 0.5f, 0f);

    public void Setup(int id, bool isActive)
    {
        nodeId = id;
        idText.text = id.ToString();

        // 버튼 이벤트 연결
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            OnNodeClicked?.Invoke(nodeId);
        });

        UpdateState(isActive);
    }

    public void UpdateState(bool isActive)
    {
        Color baseColor;

        switch (nodeType)
        {
            case NodeType.Start:
                baseColor = startNodeColor;
                break;
            case NodeType.Target:
                baseColor = isActive ? targetNodeColor : new Color(0.7f, 0.3f, 0f);
                break;
            default:
                baseColor = isActive ? activeColor : inactiveColor;
                break;
        }

        backgroundImage.color = baseColor;
    }

    public void SetNodeType(NodeType type)
    {
        nodeType = type;
    }
}
