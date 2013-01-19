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
        private bool showBuildMenu = false;

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

        public UserInterface(Game game, Tower superTower, Tower fireTower, Tower iceTower, Tower magicTower, Tower canonTower,Texture2D menuBar, Texture2D txtGold, GameSession gameSession)
        {
            this.gameSession = gameSession;
            this.superTower = superTower;
            this.canonTower = canonTower;
            this.recCanonTower = new Rectangle(300, 510, 50, 50);
            this.txtGold = txtGold;
            this.game = game;
            this.menuBar = menuBar;
            this.fireTower = fireTower;
            this.recSuperTower = new Rectangle(750, 550, 50, 50);
            this.recFireTower = new Rectangle(100, 510, 50,50);
            this.iceTower = iceTower;
            this.recIceTower = new Rectangle(160, 510, 50,50);
            this.magicTower = magicTower;
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
        public void Update(GameTime gameTime, ref Tower myCurrentTower, ref GameSession gameSession)
        {
            if (console != null && txtBoxinformation != null)
            {
                console.Update(ref gameSession.listToPrint);
                txtBoxinformation.Update(ref gameSession.listToPrint);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (gameSession.player.isSinglePlayer)
                    gameSession.mainMenu.GameStart = false;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
                showStat = !showStat;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(new Rectangle(30, 440, 10, 10)))
                showBuildMenu = true;
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                showBuildMenu = false;
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
            if (showBuildMenu && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 5, Mouse.GetState().Y + 5, 10, 10).Intersects(recSuperTower))
            {
                myCurrentTower = superTower;
                myCurrentTower.typeOfTower = "SuperTower";
                game.IsMouseVisible = false;
                showBuildMenu = false;
            }
            if (game.IsMouseVisible && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(Mouse.GetState().X + 3, Mouse.GetState().Y + 5, 5, 3).Intersects(new Rectangle(10, 550, 20, 20)))
                showBuildMenu = true;
        }
        public void Draw(SpriteBatch sp, GameTime gameTime)
        {
            if (showStat)
            {
                sp.Draw(superTower.Text2D, recSuperTower, Color.Green);
                if (gameTime.ElapsedGameTime.Milliseconds > 0)
                    sp.DrawString(spriteFont, "FPS: " + 1000 / gameTime.ElapsedGameTime.Milliseconds, new Vector2(100, 20), Color.Black);
                sp.DrawString(spriteFont, "X: " + Mouse.GetState().X + " Y: " + Mouse.GetState().Y, new Vector2(100, 0), Color.Black);
                sp.DrawString(spriteFont, "Ammount of Creeps: " + listToPrint.Count, new Vector2(100, 50), Color.Black);
                if (console != null)
                    console.Draw(sp, 0, 0);
                //sp.DrawString(spriteFont, "Time: " + gameTime.TotalGameTime + " Next mobwave: " + gameSession.mobWave.TimeToNextWave(gameTime), new Vector2(0, 450), Color.Black);
            }
            else
            {
                if (txtBoxinformation != null)
                    txtBoxinformation.Draw(sp, 0, 0);
                sp.Draw(menuBar, new Rectangle(10, 0, 600, 130), Color.White);
                sp.DrawString(spriteFont, "Ammount of Creeps: " + listToPrint.Count + " Monsters Killed: " + gameSession.player.monsterkilled, new Vector2(80, 35), Color.Black);
                if (gameSession.player.isSinglePlayer)
                {
                    sp.DrawString(spriteFont, "Next level: " + "                          " + gameSession.player.gold, new Vector2(80, 55), Color.Black);
                    sp.DrawString(spriteFont, "Mobwave: " + "                  "+ " Life left: " + gameSession.player.hp, new Vector2(80, 75), Color.Black);
                }
                else
                {
                    sp.DrawString(spriteFont, "Mobwave: " + gameSession.mobWave.waveNumber + " Life left: " + gameSession.player.hp, new Vector2(80, 75), Color.Black);
                    sp.DrawString(spriteFont, "Next mobwave: " + gameSession.mobWave.TimeToNextWave(gameTime) + "    " + gameSession.player.gold, new Vector2(80, 55), Color.Black);
                }
                
                sp.Draw(txtGold, new Rectangle(250, 60, 14, 14), Color.White);
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
                    sp.Draw(menuBar, new Rectangle(10, 550, 20,20), Color.Black);
                }
            }
        }
    }
}
