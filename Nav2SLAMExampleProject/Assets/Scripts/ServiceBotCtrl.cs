using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq; // ���� System.Linq �����ռ�

public class ServiceBotCtrl : MonoBehaviour
{
    private static ServiceBotCtrl instance;
    public static ServiceBotCtrl Instance => instance;

    private Dictionary<string, APIControllerBase> APIControllers = new Dictionary<string, APIControllerBase>();
    private List<API> APIs = new List<API>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log("ServiceBotCtrl Successfully Awaked");

        LoadAPIsFromJSON();
    }

    void Start()
    {
        // ����order���Ե����ȼ�����ִ��API����
        ExecuteAPIs();
    }

    public void RegisterAPIController(string APIName, APIControllerBase controller)
    {
        if (!APIControllers.ContainsKey(APIName))
        {
            APIControllers.Add(APIName, controller);
        }
    }

    public void LoadAPIsFromJSON()
    {
        // ��TXT�ļ��ж�ȡ��ԴAPI����
        string filePath = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts\\smart_home_api_response.txt";
        string jsonContent = File.ReadAllText(filePath);

        // ����JSON���ݲ��洢��APIs�б���
        var resourceAPIs = JsonConvert.DeserializeObject<ResourceAPIContainer>(jsonContent);
        APIs = resourceAPIs.ResourceAPIs;

        // ���API�б������
        Debug.Log("resourceAPIs: ");
        PrintAPIs(resourceAPIs.ResourceAPIs);
        Debug.Log("APIs: ");
        PrintAPIs(APIs);

    }

    public void ExecuteAPIs()
    {
        foreach (var api in APIs.OrderBy(api => api.order))
        {
            Debug.Log("Executing API: " + api.api_call);
            if (APIControllers.ContainsKey(api.api_call))
            {
                APIControllers[api.api_call].ExecuteAPI(api.parameters); // ֱ�Ӵ��ݲ����б�
                Debug.Log($"Executed {api.api_call} with parameters {string.Join(", ", api.parameters.Select(p => $"{p.Key}: {p.Value}"))}");
            }
            else
            {
                Debug.Log($"Controller for {api.api_call} not found.");
            }
        }
    }


    private void PrintAPIs(List<API> apis)
    {
        foreach (var api in apis)
        {
            Debug.Log($"Task Name: {api.task_name}, API Call: {api.api_call}, Order: {api.order}");
            foreach (var param in api.parameters)
            {
                Debug.Log($"Parameter: {param.Key}, Value: {param.Value}");
            }
        }
    }

}

[System.Serializable]
public class API
{
    public string task_name { get; set; }
    public string api_call { get; set; }
    public Dictionary<string, string> parameters { get; set; }
    public int order { get; set; }
}

public class ResourceAPIContainer
{
    [JsonProperty("task_sequence")]
    public List<API> ResourceAPIs { get; set; }
}
