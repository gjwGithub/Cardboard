using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ServerSocket
{
    int count;
    byte[] data;       
    IPEndPoint ipep;
    Socket newsock;
    IPEndPoint sender;
    EndPoint Remote;
    bool isRunning = true;
    Texture2D texture = null;
	Thread thread = null;

    public ServerSocket()
    {  
        data = new byte[102400];

        //得到本机IP，设置TCP端口号         
        ipep = new IPEndPoint(IPAddress.Any, 6000);
        newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //绑定网络地址
        newsock.Bind(ipep);

        //得到客户机IP
        sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = (EndPoint)(sender);

        ////客户机连接成功后，发送欢迎信息
        //string welcome = "Welcome ! ";

        ////字符串与字节数组相互转换
        //data = Encoding.ASCII.GetBytes(welcome);

        ////发送信息
        //newsock.SendTo(data, data.Length, SocketFlags.None, Remote);

        texture = new Texture2D(960, 720);

        thread = new Thread(start);
        thread.IsBackground = true;
        thread.Start();
		Debug.Log("Thread start");
        
    }

    public void start()
    {
        while (isRunning)
        {
            //发送接受信息
			//Debug.Log("Waiting for data");
            count = newsock.ReceiveFrom(data, ref Remote);
            byte[] des = new byte[count];
			Debug.Log("count = "+count);

			Array.Copy(des, data, count);
            texture.LoadImage(des);

            int ms = Environment.TickCount;
            FileStream fs = new FileStream("/sdcard/test/" + ms + ".jpg", FileMode.Create);
            BinaryWriter sw = new BinaryWriter(fs, Encoding.Default);
            sw.Write(des);
            sw.Close();
            fs.Close();
        }
    }

    public Texture2D getTexture()
    {
        return texture;
    }

	public void close()
	{
		isRunning = false;
		thread.Abort();
        newsock.Shutdown(SocketShutdown.Both);
		newsock.Close();
	}

}

