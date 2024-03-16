using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3f;
    Queue queue;
    bool istrue;
    // Start is called before the first frame update
    void Start()
    {
        istrue= true;
        queue = FindObjectOfType<Queue>();
    }

    // Update is called once per frame
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
            queue.count--;
            istrue= false;
        }
    }
}
