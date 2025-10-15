using UnityEngine;

public class roomManag : MonoBehaviour
{
    public GameObject[] rooms;
    
    void Start()
    {
        int randIndex = Random.Range(0, rooms.Length);
        Instantiate(rooms[randIndex], Vector3.zero, Quaternion.identity);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
