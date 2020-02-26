package com.jixin;
import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Looper;
import android.provider.Settings;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;

/**
 * Created by carlton on 2018/1/9.
 */

public class PermissionUtil {
    private static String titleStr = "标题";
    private static String openCameraStr = "该功能需要开启摄像机权限";
    private static String openStorageStr = "该功能需要开启储存权限";
    private static String openAccountStr = "该功能需要开启通讯录权限";
    private static String openAudioStr = "该功能需要开启麦克风权限";
    private static String gotoSettingStr = "请去设置界面开启权限";

    private static int PERMISSIONS_STATE_OK = 1;
    private static int PERMISSIONS_STATE_CANCEL = 2;
    private static int PERMISSIONS_STATE_DONTASK = 3;

    final private static int REQUEST_CODE_ASK_PERMISSIONS_CAMERA = 123;
    final private static int REQUEST_CODE_ASK_PERMISSIONS_STORAGE = 234;
    final private static int REQUEST_CODE_ASK_PERMISSIONS_ACCOUNT = 345;
    final private static int REQUEST_CODE_ASK_PERMISSIONS_AUDIO = 456;

    public static PermissionUtilListener permissionUtilListener = null;
    public static Activity currentActivity;
    public static void initListener(PermissionUtilListener listener) {
        permissionUtilListener = listener;
    }

    public static int GetPermissionState(String permissionName) {
        int hasWriteContactsPermission = ContextCompat.checkSelfPermission(currentActivity, permissionName);
        if (hasWriteContactsPermission != PackageManager.PERMISSION_GRANTED) {
            if (!ActivityCompat.shouldShowRequestPermissionRationale(currentActivity, permissionName)) {
                return PERMISSIONS_STATE_DONTASK;
            }
            return PERMISSIONS_STATE_CANCEL;
        }
        return PERMISSIONS_STATE_OK;
    }

    // 请求开启权限
    public static void RequestPermissions(String permissionName, int requestCode) {
        ActivityCompat.requestPermissions(currentActivity,
                new String[]{permissionName},
                requestCode);
    }

    // 是否开启权限
    public static boolean hasPermission(String permissionStr) {
        if (Build.VERSION.SDK_INT < 23) {
            return true;
        }
        Context context = currentActivity.getApplicationContext();
        return context.checkCallingOrSelfPermission(permissionStr) == PackageManager.PERMISSION_GRANTED;
    }

