using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFileType
{
    Red,
    Blue,
    Black
}

public class MatchingZone : MonoBehaviour
{
    [Header("매칭할 파일 타입(컬러)")]
    public EFileType Type;
    public bool IsMatched { get; set; }

    private FileToOrganize CurrentFile;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<FileToOrganize>(out FileToOrganize file))
        {
            // 정답이 아닌 파일이어도 부착 시켜주기
            CurrentFile = file;
            CurrentFile.transform.position = this.transform.position;
            if (file.Type == Type)
            {
                IsMatched = true;
            }
            else
            {
                IsMatched = false;
            }
        }
    }
}
