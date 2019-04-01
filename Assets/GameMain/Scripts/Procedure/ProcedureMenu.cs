using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SG1
{
    public class ProcedureMenu : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        private bool IsStartGame;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            IsStartGame = false;
            GameEntry.UI.OpenUIForm(UIFormId.MenuPage, this);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (IsStartGame)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Main"));
                GameEntry.UI.CloseAllLoadedUIForms();
                GameEntry.UI.CloseAllLoadingUIForms();
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        public void StartGame()
        {
            IsStartGame = true;
        }
    }
}