using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameFramework.Resource;
using GameFramework.Sound;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace SG1
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/XLua")]
    public sealed class XLuaComponent : GameFrameworkComponent
    {
        private LoadAssetCallbacks m_LoadLuaFileCallbacks;
        
        private IResourceManager m_ResourceManager;

        private LuaEnv m_LuaEvn;
        
        private LuaTable m_ScriptTable;

        private Dictionary<string, LuaFileInfo> m_CacheLuaDict;
        
        public float m_GCInterval = 1; // 1 秒

        public float m_LastGCTime = 0;
        
        protected override void Awake()
        {
            m_LoadLuaFileCallbacks = new LoadAssetCallbacks(LoadLuaSuccessCallback, LoadLuaFailureCallback);
            
            m_CacheLuaDict = new Dictionary<string, LuaFileInfo>();

            // 设置Lua环境
            m_LuaEvn = new LuaEnv();
            m_ScriptTable = m_LuaEvn.NewTable();
            LuaTable meta = m_LuaEvn.NewTable();
            meta.Set("__index",m_LuaEvn.Global);
            m_ScriptTable.SetMetaTable(meta);
            meta.Dispose();
            
            // 设置Lua self
            m_ScriptTable.Set("self", this);
            
            // 设置自定义Loader
            m_LuaEvn.AddLoader(CustomLoader);
            
            base.Awake();
        }

        private void LoadLuaSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            LuaFileInfo luaFileInfo = (LuaFileInfo) userData;

            if (luaFileInfo == null)
                throw new GameFrameworkException("Load lua file info is invalid.");
            
            TextAsset textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                Log.Warning("lua asset '{0}' is invalid.", assetName);
                return;
            }
            
            luaFileInfo.Bytes = textAsset.bytes;

            if (!m_CacheLuaDict.ContainsKey(luaFileInfo.LuaName))
            {
                m_CacheLuaDict.Add(luaFileInfo.LuaName, luaFileInfo);
//                Log.Info("Load lua file '{0}' OK.", luaFileInfo.LuaName);
            }
            else
            {
                m_CacheLuaDict[luaFileInfo.LuaName] = luaFileInfo;
//                Log.Warning("Already exist lua file '{0}'.", luaFileInfo.LuaName);
            }

            if (luaFileInfo.IsIndependent)
            {
                m_LuaEvn.DoString(luaFileInfo.Bytes, luaFileInfo.LuaName, m_ScriptTable);
            }
        }
        
        private void LoadLuaFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            LuaFileInfo luaFileInfo = (LuaFileInfo) userData;

            if (luaFileInfo == null)
                throw new GameFrameworkException("Load lua file info is invalid.");
            string str = Utility.Text.Format("Load lua file failure, asset name '{0}', status '{1}', error message '{2}'.", (object) assetName, (object) status.ToString(), (object) errorMessage);
            //TODO:处理载入错误
//            if (this.m_LoadDataTableFailureEventHandler == null)
//                throw new GameFrameworkException(str);
        }

        private void Start()
        {
            BaseComponent baseComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                this.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                this.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }
            
            // 这个初始化更新文件
            LoadLua(Constant.XLua.HotfixFileName, LoadType.Text,true);
        }

        public void LoadLua(string luaName, LoadType loadType,bool isIndependent)
        {
            LuaFileInfo luaFileInfo = new LuaFileInfo(luaName, loadType,isIndependent);
            m_ResourceManager.LoadAsset(luaFileInfo.AssetName, Constant.AssetPriority.LuaAsset,
                this.m_LoadLuaFileCallbacks, luaFileInfo);
        }

        private void Update()
        {
            if (Time.time - m_LastGCTime > m_GCInterval)
            {
                m_LuaEvn.Tick();
                m_LastGCTime = Time.time;
            }
        }

        private void OnDestroy()
        {
            m_LuaEvn.Dispose();
            m_CacheLuaDict.Clear();
            m_ScriptTable = null;
            m_LuaEvn = null;
        }

        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
                throw new GameFrameworkException("Resource manager is invalid.");
            m_ResourceManager = resourceManager;
        }
        
        private byte[] CustomLoader(ref string path)
        {
            if (m_CacheLuaDict.TryGetValue(path,out LuaFileInfo luaFileInfo))
            {
                return luaFileInfo.Bytes;
            }
            else
            {
                return (byte[]) null;
            }
        }
    }
    
    public sealed class LuaFileInfo
    {
        public LuaFileInfo(string luaName,LoadType loadType,bool isIndependent)
        {
            LuaName = luaName;
            AssetName = AssetUtility.GetLuaAsset(luaName, loadType);
            LoadType = loadType;
            IsIndependent = isIndependent;
            Bytes = null;
        }

        public LuaFileInfo(string luaName,LoadType loadType,bool isIndependent,params string[] dependencyLuaName):this(luaName,loadType,isIndependent)
        {
            DependencyLuaName = dependencyLuaName;
        }
        
        /// <summary>
        /// Lua文件名
        /// </summary>
        public string LuaName
        {
            get;
            private set;
        }

        /// <summary>
        /// Lua文件路径
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 加载格式
        /// </summary>
        public LoadType LoadType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Lua内容
        /// </summary>
        public byte[] Bytes
        {
            get;
            set;
        }

        public string[] DependencyLuaName
        {
            get;
            private set;
        }

        public bool IsIndependent
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取Lua文件的字符串
        /// </summary>
        public override string ToString()
        {
            return Bytes != null ? System.Text.Encoding.UTF8.GetString(Bytes) : string.Empty;
        }
    }
}

