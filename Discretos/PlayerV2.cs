using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Plateform_2D_v9.NetWorkEngine_3._0.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    /// <summary>
    /// Déconseillé
    /// </summary>
    public class PlayerV2 : Actor
    {

        private Vector2 OldPos;

        private const float JumpImpulsion = 6f;
        private const float LowerJumpImpulsion = 4.65f; //4.6f
        private const float JumpControler = 1.03f;

        private new Vector2 Velocity;

        private int width, height;

        private new readonly float Gravity = 0.35f;   //0.3f

        private bool isJump;

        private int num = 0;

        private Animation Walk;
        private Animation BasicAttack;
        private Animation SquishPlayer;

        public bool Right = false;
        public bool Left = false;
        public bool Up = false;
        public bool Down = false;

        public bool BlockMove = false;          /// For Collision correction

        public bool isLower;
        public bool wasLower;
        public bool isDead;
        public bool isRight = true;
        public bool goRight;
        public bool goLeft;
        public bool isAttack;

        public int Life = 5;
        private bool hitEnemy = false;
        private int invincibletime = 60;  //1s
        private int time = 0;

        private Solid TileRided;

        private int AttackNum = 0;

        public Vector2 PosOfDebug;

        public bool infiniJump = false;

        private Object collectedKey;
        private List<Object> collectedObjects;


        public int clientID;

        public bool LeftKey;
        public bool RightKey;
        public bool UpKey;
        public bool DownKey;
        public bool AttackKey;

        public bool LeftKey2;
        public bool RightKey2;
        public bool UpKey2;
        public bool DownKey2;
        public bool AttackKey2;

        public bool LeftOldKey;
        public bool RightOldKey;
        public bool UpOldKey;
        public bool DownOldKey;
        public bool AttackOldKey;

        public Rectangle AttackRect;

        public bool SpringTouched;

        public bool OnLadder;
        public bool OnTopOfLadder;


        public PlayerV2(Vector2 Position, int PlayerID)
            : base(new Vector2(Position.X, Position.Y))
        {
            this.actorType = ActorType.Player;

            this.ID = PlayerID;
            clientID = (int)Client.playerID;

            Walk = new Animation(Main.Player, 9, 1, 0.03f, 1);
            BasicAttack = new Animation(Main.Player_Basic_Attack, 6, 1, 0.02f);
            SquishPlayer = new Animation(Main.Squish_Player, 6, 1, 0.03f);
            Walk.Start();
            BasicAttack.Start();
            SquishPlayer.Start();

            Acceleration = new Vector2(0.5f, 0f);

            AttackRect = new Rectangle(0, 0, 0, 0);

            collectedObjects = new List<Object>();

            width = 16;
            height = 32;

            Velocity = new Vector2(4f, 4f);
            //Velocity.Y = 0;

            //Walk = new Animation(Main.Player, 7, 1, 0.04f, 1);
            //BasicAttack = new Animation(Main.Player, 5, 4, 0.05f, 4);

            

            PV = 3;

        }

        public void InitLight()
        {
            light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 50f, Color.White);
            LightManager.lights.Add(light);
        }


        public override void Update(GameTime gameTime)
        {

            //GamePad.SetVibration(PlayerIndex.One, 1f, 1f);

            if (isOnGround)
                this.Wind = Play.Wind / 2;
            if(!isOnGround)
                this.Wind = Play.Wind;
            if (isLower && isOnGround)
                this.Wind = Vector2.Zero;
            if (isLower && !isJump && (Play.Wind.X >= 1.5f || Play.Wind.X <= -1.5f))
                this.Wind = Play.Wind / 4;
            if (isLower && isJump && (Play.Wind.X >= 1.5f || Play.Wind.X <= -1.5f))
                this.Wind = Play.Wind / 2;


            if (KeyInput.getKeyState().IsKeyDown(Keys.Y) && !KeyInput.getOldKeyState().IsKeyDown(Keys.Y))
                PV -= 1;

            if (KeyInput.getKeyState().IsKeyDown(Keys.X) && !KeyInput.getOldKeyState().IsKeyDown(Keys.X))
                PV += 1;


            if (KeyInput.getKeyState().IsKeyDown(Keys.T) && !KeyInput.getOldKeyState().IsKeyDown(Keys.T))
                this.Position = Level.getSpawn(); //new Vector2(200, 200);

            if (clientID == ID)
            {
                if (KeyInput.getKeyState().IsKeyDown(Main.Left) || KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad))
                { Walk.Start();}
                else
                {
                    Walk.Reset();
                    Walk.Stop();
                }
            }
            

            #region Second PLayer

            if(clientID != ID)
            {
                if (LeftKey || RightKey)
                    Walk.Start();
                else
                {
                    Walk.Reset();
                    Walk.Stop();
                }
            }
            

            #endregion


            #region Controle

            OldPosition.X = Position.X;
            OldPosition.Y = Position.Y;

            if ((KeyInput.getKeyState().IsKeyDown(Main.Left) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad)) && ((!isLower && !isJump) || (isLower && isJump) || isJump) && ID == clientID)
            { Position.X -= 2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60; isRight = false; OnLadder = false; }
                

            if ((KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad)) && ((!isLower && !isJump) || (isLower && isJump) || isJump) && ID == clientID)
            { Position.X += 2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60; isRight = true; OnLadder = false; }


            #region Second Player

            if (LeftKey && ((!isLower && !isJump) || (isLower && isJump) || isJump) && ID != clientID)
            { Position.X -= 2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60; isRight = false; }


            if (RightKey && ((!isLower && !isJump) || (isLower && isJump) || isJump) && ID != clientID)
            { Position.X += 2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60; isRight = true; }

            #endregion


            Position.X += Wind.X;


            if (TileRided != null)
                Position.X += TileRided.GetVelocity().X;

            //Console.WriteLine(Main.tiles.Exists(x => x.getType() == 6 && x.getPosInLevel() == new Vector2(0, 1)));

            if (isDead)
                SquishPlayer.Update(gameTime);

            if (isAttack)
                BasicAttack.Update(gameTime);


            if (!isDead)
            { HorizontaleCollision(); HorizontaleCollision(); HorizontaleCollision(); }   ///HorizontaleCollision(); HorizontaleCollision(); }

            if (IsSquish())
                KillPlayer();

            OldPosition.Y = Position.Y;

            Jump();

            DisplacementOnLadder();

            if (TileRided != null)
            Position.Y += TileRided.GetVelocity().Y;

            if (!isDead && !OnLadder)
                ApplyPhysic();

            if(!isOnGround)
                Position.Y += Wind.Y;

            DetectOnSolid();

            Lowing();
            PlayerIsLowerAndEspaceEqualOne();

            if (!isDead)
            { VerticaleCollision(); VerticaleCollision(); }


            if (IsSquish())
                KillPlayer();



            #endregion

            /// test of Util
            //Console.WriteLine(Util.GetNumUnderPoint(20.123456f, 0));


            ActorCollision();

            if(clientID == ID)
            {
                if ((KeyInput.getKeyState().IsKeyDown(Main.Left) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad)) && !isLower)
                    goLeft = true;
                else
                    goLeft = false;

                if ((KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad)) && !isLower)
                    goRight = true;
                else
                    goRight = false;
            }
            

            #region Second Player

            if (clientID != ID)
            {
                if (LeftKey && !isLower)
                    goLeft = true;
                else
                    goLeft = false;

                if (RightKey && !isLower)
                    goRight = true;
                else
                    goRight = false;
            }

            

            #endregion


            Walk.Update(gameTime);

            if (((KeyInput.getKeyState().IsKeyDown(Main.Attack) && !KeyInput.getOldKeyState().IsKeyDown(Main.Attack))
                || (GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.AttackPad) && !GamePadInput.GetOldPadState((PlayerIndex)ID - 1).IsButtonDown(Main.AttackPad))) && !isAttack && !isLower  && clientID == ID)
            { isAttack = true; AttackNum++; }


            #region Second Player

            //if (Client.instance != null)
            //{
            //    //ClientSend.PlayerState();

            //    //PlayerKeyMultiPlayer();
            //    //ClientSend.PLayerKey();
            //    //ClientSend.PlayerPos();

            //}

            //Attack();

            #endregion


            if ((int)Velocity.Y > 0 || isJump && !isOnSlope)
                isOnGround = false;

            
            if (Handler.Level != null)
                if (Position.Y >= Handler.Level.GetLength(1) * 16 && !isDead)
                    KillPlayer();

            PlayerDead();


            for(int i = 0; i < collectedObjects.Count; i++)
            {
                Object obj1 = collectedObjects[i];
                Object obj2 = collectedObjects[i];
                if (i > 0)
                    obj2 = collectedObjects[i - 1];

                obj1.Update(gameTime);

                float diffx = 0;
                float diffy = 0;

                if (i == 0)
                {
                    if (isRight)
                        diffx = obj1.Position.X - Position.X + 16;
                    else
                        diffx = obj1.Position.X - Position.X - 16;

                    diffy = obj1.Position.Y - Position.Y - GetRectangle().Height + 20;
                }
                else if(i > 0)
                {
                    if (isRight)
                        diffx = obj1.Position.X - obj2.Position.X + 10;
                    else
                        diffx = obj1.Position.X - obj2.Position.X - 10;

                    diffy = obj1.Position.Y - obj2.Position.Y;
                }

                collectedObjects[i].Velocity = new Vector2(-diffx / 5, -diffy / 5);

            }


            if (Main.Debug)
                infiniJump = true;
            else
                infiniJump = false;


            if (light != null)
                light.Position = Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //if (clientID == ID)
            DrawPlayer(spriteBatch, gameTime);

        }

        /// <summary>
        /// Used for Single Player or Player of screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void DrawPlayer(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < collectedObjects.Count; i++)
            {
                collectedObjects[i].Draw(spriteBatch, gameTime);
            }

            DEBUG.DebugCollision(GetAttackRectangle(), Color.Red, spriteBatch);

            DEBUG.DebugCollision(GetRectangle(), Color.Blue, spriteBatch);

            DEBUG.DebugCollision(GetRectangleForLadder(), Color.DarkBlue, spriteBatch);

            if (isDead)
                SquishPlayer.Draw(spriteBatch, Position + new Vector2(-7, -10), SpriteEffects.None);

            if (isAttack && !isDead && Util.IsMultiple(time, 2))
                if (isRight)
                    BasicAttack.Draw(spriteBatch, Position + new Vector2(-8, -25), SpriteEffects.None);
                else
                    BasicAttack.Draw(spriteBatch, Position + new Vector2(-35, -25), SpriteEffects.FlipHorizontally);

            if (isDead)
                if (SquishPlayer.GetFrame() == SquishPlayer.GetSourceRectangle().Count - 1)
                {
                    isDead = false;
                    Position = Level.getSpawn();     //Position = new Vector2(200, 200);
                    SquishPlayer.Reset();
                    Respawn();
                }
            if (!isDead && !isAttack && Util.IsMultiple(time, 2))
                Animation(spriteBatch);

            if (isAttack)
            {
                if (BasicAttack.GetFrame() == BasicAttack.GetSourceRectangle().Count - 1)
                {
                    isAttack = false;
                    BasicAttack.Reset();
                }
            }

            if (Main.Debug)
                spriteBatch.Draw(Main.Bounds, PosOfDebug, Color.Red);

            Writer.DrawText(Main.UltimateFont, "p" + ID, Position + new Vector2(5, -10), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

            if (Main.Debug)
            {
                Writer.DrawText(Main.UltimateFont, "Jump " + isJump, Position + new Vector2(20, 0), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "Vy " + (int)Acceleration.Y, Position + new Vector2(20, 10), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "OnGround " + isOnGround, Position + new Vector2(20, 20), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

                Writer.DrawText(Main.UltimateFont, "IsRight " + isRight, Position + new Vector2(20, 30), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "GoRight " + goRight, Position + new Vector2(20, 40), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "isLower " + isLower, Position + new Vector2(20, 50), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

                Writer.DrawText(Main.UltimateFont, "AttackKey " + AttackKey, Position + new Vector2(20, 60), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "AttackOldKey " + AttackOldKey, Position + new Vector2(20, 70), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "IsAttack " + isAttack, Position + new Vector2(20, 80), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

                Writer.DrawText(Main.UltimateFont, "ID " + ID, Position + new Vector2(-50, -80), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "clientID " + clientID, Position + new Vector2(-50, -120), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);


            }
        }

        public void DrawOtherPlayer(SpriteBatch spriteBatch)
        {

        }

        public void Animation(SpriteBatch spriteBatch)
        {
            if (time == 0 || time == 5 || time == 10 || time == 15 || time == 20 || time == 25 || time == 30 || time == 35 || time == 40 || time == 45 || time == 50 || time == 55 || time == 60)
            {
                if (isRight && !goRight && !goLeft && !isJump && !isLower)
                    spriteBatch.Draw(Main.Player, Position + new Vector2(-7, -10), new Rectangle(0, 0, 30, 50), Color.White);
                else if (!isRight && !goRight && !goLeft && !isJump && !isLower)
                    spriteBatch.Draw(Main.Player, Position + new Vector2(-7, -10), new Rectangle(0, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                if (isRight && goRight && !isJump && !isLower)
                    Walk.Draw(spriteBatch, Position + new Vector2(-7, -10));
                else if (!isRight && goLeft && !isJump && !isLower)
                    Walk.Draw(spriteBatch, Position + new Vector2(-7, -10), SpriteEffects.FlipHorizontally);

                if (isRight && isJump && !isLower)
                    spriteBatch.Draw(Main.Player, Position + new Vector2(-7, -10), new Rectangle(60, 0, 30, 50), Color.White);
                else if (!isRight && isJump && !isLower)
                    spriteBatch.Draw(Main.Player, Position + new Vector2(-7, -10), new Rectangle(60, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                if (isLower && isRight && !isJump)
                    spriteBatch.Draw(Main.Player_Down, Position + new Vector2(-7, -26), new Rectangle(0, 0, 30, 50), Color.White);
                else if (isLower && !isRight && !isJump)
                    spriteBatch.Draw(Main.Player_Down, new Vector2(Position.X - 7, GetRectangle().Y - 26), new Rectangle(0, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                if (isLower && isRight && isJump)
                    spriteBatch.Draw(Main.Player_Down, Position + new Vector2(-7, -26), new Rectangle(0, 0, 30, 50), Color.White);
                else if (isLower && !isRight && isJump)
                    spriteBatch.Draw(Main.Player_Down, new Vector2(Position.X - 7, GetRectangle().Y - 26), new Rectangle(0, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

            }



        }

        public void KillPlayer()
        {
            PV = 0;
            isDead = true;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            SquishPlayer.Reset();
        }

        public void PlayerDead()
        {

            if(PV <= 0)
            {
                num++;
                Main.text = "Vous   vous    êtes    fait    tuer    !   " + (2 - (int)(num / 60)) + " sec";

                /*if (num >= 660 / 5)
                {
                    Respawn();
                }*/

            }
            
            if (PV <= 0 && !isDead)
            {
                
                //Position = new Vector2(Position.X, 1000);

                KillPlayer();
            }
        }

        public void Respawn()
        {
            //Position = Level.getSpawn() + new Vector2(0, 0);
            time = invincibletime;
            //Velocity.X = 2f;
            hitEnemy = true;
            Main.text = "";
            PV = 3;
            num = 0;
        }

        public void HorizontaleCollision()
        {

            /// Moving block
            for (int i = 0; i < Handler.solids.Count; i++)
            {

                Solid tile = Handler.solids[i];

                if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.SolidVsActor(this, tile))
                    {

                        if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Left)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Left, this, tile).X;
                            Left = true;
                            BlockMove = true;
                            //Console.WriteLine("Left");
                        }

                        if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                            Right = true;
                            BlockMove = true;
                            //Console.WriteLine("Right");
                        }

                    }
                }
            }

            /// Static Block
            if (Handler.Level != null && Handler.Level.Length != 0)
            {

                Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
                Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width - 0, GetRectangle().Y);
                Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height - 1);

                int xMin = (int)Point1.X / 16;
                int xMax = (int)Point2.X / 16;

                int yMin = (int)Point1.Y / 16;
                int yMax = (int)Point3.Y / 16;

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        Solid tile = Handler.Level[i, j];

                        int index = i - 1;  ///
                        if (index < 0)      /// For Slope BUG           (Pas encore fait dans TileV2 et EnemyV2)
                            index = 0;      ///

                        if (tile == null)                        /// provisoir
                            break;



                        if (Collision.SolidVsActor(this, tile) && Main.SolidTile[tile.getType()] && !tile.isSlope)
                        {

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Left && Handler.Level[index, j].SlopeType != 2)  /// Slope BUG
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Left, this, tile).X;
                                if ((!Left && !Right) || BlockMove)
                                    Left = true;
                                //Console.WriteLine("Left");
                            }

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right && Handler.Level[index, j].SlopeType != 1)  /// Slope BUG
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                                if ((!Left && !Right) || BlockMove)
                                    Right = true;
                                //Console.WriteLine("Right");
                            }
                        }
                    }
                }

            }

            //Console.WriteLine(Left + " ; " + Right);


        }

        public void VerticaleCollision()
        {

            isOnSlope = false;

            /// Moving Block
            for (int i = 0; i < Handler.solids.Count; i++)
            {

                Solid tile = Handler.solids[i];

                if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.SolidVsActor(this, tile))
                    {


                        if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Up)
                        {

                            //Console.WriteLine("Up");


                            Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Up, this, tile).Y;

                            Up = true;
                            Velocity.Y = 0;
                            Acceleration.Y = 0;

                            if(tile.Velocity.Y > 0)
                                Acceleration.Y = tile.Velocity.Y;

                            //SetRidingTile(null);



                        }

                        else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down)
                        {
                            Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                            Down = true;
                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                            isJump = false;
                            isOnGround = true;
                            OnLadder = false;

                            //Console.WriteLine("Down");
                        }
                    }
                }
            }

            /// Static Block
            Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width - 0, GetRectangle().Y);
            Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height - 1);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16;
            int yMax = (int)Point3.Y / 16;

            if (Handler.Level != null)
            {

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                float result = 0;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        Solid tile = Handler.Level[i, j];

                        if (tile == null)                    /////////////////// provisoir
                            break;

                        /// NoSlope
                        if (Collision.SolidVsActor(this, tile) && Main.SolidTile[tile.getType()] && !tile.isSlope)
                        {


                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Up)
                            {

                                //Console.WriteLine("Up");


                                Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Up, this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 0;

                                //SetRidingTile(null);



                            }

                            else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down)
                            {
                                Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                                Down = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                isJump = false;
                                isOnGround = true;
                                OnLadder = false;

                                //Console.WriteLine(tile.Position.X/16);

                                //Console.WriteLine("Down");
                            }
                        }

                        /// Slope
                        if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {
                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {
                                //Console.WriteLine("Collision Slope");

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, GetRectangle().Height);

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(GetRectangle().Width - 1, GetRectangle().Height);

                                //Console.WriteLine(Collision.TriangleSolidVsActorResolution(this, tile));


                                Down = true;
                                Acceleration.Y = 2;
                                Velocity.Y = 0;
                                isJump = false;
                                isOnGround = true;
                                isOnSlope = true;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && goRight)
                                    Acceleration.Y = 0;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && goLeft)
                                    Acceleration.Y = 3;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && goRight)
                                    Acceleration.Y = 3;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && goLeft)
                                    Acceleration.Y = 0;

                                if (Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height <= 1)
                                    Acceleration.Y = 0;

                                //Console.WriteLine(Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;
                            }

                            /// Up Slope
                            if((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                            {
                                

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(GetRectangle().Width - 1, 0);
                                else if((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, 0);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 1;

                            }

                        }

                        /// Platform
                        if (Collision.SolidVsActor(this, tile) && Main.SolidTileTop[tile.getType()] && !tile.isSlope)
                        {
                                                                                                                                                                                                    ///          ///Provisoir///           ///
                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down && OldPosition.Y + GetRectangle().Height <= tile.GetRectangle().Y && (!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)))
                            {

                                
                                if(!tile.isBreakable)
                                    Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;
                                else
                                    result = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                                Down = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                isJump = false;
                                isOnGround = true;

                                if (tile.isBreakable)
                                    tile.Break();


                            }

                        }


                    }
                }

                if(result != 0)
                    Position.Y = result;


            }
        }

        public override bool IsRiding()
        {
            return false;
        }

        public void DetectOnSolid()
        {
            for (int i = 0; i < Handler.solids.Count; i++)
            {
                Solid tile = Handler.solids[i];

                if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.RectVsRect(new Rectangle(GetRectangle().X, GetRectangle().Y + GetRectangle().Height + 1, GetRectangle().Width, 1), tile.GetRectangle()))
                    {
                        SetRidingTile(tile);
                        goto L_2;
                    }
                    else
                        SetRidingTile(null);

                
                        
                }
            }

        L_2:;

        }

        public override void SetRidingTile(Solid tile)
        {
            TileRided = tile;
        }

        public void PlayerIsLowerAndEspaceEqualOne()
        {
            if (wasLower && !isLower)
            {
                for (int i = 0; i < Handler.solids.Count; i++)
                {
                    Solid tile = Handler.solids[i];

                    if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                    {

                        if (Collision.RectVsRect(new Rectangle(GetRectangle().X, (int)((float)GetRectangle().Y - Velocity.Y  + 16), GetRectangle().Width, GetRectangle().Height - 16), tile.GetRectangle()))
                        {
                            isLower = true;
                            Position.Y += 16;
                        }
                    }
                }
            }
        }

        public void Jump()
        {

            float V = 1.03f;

            float Power = 1;
            if (SpringTouched) { Power = 1.22f; }

            /// Jump Controle Propulsion Vector Recuperation
            if (TileRided != null && TileRided.GetVelocity().Y !< 0)
                V = V + -TileRided.GetVelocity().Y/20;

            if (((KeyInput.getKeyState().IsKeyDown(Main.Up) && !KeyInput.getOldKeyState().IsKeyDown(Main.Up)) || (GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad) && !GamePadInput.GetOldPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad))) && !isLower && (isOnGround || infiniJump || OnLadder) && ID == clientID)//&& !isJump)
            { Acceleration.Y = -JumpImpulsion * Power; TileRided = null; isJump = true; OnLadder = false; }

            if (((KeyInput.getKeyState().IsKeyDown(Main.Up) && !KeyInput.getOldKeyState().IsKeyDown(Main.Up)) || (GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad) && !GamePadInput.GetOldPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad))) && isLower && (isOnGround || infiniJump || OnLadder) && ID == clientID)// && !isJump)
            { Acceleration.Y = -LowerJumpImpulsion * Power; TileRided = null; isJump = true; OnLadder = false; }

            /// Jump Controle Propulsion
            if ((KeyInput.getKeyState().IsKeyDown(Main.Up) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad)) && (int)Velocity.Y <= 0 && ID == clientID)
                Acceleration.Y *= V;


            #region Second Player

            /// Jump Controle Propulsion Vector Recuperation
            if (TileRided != null && TileRided.GetVelocity().Y! < 0)
                V = V + -TileRided.GetVelocity().Y / 20;

            if (UpKey && !UpOldKey && !isLower && (isOnGround || infiniJump) && ID != clientID)//&& !isJump)
            { Acceleration.Y = -JumpImpulsion * Power; TileRided = null; isJump = true; }

            if (UpKey && !UpOldKey && isLower && (isOnGround || infiniJump) && ID != clientID)// && !isJump)
            { Acceleration.Y = -LowerJumpImpulsion * Power; TileRided = null; isJump = true; }

            /// Jump Controle Propulsion
            if (UpKey && (int)Velocity.Y <= 0 && ID != clientID)
                Acceleration.Y *= V;

            #endregion


            if (SpringTouched && (KeyInput.getKeyState().IsKeyDown(Main.Up) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad)))
            { Acceleration.Y = -JumpImpulsion * Power; TileRided = null; isJump = true; SpringTouched = false; }
            else if(SpringTouched && (!KeyInput.getKeyState().IsKeyDown(Main.Up) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad)))
            { Acceleration.Y = -JumpImpulsion / Power; TileRided = null; isJump = true; SpringTouched = false; }
        }

        public void Lowing()
        {
            wasLower = isLower;

            if ((KeyInput.getKeyState().IsKeyDown(Main.Down) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && !isJump && !isLower && isOnGround && clientID == ID)
                Position.Y += 16;

            if ((!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && !isJump && isLower && isOnGround && clientID == ID)      ///     BOGUE OF SLOPE
                Position.Y -= 16;                                                                                                       ///     RESOLVED !!!!!

            if ((KeyInput.getKeyState().IsKeyDown(Main.Down) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && (!isJump && isOnGround) && clientID == ID)
                isLower = true;
            else if ((!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && (!isJump && isOnGround) && clientID == ID)
                isLower = false;

            if (isLower)
            {
                isAttack = false;
                BasicAttack.Reset();
            }


            #region Second Player

            if (DownKey && !isJump && !isLower && isOnGround && clientID != ID)
                Position.Y += 16;

            //if (!DownKey && !isJump && isLower && isOnGround && clientID != ID)      ///     BOGUE OF SLOPE
                //Position.Y -= 16;                                                                                                       ///     RESOLVED !!!!!

            if (DownKey && (!isJump && isOnGround) && clientID != ID)
                isLower = true;
            else if (!DownKey && (!isJump && isOnGround) && clientID != ID)
                isLower = false;

            if (isLower)
            {
                isAttack = false;
                BasicAttack.Reset();
            }

            #endregion

        }

        public void DisplacementOnLadder()
        {
            if (KeyInput.getKeyState().IsKeyDown(Keys.Space) && OnLadder && !OnTopOfLadder)
                Position.Y -= 1;

            if (KeyInput.getKeyState().IsKeyDown(Main.Down) && OnLadder)
                Position.Y += 1;
        }

        public override bool IsSquish()
        {
            if (Left && Right)
                goto L_1;
            else if (Up && Down)
                goto L_1;


            Left = false;
            Right = false;
            Up = false;
            Down = false;
            BlockMove = false;

            return false;

        L_1:
            {

                //Console.WriteLine(Left + " ; " + Right + "  :  " + Down + " ; " + Up);

                Left = false;
                Right = false;
                Up = false;
                Down = false;
                BlockMove = false;

                return true;
            }
        }

        public override bool IsLower()
        {
            return isLower;
        }

        public override bool HasLowerState()
        {
            return true;
        }

        public void ApplyPhysic()
        {

            Acceleration.Y += Gravity;

            Velocity.Y = Acceleration.Y;

            if (Velocity.Y > 10)
                Velocity.Y = 10;

            if (Acceleration.Y > 10)
                Acceleration.Y = 10;

            OldPos.Y = Position.Y;

            Position.Y += (int)Velocity.Y;

        }

        public void ActorCollision()
        {
            
            for (int i = 0; i < Handler.actors.Count; i++)
            {

                Actor actor = Handler.actors[i];

                if ((actor.ID == 1 || actor.ID == 2) && actor.actorType == ActorType.Enemy)
                {

                    if (Collision.RectVsRect(GetRectangle(), actor.GetRectangle()))
                    {
                        //Console.WriteLine("Ennemie ! ");

                        if (time == 0)
                        {
                            PV -= 1;
                            time = invincibletime;
                        }

                        hitEnemy = true;

                        //Handler.actors.Remove(actor);
                    }

                    if (Collision.RectVsRect(GetRectangle(), actor.GetAttackRectangle()))
                    {

                        if (time == 0)
                        {
                            PV -= 1;
                            time = invincibletime;
                        }

                        hitEnemy = true;
                    }

                }

                else if(actor.actorType == ActorType.Item)
                {
                    if (Collision.RectVsRect(GetRectangle(), actor.GetRectangle()))
                    {
                        Main.Money += actor.ID;
                        //AddMoney(actor.ID);
                        Handler.actors.Remove(actor);
                    }

                }

                else if (actor.actorType == ActorType.Object)
                {
                    if (Collision.RectVsRect(GetRectangle(), actor.GetRectangle()))
                    {
                        switch (actor.ID)
                        {
                            case 1:
                                Main.Money += 1;
                                //AddMoney(1);
                                LightManager.lights.Remove(actor.light);
                                Handler.actors.Remove(actor);
                                break;
                            case 2:
                                PV += 1;
                                LightManager.lights.Remove(actor.light);
                                Handler.actors.Remove(actor);
                                break;
                            case 3:
                                Level.setCheckPoint(actor.Position);
                                actor.CheckPointHited = true;
                                break;

                            case 4:
                                //if (collectedKey != null)
                                //  if (actor.NumOfTriggerObject == collectedKey.NumOfTriggerObject)
                                //    actor.isLocked = false;

                                for(int o = 0; o < collectedObjects.Count; o++)
                                {
                                    if (actor.NumOfTriggerObject == collectedObjects[o].NumOfTriggerObject && actor.isLocked)
                                    { actor.isLocked = false; collectedObjects.Remove(collectedObjects[o]); }
                                }

                                break;

                            case 5:
                                actor.isCollected = true;
                                collectedObjects.Add((Object)actor);
                                //collectedKey = (Object)actor;
                                Handler.actors.Remove(actor);
                                break;

                            case 6:
                                SpringTouched = true;
                                break;

                            case 7:
                                break;

                        }
                        
                    }

                }

            }


            for (int i = 0; i < Handler.ladder.Count; i++)
            {

                Actor ladder = Handler.ladder[i];

                if (Collision.RectVsRect(GetRectangleForLadder(), ladder.GetRectangle()))
                {
                    if ((KeyInput.getKeyState().IsKeyDown(Keys.Space) || (KeyInput.getKeyState().IsKeyDown(Main.Down) && GetRectangle().Y + GetRectangle().Height != ladder.GetRectangle().Y + ladder.GetRectangle().Height)) && !OnLadder && GetRectangle().Y > ladder.GetRectangle().Y - 16)
                    { OnLadder = true; isOnGround = false; Acceleration.Y = 0; Position.X = ladder.GetRectangle().X - 1; }

                    if (GetRectangle().Y < ladder.GetRectangle().Y - 16)
                        OnTopOfLadder = true;
                    else
                        OnTopOfLadder = false;

                }
                else
                { OnLadder = false; OnTopOfLadder = false; }

            }



            if (hitEnemy && time > 0)
                time--;

            if (time == 0)
            {
                hitEnemy = false;
            }

        }

        public override void RemovePV(int PV)
        {
            this.PV -= PV;
        }

        public override Rectangle GetRectangle()
        {
            if(isLower)
                return new Rectangle((int) Position.X, (int) Position.Y + 0, 16, 16);

            return new Rectangle((int)Position.X, (int)Position.Y, 16, 32);    //16, 32);
        }

        public Rectangle GetRectangleForLadder()
        {
            return new Rectangle((int)Position.X + 5, (int)Position.Y + 16, 16 - 10, 32 - 16);
        }

        public override Rectangle GetAttackRectangle()
        {
            if (isAttack && BasicAttack.GetFrame() == 3)
                if(isRight)
                    return new Rectangle((int)Position.X + 5, (int)Position.Y - 20, 44 - 7, 44);
                else
                    return new Rectangle((int)Position.X - 26, (int)Position.Y - 20, 44 - 7, 44);

            return Rectangle.Empty;   
        }

        public override Vector2 GetPosForCamera()
        {

            if (isLower)
                return new Vector2(0, -8) + Position;

            return Position;

        }


        /// <summary>
        /// Used for Multiplayer Network
        /// </summary>
        public void AddMoney(int _money)
        {
            //if(Client.instance != null)
                //if (Client.instance.tcp.socket != null)
                    //ClientSend.CollectedMoney(_money);
        }


        public bool IsAttack()
        {
            return isAttack;
        }

        public bool IsRight()
        {
            return isRight; 
        }

        public bool IsGoLeft()
        {
            return goLeft;
        }

        public bool IsGoRight()
        {
            return goRight;
        }

        public void PlayerKeyMultiPlayer()
        {
            if (KeyInput.getKeyState().IsKeyDown(Main.Down) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad))
                DownKey2 = true;
            else
                DownKey2 = false;

            if (KeyInput.getKeyState().IsKeyDown(Main.Up) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad))
                UpKey2 = true;
            else
                UpKey2 = false;

            if (KeyInput.getKeyState().IsKeyDown(Main.Left) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad))
                LeftKey2 = true;
            else
                LeftKey2 = false;

            if (KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad))
                RightKey2 = true;
            else
                RightKey2 = false;

            if (KeyInput.getKeyState().IsKeyDown(Main.Attack) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.AttackPad))
                AttackKey2 = true;
            else
                AttackKey2 = false;

        }

        public void Attack()
        {

            if (AttackKey && !AttackOldKey && !isAttack && !isLower && clientID != ID)
            { isAttack = true; AttackNum++; Console.WriteLine("ERROR"); }

        }

    }
}
