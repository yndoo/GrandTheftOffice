/*Please do support www.bitshiftprogrammer.com by joining the facebook page : fb.com/BitshiftProgrammer
Legal Stuff:
This code is free to use no restrictions but attribution would be appreciated.
Any damage caused either partly or completly due to usage this stuff is not my responsibility*/
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float MaxWobble = 0.03f; // 최대 흔들림
    public float WobbleSpeed = 5.0f; // 흔들림 속도
    public float RecoveryRate = 1f; // 회복 속도

    Renderer rend;
    Vector3 prevPos;
    Vector3 prevRot;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        // 시간이 지남에 따라 흔들림 감소
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * RecoveryRate);
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * RecoveryRate);

        // 감소하는 흔들림의 사인파 만들기
        float wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(WobbleSpeed * Time.time);
        float wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(WobbleSpeed * Time.time);

        // 셰이더에 전달
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // 이동 속도
        Vector3 moveSpeed = (prevPos - transform.position) / Time.deltaTime;
        Vector3 rotationDelta = transform.rotation.eulerAngles - prevRot;

        // 클램프된 속도를 흔들림에 추가
        wobbleAmountToAddX += Mathf.Clamp((moveSpeed.x + (rotationDelta.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((moveSpeed.z + (rotationDelta.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // 마지막 위치 저장
        prevPos = transform.position;
        prevRot = transform.rotation.eulerAngles;
    }
}
