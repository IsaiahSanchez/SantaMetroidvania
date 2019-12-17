using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public PickupObject[] pickupableObjects = new PickupObject[150];
    public int size = 34;
    public bool bossIsAlive = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //if playing the game
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            size = pickupableObjects.Length;

            for (int index = 0; index < size; index++)
            {
                pickupableObjects[index].ObjID = index;
            }

            //if has a save file load it. if not give IDs to all of the game objects in the scene
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {
                Debug.Log("File found at : " + Application.persistentDataPath.ToString() + "/gamesave.save" + " Now Loading");
                LoadGame();
            }
        }

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!(File.Exists(Application.persistentDataPath + "/gamesave.save")))
            {
                StartCoroutine(startupDialogs());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            deleteFile();
        }
    }

    public bool doesFileExist()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void deleteFile()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
            Debug.Log("File Deleted");
        }
    }

    private IEnumerator startupDialogs()
    {
        UIManager.Instance.showPowerup("Use W A S D or left stick to move");
        yield return new WaitForSeconds(6f);
        UIManager.Instance.showPowerup("Use Space, K or the A button to jump");
    }

    public void LoadGame()
    {
        //when loading go through the list one by one and find one of the game objects that are within that distance of x and y and then give them their ID and hide them if they are supposed to be gone
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        GameData saveData = (GameData)bf.Deserialize(file);
        file.Close();

        size = saveData.size;

        for (int index = 0; index < size; index++)
        {
            pickupableObjects[index].hasBeenCollected = saveData.isCollected[index];
            if (pickupableObjects[index].hasBeenCollected == true)
            {
                pickupableObjects[index].gameObject.SetActive(false);
            }
        }

        bossIsAlive = saveData.BossIsAlive;
        StartCoroutine(playerChanges(saveData));
    }

    private IEnumerator playerChanges(GameData save)
    {
        yield return new WaitForEndOfFrame();
        givePlayerPowers(save);
        changePlayerSpawnLocation(save);
        BossMain.instance.setBossAliveState(bossIsAlive);
        GameManager.Instance.mainPlayer.setPresents(save.presentsCollected);
        GameManager.Instance.mainPlayer.setPlayerHealthMax(save.playerMaxHealth, true);
        UIManager.Instance.shakePresentPanel();
    }

    private void givePlayerPowers(GameData save)
    {
        if (save.playerDoubleJump == true)
        {
            GameManager.Instance.mainPlayer.givePower(0, false);
        }

        if (save.playerDash == true)
        {
            GameManager.Instance.mainPlayer.givePower(1, false);
        }

        if (save.playerSnowball == true)
        {
            GameManager.Instance.mainPlayer.givePower(2, false);
        }
    }

    private void changePlayerSpawnLocation(GameData save)
    {
        GameManager.Instance.mainPlayer.transform.position = new Vector2(save.xSpawn, save.ySpawn);
    }

    public void saveGame(Vector2 PlayerSpawnLocation)
    {
        GameData saveData = new GameData();
        saveData.size = size;
        //saving all the pickupable objects
        for (int index = 0; index < size; index++)
        {
            PickupObject currentObject = pickupableObjects[index];
            //split the things apart
            saveData.isCollected[currentObject.ObjID] = currentObject.hasBeenCollected;
        }

        //saving all of the player information
        saveData.playerDash = GameManager.Instance.mainPlayer.hasDashPower;
        saveData.playerDoubleJump = GameManager.Instance.mainPlayer.hasDoubleJumpPower;
        saveData.playerSnowball = GameManager.Instance.mainPlayer.hasSnowBallPower;
        saveData.playerMaxHealth = GameManager.Instance.mainPlayer.maxPlayerHealth;
        saveData.presentsCollected = GameManager.Instance.mainPlayer.numberOfPresentsCollected;
        saveData.xSpawn = PlayerSpawnLocation.x;
        saveData.ySpawn = PlayerSpawnLocation.y;
        saveData.BossIsAlive = bossIsAlive;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, saveData);
        file.Close();

        Debug.Log("Finished Saving Successfully");

        //if quiting or hit a checkpoint then save game
        //save game by breaking down all pickupable objects in the list and putting them into thier arrays and saving it
        //save all other data
    }
}
