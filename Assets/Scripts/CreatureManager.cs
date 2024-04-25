using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

// Classe para representar os dados de uma criatura
[System.Serializable]
public class CreatureData
{
    public string name;
    public string tipo;
    public string descricao;

}
[System.Serializable]
public class CreatureList
{
    public List<CreatureData> creature;
    
}

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager instance;
    public CreatureList creaturesContainer;
    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
    }

}
