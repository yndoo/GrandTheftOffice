using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FileOrganization : Puzzle, IPuzzleCheckable
{
    public AudioClip putClip;
    public AudioSource audioSource;

    List<MatchingZone> matchingZones;

    private void Awake()
    {
        matchingZones = new List<MatchingZone>();
        foreach(MatchingZone zone in GetComponentsInChildren<MatchingZone>())
        {
            matchingZones.Add(zone);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PutInFolder() //사운드
    {
        audioSource.PlayOneShot(putClip);

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
 
        Complete();

        return true;
    }

    /// <summary>
    /// 퍼즐 완료 UI, 보상 지급 등
    /// </summary>
    private void Complete()
    {
        GetReward();
    }
}
