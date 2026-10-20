
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

[SerializeField] private float maxHp = 100;
[SerializeField] private float curHp;

public float CurHp
    {
        get
        {
            return curHp;
        }
        set
        {
            curHp = value;
            curHp = Mathf.Clamp(value, 0f, maxHp);
        }
    }


[SerializeField] private float moveForce = 30f;    //미는 힘의 세기
[SerializeField] private float maxSpeed = 8f;      //XZ 최고 속도 제한

[SerializeField] private GameObject firePoint;
[SerializeField] private GameObject bullet;

[SerializeField] private float spreadX = 8f; //좌우 최대 퍼짐 각도
[SerializeField] private float spreadY = 4f; //위아래 최대 퍼짐 각도


[SerializeField] private float aimDuration = 0.3f; //0,0,0 으로 돌아가는 시간(초)
private Tween aimTween;                             //진행 중인 트윈 참조

private Vector3 inputDirection;                    //Update에서 읽고 FixedUpdate에서 사용

    public Rigidbody rb; //리지드바디 선언

    private void Awake()
    {
        curHp = maxHp;
    }

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
        // Vector3 velocity = direction * speed;
        // velocity.y = rb.linearVelocity.y; //현재 Y 속도(중력 누적분)를 그대로 유지
        // rb.linearVelocity = velocity;

        inputDirection = direction;   //대각선 보정이 끝난 값을 저장만 함

        // 변위를 방향 * 속력 * Time.deltaTime;
        // Vector3 delta = direction * speed * Time.deltaTime;
        // transform.position += delta;

        // if(Input.GetButtonDown("Fire1"))
        // {
        //     //ShotGun
        //     Debug.Log("클릭");

        //     //Vector3.back은 월드 좌표 기준 고정 벡터
        //     //rb.AddForce(Vector3.back * speed, ForceMode.Impulse);

        //     Instantiate((bullet), firePoint.transform.position, transform.rotation);
        //     //캐릭터가 바라보는 방향 기준으로 하고 싶다면 Transform의 로컬 축 프로퍼티
        //     rb.AddForce(-transform.forward  * maxSpeed, ForceMode.Impulse);
        // }

        if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0f, 2, 0f, Space.World);
        }
        if(Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0f, -2, 0f, Space.World);
        }


        if(Input.GetButtonDown("Fire1"))
        {
            //ShotGun
            Debug.Log("클릭");

            //Vector3.back은 월드 좌표 기준 고정 벡터
            //rb.AddForce(Vector3.back * speed, ForceMode.Impulse);





            for(int i=0; i <6; i++)
            {
            float randX = Random.Range(-spreadY, spreadY); //위아래 퍼짐 (X축 회전)
            float randY = Random.Range(-spreadX, spreadX); //좌우 퍼짐 (Y축 회전)
            Quaternion nowAngle = transform.rotation;
            Quaternion nextAngle = Quaternion.Euler(randX, randY, 0);
            Quaternion finalangle = nowAngle * nextAngle;
            Instantiate((bullet), firePoint.transform.position, finalangle);
                
            }
            Instantiate((bullet), firePoint.transform.position, transform.rotation);

            //캐릭터가 바라보는 방향 기준으로 하고 싶다면 Transform의 로컬 축 프로퍼티
            rb.AddForce(-transform.forward  * maxSpeed, ForceMode.Impulse);
        }

        if(Input.GetButtonDown("Fire2"))
        {
            //Aiming
            Debug.Log("클릭");

            aimTween?.Kill();   //진행 중인 트윈이 있으면 중단 (Q/E 회전과 충돌 방지)
            //aimTween = transform.DORotate(Vector3(0, transform.rotation, 0), aimDuration).SetEase(Ease.OutQuad);
            aimTween = transform.DORotate(new Vector3(0f, transform.eulerAngles.y, 0f), aimDuration).SetEase(Ease.OutQuad);
        }   
    }

    #region Movement1
    private void FixedUpdate()
    {
        //입력 방향으로 힘을 가함 (속도를 덮어쓰지 않으므로 중력, AddForce와 충돌 없음)
        rb.AddForce(inputDirection * moveForce, ForceMode.Acceleration);

        //XZ 속도만 따로 떼어 최고 속도 제한 (Y는 건드리지 않아 낙하 정상)
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limited = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }
    }

    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            TakeDamage(10);
        }
    }


    public void TakeDamage(int damage)
    {
        CurHp -= damage;

        if(CurHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }



}
