using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Purpose of this enum is so that we know what type of object certain scripts belong to
public enum ObjectIdentity
{
    player,
    tree,
    rock,
    enemy1,
    enemy2,
    enemy3,
    enemy4,
    // Each enemy should have its own identity. Make sure to add these at the END of the enum to not mess up the serialization. If its out of order thats fine.
}
