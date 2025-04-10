using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float time;
    void Start()
    {
        Destroy(this.gameObject,time);
    }

    
}
