using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileOrganization : MonoBehaviour, IPuzzleCheckable
{
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

        // TODO : 퍼즐 보상 등의 후처리

        return true;
    }

}
