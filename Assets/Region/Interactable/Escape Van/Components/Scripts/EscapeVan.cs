using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeVan : MonoBehaviour {
    private Camera camera;
    private Vector3 mousePos;
    private Vector2 mousePosWorld;
    
    
    public bool stick = false;
    public bool proviant = false;
    public bool zucchini = false;
    public bool water = false;
    public bool cracker = false;
    public bool batterie = false;
    public bool finished = false;
    public bool wolf = false;
    public bool doorCheck = false;
    public bool batteryCheck = false;
    public bool check = false;

    
    public GameObject inventory;
    public GameObject player;
    public GameObject end;




    void Start() {
        this.camera = Camera.main;
        
    }

    void Update() {
        mousePos = Input.mousePosition; //Stores the mouse position
        Vector3 mouseWorldPos3D = camera.ScreenToWorldPoint(mousePos);
        mousePosWorld = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosWorld, Vector2.zero, Mathf.Infinity, 1 << this.gameObject.layer);

        List<GameObject> list = inventory.GetComponent<Inventory>().list;
        for(int i = 0; i < list.Count; i++) {
            
            if (list[i].name == "Cracker und Dip") {
                cracker = true;
            }
            
        }
        


        proviant = zucchini && water && cracker;
        finished = stick && proviant && batterie;
        check = batteryCheck && doorCheck;

        if (!stick && !proviant && !batterie && !check) { //nüscht
            this.gameObject.GetComponent<Tooltip>().tooltip = "Er sieht gut aus, aber ich muss erst einmal checken ob ihm etwas fehlt. ";
        }

        else if (!stick && !proviant && !batterie && check) { //nüscht
            this.gameObject.GetComponent<Tooltip>().tooltip = "Ich kann mit ihm wegfliegen! Aber es fehlen ihm noch eine Batterie und das Navigationssystem. Außerdem brauche ich Proviant. ";
        }

        else if (!stick && proviant && !batterie) { //nur proviant
            this.gameObject.GetComponent<Tooltip>().tooltip = "Es fehlen noch das Navigationssystem und die Batterie";
        }

        else if (!stick && !proviant && batterie) { //nur batterie
            this.gameObject.GetComponent<Tooltip>().tooltip = "Es fehlen noch Navigationssystem und Proviant.";
        }

        else if (stick && !proviant && !batterie) { //nur stick
            this.gameObject.GetComponent<Tooltip>().tooltip = "Es fehlen noch Proviant und Batterie.";
        }

        else if (stick && proviant && !batterie) { //stick und proviant
            this.gameObject.GetComponent<Tooltip>().tooltip = "Es fehlt noch die Batterie.";
        }

        else if (stick && !proviant && batterie) { //stick und batterie
            this.gameObject.GetComponent<Tooltip>().tooltip = "Mir fehlt noch Proviant.";
        }

        else if (!stick && proviant && batterie) { //proviant und batterie
            this.gameObject.GetComponent<Tooltip>().tooltip = "Es fehlt noch das Navigationssystem.";
        }

        else if (finished && !wolf) { //alles
            player.GetComponent<TextResponses>().display("So, das müsste es sein! Es kann losgehen.");

            this.gameObject.GetComponent<Tooltip>().tooltip = "Er ist startklar und ich habe Proviant! Jetzt muss ich nur noch einsteigen.";
            wolf = true;
        }
    }
}

       