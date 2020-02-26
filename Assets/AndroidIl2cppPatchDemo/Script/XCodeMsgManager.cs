using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XCodeMsgManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #if TEST0
    public void JavaCallback0(string dat){
        Debug.Log("JavaCallback0 dat:"+dat);
    }
    #elif TEST1
    public void JavaCallback1(string dat){
        Debug.Log("JavaCallback1 dat:"+dat);
    }
    #elif TEST2
    public void JavaCallback2(string dat){
        Debug.Log("JavaCallback2 dat:"+dat);
    }
    #elif TEST3
    public void JavaCallback3(string dat){
        Debug.Log("JavaCallback3 dat:"+dat);
    }
    #else
    public void JavaCallback(string dat){
        Debug.Log("JavaCallback dat:"+dat);
    }
    #endif
}
