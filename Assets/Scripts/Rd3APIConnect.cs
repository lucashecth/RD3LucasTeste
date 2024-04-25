using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using static CreatureManager;

public class Rd3APIConnect : MonoBehaviour
{
    private string loginEndpoint = "https://rd3space.com/becoapi/login";
    private string apiKeyEndpoint = "https://rd3space.com/becoapi/creature.php?key=";
    private string email = "testedev@rd3.digital";
    private string password = "rd3digital";
    private string token;
    public TMP_Text statusResponse;

    void Start()
    {
        StartCoroutine(LoginAndGetData());
    }

    IEnumerator LoginAndGetData()
    {
        // Login
        yield return StartCoroutine(Login());

        // Após o login bem-sucedido, consumir a API para obter dados
        if (!string.IsNullOrEmpty(token))
        {
            yield return StartCoroutine(GetData("pu")); // Exemplo de consulta para o modelo Pulsatrix
            yield return StartCoroutine(GetData("et")); // Exemplo de consulta para o modelo Eternus
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginEndpoint, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Erro no login: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                token = responseText.Replace("\"", "");
                token = token.Replace("{token:", "");
                token = token.Replace("}", "");
            }
        }
    }

    IEnumerator GetData(string key)
    {
        string url = apiKeyEndpoint + key;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Authorization", token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Erro ao obter dados: " + www.error);
            }
            else
            { 
                statusResponse.text = "O Retorno da chamada da API: " + www.responseCode;
                // Attempt to deserialize the JSON data
                CreatureList localCreaturesList = JsonUtility.FromJson<CreatureList>(www.downloadHandler.text);
                Debug.Log(CreatureManager.instance.creaturesContainer.ToString());

                foreach (CreatureData creature in localCreaturesList.creature)
                {
                    CreatureManager.instance.creaturesContainer.creature.Add(creature);
                }
            }
        }

    }

}
