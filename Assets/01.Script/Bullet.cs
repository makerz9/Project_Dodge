using UnityEngine;

public class Bullet : MonoBehaviour
{
    //물리 선언
    private Rigidbody rb;
    private float speed = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.GetComponent<Rigidbody>(); 왜안됨??
        rb.linearVelocity = transform.forward * speed;
        //앞으로 속도를 가지고 이동
        //linearVelocity <-?
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Rigidbody playerRb = other.GetComponent<Rigidbody>();
            // playerRb.AddForce(Vector3.up * speed, ForceMode.Impulse);

            other.gameObject.SetActive(false);
            Destroy(gameObject);

        }

        if(!other.CompareTag("Bullet"))
        {
           Destroy(gameObject);
        }
    }



}
