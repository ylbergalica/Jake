using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject camera;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Switching to scene " + sceneToLoad);

            List<GameObject> invItems = player.GetComponent<Player>().GetItems();

            foreach (GameObject item in invItems) {
                DontDestroyOnLoad(item);
            }
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(camera);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

            // Instantiate(other.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
