using UnityEngine;
public class Tile
{
    public bool isFull { get; private set; }
    public Vector2 position { get; private set; }
    public GameObject self { get; set; }
    private GameObject _child;
    public GameObject child { get { return _child; } set { _child = value; isFull = value != null; } }

    public Tile(Vector2 pos, GameObject _self)
    {
        isFull = false;
        position = pos;
        self = _self;
    }
    public Tile(Vector2 pos, GameObject selfObject, GameObject childObject)
    {
        isFull = true;
        position = pos;
        self = selfObject;
        child = childObject;
    }
}