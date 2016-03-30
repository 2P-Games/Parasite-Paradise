using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        switch (this.gameObject.tag)
        {
            /** Special tagged case collisions, if desired **/
            default:
                // Projectile is destroyed
                Object.Destroy(this.gameObject);
                break;
        }

    }
}
