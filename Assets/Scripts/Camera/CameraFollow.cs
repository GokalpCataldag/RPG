using UnityEngine;
using UnityEngine.Rendering;

public class CameraFollow : MonoBehaviour
{
    public float followHeight = 7f;
    public float followDistance = 6f;
    public float followHeightSpeed = 0.9f;

    private Transform Player;

    private float targetHeight;
    private float currentHeight;
    private float currentRotation;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        targetHeight = Player.position.y + followHeight;

        currentHeight = transform.eulerAngles.y;

        currentHeight = Mathf.Lerp(transform.position.y, targetHeight, followHeightSpeed * Time.deltaTime);

        Quaternion euler = Quaternion.Euler(0f, currentRotation, 0f);

        Vector3 targetPosition = Player.position- (euler*Vector3.forward)*followDistance;

        targetPosition.y = currentHeight;

        transform.position = targetPosition;

        transform.LookAt(Player);
    }
}
