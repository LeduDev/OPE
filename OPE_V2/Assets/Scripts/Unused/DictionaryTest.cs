using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TestDictionary()
    {
        Dictionary<string, int> testDictionary = new Dictionary<string, int>();
        testDictionary.Add("Age", 24);
        testDictionary.Add("Power", 200);
        testDictionary.Add("Mind", 180);

        Debug.Log("My age is " + testDictionary["Age"]);
        Debug.Log("My Power is " + testDictionary["Power"]);
        Debug.Log("My Mind is " + testDictionary["Mind"]);

    }
}
