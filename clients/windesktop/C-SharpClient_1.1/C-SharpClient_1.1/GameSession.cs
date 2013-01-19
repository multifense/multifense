using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace C_SharpClient_1._1
{
    /// <summary>
    /// A class containing classes:
    /// TcpGame (Which is the games tcp connection)
    /// GamePath (Gives the monster a path (and fix pathblocking)
    /// SharedModule (A wrapper class for the shared module)
    /// </summary>
    class GameSession
    {
        //A list with every sprite that will be drawn (With expection of the hidden)
        public List<GameObject> listToPrint = new List<GameObject>();
        public List<GameObject> animations = new List<GameObject>();

        public Tower fireTower;
        public Tower magicTower;
        public Tower iceTower;
        public Tower canonTower;

        public State s = State.INGAME;
        public enum State
        {
            WIN,
            LOSE,
            INGAME
        }
        public TcpGame tcpGame;
        public UserInterface ui;
        public Player player;
        public MultiMobWave multiMobWave;
        public List<MobWave> mobWaves = new List<MobWave>();
        public int mobWave = 0;
        public List<Player> otherPlayer = new List<Player>();
        public Hashtable idToMonster = new Hashtable();
        private List<Monster> monsterArcheType = new List<Monster>();

        public Map map; // the class :P
        public MainMenu mainMenu; // the class
        public TextBox console;
        public TextBox informationBox;

        public SpriteFont fontTimesNewSimon;
        public GamePath gamePath;
        public SharedModule sharedModule;
        public int mapNumber = 1;
        public Texture2D map1;
        public Texture2D map2;

        public Sounds sounds;

        /// <summary>
        /// For SinglePlayer with roadblock
        /// </summary>
        public GameSession(string playerName)
        {
            player = new Player(playerName, 0);
            player.isSinglePlayer = true;
            sharedModule = new SharedModule(this);

        }
        
        public GameSession(string playerName, string ip, int port)
        {
            player = new Player(playerName, 0);
            player.isSinglePlayer = false;
            multiMobWave = new MultiMobWave(this);
            //NetWorkStuff
            if (tcpGame == null)
            {
                try
                {
                    tcpGame = new TcpGame(ip, port);
                }
                catch
                {
                    throw new Exception("Could not connect to server: " + ip + ":" + port);
                }
            }
            if (sharedModule == null)
                sharedModule = new SharedModule(this);
        }
        public int AmmountOfMonsterOnField()
        {
            int counter = 0;
            foreach (GameObject g in listToPrint)
                if (g.type == "Monster")
                    counter++;
            return counter;
        }
        public void LoadMap(int number, Game gameLoad)
        {
            gamePath = new GamePath();
            map1 = gameLoad.Content.Load<Texture2D>("map1600x960ice");
            map2 = gameLoad.Content.Load<Texture2D>("awsomemap");
            if (number == 0)
            {
                map = new Map(map1, new Rectangle(0, 0, 1600, 960));
                sharedModule.LoadMapData(1);
                gamePath.InitlazieHidden(ref listToPrint, gameLoad.Content.Load<Texture2D>("red"), this);
            }
            else
            {
                map = new Map(map2, new Rectangle(0, 0, 1600, 960));
                sharedModule.LoadMapData(2);
                gamePath.InitlazieHidden(ref listToPrint, gameLoad.Content.Load<Texture2D>("red"), this);
                animations.Add(new GameObjects.Portals(gameLoad.Content.Load<Texture2D>("portalinsprite"), new Rectangle(130,0,150, 100), new Rectangle(0,0,150,100)));
                animations.Add(new GameObjects.Portals(gameLoad.Content.Load<Texture2D>("portalutsprite"), new Rectangle(1080, 860, 150, 100), new Rectangle(0, 0, 150, 100)));
            }
        }

        public void YouWin()
        {
            mainMenu.GameStart = false;
            mainMenu.gameHasStarted = false;
            s = GameSession.State.WIN;
            if (!player.isSinglePlayer)
                CloseConnection();
        }
        public void YouLose()
        {
            mainMenu.GameStart = false;
            mainMenu.gameHasStarted = false;
            s = GameSession.State.LOSE;
            if (!player.isSinglePlayer)
                CloseConnection();
        }
        public void CloseConnection()
        {
            if (tcpGame != null)
            {
                tcpGame.Close();
                tcpGame = null;
            }
        }

        public void LoadContent(Texture2D textConsole)
        {
            console = new TextBox(textConsole, new Rectangle(50, 120, 600, 400), fontTimesNewSimon);
            informationBox = new TextBox(textConsole, new Rectangle(50, 490, 700, 100), fontTimesNewSimon); 
        }
        public void SetUpMonsterWave()
        {
            if (player.isSinglePlayer && monsterArcheType.Count > 0)
            {
                try
                {
                    StreamReader str = new StreamReader("data.txt");
                    string strin = str.ReadLine();
                    int counter = 0;
                    mobWaves.Add(new MobWave(this));
                    while (strin != "")
                    {
                        string[] split = strin.Split(' ');
                        
                        mobWaves[counter].addEntry(new MobWave.MobWaveEntry((Monster)idToMonster[Convert.ToInt32(split[0])], 
                            Convert.ToInt32(split[1]), 
                            Convert.ToInt32(split[2]), 
                            Convert.ToInt32(split[3]), 
                            Convert.ToInt32(split[4])));
                        
                        strin = str.ReadLine();
                        if (strin == "-")
                        {
                            mobWaves[counter].CreateWave();
                            counter++;
                            strin = str.ReadLine();
                            if (strin == null)
                                break;
                            mobWaves.Add(new MobWave(this));
                        }
                    }
                    str.Close();
                }
                catch
                {
                    throw new Exception("Problem with data.txt");
                }
            }
        }

        public Monster SpawnMonster(int id)
        {
            Monster mo = (Monster)idToMonster[id];
            Monster motmp = mo.Clone();
            listToPrint.Add(motmp);
            return motmp;
        }

        public void AddMonster(Monster mo, int id, int cost)
        {
            if (!idToMonster.ContainsKey(id))
            {
                mo.typeID = id;
                mo.cost = cost;
                mo.SetGameSession(this);
                mo.font1text = fontTimesNewSimon;
                mo.ScaleRectangle = new Rectangle(0, 0, 100, 100);
                idToMonster.Add(id, mo);
                monsterArcheType.Add(mo);
            }
        }
        public void AddMonster(Monster mo, int id, int cost, Color c)
        {
            if (!idToMonster.ContainsKey(id))
            {
                mo.typeID = id;
                mo.cost = cost;
                mo.SetGameSession(this);
                mo.font1text = fontTimesNewSimon;
                mo.color = c;
                mo.ScaleRectangle = new Rectangle(0, 0, 100, 100);
                idToMonster.Add(id, mo);
                monsterArcheType.Add(mo);
            }
        }

        public Monster GetidToMonster(int id)
        {
            return (Monster)idToMonster[id];
        }
        public List<Monster> GetallMonsters()
        {
            return monsterArcheType;
        }

        public class GamePath
        {
            public class GamePathPoint
            {

                public GamePathPoint next;
                public Vector2 dir;
                public double length;
                public Vector2 p;

                //Only used to create first gamePath
                public GamePathPoint(Vector2 p)
                {
                    this.p = p;
                }

                public GamePathPoint(Vector2 p, Vector2 dir, double length)
                {
                    this.p = p;
                    this.dir = dir;
                    this.length = length;
                }


                public void ConnectWithPathPoint(GamePathPoint GPP)
                {
                    this.next = GPP;
                    this.dir = GPP.p - this.p;
                    this.length = Math.Sqrt(Math.Pow((GPP.p.X - this.p.X), 2) + Math.Pow((GPP.p.Y - this.p.Y), 2));
                    this.dir.Normalize();
                }

                //Lolcode
                public void SetNext(GamePathPoint GPP)
                {
                    this.next = GPP;
                }
            }


            //GamePath class begins here

            GamePathPoint pathEnd;
            GamePathPoint pathStart;
            public GamePathPoint PathStart { get { return pathStart; } }

            public void Initialize1()
            {  
                AddPathPoint(-100,225);
                AddPathPoint(140, 225);
                AddPathPoint(140, 740);
                AddPathPoint(480, 740);
                AddPathPoint(480, 490);
                AddPathPoint(690, 490);
                AddPathPoint(690, 100);
                AddPathPoint(910, 100);
                AddPathPoint(910, 725);
                AddPathPoint(1315, 725);
                AddPathPoint(1315, 300);
                AddPathPoint(1700, 300);
            }
            public void Initailize2()
            {
                AddPathPoint(200,0);
                AddPathPoint(200,400);
                AddPathPoint(100,400);
                AddPathPoint(100,600);
                AddPathPoint(1400,600);
                AddPathPoint(1400,150);
                AddPathPoint(550,150);
                AddPathPoint(550,800);
                AddPathPoint(1150,800);
                AddPathPoint(1150,960);
            }
            /// <summary>
            /// Method 
            /// </summary>
            /// <param name="listToPrint"></param>
            public void InitlazieHidden(ref List<GameObject> listToPrint , Texture2D txtred, GameSession gameSession)
            {
                //listToPrint.Add(new Hidden(new Rectangle(0, 0, 100, 100)));
                //Add code here
                GamePathPoint tmpPoint = pathStart;

                while (tmpPoint != null)
                {
                    int x1 = (int)tmpPoint.p.X;
                    int y1 = (int)tmpPoint.p.Y;
                    tmpPoint = tmpPoint.next;
                    if (tmpPoint == null)
                        break;
                    int x2 = (int)tmpPoint.p.X;
                    int y2 = (int)tmpPoint.p.Y;


                    if (x1-50 < 0)
                    {
                        x1 = 50;
                    }
                    else if (x1+50 > 1600)
                    {
                        x1 = 1550;
                    }
                    if (x2 -50< 0)
                    {
                        x2 = 50;
                    }
                    else if (x2+50 > 1600)
                    {
                        x2 = 1550;
                    }

                    if (y1-50 < 0)
                    {
                        y1 = 50;
                    }
                    else if (y1+50 > 900)
                    {
                        y1 = 850;
                    }
                    if (y2-50 < 0)
                    {
                        y2 = 50;
                    }
                    else if (y2+50 > 900)
                    {
                        y2 = 850;
                    }
                    Hidden hidden;
                    if (x1 <= x2 && y1 <= y2) // om den nya punkten är större i x och y värde
                    {
                        hidden = new Hidden(txtred, new Rectangle(x1 - 50, y1 - 50, x2 - x1 + 100, y2 - y1 + 100));
                        hidden.SetGameSession(gameSession);
                        listToPrint.Add(hidden);
                    }
                    if (x1 <= x2 && y1 >= y2) // om den nya punkten är större i x men mindre i y värde
                    {
                        hidden = new Hidden(txtred, new Rectangle(x1 - 50, y2 - 50, x2 - x1 + 100, y1 - y2 + 100));
                        hidden.SetGameSession(gameSession);
                        listToPrint.Add(hidden);
                    }
                    if (x1 >= x2 && y1 <= y2) // om den nya punkten är mindre i x men större i y värde
                    {
                        hidden = new Hidden(txtred, new Rectangle(x2 - 50, y1 - 50, x1 - x2 + 100, y2 - y1 + 100));
                        hidden.SetGameSession(gameSession);
                        listToPrint.Add(hidden);
                    }
                    if (x1 >= x2 && y1 >= y2) // om den nya punkten är mindre i x och y värde
                    {
                        hidden = new Hidden(txtred, new Rectangle(x2 - 50, y2 - 50, x1 - x2 + 100, y1 - y2 + 100));
                        hidden.SetGameSession(gameSession);
                        listToPrint.Add(hidden);
                    }
                }

            }

            public void AddCodedPathPoint(int x, int y, int dirx, int diry, int length)
            {
                GamePathPoint pp = new GamePathPoint(new Vector2(x, y), new Vector2(dirx, diry), length);
                if (pathStart == null)
                {
                    pathStart = pathEnd = pp;
                }
                else
                {
                    pathEnd = pathEnd.next = pp;
                }
            }

            public void AddPathPoint(int x, int y)
            {
                AppendPathPoint(new GamePathPoint(new Vector2(x, y)));
            }

            public bool LoadPathFromFile(string name)
            {
                return false;
            }
            
            private void AppendPathPoint(GamePathPoint GPP)
            {
                if (pathStart == null)
                {
                    pathStart = GPP;
                    pathEnd = GPP;
                }
                else
                {
                    pathEnd.ConnectWithPathPoint(GPP);
                    pathEnd = GPP;
                }
            }
        }

        public class TcpGame
        {
            TcpClient tcpClient;
            NetworkStream netStream;
            private Thread th;

            System.IO.StreamReader str;
            System.IO.StreamWriter stw;

            private Queue<string> queue = new Queue<string>();
            /// <summary>
            /// Inilize a tcp connection
            /// </summary>
            /// <param name="ip">The IP adress you want to connect to</param>
            /// <param name="port">The port you want to connect to</param>
            public TcpGame(string ip, int port)
            {
                tcpClient = new TcpClient(ip, port);
                netStream = tcpClient.GetStream();
                stw = new System.IO.StreamWriter(netStream);
                th = new Thread(UpdateBuffer);
                th.IsBackground = true;
                th.Start();
            }
            /// <summary>
            /// Fills the queue with strings that the server sent
            /// </summary>
            private void UpdateBuffer()
            {
                str = new System.IO.StreamReader(netStream);
                while (true)
                {
                    try
                    {
                        queue.Enqueue(str.ReadLine());
                    }
                    catch { }
                }
            }
            /// <summary>
            /// Send message to server
            /// </summary>
            /// <param name="msg">The string you wanna send</param>
            public void Send(string msg)
            {
                try
                {
                    stw.WriteLine(msg);
                    stw.Flush();
                }
                catch
                {
                    //throw new Exception("Could not send to server");
                }
            }

            /// <summary>
            /// Get a string from the server
            /// </summary>
            /// <returns>Returns "" if there is nothing to get</returns>
            public string Get()
            {
                if (queue.Count > 0)
                    return queue.Dequeue();
                return "";
            }
            /// <summary>
            /// Close the Connection
            /// </summary>
            public void Close()
            {
                th.Abort();
                tcpClient.Close();
            }
        }
        public class TextBox : GameObject
        {
            private int counter = 0;
            private int max;
            private Queue<string> queue = new Queue<string>();

            public TextBox(Texture2D txt2D, Rectangle rec, SpriteFont spriteFont)
            {
                base.color = new Color(255, 255, 255, 0);
                base.strouput = "";
                base.font1text = spriteFont;
                base.Text2D = txt2D;
                base.Rec = rec;
                max = (base.Rec.Height / 22);
                base.setOutPutVector = new Vector2(base.Rec.X + 5, base.Rec.Y);
            }
            public void AddMessage(string s)
            {
                if (queue.Count >= max)
                    queue.Dequeue();
                queue.Enqueue(s);
                UpdateMessage();
            }
            public override void SetGameSession(GameSession gameSession)
            {     }
            public override bool Update(ref List<GameObject> listToPrint)
            {
                counter++;
                if (counter > (5000 / 33))
                {
                    counter = 0;
                    if (queue.Count > 0)
                        queue.Dequeue();
                    UpdateMessage();
                }
                return false;
            }
            private void UpdateMessage()
            {
                strouput = "";
                string[] tmp = queue.ToArray();
                for (int i = tmp.Length; i != 0; i--)
                {
                    strouput += tmp[i-1] + "\r\n";
                }
            }
        }
    }
}
