using System;
using System.Diagnostics;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SG1
{
    public class ProcedureMain : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public NormalGame m_game;
        public bool isrestart=false;
        public FightPage m_fightPage;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.OpenUIForm(UIFormId.FightPage, this);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId,OnShowUISuccess);
//            GameEntry.UI.OpenUIForm(UIFormId.MainPage, this);
            m_game=new NormalGame();
            m_game.Initialize();
            isrestart = false;
           
        }

        private void OnShowUISuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            m_fightPage = (FightPage) ne.UIForm.Logic;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            m_game.Update(elapseSeconds,realElapseSeconds);
            try
            {
                m_fightPage.HP = m_game.m_player.Data.HP;
            }
            catch 
            {
                
            }
            if (m_game.GameOver||isrestart)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId,OnShowUISuccess);
            m_game.Shutdown();
        }
    }
}