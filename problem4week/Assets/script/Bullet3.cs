using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    float speed = 6f;
    stackreal stack;
    bool istrue;

    void Start()
    {
        istrue = true;
        stack = FindObjectOfType<stackreal>();
    }

    void Update()
    {
        if (istrue)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedBlock"))
        {
            GameObject.Destroy(gameObject);
            //gameObject.SetActive(false);
            stack.Pop();
            istrue = false;
        }
    }
}
