using System.Collections.Generic;
using UIBinding;
using UnityEngine;

namespace SG1
{
   public class FightPage : UGuiForm
   {
      private Property<int> _privateHPProperty = new Property<int>();

      public int HP
      {
         get { return _privateHPProperty.GetValue(); }
         set { _privateHPProperty.SetValue(value); }
      }


      public ProcedureMain m_main;

      protected override void OnInit(object userData)
      {
         base.OnInit(userData);
      }

      protected override void OnOpen(object userData)
      {
         base.OnOpen(userData);
         m_main=(ProcedureMain)userData;
      }

      protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
      {
         base.OnUpdate(elapseSeconds, realElapseSeconds);
      }

      public void ReStart()
      {
         m_main.isrestart = true;
      }

      public void NewMonster()
      {
         GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),6)
         {
            Position = new Vector2(5-2*Random.Range(-1,5),-4+2*Random.Range(-1,6))
         });
      }
   }
}