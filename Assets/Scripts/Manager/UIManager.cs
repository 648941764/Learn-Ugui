using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : MonoSingleton<UIManager>
{
    public const string DATA_PATH = "Assets/UIPanel/{0}.prefab";
    public readonly Vector3 PANEL_PATH = new Vector3(0f, -10000f, 0f);

    private Dictionary<Type, BaseUI> panels  = new Dictionary<Type, BaseUI>();

    private Transform panelParents;

    protected override void Awake()
    {
        base.Awake();
        panelParents = new GameObject("PanelParent").transform;
        panelParents.position = PANEL_PATH;

        DontDestroyOnLoad(panelParents);
    }

    public async Task<T> LoadAsset<T>(string name) where T : BaseUI
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Assets/UIPanel/{name}.prefab");
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject asset = handle.Result;
            T assetComponet = asset.GetComponent<T>();
            return assetComponet;       
        }
        else
        {
            Debug.LogError("<color=yellow>Failed to load asset: </color>" + handle.OperationException);
            return null;
        }
    }

    public async Task<T> CreatePanel<T>() where T : BaseUI
    {
        T LoadedAsset = await LoadAsset<T>(typeof(T).Name);
        T panel = Instantiate(LoadedAsset, Vector3.zero, Quaternion.identity);
        panel.transform.SetParent(panelParents);
        panels.Add(typeof(T), panel);
        Debug.Log(panels[typeof(T)]);
        return panel;
    }

    public  async Task OpenPanel<T>() where T : BaseUI
    {
        bool needCreate = !panels.ContainsKey(typeof(T));
        T panel = needCreate ? await CreatePanel<T>() : (T)panels[typeof(T)];
        panel.Enable();
    }

    public void ClosePanel<T>() where T : BaseUI
    {
        if (!panels.ContainsKey(typeof(T))) 
        {
            Debug.Log("<color=red>对象没有被创建</color>");
        }
        else
        {
            panels[typeof(T)].Disable();
            Debug.Log("关闭页面");
        }
    }

    public void DestroyPaenl<T>() where T : BaseUI
    {
        if (panels.ContainsKey(typeof(T)))
        {
            panels[typeof(T)].Destroy();
            panels.Remove(typeof(T));
        }
        else
        {
            Debug.Log("<color=red>对象没有被创建</color>");
        }
    }
}
