using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public bool inventoryOpen = false;
    private bool inventoryRequestable = true;
    private bool itemSelected = false;

    public AudioClip craft;
    public AudioClip fail;

    public GameObject itemPrefab;
    public int[] size = {10, 10};
    public float[] itemDimensions = {1, 1};
    public float[] inventorySize = {6, 6};
    public float[] buffers = {1, 1}; //buffers between items
    public TextAsset craftingListFile;
	public List<GameObject> list = new List<GameObject>(); //new list is created
	public GameObject firstItem;
	public GameObject secondItem;
	public GameObject waterBottle;
	public List<string> craftingList = new List<string>();
	public List<string> failedList = new List<string>();
	private string[] failedResponses = { "Das bringt mir hier nichts.", "Das hilft hier nicht.", "Das klappt nicht." };

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        this.gameObject.GetComponent<AudioSource>().volume = Camera.main.GetComponent<CameraScript>().volume;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        craftingListFile = (TextAsset)Resources.Load("Crafting/CraftingList");
        char[] delims = new[] { '\r', '\n' };
        string[] lines = craftingListFile.text.Split(delims, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++) {
            craftingList.Add(lines[i]);
        }

         craftingListFile = (TextAsset)Resources.Load("Crafting/FailedCraftingList");
         lines = craftingListFile.text.Split(delims, StringSplitOptions.RemoveEmptyEntries);
         for (int i = 0; i < lines.Length; i++) {
             failedList.Add(lines[i]);
         }

        GameObject newItem = Instantiate(waterBottle, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item
        newItem.name = "Leere Flasche";
        newItem.transform.parent = this.gameObject.transform;
        newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
        newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
        list.Add(newItem); //item is added to inventory list

    }

    // Update is called once per frame
    void Update() {

        itemSelected = false;
        spriteRenderer.enabled = inventoryOpen;
            for (int y = 0; y < size[0]; y++) { //y-axis "searched" through
                for (int x = 0; x <= size[1]; x++) { //for each line x-axis is searched through
                    if (y * size[0] + x >= list.Count) {
                        break;
                    }
                    GameObject item = list[y * size[0] + x];

                    float xPos = x * itemDimensions[0];
                    xPos -= inventorySize[0] / 2;
                    if (x != 0) {
                        xPos += buffers[0];
                    }

                    float yPos = y * itemDimensions[1];
                    yPos += inventorySize[1] / 2;
                    if (y != 0) {
                        yPos += buffers[1];
                    }
                    yPos = inventorySize[1] - yPos;

                    Vector2 position = new Vector2(xPos, yPos);
                    item.GetComponent<ItemScript>().initialInventoryPosition = position;

                    bool itemFollow = item.GetComponent<ItemScript>().follow;

                    if (!itemFollow) {
                        item.transform.position = position;
                    }
                    item.GetComponent<SpriteRenderer>().sortingLayerName = "InventoryItem";

                    if (itemFollow) {
                        item.GetComponent<SpriteRenderer>().sortingLayerName = "SelectedInventoryItem";
                    }

                    if (itemFollow) {
                        item.active = true;
                        itemSelected = true;
                    } else {
                        item.GetComponent<SpriteRenderer>().enabled = inventoryOpen;
                        item.GetComponent<BoxCollider2D>().enabled = inventoryOpen;
                    }
                }
            }

        if (!itemSelected) { //if no item is selected
            firstItem = null; //first item is empty
        }

        if (firstItem == null) { //if first item is empty
            secondItem = null; //second item is empty
        }

        if (firstItem != null && secondItem != null) { //if both first and second item are filled
            tryCrafting(firstItem, secondItem);
        }

        bool pressed = Input.GetKeyDown(KeyCode.I); //boolean pressed defined

        if (pressed && inventoryRequestable) { //if key i is pressed and inventory is requestable
            inventoryRequestable = false; //inventory is not requestable
            inventoryOpen = !inventoryOpen; //inventory open is set to opposite
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = inventoryOpen;
        }
        if (!pressed) { //if i key is not pressed
            inventoryRequestable = true; //inventory is requestable
        }
    }

    bool tryCrafting(GameObject item1, GameObject item2){
        for (int i = 0; i < craftingList.Count; i++){ //searches crafting list line by line
            string recipeString = craftingList[i]; //recipe string is set to current line of crafting list
            string[] recipe = recipeString.Split(','); //recipe string is split at every comma

            if ((firstItem.name == recipe[0] || firstItem.name == recipe[1]) && 
                (secondItem.name == recipe[0] || secondItem.name == recipe[1])
                 && firstItem.name != secondItem.name) { //if first item name and second item name are identical with the first and second split string and first item and second item don't have the same name

                this.gameObject.GetComponent<AudioSource>().clip = craft;
                this.gameObject.GetComponent<AudioSource>().Play();
                list.Remove(firstItem); //first item is removed from inventory list
                list.Remove(secondItem); //second item is removed from inventory list
                Destroy(firstItem); //first item is destroyed
                Destroy(secondItem); //second item ist destroyed
                firstItem = null; //first item is empty
                secondItem = null; //second itemm is empty
                GameObject newItem = Instantiate(itemPrefab, new Vector2(0, 0), Quaternion.identity); //new game object is created from prefab item

                newItem.name = recipe[2]; //name of new item is set to third split string
                newItem.transform.parent = this.gameObject.transform;
                newItem.GetComponent<ItemScript>().collected = true; //item is set to collected
                newItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Crafting/CraftedItemSprites/" + recipe[2]); //item sprite is set to correlating image
                newItem.GetComponent<Tooltip>().tooltip = recipe[3];
                newItem.GetComponent<SpriteRenderer>().size = new Vector2(1f, 1f);
                list.Add(newItem); //item is added to inventory list

                return true; //true is returned
            }
        }
        //hier bitte eine textresponse die random aus einem txt file gewählt wurde
        bool found = false;

        this.gameObject.GetComponent<AudioSource>().clip = fail;
        this.gameObject.GetComponent<AudioSource>().Play();
        for (int i = 0; i < failedList.Count; i++) {
            string failedString = this.failedList[i];
            string[] splitFailedString = failedString.Split(',');
            print(firstItem.name);
            print(splitFailedString[0]);
            print(secondItem.name);
            print(splitFailedString[1]);
            print(splitFailedString[2]);
            if ((firstItem.name == splitFailedString[0] || firstItem.name == splitFailedString[1])
                && (secondItem.name == splitFailedString[0] || secondItem.name == splitFailedString[1])
                && firstItem.name != secondItem.name) {
                found = true;
                GameObject.Find("Player").GetComponent<TextResponses>().display(splitFailedString[2]);
                break;
            }
        }
        if (!found) {
            string randomFailedResponse = this.failedResponses[UnityEngine.Random.Range(0, this.failedResponses.Length - 1)];
            GameObject.Find("Player").GetComponent<TextResponses>().display(randomFailedResponse);
        }
        firstItem = null; //first item is empty
        secondItem = null; //second item is empty
        return false; //false is returned
    }
}
