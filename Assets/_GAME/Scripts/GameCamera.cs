using UnityEngine;

public class GameCamera : MonoBehaviour
{
    bool isTap;
    Vector3 touchPosition;
    void Update()
    {
#if !UNITY_EDITOR
        isTap = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        touchPosition = Input.GetTouch(0).position;
#else
        isTap = Input.GetMouseButtonDown(0);
        touchPosition = Input.mousePosition;
#endif

        if (isTap)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Sphere sphere = hit.transform.GetComponent<Sphere>();
                sphere.Click();
            }
        }
    }
}
