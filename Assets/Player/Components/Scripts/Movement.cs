using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour {
    private Vector3 mousePos; //For storing the mouse position after a click
    private Vector2 mousePosWorld; //Mouse position in the world
    private Vector2 targetPos; //Target moving position
    private float initialPlayerPosition;
    private bool walking = false;
    private Pathfinding pathfinding;
    private float scalingFactor = 0;

    public Vector2 initialStartingScale;
    public Vector2 startingScale;
    public Vector2 playerScale;
    public long walkingId = 0;
    public List<Vector2> walkingPath = new List<Vector2>();
    public string playerSpritesPath = "Assets/Player/";
    public Sprite idleSprite, walkingSprite;
    public bool canWalk = true;
    private Camera camera; 
    private GameObject player;
    private float cameraFactor;
    public float cameraAngle = 45;
    public float speed;
    public float startingSpeed;
    public GameObject startingScene;
    public AudioClip walkSound;
    public bool soundStart = false;
    public float initialDistanceToCamera;
    public float initialDistanceToText;
    public float distanceToText;

    

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        this.camera = Camera.main;
        this.startingSpeed = speed;
        this.player = this.gameObject;
        this.initialPlayerPosition = player.transform.position.y;
        this.player.GetComponent<SpriteRenderer>().sprite = idleSprite;
        float hoxton = startingScene.transform.Find("Background").GetComponent<Background>().playerSize;
        this.initialStartingScale = new Vector2(player.GetComponent<SpriteRenderer>().size.x, player.GetComponent<SpriteRenderer>().size.y);
        this.startingScale = initialStartingScale * new Vector2(hoxton, hoxton);
        this.pathfinding = this.gameObject.GetComponent<Pathfinding>();
        this.initialDistanceToCamera = startingScene.transform.Find("Background").GetComponent<Background>().cameraDistance;
        this.initialDistanceToText = this.gameObject.transform.Find("Canvas").localPosition.y;
        this.distanceToText = initialDistanceToText;
    }

    void Update() {
        cameraFactor = cameraAngle / 90;
        if (Input.GetMouseButtonDown(0)) {
            mousePos = Input.mousePosition; //Stores the mouse position
            Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
            mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero);
            if (hit.collider == null && !GameObject.Find("Inventory").GetComponent<Inventory>().inventoryOpen && canWalk) {
                 walkingPath = pathfinding.findPath(100, 100, player.transform.position, new Vector2(mousePosWorld.x, mousePosWorld.y));
                 walkingId++;
            }
        }
    }

    void FixedUpdate() {
        movePlayer();
    }

    void checkSpriteFlip(Vector2 nextPosition) {
        player.GetComponent<SpriteRenderer>().sprite = this.walkingSprite;
        if (player.transform.position.x > nextPosition.x) {
            //Nach links blicken
            player.GetComponent<SpriteRenderer>().flipX = true;
        } else if (player.transform.position.x < nextPosition.x) {
            //Nach rechts blicken
            player.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void movePlayer() {
        if (walkingPath != null) {
            if (walkingPath.Count > 0) {
                if (soundStart == false) { 
                this.gameObject.GetComponent<AudioSource>().clip = walkSound;
                this.gameObject.GetComponent<AudioSource>().Play();
                    soundStart = true;
                }
                player.GetComponent<Walkanimation>().walking = true;
                player.transform.position = Vector3.MoveTowards(player.transform.position, walkingPath[0], speed);
                checkSpriteFlip(walkingPath[0]);
                if (player.transform.position.x == walkingPath[0].x && player.transform.position.y == walkingPath[0].y) {
                    walkingPath.Remove(walkingPath[0]);
                }
            } else {
                this.gameObject.GetComponent<AudioSource>().Stop();
                soundStart = false;
                player.GetComponent<Walkanimation>().walking = false;
                player.GetComponent<SpriteRenderer>().sprite = this.idleSprite;
            }
        }
        scalePlayer();
    }

    public void scalePlayer() {
        float additionalDistance;
        additionalDistance = initialPlayerPosition - player.transform.position.y;
        this.scalingFactor = (float) (Math.Pow(1 + additionalDistance / initialDistanceToCamera, 2));
        this.playerScale = new Vector2(startingScale.x * this.scalingFactor, startingScale.y * this.scalingFactor);
        player.GetComponent<SpriteRenderer>().size = this.playerScale;
        this.gameObject.transform.Find("Canvas").localPosition = new Vector3(0f, this.distanceToText * scalingFactor, 0f);
        this.speed = this.startingSpeed * this.scalingFactor;
    }
}