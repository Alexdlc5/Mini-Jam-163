using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class player_movement : MonoBehaviour
{
    private float cd = .2f;
    public AudioSource jump;
    public AudioSource pickup;
    public AudioSource reset;
    public TextMeshProUGUI timer_display;
    public TextMeshProUGUI best_time;
    public TextMeshProUGUI pie_display;
    public HashSet<GameObject> active_pies = new HashSet<GameObject>();
    public float acceleration;
    public float jump_velocity;
    public float jump_boost_mult;
    private bool[] keys_pressed = { false, false, false };
    private Rigidbody2D rb;
    private ground_check ground_Check;
    private Vector2 spawn;
    private float reset_counter = 0;
    private float timer = 0;
    private int total_pies;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground_Check = GetComponentInChildren<ground_check>();
        GameObject[] pies = GameObject.FindGameObjectsWithTag("Pie");
        foreach (GameObject p in pies) { active_pies.Add(p);}
        spawn = transform.position;
        total_pies = pies.Length;
        if (player_data.best_time != 9999999) best_time.text = "Fastest Time: " +  player_data.best_time + "sec";
        else best_time.text = "Fastest Time: N/A";
        animator = GetComponentInChildren<Animator>();
    }
    void FixedUpdate()
    {
        pie_display.text = "Pies Found: " + (total_pies - active_pies.Count) + " / " + total_pies;
        timer += Time.fixedDeltaTime;
        timer_display.text = "Current Time: " + (int)timer + "sec";
        //reset 
        if (Input.GetKey(KeyCode.R))
        {
            if (reset_counter < 1)
            {
                reset_counter += Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = Vector2.zero; //spawn home
                transform.position = spawn;
                reset.Play();
                reset_counter = 0;
            }
        }
        else reset_counter = 0;
        //win condition
        if (active_pies.Count == 0)
        {
            if (player_data.best_time > timer || player_data.best_time == 9999999) player_data.best_time = (int)timer;
            player_data.previous_time = (int)timer;
            SceneManager.LoadScene(2); //load win
        }

        //get key input
        if (Input.GetKey(KeyCode.A) && ground_Check.grounded)
        {
            keys_pressed[0] = true;
        }
        else
        {
            keys_pressed[0] = false;
        }
        if (Input.GetKey(KeyCode.D) && ground_Check.grounded)
        {
            keys_pressed[1] = true;
        }
        else
        {
            keys_pressed[1] = false;
        }
        if (Input.GetKey(KeyCode.Space) && ground_Check.grounded && cd <= 0)
        {
            keys_pressed[2] = true;
        }
        else
        {
            cd -= Time.fixedDeltaTime;
            keys_pressed[2] = false;
        }
        //animation handler
        if (!ground_Check.grounded) animator.SetBool("InAir", true); //in air
        else animator.SetBool("InAir", false); //not in air
        if (keys_pressed[0] || keys_pressed[1]) //moving
        {
            //change direction
            if (keys_pressed[0]) animator.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            else animator.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            //set anim
            animator.SetBool("Moving", true);
        }
        else animator.SetBool("Moving", false); //not moving
        //apply key input
        if (keys_pressed[0] == true) //left
        {
            rb.velocity += acceleration * Vector2.left * Time.fixedDeltaTime;
        }
        if (keys_pressed[1] == true) //right
        {
            rb.velocity += acceleration * Vector2.right * Time.fixedDeltaTime;
        }
        if (keys_pressed[2] == true)  
        {
            rb.velocity += jump_velocity * Vector2.up * Time.fixedDeltaTime;
            rb.velocity *= new Vector2(jump_boost_mult,1);
            jump.Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Eliminate")
        {
            reset.Play();
            rb.velocity = Vector2.zero;
            transform.position = spawn;
        }
        if (collision.gameObject.tag == "Pie")
        {
            active_pies.Remove(collision.gameObject); //close in on win condition
            pickup.Play();
            Destroy(collision.gameObject);
        }
    }
}
