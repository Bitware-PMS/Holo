using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    #region GUI
    public TMP_InputField usernameGUI;
    public TMP_InputField passwordGUI;
    public GameObject loginMenu;
    public GameObject mainMenu;
    public GameObject musicMenu;
    public GameObject gamesMenu;
    public GameObject battleshipMenu;
    private String currentMenu;
    #endregion

    #region BattleShip Game Controls
    #endregion

    #region Network
    public Network client;
    #endregion

    private void Start()
    {
        client = new Network();
        currentMenu = "Login";

        if (!client.isConnected())
            connectToTable();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            switch(currentMenu)
            {
                case "Games":
                    setMenu("mainMenu");
                    break;
                case "Music":
                    setMenu("mainMenu");
                    break;
                case "Battleship":
                    stopGame();
                    setMenu("Games");
                    break;
                default:
                    break;
            }
    }

    public void connectToTable()
    {
        try
        {
            //client.clientConnect("192.168.1.98", 8000);
            client.clientConnect("localhost", 8000);
            Debug.Log("Connected to Table!");
        }
        catch (Exception err)
        {
            Debug.Log("Cannot connect to table: " + err);
        }
    }

    #region Menu Functions
    public void login()
    {

        if (usernameGUI.text == "" || passwordGUI.text == "")
            Debug.Log("Please fill all camps!");
        client.send("login");
        if (!client.receive().Equals("login"))
            return;
        client.send(usernameGUI.text);
        client.send(passwordGUI.text);
        if (client.receive().Equals("ok"))
        {
            Debug.Log("Logged in");
            setMenu("mainMenu");
        }
        else
            Debug.Log("Not Logged in");
    }

    public void setMenu(string menu)
    {
        if (menu.Equals("Login"))
        {
            loginMenu.SetActive(true);
            currentMenu = "Login";
        }
        else
            loginMenu.SetActive(false);

        if (menu.Equals("mainMenu"))
        {
            mainMenu.SetActive(true);
            currentMenu = "mainMenu";
        }
        else
            mainMenu.SetActive(false);

        if (menu.Equals("Music"))
        {
            musicMenu.SetActive(true);
            currentMenu = "Music";
        }
        else
            musicMenu.SetActive(false);

        if (menu.Equals("Games"))
        {
            gamesMenu.SetActive(true);
            currentMenu = "Games";
        }
        else
            gamesMenu.SetActive(false);

        if (menu.Equals("Battleship"))
        {
            if (startGame("Battleship"))
            {
                battleshipMenu.SetActive(true);
                currentMenu = "Battleship";
            }
            else
                setMenu("Games");
        }
        else
            battleshipMenu.SetActive(false);

    }
    #endregion

    #region Games Functions
    public bool startGame(string game)
    {
        switch(game)
        {
            case "Battleship":
                client.send("Battleship");
                if(client.receive().Equals("Battleship"))
                    return true;
                break;
            default:
                break;
        }
        return false;
    }
    #endregion

    #region Battleship Game Functions
    
    public void selectButtonBattleShip()
    {
        client.send("");
    }

    public void turnLeftBattleShip()
    {
        client.send("left");
    }

    public void shootGame()
    {
        client.send("shoot");
    }

    public void stopGame()
    {
        client.send("stop");
    }
    #endregion
}
