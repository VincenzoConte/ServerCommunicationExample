using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerExample : MonoBehaviour
{
    private const string URL = "ec2-34-218-246-178.us-west-2.compute.amazonaws.com:80";
    //private const string URL = "localhost:1337";
    static ServerExample instance;

    private void Awake()
    {
        instance = this;
    }

    public struct KeyValue
    {
        public string key;
        public string value;
    }

    #region User registration
    public static void UserRegistration(string username, string facebookID = null)
    {
        instance.StartCoroutine(UserRegistrationCoroutine(username, facebookID));
    }

    static IEnumerator UserRegistrationCoroutine(string username, string facebookID = null)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        if (facebookID != null)
            form.AddField("facebookid", facebookID);

        //creating request
        UnityWebRequest request = UnityWebRequest.Post(URL + "/register", form);

        //sending request and waiting for response
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            //error handling
            Debug.Log("request ended with an error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            //response ended succesfully

            //get user id
            string id = request.GetResponseHeader("id");

            Debug.Log("registration completed, id: " + id);

            //id must be saved for future requests
        }

    }
    #endregion


    #region Facebook association
    public static void FacebookAssociation(string userID, string facebookID)
    {
        instance.StartCoroutine(FacebookAssociationCoroutine(userID, facebookID));
    }

    static IEnumerator FacebookAssociationCoroutine(string userID, string facebookID)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("userid", userID);
        form.AddField("facebookid", facebookID);

        //creating request
        UnityWebRequest request = UnityWebRequest.Post(URL + "/associatefacebook", form);

        //sending request and waiting for response
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            //error handling
            Debug.Log("request ended with an error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            //response ended succesfully
            Debug.Log(request.downloadHandler.text);
        }

    }

    #endregion



    #region Start game session
    public static void StartGameSession(string userID, string locationID)
    {
        instance.StartCoroutine(StartGameSessionCoroutine(userID, locationID));
    }

    static IEnumerator StartGameSessionCoroutine(string userID, string locationID)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("userid", userID);
        form.AddField("locationid", locationID);

        //creating request
        UnityWebRequest request = UnityWebRequest.Post(URL + "/startsession", form);

        //sending request and waiting for response
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            //error handling
            Debug.Log("request ended with an error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            //response ended succesfully

            //get session id
            string sessionID = request.GetResponseHeader("sessionid");

            Debug.Log("session created with id: " + sessionID);

            //session id must be saved for future requests
        }

    }

    #endregion


    #region Confirm session
    public static void ConfirmSession(string userID, string locationID)
    {
        instance.StartCoroutine(ConfirmSessionCoroutine(userID, locationID));
    }

    static IEnumerator ConfirmSessionCoroutine(string userID, string locationID)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("userid", userID);
        form.AddField("locationid", locationID);

        //creating request
        UnityWebRequest request = UnityWebRequest.Post(URL + "/confirmsession", form);

        //sending request and waiting for response
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            //error handling
            Debug.Log("request ended with an error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            //response ended succesfully

            Debug.Log(request.downloadHandler.text);
        }

    }

    #endregion


    #region Get new session
    public static void GetNewSession(string locationID)
    {
        instance.StartCoroutine(GetNewSessionCoroutine(locationID));
    }

    static IEnumerator GetNewSessionCoroutine(string locationID)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("locationid", locationID);

        //creating request
        UnityWebRequest request = UnityWebRequest.Post(URL + "/getnewsession", form);

        //sending request and waiting for response
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            //error handling
            Debug.Log("request ended with an error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            //response ended succesfully
            var sessionid = request.GetResponseHeader("sessionid");
            var userid = request.GetResponseHeader("userid");

            Debug.LogFormat("Found session with ID {0} for user {1}", sessionid, userid);
        }

    }

    #endregion


    #region End game session
    public static void EndGameSession(string userID, string locationID, List<KeyValue> gameState)
    {
        var gameStateString = FormatGameState(gameState);
        if (gameStateString != null)
            instance.StartCoroutine(EndGameSessionCoroutine(userID, locationID, gameStateString));
    }

    static string FormatGameState(List<KeyValue> gameState)
    {
        if (gameState.Count <= 0)
            return null;

        StringBuilder formattedString = new StringBuilder();
        foreach (var pair in gameState)
        {
            formattedString.AppendFormat("{0}:{1};", pair.key, pair.value);
        }

        return formattedString.ToString();
    }

    static IEnumerator EndGameSessionCoroutine(string userID, string locationID, string gameState)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("userid", userID);
        form.AddField("locationid", locationID);
        form.AddField("gamestate", gameState);

        //creating request
        UnityWebRequest request = UnityWebRequest.Post(URL + "/endsession", form);

        //sending request and waiting for response
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            //error handling
            Debug.Log("request ended with an error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            //response ended succesfully
            Debug.Log(request.downloadHandler.text);
        }

    }

    #endregion

}
