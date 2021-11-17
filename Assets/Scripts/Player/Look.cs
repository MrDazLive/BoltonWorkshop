using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Look : MonoBehaviour
{
  public bool InvertYAxis = false;

  // Start is called before the first frame update
  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    var rotation = transform.eulerAngles;

    rotation.y += Input.GetAxis("Mouse X");
    rotation.x += InvertYAxis ? Input.GetAxis("Mouse Y") : -Input.GetAxis("Mouse Y");

    rotation.x = Mathf.Clamp(rotation.x < 180.0f ? rotation.x : rotation.x - 360.0f, -30.0f, 30.0f);

    transform.eulerAngles = rotation;

    DrawLineOfSight();
  }

  void DrawLineOfSight()
  {
    var hits = Physics.RaycastAll(transform.position, transform.forward);
    var hit = hits.OrderBy(hit => hit.distance).FirstOrDefault();

    var line = GetComponent<LineRenderer>();
    line.SetPosition(0, Vector3.zero);
    line.SetPosition(1, hit.point);
  }
}
