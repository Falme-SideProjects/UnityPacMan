using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    private string scenarioString;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetScenarioString()
    {
        if (scenarioString == null) scenarioString = string.Empty;
        return scenarioString;
    }

    public List<List<char>> GetScenarioGrid(int width, int height)
    {
        if(GetScenarioString().Equals(string.Empty)) return new List<List<char>>();

        List<List<char>> _map = new List<List<char>>();
        int index = 0;
        string _scenarioString = GetScenarioString();

        for (int h=0; h<height; h++)
        {
            _map.Add(new List<char>());

            for (int w = 0; w < width; w++, index++)
            {
                if(_scenarioString.Length <= index)
                {
                    _map[h].Add('0');
                    continue;
                }
                _map[h].Add(_scenarioString[index]);
            }
        }

        return _map;
    }

    public void SetScenarioString(string scenario)
    {
        scenarioString = scenario;
    }
}
