using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using LitJson;
using System.Text;

public class Client
{
    private static Socket client;
    public static Thread thread;
    public static ThreadStart ts;
    public static byte[] readBuffer;
    public static int maxBuffer = 8196;
    public static int alread = 0;
    public static int useByte = 0;
    public static bool isRun = false;

    public static void StartThread()
    {
        client = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //IPEndPoint iep = new IPEndPoint(IPAddress.Parse("123.207.26.191"), 7777);
        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);
        try
        {
            client.Connect(iep);
            Debug.Log("绑定");
        }
        catch(Exception e)
        {
            Debug.Log(e + "连接失败了");
        }
        Client.readBuffer = new Byte[Client.maxBuffer];
        Client.ts = new ThreadStart(Run);
        thread = new Thread(ts);
        thread.IsBackground = true;
        thread.Start();




    }

    
    public static void Run()
    {
        Debug.Log("RUN");
        Client.isRun = true;//作用是啥？忘了
        if (Client.isRun == false)
            return;
        try
        {
            client.BeginReceive(readBuffer,0,maxBuffer,0,new AsyncCallback(Receive),client);
        }
        catch(Exception e)
        {
            Debug.Log(e + "运行错误");
        }
    }
    public static void Receive(IAsyncResult ar)
    {
        Debug.Log("receive");
        Socket socket = (Socket)ar.AsyncState;
        int length = 0;
        try
        {

            Debug.Log("接收中");
            length = socket.EndReceive(ar);
            Debug.Log("接收中"+length);
        }
        catch(Exception e)
        {
            Debug.Log(e+"接收错误");
        }

        if (length > 0)
        {
            Client.alread += length;
        }
        if(Client.alread>=4)
        {
            Dispose();
        }
        client.BeginReceive(readBuffer, 0, maxBuffer, 0, new AsyncCallback(Receive), client);
    }

    private static void Dispose()
    {
        while(true)
        {
            byte[] head = new byte[4];
            Buffer.BlockCopy(readBuffer, Client.useByte, head, 0, 4);//应该是从use的偏差度，而不是alread
            int length = BitConverter.ToInt32(head,0);//没有位数因为toint
            if (alread >= useByte + 4 + length)
            {
                byte[] tempcontent = new byte[length];
                Buffer.BlockCopy(readBuffer, useByte + 4, tempcontent, 0, length);
                string content = Encoding.UTF8.GetString(tempcontent);

                JsonData jd = JsonMapper.ToObject<JsonData>(content);
                if(jd!=null)
                {
                    jd = JsonMapper.ToObject<JsonData>(content);//是什么语句，忘了
                    DisposePacket(jd);
                }
                useByte = useByte + length + 4;
            }
            if (alread - useByte >= 4)
            {
                continue;
            }
            else 
            { 
                if(alread-useByte<=0)
                {
                    readBuffer = new byte[maxBuffer];
                }
                else
                {
                    byte[] temp = new byte[alread - useByte];
                    Buffer.BlockCopy(readBuffer,useByte,temp,0,temp.Length);
                    readBuffer = new byte[maxBuffer];
                    Buffer.BlockCopy(temp,0,readBuffer,0,temp.Length);
                }
                alread = alread - useByte;
                useByte = 0;
                break;
            }

        }
    }

    private static void DisposePacket(JsonData _jd)
    {
        //JsonData jd = _jd.ToJson();//坑啊
        GameManager.AddMessages(_jd);
    }

    public static void CloseThread()
    {
        isRun = false;
        thread.Abort();//关？
        Client.client.Close();//释放socket;
        Debug.Log("线程关闭了");
    }
    //发送消息
    public static void Send(JsonData _jd)
    {
        byte[] head =new byte[4];
        byte[] content= Encoding.UTF8.GetBytes(_jd.ToJson());
        int length = content.Length;
        head = BitConverter.GetBytes(length);
        byte[] sendByte = new byte[length + 4];
        Buffer.BlockCopy(head, 0, sendByte, 0, 4);
        Buffer.BlockCopy(content,0,sendByte,4,length);
        client.Send(sendByte);
    }
}

