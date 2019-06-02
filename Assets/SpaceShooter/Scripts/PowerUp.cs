using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //0 = TripleShot
    //1 = SpeedBoost
    //2 = Shield
    [SerializeField]
    private int powerUpID = 0;

    [SerializeField]
    private AudioClip _audioClip = null;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroyPowerUp_Routine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //access player gameObject
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.Toggle_PowerUp("TripleShot");
                        break;
                    case 1:
                        player.Toggle_PowerUp("SpeedBoost");
                        break;
                    case 2:
                        player.Toggle_PowerUp("Shield");
                        break;
                    default:

                        break;
                }
            }

            //Too loud, so I moved the audio position farther away from the main camera
            Vector3 audioPosition = Camera.main.transform.position;
            audioPosition.z = -25.0f;

            AudioSource.PlayClipAtPoint(_audioClip, audioPosition);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyPowerUp_Routine()
    {
        yield return new WaitForSeconds(3.0f);

        Destroy(this.gameObject);
    }
}
