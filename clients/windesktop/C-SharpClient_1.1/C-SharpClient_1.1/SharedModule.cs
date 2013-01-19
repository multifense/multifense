using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace C_SharpClient_1._1
{
    class SharedModule
    {
        private Game thisGame;

        GameSession gameSession;
        DNetIO _debugLog;
        DNetIO _netWrite;
        DPlayerWithName _playerJoined;
        DPlayer _playerLeft;
        DPlayerWithTime _didFindGame;
        DPlayerReferenceWithTypeAndCoords _towerWasCreated;
        DPlayerReferenceWithType _monsterWasCreated;
        DReference _monsterWasKileld;
        DPlayer _waveWasKilledForPlayer;
        DPlayerVersusPlayerWithArgument _playerWasDamagedByPlayer;
        DPlayer _playerDied;
        DPlayer _playerSurrendered;
        DReferenceWithHealthViaPlayer _monsterWasSentByPlayer;
        DTime _spawnNextMobWave;
        DPlayer _victorWasDecided;
        DMonsterDefinition _monsterDef;
        DTowerDefinition _towerDef;
	    DPathDefinition _pathDef;


        public void SetGameSession(GameSession gam)
        {
            this.gameSession = gam;
        }

        public SharedModule(GameSession gameSession)
        {
            this.gameSession = gameSession;
            _debugLog = new DNetIO(DebugLog); 
            _netWrite = new DNetIO(NetWrite);
            _playerJoined = new DPlayerWithName(PlayerJoined);
            _playerLeft = new DPlayer(PlayerLeft);
            _didFindGame = new DPlayerWithTime(DidFindGame);
            _towerWasCreated = new DPlayerReferenceWithTypeAndCoords(TowerWasCreated);
            _monsterWasCreated = new DPlayerReferenceWithType(MonsterWasCreated);
            _monsterWasKileld = new DReference(MonsterWasKilled);
            _waveWasKilledForPlayer = new DPlayer(WaveWasKilledForPlayer); 
            _playerWasDamagedByPlayer = new DPlayerVersusPlayerWithArgument(PlayerWasDamagedByPlayer);
            _playerDied = new DPlayer(PlayerDied);
            _playerSurrendered = new DPlayer(PlayerSurrendered);
            _monsterWasSentByPlayer = new DReferenceWithHealthViaPlayer(MonsterWasSentByPlayer);
            _spawnNextMobWave = new DTime(SpawnNextMobWave);
            _victorWasDecided = new DPlayer(VictorWasDes);
            _monsterDef = new DMonsterDefinition(MonsterDef);
            _towerDef = new DTowerDefinition(TowerDef);
	        _pathDef = new DPathDefinition(PathDef);

            TdmpKernel(_debugLog, _netWrite, _playerJoined, _playerLeft, _didFindGame, _towerWasCreated, _monsterWasCreated, _monsterWasKileld, 
                _waveWasKilledForPlayer, _playerWasDamagedByPlayer, _playerDied, _playerSurrendered, _monsterWasSentByPlayer, _spawnNextMobWave,
                _victorWasDecided, _monsterDef, _towerDef, _pathDef);

            NewGame();

            if (!gameSession.player.isSinglePlayer)
            {
                setNickName(gameSession.player.playerName);
            }
        }
        public void LoadContent(Game thisGame)
        {
            towerCouter = 0;
            this.thisGame = thisGame;
            loadData();
        }
        public void Update()
        {

            if (gameSession.tcpGame != null)
            {
                string s = gameSession.tcpGame.Get();

                if (s != "")
                {
                    didRead(s +"\r\n");
                }
            }
        }
        
        const string KERNELDLL = @"kernal.dll";
        [DllImport(KERNELDLL, EntryPoint = "?didRead@kernel@tdmp@@QAEXPBD@Z", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void didRead(string s);
        [DllImport(KERNELDLL, EntryPoint = "?loadData@kernel@tdmp@@QAEXXZ", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void loadData();
        [DllImport(KERNELDLL, EntryPoint = "?setNickname@kernel@tdmp@@QAEXPBD@Z", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void setNickName(string s);
        [DllImport(KERNELDLL, EntryPoint = "?didCreateTower@kernel@tdmp@@QAEXHHHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didCreateTower(int tower, int type, int x, int y);
        [DllImport(KERNELDLL, EntryPoint = "?didDie@kernel@tdmp@@QAEXXZ", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didDie();
        [DllImport(KERNELDLL, EntryPoint = "?didKillLastMonster@kernel@tdmp@@QAEXXZ", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didKillLastMonster();
        [DllImport(KERNELDLL, EntryPoint = "?didKillMonster@kernel@tdmp@@QAEXH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didKillMonster(int monster);
        [DllImport(KERNELDLL, EntryPoint = "?didRecruitMonster@kernel@tdmp@@QAEXHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didRecruitMonster(int type, int hp);
        [DllImport(KERNELDLL, EntryPoint = "?didSpawnMonster@kernel@tdmp@@QAEXHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didSpawnMonster(int monster, int type);
        [DllImport(KERNELDLL, EntryPoint = "?didSurrender@kernel@tdmp@@QAEXXZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern void didSurrender();
        [DllImport(KERNELDLL, EntryPoint = "?didTakeDamage@kernel@tdmp@@QAEXHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void didTakeDamage(int damage, int culprit);
        [DllImport(KERNELDLL, EntryPoint = "?findGame@kernel@tdmp@@QAEXH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void findGame(int player);
        [DllImport(KERNELDLL, EntryPoint = "?sharedModuleSelfTest@kernel@tdmp@@QAEXXZ", CallingConvention = CallingConvention.Cdecl)]
        private static extern void sharedModuleSelfTest();
        [DllImport(KERNELDLL, EntryPoint = "_TdmpKernel@72", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void TdmpKernel(
                               DNetIO a,// callback for debug messages
                               DNetIO b,                             // callback for sending strings to server
                               DPlayerWithName c,                    // playerJoined;
                               DPlayer d,                            // playerLeft;
                               DPlayerWithTime e,                    // didFindGame; player = your own player ID; set me.playerID = id on call
                               DPlayerReferenceWithTypeAndCoords f,  // towerWasCreated
                               DPlayerReferenceWithType g,           // monsterWasCreated
                               DReference h,                         // monsterWasKilled
                               DPlayer i,                            // waveWasKilledForPlayer
                               DPlayerVersusPlayerWithArgument k,    // playerWasDamagedByPlayer
                               DPlayer j,                            // playerDied
                               DPlayer l,                            // playerSurrendered
                               DReferenceWithHealthViaPlayer m,      // monsterWasSentByPlayer
                               DTime n,                             // Next mobwave in seconds 
                               DPlayer o,
                               DMonsterDefinition p,
                               DTowerDefinition q,
			                   DPathDefinition r
                               );

        [DllImport(KERNELDLL, EntryPoint = "?newGame@kernel@tdmp@@QAEXXZ", CallingConvention = CallingConvention.Cdecl)]
        private static extern void NewGame(); // (economy) update single player wave count 
        [DllImport(KERNELDLL, EntryPoint = "?updateSPIncome@kernel@tdmp@@QAEHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern int updateSPIncome(int waveCount); // (economy) update single player wave count 
        public int UpdateSPIncome(int wave)
        {
            if (wave != 0)
            {
                return updateSPIncome(wave);
            }
            return spIncome();
        }
        [DllImport(KERNELDLL, EntryPoint = "?getBountyForMonster@kernel@tdmp@@QAEHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getBountyForMonster(int type);                      // (economy) how much $ should player get for killing mob
        public int GetBountyForMonster(int type)
        { return getBountyForMonster(type); }
        [DllImport(KERNELDLL, EntryPoint = "?updateMPIncomeForBuyingMonster@kernel@tdmp@@QAEHH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern int updateMPIncomeForBuyingMonster(int type);           // (economy) increase and return new income for buying mob
        public int UpdateMPIncomeForBuyingMonster(int type)
        { return updateMPIncomeForBuyingMonster(type); }
        [DllImport(KERNELDLL, EntryPoint = "?mpIncome@kernel@tdmp@@QAEHXZ", CallingConvention = CallingConvention.Cdecl)]
        private static extern int mpIncome();
        public int MpIncome()
        { return mpIncome(); }
        [DllImport(KERNELDLL, EntryPoint = "?spIncome@kernel@tdmp@@QAEHXZ", CallingConvention = CallingConvention.Cdecl)]
        private static extern int spIncome();
        public int SpIncome()
        { return spIncome(); }
        [DllImport(KERNELDLL, EntryPoint = "?loadMapData@kernel@tdmp@@QAEXH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern void loadMapData(int mapid);                               // load path points for a given map
        public void LoadMapData(int mapid)
        { loadMapData(mapid); }
        [DllImport(KERNELDLL, EntryPoint = "?getNameForMap@kernel@tdmp@@QAEPBDH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern string getNameForMap(int mapid);                   // obtain the name of a given map
        public string GetNameForMap(int mapid)
        { return getNameForMap(mapid); }
        [DllImport(KERNELDLL, EntryPoint = "?getImageNameForMap@kernel@tdmp@@QAEPBDH@Z", CallingConvention = CallingConvention.Cdecl)]
        private static extern string getImageNameForMap(int mapid);                 // obtain the .png name of a given map
        public string GetImageNameForMap(int mapid)
        { return getImageNameForMap(mapid);}


        public delegate void DPlayerReferenceWithTypeAndCoords(int player, int reference, int Type, int x, int y);
        public delegate void DPlayerReferenceWithType(int player, int reference, int type);
        public delegate void DReference(int reference);
        public delegate void DPlayer(int player);
        public delegate void DPlayerVersusPlayerWithArgument(int player, int versusPlayer, int argument);
        public delegate void DVoid();
        public delegate void DReferenceViaPlayer(int reference, int viaPlayer);
        public delegate void DNetIO(string netIO);
        public delegate void DReferenceWithHealthViaPlayer(int reference, int health, int player);
        public delegate void DPlayerWithTime(int player, int time);
        public delegate void DPlayerWithName(int player, string name);
        public delegate void DTime(int time);
        public delegate void DMonsterDefinition(int type, string sprite, int health, int speed, int sendCost, int incomeIncrease, int coloring);
        public delegate void DTowerDefinition(int type, string sprite, int damage, int fireRate, int range, int cost, string projSprite, int projSpeed, string projSound);
        public delegate void DPathDefinition(int x, int y, int dirx, int diry, int length);

        private void DebugLog(string netIO)
        {
            System.Diagnostics.Debug.WriteLine(netIO);
        }
        private void NetWrite(string netIO)
        {
            if (this.gameSession != null && this.gameSession.tcpGame != null)
                this.gameSession.tcpGame.Send(netIO);
        }
        private void VictorWasDes(int player)
        {
            if (player == 0)
                gameSession.YouWin();
            else
                gameSession.YouLose();
        }
        private void MonsterDef(int type, string sprite, int health, int speed, int sendCost, int incomeIncrease, int coloring)
        {
            Monster mo = new Monster(thisGame.Content.Load<Texture2D>(sprite.Substring(0, sprite.Length -4)), new Rectangle(0, 0, 50, 50), health, speed);
            if (coloring == 1)
                mo.color = Color.Tomato;
            if (coloring == 2)
                mo.color = Color.Brown;
            if (coloring == 3)
                mo.color = new Color(new Vector4(0,0,0,100));
            gameSession.AddMonster(mo, type, sendCost);
        }
        //cannon
        //magic
        //fire
        //ice
        private int towerCouter = 0;
        private void TowerDef(int type, string sprite, int damage, int fireRate, int range, int cost, string projSprite, int projSpeed, string projSound)
        {
            Tower t = new Tower(thisGame.Content.Load<Texture2D>(sprite.Substring(0, sprite.Length - 4)), new Rectangle(0, 0, 50, 50), fireRate, damage, range, thisGame.Content.Load<Texture2D>(projSprite.Substring(0, projSprite.Length - 4)), cost, thisGame.Content.Load<SoundEffect>(projSound.Substring(0, projSound.Length - 4)), projSpeed);
            if (towerCouter == 0)
                gameSession.canonTower = t;
            if (towerCouter == 1)
                gameSession.magicTower = t;
            if (towerCouter == 2)
            {
                t.scaleTheBullet = new Rectangle(0, 0, 25, 25);
                gameSession.fireTower = t;
            }
            if (towerCouter == 3)
                gameSession.iceTower = t;
            towerCouter++;

            //ANvänds inte!
        }

        private void PathDef(int x, int y, int dirx, int diry, int length)
        {
            gameSession.gamePath.AddCodedPathPoint(x, y, dirx, diry, length);
        }

        private void PlayerJoined(int player, string name)
        {
            gameSession.console.AddMessage("player with id: " + player + " joined");
            gameSession.otherPlayer.Add(new Player(name, player));
        }
        private void PlayerLeft(int player)
        {
            gameSession.console.AddMessage("player with id: " + player + " left");
            Player removePlayer = null;
            foreach (Player p in gameSession.otherPlayer)
                if (p.id == player)
                    removePlayer = p;
            if (removePlayer != null)
                gameSession.otherPlayer.Remove(removePlayer);
        }
        private void DidFindGame(int youPayerID, int startInseconds)
        {
            gameSession.console.AddMessage("You get yours id: " + youPayerID);
            gameSession.mainMenu.countDownToStart = new GameTime().TotalGameTime.Seconds + new GameTime().TotalGameTime.Minutes * 60 +startInseconds;
        }
        private void PlayerDied(int player)
        {
            gameSession.console.AddMessage("Player died: " + player);
            gameSession.informationBox.AddMessage("Player died: " + player);
        }
        private void TowerWasCreated(int player, int reference, int type, int x, int y)
        {
            gameSession.console.AddMessage("A tower was created by: " + player + " with id " + reference + " with type: " + type + " on spot: " + x + ":" + y);
            gameSession.informationBox.AddMessage("A tower was created by: " + player + " with id " + reference + " with type: " + type + " on spot: " + x + ":" + y);
        }
        private void MonsterWasCreated(int player, int reference, int type)
        {
            gameSession.console.AddMessage("A monster was send from: " + player + " with id " + reference + " and type: " + type);
            gameSession.informationBox.AddMessage("A monster was send from: " + player + " with id " + reference + " and type: " + type);
        }
        private void MonsterWasKilled(int reference)
        {
            gameSession.console.AddMessage("Monster with id: " + reference + " was killed");
        }
        private void WaveWasKilledForPlayer(int player)
        {
            gameSession.console.AddMessage("Wave was killed for player " + player);
            gameSession.informationBox.AddMessage("Wave was killed for player " + player);
        }
        private void PlayerWasDamagedByPlayer(int player, int versusPlayer, int argument)
        {
            System.Diagnostics.Debug.WriteLine("Player: " + player + " VersusPlayer: " + versusPlayer);
            gameSession.console.AddMessage("Player: " + player + " was hurted by " + versusPlayer + " the ammount of damage " + argument);
            gameSession.informationBox.AddMessage("Player: " + player + " was hurted by " + versusPlayer + " the ammount of damage " + argument);
            if (versusPlayer == 0 && gameSession.player.hp > 0)
                gameSession.player.hp++;
            
            foreach (Player p in gameSession.otherPlayer)
            {
                if (p.id == player && p.hp  != 0)
                    p.hp--;
                if (p.id == versusPlayer && p.hp > 0)
                    p.hp++;
            }
        }
        private void PlayerSurrendered(int player)
        {
            gameSession.console.AddMessage("Player did surrender: " + player);
            gameSession.informationBox.AddMessage("Player did surrender: " + player);
        }
        private void MonsterWasSentByPlayer(int reference, int health, int viaPlayer)
        {
            gameSession.console.AddMessage("Monster: " + reference + " was sent by: " + viaPlayer);
            gameSession.informationBox.AddMessage("Monster: " + reference + " was sent by: " + viaPlayer);
            gameSession.multiMobWave.AddMonsterToNextMobWave(reference, health, viaPlayer);
            //Spawnar ett monster
        }
        private void SpawnNextMobWave(int timer)
        {
            gameSession.multiMobWave.SetSpawnTimer(timer);
            gameSession.console.AddMessage("Next mobwave will spawn in: " + timer + " seconds");
            gameSession.informationBox.AddMessage("Next mobwave will spawn in: " + timer + " seconds");
        }

        /// <summary>
        /// ////////////////////////////////////////
        /// </summary>
        /// <param name="type"></param>
        public void RecruitMonster(int type, int hp)
        {
            didRecruitMonster(type, hp);
        }
        public void SetNick(string name)
        {
            setNickName(name);
        }
        public void FindGame(int players)
        {
            findGame(players);
        }
        public void Surrender()
        {
            didSurrender();
        }
        public void KilledLastMonster()
        {
            didKillLastMonster();
        }
        public void CreateTower(int id, int type, int x, int y)
        {
            didCreateTower(id, type, x, y);
        }
        public void TakeDamage(int damage, int culprit)
        {
            didTakeDamage(damage, culprit);
        }
    }
}