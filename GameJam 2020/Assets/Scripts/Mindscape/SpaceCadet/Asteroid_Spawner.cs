using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid_Spawner : MonoBehaviour
{ 
  public GameObject asteroid;
  void Start() 
  {
    StartCoroutine(Spawn_Enemy());
  }

  public IEnumerator Spawn_Enemy()
  {
      Debug.Log("Hiii");
      yield return new WaitForSeconds(Random.value * 2 + 1);
      Debug.Log("Good");
      Instantiate (asteroid, new Vector3(0, 0, 0), Quaternion.identity);
      StartCoroutine(Spawn_Enemy());
  }
}