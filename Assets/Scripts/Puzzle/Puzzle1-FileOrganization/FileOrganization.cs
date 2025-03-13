using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileOrganization : MonoBehaviour, IPuzzleCheckable
{
    public GameObject Reward;

    List<MatchingZone> matchingZones;

    private void Awake()
    {
        matchingZones = new List<MatchingZone>();
        foreach(MatchingZone zone in GetComponentsInChildren<MatchingZone>())
        {
            matchingZones.Add(zone);
        }
    }

    public bool IsCorrect()
    {
        for(int i = 0; i <matchingZones.Count; i++)
        {
            if (matchingZones[i].IsMatched == false) return false;
        }

        Debug.Log("퍼즐 정답");

        // 퍼즐 완료 시 오브젝트 고정 
        for (int i = 0; i < matchingZones.Count; i++)
        {
            matchingZones[i].CurrentFile.gameObject.isStatic = true; // 파일 고정
            matchingZones[i].gameObject.SetActive(false); // 매칭존 끄기
        }
 
        GetReward();

        return true;
    }

    /// <summary>
    /// 퍼즐 완료 보상 (ex: 힌트 오브젝트, 점수, 완료 UI 등)
    /// </summary>
    private void GetReward()
    {
        Reward.SetActive(true);
    }
}
