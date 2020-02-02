using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid_Spawner : MonoBehaviour
{ 
  public GameObject asteroid;
  public GameObject player;
  private float playerVel;
  private float maxVel;

  void Start() 
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerVel = player.GetComponent<Spaceship_Controller>().verticalVelocity;
    maxVel = player.GetComponent<Spaceship_Controller>().maxVertVelocity;
    StartCoroutine(Spawn_Enemy());
  }

  public IEnumerator Spawn_Enemy()
  {
      playerVel = player.GetComponent<Spaceship_Controller>().verticalVelocity;
      yield return new WaitForSeconds(Random.value * (maxVel - playerVel) / maxVel + 0.1f);
      Instantiate (asteroid, new Vector3(0, 0, 0), Quaternion.identity);
      StartCoroutine(Spawn_Enemy());
  }
}