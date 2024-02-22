using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;

public class AddressablesTest : MonoBehaviour
{

    [SerializeField] private List<AssetReference> _assetReferences = new List<AssetReference>();
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private Button _btnOpenPanel;
    [SerializeField] private Slider _silder;
    [SerializeField] private Text _silderText;
    [SerializeField] private GameObject _loadScreen;

    private void Awake()
    {
        InitButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadAsset<TestPanel>();
        }
    }

    private void InitButtons()
    {
        for (int i = 0;  i < _buttons.Count; i++)
        {
            int seceneIndex = i;
            _buttons[i].onClick.AddListener(() => LoadSceneAsync(seceneIndex));
        }

        _btnOpenPanel.onClick.AddListener(OnBtnOpenPanelClicked);
    }

    private async void LoadSceneAsync(int index)
    {
        _loadScreen.SetActive(true);
        await Task.Delay(TimeSpan.FromSeconds(2));
        AsyncOperationHandle<SceneInstance> sceneHandle = Addressables.LoadSceneAsync(_assetReferences[index]/*.RuntimeKey.ToString()*/, LoadSceneMode.Additive);

        while (!sceneHandle.IsDone)
        {
            float progress = sceneHandle.PercentComplete;
            UpdateSilder(progress);
            await Task.Yield();
        }

        sceneHandle.Completed += OnSceneLoaded;
        await sceneHandle.Task;
    }

    void UpdateSilder(float progress)
    {
        _silder.value = progress;
        Debug.Log(_silder.value);
        _silderText.text = $"{progress * 100}";
    }

    private void LoadAsset<T>() where T : BaseUI
    {
        AsyncOperationHandle<GameObject> objHandle = Addressables.LoadAssetAsync<GameObject>("Assets/UIPanel/TestPanel.prefab");
        objHandle.Completed += OnAssetLoaded;
    }

    private void OnAssetLoaded<T>(AsyncOperationHandle<T> objHandle)
    {
        if (objHandle.Status == AsyncOperationStatus.Succeeded)
        {
            T asset = objHandle.Result;
            Debug.Log($"<color=#CCFF00>资源加载成功</color>");
            //Instantiate(asset, GameObject.Find("Canvas").transform);
        }

        else
        {
            Debug.Log($"<color=#FF0000>资源加载失败</color>");
        }
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        _loadScreen.SetActive(false);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"<color=green>场景加载成功</color>");
        }
    }

    //测试专用

    public async void  OnBtnOpenPanelClicked()
    {
       await UIManager.Instance.OpenPanel<TestPanel>();
    }
}
