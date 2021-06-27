using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node { //saves data, can transfer from node to world space
    private Vector2 nodePosition; 
    private Vector2 absoluteCenter;
    private Node parent; //parent node
    private List<Node> successors; //list of nodes following parent node
    private bool closed = false; //whether node is not considered
    private bool open = false; //whether node is considered
    private bool walkable = true; //whether node is inside a collider
    private double g = 0.0; //distance parent node to node
    private double f = 0.0; //g+h

    public void setNodePosition(int x, int y) { //method for initialising the node position
        this.nodePosition = new Vector2(x, y);
    }
    public Vector2 getNodePosition() { //method returning the node's position
        return this.nodePosition;
    }

    public Vector2 getAbsoluteCenter(int[] nodeGridSize) { //method returning the absolute center of the node in the screenspace

        Vector2 size = new Vector2(Screen.width / nodeGridSize[0], Screen.height / nodeGridSize[1]); //size of node

        float x = (float)Screen.width / (float)nodeGridSize[0] * (float)nodePosition.x + (float)size.x / 2f; //centre point of node in screen on x axis is found
        float y = (float)Screen.height / (float)nodeGridSize[1] * (float)nodePosition.y + (float)size.y / 2f; //centre point of node in screen on x axis is found
        return new Vector2(x, y); //centre point of node in screen is returned
    }

    public void setParent(Node parent) { //method to set the parent node 
        this.parent = parent;
    }
    public Node getParent() { //method to return the parent node
        return this.parent;
    }

    public void setSuccessors(List<Node> successors) { //method to set the list of child nodes
        this.successors = successors;
    }
    List<Node> getSuccessors() { //method to return the successor list
        return this.successors;
    }

    public void setClosed(bool closed) { //method setting the node's closed
        this.closed = closed;
    }
    public bool isClosed() { //method returning the node's closed
        return this.closed;
    }

    public void setOpen(bool open) { //method setting the node's open
        this.open = open;
    }
    public bool isOpen() { //method returning the node's open
        return this.open;
    }

    public void setWalkable(bool walkable) {
        this.walkable = walkable;
    }
    public bool setWalkable() {
        return this.walkable;
    }

    public void setG(double g) {
        this.g = g;
    }
    public double getG() {
        return this.g;
    }

    public void setF(double f) {
        this.f = f;
    }
    public double getF() {
        return this.f;
    }
}




public class Pathfinding : MonoBehaviour { //class pathfinding

    private int[] gridSize = new int[2];
    private Vector2 absoluteTargetPosition;
    private Node startingNode;
    private Node goalNode;
    private List<Node> open = new List<Node>(); //"open" list for storing nodes 
    private List<Node> closed = new List<Node>(); //"closed" list for storing nodes

    public List<Vector2> findPath(int sizeX, int sizeY, Vector2 startingPosition, Vector2
    targetPosition) { //"findPath" method returning list with vector data forming a path

        this.absoluteTargetPosition = targetPosition; 

        open.Clear(); //list is emptied
        closed.Clear(); //list is emptied

        this.gridSize[0] = sizeX;
        this.gridSize[1] = sizeY;

        this.startingNode = new Node();
        Vector2 startingNodePosition = positionToNodePosition(startingPosition);
        this.startingNode.setNodePosition((int)startingNodePosition.x, (int)startingNodePosition.y); 
        open.Add(this.startingNode); //starting node is added to "open" list

        this.goalNode = new Node();
        Vector2 goalNodePosition = positionToNodePosition(targetPosition);
        this.goalNode.setNodePosition((int)goalNodePosition.x, (int)goalNodePosition.y);


        int iterations = 0; //iterations initialised to 0, failsafe for checking if iterations are over a 1000 (-> process is taking too long)

        while (open.Count > 0 && iterations < 1000) {
            Node currentNode = getNodeWithLeastF(this.open); //node with least f value of the "open" list is returned
            open.Remove(currentNode); //"open" removes current node
            closed.Add(currentNode); //current node is added to "closed" list

            if (currentNode.getNodePosition() == goalNode.getNodePosition()) { //if goal node is reached, the full path is returned
                goalNode.setParent(currentNode);
                return generatePath();
            } else { //else the children of the current node are generated
                generateChildren(currentNode); 
            }
            iterations++;
        }
        return null;
    }



    private Vector2 positionToNodePosition(Vector2 position) { //method returning node position

        position = Camera.main.WorldToScreenPoint(position); //position is transferred from world to screen

        double xSize = (double) Screen.width / (double) this.gridSize[0]; //node size is set
        print("XSize: " + xSize);
        double ySize = (double) Screen.height / (double) this.gridSize[1];
        print("YSize: " + ySize);

        for (int i = 0; i < this.gridSize[0]; i++) { //x axis index in grid searched
            if (position.x > i * xSize && position.x <= (i + 1) * xSize) {
                print("X: " + i);
                for (int j = 0; j < this.gridSize[1]; j++) { //y axis index in grid searched
                    if (position.y > j * ySize && position.y <= (j + 1) * ySize) {
                        print("Y: " + j);
                        return new Vector2(i, j); //node is found
                    }
                }
            }
        }
        return new Vector2(0, 0);
    }



