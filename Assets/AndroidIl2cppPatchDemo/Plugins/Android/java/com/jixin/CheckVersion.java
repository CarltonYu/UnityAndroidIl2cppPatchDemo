package com.jixin;

import android.os.Handler;
import android.os.Message;
import android.util.Log;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.zip.ZipFile;

import io.github.noodle1983.Boostrap;

public class CheckVersion {
    public static String cpuTarget = "";
    public static String TAG = "CheckVersion";
    public static String Host = "http://192.168.50.188";
//    public static String Host = "http://jixin.xdapp.com";
    public static String base_url = Host+"/patch/check/index.php";
    public static String base_download_format = Host+"/patch/Patch_%s_%s.zip";
    public static String local_path_format = "%s/AllAndroidPatchFiles_Version%s.zip";
    public static String unzip_dir_format = "%s/Version%s";
    public static String sozip_path_format = "%s/Version%s/lib_%s_libil2cpp.so.zip";
    public static String cache_format = "%s/il2cpp";
    public static void CheckVersion(String cpu,final Handler handler,final String local_dir,final String local_unzip_root_dir){
        cpuTarget = cpu;
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    URL url = new URL(base_url);
                    Log.e(TAG,"===== url:"+url);
                    //得到connection对象。
                    HttpURLConnection connection = (HttpURLConnection) url.openConnection();
                    //设置请求方式
                    connection.setRequestMethod("GET");
                    //连接
                    connection.connect();
                    //得到响应码
                    int responseCode = connection.getResponseCode();
                    if(responseCode == HttpURLConnection.HTTP_OK){
                        //得到响应流
                        InputStream inputStream = connection.getInputStream();
                        //将响应流转换成字符串
                        String result = is2String(inputStream);//将流转换为字符串。
//                        http://jixin.xdapp.com/patch/AllAndroidPatchFiles_Version0.zip

//                        Message msg = new Message();
//                        msg.what = StartActivity.Handler_CheckVersion;
//                        msg.obj = String.format(base_download_format,result);
//                        handler.sendMessage(msg);
                        String download_url = String.format(base_download_format,cpuTarget,result);
                        String local_path = String.format(local_path_format,local_dir,result);
                        Log.e("kwwl","result============="+result+"\ndownload_url:"+download_url+"\nlocal_path:"+local_path);
                        DownLoadZip(handler,download_url,local_path,local_unzip_root_dir,result);
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }).start();
    }

    public static void DownLoadZip(final Handler handler,String download_url,String local_path,String local_dir,String version) throws Exception {

        File file = new File(local_path);
        if(file.exists() == false){
            HttpURLConnection httpURLConnection = getDownloadConnection(download_url);
            int contentLength = httpURLConnection.getContentLength();
            Log.e(TAG,"======= 文件的大小是:"+contentLength);
            if (contentLength>32) {
                InputStream is= httpURLConnection.getInputStream();
                BufferedInputStream bis = new BufferedInputStream(is);
                FileOutputStream fos = new FileOutputStream(local_path);
                BufferedOutputStream bos= new BufferedOutputStream(fos);
                int b = 0;
                byte[] byArr = new byte[1024];
                while((b= bis.read(byArr))!=-1){
                    bos.write(byArr, 0, b);
                }
                is.close();
                bis.close();
                bos.flush();
                bos.close();
                Log.e(TAG,"====== 下载的文件的大小是:"+contentLength);
            }
        }
        UnZipFile(handler,local_path,local_dir,version);
    }
    public static void UnZipFile(final Handler handler,String local_path,String local_dir,String version) throws Exception {

        String sozipfile = String.format(sozip_path_format,local_dir,version, Boostrap.getarchabi());
//        Log.e(TAG,"====== sozipfile:"+sozipfile);
        File sozipf = new File(sozipfile);
        String unzip_dir = String.format(unzip_dir_format,local_dir,version);
        if(sozipf.exists() == false){
            File VersionF = new File(unzip_dir);
            VersionF.deleteOnExit();
            ZipUtils.UnZipFolder(local_path,unzip_dir);
            Log.e(TAG,"====== 解压完成 目录地址:"+unzip_dir);
            ZipUtils.UnZipFolder(sozipfile,unzip_dir);
        }

        String error = Boostrap.usedatadir(unzip_dir,"");
        Log.e(TAG,"====== Boostrap.usedatadir:"+unzip_dir);
        if((error == null || error.isEmpty()) == false){
            Log.e(TAG,"====== return!  error:"+error);
            return;
        }
        String cacheDir = String.format(cache_format,local_dir);
        Log.e(TAG,"====== cacheDir:"+cacheDir);
        File cachefile = new File(cacheDir);
        if(cachefile.exists()){
            cachefile.delete();
        }

        Message msg = new Message();
        msg.what = StartActivity.Handler_UnzipFinish;
        msg.obj = version;
        Log.e(TAG,"======= sendMessage");
        handler.sendMessage(msg);
    }
    public static HttpURLConnection getDownloadConnection(String httpUrl) throws Exception {
        URL url = new URL(httpUrl);
        HttpURLConnection connection =  (HttpURLConnection) url.openConnection();
        connection.setRequestMethod("GET");
        connection.setRequestProperty("Content-Type", "application/octet-stream");
        connection.setDoOutput(true);
        connection.setDoInput(true);
        connection.setRequestProperty("Connection", "Keep-Alive");
        connection.connect();
        return connection;

    }
    public static String is2String(InputStream is) throws Exception{

        //连接后，创建一个输入流来读取response
        BufferedReader bufferedReader = new BufferedReader(new
                InputStreamReader(is,"utf-8"));
        String line = "";
        StringBuilder stringBuilder = new StringBuilder();
        String response = "";
        //每次读取一行，若非空则添加至 stringBuilder
        while((line = bufferedReader.readLine()) != null){
            stringBuilder.append(line);
        }
        //读取所有的数据后，赋值给 response
        return stringBuilder.toString().trim();
    }
}
