using System.Collections.Generic;
using UnityEngine;

public class APIControllerBase : MonoBehaviour
{
    // 在这里定义设备的通用控制函数
    public virtual void ExecuteAPI(Dictionary<string, string> parameters)
    {
        Debug.Log("API executed");
    }
}