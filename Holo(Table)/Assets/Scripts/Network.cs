using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Text;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Network : MonoBehaviour
{
    #region GUI
    public TMP_Text textoCima;
    public TMP_Text textoBaixo;
    public TMP_Text textoEsquerda;
    public TMP_Text textoDireita;
    public GameObject textoBackground;
    private String guiText;
    #endregion

    #region Variables
    private TcpClient client;
    private TcpListener server;
    private BinaryWriter writer;
    private BinaryReader reader;
    //Network Settings
    private int port = 8000;
    #endregion

    #region BattleShip
    private String battleShipEstado = "";
    public GameObject nave;
    //Nave no jogo (Player)
    private GameObject ship;
    private float score = 0;
    #endregion

    // Use this for initialization
    void Start()
    {
        guiText = "Holo";
        Debug.Log("Starting Network Script...");

        Thread network = new Thread(new ThreadStart(mainThread));
        network.IsBackground = true;
        network.Start();

        Debug.Log("Thread Started!");
    }

    private void FixedUpdate()
    {
        textoCima.text = guiText;
        textoBaixo.text = guiText;
        textoEsquerda.text = guiText;
        textoDireita.text = guiText;
        if(ship != null)
        if (ship.GetComponent<Rigidbody>() == null)
            Debug.Log("Player not in game");

        switch (battleShipEstado)
        {
            case "start":
                textoBackground.SetActive(false);
                guiText = "";
                ship = Instantiate(nave, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                score = 0;
                ship.GetComponent<Jogo>().setNetwork(this.gameObject);
                break;
            case "left":
                if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
                    ship.gameObject.GetComponent<Jogo>().virarNave(-1);
                break;
            case "right":
                if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
                    ship.gameObject.GetComponent<Jogo>().virarNave(1);
                break;
            case "shoot":
                if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
                    ship.gameObject.GetComponent<Jogo>().disparar();
                break;
            case "gameOver":
                guiText = "Score: " + score;
                textoBackground.SetActive(true);
                Destroy(ship);
                break;
            case "stop":
                Destroy(ship);
                textoBackground.SetActive(true);
                guiText = "Holo Menu";
                break;
            default:
                break;
        }
        battleShipEstado = "";

        switch(commandBoxMusic)
        {
            case "play":
                emissor.clip = songs[currentSong];
                emissor.Play();
                break;

            case "next":
                if (currentSong == songs.Length - 1)
                    currentSong = 0;
                else
                    currentSong++;
                emissor.clip = songs[currentSong];
                emissor.Play();
                break;

            case "back":
                if (currentSong == 0)
                    currentSong = songs.Length - 1;
                else
                    currentSong--;
                emissor.clip = songs[currentSong];
                emissor.Play();
                break;

            case "pause":
                emissor.Pause();
                break;

            case "stop":
                emissor.Stop();
                break;
            case "volUp":
                if (musicVolume < 1.0f)
                    musicVolume+=0.1f;
                emissor.volume = musicVolume;
                Debug.Log("Vol: " + musicVolume);
                break;
            case "volDown":
                if (musicVolume > 0)
                    musicVolume-= 0.1f;
                emissor.volume = musicVolume;
                Debug.Log("Vol: " + musicVolume);
                break;
        }
        commandBoxMusic = "";
    }

    /** Main Network Server Code **/
    private void mainThread()
    {
        Debug.Log("[1] Thread 1 Started!");
        startServer(port);
        handleClient();
    }

    // Start Server
    private void startServer(int port)
    {
        try
        {
            Debug.Log("Starting server...");
            server = new TcpListener(port);
            server.Start();
            Debug.Log("Server started on port " + port);

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void handleClient()
    {
        while (true)
        {
            client = server.AcceptTcpClient();
            writer = new BinaryWriter(client.GetStream());
            reader = new BinaryReader(client.GetStream());
            String tmp = reader.ReadString();
            if (tmp.Equals("HoloWatch"))
            {
                writer.Write("HoloTable");
                Debug.Log("Connected to HoloWatch!");
                guiText = "";
                while (true)
                {
                    switch (reader.ReadString())
                    {
                        case "login":
                            login();
                            break;
                        case "Battleship":
                            guiText = "";
                            Debug.Log("Lets Battle!");
                            writer.Write("Battleship");

                            playBattleship();
                            break;
                        case "Music":
                            writer.Write("Music");
                            musicPlayer();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                writer.Close();
                reader.Close();
                client.Close();
            }
        }
    }

    public AudioSource emissor;
    public AudioClip[] songs;
    public int currentSong = 0;
    public String commandBoxMusic = "";
    public float musicVolume = 1.0f;

    #region Music Player
    private void musicPlayer()
    {
        while(true)
        {
            switch(reader.ReadString())
            {
                case "start":
                    writer.Write("start");
                    guiText = "Music Player";
                    break;

                case "play":
                    commandBoxMusic = "play";
                    break;

                case "next":
                    commandBoxMusic = "next";
                    break;

                case "back":
                    commandBoxMusic = "back";
                    break;

                case "pause":
                    commandBoxMusic = "pause";
                    break;

                case "stop":
                    commandBoxMusic = "stop";
                    break;
                case "volUp":
                    commandBoxMusic = "volUp";
                    break;
                case "volDown":
                    commandBoxMusic = "volDown";
                    break;
                case "stopMusic":
                    guiText = "";

                    writer.Write("stop");
                    return;
                    break;
            }
        }
    }
    #endregion

    #region App Functions
    private void login()
    {
        Debug.Log("Starting Login Process.");
        writer.Write("login");
        Debug.Log("Starting Login Process..");
        String username = reader.ReadString();
        String password = reader.ReadString();

        Debug.Log("HoloWatch [Username => " + username + ", Password => " + password + "]");
        switch (username)
        {
            case "Ivan":
                if (password.Equals("12345"))
                    writer.Write("ok");
                else
                    writer.Write("nok");
                break;
            case "Isabel":
                if (password.Equals("12345"))
                    writer.Write("ok");
                else
                    writer.Write("nok");
                break;
            case "Eurico":
                if (password.Equals("12345"))
                    writer.Write("ok");
                else
                    writer.Write("nok");
                break;
            case "demo":
                if (password.Equals("demo"))
                    writer.Write("ok");
                else
                    writer.Write("nok");
                break;
            default:
                writer.Write("nok");
                break;
        }

    }

    #region Battleship
    private void playBattleship()
    {
        setBattleShipEstado("start");
        guiText = "";
        while (true)
        {
            string estado = reader.ReadString();
            setBattleShipEstado(estado);
            if (estado.Equals("stop"))
                break;
        }
    }
    
    public void setBattleShipEstado(String estado)
    {
        battleShipEstado = estado;
    }
    
    public void gameOver()
    {
        battleShipEstado = "gameOver";
        guiText = "Score: " + score;
        textoBackground.SetActive(true);
        Destroy(ship);
    }

    public void setScore(float score)
    {
        this.score = score;
    }

    #endregion

    #endregion
}
