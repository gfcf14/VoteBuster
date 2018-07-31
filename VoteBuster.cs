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

namespace VoteBuster
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class VoteBuster : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        //buttons
        const int NUMBER_OF_BUTTONS = 6,
            START_GAME_BUTTON_INDEX = 0,
            INSTRUCTIONS_BUTTON_INDEX = 1,
            ABOUT_BUTTON_INDEX = 2,
            NEXT_LEVEL_BUTTON_INDEX = 3,
            CONTINUE_BUTTON_INDEX = 4,
            BACK_BUTTON_INDEX = 5,
            BUTTON_HEIGHT = 40,
            BUTTON_WIDTH = 88;
        Color background_color;
        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];

        //system variables
        int windowHeight = 600;
        int windowWidth = 1000;
        Random r = new Random();
        int mouseX;
        int mouseY;
        bool mouseleftpressed;
        bool mouselefthold;
        KeyboardState oldState;
        MouseState oldMState;
        int time = 120; //default time is 2 minutes (120 in seconds)
        int resulttime = 5;
        int minfactor = 60;
        int min = 0;
        int sec = 0;
        long secinterval = 1000;

        //game-state variables
        bool ingame = false;
        bool ispaused = false;
        bool inmenu = true;
        bool select = false;
        bool about = false;
        bool inst = false;
        bool nxtlvl = false;
        bool final = false;
        bool terminate = false;
        bool results = false;
        bool lvlfinis = false;

        //game variables         
        float timecounter = 0.0F;
        long millcounter = 0;
        SpriteFont diagFont;
        string diagnosticText = "diagnostic text";
        Vector2 fontPosition;
        int levelnumber = 1;
        bool passlevel = false;

        //player variables
        Color playerColor;
        string playerCandidate = "";
        string[] bluestates = new string[] { "washington", "newjersey", "michigan", "pennsylvania", "illinois", "ohio", "florida", "newyork", "california", "washingtondc" };
        string[] redstates = new string[] { "arizona", "indiana", "virginia", "northcarolina", "georgia", "ohio", "florida", "missouri", "texas", "washingtondc" };
        string[] bluelevels = new string[10];
        string[] redlevels = new string[10];
        string[] lvlconditions = new string[10];
        int obamalevel = 1;
        int romneylevel = 1;

        //vote variables
        int voteaddup = 1;
        int obamavotes = 0;
        int romneyvotes = 0;
        int obamadead = 0;
        int romneydead = 0;
        int obama2nd = 0;
        int romney2nd = 0;
        int maxvotes; //easy 10, medium 15, hard 20, super 25
        static float speed; //easy 3, medium 4, hard 5
        //BY LEVEL: 1-3(EE), 4-6(MM), 7-9(HH), 10(HS)
        List<Vote> votes = new List<Vote>();
        Vector2 votespeed = new Vector2(speed, 0.0F);
        Vector2 fallspeed = new Vector2(0.0F, speed);
        string[] faces = new string[] { "obama", "romney" };

        //sniper variables
        Texture2D sniperSight;
        Color sniperColor;
        Vector2 sightPosition = new Vector2(0.0F, 0.0F);
        bool isshooting = false;

        //background variables
        Texture2D USflag;
        int flagThreshold = 800;
        Vector2 flagPosition;
        Texture2D statsRectangle;
        Texture2D bgtext;
        Texture2D statebg;
        Texture2D oSL;
        Rectangle oSLrect = new Rectangle(200, 150, 200, 200);
        Texture2D rSL;
        Rectangle rSLrect = new Rectangle(600, 150, 200, 200);
        SpriteFont gameFont;
        string gameStatFont = "";
        Vector2 statfont;
        Texture2D obamaface;
        Texture2D romneyface;
        Vector2 toptext;
        string stoptext;
        Vector2 midtext;
        string smidtext;
        string h1v = "";
        string h2v = "";
        string sv = "";
        string tv = "";
        string lvlc = "";
        bool disp1sthv = false;
        bool disp2ndhv = false;
        bool dispsv = false;
        bool disptv = false;
        bool disppicsab = false;
        string levelnametext = "";
        string leveldescptext = "";
        SpriteFont stateFont;
        Texture2D obamaresults;
        Texture2D romneyresults;
        Texture2D logos;
        Texture2D curtains;
        Texture2D middleline;

        //final variables
        Texture2D finalbg, finalObama, finalRomney, podium, people, GaryJ;
        Rectangle bgLocation;
        string[] fobama = { "obamaexcited", "obamasurprised", "obamalose", "obamaangry", "obamaattack" };
        string[] fromney = { "romneyexcited", "romneysurprised", "romneylose", "romneyangry", "romneyattack" };
        int candidchange = 0;
        string[] fdialogue = new string[11];
        int continuepress = 0;
        string ftext = "";
        int subs = 0;
        bool finalcontinuebok = false;
        int twentiethsecint = 50;
        int garyx = 0;
        int garyy = 400;
        int fodisp = 0, frdisp = 0;
        bool attacknow = false;
        int garyup = 0;
        string[] fgary = { "garyappeasingleft", "garyappeasingright" };
        bool fightstarted = false;
        int fightindex = 1;
        int fightcount = 0;
        int finalindex = 0;

        //sound variables and related
        bool noaudioh = false;
        bool soundEnabled = true;
        Texture2D checkaudio;
        Rectangle soundbrec;

        SoundEffect sightshot;
        SoundEffect opponentshot;
        SoundEffect ownshot;
        SoundEffect bassdrum;
        SoundEffect nextlvlsound;
        SoundEffect resultsound;
        Song introsound;

        SoundEffect drumroll;
        bool drumrollisplaying = false;

        Song backsong;
        bool backsongisplaying = false;

        SoundEffect ownin;
        SoundEffect oppin;
        SoundEffect punch;
        SoundEffect votechange;
        SoundEffect ingamestop;
        SoundEffect scream;

        public VoteBuster()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.ApplyChanges();

            oldState = Keyboard.GetState();
            oldMState = Mouse.GetState();

            diagnosticText = "diagnostic text" + "\n" + "IG: " + ingame + "; IP: " + ispaused + "; IM: " + inmenu + "; S: " + select + "; A: " +
                about + "; I: " + inst + "; NL: " + nxtlvl + "; F: " + final + "; TR: " + terminate + "; R: " + results + "; LF: " + lvlfinis;

            base.Initialize();

            /*FOR THE BUTTONS*/
            // starting x and y locations to stack buttons 
            // vertically in the middle of the screen
            //int x = Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            int y = 220 + Window.ClientBounds.Height / 2 -
                NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < NUMBER_OF_BUTTONS - 3; i++)
            {
                button_color[i] = Color.White;
                int x = (Window.ClientBounds.Width - button_texture[i].Width) / 2;
                button_rectangle[i] = new Rectangle(x, y, button_texture[i].Width, button_texture[i].Height);
                y += button_texture[i].Height;
            }

            button_rectangle[NEXT_LEVEL_BUTTON_INDEX] = new Rectangle((Window.ClientBounds.Width - button_texture[NEXT_LEVEL_BUTTON_INDEX].Width) / 2, windowHeight - 72, 278, 44);
            button_rectangle[CONTINUE_BUTTON_INDEX] = new Rectangle(windowWidth - 334, windowHeight - 72, 278, 44);
            button_rectangle[BACK_BUTTON_INDEX] = new Rectangle(56, windowHeight - 72, 278, 44);

            //IsMouseVisible = true;
            background_color = Color.CornflowerBlue;
            /*FOR THE BUTTONS*/

            bgLocation = new Rectangle(0, (windowHeight * 2), windowWidth, windowHeight);

            string lvlfileline;
            int levelconditioncounter = 0;
            System.IO.StreamReader obamalevelsfile = new System.IO.StreamReader("Content/lvlconditions.txt");
            while ((lvlfileline = obamalevelsfile.ReadLine()) != null)
            {
                lvlconditions[levelconditioncounter] = lvlfileline;
                levelconditioncounter++;
            }

            string finalline;
            int finalcounter = 0;
            System.IO.StreamReader finalfile = new System.IO.StreamReader("Content/finalwords.txt");
            while ((finalline = finalfile.ReadLine()) != null)
            {
                fdialogue[finalcounter] = finalline;
                finalcounter++;
            }

            for (int i = 0; i < fdialogue.Count(); i++)
            {
                fdialogue[i] = fdialogue[i].Replace("@", System.Environment.NewLine);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            diagFont = Content.Load<SpriteFont>("diagfont");
            fontPosition = new Vector2(0, 0);

            gameFont = Content.Load<SpriteFont>("gamefont");
            statfont = new Vector2(20, 535);
            toptext = new Vector2(300, 40);
            midtext = new Vector2(50, 430);

            stateFont = Content.Load<SpriteFont>("statelevelfont");

            RestartLevelZone();

            sniperSight = Content.Load<Texture2D>("pointer");
            sniperColor = Color.Green;

            logos = Content.Load<Texture2D>("votebusterlogo");
            curtains = Content.Load<Texture2D>("curtains");

            finalbg = Content.Load<Texture2D>("finalbg");
            finalObama = Content.Load<Texture2D>(fobama[candidchange]);
            finalRomney = Content.Load<Texture2D>(fromney[candidchange]);
            podium = Content.Load<Texture2D>("podium");
            people = Content.Load<Texture2D>("peoplenormal");
            GaryJ = Content.Load<Texture2D>("garyjohnson");

            USflag = Content.Load<Texture2D>("usflag");
            flagPosition = new Vector2(flagThreshold, 0);

            statsRectangle = Content.Load<Texture2D>("greensquare");
            middleline = Content.Load<Texture2D>("midline");

            /*FOR THE BUTTONS*/
            button_texture[START_GAME_BUTTON_INDEX] = Content.Load<Texture2D>("startgame");
            button_texture[INSTRUCTIONS_BUTTON_INDEX] = Content.Load<Texture2D>("instructions");
            button_texture[ABOUT_BUTTON_INDEX] = Content.Load<Texture2D>("about");
            button_texture[NEXT_LEVEL_BUTTON_INDEX] = Content.Load<Texture2D>("nextlevel");
            button_texture[CONTINUE_BUTTON_INDEX] = Content.Load<Texture2D>("continue");
            button_texture[BACK_BUTTON_INDEX] = Content.Load<Texture2D>("back");
            /*FOR THE BUTTONS*/

            playerColor = Color.Blue;

            oSL = Content.Load<Texture2D>("obamaselect");
            rSL = Content.Load<Texture2D>("romneyselect");

            obamaface = Content.Load<Texture2D>("obamaface");
            romneyface = Content.Load<Texture2D>("romneyface");

            checkaudio = Content.Load<Texture2D>("soundenabled");
            soundbrec = new Rectangle((windowWidth - checkaudio.Width) / 2, 550, checkaudio.Width, checkaudio.Height);

            //check for noaudiohardware exception!!
            try
            {
                sightshot = Content.Load<SoundEffect>("corkpop");
                opponentshot = Content.Load<SoundEffect>("fallfast");
                ownshot = Content.Load<SoundEffect>("buzz");
                bassdrum = Content.Load<SoundEffect>("bassdrum");
                nextlvlsound = Content.Load<SoundEffect>("nextlevelsound");
                introsound = Content.Load<Song>("votebusterintro");
                drumroll = Content.Load<SoundEffect>("longdrumroll");
                ownin = Content.Load<SoundEffect>("flop");
                oppin = Content.Load<SoundEffect>("gong");
                punch = Content.Load<SoundEffect>("punch1");
                votechange = Content.Load<SoundEffect>("zipin");
                ingamestop = Content.Load<SoundEffect>("pausesound");
                scream = Content.Load<SoundEffect>("wilhelm");

                MediaPlayer.Play(introsound);
            }
            catch (NoAudioHardwareException n)
            {
                noaudioh = true;
                soundEnabled = false;
                checkaudio = Content.Load<Texture2D>("noaudio");
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            diagnosticText = "diagnostic text" + "\n" + "IG: " + ingame + "; IP: " + ispaused + "; IM: " + inmenu + "; S: " + select + "; A: " +
                about + "; I: " + inst + "; NL: " + nxtlvl + "; F: " + final + "; TR: " + terminate + "; R: " + results + "; LF: " + lvlfinis;

            // update buttons
            ButtonUpdate();

            if (sec < 10) gameStatFont = "TIME " + min + ":0" + sec + "                           = " + obamavotes + "                      = " + romneyvotes + "                   " + "LEVEL " + levelnumber;
            else gameStatFont = "TIME " + min + ":" + sec + "                           = " + obamavotes + "                      = " + romneyvotes + "                   " + "LEVEL " + levelnumber;

            if (!ingame) sniperSight = Content.Load<Texture2D>("pointer");

            if (inmenu)
            {
                sniperColor = Color.Green;

                if (noaudioh)
                {
                    checkaudio = Content.Load<Texture2D>("noaudio");
                    try
                    {
                        sightshot = Content.Load<SoundEffect>("corkpop");
                        checkaudio = Content.Load<Texture2D>("soundenabled");
                        noaudioh = false;
                        soundEnabled = true;
                    }
                    catch (NoAudioHardwareException n) { }
                }
                else
                {
                    /*try
                    {
                        sightshot = Content.Load<SoundEffect>("corkpop");
                        checkaudio = Content.Load<Texture2D>("soundenabled");
                        noaudioh = false;
                    }
                    catch (NoAudioHardwareException n) 
                    {
                        noaudioh = true;
                    }*/

                    if (((mouseX > soundbrec.X) && (mouseX < (soundbrec.X + checkaudio.Width))) && ((mouseY > soundbrec.Y) && (mouseY < (soundbrec.Y + checkaudio.Height))) && mouseleftpressed && !mouselefthold)
                    {
                        if (soundEnabled)
                        {
                            checkaudio = Content.Load<Texture2D>("sounddisabled");
                            soundEnabled = false;
                            MediaPlayer.Stop();
                            mouseleftpressed = false;
                        }
                        else
                        {
                            checkaudio = Content.Load<Texture2D>("soundenabled");
                            soundEnabled = true;
                            sightshot.Play();
                            mouseleftpressed = false;
                        }
                    }
                }

            }

            if (select)
            {
                if (mouseleftpressed)
                {
                    if ((mouseX >= oSLrect.X && mouseX <= (oSLrect.X + oSLrect.Width)) && (mouseY >= oSLrect.Y && mouseY <= (oSLrect.Y + oSLrect.Height)))
                    {
                        playerCandidate = "obama";
                        smidtext = "Your Candidate: President Barack Obama, Democrat (Blue)";
                    }
                    if ((mouseX >= rSLrect.X && mouseX <= (rSLrect.X + rSLrect.Width)) && (mouseY >= rSLrect.Y && mouseY <= (rSLrect.Y + rSLrect.Height)))
                    {
                        playerCandidate = "romney";
                        smidtext = "Your Candidate: Governor Mitt Romney, Republican (Red)";
                    }
                }
            }

            if (results)
            {
                timecounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                millcounter = (long)timecounter;

                int displaytime = resulttime - (int)(millcounter / secinterval);
                if (displaytime == 4)
                {
                    if (soundEnabled) if (!disp1sthv) bassdrum.Play();
                    disp1sthv = true;
                }
                if (displaytime == 3)
                {
                    if (soundEnabled) if (!disp2ndhv) bassdrum.Play();
                    disp2ndhv = true;
                }
                if (displaytime == 2)
                {
                    if (soundEnabled) if (!dispsv) bassdrum.Play();
                    dispsv = true;
                }
                if (displaytime == 1)
                {
                    if (soundEnabled) if (!disptv) bassdrum.Play();
                    disptv = true;
                }
                if (displaytime == 0)
                {
                    if (!disppicsab)
                    {
                        if (soundEnabled)
                        {
                            if (passlevel) resultsound = Content.Load<SoundEffect>("winsound");
                            else resultsound = Content.Load<SoundEffect>("losesound");
                            resultsound.Play();
                        }
                    }
                    disppicsab = true;
                }
            }

            if (ingame && !ispaused)
            {
                //the two below take account of elapsed time in milliseconds
                timecounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                millcounter = (long)timecounter;

                //counts down to finish the levels
                int remainingtime = time - (int)(millcounter / secinterval);
                if (remainingtime == 0)
                {
                    ispaused = true;
                    if (!lvlfinis)
                    {
                        if (soundEnabled)
                        {
                            MediaPlayer.Stop();
                            ingamestop.Play();
                        }
                    }
                    lvlfinis = true;
                    logos = Content.Load<Texture2D>("timeup");
                }

                min = remainingtime / minfactor;
                sec = remainingtime % minfactor;
                string colon = ":";
                if (sec < 10) colon += "0";

                diagnosticText = "SP: " + speed + " - MAX: " + maxvotes + " - TIME(ms): " + millcounter + " - REALTIME: " + min + colon + sec +
                    " - OBAMA: " + obamavotes + "; ROMNEY: " + romneyvotes + " - DO: " + obamadead + "; DR: " + romneydead + "; LVL# " + levelnumber +
                    "\n" + "IG: " + ingame + "; IP: " + ispaused + "; IM: " + inmenu + "; S: " + select + "; A: " + about + "; I: " + inst + "; NL: " +
                    nxtlvl + "; F: " + final + "; TR: " + terminate + "; R: " + results + "; LF: " + lvlfinis;

                foreach (Vote v in votes)
                {
                    v.setVoteFace(Content.Load<Texture2D>(v.getMood() + v.getCandidate() + "vote"));

                    if (v.isInScreen())
                    {
                        if (!v.isShooted())
                        {
                            votespeed = new Vector2(speed, speed * v.getYDirection());
                            Vector2 newVoteVector = v.getvoteVector() + votespeed;
                            v.setvoteVector(newVoteVector);
                            v.setX(v.getX() + (int)votespeed.X);
                            v.setY(v.getY() + (int)votespeed.Y);

                            if (v.getY() < 0 || v.getY() > (windowHeight - (v.getVoteFace().Height * 2)))
                            {
                                v.setYDirection(v.getYDirection() * -1);
                            }

                            if (v.getX() > windowWidth + 100)
                            {
                                v.setScreenStatus(false);
                            }
                        }
                        else
                        {
                            fallspeed = new Vector2(0.0F, speed);
                            Vector2 newVoteVector = v.getvoteVector() + fallspeed;
                            v.setvoteVector(newVoteVector);

                            //gives score once by doing so only when the vote hasn't been scored. Once given, vote is marked as scored to avoid repeat
                            if (!v.isOut())
                            {
                                if (soundEnabled)
                                {
                                    if (playerCandidate.Equals(v.getCandidate())) ownshot.Play();
                                    else opponentshot.Play();
                                }
                                if (v.getCandidate().Equals("obama")) obamadead++;
                                else romneydead++;

                                v.setOutVote(true);
                            }

                            v.setY(v.getY() + (int)fallspeed.Y);
                            if (v.getY() > windowHeight)
                            {
                                v.setScreenStatus(false);
                                v.setShot(false);
                            }
                        }
                    }
                    else
                    {
                        v.setX((0 - v.getVoteFace().Width) * (r.Next(2, 11) / 2));
                        v.setY(r.Next(0, 9) * 50);
                        v.setMood("");
                        string newcandidate = faces[r.Next(0, 2)];
                        v.setVoteFace(Content.Load<Texture2D>("" + newcandidate + "vote"));
                        v.setCandidate(newcandidate);
                        Vector2 dummyvector = new Vector2(v.getX(), v.getY());
                        v.setvoteVector(dummyvector);
                        v.setScreenStatus(true);
                        v.setOutVote(false);
                        v.setFree(false);
                        v.setVoteDecided(false);

                        if (levelnumber == 1 || levelnumber == 4 || levelnumber == 7 || levelnumber == 10)
                        {
                            int directionholder = r.Next(0, 5);
                            if (directionholder == 0) //25% of the votes will go diagonal
                            {
                                int[] upordown = { -1, 1 };
                                v.setYDirection(upordown[r.Next(0, 2)]);
                            }
                            else v.setYDirection(0);
                        }
                        if (levelnumber == 3 || levelnumber == 6 || levelnumber == 9)
                        {
                            int directionholder = r.Next(0, 5);
                            if (directionholder != 0) //75% of the votes will go diagonal
                            {
                                int[] upordown = { -1, 1 };
                                v.setYDirection(upordown[r.Next(0, 2)]);
                            }
                            else v.setYDirection(0);
                        }
                        if (levelnumber == 2 || levelnumber == 5 || levelnumber == 8)
                        {
                            int directionholder = r.Next(0, 5);
                            if (directionholder == 0 || directionholder == 1) //50% of the votes will go diagonal
                            {
                                int[] upordown = { -1, 1 };
                                v.setYDirection(upordown[r.Next(0, 2)]);
                            }
                            else v.setYDirection(0);
                        }

                    }

                }

            }


            UpdateKeyboard();
            UpdateMouse();

            if (final)
            {
                if (bgLocation.Y != 0)
                {
                    bgLocation = new Rectangle(bgLocation.X, bgLocation.Y - 5, bgLocation.Width, bgLocation.Height);
                }
                else
                {
                    if (soundEnabled)
                    {
                        if (!drumrollisplaying)
                        {
                            drumroll.Play();
                            drumrollisplaying = true;
                        }
                    }
                    if (continuepress == 0) finalcontinuebok = true;
                    else
                    {
                        if (continuepress <= fdialogue.Count())
                        {
                            timecounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                            int rawtime = (int)timecounter;
                            if (rawtime % twentiethsecint == 0 && !finalcontinuebok)
                            {
                                ftext += fdialogue[continuepress - 1].Substring(subs, 1);
                                subs++;
                                if (subs == fdialogue[continuepress - 1].Count())
                                {
                                    finalcontinuebok = true;
                                    subs = 0;
                                }
                            }
                            if (timecounter > 4000) timecounter = 0;
                        }
                        else
                        {
                            if (soundEnabled)
                            {
                                if (!backsongisplaying)
                                {
                                    MediaPlayer.Play(backsong);
                                    backsongisplaying = true;
                                }
                            }
                            if (garyy != 200 && !fightstarted) garyy--;
                            else
                            {
                                if (candidchange < 3)
                                {
                                    candidchange++;
                                    finalObama = Content.Load<Texture2D>(fobama[candidchange]);
                                    finalRomney = Content.Load<Texture2D>(fromney[candidchange]);
                                    people = Content.Load<Texture2D>("peopledoubt");
                                    timecounter = 0;
                                }
                                else
                                {
                                    timecounter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                    int rawtime = (int)timecounter;
                                    if (rawtime == 3000 && !attacknow)
                                    {
                                        candidchange++;
                                        finalObama = Content.Load<Texture2D>(fobama[candidchange]);
                                        finalRomney = Content.Load<Texture2D>(fromney[candidchange]);
                                        attacknow = true;
                                        timecounter = 0;
                                    }
                                    if (attacknow && !fightstarted)
                                    {
                                        fodisp++;
                                        frdisp--;
                                        if (fodisp == 50/* || frdisp == -50*/)
                                        {
                                            fightstarted = true;
                                            timecounter = 0;
                                            garyy -= 100;
                                            garyx -= 70;
                                            GaryJ = Content.Load<Texture2D>("fight" + fightindex);
                                            if (soundEnabled) scream.Play();
                                        }

                                        if (rawtime % 250 == 0)
                                        {
                                            GaryJ = Content.Load<Texture2D>(fgary[(garyup % 2)]);
                                            garyup++;
                                        }
                                    }
                                    if (fightstarted && !terminate)
                                    {
                                        if (rawtime % 250 == 0)
                                        {
                                            if (Convert.ToBoolean(r.Next(0, 2))) if (soundEnabled) punch.Play();
                                            fightcount++;
                                            fightindex++;
                                            if (fightindex == 4) fightindex = 1;
                                            GaryJ = Content.Load<Texture2D>("fight" + fightindex);
                                            if (fightcount == 20)
                                            {
                                                terminate = true;
                                                //logos = Content.Load<Texture2D>("final" + finalindex);
                                            }
                                        }
                                    }
                                    if (terminate)
                                    {
                                        if (rawtime % 2000 == 0)
                                        {
                                            if (finalindex < 5) finalindex++;
                                            logos = Content.Load<Texture2D>("final" + finalindex);
                                        }
                                    }
                                    if (timecounter > 5000) timecounter = 0;
                                }

                            }
                        }
                    }

                }
            }
            base.Update(gameTime);
        }

        private void UpdateKeyboard()
        {

            KeyboardState newState = Keyboard.GetState(); //Gets a newer keyboard state than that of oldState

            if (newState.IsKeyDown(Keys.Space))
            { //if the SPACE key has been pressed

                if (!oldState.IsKeyDown(Keys.Space))
                { //if oldState doesn't have it as pressed, then it can be assumed SPACE was just pressed
                    //Do code for key press here
                    if (!ispaused)
                    {
                        if (!lvlfinis)
                        {
                            if (ingame)
                            {
                                ispaused = true;
                                if (soundEnabled)
                                {
                                    MediaPlayer.Pause();
                                    ingamestop.Play();
                                }
                            }

                        }
                    }
                    else
                    {
                        if (ingame)
                        {
                            if (!lvlfinis) ispaused = false;
                            if (soundEnabled)
                            {
                                MediaPlayer.Resume();
                            }
                        }
                    }
                }
            }
            else if (oldState.IsKeyDown(Keys.Space))
            { //if newState hasn't pressed the SPACE and oldState has, then the SPACE was released
                //Do code for key release here

            }

            oldState = newState; //update oldState to be able to do this over and over
        }

        protected void UpdateMouse()
        {
            MouseState newMState = Mouse.GetState();
            mouseX = newMState.X;
            mouseY = newMState.Y;

            if (newMState.LeftButton == ButtonState.Pressed)
            {
                mouseleftpressed = true;
                if (soundEnabled)
                {
                    if (mouseleftpressed && ingame && !mouselefthold && isshooting && !lvlfinis)
                    {
                        sightshot.Play();
                    }
                }
                if (oldMState.LeftButton != ButtonState.Pressed) isshooting = true;
                else mouselefthold = true;
            }
            else if (newMState.LeftButton == ButtonState.Released)
            {
                mouseleftpressed = false;
                isshooting = false;
                mouselefthold = false;
            }

            oldMState = newMState;

        }

        public void RestartLevelZone()
        {
            timecounter = 0;
            obamavotes = 0;
            obama2nd = 0;
            obamadead = 0;
            romneyvotes = 0;
            romney2nd = 0;
            romneydead = 0;
            votes.Clear();
            for (int i = 0; i < maxvotes; i++)
            {
                if (levelnumber == 6 || levelnumber == 7 || levelnumber == 10)
                {
                    Vote v = new Vote(Content.Load<Texture2D>(faces[0] + "vote"), faces[0], "", 0, 0, new Vector2(0.0F, 0.0F), false, false, false, false, false, 0);
                    votes.Add(v);
                }
                else
                {
                    Vote b = new Vote(Content.Load<Texture2D>(faces[0] + "vote"), faces[0], "", 0, 0, new Vector2(0.0F, 0.0F), false, false, false, false, true, 0);
                    votes.Add(b);
                }
            }

            //set vote speed and incidence by level number:
            if (levelnumber <= 3)
            {
                speed = 3.0F;
                maxvotes = 10;
            }
            else if (levelnumber > 3 && levelnumber <= 6)
            {
                speed = 4.0F;
                maxvotes = 15;
            }
            else if (levelnumber > 6 && levelnumber <= 9)
            {
                speed = 5.0F;
                maxvotes = 20;
            }
            else if (levelnumber == 10)
            {
                speed = 5;
                maxvotes = 25;
            }

            if (levelnumber != 10)
            {
                if (playerCandidate.Equals("obama"))
                {
                    if (!noaudioh)
                    {
                        try
                        {
                            backsong = Content.Load<Song>(redstates[levelnumber - 1] + "song");
                        }
                        catch (InvalidOperationException i) { }
                    }
                }
                else
                {
                    if (!noaudioh)
                    {
                        try
                        {
                            backsong = Content.Load<Song>(bluestates[levelnumber - 1] + "song");
                        }
                        catch (InvalidOperationException i) { }
                    }
                }
            }
            else
            {
                backsong = Content.Load<Song>("hailtothechief");
            }

        }

        public void prepareNextResults()
        {
            disp1sthv = false;
            disp2ndhv = false;
            dispsv = false;
            disptv = false;
            disppicsab = false;
        }

        public void LoadPartyLevelText(string[] partylevels)
        {
            string fileline;
            int levelcounter = 0;
            System.IO.StreamReader obamalevelsfile = new System.IO.StreamReader("Content/" + playerCandidate + "levels.txt");
            while ((fileline = obamalevelsfile.ReadLine()) != null)
            {
                partylevels[levelcounter] = fileline;
                levelcounter++;
            }

        }

        public void PrepareTextbyLevel(string[] partylevels)
        {
            string[] fornextlvl = partylevels[levelnumber - 1].Split(';');
            levelnametext = fornextlvl[0];
            leveldescptext = fornextlvl[1] + "@@2ND HALF VOTE PREDICTIONS:@@" + lvlconditions[levelnumber - 1];

            lvlc = "@@2ND HALF VOTE PREDICTIONS:@@" + lvlconditions[levelnumber - 1];

            //this replaces every @ in the text with a new line
            leveldescptext = leveldescptext.Replace("@", System.Environment.NewLine);
            lvlc = lvlc.Replace("@", System.Environment.NewLine);
        }

        // determine state and color of button
        void ButtonUpdate()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                //if mouse is within boundaries of a button
                if ((mouseX >= button_rectangle[i].X && mouseX <= (button_rectangle[i].X + button_rectangle[i].Width)) && (mouseY >= button_rectangle[i].Y && mouseY <= (button_rectangle[i].Y + button_rectangle[i].Height)))
                {
                    if (!mouseleftpressed) button_color[i] = Color.LightGreen;
                    else
                    {
                        button_color[i] = Color.DarkGreen;
                        Button_Action(i);
                    }
                }
                else button_color[i] = Color.White;
            }
        }


        // Logic for each button click goes here
        void Button_Action(int i)
        {
            //take action corresponding to which button was clicked
            switch (i)
            {
                case START_GAME_BUTTON_INDEX:
                    if (inmenu) //only works if inmenu is true, to avoid clicking an invisible button
                    {
                        MediaPlayer.Stop();
                        if (soundEnabled) sightshot.Play();
                        select = true;
                        mouseleftpressed = false; //to avoid auto-selecting the candidate to the left
                        playerCandidate = "";
                        inmenu = false;
                        stoptext = "PLEASE SELECT YOUR CANDIDATE";
                        smidtext = "Your Candidate: ";
                    }
                    break;
                case INSTRUCTIONS_BUTTON_INDEX:
                    if (inmenu)
                    {
                        if (soundEnabled) sightshot.Play();
                        inmenu = false;
                        inst = true;
                        bgtext = Content.Load<Texture2D>("instructionsbg");
                    }
                    break;
                case ABOUT_BUTTON_INDEX:
                    if (inmenu)
                    {
                        if (soundEnabled) sightshot.Play();
                        inmenu = false;
                        about = true;
                        bgtext = Content.Load<Texture2D>("aboutbg");
                    }
                    break;
                case NEXT_LEVEL_BUTTON_INDEX:
                    if (nxtlvl)
                    {
                        if (soundEnabled) sightshot.Play();
                        nxtlvl = false;
                        RestartLevelZone();
                        ingame = true;
                        statsRectangle = Content.Load<Texture2D>("statsbar");
                        logos = Content.Load<Texture2D>("pause");
                        lvlfinis = false;
                        ispaused = false;

                        if (soundEnabled)
                        {
                            MediaPlayer.IsRepeating = true;
                            MediaPlayer.Play(backsong);
                        }
                    }

                    break;
                case CONTINUE_BUTTON_INDEX:
                    if (final)
                    {
                        if (soundEnabled)
                        {
                            if (finalcontinuebok) sightshot.Play();
                        }
                        if (finalcontinuebok)
                        {
                            continuepress++;
                            if (continuepress == 10 || continuepress == 11)
                            {
                                if (continuepress == 10) people = Content.Load<Texture2D>("peoplesurprised");
                                candidchange++;
                            }
                            finalcontinuebok = false;
                            ftext = "";
                            timecounter = 0;
                            finalObama = Content.Load<Texture2D>(fobama[candidchange]);
                            finalRomney = Content.Load<Texture2D>(fromney[candidchange]);
                        }
                    }
                    if (select)
                    {
                        if (soundEnabled) sightshot.Play();
                        if (!playerCandidate.Equals(""))
                        {
                            select = false;
                            if (playerCandidate.Equals("obama"))
                            {
                                playerColor = Color.Blue;
                                levelnumber = obamalevel;
                                sniperColor = playerColor;
                                LoadPartyLevelText(bluelevels);
                                PrepareTextbyLevel(bluelevels);
                            }
                            else
                            {
                                playerColor = Color.Red;
                                levelnumber = romneylevel;
                                sniperColor = playerColor;
                                LoadPartyLevelText(redlevels);
                                PrepareTextbyLevel(redlevels);
                            }
                            nxtlvl = true;
                            if (soundEnabled) nextlvlsound.Play();

                            stoptext = "LEVEL " + levelnumber + ": ";
                            toptext = new Vector2(300, 80);
                            bgtext = Content.Load<Texture2D>("nxtlvlbg");
                            if (playerCandidate.Equals("obama")) statebg = Content.Load<Texture2D>(redstates[levelnumber - 1]);
                            else statebg = Content.Load<Texture2D>(bluestates[levelnumber - 1]);
                        }
                    }
                    if (results)
                    {
                        if (soundEnabled && disppicsab) sightshot.Play();
                        if (disppicsab)
                        {
                            prepareNextResults();
                            if (passlevel)
                            {
                                if (playerCandidate.Equals("obama"))
                                {
                                    obamalevel++;
                                    levelnumber = obamalevel;
                                }
                                else
                                {
                                    romneylevel++;
                                    levelnumber = romneylevel;
                                }

                                passlevel = false;
                            }
                            results = false;
                            stoptext = "LEVEL " + levelnumber;
                            toptext = new Vector2(300, 80);

                            if (levelnumber <= 10)
                            {
                                nxtlvl = true;
                                if (soundEnabled) nextlvlsound.Play();

                                if (playerCandidate.Equals("obama"))
                                {
                                    statebg = Content.Load<Texture2D>(redstates[levelnumber - 1]);
                                    PrepareTextbyLevel(bluelevels);
                                }
                                else
                                {
                                    statebg = Content.Load<Texture2D>(bluestates[levelnumber - 1]);
                                    PrepareTextbyLevel(redlevels);
                                }
                            }
                            else
                            {
                                final = true;
                                backsong = Content.Load<Song>("hailtothechief");
                                timecounter = 0;
                            }
                        }
                    }
                    if (ingame && lvlfinis)
                    {
                        if (soundEnabled) sightshot.Play();
                        ingame = false;
                        lvlfinis = false;
                        timecounter = 0;
                        results = true;
                        stoptext = "LEVEL " + levelnumber + " RESULTS:\n\n" +
                                   "VOTES           OBAMA       ROMNEY\n\n" +
                                   "1ST HALF\n\n" +
                                   "2ND HALF\n\n" +
                                   "SHOT\n\n" +
                                   "_____________________________\n\n" +
                                   "TOTAL";
                        toptext = new Vector2(0, 50);

                        int totalobama = 0, totalromney = 0;
                        if (playerCandidate.Equals("obama"))
                        {

                            if (levelnumber == 1)
                            {
                                obama2nd = 10;
                                romney2nd = 55;
                                totalobama = (obamavotes + obama2nd - obamadead);
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 2)
                            {
                                obama2nd = 10;
                                romney2nd = 70;
                                totalobama = (obamavotes + obama2nd - obamadead);
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 3)
                            {
                                obama2nd = 10;
                                romney2nd = 80;
                                totalobama = (obamavotes + obama2nd - obamadead);
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 4)
                            {
                                obama2nd = 20;
                                romney2nd = 80;
                                totalobama = (obamavotes + obama2nd - (obamadead * 2));
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 5)
                            {
                                romney2nd = 100;
                                totalobama = (obamavotes + obama2nd - obamadead);
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 6)
                            {
                                obama2nd = 10;
                                romney2nd = 20;
                                totalobama = (obamavotes + obama2nd - obamadead);
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 7)
                            {
                                romney2nd = 10;
                                totalobama = (obamavotes + obama2nd - (obamadead * 2));
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 8)
                            {
                                romney2nd = 100;
                                totalobama = (obamavotes + obama2nd - (obamadead * 3));
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 9)
                            {
                                romney2nd = 5;
                                totalobama = (obamavotes + obama2nd - (obamadead * 4));
                                totalromney = (romneyvotes + romney2nd);
                            }
                            if (levelnumber == 10)
                            {
                                romney2nd = obamadead;
                                totalobama = (obamavotes + obama2nd - obamadead);
                                totalromney = (romneyvotes + romney2nd);
                            }

                            tv = totalobama + "                    " + totalromney;

                            if (totalobama > totalromney)
                            {
                                obamaresults = Content.Load<Texture2D>("obamawin");
                                romneyresults = Content.Load<Texture2D>("romneylose");
                                passlevel = true;
                                logos = Content.Load<Texture2D>("win");
                            }
                            else
                            {
                                obamaresults = Content.Load<Texture2D>("obamalose");
                                romneyresults = Content.Load<Texture2D>("romneywin");
                                logos = Content.Load<Texture2D>("lose");
                            }
                        }
                        else
                        {
                            if (levelnumber == 1)
                            {
                                romney2nd = 10;
                                obama2nd = 55;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - romneydead);
                            }
                            if (levelnumber == 2)
                            {
                                romney2nd = 10;
                                obama2nd = 70;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - romneydead);
                            }
                            if (levelnumber == 3)
                            {
                                romney2nd = 10;
                                obama2nd = 80;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - romneydead);
                            }
                            if (levelnumber == 4)
                            {
                                romney2nd = 10;
                                obama2nd = 20;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - (romneydead * 2));
                            }
                            if (levelnumber == 5)
                            {
                                obama2nd = 100;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - romneydead);
                            }
                            if (levelnumber == 6)
                            {
                                romney2nd = 10;
                                obama2nd = 20;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - romneydead);
                            }
                            if (levelnumber == 7)
                            {
                                obama2nd = 10;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - (romneydead * 2));
                            }
                            if (levelnumber == 8)
                            {
                                obama2nd = 100;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - (romneydead * 3));
                            }
                            if (levelnumber == 9)
                            {
                                obama2nd = 5;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - (romneydead * 4));
                            }
                            if (levelnumber == 10)
                            {
                                obama2nd = romneydead;
                                totalobama = (obamavotes + obama2nd);
                                totalromney = (romneyvotes + romney2nd - romneydead);
                            }

                            tv = totalobama + "                    " + totalromney;

                            if (totalromney > totalobama)
                            {
                                romneyresults = Content.Load<Texture2D>("romneywin");
                                obamaresults = Content.Load<Texture2D>("obamalose");
                                passlevel = true;
                                logos = Content.Load<Texture2D>("win");
                            }
                            else
                            {
                                romneyresults = Content.Load<Texture2D>("romneylose");
                                obamaresults = Content.Load<Texture2D>("obamawin");
                                logos = Content.Load<Texture2D>("lose");
                            }
                        }

                        h1v = obamavotes + "                    " + romneyvotes;
                        h2v = obama2nd + "                    " + romney2nd;
                        sv = obamadead + "                    " + romneydead;
                    }

                    break;
                case BACK_BUTTON_INDEX:
                    if (select)
                    {
                        if (soundEnabled) sightshot.Play();
                        select = false;
                        inmenu = true;
                        statsRectangle = Content.Load<Texture2D>("greensquare");
                        logos = Content.Load<Texture2D>("votebusterlogo");
                    }
                    if (nxtlvl)
                    {
                        if (soundEnabled) sightshot.Play();
                        nxtlvl = false;
                        inmenu = true;
                        statsRectangle = Content.Load<Texture2D>("greensquare");
                        logos = Content.Load<Texture2D>("votebusterlogo");
                    }
                    if (about)
                    {
                        if (soundEnabled) sightshot.Play();
                        about = false;
                        inmenu = true;
                        logos = Content.Load<Texture2D>("votebusterlogo");
                    }
                    if (inst)
                    {
                        if (soundEnabled) sightshot.Play();
                        inst = false;
                        inmenu = true;
                        logos = Content.Load<Texture2D>("votebusterlogo");
                    }
                    if (terminate && finalindex == 5)
                    {
                        if (soundEnabled) sightshot.Play();
                        final = false;
                        terminate = false;
                        if (playerCandidate.Equals("obama")) obamalevel = 1;
                        else romneylevel = 1;
                        inmenu = true;
                        logos = Content.Load<Texture2D>("votebusterlogo");

                        bgLocation = new Rectangle(0, (windowHeight * 2), windowWidth, windowHeight);
                        candidchange = 0;
                        continuepress = 0;
                        ftext = "";
                        subs = 0;
                        finalcontinuebok = false;
                        garyx = 0;
                        garyy = 400;
                        fodisp = 0;
                        frdisp = 0;
                        attacknow = false;
                        garyup = 0;
                        fightstarted = false;
                        fightindex = 1;
                        fightcount = 0;
                        finalindex = 0;
                        people = Content.Load<Texture2D>("peoplenormal");
                        finalObama = Content.Load<Texture2D>(fobama[candidchange]);
                        finalRomney = Content.Load<Texture2D>(fromney[candidchange]);
                        GaryJ = Content.Load<Texture2D>("garyjohnson");
                        MediaPlayer.Stop();
                        backsongisplaying = false;
                        MediaPlayer.IsRepeating = false;
                        if (soundEnabled) MediaPlayer.Play(introsound);
                        statsRectangle = Content.Load<Texture2D>("greensquare");
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(background_color);

            // TODO: Add your drawing code here
            spriteBatch.Begin();


            if (!ingame && !final) spriteBatch.Draw(curtains, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

            if (inmenu)
            {
                spriteBatch.Draw(logos, new Rectangle(((windowWidth - logos.Width) / 2), 90, logos.Width, logos.Height), Color.White);
                /*FOR THE BUTTONS*/
                for (int i = 0; i < NUMBER_OF_BUTTONS - 3; i++) spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);

                spriteBatch.Draw(checkaudio, soundbrec, Color.White);
            }

            if (inst)
            {
                spriteBatch.Draw(bgtext, new Rectangle(0, 0, bgtext.Width, bgtext.Height), Color.White);
                spriteBatch.Draw(button_texture[BACK_BUTTON_INDEX], button_rectangle[BACK_BUTTON_INDEX], button_color[BACK_BUTTON_INDEX]);
            }

            if (about)
            {
                spriteBatch.Draw(bgtext, new Rectangle(0, 0, bgtext.Width, bgtext.Height), Color.White);
                spriteBatch.Draw(button_texture[BACK_BUTTON_INDEX], button_rectangle[BACK_BUTTON_INDEX], button_color[BACK_BUTTON_INDEX]);
            }

            if (select)
            {
                spriteBatch.DrawString(gameFont, stoptext, toptext, Color.DarkGreen);
                if (playerCandidate.Equals("obama")) spriteBatch.Draw(statsRectangle, new Rectangle(150, 100, 300, 300), new Color(0, 200, 0));
                if (playerCandidate.Equals("romney")) spriteBatch.Draw(statsRectangle, new Rectangle(550, 100, 300, 300), new Color(0, 200, 0));
                spriteBatch.Draw(oSL, oSLrect, Color.White);
                spriteBatch.Draw(rSL, rSLrect, Color.White);
                spriteBatch.DrawString(gameFont, smidtext, midtext, Color.DarkGreen);
                spriteBatch.Draw(button_texture[BACK_BUTTON_INDEX], button_rectangle[BACK_BUTTON_INDEX], button_color[BACK_BUTTON_INDEX]);
                spriteBatch.Draw(button_texture[CONTINUE_BUTTON_INDEX], button_rectangle[CONTINUE_BUTTON_INDEX], button_color[CONTINUE_BUTTON_INDEX]);
            }

            if (nxtlvl)
            {
                spriteBatch.Draw(bgtext, new Rectangle(((windowWidth - bgtext.Width) / 2), 50, bgtext.Width, bgtext.Height), playerColor);
                spriteBatch.Draw(statebg, new Rectangle(((windowWidth - bgtext.Width) / 2), 150, statebg.Width, statebg.Height), Color.White);
                spriteBatch.DrawString(gameFont, stoptext, toptext, Color.White);
                spriteBatch.DrawString(gameFont, levelnametext, new Vector2(toptext.X + 150, toptext.Y), Color.White);
                spriteBatch.DrawString(stateFont, leveldescptext, new Vector2(toptext.X + 210, toptext.Y + 80), Color.White);
                spriteBatch.Draw(button_texture[BACK_BUTTON_INDEX], button_rectangle[BACK_BUTTON_INDEX], button_color[BACK_BUTTON_INDEX]);
                spriteBatch.Draw(button_texture[NEXT_LEVEL_BUTTON_INDEX], button_rectangle[NEXT_LEVEL_BUTTON_INDEX], button_color[NEXT_LEVEL_BUTTON_INDEX]);
            }

            if (ingame)
            {
                spriteBatch.Draw(USflag, flagPosition, Color.White);

                if (levelnumber == 6 || levelnumber == 7 || levelnumber == 10) spriteBatch.Draw(middleline, new Rectangle(490, 0, 10, windowHeight), new Color(100, 0, 100, 1));

                foreach (Vote v in votes)
                {
                    if (!v.isShot(mouseX, mouseY))
                    {
                        //if statement for levels 6, 7, and 10
                        if ((levelnumber == 6 || levelnumber == 7 || levelnumber == 10) && (v.getX() > (windowWidth / 2) - 50) && !v.isVoteDecided())
                        {
                            string newcandidate = faces[r.Next(0, 2)];
                            string prevcandidate = v.getCandidate();

                            if (!prevcandidate.Equals(newcandidate))
                            {
                                if (soundEnabled)
                                {
                                    votechange.Play();
                                }
                            }

                            v.setVoteFace(Content.Load<Texture2D>("" + newcandidate + "vote"));
                            v.setCandidate(newcandidate);
                            v.setVoteDecided(true);
                        }

                        //make if to check if vote passed to safe zone
                        if (v.getX() > flagThreshold && !(v.isFree()))
                        {
                            if (v.getCandidate().Equals("obama")) obamavotes += voteaddup;
                            else romneyvotes += voteaddup;

                            if (soundEnabled)
                            {
                                if (playerCandidate.Equals(v.getCandidate())) ownin.Play();
                                else oppin.Play();
                            }

                            v.setFree(true);
                            v.setMood("happy");
                            v.setVoteFace(Content.Load<Texture2D>(v.getMood() + v.getCandidate() + "vote"));
                        }
                    }
                    else
                    {
                        if (!(v.isFree()) && isshooting && !mouselefthold && !ispaused) v.setShot(true);
                    }

                    if (v.isShooted())
                    {
                        v.setMood("sad");
                        v.setVoteFace(Content.Load<Texture2D>(v.getMood() + v.getCandidate() + "vote"));
                    }

                    spriteBatch.Draw(v.getVoteFace(), v.getvoteVector(), Color.White);
                }

                spriteBatch.Draw(statsRectangle, new Rectangle(0, windowHeight - 100, windowWidth, 100), new Color(0, 200, 0));
                spriteBatch.DrawString(gameFont, gameStatFont, statfont, Color.White);
                spriteBatch.Draw(obamaface, new Rectangle(220, 500, obamaface.Width, obamaface.Height), Color.White);
                spriteBatch.Draw(romneyface, new Rectangle(420, 500, romneyface.Width, romneyface.Height), Color.White);

                if (lvlfinis)
                {
                    spriteBatch.Draw(logos, new Rectangle(((windowWidth - logos.Width) / 2), ((windowHeight - logos.Height) / 2) - 50, logos.Width, logos.Height), Color.White);
                    spriteBatch.Draw(button_texture[CONTINUE_BUTTON_INDEX], button_rectangle[CONTINUE_BUTTON_INDEX], button_color[CONTINUE_BUTTON_INDEX]);
                }
            }

            if (ispaused)
            {
                if (ingame) spriteBatch.Draw(logos, new Rectangle(((windowWidth - logos.Width) / 2), ((windowHeight - logos.Height) / 2) - 50, logos.Width, logos.Height), Color.White);
            }

            if (results)
            {
                if (disp1sthv) spriteBatch.DrawString(gameFont, h1v, new Vector2(180, 165), Color.Blue);
                if (disp2ndhv) spriteBatch.DrawString(gameFont, h2v, new Vector2(180, 220), Color.Blue);
                if (dispsv)
                {
                    spriteBatch.DrawString(gameFont, sv, new Vector2(180, 285), Color.Red);
                    spriteBatch.DrawString(gameFont, lvlc, new Vector2(50, 385), Color.Red);
                }
                if (disptv) spriteBatch.DrawString(gameFont, tv, new Vector2(180, 395), Color.White);
                spriteBatch.DrawString(gameFont, stoptext, toptext, Color.DarkGreen);
                if (disppicsab)
                {
                    spriteBatch.Draw(logos, new Rectangle(450, 0, logos.Width, logos.Height), Color.White);
                    spriteBatch.Draw(obamaresults, new Rectangle(500, 165, obamaresults.Width, obamaresults.Height), Color.White);
                    spriteBatch.Draw(romneyresults, new Rectangle(700, 165, romneyresults.Width, romneyresults.Height), Color.White);
                    spriteBatch.Draw(button_texture[CONTINUE_BUTTON_INDEX], button_rectangle[CONTINUE_BUTTON_INDEX], button_color[CONTINUE_BUTTON_INDEX]);
                }
            }

            if (final)
            {
                spriteBatch.Draw(finalbg, bgLocation, Color.White);
                spriteBatch.Draw(people, new Rectangle(bgLocation.X, bgLocation.Y + 85, bgLocation.Width, bgLocation.Height), Color.White);
                if (continuepress > fdialogue.Count()) spriteBatch.Draw(GaryJ, new Rectangle(400 + garyx, bgLocation.Y + garyy, GaryJ.Width, GaryJ.Height), Color.White);
                if (continuepress != 0)
                {
                    spriteBatch.Draw(statsRectangle, new Rectangle(0, windowHeight - 101, windowWidth, 100), new Color(0, 255, 0));
                    spriteBatch.DrawString(gameFont, ftext, new Vector2(5, windowHeight - 100), Color.White);
                }
                spriteBatch.Draw(podium, bgLocation, Color.White);
                if (!fightstarted) spriteBatch.Draw(finalObama, new Rectangle(220 + fodisp, bgLocation.Y + 200, finalObama.Width, finalObama.Height), Color.White);
                if (!fightstarted) spriteBatch.Draw(finalRomney, new Rectangle(580 + frdisp, bgLocation.Y + 200, finalRomney.Width, finalRomney.Height), Color.White);
                if (finalcontinuebok) spriteBatch.Draw(button_texture[CONTINUE_BUTTON_INDEX], button_rectangle[CONTINUE_BUTTON_INDEX], button_color[CONTINUE_BUTTON_INDEX]);
                if (terminate)
                {
                    if (finalindex > 0) spriteBatch.Draw(logos, new Rectangle(0, 0, logos.Width, logos.Height), Color.White);
                    if (finalindex == 5) spriteBatch.Draw(button_texture[BACK_BUTTON_INDEX], button_rectangle[BACK_BUTTON_INDEX], button_color[BACK_BUTTON_INDEX]);
                }
            }

            if (mouseleftpressed)
            {
                if (ingame) sniperSight = Content.Load<Texture2D>("snipershot");
            }
            else
            {
                if (ingame) sniperSight = Content.Load<Texture2D>("snipersight");
            }

            /*FOR SECTION TEST ONLY!!*/
            //spriteBatch.DrawString(diagFont, diagnosticText, fontPosition, Color.Green, 0, new Vector2(0.0F, 0.0F), 1.0F, SpriteEffects.None, 0.5F);

            if (ingame)
            {
                sightPosition = new Vector2(mouseX - (sniperSight.Width / 2), mouseY - (sniperSight.Height / 2));
                spriteBatch.Draw(sniperSight, sightPosition, Color.White);
            }
            else
            {
                sightPosition = new Vector2(mouseX, mouseY);
                spriteBatch.Draw(sniperSight, sightPosition, sniperColor);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
