using System.Collections;
using System.Collections.Generic;
using static PythonScriptManager;
using UnityEngine;

public class SweeperCtrl : APIControllerBase
{
    private void Awake()
    {
        ServiceBotCtrl.Instance.RegisterAPIController("SweeperCtrl()", this);
        Debug.Log("SweeperCtrl API reported");
    }

    public void ExecuteAPIs(Dictionary<string, string> parameters)
    {
        PythonScriptManager.SetParameters(parameters);
        PythonScriptManager.CallPythonScriptwithPara("SweeperCtrl.py");
        Debug.Log("SweeperCtrl Running");
    }
}