    private Node getNodeWithLeastF(List<Node> list) { //method to get a node from a list with the smallest f value
        double smallestF = 10000;
        int leastFIndex = 0;

        for (int i = 0; i < list.Count; i++) { //same stuff as before
            if (list[i].getF() < smallestF) {
                smallestF = list[i].getF();
                leastFIndex = i;
            }
        }

        return list[leastFIndex]; //node with least f index is returned
    }



    private int isInList(List<Node> nodeList, Node node) { //method returning integer of index in list
        for (int i = 0; i < nodeList.Count; i++) {
            if (nodeList[i].getNodePosition() == node.getNodePosition()) { //if found, returns the index
                return i;
            }
        }
        return -1; //if not found returns -1
    }



    private List<Vector2> generatePath() { //method returning the generated path
        List<Vector2> rawPath = new List<Vector2>();
        List<Vector2> path = new List<Vector2>();

        Node lastNode = this.goalNode;

        while (lastNode.getParent() != null) { //if the final node's parent is not empty, all nodes are put in a list to form rawPath
            rawPath.Add(Camera.main.ScreenToWorldPoint(lastNode.getAbsoluteCenter(this.gridSize)));
            lastNode = lastNode.getParent();
        }

        for (int i = rawPath.Count - 1; i > 1; i--) { //path is being umgest�lpt
            path.Add(rawPath[i]);
        }
        path.Add(this.absoluteTargetPosition); //final node is added

        return path; //path is returned
    }



    public Vector2 findClosestNode (int sizeX, int sizeY, Vector2 position) { //method to find the closest node to an item

        this.gridSize[0] = sizeX;// identical with the child part, but a little bit different
        this.gridSize[1] = sizeY;

        Vector2 nodePosition = positionToNodePosition(position);

        int startingX = (int) Mathf.Max(nodePosition.x - 1, 0);
        int endingX = (int) Mathf.Min(nodePosition.x + 1, this.gridSize[0]);
        int startingY = (int) Mathf.Max(nodePosition.y - 1, 0);
        int endingY = (int) Mathf.Min(nodePosition.y + 1, this.gridSize[1]);

        while (true) {
            for (int i = startingX; i < endingX; i++) {
                for (int j = startingY; j < endingY; j++) {
                    Node node = new Node();
                    node.setNodePosition(i, j);
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(node.getAbsoluteCenter(this.gridSize)), Vector2.zero);
                    if (hit.collider == null) {
                        return Camera.main.ScreenToWorldPoint(node.getAbsoluteCenter(this.gridSize));
                    }
                }
            }

            startingX = (int) Mathf.Max(startingX - 1, 0);
            endingX = (int) Mathf.Min(endingX + 1, this.gridSize[0]);
            startingY = (int) Mathf.Max(startingY - 1, 0);
            endingY = (int) Mathf.Min(endingY + 1, this.gridSize[1]);
        }
    }



    private void generateChildren(Node node) {

        Vector2 position = node.getNodePosition(); 

        //This exact part needs to be redone   //comment on the code stating the following code has to be redone
        int startingX = (int) Mathf.Max(position.x - 1, 0); //outermost points of the grid are calculated
        int endingX = (int) Mathf.Min(position.x + 1, this.gridSize[0]);
        int startingY = (int) Mathf.Max(position.y - 1, 0);
        int endingY = (int) Mathf.Min(position.y + 1, this.gridSize[1]);

        for (int i = startingX; i < endingX + 1; i++) { //nodes are generated from grid
            for (int j = startingY; j < endingY + 1; j++) {
                if (i == position.x && j == position.y) {
                    continue;
                }
                Node child = new Node();
                child.setNodePosition(i, j);
                if (isInList(closed, child) != -1) { //if node is in "closed" list it is skipped
                    continue;
                }

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(child.getAbsoluteCenter(this.gridSize)), Vector2.zero);
                if (hit.collider != null) { //if the centre of the node is within a collider
                    if (this.goalNode.getNodePosition() != child.getNodePosition()) { //if the node is not the goal node
                        closed.Add(child); //node is put on "closed" list
                        continue; //node is skipped
                    }
                }

                double g = 0;
                if (i == position.x || j == position.y) {
                    g += 1;
                } else {
                    g += Mathf.Sqrt(2);
                }
                double h = Vector2.Distance(child.getNodePosition(), this.goalNode.getNodePosition());
                child.setG(g);
                child.setF(g + h);
                int listIndex = isInList(open, child);
                if (listIndex != -1) {
                    if (child.getG() > open[listIndex].getG()) { //check whether a node with the same position as the child is in the open list, which has a higher g value than the child node
                        continue;
                    }
                }

                child.setParent(node); //node wird als parent von child gesetzt
                this.open.Add(child); //child wird der "open" list hinzugefügt
            }
        }
    }
}