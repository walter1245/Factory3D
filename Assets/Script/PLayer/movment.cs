using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movment : MonoBehaviour {

    public float movmentSpeed=5f;
    private Rigidbody rb;
    private Vector3 movement;
    
    public Texture2D walkabilityMap; // Texture della maschera per ostacoli
    public Vector2 mapSize; // Dimensione del mondo della mappa
    public Color walkableColor = Color.white;
    public SpriteRenderer mapSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Calcola la dimensione della mappa utilizzando il riferimento al SpriteRenderer
        if (mapSpriteRenderer != null) {
            Bounds bounds = mapSpriteRenderer.sprite.bounds;
            mapSize = new Vector2(bounds.size.x, bounds.size.y);
        } else {
            Debug.LogError("SpriteRenderer della mappa non assegnato!");
        }
    }

    // Update is called once per frame
    void Update() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcola la direzione del movimento
        movement = new Vector3(moveX, 0f, moveZ).normalized; // Normalizza per evitare velocità troppo alte
       
    }

    private void FixedUpdate() {
        // Calcola la nuova posizione del giocatore
        Vector3 newposition= rb.position + movement * movmentSpeed * Time.fixedDeltaTime;
        
        // Controlla se la nuova posizione è valida
        if (IsWalkable(newposition)) {
            rb.MovePosition(newposition);

            Debug.Log("wwww");
            if (movement!= Vector3.zero) {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, 360 * Time.deltaTime);
            }
        }
    }

    private bool IsWalkable(Vector3 worldposition) {
        // Converti la posizione del mondo in coordinate UV della texture
        Vector2 uv= WorldToUV(worldposition);
        
        // Controlla se UV è dentro i limiti della texture
        if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1) {
            Debug.Log($"UV Coordinates: {uv.x}, {uv.y}");
            return false;
        }

        // Ottieni il colore del pixel corrispondente
        Color pixelColor = walkabilityMap.GetPixelBilinear(uv.x, uv.y);
        
        Debug.Log($"Pixel Color: {pixelColor}, Walkable Color: {walkableColor}");
        
        // Confronta il colore con tolleranza per evitare problemi di precisione
        return IsColorClose(pixelColor, walkableColor, 0.1f); // 0.1 è la tolleranza
        
       
    }

    private bool IsColorClose(Color pixelColor, Color color, float f) {
        return Mathf.Abs(pixelColor.r - color.r) < f &&
            Mathf.Abs(pixelColor.g - color.g) < f &&
            Mathf.Abs(pixelColor.b - color.b) < f;
    }

    private Vector2 WorldToUV(Vector3 worldposition) {
        // Ottieni il centro della mappa
        Vector3 mapCenter = mapSpriteRenderer.transform.position;

        // Ottieni la scala della mappa
        Vector3 mapScale = mapSpriteRenderer.transform.localScale;

        // Calcola le dimensioni effettive della mappa nel mondo
        float scaledWidth = mapSize.x * mapScale.x;
        float scaledHeight = mapSize.y * mapScale.y;

        // Calcola le coordinate UV, tenendo conto della scala
        float u = (worldposition.x - mapCenter.x + scaledWidth / 2) / scaledWidth;
        float v = (worldposition.z - mapCenter.z + scaledHeight / 2) / scaledHeight;

        Debug.Log($"World Position: {worldposition}, UV: {u}, {v}, Scaled Width: {scaledWidth}, Scaled Height: {scaledHeight}");
        return new Vector2(u, v);
    }

}


    


