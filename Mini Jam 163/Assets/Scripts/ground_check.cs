using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground_check : MonoBehaviour
{
    public bool grounded = false;
    public HashSet<GameObject> ground_touched = new HashSet<GameObject>();
    private void Update()
    {
        if (ground_touched.Count == 0) grounded = false; else grounded = true; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !ground_touched.Contains(collision.gameObject))
        {
            ground_touched.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ground_touched.Remove(collision.gameObject);
        }
    }
}
