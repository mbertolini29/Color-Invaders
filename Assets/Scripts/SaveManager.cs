using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveManager : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();

        Load();
    }

    public void Save()
    {
        //Debug.Log("Saving");

        //create a file or open a file to save to
        FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.OpenOrCreate);

        try
        {
            //Binary Fornmater -- allows us to write data to a file
            BinaryFormatter formatter = new BinaryFormatter();
            //serialization method to write to the file
            formatter.Serialize(file, player.myStats);
        }
        catch (SerializationException e)
        {
            Debug.LogError("There was an issue serializing this data: " + e.Message);
                
        }
        finally
        {
            file.Close();
        }
    }

    public void Load()
    {
        //Debug.Log("Load");

        FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.Open);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            player.myStats = (Stats)formatter.Deserialize(file);
        }
        catch (SerializationException e)
        {
            Debug.LogError("There was an issue serializing this data: " + e.Message);

        }
        finally
        {
            file.Close();
        }
    }

}
