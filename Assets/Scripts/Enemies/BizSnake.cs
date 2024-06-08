using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* >>> InnocentStudent GameObject (Impervious to Piper projectiles)
* Innocently goes up to Piper with "Do you want to group up?"
* When contacted, latches on to Piper and slows her down, seen as 'friendly' in this state
* After 3s, turns into 'BizSnake'
* (Piper gains a new ability to push or shoo away innocent students by pressing 'e' when within range. 
*  Doesn't damage them, but pushes them away and they stop moving for 3s. 
*  Can only be destroyed by pushing them into other enemies) --> Might need to include tutorial in level 3 cutscene or upon level start
*  
* Parameters: *HP, 0DMG, 1.5 Speed, Spawn every 8s, No projectile
* 
* >>> BizSnake GameObject
* 2s delay for Piper to run away after transforming into BizSnake
* Position is fixed, launches toxic words at Piper no matter where she is
* Shouts "Don't burden leh" with every projectile
* 
* Parameters: 20HP, 10DMG, 0 Speed, No Spawn, Shoot every 3s
*/

public class BizSnake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
