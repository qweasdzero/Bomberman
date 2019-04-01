using System;
using System.Collections.Generic;
using System.Net;
using GameFramework;
using GameFramework.Photon;
using UnityEngine;
using ExitGames.Client.Photon;


namespace UnityGameFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Photon")]
    public class PhotonComponent : GameFrameworkComponent
    {
        private IPhotonManager m_PhotonManager = null;
        
        private EventComponent m_EventComponent = null;

//        public String m_ApplicationName = "Default";
//
//        public string m_ServerIPAddress = "127.0.0.1";
//
//        public int m_Port = 5055;

        /// <summary>
        /// 获取Photon频道数量。
        /// </summary>
        public int PhotonChannelCount
        {
            get
            {
                return m_PhotonManager.PhotonChannelCount;
            }
        }
        
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {   
            base.Awake();

            m_PhotonManager = GameFrameworkEntry.GetModule<IPhotonManager>();
            if (m_PhotonManager == null)
            {
                Log.Fatal("Photon manager is invalid.");
                return;
            }

            m_PhotonManager.PhotonDebugReturn += OnPhotonDebugReturn;
            m_PhotonManager.PhotonEvent += OnPhotonEvent;
            m_PhotonManager.PhotonOperationResponse += OnPhotonOperationResponse;
            m_PhotonManager.PhotonStatusChanged += OnPhotonStatusChanged;
        }
        
        private void Start()
        {
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }
        }
        
        private void OnPhotonDebugReturn(object sender, GameFramework.Photon.PhotonDebugReturnEventArgs e)
        {
            m_EventComponent.Fire(this, ReferencePool.Acquire<PhotonDebugReturnEventArgs>().Fill(e));
        }

        private void OnPhotonEvent(object sender, GameFramework.Photon.PhotonEventEventArgs e)
        {
            m_EventComponent.Fire(this, ReferencePool.Acquire<PhotonEventEventArgs>().Fill(e));
        }

        private void OnPhotonOperationResponse(object sender, GameFramework.Photon.PhotonOperationResponseEventArgs e)
        {
            m_EventComponent.Fire(this, ReferencePool.Acquire<PhotonOperationResponseEventArgs>().Fill(e));
        }

        private void OnPhotonStatusChanged(object sender, GameFramework.Photon.PhotonStatusChangedEventArgs e)
        {
            // 这里为连接成功和断开连接做一个事件发送,为了处理在连接成功或者失败之后不用在外部判断当前的状态
            switch (e.StatusCode)
            {
                case StatusCode.Connect:
                    m_EventComponent.Fire(this, ReferencePool.Acquire<PhotonConnectedEventArgs>().Fill(e));
                    break;
                case StatusCode.Disconnect:
                    m_EventComponent.Fire(this, ReferencePool.Acquire<PhotonDisconnectedEventArgs>().Fill(e));
                    break;
            }
            m_EventComponent.Fire(this, ReferencePool.Acquire<PhotonStatusChangedEventArgs>().Fill(e));
        }

        /// <summary>
        /// 检查是否存在Photon频道。
        /// </summary>
        /// <param name="applicationName">Photon频道名称。</param>
        /// <returns>是否存在Photon频道。</returns>
        public bool HasPhotonChannel(string applicationName)
        {
            return m_PhotonManager.HasPhotonChannel(applicationName);
        }
        
        /// <summary>
        /// 获取Photon频道。
        /// </summary>
        /// <param name="applicationName">Photon频道名称。</param>
        /// <returns>要获取的Photon频道。</returns>
        public IPhotonChannel GetPhotonChannel(string applicationName)
        {
            return m_PhotonManager.GetPhotonChannel(applicationName);
        }

        /// <summary>
        /// 获取所有Photon频道。
        /// </summary>
        /// <returns>所有Photon频道。</returns>
        public IPhotonChannel[] GetAllPhotonChannels()
        {
            return m_PhotonManager.GetAllPhotonChannels();
        }

        /// <summary>
        /// 获取所有Photon频道。
        /// </summary>
        /// <param name="results">所有Photon频道。</param>
        public void GetAllPhotonChannels(List<IPhotonChannel> results)
        {
            m_PhotonManager.GetAllPhotonChannels(results);
        }

        /// <summary>
        /// 创建Photon频道。
        /// </summary>
        /// <param name="applicationName">Photon频道名称。</param>
        /// <returns>要创建的Photon频道。</returns>
        public IPhotonChannel CreatePhotonChannel(string applicationName)
        {
            return m_PhotonManager.CreatePhotonChannel(applicationName);
        }

        /// <summary>
        /// 销毁Photon频道。
        /// </summary>
        /// <param name="applicationName">Photon频道名称。</param>
        /// <returns>是否销毁Photon频道成功。</returns>
        public bool DestroyPhotonChannel(string applicationName)
        {
            return m_PhotonManager.DestroyPhotonChannel(applicationName);
        }
    }
}

