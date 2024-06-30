using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MoveTest
{
    [UnityTest]
    public IEnumerator MoveNorth()
    {
        var gameObject = new GameObject();
        var piper = gameObject.AddComponent<PiperScript>();

        piper.Move(1);
        yield return new WaitForSeconds(1);

        Assert.AreEqual(5*Vector3.up, gameObject.transform.position);
    }
}
