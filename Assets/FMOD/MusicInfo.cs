using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Area { 
    Forest, 
    Monastery,
    Highlands,
    None
    // adding shouldn't be an issue. None should always be last
}

[CreateAssetMenu(fileName = "MusicInfo", menuName = "MusicInfo", order = 0)]
public class MusicInfo : ScriptableObject
{
    public Area area;

    public int numVariants;
    public int numTracks;

    //public bool[][] trackOnGrid = new bool[numTracks][] ;


    public string trackOnOffs;



}
