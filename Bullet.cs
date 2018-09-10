using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private bool bulletFlyMarker;
    private float traveledDistance;
    private Vector3 startingPoint;
    private const short BULLET_SPEED = 300;
    private float weaponDamage;
    private Life enemyLife;
    private float weaponRange;
    public bool BulletFlyMarker {
        get { return bulletFlyMarker; }
        set { bulletFlyMarker = value; }
    }
    public float SetWeaponRange {
        set { weaponRange = value; }
    }
    public float SetTraveledDistance {
        set { traveledDistance = value; }
    }
    public float SetWeaponDamage {
        set { weaponDamage = value; }
    }
    public Vector3 SetStartingPoint {
        set { startingPoint = value; }
    }

    private void Update() {
        if (bulletFlyMarker)
            BulletFly(weaponRange, startingPoint);
    }

    // Bullet flying logic
    public void BulletFly(float weaponRange, Vector3 startingPoint) {
        traveledDistance += Time.deltaTime * BULLET_SPEED;
        Debug.DrawRay(startingPoint, transform.forward * traveledDistance, Color.cyan);

        RaycastHit hit;

        if (Physics.Raycast(startingPoint, transform.forward, out hit, traveledDistance)) {
            bulletFlyMarker = false;
            gameObject.SetActive(false);

            if (hit.transform.tag == "Ground") {
                ParticleSystem groundImpact = PoolManager.GetObject("groundImpact", hit.point, Quaternion.LookRotation(hit.normal)).GetComponent<ParticleSystem>();
                groundImpact.Play();

            } else if (hit.transform.tag == "Concrete") {
                ParticleSystem concreteImpact = PoolManager.GetObject("concreteImpact", hit.point, Quaternion.LookRotation(hit.normal)).GetComponent<ParticleSystem>();
                concreteImpact.Play();
            }

            if (hit.transform.gameObject.GetComponent("Life")) {
                enemyLife = hit.transform.gameObject.GetComponent<Life>();
                enemyLife.GetDamage(weaponDamage);
                traveledDistance = 0;
            }
        }

        if (traveledDistance >= weaponRange)
            traveledDistance = weaponRange;

        if (traveledDistance == weaponRange) {
            gameObject.SetActive(false);
            bulletFlyMarker = false;
            traveledDistance = 0;
        }

        transform.position += transform.forward * Time.deltaTime * BULLET_SPEED;        
    }
}
