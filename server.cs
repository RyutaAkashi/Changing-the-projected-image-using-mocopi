using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position_Lhand : MonoBehaviour
{
    public string ipAddress = "192.168.XX.XX";
    public int port = XXXX;
    private Socket clientSocket;

    void Start()
    {
        ConnectToServer();
    }

    // server
    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress serverIP = IPAddress.Parse(ipAddress);
        IPEndPoint serverEndPoint = new IPEndPoint(serverIP, port);

        try
        {
            clientSocket.Connect(serverEndPoint);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError("Socket connection error: " + e.Message);
        }
    }

    // calculate length
    private double cal_len(float[] a,float[] b)
    {
        double c = System.Math.Pow((a[0] - b[0]), 2)
                    + System.Math.Pow((a[1] - b[1]), 2)
                    + System.Math.Pow((a[2] - b[2]), 2);
        return System.Math.Sqrt(c);
    }

    // Update is called once per frame
    void Update()
    {
        int cnt = 0;
        float[] right_hand  = new float[3] { 0.0f, 0.0f, 0.0f };
        float[] left_hand   = new float[3] { 0.0f, 0.0f, 0.0f };
        float[] right_elbow = new float[3] { 0.0f, 0.0f, 0.0f };
        float[] left_elbow  = new float[3] { 0.0f, 0.0f, 0.0f };
        float[] torso       = new float[3] { 0.0f, 0.0f, 0.0f };

        GameObject[] hands = GameObject.FindGameObjectsWithTag("USE_pos");
        foreach (var hand in hands)
        {
            // receive USE_pos tag position
            var pos = hand.gameObject.transform.position;

            cnt++;
            switch (cnt)
            {
                case 1:
                    {
                        right_hand[0] = pos.x;
                        right_hand[1] = pos.y;
                        right_hand[2] = pos.z;
                    }
                    break;
                case 2:
                    {
                        left_hand[0] = pos.x;
                        left_hand[1] = pos.y;
                        left_hand[2] = pos.z;
                    }
                    break;
                case 3:
                    {
                        right_elbow[0] = pos.x;
                        right_elbow[1] = pos.y;
                        right_elbow[2] = pos.z;
                    }
                    break;
                case 4:
                    {
                        torso[0] = pos.x;
                        torso[1] = pos.y;
                        torso[2] = pos.z;
                    }
                    break;
                case 5:
                    {
                        left_elbow[0] = pos.x;
                        left_elbow[1] = pos.y;
                        left_elbow[2] = pos.z;
                    }
                    break;
            }            
        }

        // calculate length
        double len_hands = cal_len(right_hand, left_hand);
        double len_elbow = cal_len(right_elbow, left_elbow);
        double len_right = cal_len(right_hand, torso);
        double len_left  = cal_len(left_hand, torso);

        int flg_no = 0;
        int flg_right = 0;
        int flg_left = 0;

        // judge length
        if (len_hands <= 0.1 && len_elbow >= 0.2 && right_hand[1]-right_elbow[1] >= 0.1)
        { 
            flg_no = 1;
        }
        if (len_right >= 0.6 && right_hand[1] >= 1.0)
        { 
            flg_right = 1;
        }
        if (len_left >= 0.6 && left_hand[1] >= 1.0)
        {
            flg_left = 1;
        }
        
        // result
        string result;
        if(flg_no == 1)
        {
            result = "batu";
        }
        else if(flg_right == 1 && flg_right == 1)
        {
            result = "SOS";
        }
        else if(flg_right == 1)
        {
            result = "right";
        }
        else if (flg_left == 1)
        {
            result = "left";
        }
        else
        {
            result = "non";
        }

        Debug.Log(result);
        SendDataToServer(result);
    }

    void SendDataToServer(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    void OnDestroy()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }

}

