using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System;

public class HTTPManager : MonoBehaviour
{
    public void PostData(string name, int score) => StartCoroutine(PostData_Coroutine(name, score));

    IEnumerator PostData_Coroutine(string name, int score)
    {
        var createJSON = new CreateJSON
        {
            name = name,
            score = score
        };
        string uri = "https://script.google.com/macros/s/AKfycbwzvyt4PGM70Y46At-DbGTSZ4tJO1Dr90hcjgcg9XAv_KIOYoR8CbzTHlAAEEX_fP2G/exec";
        string logs = JsonUtility.ToJson(createJSON);
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.SetRequestHeader("Content-Type", "text/plain");
        byte[] body = new System.Text.UTF8Encoding().GetBytes(logs);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log(request.error);
        else
            Debug.Log(request.downloadHandler.text);
    }





    IEnumerator GetScoreboard()
    {
        string spreadsheet_id = "1fMX6Ia_oV_QawO1m_77QnllaIeDG5xD1LzEHFCEyPbc";
        string tab_name = "Sheet1";
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheet_id}/values/{tab_name}?alt=json&key=AIzaSyDxifIroKLD_vluX4FZ7zKZXdU5dWrkJPo";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
                Debug.Log(request.error);
            else
            {
                var message = JsonConvert.DeserializeObject<ParseScoreboard>(request.downloadHandler.text);
                if (GameManager.Instance.getGameState() == GameState.StartMenu)
                {
                    SetScoreboardItemsStart(message.values);
                }
                else
                {
                    SetScoreboardItems(message.values);
                }

            }

        }
    }

    private string currentName;
    private int currentScore;
    private bool isSelected;
    public void GetData(string currentName, int currentScore) 
    {
        this.currentName = currentName;
        this.currentScore = currentScore;
        yPos = 0;
        StartCoroutine(GetScoreboard());
    } 

    [SerializeField] private GameObject scoreboardItem;
    [SerializeField] private RectTransform scoreboardItemPos, scoreboardItemPosStartMenu;
    private float yPos;
    private bool isFade;
    private void SetScoreboardItems(string[][] values)
    {
        int length = values.Length;
        for (int i = 1; i < length; i++)
        {
            GameObject item = Instantiate(scoreboardItem, scoreboardItemPos.position, Quaternion.identity);
            item.transform.SetParent(scoreboardItemPos.transform, false);
            ScoreboardItem itemComponent = item.GetComponent<ScoreboardItem>();

            int offset = isSelected ? 1 : 0;
            if (currentScore >= Int32.Parse(values[i - offset][1]) && !isSelected)
            {
                isSelected = true;
                length++;
                itemComponent.score.text = currentScore.ToString();
                itemComponent.playerName.text = currentName;
                itemComponent.HighlightName();
            }
            else
            {
                itemComponent.score.text = values[i - offset][1];
                itemComponent.playerName.text = values[i - offset][0];
            }
            itemComponent.rank.text = i.ToString();
            itemComponent.backgroundimage.alpha = isFade ? 1 : 0.6f;
            
            item.transform.position = scoreboardItemPos.position;
            Vector3 itemPosition = item.GetComponent<RectTransform>().anchoredPosition;
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(itemPosition.x, yPos, itemPosition.z);
            isFade = !isFade;
            yPos -= 50f;
        }
    }

    private void SetScoreboardItemsStart(string[][] values)
    {
        int length = values.Length;
        for (int i = 1; i < length; i++)
        {
            GameObject item = Instantiate(scoreboardItem, scoreboardItemPosStartMenu.position, Quaternion.identity);
            item.transform.SetParent(scoreboardItemPosStartMenu.transform, false);
            ScoreboardItem itemComponent = item.GetComponent<ScoreboardItem>();
            itemComponent.score.text = values[i][1];
            itemComponent.playerName.text = values[i][0];
            
            itemComponent.rank.text = i.ToString();
            itemComponent.backgroundimage.alpha = isFade ? 1 : 0.6f;

            item.transform.position = scoreboardItemPosStartMenu.position;
            Vector3 itemPosition = item.GetComponent<RectTransform>().anchoredPosition;
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(itemPosition.x, yPos, itemPosition.z);
            isFade = !isFade;
            yPos -= 50f;
        }
    }
}




public class ParseScoreboard
{
    public string range;
    public string majorDimension;
    public string[][] values;
}

public class CreateJSON
{
    public string name;
    public int score;
}