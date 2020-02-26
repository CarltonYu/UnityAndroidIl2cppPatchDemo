package com.jixin;

import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;

import io.github.noodle1983.Boostrap;

public class StartActivity extends Activity {
    public static String TAG = "StartActivity";
    public Activity activity;

    public static final int Handler_CheckVersion = 1000;
    public static final int Handler_DownloadFinish = 1001;
    public static final int Handler_UnzipFinish = 1002;
    Handler mainHandler = new Handler(new Handler.Callback() {

        @Override
        public boolean handleMessage(Message msg) {
            switch (msg.what) {
                case Handler_CheckVersion:
                    break;
                case Handler_DownloadFinish:
                    break;
                case Handler_UnzipFinish:
                    UnzipFinish((String) msg.obj);
                    break;
                default:
                    break;
            }
            return false;
        }
    });

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        activity = this;
        PluginsCtrl.pluginsCtrl = new PluginsCtrl();
        PermissionUtil.currentActivity = this;
        PermissionUtil.initListener(new PermissionUtilListener() {
            @Override
            public void onRequestPermissionsResult(int requestCode, String permissions, int grantResults, boolean notRemind) {
                if(requestCode == 10086){
                    if (grantResults == PackageManager.PERMISSION_GRANTED) {
                        Log.w(TAG,"PERMISSION_GRANTED:"+ Manifest.permission.WRITE_EXTERNAL_STORAGE);
                        startGame();
                    }else{
                        Log.w(TAG,grantResults+":"+Manifest.permission.WRITE_EXTERNAL_STORAGE);
                        new AlertDialog.Builder(activity)
                                .setCancelable(false)
                                .setTitle("缺少运行权限")
                                .setMessage("游戏正常运行中读取/更新素材，必须取得文件写入权限")
                                .setPositiveButton("再获取", new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialogInterface, int i) {
                                        CheckPermission();
                                    }
                                })
                                .setNegativeButton("退出游戏",new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialogInterface, int i) {
                                        activity.finish();
                                    }
                                })
                                .create()
                                .show();
                    }
                }
            }
        });
        CheckPermission();
    }

    public void CheckPermission(){
        Log.e(TAG,"======== CheckPermission");
        if(PermissionUtil.hasPermission(Manifest.permission.WRITE_EXTERNAL_STORAGE) == false){
            Log.e(TAG,"======== CheckPermission false");
            PermissionUtil.RequestPermissions(Manifest.permission.WRITE_EXTERNAL_STORAGE,10086);
        }else{
            startGame();
        }
    }

    public void startGame(){
        String filepath = getApplication().getApplicationContext().getFilesDir().getPath();
        Log.e(TAG,"======= InitNativeLibBeforeUnityPlay filepath:"+filepath);
        Boostrap.InitNativeLibBeforeUnityPlay(filepath);
        String cpu = Boostrap.getarchabi();
        Log.e(TAG,"======= Boostrap.get_arch_abi:"+cpu);
//        initUnity();
        String local_dir = this.getExternalFilesDir("download").getAbsolutePath();
//        Log.e(TAG,"======= local_dir:"+local_dir);
        String local_unzip_root_dir = this.getExternalFilesDir("").getAbsolutePath();
//        Log.e(TAG,"======= local_unzip_root_dir:"+local_unzip_root_dir);
        CheckVersion.CheckVersion(cpu,mainHandler,local_dir,local_unzip_root_dir);
    }

    public void UnzipFinish(String version){

        String filepath = getApplication().getApplicationContext().getFilesDir().getPath();
        Log.e(TAG,"======= ReInitNativeLibBeforeUnityPlay filepath:"+filepath);
        Boostrap.ReInitNativeLibBeforeUnityPlay();
        initUnity();
//        String local_dir = this.getExternalFilesDir("download").getAbsolutePath();
//        Log.e(TAG,"======= local_dir:"+local_dir);
//        String local_unzip_root_dir = this.getExternalFilesDir("").getAbsolutePath();
//        Log.e(TAG,"======= local_unzip_root_dir:"+local_unzip_root_dir);
//        String local_path = String.format(CheckVersion.local_path_format,local_dir,version);
//        try {
//            CheckVersion.UnZipFile(mainHandler,local_path,local_unzip_root_dir,version);
//        } catch (Exception e) {
//            e.printStackTrace();
//        }

    }

    public void initUnity(){
        Intent intent = new Intent();
        intent.setClass(this, UnityPlayerActivity.class);
        startActivity(intent);

    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}
