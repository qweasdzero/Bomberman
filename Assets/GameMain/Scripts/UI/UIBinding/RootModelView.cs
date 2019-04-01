using System.Collections.Generic;
using UnityEngine;
using UIBinding;

namespace SG1
{
    [DefaultExecutionOrder(int.MinValue)]
    public class RootModelView : ModelView
    {
        public UGuiForm DefaultContext;

        private void Awake()
        {
            if (DefaultContext != null) SetContext(DefaultContext);
        }

        public void SetContext(IContext context, bool rewrite = false)
        {
            if (rewrite)
            {
                Context = context;
            }
            else
            {
                if (Context == null) Context = context;
            }
        }


        [InspectorButton(InspectorDiplayMode.DisabledInPlayMode)]
        public void SetValueInEditor()
        {
            var bindings = GetBindings(gameObject);

            foreach (var t in bindings)
            {
                t.ModelView = t.enabled ? this : null;
            }
        }

        private static IEnumerable<BaseBinding> GetBindings(GameObject go)
        {
            var bindings = new List<BaseBinding>();
            bindings.AddRange(go.GetComponents<BaseBinding>());

            for (var i = 0; i < go.transform.childCount; i++)
            {
                var modelView = go.transform.GetChild(i).GetComponent<ModelView>();
                if (modelView == null) bindings.AddRange(GetBindings(go.transform.GetChild(i).gameObject));
            }

            return bindings;
        }


        private void OnValidate()
        {
            SetValueInEditor();
        }

        private void Reset()
        {
            OnValidate();
        }
    }
}