using System;
using System.Linq;
using UnityEngine;

public static class Bullet
{
  public static GameObject Shoot(Vector3 position, Vector3 direction, Func<GameObject, bool> hitFilter)
  {
    var hits = Physics.RaycastAll(position, direction).OrderBy(hit => hit.distance).ToList();

    if (hits.Any())
    {
      var hit = hits.First();
      if (hitFilter(hit.transform.gameObject))
      {
        Debug.DrawLine(position, hit.point, Color.green, 2.0f);
        return hit.transform.gameObject;
      }
      else
      {
        Debug.DrawLine(position, hit.point, Color.yellow, 2.0f);
        return null;
      }
    }
    else
    {
      Debug.DrawRay(position, direction.normalized * 15.0f, Color.red, 2.0f);
      return null;
    }
  }
}
