using ModelReplacement;
using UnityEngine;

namespace LethalWicker
{
    public class MRLETHALWICKERRED : BodyReplacementBase
    {
        protected override GameObject LoadAssetsAndReturnModel()
        { 
            string model_name = "LethalWickerRed";
            return Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);
        }
    }
    public class MRLETHALWICKERGREEN : BodyReplacementBase
    {
        protected override GameObject LoadAssetsAndReturnModel()
        { 
            string model_name = "LethalWickerGreen";
            return Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);
        }
    }
    public class MRLETHALWICKERBLUE : BodyReplacementBase
    {
        protected override GameObject LoadAssetsAndReturnModel()
        { 
            string model_name = "LethalWickerBlue";
            return Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);
        }
    }
}