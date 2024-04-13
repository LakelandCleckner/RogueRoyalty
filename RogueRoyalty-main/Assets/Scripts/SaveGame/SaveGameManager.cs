using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/* SaveGameManager.cs
 * Lakeland Cleckner (301344797) 
 * 2024-02-25
 * 
 * Last Modified Date: 2024-02-27
 * Last Modified by: Lakeland Cleckner
 * 
 * 
 * Version History:
 *      -> February 25th, 2024 (Lakeland Cleckner)
 *          - Created script to handle saving and loading functions. 
 *      -> February 27th, 2024 (Lakeland Cleckner)   
 *          -Fixed LoadButton losing reference to method
 *          
 * 
 * 
 *Saving/loading
 * V 1.0
 */

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public string sceneName;
}

public class SaveGameManager : MonoBehaviour
{
    private static SaveGameManager _instance;
    public static SaveGameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private Vector3 loadedPlayerPosition;
    private string loadedSceneName;
    private bool isLoadRequested = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Debug.Log("An instance of SaveGameManager already exists. Destroying duplicate.");
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("SaveGameManager instance assigned.");
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded += AssignLoadButtonListener;
        //Debug.Log("SaveGameManager enabled and listening for sceneLoaded.");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded -= AssignLoadButtonListener;
        //Debug.Log("SaveGameManager disabled and no longer listening for sceneLoaded.");
    }

    private void AssignLoadButtonListener(Scene scene, LoadSceneMode mode)
    {
        GameObject buttonObj = GameObject.FindGameObjectWithTag("LoadButton");
        if (buttonObj != null)
        {
            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(RequestLoadGame);
                //Debug.Log("Load button listener assigned.");
            }
        }
    }


    public void SaveGame(Transform playerTransform)
    {
        try
        {
            PlayerData data = new PlayerData();
            data.position = playerTransform.position;
            data.sceneName = SceneManager.GetActiveScene().name;

            string json = JsonUtility.ToJson(data);
            string filePath = Application.persistentDataPath + "/playerInfo.json";
            File.WriteAllText(filePath, json);

            //Debug.Log($"Game saved with position: {data.position} and scene: {data.sceneName}");
        }
        catch (Exception ex)
        {
            //Debug.LogError("Failed to save game: " + ex.ToString());
        }
    }

    public void RequestLoadGame()
    {
        string path = Application.persistentDataPath + "/playerInfo.json";
        if (File.Exists(path))
        {
            try
            {
                //Debug.Log("Save file found, preparing to load game.");
                string json = File.ReadAllText(path);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                if (data != null && !string.IsNullOrEmpty(data.sceneName))
                {
                    loadedPlayerPosition = data.position;
                    loadedSceneName = data.sceneName;
                    isLoadRequested = true;

                    //Debug.Log($"Requested load game with position: {loadedPlayerPosition} and scene: {loadedSceneName}");
                    SceneManager.LoadSceneAsync(loadedSceneName); // Load the saved scene.
                }
                else
                {
                    //Debug.LogError("Save data is corrupt or invalid.");
                }
            }
            catch (Exception ex)
            {
                //Debug.LogError("Failed to load game: " + ex.ToString());
            }
        }
        else
        {
            //Debug.LogError("Save file not found in path: " + path);
        }
    }

    private IEnumerator ApplyLoadedPositionWithDelay()
    {
        //Debug.Log("Applying loaded position after delay.");
        // Wait for the end of frame to ensure all GameObjects are fully loaded
        yield return new WaitForEndOfFrame();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //Debug.Log($"Setting player position to: {loadedPlayerPosition}");
            player.transform.position = loadedPlayerPosition;
            //Debug.Log($"Player position after setting: {player.transform.position}");
        }
        else
        {
            //Debug.LogError("Player object not found in the scene.");
        }

        isLoadRequested = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Scene loaded: {scene.name}, Load Mode: {mode}");
        if (isLoadRequested)
        {
            if (scene.name == loadedSceneName)
            {
                //Debug.Log("Load request is true and scene name matches. Starting ApplyLoadedPositionWithDelay coroutine.");
                StartCoroutine(ApplyLoadedPositionWithDelay());
            }
            else
            {
               //Debug.LogError($"Mismatch in scene names. Expected: {loadedSceneName}, but loaded: {scene.name}");
            }
        }
        else
        {
            //Debug.Log("Scene loaded, but no load was requested. Not applying loaded position.");
        }
    }

}
