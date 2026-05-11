using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public Vector3 LastPlayerPosition { get; set; }

    private void Awake()
    {
        Services.Session = this;
    }

    public void ResetGameState()
    {
        Services.Health?.ResetState();
        Services.Energy?.ResetState();
        Services.Flashlight?.ResetState();
        Services.Inventory?.ResetState();
        Services.Doors?.Clear();
    }

    public void RestartSession(string sceneName)
    {
        LastPlayerPosition = Vector3.zero;
        ResetGameState();
        Services.SceneLoader?.LoadSceneByName(sceneName);
    }

    public void RestartSession(SceneReference sceneReference)
    {
        LastPlayerPosition = Vector3.zero;
        ResetGameState();
        Services.SceneLoader?.LoadScene(sceneReference);
    }

    public bool TryRestoreTaggedPlayerPosition()
    {
        if (LastPlayerPosition == Vector3.zero)
        {
            return false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return false;
        }

        player.transform.position = LastPlayerPosition;
        return true;
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
