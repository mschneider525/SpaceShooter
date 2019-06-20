using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.75f)
        {
            Destroy(this.gameObject);
        }
    }
}
