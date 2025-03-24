using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class movment : MonoBehaviour
{
 public float moveSpeed = 5f; // Velocità di movimento del player
    private Rigidbody rb;       // Riferimento al Rigidbody
    private Vector3 moveDirection;

    void Start()
    {
        // Ottieni il riferimento al Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ottieni input dal giocatore
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcola la direzione del movimento
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized; // Normalizza per evitare velocità troppo alte
    }

    void FixedUpdate()
    {
        // Applica il movimento con il Rigidbody
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        if (moveDirection != Vector3.zero)
        {
           Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
           rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, 360 * Time.deltaTime);
        }
    }
}
