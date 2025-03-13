using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFileType
{
    Red,
    Blue,
    Black
}

public abstract class MatchingSystem : MonoBehaviour
{
    public bool IsMatched { get; set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Matching(other);
    }

    public abstract void Matching(Collider other);
}

public class MatchingZone : MatchingSystem
{
    [Header("매칭할 파일 타입(컬러)")]
    public EFileType Type;

    private FileToOrganize CurrentFile;

    public override void Matching(Collider other)
    {
        if (other.TryGetComponent<FileToOrganize>(out FileToOrganize file))
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