    // 用户操作回调
    public static void onRequestPermissionsResult(int requestCode, String permissions, int grantResults, boolean notRemind) {
        if(permissionUtilListener!=null)
            permissionUtilListener.onRequestPermissionsResult(requestCode, permissions, grantResults, notRemind);
        switch (requestCode) {
            case REQUEST_CODE_ASK_PERMISSIONS_CAMERA:
                if (grantResults == PackageManager.PERMISSION_GRANTED) {
                    // Permission Granted
                } else {
                    // Permission Denied
                    if (notRemind) {
                        showMessage(titleStr, gotoSettingStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                openSetting();
                            }
                        });
                    } else {
                        showMessage(titleStr, openCameraStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                RequestPermissions(Manifest.permission.CAMERA, REQUEST_CODE_ASK_PERMISSIONS_CAMERA);
                            }
                        });
                    }
                }
                break;
            case REQUEST_CODE_ASK_PERMISSIONS_STORAGE:
                if (grantResults == PackageManager.PERMISSION_GRANTED) {
                    // Permission Granted

                } else {
                    // Permission Denied
                    // 检查是否勾选‘不再提醒’
                    if (notRemind) {
                        showMessage(titleStr, gotoSettingStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                openSetting();
                            }
                        });
                    } else {
                        showMessage(titleStr, openStorageStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                RequestPermissions(Manifest.permission.READ_EXTERNAL_STORAGE, REQUEST_CODE_ASK_PERMISSIONS_STORAGE);
                            }
                        });
                    }
                }
                break;
            case REQUEST_CODE_ASK_PERMISSIONS_ACCOUNT:
                if (grantResults == PackageManager.PERMISSION_GRANTED) {
                    // Permission Granted

                } else {
                    // Permission Denied
                    // 检查是否勾选‘不再提醒’
                    if (notRemind) {
                        showMessage(titleStr, gotoSettingStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                openSetting();
                            }
                        });
                    } else {
                        showMessage(titleStr, openAccountStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                RequestPermissions(Manifest.permission.GET_ACCOUNTS, REQUEST_CODE_ASK_PERMISSIONS_ACCOUNT);
                            }
                        });
                    }
                }
                break;
            case REQUEST_CODE_ASK_PERMISSIONS_AUDIO:
                if (grantResults == PackageManager.PERMISSION_GRANTED) {
                    // Permission Granted

                } else {
                    // Permission Denied
                    // 检查是否勾选‘不再提醒’
                    if (notRemind) {
                        showMessage(titleStr, gotoSettingStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                openSetting();
                            }
                        });
                    } else {
                        showMessage(titleStr, openAudioStr, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                RequestPermissions(Manifest.permission.RECORD_AUDIO, REQUEST_CODE_ASK_PERMISSIONS_AUDIO);
                            }
                        });
                    }
                }
                break;
            default:
                break;
        }
    }

    public static void showMessage(final String title, final String message, final DialogInterface.OnClickListener okListener) {
        new android.os.Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                new AlertDialog.Builder(currentActivity)
                        .setCancelable(false)
                        .setTitle(title)
                        .setMessage(message)
                        .setPositiveButton("OK", okListener)
                        .create()
                        .show();
            }
        });
    }

    public static void showMessageOK(final String title, final String message) {
        new android.os.Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                new AlertDialog.Builder(currentActivity)
                        .setCancelable(false)
                        .setTitle(title)
                        .setMessage(message)
                        .setPositiveButton("OK", null)
                        .create()
                        .show();
            }
        });
    }

    // 打开系统设置
    public static void openSetting() {
        new android.os.Handler(Looper.getMainLooper()).post(new Runnable() {
            @Override
            public void run() {
                Intent intent = new Intent(Settings.ACTION_SETTINGS);
                currentActivity.startActivity(intent);
            }
        });
    }

    // 请求摄像机权限
    public static boolean RequestPermissionsCamera(final boolean force) {
        int state = GetPermissionState(Manifest.permission.CAMERA);
        if (state == PERMISSIONS_STATE_OK) {
            return true;
        } else {

            showMessage(titleStr, openCameraStr, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    if (force) {
                        ActivityCompat.requestPermissions(currentActivity,
                                new String[]{Manifest.permission.CAMERA},
                                REQUEST_CODE_ASK_PERMISSIONS_CAMERA);
                    } else
                        RequestPermissions(Manifest.permission.CAMERA, REQUEST_CODE_ASK_PERMISSIONS_CAMERA);
                }
            });

            return false;
        }
    }

    // 请求储存权限
    public static boolean RequestPermissionsStorage(final boolean force) {
        int state = GetPermissionState(Manifest.permission.READ_EXTERNAL_STORAGE);
        if (state == PERMISSIONS_STATE_OK) {
            return true;
        } else {
            showMessage(titleStr, openStorageStr, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    if (force) {
                        ActivityCompat.requestPermissions(currentActivity,
                                new String[]{Manifest.permission.READ_EXTERNAL_STORAGE},
                                REQUEST_CODE_ASK_PERMISSIONS_STORAGE);
                    } else
                        RequestPermissions(Manifest.permission.READ_EXTERNAL_STORAGE, REQUEST_CODE_ASK_PERMISSIONS_STORAGE);
                }
            });

            return false;
        }
    }

    // 请求通讯录权限
    public static boolean RequestPermissionsAccount(final boolean force) {
        int state = GetPermissionState(Manifest.permission.GET_ACCOUNTS);
        if (state == PERMISSIONS_STATE_OK) {
            return true;
        } else {
            showMessage(titleStr, openAccountStr, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    if (force) {
                        ActivityCompat.requestPermissions(currentActivity,
                                new String[]{Manifest.permission.GET_ACCOUNTS},
                                REQUEST_CODE_ASK_PERMISSIONS_ACCOUNT);
                    } else
                        RequestPermissions(Manifest.permission.GET_ACCOUNTS, REQUEST_CODE_ASK_PERMISSIONS_ACCOUNT);
                }
            });

            return false;
        }
    }

    // 请求麦克风权限
    public static boolean RequestPermissionsAudio(final boolean force) {
        int state = GetPermissionState(Manifest.permission.RECORD_AUDIO);
        if (state == PERMISSIONS_STATE_OK) {
            return true;
        } else {
            showMessage(titleStr, openAudioStr, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    if (force) {
                        ActivityCompat.requestPermissions(currentActivity,
                                new String[]{Manifest.permission.RECORD_AUDIO},
                                REQUEST_CODE_ASK_PERMISSIONS_AUDIO);
                    } else
                        RequestPermissions(Manifest.permission.RECORD_AUDIO, REQUEST_CODE_ASK_PERMISSIONS_AUDIO);
                }
            });

            return false;
        }
    }
}