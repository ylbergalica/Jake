using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playerPref;

    public void NewGame()
    {
        Vector3 spawnPoint = new Vector3(-730,600,0);

        SceneManager.LoadScene("SandboxArea", LoadSceneMode.Single);
        // GameObject.Find("Circle").GetComponent<BallMove>().SetGameOver(false);

        GameObject player = Instantiate(playerPref, spawnPoint, Quaternion.identity);
        GameObject.Find("Camera").GetComponent<CameraFollow>().targetFollow = player;
    }
}
