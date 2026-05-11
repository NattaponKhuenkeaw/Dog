using UnityEngine;

[CreateAssetMenu(menuName = "Dog/Scene Reference")]
public class SceneReference : ScriptableObject
{
    [SerializeField] private string sceneName;

    public string SceneName => sceneName;
}
