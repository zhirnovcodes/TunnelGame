using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Vector3 _angSpeed;

    void Update()
    {
        transform.Rotate(_angSpeed * Time.deltaTime);
    }
}
