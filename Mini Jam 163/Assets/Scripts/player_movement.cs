using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_movement : MonoBehaviour
{
    public HashSet<GameObject> active_pies = new HashSet<GameObject>();
    public float sprint_multiplier;
    public float acceleration;
    public float jump_velocity;
    private bool[] keys_pressed = { false, false, false };
    private Rigidbody2D rb;
    private ground_check ground_Check;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground_Check = GetComponentInChildren<ground_check>();
        GameObject[] pies = GameObject.FindGameObjectsWithTag("Pie");
        foreach (GameObject p in pies) { active_pies.Add(p);}
    }

    private void Update()
    {
        //win condition
        if (active_pies.Count == 0)
        {
            SceneManager.LoadScene(3); //load win
        }

        //get key input
        if (Input.GetKey(KeyCode.A))
        {
            keys_pressed[0] = true;
        }
        else
        {
            keys_pressed[0] = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            keys_pressed[1] = true;
        }
        else
        {
            keys_pressed[1] = false;
        }
        if (Input.GetKey(KeyCode.Space) && ground_Check.grounded) 
        {
            keys_pressed[2] = true;
        }
        else
        {
            keys_pressed[2] = false;
        }
    }
    void FixedUpdate()
    {
        //apply key input
        if (keys_pressed[0] == true) // left
        {
            rb.velocity += acceleration * Vector2.left * Time.fixedDeltaTime;
        }
        if (keys_pressed[1] == true) // right
        {
            rb.velocity += acceleration * Vector2.right * Time.fixedDeltaTime;
        }
        if (keys_pressed[2] == true)  
        {
            rb.velocity += jump_velocity * Vector2.up * Time.fixedDeltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Eliminate")
        {
            SceneManager.LoadScene(2); //load game over
        }
        if (collision.gameObject.tag == "Pie")
        {
            active_pies.Remove(collision.gameObject); //close in on win condition
            Destroy(collision.gameObject);
        }
    }
}
