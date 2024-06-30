using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FreshieTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void FreshieTestSimplePasses()
    {
        var gameObject = new GameObject();
        var script = gameObject.AddComponent<Freshie>();

        Assert.AreEqual(1, script.freshieDiagonalSpeed);
        Assert.AreEqual(1, script.moveDuration);
        Assert.AreEqual(0, script.stopDuration);
    }
}
