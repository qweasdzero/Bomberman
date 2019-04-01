using System;
using GameFramework.Resource;
using UnityEngine;
using UIBinding;

namespace SG1
{
    public class UGUIItemCollectionBinding : ItemCollectionBinding
    {
        public string TemplatePath;

        private LoadAssetCallbacks m_LoadAssetCallbacks = null;
        
        protected override void Bind()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback,LoadAssetFailureCallback);
            
            base.Bind();
        }

        private void LoadAssetFailureCallback(string assetname, string dependencyassetname, int loadedcount, int totalcount, object userdata)
        {
            Debug.LogError("Template is Null");
        }

        private void LoadAssetSuccessCallback(string assetname, object asset, float duration, object userdata)
        {
            GameObject asert = (GameObject) asset;
            ItemContextArgs args = (ItemContextArgs) userdata;
            
            var itemObject = Instantiate(asert, transform);
            itemObject.transform.localScale = Vector3.one;
            itemObject.transform.localPosition = Vector2.one * 50000;
            itemObject.transform.SetSiblingIndex(args.Position);
            itemObject.name = asert.name + "_" + args.Position;
            var modelView = itemObject.GetComponent<ItemModelView>();
            if (modelView == null)
            {
                modelView = itemObject.AddComponent<ItemModelView>();
            }
            modelView.SetContext(args.Item, args.Position);
        }
        
        private class ItemContextArgs
        {
            public int Position;
            public ItemContext Item;
        }

        public override void OnItemInsert(int position, ItemContext item)
        {
            if (!string.IsNullOrEmpty(TemplatePath))
            {
                GameEntry.Resource.LoadAsset(AssetUtility.GetUIItemAsset(TemplatePath), m_LoadAssetCallbacks,
                    new ItemContextArgs() {Item = item, Position = position});
            }
        }


        public override void OnItemRemove(int position)
        {
            var item = transform.GetChild(position);
            if (item != null)
            {
                DestroyImmediate(item.gameObject);
            }
        }

        public override void OnItemsClear()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}