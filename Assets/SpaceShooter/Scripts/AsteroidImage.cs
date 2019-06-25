using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidImage : MonoBehaviour
{
    private float _spinSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_spinSpeed == 0.0f)
        {
            _spinSpeed = GetRandomSpinSpeed();
        }

        if (Time.timeScale != 0) //If game is not paused
        {
            this.transform.Rotate(0, 0, _spinSpeed); 
        }
    }

    private float GetRandomSpinSpeed()
    {
        float spinSpeed = 0.0f;

        if (Random.Range(0, 2) == 1)
            spinSpeed = Random.value;
        else
            spinSpeed = -Random.value;

        return spinSpeed;
    }
}
