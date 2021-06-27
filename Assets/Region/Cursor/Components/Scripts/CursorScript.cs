using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour {

    private Camera camera;
    // Start is called before the first frame update
    void Start() {
        camera = Camera.main;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        Vector2 mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
        this.gameObject.transform.position = mousePosWorld;
    }
}