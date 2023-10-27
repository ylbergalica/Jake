using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShot : MonoBehaviour
{
	public bool isRay;
	public float burst;
	public float burstDelay;
	public GameObject projectile;
	public ScriptableObject itemReference;
    private IItem item;

	private Player player;
    private Quaternion lockRot;
    private Quaternion lockRotParent;
	private bool lockRotation = false;

	public float lockRotationTiming;
	private float length;

	private Animator animator;
	private AnimationClip clip;
	private string clipName;

    // Start is called before the first frame update
    void Start()
    {
		if (Random.Range(-1, 1) < 0) {
			transform.Rotate(0, 180, 0);
		}

		player = transform.parent.GetComponent<Player>();

        item = (IItem)itemReference;
		animator = gameObject.GetComponent<Animator>();
		clip = animator.runtimeAnimatorController.animationClips[0];
		length = clip.length;
		clipName = clip.name.Replace(".prefab", "");

		StartCoroutine(Shoot());
    }

	private IEnumerator Shoot() {
		if (burst > 1) {
			player.SetRotationSpeed(5f);
		}

		Vector3 realOffset = player.transform.up * item.GetStats()["offset"];
		float projectileSize = item.GetStats()["primary_projectile_size"];
		
		for (int i = 0; i < burst; i++) {
			animator.Play(clipName, 0, 0f);
			GameObject projectileObject = Instantiate(projectile, transform.position + transform.up * -2f, transform.rotation);
			projectileObject.transform.localScale = new Vector3(projectileSize, projectileSize, 1);

			IProjectile projectileRef = projectileObject.GetComponent(typeof(IProjectile)) as IProjectile;

			projectileRef.SetStats(
				item.GetStats()["primary_projectile_lifetime"],
				item.GetStats()["primary_projectile_speed"],
				item.GetStats()["primary_damage"],
				item.GetStats()["primary_knockback"]
			);
			
			projectileRef.AddHitObject(player.gameObject.name);
			// Shake the camera
			Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f + item.GetStats()["primary_knockback"]*0.005f, item.GetStats()["primary_knockback"]*1.1f);
			
			yield return new WaitForSeconds(burstDelay);
		}
		
		player.SetRotationSpeed(0f, lockRotationTiming);
	}
}
