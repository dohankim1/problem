using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove1 : MonoBehaviour
{
    public GameObject cleartext;
    public GameObject resetbutton;
    float speed = 5;
    float pluse = 0;
    float pluse2 = 0;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector3.zero;
        }
        if (collision.gameObject.CompareTag("End"))
        {
            cleartext.SetActive(true);
            resetbutton.SetActive(true);
        }
    }
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Lecture9_Midterm");
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.O)) 
        {
            pluse += -90;
            transform.rotation= Quaternion.Euler(0,pluse,0);
            if(pluse == 360)
            {
                pluse = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pluse2 += -90;
            transform.rotation = Quaternion.Euler(0, pluse2, 0);
            if (pluse2 == 360)
            {
                pluse2 = 0;
            }
        }
    }
}
