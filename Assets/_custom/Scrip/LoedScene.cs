using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class DoorClick : MonoBehaviour
{
    public string sceneName;
    public static bool roomOpened = false; // กันโหลดซ้ำ






   

    public void OpenDoor()
    {
       
        SceneManager.LoadScene(sceneName);
    }


    public void changScene()
    {
        GameManager.instance.lastPlayerPosition = Vector3.zero;
        GameManager.instance.isDead = false;
        GameManager.instance.flashlightPower = GameManager.instance.maxFlashlightPower;
        SceneManager.LoadScene(sceneName);

    }

    /* void Update()
      {




          / Mouse (PC)
          if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
          {
              Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
              Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

              RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
              Debug.Log("Click ที่: " + touchPos);



              if (hit.collider != null && hit.collider.gameObject == gameObject)
              {
                  if (playerIsNear)
                  {
                      Debug.Log("เข้า Door " + sceneName);
                      SceneManager.LoadScene(sceneName);
                  }
                  else
                  {
                      Debug.Log("แตะประตู แต่ Player ยังไม่ใกล้");
                  }
              }
          }

          // Touch (มือถือ)
          if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
          {
              Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
              Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

              RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
              Debug.Log("Tap ที่: " + touchPos);

              if (hit.collider != null)
              {
                  Debug.Log("Raycast โดน: " + hit.collider.name);
              }

              if (hit.collider != null && hit.collider.gameObject == gameObject)
              {
                  if (playerIsNear)
                  {
                      Debug.Log("เข้า Door " + sceneName);
                      SceneManager.LoadScene(sceneName);
                  }
                  else
                  {
                      Debug.Log("แตะประตู แต่ Player ยังไม่ใกล้");
                  }
              }
          }
      }*/



}
