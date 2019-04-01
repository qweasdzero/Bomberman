using UIBinding;

namespace SG1
{
    public class ItemModelView : ModelView
    {
        [InspectorReadOnly] public BaseBinding[] CachedBindings = new BaseBinding[0];

        public void SetContext(ItemContext c, int index)
        {
            Context = c;
            CachePathPrefix = index.ToString();
            if (CachedBindings.Length == 0) CachedBindings = gameObject.GetComponentsInChildren<BaseBinding>();

            foreach (var t in CachedBindings)
            {
                t.ModelView = this;
                t.OnContextChange();
            }
        }


        [InspectorButton(InspectorDiplayMode.DisabledInPlayMode)]
        private void CacheBinding()
        {
            CachedBindings = gameObject.GetComponentsInChildren<BaseBinding>(true);
        }


        [InspectorButton(InspectorDiplayMode.DisabledInPlayMode)]
        private void DisableBinding()
        {
            Toggle(false);
        }

        [InspectorButton(InspectorDiplayMode.DisabledInPlayMode)]
        private void EnableBinding()
        {
            Toggle(true);
        }

        private void Toggle(bool value)
        {
            var bindings = gameObject.GetComponentsInChildren<BaseBinding>(true);
            foreach (var binding in bindings) binding.enabled = value;
        }
    }
}