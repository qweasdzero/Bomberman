using UnityEngine;

namespace SG1
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 内置信息组件
        /// </summary>
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }
        
        /// <summary>
        /// XLua热更新组件
        /// </summary>
        public static XLuaComponent XLua
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            XLua = UnityGameFramework.Runtime.GameEntry.GetComponent<XLuaComponent>();
        }
    }
}
