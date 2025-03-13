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

/// <summary>
/// Puzzle1의 매칭 존 (추후 이름 변경 필요)
/// </summary>
public class MatchingZone : MatchingSystem
{
    [Header("매칭할 파일 타입(컬러)")]
    public EFileType Type;

    [HideInInspector] public FileToOrganize CurrentFile;
    private FileOrganization Puzzle1;

    private void Awake()
    {
        Puzzle1 = transform.parent.GetComponent<FileOrganization>();
    }

    public override void Matching(Collider other)
    {
        if (other.TryGetComponent<FileToOrganize>(out FileToOrganize file))
        {
            // 정답이 아닌 파일이어도 부착 시켜주기
            CurrentFile = file;
            CurrentFile.transform.position = this.transform.position;
            CurrentFile.transform.rotation = this.transform.rotation;
            if (file.Type == Type)
            {
                IsMatched = true;
                Puzzle1.IsCorrect();
            }
            else
            {
                IsMatched = false;
            }
        }
    }
}
