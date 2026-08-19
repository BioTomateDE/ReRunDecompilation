using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public PlayerSave state;

    public static SaveManager Instance { get; set; }

    public void Awake()
    {
        Instance = this;
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Serialize(state));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
        }
        else
        {
            NewSave();
        }
    }

    public void NewSave()
    {
        state = new PlayerSave();
        Save();
        MonoBehaviour.print("Creating new save file");
    }

    public static string Serialize<T>(T toSerialize)
    {
        XmlSerializer xmlSerializer = new(typeof(T));
        StringWriter stringWriter = new();
        xmlSerializer.Serialize(stringWriter, toSerialize);
        return stringWriter.ToString();
    }

    public static T Deserialize<T>(string toDeserialize)
    {
        XmlSerializer xmlSerializer = new(typeof(T));
        StringReader textReader = new(toDeserialize);
        return (T)xmlSerializer.Deserialize(textReader);
    }
}
