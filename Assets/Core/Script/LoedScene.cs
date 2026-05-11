using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class DoorClick : MonoBehaviour
{
    public string sceneName;
    public static bool roomOpened = false; // กันโหลดซ้ำ
    public Door door;
    public void OpenDoor()
    {
        
        

        SceneManager.LoadScene(sceneName);
        GameManager.instance.LockDoor(door.doorID);
    }


    public void changScene()
    {
        GameManager.instance.lastPlayerPosition = Vector3.zero;
        GameManager.instance.isDead = false;
        GameManager.instance.flashlightPower = GameManager.instance.maxFlashlightPower;
        SceneManager.LoadScene(sceneName);
        GameManager.instance.ResetGameState();
    }

    public void Quit()
    {
        Debug.Log("Game Quit");  // ทดสอบตอนอยู่ใน Editor
        Application.Quit();      // คำสั่งออกจากเกมจริงตอน Build
    }







}


