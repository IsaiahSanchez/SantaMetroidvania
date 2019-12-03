using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public PickupObject[] pickupableObjects = new PickupObject[150];
    public int size = 34;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        size = pickupableObjects.Length;

        //if playing the game
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            saveGame();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {
                File.Delete(Application.persistentDataPath + "/gamesave.save");
                Debug.Log("File Deleted");
            }
        }
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
    }

    public void saveGame()
    {
        GameData saveData = new GameData();
        saveData.size = size;
        for (int index = 0; index < size; index++)
        {
            PickupObject currentObject = pickupableObjects[index];
            //split the things apart
            saveData.isCollected[currentObject.ObjID] = currentObject.hasBeenCollected;
            Debug.Log("One Saved");
        }

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
