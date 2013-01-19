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
        Texture2D quikMatchButton;

        Texture2D textSreaching;
        Texture2D textGetReady;
        Texture2D textMapSelect;

        Rectangle map1Rec = new Rectangle(200,100, 200, 200);
        Rectangle map2Rec = new Rectangle(500,100, 200, 200);
        public int countDownToStart = 0;
        private int buttonSlowDown = 0;
        public bool showLobby = false;
        bool showChangeName = false;
        bool showMapMenu = false;
        public bool gameHasStarted = false;

        public MainMenu(ref GameSession gameSession, Texture2D mSprite, Texture2D quikMatchButton, Texture2D background, Texture2D textScroll, Texture2D textResume, Texture2D textSreaching, Texture2D textGetReady, Texture2D textMapSelect)
        {
            this.textMapSelect = textMapSelect;
            this.textGetReady = textGetReady;
            this.textSreaching = textSreaching;
            this.quikMatchButton = quikMatchButton;
            this.recMultiPlayer = new Rectangle(570, 450, 200, 74);
            this.spriteFont = gameSession.fontTimesNewSimon;
            this.gameSession = gameSession;
            this.textScroll = textScroll;
            this.recScroll = new Rectangle(200, 140, 400, 400);
            this.rectangleResumeButton = new Rectangle(570, 250, 200, 74);
            this.textResumebutton = textResume;
            this.textureBackground = background;

            rectangleStartButton = new Rectangle(570, 150, 200, 74);
            this.textureButton = mSprite;
           
        }

        private bool MapGo = false;
        public bool Update(ref GameSession gameSession, GameTime gameTime)
        {
            if (gameSession.s != GameSession.State.INGAME && (Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                gameSession.s = GameSession.State.INGAME;
                showLobby = false;
                countDownToStart = 0;
                gameSession.sounds.PlayMultower();
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                MapGo = true;
            if (showMapMenu && MapGo)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && map1Rec.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    showMapMenu = false;
                    Sounds s = gameSession.sounds;
                    gameSession = new GameSession(gameSession.player.playerName);
                    this.gameSession = gameSession;
                    this.gameSession.mapNumber = 0;
                    this.gameSession.sounds = s;
                    s.PlayDeFlowered();
                    GameStart = true;
                    gameSession.mainMenu = this;
                    gameHasStarted = true;
                    return true;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && map2Rec.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    showMapMenu = false;
                    Sounds s = gameSession.sounds;
                    gameSession = new GameSession(gameSession.player.playerName);
                    this.gameSession = gameSession;
                    this.gameSession.mapNumber = 1;
                    this.gameSession.sounds = s;
                    s.PlayDeFlowered();
                    GameStart = true;
                    gameSession.mainMenu = this;
                    gameHasStarted = true;
                    return true;
                }
                return false;
            }
            this.gameSession = gameSession;
            if (!showLobby && !showChangeName && Mouse.GetState().LeftButton == ButtonState.Pressed && new Rectangle(10, 550, 200, 20).Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                showChangeName = true;
            if (!showLobby)
            {
                if (showChangeName)
                {
                    if (buttonSlowDown > 80 && Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        buttonSlowDown = 0;
                        if (Keyboard.GetState().GetPressedKeys()[0] == Keys.Enter)
                        {
                            showChangeName = false;
                            gameSession.player.playerName = tmpName;
                            tmpName = "";
                            return false;
                        }
                        if (Keyboard.GetState().GetPressedKeys()[0] == Keys.Space)
                            tmpName += " ";
                        else
                        {
                            if (Keyboard.GetState().GetPressedKeys()[0] == Keys.Back && tmpName.Length > 0)
                                tmpName = tmpName.Substring(0, tmpName.Length - 1);
                            else
                                tmpName += Keyboard.GetState().GetPressedKeys()[0].ToString();
                        }
                    }
                    else
                        buttonSlowDown += gameTime.ElapsedGameTime.Milliseconds;
                }                    

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && rectangleStartButton.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    showMapMenu = true;
                    MapGo = false;
                }
                if (gameHasStarted && Mouse.GetState().LeftButton == ButtonState.Pressed && rectangleResumeButton.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    this.GameStart = true;
                    return false;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && recMultiPlayer.Intersects(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 2, 2)))
                {
                    showLobby = true;
                    Sounds s = gameSession.sounds;
                    gameSession = new GameSession(gameSession.player.playerName, "130.229.154.15", 1337);
                    gameSession.mapNumber = 0;
                    gameSession.sharedModule.SetGameSession(gameSession);
                    gameSession.mainMenu = this;
                    gameSession.sounds = s;
                    s.PlayDeFlowered();
                    gameSession.sharedModule.Update();
                    gameSession.sharedModule.FindGame(0);
                    return true;
                }
            }
            else //What happens if your in lobby
            {
                if (!gameSession.player.isSinglePlayer)
                    gameSession.sharedModule.Update();

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    countDownToStart = 0;
                    showLobby = false;
                    gameHasStarted = false;
                    gameSession.sounds.PlayMultower();

                    if (!gameSession.player.isSinglePlayer)
                    {
                        gameSession.sharedModule.Surrender();
                        gameSession.CloseConnection();
                    }
                }
                //If the countdown starts (Tells the game will start soon
                if (countDownToStart != 0)
                {
                    if (CountDown(gameTime) == 0)
                    {
                        gameSession.mainMenu.GameStart = true;
                        this.showLobby = false;
                        countDownToStart = 0;
                    }
                }
                else
                    timeLeft = 0;
            }
            return false;
        }
        public int timeLeft = 0;
        private int CountDown(GameTime gameTime)
        {
            timeLeft += gameTime.ElapsedGameTime.Milliseconds;
            if (timeLeft > 1000)
            {
                timeLeft -= 1000;
                countDownToStart--;
            }
            return countDownToStart;
        }

        /// <summary>
        /// Draws the whole stuff
        /// </summary>
        /// <param name="sp">A SpriteBatch to draw everything</param>
        public void Draw(SpriteBatch sp)
        {

            sp.Draw(textureBackground, new Rectangle(0, 0, 800, 600), Color.White);
            if (!showChangeName)
                sp.DrawString(spriteFont, "PlayerName: " + gameSession.player.playerName, new Vector2(10, 550), Color.White);
            if (showMapMenu)
            {
                sp.Draw(textMapSelect, new Rectangle(0, 0, 800, 600), Color.White);
                sp.DrawString(spriteFont, "Glacier of Despair", new Vector2(map1Rec.X, map1Rec.Y - 25), Color.White);
                sp.DrawString(spriteFont, "Fields of Dechamak", new Vector2(map2Rec.X, map2Rec.Y - 25), Color.White);
                sp.Draw(gameSession.map1, map1Rec, Color.White);
                sp.Draw(gameSession.map2, map2Rec, Color.White);
                return;
            }

            if (showChangeName)
            {
                sp.DrawString(spriteFont, "TYPE YOUR NAME: " + tmpName, new Vector2(10, 550), Color.White);
                return;
            }
            if (showLobby)
            {
                if (countDownToStart > 0)
                    sp.Draw(textGetReady, new Rectangle(0, 0, 800, 600), Color.White);
                else
                    sp.Draw(textSreaching, new Rectangle(0, 0, 800, 600), Color.White);
                sp.DrawString(spriteFont, "Players in game:" , new Vector2(recScroll.X + 40, recScroll.Y + 70), Color.Red);
                int counter = 0;
                sp.DrawString(spriteFont, gameSession.player.playerName, new Vector2(recScroll.X + 40, recScroll.Y + 100 + 30 * counter), Color.Blue);
                foreach (Player p in this.gameSession.otherPlayer)
                {
                    counter++;
                    sp.DrawString(spriteFont, p.playerName, new Vector2(recScroll.X + 40, recScroll.Y + 100 + 30 * counter), Color.White);
                }
                if (countDownToStart > 0)
                    sp.DrawString(spriteFont, "Time to start: " + countDownToStart + ":" + timeLeft, new Vector2(recScroll.X + 40, 400), Color.White);
            }
            else
            {
                sp.Draw(textureButton, rectangleStartButton, Color.White);
                if (gameHasStarted)
                {
                    sp.Draw(textResumebutton, rectangleResumeButton, Color.White);
                    sp.Draw(quikMatchButton, recMultiPlayer, Color.White);
                }
                else
                {
                    sp.Draw(quikMatchButton, recMultiPlayer, Color.White);
                    sp.Draw(textResumebutton, rectangleResumeButton, Color.White * 0.5F);
                }
            }
        }
    }
}
