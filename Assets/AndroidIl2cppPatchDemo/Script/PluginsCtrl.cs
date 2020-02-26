using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginsCtrl
{
    #if UNITY_ANDROID
	public static AndroidJavaObject getAccountCenterJobject()
	{
        try
        {
            AndroidJavaClass jclass = new AndroidJavaClass("com.jixin.PluginsCtrl");
            AndroidJavaObject jobject = jclass.GetStatic<AndroidJavaObject>("pluginsCtrl");
            return jobject;
        }
        catch
        {
            return null;
        }
	}

    public static void AndroidCall(string methodName, params object[] args)
    {
        AndroidJavaObject obj = getAccountCenterJobject();
        if (obj != null)
            obj.Call(methodName, args);
    }

    public static string AndroidCallString(string methodName, params object[] args)
    {
        AndroidJavaObject obj = getAccountCenterJobject();
        if (obj != null)
            return obj.Call<string>(methodName, args);

        return "";
    }
#endif

#if TEST0
public static void Test0(string dat){
#if UNITY_ANDROID
		if( Application.platform == RuntimePlatform.Android)
		{
            AndroidCall("TestJni0",dat);
		}
#endif
    }
#elif TEST1
public static void Test1(string dat){
#if UNITY_ANDROID
		if( Application.platform == RuntimePlatform.Android)
		{
            AndroidCall("TestJni1",dat);
		}
#endif
    }
#elif TEST2
public static void Test2(string dat){
#if UNITY_ANDROID
		if( Application.platform == RuntimePlatform.Android)
		{
            AndroidCall("TestJni2",dat);
		}
#endif
    }
#elif TEST3
public static void Test3(string dat){
#if UNITY_ANDROID
		if( Application.platform == RuntimePlatform.Android)
		{
            AndroidCall("TestJni3",dat);
		}
#endif
    }
#else
public static void Test(string dat){
#if UNITY_ANDROID
		if( Application.platform == RuntimePlatform.Android)
		{
            AndroidCall("TestJni",dat);
		}
#endif
    }
#endif
    
}
