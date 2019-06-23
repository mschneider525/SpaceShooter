using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    protected GameObject _explosionLaserPrefab = null;
    [SerializeField]
    protected GameObject _explosionEnemyLaserPrefab = null;

    protected IEnumerator DamageColorChange_Routine(GameObject gameObject)
    {
        SpriteRenderer spriteRenderer = null;

        if (gameObject.tag == "Asteroid")
        {
            spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        

        for (int i = 1; i <= 4; i++)
        {
            yield return new WaitForSeconds(0.1f);

            switch (i)
            {
                case 1:
                case 3:
                    spriteRenderer.color = Color.red;
                    break;
                case 2:
                case 4:
                    if (gameObject.name == "Player1(Clone)")
                    {
                        spriteRenderer.color = new Color(0.2122642f, 0.6918032f, 1.0f);
                    }
                    else if (gameObject.name == "Player2(Clone)")
                    {
                        spriteRenderer.color = new Color(1.0f, 0.25f, 0.25f);
                    }
                    else
                    {
                        spriteRenderer.color = Color.white;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    protected void ObjectExplosion(GameObject gameObject, Vector3 position)
    {
        GameObject gameObjectExplosion = Instantiate(gameObject, position, Quaternion.identity);
        Destroy(gameObjectExplosion, 2.5f);
    }
}
