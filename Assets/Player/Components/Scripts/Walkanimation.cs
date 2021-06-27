using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkanimation : MonoBehaviour {

    private GameObject player;
    public bool walking = false;
    public Sprite w0, w1, w2, w3, w4, w5, w6, w7, w8, w9, w10, w11, w12, w13, w14, w15;
    public float speed;
    private Sprite[] walkSprites = new Sprite[16];
    private float time = 0f;
    private float segment;

    // Start is called before the first frame update
    void Start() {
        player = this.gameObject;
        walkSprites[0] = w0;
        walkSprites[1] = w1;
        walkSprites[2] = w2;
        walkSprites[3] = w3;
        walkSprites[4] = w4;
        walkSprites[5] = w5;
        walkSprites[6] = w6;
        walkSprites[7] = w7;
        walkSprites[8] = w8;
        walkSprites[9] = w9;
        walkSprites[10] = w10;
        walkSprites[11] = w11;
        walkSprites[12] = w12;
        walkSprites[13] = w13;
        walkSprites[14] = w14;
        walkSprites[15] = w15;
        segment = 1f / walkSprites.Length;
    }

    // Update is called once per frame
    void Update() {
        if (walking) {
            time += Time.deltaTime * speed;
            player.GetComponent<SpriteRenderer>().sprite = walkSprites[Mathf.Min((int)(time / segment), walkSprites.Length - 1)];
            player.GetComponent<SpriteRenderer>().size = player.GetComponent<Movement>().playerScale;
            if (time >= 1f) {
                time -= 1f;
            }
        } else {
            time = 0;
        }
    }
}
