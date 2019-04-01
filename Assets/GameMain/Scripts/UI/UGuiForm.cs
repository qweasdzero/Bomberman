//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UIBinding;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SG1
{
    public abstract class UGuiForm : UIFormLogic,UIBinding.IContext
    {
        public const int DepthFactor = 10;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;

        private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();
        
         public void SetPropertyValue(string propertyName, object value)
        {
            if (_properties.ContainsKey(propertyName))
            {
                switch (value)
                {
                    case string s:
                        (_properties[propertyName] as Property<string>)?.SetValue(s);
                        break;
                    case int i:
                        (_properties[propertyName] as Property<int>)?.SetValue(i);
                        break;
                    case float f:
                        (_properties[propertyName] as Property<float>)?.SetValue(f);
                        break;
                    case double d:
                        (_properties[propertyName] as Property<double>)?.SetValue(d);
                        break;
                    case object o:
                        (_properties[propertyName] as Property<object>)?.SetValue(o);
                        break;
                }
            }
            else
            {
                Debug.LogError(propertyName + " not exist ");
            }
        }

        public object FindProperty(string propertyName)
        {
            if (!_properties.ContainsKey(propertyName))
            {
                var fieldInfo = GetType().GetField($"_private{propertyName}Property",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fieldInfo != null)
                {
                    _properties.Add(propertyName, fieldInfo.GetValue(this) as Property);
                }
                else
                {
                    var propertyInfo = GetType().GetProperty($"{propertyName}Property",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo != null)
                    {
                        _properties.Add(propertyName, propertyInfo.GetValue(this, null) as Property);
                    }
                    else
                    {
                        _properties.Add(propertyName, null);
                    }
                }
            }

            return _properties[propertyName];
        }

        public void AddPropetyRuntime(string propertyName, Type type)
        {
            if (_properties.ContainsKey(propertyName))
            {
                Debug.LogError(propertyName + " already exist");
                return;
            }

            if (type == typeof(string))
            {
                _properties.Add(propertyName, new Property<string>());
            }
            else if (type == typeof(int))
            {
                _properties.Add(propertyName, new IntProperty());
            }
            else if (type == typeof(float))
            {
                _properties.Add(propertyName, new FloatProperty());
            }
            else if (type == typeof(double))
            {
                _properties.Add(propertyName, new DoubleProperty());
            }
            else if (type == typeof(bool))
            {
                _properties.Add(propertyName, new BoolProperty());
            }
            else if (type == typeof(Vector3))
            {
                _properties.Add(propertyName, new Vector3Property());
            }
            else if (type == typeof(Quaternion))
            {
                _properties.Add(propertyName, new QuaternionProperty());
            }
            else
            {
                _properties.Add(propertyName, new Property<object>());
            }
        }

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public virtual bool FadeToAlpha
        {
            get
            {
                return true;
            }
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;

            GameObject go = new GameObject();
            go.AddComponent<Text>().font = mainFont;
            Destroy(go);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition3D = Vector3.zero;
            transform.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            Text[] texts = GetComponentsInChildren<Text>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].font = s_MainFont;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            CanvasGroupFadeToAlpha();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            base.OnClose(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnPause()
#else
        protected internal override void OnPause()
#endif
        {
            base.OnPause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume()
#else
        protected internal override void OnResume()
#endif
        {
            base.OnResume();

            CanvasGroupFadeToAlpha();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover()
#else
        protected internal override void OnCover()
#endif
        {
            base.OnCover();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal()
#else
        protected internal override void OnReveal()
#endif
        {
            base.OnReveal();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
        {
            base.OnRefocus(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            Canvas[] canvases = GetComponentsInChildren<Canvas>(true);
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].sortingOrder += deltaDepth;
            }
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this);
        }

        private void CanvasGroupFadeToAlpha()
        {
            m_CanvasGroup.alpha = FadeToAlpha ? 0f : 1f;
            if (FadeToAlpha)
            {
                StopAllCoroutines();
                StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
            }
        }
    }
}
