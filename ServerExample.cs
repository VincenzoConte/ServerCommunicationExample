using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerExample: MonoBehaviour
{
    private const string URL = "ec2-34-218-246-178.us-west-2.compute.amazonaws.com:80";

    static ServerExample instance;

    private void Awake()
    {
        instance = this;
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
        UnityWebRequest request = UnityWebRequest.Post(URL+"/register", form);

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
    public static void StartGameSession(string userID, string locationID /*id of the actual location of the player*/)
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

            Debug.Log("session created with id: "+sessionID);

            //session id must be saved for future requests
        }

    }

    #endregion



    #region End game session
    public static void EndGameSession(string userID, string sessionID, int score)
    {
        instance.StartCoroutine(EndGameSessionCoroutine(userID, sessionID, score));
    }

    static IEnumerator EndGameSessionCoroutine(string userID, string sessionID, int score)
    {
        //adding parameters for the POST request
        WWWForm form = new WWWForm();
        form.AddField("userid", userID);
        form.AddField("sessionid", sessionID);
        form.AddField("score", score);

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
