using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float fallDelay = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("WaitAndFall");
    }

    IEnumerator WaitAndFall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.constraints = RigidbodyConstraints2D.None;

    }

    
}
