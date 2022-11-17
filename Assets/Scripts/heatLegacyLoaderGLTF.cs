using System.Collections;
using System.Collections.Generic;
using System;
using System.IO; 
using UnityEngine;
using UnityEngine.Networking;
using Siccity.GLTFUtility;

public class heatLegacyLoaderGLTF : MonoBehaviour
{
    string filePath;  
    GameObject spaceShip;   
    string url;
    AnimationClip[] animClips;
    GameObject selectedAvatar;

    void Start()
    {
        selectedAvatar = GameObject.Find("HEAT_T69H_unity_mesh");

        filePath = $"{Application.persistentDataPath}/Files/HEAT_SCADUTO.gltf";

        url = "https://drive.google.com/uc?export=download&id=1caEzV7kwNg4fMO-kpXjw6_WU-xWhi-lo"; 

        DownloadFile(url);
    }

    void Update()
    {
        
    }
   
    public void DownloadFile(string url)
    {
        string path = filePath;      //GetFilePath(url); 

        // if(File.Exists(path))
        // {
        //     Debug.Log("Found the same file locally, Loading!!!");

        //     LoadModel(path);

        //     return; 
        // }

        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
        {
            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
            }

            else
            {
                //Save the model fetched from firebase into spaceShip 
                LoadModel(path); 
            }
        })); 
    }    

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}"; 
    }

    void LoadModel(string path)
    {
        var i = new ImportSettings();
        // legacyAnimSetting = new ImportSettings(); legacyAnimSetting.useLegacyClips = true; legacyAnimSetting.interpolationMode = InterpolationMode.STEP;
        GameObject result = Importer.LoadFromFile(filePath, i, out animClips);

        if (animClips.Length > 0)
        {
            Debug.Log("here");
            Animation anim = result.AddComponent<Animation>();
            Debug.Log(animClips[0].name);
            animClips[0].legacy = true;
            // anim.AddClip(animClips[0], animClips[0].name);
            // anim.clip = anim.GetClip(animClips[0].name);
            // anim.wrapMode = WrapMode.Loop;
            // anim.Play();

            // PLAY ANIMATION
            // selectedAvatar.GetComponent<Animation>().AddClip(animClips[0], animClips[0].name);
            // selectedAvatar.GetComponent<Animation>().clip = selectedAvatar.GetComponent<Animation>().GetClip(animClips[0].name);
            // selectedAvatar.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            // selectedAvatar.GetComponent<Animation>().Play(animClips[0].name);
        }
    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(filePath);

            yield return req.SendWebRequest(); 
            callback(req);
        }
    }
}
