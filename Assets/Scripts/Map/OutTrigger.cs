using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutTrigger : MonoBehaviour
{
    public LayerMask portableLayer;
    public bool canOut = false;
    public List<GameObject> QPrefabs; // 프리팹 리스트
    private DragDrop playerDragDrop; // 플레이어 드래그 앤 드롭 스크립트
    private GameObject[] collectedObjects; // 수집된 오브젝트 저장 배열
    private Button buttonScript; // 버튼 스크립트

    private void Start()
    {
        if (playerDragDrop == null)
        {
            playerDragDrop = FindObjectOfType<DragDrop>();
        }

        // Button 스크립트 찾기
        buttonScript = FindObjectOfType<Button>();

        collectedObjects = new GameObject[QPrefabs.Count];
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((portableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Transform parentTransform = other.transform.parent;
            GameObject parentObject = parentTransform != null ? parentTransform.gameObject : null;

            if (parentObject == null)
            {
                Debug.Log("부모가 없는 오브젝트: " + other.gameObject.name);
                return;
            }

            bool isMatched = false;
            int matchedIndex = -1;

            for (int i = 0; i < QPrefabs.Count; i++)
            {
                if (parentObject.name.Contains(QPrefabs[i].name))
                {
                    isMatched = true;
                    matchedIndex = i;
                    break;
                }
            }

            if (isMatched)
            {
                if (collectedObjects[matchedIndex] != null)
                {
                    Debug.Log("틀리고 (이미 수집된 프리팹)");
                }
                else
                {
                    if (playerDragDrop.isHolding)
                    {
                        playerDragDrop.Drop();
                    }
                    
                    // 새로운 배열에 저장
                    collectedObjects[matchedIndex] = QPrefabs[matchedIndex];

                    // 씬에서 오브젝트 삭제
                    Destroy(other.gameObject);

                    Debug.Log($"맞고! {other.gameObject.name}을 삭제하고 {QPrefabs[matchedIndex].name}을 저장함.");
                    
                    CheckOut();
                }
            }
            else
            {
                Debug.Log("틀리고");
                Debug.Log($"오브젝트 이름: {other.gameObject.name}");
                Debug.Log($"부모 오브젝트 이름: {parentObject.name}");
            }
        }
    }

    private void CheckOut()
    {
        for (int i = 0; i < collectedObjects.Length; i++)
        {
            if (collectedObjects[i] == null)
            {
                canOut = false;
                return;
            }
        }

        canOut = true;

        // 버튼을 클릭 가능하게 만듦
        if (buttonScript != null)
        {
            buttonScript.isClickable = true;
            Debug.Log("모든 아이템을 수집 완료! 버튼이 활성화됨.");
        }
    }
}
