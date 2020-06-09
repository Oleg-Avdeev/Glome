using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float R = 10;
    public Quaternion Q;
    public Vector3 v3 = Vector3.right;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 1f);
        Gizmos.DrawLine(transform.position, transform.position + Q * Vector3.forward * R);
        Gizmos.DrawLine(transform.position, transform.position + Q * Vector3.right * R);
        Gizmos.DrawLine(transform.position, transform.position + Q * Vector3.up * R);
        Gizmos.color = new Color(1, 0, 1, 0.8f);
        Gizmos.DrawLine(transform.position, transform.position + v3 * R);
    }

    void Update()
    {
        HandleKeyPress();
    }

    private void HandleKeyPress()
    {
        float dp = 0;
        float dt = 0;
    
        if (Input.GetKey(KeyCode.W)) dt = 1f;
        if (Input.GetKey(KeyCode.S)) dt = -1f;
        if (Input.GetKey(KeyCode.D)) dp = 1f;
        if (Input.GetKey(KeyCode.A)) dp = -1f;

        float phi =   (180f / Mathf.PI) * Mathf.Atan2(v3.y, v3.x);
        float theta = (180f / Mathf.PI) * Mathf.Atan2(v3.z, Mathf.Sqrt(v3.x*v3.x + v3.y*v3.y));
        Q = Quaternion.Euler(0, theta, phi);
        v3 = Quaternion.Euler(0, dp, dt) * v3;
    }

}
