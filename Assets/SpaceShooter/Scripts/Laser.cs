using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.0f;


    // Update is called once per frame
    void Update()
    {
        if (this.tag == "Laser" || this.tag == "TripleShot")
        {
            _speed = 10.0f;

            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y > 5.75f)
            {
                Destroy(this.gameObject);
            }
        }

        if (this.tag == "Laser_Enemy")
        {
            _speed = 5.0f;

            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -4.75f)
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
