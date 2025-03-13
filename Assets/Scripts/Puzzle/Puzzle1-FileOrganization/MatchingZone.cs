using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFileType
{
    Red,
    Blue,
    Black
}

/// <summary>
/// 정답 Matching이 필요한 퍼즐에 사용 
/// </summary>
public abstract class MatchingSystem : MonoBehaviour
{
    public bool IsMatched { get; set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Matching(other);
    }

    /// <summary>
    /// 정답인지 체크하는 함수, 트리거 이벤트 발생 시 실행되는 함수
    /// </summary>
    /// <param name="other">트리거 된 충돌체 정보</param>
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
