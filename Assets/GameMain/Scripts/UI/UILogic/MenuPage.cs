
using System.Net;
using Common;
using ExitGames.Client.Photon;
using GameFramework;
using GameFramework.Event;
using GameFramework.Photon;
using UnityGameFramework.Runtime;
using UIBinding;
using PhotonOperationResponseEventArgs = UnityGameFramework.Runtime.PhotonOperationResponseEventArgs;


namespace SG1
{
   public class MenuPage : UGuiForm
   {
      public ProcedureMenu m_menu;
      protected override void OnOpen(object userData)
      {
         base.OnOpen(userData);
         m_menu=(ProcedureMenu)userData;
      }

      public void OnStartClick()
      {
         m_menu.StartGame();
      }
   }
}