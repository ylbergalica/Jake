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

        GameObject camera = GameObject.Find("Camera");
		camera.GetComponent<CameraFollow>().targetFollow = player;

		Transform UI = camera.transform.Find("UI");
		Transform UI_Inventory = UI.Find("Canvas").Find("UI_Inventory");
		UI.gameObject.SetActive(true);

		player.GetComponent<Player>().AddInventoryUI(UI_Inventory.gameObject.GetComponent<UI_Inventory>());

		GameObject UI_HealthBar = UI.Find("Canvas").Find("HealthBar").gameObject;
		UI_HealthBar.GetComponent<HealthBar>().player = player.GetComponent<Player>();
    }
}
