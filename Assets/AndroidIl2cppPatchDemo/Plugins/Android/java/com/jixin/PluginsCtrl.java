package com.jixin;

import android.util.Log;

import com.unity3d.player.UnityPlayer;

public class PluginsCtrl {
    public static String TAG = "PluginsCtrl";
    public static PluginsCtrl pluginsCtrl;

    public PluginsCtrl(){
        pluginsCtrl = this;
    }

    public void TestJni(String dat){
        Log.e(TAG,"TestJni:"+dat);
        UnityPlayer.UnitySendMessage("XCodeMsgManager","JavaCallback",dat);
    }

    public void TestJni0(String dat){
        Log.e(TAG,"TestJni0:"+dat);
        UnityPlayer.UnitySendMessage("XCodeMsgManager","JavaCallback0",dat);
    }

    public void TestJni1(String dat){
        Log.e(TAG,"TestJni1:"+dat);
        UnityPlayer.UnitySendMessage("XCodeMsgManager","JavaCallback1",dat);
    }

    public void TestJni2(String dat){
        Log.e(TAG,"TestJni2:"+dat);
        UnityPlayer.UnitySendMessage("XCodeMsgManager","JavaCallback2",dat);
    }

    public void TestJni3(String dat){
        Log.e(TAG,"TestJni3:"+dat);
        UnityPlayer.UnitySendMessage("XCodeMsgManager","JavaCallback3",dat);
    }
}
