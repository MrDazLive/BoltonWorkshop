using System;
using System.Collections;
using UnityEngine;

public class Magazine : MonoBehaviour
{
  [SerializeField]
  private int m_clipSize = 30;

  [SerializeField]
  private float m_reloadTime = 3.0f;

  private int m_clipCapacity;
  private bool m_reloading = false;

  public bool IsReloading { get { return m_reloading; } }

  public void Reload()
  {
    if (!m_reloading && m_clipCapacity != m_clipSize)
      StartCoroutine(nameof(ReloadAsync));
  }

  public void Fire(Action fireFunc)
  {
    if(m_clipCapacity == 0)
    {
      Reload();
    }
    else if(!m_reloading)
    {
      fireFunc.Invoke();
      --m_clipCapacity;
    }
  }

  private IEnumerator ReloadAsync()
  {
    Debug.Log("Reloading...");
    m_reloading = true;
    yield return new WaitForSeconds(m_reloadTime);
    m_clipCapacity = m_clipSize;
    m_reloading = false;
    Debug.Log("Reloaded.");
  }
  
  // Start is called before the first frame update
  void Start()
  {
    m_clipCapacity = m_clipSize;
  }
}
