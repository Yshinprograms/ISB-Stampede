using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CSMuggerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CSMuggerTestSimplePasses()
    {
        var gameObject = new GameObject();
        var script = gameObject.AddComponent<CSMugger>();

        Assert.AreEqual(10, script.startTimeBtwCode);
        Assert.AreEqual(3, script.maxCodeProduce);
    }
}