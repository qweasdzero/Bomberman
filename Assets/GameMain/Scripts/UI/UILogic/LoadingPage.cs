using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UIBinding;
using UnityGameFramework.Runtime;

namespace SG1
{
    public class LoadingPage : UGuiForm
    {
        public override bool FadeToAlpha
        {
            get
            {
                return false;
            }
        }
        
        private Property<float> _privateProgressProperty = new Property<float>();

        private static readonly int c_Count = ProcedurePreload.DataTableNames.Length;
        public float Progress
        {
            get { return _privateProgressProperty.GetValue(); }
            set { _privateProgressProperty.SetValue(value); }
        }

        private Property<string> _privateInfoProperty = new Property<string>();

        public string Info
        {
            get { return _privateInfoProperty.GetValue(); }
            set { _privateInfoProperty.SetValue(value); }
        }
        
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            Progress = 0.0f;
        }

        public void OnLoadDataTable(string name)
        {
            Info = name;
            Progress = ((float) GameEntry.DataTable.Count) / c_Count;
        }
    }
}

