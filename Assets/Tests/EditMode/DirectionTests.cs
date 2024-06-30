using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DirectionTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void South()
    {
        int dir = 0;
        if (Input.GetKey(KeyCode.S))
        {
            dir = 5;
        }
        Assert.AreEqual(5, dir);
    }

}
