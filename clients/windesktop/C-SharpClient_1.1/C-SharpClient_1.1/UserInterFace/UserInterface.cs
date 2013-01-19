using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace C_SharpClient_1._1
{
    class UserInterface
    {
        private GameSession gameSession;

        public GameSession.TextBox console;
        public GameSession.TextBox txtBoxinformation;

        private bool showStat = false;
        public bool showBuildMenu = false;

        private List<GameObject> listToPrint;
        private SpriteFont spriteFont;

        //private SpriteFont menuFront;
        private Texture2D menuBar;

        private Tower fireTower;
        private Rectangle recFireTower;

        private Tower iceTower;
        private Rectangle recIceTower;

        private Tower magicTower;
        private Rectangle recMagicTower;

        private Tower canonTower;
        private Rectangle recCanonTower;

        private Tower superTower;
        private Rectangle recSuperTower;

        private Game game;
        private Texture2D txtGold;

        private Texture2D txtPlayer;
        private Rectangle recPlayer;
        private bool showPlayer = true;

        private Texture2D txtBuildButton;
        private bool isMouseVisable;
        public bool IsMouseVisable { get { return isMouseVisable; } }

        public UserInterface(Game game, Tower superTower,Texture2D menuBar, Texture2D buildTowerButton, Texture2D txtGold, Texture2D txtPlayer, GameSession gameSession)
        {
            this.txtPlayer = txtPlayer;
            this.recPlayer = new Rectangle(730, 50, 50, 50);
            this.txtBuildButton = buildTowerButton;
            this.gameSession = gameSession;
            this.superTower = superTower;
            this.canonTower = gameSession.canonTower;
            this.recCanonTower = new Rectangle(300, 510, 50, 50);
            this.txtGold = txtGold;
            this.game = game;
            this.menuBar = menuBar;
            this.fireTower = gameSession.fireTower;
            this.recSuperTower = new Rectangle(750, 550, 50, 50);
            this.recFireTower = new Rectangle(100, 510, 50,50);
            this.iceTower = gameSession.iceTower;
            this.recIceTower = new Rectangle(160, 510, 50,50);
            this.magicTower = gameSession.magicTower;
            this.recMagicTower = new Rectangle(230, 510, 50, 50);

            this.listToPrint = gameSession.listToPrint;
            this.spriteFont = gameSession.fontTimesNewSimon;
        }
        public void UpdateScroll(ref Map map)
        {
            if (map.Rec.Y > -360 && Keyboard.GetState().IsKeyDown(Keys.Down))
                map.Rec = new Rectangle(map.Rec.X, map.Rec.Y - 10, map.Rec.Width, map.Rec.Height);
            if (map.Rec.Y < 0 && Keyboard.GetState().IsKeyDown(Keys.Up))
                map.Rec = new Rectangle(map.Rec.X, map.Rec.Y + 10, map.Rec.Width, map.Rec.Height);
            if (map.Rec.X < 0 && Keyboard.GetState().IsKeyDown(Keys.Left))
                map.Rec = new Rectangle(map.Rec.X + 10, map.Rec.Y, map.Rec.Width, map.Rec.Height);
            if (map.Rec.X > -800 && Keyboard.GetState().IsKeyDown(Keys.Right))
                map.Rec = new Rectangle(map.Rec.X - 10, map.Rec.Y, map.Rec.Width, map.Rec.Height);
        }
        public void Update(GameTime gameTime, ref Tower myCurrentTower, ref GameSession gameSession, bool isMouseVisable)
        {
            this.isMouseVisable = isMouseVisable;
            if (console != null && txtBoxinformation != null)
            {
                console.Update(ref gameSession.listToPrint);
                txtBoxinformation.Update(ref gameSession.listToPrint);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameSession.mainMenu.GameStart = false;
                gameSession.sounds.PlayMultower();
                if (!gameSession.player.isSinglePlayer)
                     gameSession.CloseConnection();

            }
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
                showStat = !showStat;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(new Rectangle(30, 440, 10, 10)))
                showBuildMenu = true;
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                showPlayer = true;
                showBuildMenu = false;
                showRecrutMonsterButton = false;
                game.IsMouseVisible = true;
            }
            if (showBuildMenu && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recFireTower))
            {
                myCurrentTower = fireTower;
                myCurrentTower.typeOfTower = "FireTower";
                game.IsMouseVisible = false;
                showBuildMenu = false;
            }
            if (showBuildMenu && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recIceTower))
            {
                myCurrentTower = iceTower;
                myCurrentTower.typeOfTower = "IceTower";
                game.IsMouseVisible = false;
                showBuildMenu = false;
            }
            if (showBuildMenu && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recMagicTower))
            {
                myCurrentTower = magicTower;
                myCurrentTower.typeOfTower = "MagicTower";
                game.IsMouseVisible = false;
                showBuildMenu = false;
            }
            if (showBuildMenu && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recCanonTower))
            {
                myCurrentTower = canonTower;
                myCurrentTower.typeOfTower = "CanonTower";
                game.IsMouseVisible = false;
                showBuildMenu = false;
            }
            if (showStat && showBuildMenu && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recSuperTower))
            {
                myCurrentTower = superTower;
                myCurrentTower.typeOfTower = "SuperTower";
                game.IsMouseVisible = false;
                showBuildMenu = false;
            }
            if (game.IsMouseVisible && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 3, Mouse.GetState().Y + 5, 5, 3).Intersects(new Rectangle(10, 525, 50, 50)))
                showBuildMenu = true;

            if (!gameSession.player.isSinglePlayer)
                UpdateMultiPlayer(gameTime);
        }
        private int slowDown = 0;
        private void UpdateMultiPlayer(GameTime gameTime)
        {
            slowDown += gameTime.ElapsedGameTime.Milliseconds;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recRecrutMonsterButton))
            {
                showRecrutMonsterButton = true;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recPlayer))
            {
                showPlayer = false;
            }
            int counter = 0;
            foreach (Monster mo in gameSession.GetallMonsters())
            {
                counter++;
                if (slowDown > 100 && showRecrutMonsterButton && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 2, 2).Intersects(new Rectangle(310 + (70 * counter), 510, 50, 50)))
                {
                    slowDown = 0;
                    if (gameSession.player.gold >= mo.cost)
                    {
                        gameSession.player.gold -= mo.cost;
                        gameSession.sharedModule.UpdateMPIncomeForBuyingMonster(mo.typeID);
                        gameSession.informationBox.AddMessage("Monster sent with id: " + mo.typeID);
                        gameSession.sharedModule.RecruitMonster(Convert.ToInt16(mo.typeID), mo.maxHP);
                    }
                   
                }
            }
        }


        private Texture2D txtRecrutMonsterButton;
        private Rectangle recRecrutMonsterButton;

        private bool showRecrutMonsterButton = false;

        public void InitMonsterSpawner(Texture2D txtRecrutMonsterButton)
        {
            this.txtRecrutMonsterButton = txtRecrutMonsterButton;
            this.recRecrutMonsterButton = new Rectangle(740, 525, 50, 50);
        }
        public void Draw(SpriteBatch sp, GameTime gameTime)
        {
            if (showStat)
            {
                sp.Draw(superTower.Text2D, recSuperTower, Color.Green);
                if (gameTime.ElapsedGameTime.Milliseconds > 0)
                    sp.DrawString(spriteFont, "FPS: " + 1000 / gameTime.ElapsedGameTime.Milliseconds, new Vector2(100, 20), Color.Black);
                sp.DrawString(spriteFont, "X: " + Mouse.GetState().X + " Y: " + Mouse.GetState().Y, new Vector2(100, 0), Color.Black);
                sp.DrawString(spriteFont, "Ammount of gameObjects: " + gameSession.listToPrint.Count, new Vector2(100, 50), Color.Black);
                if (console != null)
                    console.Draw(sp, 0, 0);
                //sp.DrawString(spriteFont, "Time: " + gameTime.TotalGameTime + " Next mobwave: " + gameSession.mobWave.TimeToNextWave(gameTime), new Vector2(0, 450), Color.Black);
            }
            else
            {
                if (txtBoxinformation != null)
                    txtBoxinformation.Draw(sp, 0, 0);
                sp.Draw(menuBar, new Rectangle(10, 0, 600, 130), Color.White * 0.5f);
                if (gameSession.player.isSinglePlayer)
                {
                    sp.DrawString(spriteFont, "Ammount of Creeps: " + gameSession.AmmountOfMonsterOnField() + " Monsters Killed: " + gameSession.player.monsterkilled, new Vector2(80, 35), Color.Black);
                    sp.DrawString(spriteFont, "Income: " + +gameSession.sharedModule.SpIncome() +"         " + gameSession.player.gold, new Vector2(80, 55), Color.Black);
                    sp.DrawString(spriteFont, "Mobwaves : " + gameSession.mobWave + " Life left: " + gameSession.player.hp , new Vector2(80, 75), Color.Black);
                }
                else
                {
                    sp.DrawString(spriteFont, "Ammount of Creeps: " + gameSession.multiMobWave.monstersInThisWave + " Monsters Killed: " + gameSession.player.monsterkilled, new Vector2(80, 35), Color.Black);
                    sp.DrawString(spriteFont, "Mobwave: " + gameSession.multiMobWave.waveNumber + " Life left: " + gameSession.player.hp + " Income: " + gameSession.sharedModule.MpIncome(), new Vector2(80, 75), Color.Black);
                    sp.DrawString(spriteFont, "Next mobwave: " + gameSession.multiMobWave.TimeToNextWave(gameTime) + "    " + gameSession.player.gold, new Vector2(80, 55), Color.Black);
                }
                
                sp.Draw(txtGold, new Rectangle(265, 60, 14, 14), Color.White);

                //Menus for recruting monster
                if (!gameSession.player.isSinglePlayer)
                {
                    if (showPlayer)
                        sp.Draw(txtPlayer, recPlayer, Color.White);
                    else
                    {
                        sp.Draw(txtBoxinformation.Text2D, new Rectangle(570, 150, 200, 25 * gameSession.otherPlayer.Count), new Color(255, 255, 255, 0));
                        int counter  = 0;
                        foreach (Player p in gameSession.otherPlayer)
                        {
                            sp.DrawString(spriteFont, p.playerName + " HP: " + p.hp, new Vector2(580, 150 + (20 * counter)), Color.Black);
                            counter++;
                        }
                    }
                    if (showRecrutMonsterButton)
                    {
                        sp.Draw(menuBar, new Rectangle(350, 500, 410, 70), Color.White);
                        int counter = 0;
                        foreach (Monster mo in gameSession.GetallMonsters())
                        {
                            counter++;
                            Color c = Color.Black;
                            if (gameSession.player.gold < mo.cost)
                                c = Color.Red;
                            sp.Draw(mo.Text2D, new Rectangle(310 + (70 * counter), 510, 50, 50),new Rectangle(0, 100, 100,100), mo.color);
                            sp.DrawString(spriteFont, Convert.ToString(mo.cost), new Vector2(310 + (70 * counter), 510), c);
                        }
                    }
                    else
                    {
                        sp.Draw(this.txtRecrutMonsterButton, this.recRecrutMonsterButton, Color.White);
                    }
                }
                //For building Tower
                if (showBuildMenu)
                {
                    sp.Draw(menuBar, new Rectangle(50, 500, 350, 70), Color.White);
                    sp.Draw(fireTower.Text2D, recFireTower, Color.White);
                    sp.Draw(iceTower.Text2D, recIceTower, Color.White);
                    sp.Draw(magicTower.Text2D, recMagicTower, Color.White);
                    sp.Draw(canonTower.Text2D, recCanonTower, Color.White);
                    Color tmp;
                    if (gameSession.player.gold < magicTower.price)
                        tmp = Color.Red;
                    else
                        tmp = Color.Black;
                    sp.DrawString(spriteFont, Convert.ToString(magicTower.price), new Vector2(recMagicTower.X, recMagicTower.Y), tmp);

                    tmp = Color.Black;
                    if (gameSession.player.gold < fireTower.price)
                        tmp = Color.Red;
                    else
                        tmp = Color.Black;
                    sp.DrawString(spriteFont, Convert.ToString(fireTower.price), new Vector2(recFireTower.X, recFireTower.Y), tmp);

                    if (gameSession.player.gold < iceTower.price)
                        tmp = Color.Red;
                    else
                        tmp = Color.Black;
                    sp.DrawString(spriteFont, Convert.ToString(iceTower.price), new Vector2(recIceTower.X, recIceTower.Y), tmp);

                    if (gameSession.player.gold < canonTower.price)
                        tmp = Color.Red;
                    else
                        tmp = Color.Black;
                    sp.DrawString(spriteFont, Convert.ToString(canonTower.price), new Vector2(recCanonTower.X, recCanonTower.Y), tmp);
                }
                else
                {
                    sp.Draw(txtBuildButton, new Rectangle(10, 525, 50,50), Color.White);
                }
            }
        }
    }
}
