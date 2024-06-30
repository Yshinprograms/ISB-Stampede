using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FriendTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void FriendTestSimplePasses()
    {
        var gameObject = new GameObject();
        var friendScript = gameObject.AddComponent<FriendScript>();

        Assert.AreEqual(PiperScript.piperMoveSpeed, friendScript.moveSpeed);
    }

}
