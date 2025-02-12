using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHit : ShootableObject
{
    public GameObject particlesPrefab;
    public float particleLifeTime = 5f;

    public override void OnHit(ref HitInfo hitInfo)
    {
       GameObject instantiatedParticles = (GameObject)Instantiate(particlesPrefab, 
                                                                   hitInfo.hit.point + hitInfo.hit.normal * 0.005f,
                                                                   Quaternion.FromToRotation(Vector3.up,hitInfo.hit.normal),
                                                                   transform.root.parent);
       Destroy(instantiatedParticles, particleLifeTime);
       hitInfo.destroyBullet=true;

    }
}


