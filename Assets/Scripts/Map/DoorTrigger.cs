using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> DoorPrefabs; // 문 프리팹 리스트
    private float totalWeight = 0f; // 현재 올라온 총 무게
    private float weightThreshold = 50f; // 문을 열기 위한 최소 무게

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            totalWeight += rb.mass; // 무게 합산
            CheckWeight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            totalWeight -= rb.mass; // 무게 제거
            CheckWeight();
        }
    }

    private void CheckWeight()
    {
        if (totalWeight >= weightThreshold)
        {
            Debug.Log("✅ 무게 기준 충족! 문 열기");
            OpenAllDoors();
        }
        else
        {
            Debug.Log("무게 부족! 문 닫기");
            CloseAllDoors();
        }
    }

    private void OpenAllDoors()
    {
        foreach (GameObject door in DoorPrefabs)
        {
            if (door.TryGetComponent<OpenDoor>(out OpenDoor doorScript))
            {
                doorScript.ToggleDoor(); // 🔹 문 열기 함수 실행
            }
        }
    }

    private void CloseAllDoors()
    {
        foreach (GameObject door in DoorPrefabs)
        {
            if (door.TryGetComponent<OpenDoor>(out OpenDoor doorScript))
            {
                doorScript.ToggleDoor(); // 🔹 문 닫기 함수 실행
            }
        }
    }
}