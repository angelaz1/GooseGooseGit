using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Weapon : MonoBehaviour
{
    public int damage;

    public BoxCollider2D weapon_hitbox;

    public List<Collider2D> hit_things = new List<Collider2D>();

    public void Start()
    {
        weapon_hitbox.enabled = false;
    }

    public void Attack()
    {
        StartCoroutine(Attacka());
    }

    public IEnumerator Attacka()
    {
        hit_things.Clear();
        Debug.Log("Attacka");
        weapon_hitbox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        weapon_hitbox.enabled = false;
        hit_things.Clear();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hit_things.Contains(collision))
        {
            if (collision.GetComponent<Player_Controller>() != null)
            {
                Player_Controller player = collision.GetComponent<Player_Controller>();
                player.Take_Damage(damage);
                Enemy_Controller enem = GetComponentInParent<Enemy_Controller>();
                player.Add_Impact(8f, player.char_.transform.position - enem.char_.transform.position);
            }
            hit_things.Add(collision);
        }
    }
}
