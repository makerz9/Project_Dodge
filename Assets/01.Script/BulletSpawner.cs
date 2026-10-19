using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //프리팹 선언
    public GameObject bulletPrefab;
    private Transform target;//플레이어 위치 찾아

    public float interval = 1f;
    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("Player");
        target = findGo.transform;
        //태그를 보고 위치를 참조
    }

    // Update is called once per frame
    private void Update()
    {
        //이게 어떻게 반복이되지?
        timer += Time.deltaTime;
        if(timer > interval)
        {
            timer = 0f;

            Spawn();
        }
    }

    private void Spawn()
    {
        //위치 잡아주기
        Vector3 position = transform.position;
        Vector3 direction = (target.position - transform.position);

        Quaternion rotation = Quaternion.LookRotation(direction);

        Instantiate((bulletPrefab), position, rotation);
        Debug.Log("Spawn");


    }
}
