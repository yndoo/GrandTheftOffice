using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour
{
    public Color inkColor; // 이 오브젝트의 색상

    public Color GetColor()
    {
        return inkColor;
    }
}