using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public string sceneToLoad;
    // public GameObject camera;
    // public GameObject player;
    private Vector3 spawnPoint;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Switching to scene " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

            List<GameObject> invItems = other.GetComponent<Player>().GetItems();

            foreach (GameObject item in invItems) {
                if (item != null) {
                    DontDestroyOnLoad(item);
                }
            }
            // DontDestroyOnLoad(player);
            // DontDestroyOnLoad(camera);

            spawnPoint = this.transform.Find("SpawnPoint").transform.position;

            other.transform.position = spawnPoint;

            // Instantiate(other.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
