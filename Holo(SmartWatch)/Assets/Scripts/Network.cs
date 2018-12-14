using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Network : MonoBehaviour {
    #region Client
    //Socket data writter
    private BinaryWriter writer;
    //Socket data reader
    private BinaryReader reader;
    //Socket connection (If is client, use client1 only, if server, use both.
    private TcpClient client;
    #endregion

    #region Métodos Construtores
    public Network()
    {

    }
    #endregion

    

    #region Métodos relacionados com o cliente
    public bool clientConnect(string ip, int port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(ip, port);
            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());
            writer.Write("HoloWatch");
            if(reader.ReadString().Equals("HoloTable"))
                return true;

        }
        catch (Exception err)
        {
            Debug.Log("Cannot connect to Server! Reason: "+err);
        }
        return false;
    }

    public void clientDisconnect()
    {
        try
        {
            writer.Close();
            writer = null;
        }catch(Exception err)
        {
            Debug.Log("ERROR Closing Writer: " + err);
        }
        try
        {
            reader.Close();
            reader = null;
        }
        catch (Exception err)
        {
            Debug.Log("ERROR Closing Reader: " + err);
        }
        try
        {
            client.Close();
            client = null;
        }
        catch (Exception err)
        {
            Debug.Log("ERROR Closing Client: " + err);
        }
    }


    public bool isConnected()
    {
        if (client != null && client.Client != null && client.Client.Connected)
            return true;
        else
            return false;
    }

    public void send(string data)
    {
        try
        {
            writer.Write(data);
        }catch(Exception err)
        {
            Debug.Log("Cannot send data: " + err);
        }
    }

    public string receive()
    {
        try
        {
            return reader.ReadString();
        }
        catch (Exception err)
        {
            Debug.Log("Cannot read data: " + err);
            return null;
        }
    }
    #endregion
}
