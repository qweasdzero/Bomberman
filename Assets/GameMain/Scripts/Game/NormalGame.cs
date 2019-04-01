using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using SG1;
using UnityEngine;
using UnityGameFramework.Runtime;
using Debug = System.Diagnostics.Debug;

namespace SG1
{
    public class NormalGame : GameBase
    {       
        private static int xitm=6;
        private static int yitm=5;

        public Player m_player;
        
        public override GameMode GameMode
        {
            get
            {
                return GameMode.NormalGame;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(),3)
            {
                Position = new Vector2(-xitm-1,yitm+1)
            });

            Boundary();
            IronWallController();
            WoodWallController();
            for (int i = 0; i <Random.Range(2, 6) ; i++)
            {
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),6)
                {
                    Position = new Vector2(xitm+1-2*i,-yitm-1+2*i)
                });
            }
   
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            if (m_player&&m_player.m_data.HP <= 0)
            {
                GameOver=true;
            }
        }

        private void IronWallController()//生成中间阻挡物
        {
            for (int x = 0; x <= xitm; x+=2)
            {
                for (int y = 1; y <= yitm; y += 2)
                {
                    GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                    {
                        Position = new Vector2(x,y)
                    });
                    GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                    {
                        Position = new Vector2(x,-y)
                    });
                    if (x != 0)
                    {
                        GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                        {
                            Position = new Vector2(-x,-y)
                        });
                        GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                        {
                            Position = new Vector2(-x,y)
                        });
                    }
                }
            }
        }
        private void Boundary()
        {
            for (int i = 0; i < xitm+2; i++)
            {
                GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                {
                    Position = new Vector2(xitm+2,i)
                });                  
                GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                {
                    Position = new Vector2(-(xitm+2),i)
                });

                if (i != 0)
                {
                    GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                    {
                        Position = new Vector2(xitm+2,-i)
                    });

                    GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                    {
                        Position = new Vector2(-(xitm+2),-i)
                    });   
                }
            
            }
            for (int i = 0; i <= yitm+2; i++)
            {
                GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                {
                    Position = new Vector2(i,yitm+2)
                });
                GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                {
                    Position = new Vector2(i,-(yitm+2))
                });
                if(i!=0)
                {
                    GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                    {
                        Position = new Vector2(-i,yitm+2)
                    });  
                    GameEntry.Entity.ShowIronWall(new WallData(GameEntry.Entity.GenerateSerialId(),2)
                    {
                        Position = new Vector2(-i,-(yitm+2))
                    });  
                }
             
            }
        }//生成边界
        
        private void WoodWallController()//生成中间阻挡物
        {
            for (int x = 0; x <= xitm+1; x+=2)
            {
                for (int y = 0; y <= yitm+1; y += 2)
                {
                    ShowWoodWall(new Vector2(x, y));
                    ShowWoodWall(new Vector2(x, -y));
                    if (x != 0) {
                        ShowWoodWall(new Vector2(-x, y));
                        ShowWoodWall(new Vector2(-x, -y));   
                    }
                }
            }
            for (int x = 1; x <= xitm+1; x+=2)
            {
                for (int y = 1; y <= yitm+1; y += 2)
                {
                    ShowWoodWall(new Vector2(x, y));
                    ShowWoodWall(new Vector2(x, -y));
                    ShowWoodWall(new Vector2(-x, y));
                    ShowWoodWall(new Vector2(-x, -y));                
                }
            }
        }

        private void ShowWoodWall(Vector2 vector)
        {
            if (Random.Range(0, 4) == 1)
            {
                GameEntry.Entity.ShowWoodWall(new WallData(GameEntry.Entity.GenerateSerialId(),1)
                {
                    Position = vector
                });
            }
        }

        protected override void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Player))
            {
                m_player = (Player)ne.Entity.Logic;
            }
        }
    }


}
