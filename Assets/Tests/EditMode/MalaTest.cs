using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MalaTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void MalaTestSimplePasses()
    {
        var gameObject = new GameObject();
        var malaScript = gameObject.AddComponent<MalaScript>();

        Assert.AreEqual(6, malaScript.timeOnMap);
    }
}
