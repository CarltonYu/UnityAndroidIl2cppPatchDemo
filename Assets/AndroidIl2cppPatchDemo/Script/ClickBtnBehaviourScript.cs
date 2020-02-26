using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBtnBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(string index){
        #if TEST0
        PluginsCtrl.Test0(index);
        #elif TEST1
        PluginsCtrl.Test1(index);
        #elif TEST2
        PluginsCtrl.Test2(index);
        #elif TEST3
        PluginsCtrl.Test3(index);
        #else
        PluginsCtrl.Test(index);
        #endif
    }
}
