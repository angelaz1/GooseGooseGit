using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Event : MonoBehaviour
{
    public bool can_combo = false;

    public int weapon_damage = 1;

    public List<Collider2D> hit_things = new List<Collider2D>();

    public void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Start_Combo_Window()
    {
        can_combo = true;
    }

    public void End_Combo_Window()
    {
        can_combo = false;
    }

    public void Attack()
    {
        StartCoroutine(Attacka());
    }

    public IEnumerator Attacka()
    {
        hit_things.Clear();
        Debug.Log("Attacka");
        GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider2D>().enabled = false;
        hit_things.Clear();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hit_things.Contains(collision))
        {
            if (collision.GetComponent<Enemy_Controller>() != null)
            {
                Player_Controller player = GetComponentInParent<Player_Controller>();
                Enemy_Controller enem = collision.GetComponent<Enemy_Controller>();
                //enem.Add_Impact(1f, enem.char_.transform.position - player.char_.transform.position);
                enem.Take_Damage(weapon_damage);
            }
            hit_things.Add(collision);
        }
    }
}
