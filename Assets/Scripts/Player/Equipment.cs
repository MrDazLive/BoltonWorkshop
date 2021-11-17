using System.Linq;
using UnityEngine;

public class Equipment : MonoBehaviour
{
  private IWeapon m_activeWeapon;

  // Start is called before the first frame update
  void Start()
  {
    ActivateWeapon(GetComponentInChildren<IWeapon>(true));
  }

  void CheckTrigger(eTrigger trigger)
  {
    if (Input.GetMouseButtonDown((int)trigger))
      m_activeWeapon.OnTriggerDown(trigger);
    if (Input.GetMouseButton((int)trigger))
      m_activeWeapon.OnTriggerHold(trigger);
    if (Input.GetMouseButtonUp((int)trigger))
      m_activeWeapon.OnTriggerRelease(trigger);
  }

  // Update is called once per frame
  void Update()
  {
    var weapons = gameObject.GetComponentsInChildren<IWeapon>(true);
    for (int i = 0; i < Mathf.Min(8, weapons.Count()); ++i)
      if (Input.GetKeyDown((i + 1).ToString()))
        ActivateWeapon(weapons[i]);

    CheckTrigger(eTrigger.PRIMARY);
    CheckTrigger(eTrigger.SECONDARY);
  }

  void ActivateWeapon(IWeapon weapon)
  {
    if (weapon == m_activeWeapon)
      return;

    (m_activeWeapon as MonoBehaviour)?.gameObject.SetActive(false);
    m_activeWeapon = weapon;
    (m_activeWeapon as MonoBehaviour)?.gameObject.SetActive(true);
  }
}
