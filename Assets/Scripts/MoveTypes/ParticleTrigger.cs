using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ScriptableObject itemReference;
    private IItem item;

    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Awake() {
        item = (IItem)itemReference;
        
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject collider)
    {
        int numCollisionEvents = part.GetCollisionEvents(collider, collisionEvents);

        Vector3 pos = collisionEvents[0].intersection;

        if(collider.gameObject.tag == "Enemy"){
            IEnemy enemy = collider.GetComponent(typeof(IEnemy)) as IEnemy;
            enemy.Hurt(item.GetStats()["swing_damage"]);
            enemy.Knockback(part.transform, item.GetStats()["swing_knockback"]);

            // Do hit idicator when enemy gets hurtd
            // ContactPoint contact = collider.contacts[0];
            // Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, pos, Quaternion.identity);
			
			// Shake the camera
			Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f, 20f);
        }
    }
}
