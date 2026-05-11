using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        Services.SceneLoader = this;
    }

    public void LoadScene(SceneReference sceneReference, SceneTransitionData transitionData = null)
    {
        if (sceneReference == null)
        {
            Debug.LogWarning("SceneLoader received a null SceneReference.");
            return;
        }

        LoadSceneByName(sceneReference.SceneName, transitionData);
    }

    public void LoadSceneByName(string sceneName, SceneTransitionData transitionData = null)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("SceneLoader received an empty scene name.");
            return;
        }

        ApplyTransitionData(transitionData);
        SceneManager.LoadScene(sceneName);
    }

    public void RestartSession(string sceneName)
    {
        LoadSceneByName(sceneName, SceneTransitionData.ForRestart());
    }

    public void RestartSession(SceneReference sceneReference)
    {
        LoadScene(sceneReference, SceneTransitionData.ForRestart());
    }

    private void ApplyTransitionData(SceneTransitionData transitionData)
    {
        if (transitionData == null)
        {
            return;
        }

        if (transitionData.ResetSession)
        {
            Services.Session?.ResetGameState();
        }

        if (transitionData.ClearPlayerPosition && Services.Session != null)
        {
            Services.Session.LastPlayerPosition = Vector3.zero;
        }

        if (!string.IsNullOrWhiteSpace(transitionData.LockedDoorId))
        {
            Services.Doors?.LockDoor(transitionData.LockedDoorId);
        }
    }
}
