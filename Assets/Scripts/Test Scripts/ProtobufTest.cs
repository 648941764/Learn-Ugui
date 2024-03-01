using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using TestGoogleBuffer;
using System.IO;
using System.Threading.Tasks;

public class ProtobufTest : MonoBehaviour
{
     private void Start()
    {
        //Person person = new Person()
        //{
        //    Age = 12,
        //    Name = "Tom"
        //};

        //byte[] serializeData = Serialize(person);
        //Debug.Log(serializeData.Length);

        //Person deserializedPerson = Deserialize<Person>(serializeData);
        //Debug.Log("Deserialized Person: " + deserializedPerson.Name + ", " + deserializedPerson.Age);

        LoadAssetBundle();
    }

    private void LoadAssetBundle()
    {
        AssetBundleCreateRequest assetBundle = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui.unity2d");//异步加载AssetBundle资源包
        assetBundle.completed += AssetBundleLoaded; 
    }

    private void AssetBundleLoaded(AsyncOperation handle)
    {
        AssetBundleCreateRequest request = (AssetBundleCreateRequest) handle;
        AssetBundle bundle = request.assetBundle;
        if (bundle != null)
        {
            Debug.Log("<color=blue>资源加载成功</color>");
            AssetBundleRequest obj = bundle.LoadAssetAsync<GameObject>("CountPanel");//异步加载Bundle包里面的资源
            //bundle.LoadAssetWithSubAssets("CountPanel");加载资源和他的子资源（sprite切割后的图片为例子，用于方法了解，此代码不会生效）
            GameObject.Instantiate(obj.asset as GameObject, transform);
        }

    }

    public static byte[] Serialize(IMessage message)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            message.WriteTo(stream);
            return stream.ToArray();
        }
    }

    public static T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
    {
        T message = new T();
        using (MemoryStream stream = new MemoryStream(data))
        {
            message.MergeFrom(stream);
        }
        return message;
    }
}

