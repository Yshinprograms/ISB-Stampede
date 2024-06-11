using UnityEngine;
public class LandmarkScript : MonoBehaviour
{
    void Update()
    {
        transform.position = ChineseTourist.landmark;
    }
}
