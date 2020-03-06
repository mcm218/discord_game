using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToSpawn
{
    public GameObject item;
    public int numClumps = 1;
    public int minClumpSize = 1;
    public int maxClumpSize = 5;

    public int maxDistFromSeed = 1;

    public int spawnChance = 10;

    public bool distanceWeight = false;
}