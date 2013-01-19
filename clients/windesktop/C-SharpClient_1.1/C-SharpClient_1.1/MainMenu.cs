using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace C_SharpClient_1._1
{
    class MainMenu
    {
        //GameSession
        GameSession gameSession;
        //A bool that indicates if are at the menu or in a game
        private bool gameStart = false; //we are in the menu at first
        public bool GameStart { get { return gameStart; } set { gameStart = value; } }

        private string tmpName = "";
        SpriteFont spriteFont;
        Texture2D textureButton;
        Texture2D textResumebutton;
        Rectangle rectangleResumeButton;
        Texture2D textureBackground;
        Rectangle rectangleStartButton;
        Texture2D textScroll;
        Rectangle recScroll;
        Rectangle recMultiPlayer;
        private int countDownToStart = 0;
        private int buttonSlowDown = 0;
        bool showLobby = false;
        bool gameHasStarted = false;

        public MainMenu(ref GameSession gameSession, Texture2D mSprite, Texture2D background, Texture2D textScroll, Texture2D textResume)
        {
            this.recMultiPlayer = new Rectangle(300, 300, 200, 200);
            this.spriteFont = gameSession.fontTimesNewSimon;
            this.gameSession = gameSession;
            this.textScroll = textScroll;
            this.recScroll = new Rectangle(200, 80, 400, 300); 
            this.rectangleResumeButton = new Rectangle(330, 80, 148, 44);
            this.textResumebutton = textResume;
            this.textureBackground = background;

            rectangleStartButton = new Rectangle(330, 20, 148, 44);
            this.textureButton = mSprite;
           
        }
        
        public bool Update(ref GameSession gameSession, GameTime gameTime)
        {
            this.gameSession = gameSession;
            if (!showLobby)
            {
                if (Mouse.GetState().RightButton == ButtonState.Pressed && Keyboard.GetState().GetPressedKeys().Length > 0 && buttonSlowDown > 180)
                {
                    buttonSlowDown = 0;
                    if (Keyboard.GetState().GetPressedKeys()[0] == Keys.Space)
                        tmpName += " ";
                    else
                    {
                        if (Keyboard.GetState().GetPressedKeys()[0] == Keys.Back)
                            tmpName = tmpName.Substring(0, tmpName.Length - 1);
                        else
                            tmpName += Keyboard.GetState().GetPressedKeys()[0].ToString();
                    }
                }
                else
                    buttonSlowDown += gameTime.ElapsedGameTime.Milliseconds;
                if (Mouse.GetState().RightButton == ButtonState.Released)
                {
                    buttonSlowDown = 0;
                    if (tmpName.Length > 2)
                    {
                        this.gameSession.player.playerName = tmpName;
                        tmpName = "";
                    }
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && rectangleStartButton.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    gameSession = new GameSession(gameSession.player.playerName);
                    this.gameSession = gameSession;
                    GameStart = true;
                    gameSession.mainMenu = this;
                    gameHasStarted = true;
                    return true;
                }
                if (gameHasStarted && Mouse.GetState().LeftButton == ButtonState.Pressed && rectangleResumeButton.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    this.GameStart = true;
                    return false;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && recMultiPlayer.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    showLobby = true;
                    gameSession = new GameSession(gameSession.player.playerName, "130.229.154.15", 1337);
                    gameSession.mainMenu = this;
                    gameSession.tcpGame.Send(gameSession.player.playerName + "..122..JOINRAN");
                    return true;
                }
            }
            else //What happens if your in lobby
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    countDownToStart = 0;
                    showLobby = false;
                    gameSession.tcpGame.Send(gameSession.player.playerName + "..13..QUIT");
                }
                //If the countdown starts (Tells the game will start soon
                if (countDownToStart != 0)
                {
                    if (CountDown(gameTime) < 0)
                    {
                        gameSession.mainMenu.GameStart = true;
                        countDownToStart = 0;
                    }
                }
                string s = gameSession.tcpGame.Get();
                if (s.Split(',').Length > 1)
                {
                    if (s.Split(',')[0] == "Server..0..START")
                    {
                        countDownToStart = gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60 + Convert.ToInt32(s.Split(',')[1]);
                    }
                    if (s.Split(',')[0] == "Server..0..GAMEROOM")
                    {
                        for (int i = 1; i != s.Split(',').Length; i++)
                        {
                            bool dontKnowAlready = true;
                            Player p = new Player(s.Split(',')[i]);
                            foreach (Player player in gameSession.otherPlayer)
                            {
                                if (p.playerName == player.playerName)
                                    dontKnowAlready = false;
                            }
                            if (dontKnowAlready)
                            {
                                gameSession.otherPlayer.Add(p);
                            }
                        }
                    }
                }
            }
            return false;
        }
        public int timeLeft = 0;
        private int CountDown(GameTime gameTime)
        {
            timeLeft = (countDownToStart - (gameTime.TotalGameTime.Minutes * 60 + gameTime.TotalGameTime.Seconds));
            return timeLeft;
        }

        /// <summary>
        /// Draws the whole stuff
        /// </summary>
        /// <param name="sp">A SpriteBatch to draw everything</param>
        public void Draw(SpriteBatch sp)
        {

            sp.Draw(textureBackground, new Rectangle(0, 0, 800, 600), Color.White);
            sp.DrawString(spriteFont, "PlayerName: " + gameSession.player.playerName, new Vector2(10, 550), Color.Black);
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                sp.DrawString(spriteFont, "TYPE YOUR NAME: " + tmpName, new Vector2(100, 200), Color.Black);
                return;
            }
            if (showLobby)
            {
                sp.Draw(textScroll, recScroll, Color.White);
                sp.DrawString(spriteFont, "Players in game:" , new Vector2(recScroll.X + 40, recScroll.Y + 70), Color.Red);
                int counter = 0;
                foreach (Player p in this.gameSession.otherPlayer)
                {
                    sp.DrawString(spriteFont, p.playerName, new Vector2(recScroll.X + 40, recScroll.Y + 100 + 30 * counter), Color.Black);
                    counter++;
                }
                if (countDownToStart > 0)
                    sp.DrawString(spriteFont, "Time to start: " + timeLeft, new Vector2(50, 400), Color.Black);
            }
            else
            {
                sp.Draw(textureButton, rectangleStartButton, Color.White);
                if (gameHasStarted)
                {
                    sp.Draw(textResumebutton, rectangleResumeButton, Color.White);
                    sp.Draw(textureButton, recMultiPlayer, Color.White);
                }
                else
                {
                    sp.Draw(textureButton, recMultiPlayer, Color.White);
                }
            }
        }
    }
}
