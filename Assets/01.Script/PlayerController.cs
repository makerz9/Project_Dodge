using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    public Rigidbody rb; //리지드바디 선언

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.GetComponent<Rigidbody>(); 이거링 뭔차이지 
        //
    }

    // Update is called once per frame
    private void Update()
    {
        //방향 입력받고 속도, 시간도 입력받기
        Vector3 direction = Vector3.zero;
        //축 단위로 입력
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        //대각선 가속 보정
        if(direction.magnitude > 1f)//왜 하필 1이지?
        {
            direction.Normalize();
        }
        rb.linearVelocity = direction * speed; //굴러가욧

        // 변위를 방향 * 속력 * Time.deltaTime;
        // Vector3 delta = direction * speed * Time.deltaTime;
        // transform.position += delta;
    }
}
