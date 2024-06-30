using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PaperBallTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PaperBallTestSimplePasses()
    {
        var gameObject = new GameObject();
        var paperBallScript = gameObject.AddComponent<PaperBallScript>();

        Assert.AreEqual(4, paperBallScript.paperBallSpeed);
    }

}
