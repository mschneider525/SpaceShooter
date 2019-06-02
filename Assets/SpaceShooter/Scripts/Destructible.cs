using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public IEnumerator DamageColorChange_Routine(GameObject gameObject)
    {
        //Color originalColor = spriteRenderer.color;

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

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

    public void ObjectExplosion(GameObject gameObject)
    {
        GameObject gameObjectExplosion = Instantiate(gameObject, this.transform.position, Quaternion.identity);
        Destroy(gameObjectExplosion, 2.5f);
    }
}
