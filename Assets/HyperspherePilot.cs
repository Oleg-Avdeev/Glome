using UnityEngine;

public class HyperspherePilot : MonoBehaviour
{
    public float R = 10;
    public Vector4 V;
    public Vector4 P;

    private int _historyIndex = 0;
    private int _historyLength = 50;
    private float _historyStep = 1f;
    public Color _historyColor = Color.white;
    private Vector3[] _history = new Vector3[50];

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.8f);
        Gizmos.DrawWireCube(new Vector3(0.5f, 0.5f, 1) * R, new Vector3(1, 1, 2) * R);        

        Gizmos.color = _historyColor;
        for (int i = 0; i < _historyLength; i++)
        {
            if (_history[i] != Vector3.zero)
            {
                Gizmos.DrawWireSphere(_history[i], 0.2f);        
            }
        }

        Gizmos.DrawLine(transform.position, transform.position + MapTo3D(V));
    }

    void Update()
    {
        P = MapTo4D(transform.position * Mathf.PI / R);
        // HandleKeyPress();

        var p = P;
        var p1 = p + V;
        var p2 = (R / p1.magnitude) * p1;

        V = (p2 - p).normalized;


        transform.position = MapTo3D(p2) / Mathf.PI * R;

        if ((transform.position - _history[_historyIndex]).magnitude > _historyStep)
        {
            _historyIndex = (_historyIndex + 1) % _historyLength;
            _history[_historyIndex] = transform.position;
        }

        var error = (p2 - MapTo4D(MapTo3D(p2))).magnitude;
        if (error > 0.0001)
        {
            Debug.LogError($"{error:0.00000}");
        }
    }

    private void HandleKeyPress()
    {
        float dp = 0;
        float dt = 0;
    
        if (Input.GetKeyDown(KeyCode.W)) dt = 0.1f;
        if (Input.GetKeyDown(KeyCode.S)) dt = -0.1f;
        if (Input.GetKeyDown(KeyCode.D)) dp = 0.1f;
        if (Input.GetKeyDown(KeyCode.A)) dp = -0.1f;

        Vector3 v3 = MapTo3D(V);
        float phi = Mathf.Atan2(v3.y, v3.x);
        float theta = Mathf.Atan2(v3.z, Mathf.Sqrt(v3.x*v3.x + v3.y*v3.y));
        Quaternion qr = Quaternion.Euler(phi+dp, theta+dt, 0);
        v3 = qr * Vector3.right;

        V = MapTo4D(v3);
    }

    private Vector3 MapTo3D(Vector4 h)
    {
        var v = new Vector3();
        v.x = Mathf.Acos(h.x / R);
        v.y = Mathf.Acos(h.y / Mathf.Sqrt(h.y * h.y + h.z * h.z + h.w * h.w));

        if (h.w >= 0)
        {
            v.z = Mathf.Acos(h.z/Mathf.Sqrt(h.z*h.z + h.w*h.w));
        }
        else
        {
            v.z = 2 * Mathf.PI - Mathf.Acos(h.z/Mathf.Sqrt(h.z*h.z + h.w*h.w));
        }

        if (h.w == 0)
        {
            if (h.z == 0)
            {
                v.z = 0;
                if (h.y == 0)
                {
                    v.y = 0;
                }
            }
        }

        return v;
    }

    private Vector4 MapTo4D(Vector3 s)
    {
        var v = new Vector4();
        v.x = R * Mathf.Cos(s.x);
        v.y = R * Mathf.Sin(s.x) * Mathf.Cos(s.y); 
        v.z = R * Mathf.Sin(s.x) * Mathf.Sin(s.y) * Mathf.Cos(s.z); 
        v.w = R * Mathf.Sin(s.x) * Mathf.Sin(s.y) * Mathf.Sin(s.z);
        return v;
    }
}
