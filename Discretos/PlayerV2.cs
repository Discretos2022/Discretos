using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.Utilities.Deflate;
using NetworkEngine_5._0.Client;
using NetworkEngine_5._0.Server;
using Plateform_2D_v9.Core;
using Plateform_2D_v9.NetCore;
using Plateform_2D_v9.Objects;
using System;
using System.Collections.Generic;
using System.Text;

/// Solved : Player Collision Between 2 blocks with left and right pressed -> kill player

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

        private int width, height;

        private new readonly float Gravity = 0.35f;  // 0.35f                  //0.3f

        private bool isJump;

        private int num = 0;

        private Animation Walk;
        private Animation BasicAttack;
        private Animation SquishPlayer;

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

        private int AttackNum = 0;

        public Vector2 PosOfDebug;

        public bool infiniJump = false;

        private Object collectedKey;
        private List<ObjectV2> collectedObjects;


        /// <summary>
        /// Used for multiplayer
        /// </summary>
        public int myPlayerID;      // my ID of player

        public int ID;

        public int PV;

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

        public PlayerV2(Vector2 Position, int PlayerID, int _myPlayerID = 1)
            : base(new Vector2(Position.X, Position.Y))
        {
            this.actorType = ActorType.Player;

            this.ID = PlayerID;
            myPlayerID = _myPlayerID;

            Walk = new Animation(Main.Player, 9, 1, 0.03f, 1);
            BasicAttack = new Animation(Main.Player_Basic_Attack, 6, 1, 0.02f);
            SquishPlayer = new Animation(Main.Squish_Player, 6, 1, 0.03f);
            Walk.Start();
            BasicAttack.Start();
            SquishPlayer.Start();

            Acceleration = new Vector2(0.5f, 0f);

            AttackRect = new Rectangle(0, 0, 0, 0);

            collectedObjects = new List<ObjectV2>();

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
            light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 50f, Color.White); // 50f
            LightManager.lights.Add(light);
        }


        public override void Update(GameTime gameTime)
        {

            if(myPlayerID == ID)
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

                if (KeyInput.getKeyState().IsKeyDown(Main.Left) || KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad))
                { Walk.Start();}
                else
                {
                    Walk.Reset();
                    Walk.Stop();
                }

                #region Controle

                OldPosition.X = Position.X;
                OldPosition.Y = Position.Y;


                if (isDead)
                    SquishPlayer.Update(gameTime);

                if (isAttack)
                    BasicAttack.Update(gameTime);

                if (IsSquish())
                    KillPlayer();

                OldPosition.Y = Position.Y;

                if (IsSquish())
                    KillPlayer();



                #endregion

                /// test of Util
                //Console.WriteLine(Util.GetNumUnderPoint(20.123456f, 0));

                ActorCollision();
                
                if ((KeyInput.getKeyState().IsKeyDown(Main.Left) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad)) && !isLower)
                    goLeft = true;
                else
                    goLeft = false;

                if ((KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad)) && !isLower)
                    goRight = true;
                else
                    goRight = false;

                Walk.Update(gameTime);

                if (((KeyInput.getKeyState().IsKeyDown(Main.Attack) && !KeyInput.getOldKeyState().IsKeyDown(Main.Attack))
                    || (GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.AttackPad) && !GamePadInput.GetOldPadState((PlayerIndex)ID - 1).IsButtonDown(Main.AttackPad))) && !isAttack && !isLower  && myPlayerID == ID)
                { isAttack = true; AttackNum++; }

                if ((int)Velocity.Y > 0 || isJump && isOnSlope == TileV2.SlopeType.None)
                    isOnGround = false;

            
                if (Handler.Level != null)
                    if (Position.Y >= Handler.Level.GetLength(1) * 16 && !isDead)
                        KillPlayer();

                PlayerDead();

                if (Main.Debug)
                    infiniJump = true;
                else
                    infiniJump = false;

                if(!OnLadder)
                    ApplyPhysic();
            }

            UpdateCollectedObject(gameTime);

            if (light != null)
                light.Position = Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //if (clientID == ID)
            DrawPlayer(spriteBatch, gameTime);


            ///*****************************************************************************************************

            /*
            Render.DrawRectangleV1_1(Main.Bounds, GetRectangle(), spriteBatch, Color.Red, 1);


            Vector2 Point1 = new Vector2(GetRectangle().X - 16 * 4, GetRectangle().Y - 16*4);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width + 16 * 4, GetRectangle().Y - 16 * 4);
            Vector2 Point3 = new Vector2(GetRectangle().X - 16 * 4, GetRectangle().Y + GetRectangle().Height + 16 * 4);

            //Render.DrawLineV1_1(Main.Bounds, Point1, Point2, spriteBatch, Color.Blue, 1);
            //Render.DrawLineV1_1(Main.Bounds, Point1, Point3, spriteBatch, Color.Blue, 1);

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

                    //if(Handler.Level[i, j].type != 0)
                    //Render.DrawRectangleV1_1(Main.Bounds, Handler.Level[i, j].GetRectangle(), spriteBatch, Color.Orange, 1);

                    if (Handler.Level[i, j].type != 0)
                    {

                        if(j > 0 && Handler.Level[i, j - 1].type == 0)
                        {
                            Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition(), spriteBatch, Color.Blue, 1);
                            Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition() + new Vector2(16, 0), spriteBatch, Color.Blue, 1);
                        }

                        if (j < Handler.Level.GetLength(1) - 1 && Handler.Level[i, j + 1].type == 0)
                        {
                            Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition() + new Vector2(0, 16), spriteBatch, Color.Yellow, 1);
                            Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition() + new Vector2(16, 16), spriteBatch, Color.Yellow, 1);
                        }   

                        //Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition(), spriteBatch, Color.Blue, 1);
                        //Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition() + new Vector2(16, 0), spriteBatch, Color.Blue, 1);
                        //Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition() + new Vector2(0, 16), spriteBatch, Color.Blue, 1);
                        //Render.DrawLineV1_1(Main.Bounds, Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), Handler.Level[i, j].GetPosition() + new Vector2(16, 16), spriteBatch, Color.Blue, 1);
                    }




                }
            }*/

            ///*****************************************************************************************************

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

                //Writer.DrawText(Main.UltimateFont, "IsRight " + isRight, Position + new Vector2(20, 30), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                //Writer.DrawText(Main.UltimateFont, "GoRight " + goRight, Position + new Vector2(20, 40), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                //Writer.DrawText(Main.UltimateFont, "isLower " + isLower, Position + new Vector2(20, 50), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

                //Writer.DrawText(Main.UltimateFont, "AttackKey " + AttackKey, Position + new Vector2(20, 60), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                //Writer.DrawText(Main.UltimateFont, "AttackOldKey " + AttackOldKey, Position + new Vector2(20, 70), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                //Writer.DrawText(Main.UltimateFont, "IsAttack " + isAttack, Position + new Vector2(20, 80), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

                //Writer.DrawText(Main.UltimateFont, "ID " + ID, Position + new Vector2(-50, -80), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                //Writer.DrawText(Main.UltimateFont, "clientID " + myPlayerID, Position + new Vector2(-50, -120), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

                Writer.DrawText(Main.UltimateFont, "isonslope " + isOnSlope.ToString().ToLower(), Position + new Vector2(20, 90), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

            }
        }

        public void DrawOtherPlayer(SpriteBatch spriteBatch)
        {

        }

        public void Animation(SpriteBatch spriteBatch)
        {
            if (time == 0 || time == 5 || time == 10 || time == 15 || time == 20 || time == 25 || time == 30 || time == 35 || time == 40 || time == 45 || time == 50 || time == 55 || time == 60)
            {
                /// Idle Player
                if (isRight && !goRight && !goLeft && !isJump && !isLower && (int)Velocity.Y <= 0)
                    spriteBatch.Draw(Main.Player, Position + new Vector2(-7, -10), new Rectangle(0, 0, 30, 50), Color.White);
                else if (!isRight && !goRight && !goLeft && !isJump && !isLower && (int)Velocity.Y <= 0)
                    spriteBatch.Draw(Main.Player, Position + new Vector2(-7, -10), new Rectangle(0, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                /// Simple walk
                if (isRight && goRight && !isJump && !isLower && (int)Velocity.Y <= 0) //     && isOnGround)
                    Walk.Draw(spriteBatch, Position + new Vector2(-7, -10));
                else if (!isRight && goLeft && !isJump && !isLower && (int)Velocity.Y <= 0)
                    Walk.Draw(spriteBatch, Position + new Vector2(-7, -10), SpriteEffects.FlipHorizontally);

                /// Jump
                if (isRight && isJump && !isLower && (int)Velocity.Y <= 0)       //&& !isOnGround)
                    spriteBatch.Draw(Main.Player_Jump, Position + new Vector2(-7, -10), new Rectangle(0, 0, 30, 50), Color.White);
                else if (!isRight && isJump && !isLower && (int)Velocity.Y <= 0)
                    spriteBatch.Draw(Main.Player_Jump, Position + new Vector2(-7, -10), new Rectangle(0, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                /// Fall before jump
                if (isRight && isJump && !isLower && (int)Velocity.Y > 0)       //&& !isOnGround)
                    spriteBatch.Draw(Main.Player_Jump, Position + new Vector2(-7, -10), new Rectangle(0, 50, 30, 50), Color.White);
                else if (!isRight && isJump && !isLower && (int)Velocity.Y > 0)
                    spriteBatch.Draw(Main.Player_Jump, Position + new Vector2(-7, -10), new Rectangle(0, 50, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                /// Fall
                if (isRight && !isJump && !isLower && (int)Velocity.Y > 0)       //&& !isOnGround)
                    spriteBatch.Draw(Main.Player_Jump, Position + new Vector2(-7, -10), new Rectangle(0, 50, 30, 50), Color.White);
                else if (!isRight && !isJump && !isLower && (int)Velocity.Y > 0)
                    spriteBatch.Draw(Main.Player_Jump, Position + new Vector2(-7, -10), new Rectangle(0, 50, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                /// Jump with lowing
                if (isLower && isRight && !isJump)
                    spriteBatch.Draw(Main.Player_Down, Position + new Vector2(-7, -26), new Rectangle(0, 0, 30, 50), Color.White);
                else if (isLower && !isRight && !isJump)
                    spriteBatch.Draw(Main.Player_Down, new Vector2(Position.X - 7, GetRectangle().Y - 26), new Rectangle(0, 0, 30, 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                /// Lowing
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

        public void Jump()
        {

            float V = 1.03f;

            float Power = 1;
            if (SpringTouched) { Power = 1.22f; }

            /// Jump Controle Propulsion Vector Recuperation
            if (blockUnder != null && blockUnder.Velocity.Y !< 0)
                V = V + -blockUnder.Velocity.Y/20;

            if (((KeyInput.getKeyState().IsKeyDown(Main.Up) && !KeyInput.getOldKeyState().IsKeyDown(Main.Up)) || (GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad) && !GamePadInput.GetOldPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad))) && !isLower && (isOnGround || infiniJump || OnLadder) && ID == myPlayerID)//&& !isJump)
            { Acceleration.Y = -JumpImpulsion * Power; blockUnder = null; isJump = true; OnLadder = false; }

            if (((KeyInput.getKeyState().IsKeyDown(Main.Up) && !KeyInput.getOldKeyState().IsKeyDown(Main.Up)) || (GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad) && !GamePadInput.GetOldPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad))) && isLower && (isOnGround || infiniJump || OnLadder) && ID == myPlayerID)// && !isJump)
            { Acceleration.Y = -LowerJumpImpulsion * Power; blockUnder = null; isJump = true; OnLadder = false; }

            /// Jump Controle Propulsion
            if ((KeyInput.getKeyState().IsKeyDown(Main.Up) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad)) && (int)Velocity.Y <= 0 && ID == myPlayerID)
                Acceleration.Y *= V;


            #region Second Player

            /// Jump Controle Propulsion Vector Recuperation
            if (blockUnder != null && blockUnder.Velocity.Y! < 0)
                V = V + -blockUnder.Velocity.Y / 20;

            if (UpKey && !UpOldKey && !isLower && (isOnGround || infiniJump) && ID != myPlayerID)//&& !isJump)
            { Acceleration.Y = -JumpImpulsion * Power; blockUnder = null; isJump = true; }

            if (UpKey && !UpOldKey && isLower && (isOnGround || infiniJump) && ID != myPlayerID)// && !isJump)
            { Acceleration.Y = -LowerJumpImpulsion * Power; blockUnder = null; isJump = true; }

            /// Jump Controle Propulsion
            if (UpKey && (int)Velocity.Y <= 0 && ID != myPlayerID)
                Acceleration.Y *= V;

            #endregion


            if (SpringTouched && (KeyInput.getKeyState().IsKeyDown(Main.Up) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad)))
            { Acceleration.Y = -JumpImpulsion * Power; blockUnder = null; isJump = true; SpringTouched = false; }
            else if(SpringTouched && (!KeyInput.getKeyState().IsKeyDown(Main.Up) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.UpPad)))
            { Acceleration.Y = -JumpImpulsion / Power; blockUnder = null; isJump = true; SpringTouched = false; }
        }

        public void Lowing()
        {
            wasLower = isLower;

            if ((KeyInput.getKeyState().IsKeyDown(Main.Down) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && !isJump && !isLower && isOnGround && myPlayerID == ID)
                Position.Y += 16;

            if ((!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && !isJump && isLower && isOnGround && myPlayerID == ID && !tileOnUp)      ///     BOGUE OF SLOPE
                Position.Y -= 16;                                                                                                       ///     RESOLVED !!!!!

            if ((KeyInput.getKeyState().IsKeyDown(Main.Down) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && (!isJump && isOnGround) && myPlayerID == ID)
                isLower = true;
            else if ((!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)) && (!isJump && isOnGround) && myPlayerID == ID && !tileOnUp)
                isLower = false;

            if (isLower)
            {
                isAttack = false;
                BasicAttack.Reset();
            }


            #region Second Player

            if (DownKey && !isJump && !isLower && isOnGround && myPlayerID != ID)
                Position.Y += 16;

            //if (!DownKey && !isJump && isLower && isOnGround && clientID != ID)      ///     BOGUE OF SLOPE
                //Position.Y -= 16;                                                                                                       ///     RESOLVED !!!!!

            if (DownKey && (!isJump && isOnGround) && myPlayerID != ID)
                isLower = true;
            else if (!DownKey && (!isJump && isOnGround) && myPlayerID != ID)
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
            if (LeftCollision && RightCollision)
                goto L_1;
            else if (UpCollision && DownCollision)
                goto L_1;


            LeftCollision = false;
            RightCollision = false;
            UpCollision = false;
            DownCollision = false;
            BlockMove = false;

            return false;

        L_1:
            {

                //Console.WriteLine(Left + " ; " + Right + "  :  " + Down + " ; " + Up);

                LeftCollision = false;
                RightCollision = false;
                UpCollision = false;
                DownCollision = false;
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

        }

        public void ActorCollision()
        {
            
            for (int i = 0; i < Handler.actors.Count; i++)
            {

                Actor actor = Handler.actors[i];

                if (actor.actorType == ActorType.Enemy)
                {

                    if (GetRectangle().Intersects(actor.GetRectangle()))
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

                    if (GetRectangle().Intersects(actor.GetAttackRectangle()))
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
                    if (GetRectangle().Intersects(actor.GetRectangle()))
                    {
                        Main.Money += (int)((ItemV2)actor).ID;
                        Handler.RemoveActor(actor);
                    }

                }

                else if (actor.actorType == ActorType.Object)
                {

                    if (GetRectangle().Intersects(actor.GetRectangle()))
                    {

                        ObjectV2 element = (ObjectV2)actor;

                        switch (element.objectID)
                        {
                            case ObjectV2.ObjectID.coin:
                                Main.Money += 1;
                                LightManager.lights.Remove(element.light);
                                Handler.RemoveActor(element);
                                break;
                            case ObjectV2.ObjectID.core:
                                PV += 1;
                                LightManager.lights.Remove(element.light);
                                Handler.RemoveActor(element);
                                break;
                            case ObjectV2.ObjectID.checkPoint:

                                if (NetPlay.IsMultiplaying)
                                {
                                    if (NetPlay.MyPlayerID() == 1)
                                        ServerSender.SendCheckPointHited(Handler.actors.IndexOf(element), NetPlay.MyPlayerID());
                                    else
                                        ClientSender.SendCheckPointHited(Handler.actors.IndexOf(element), NetPlay.MyPlayerID());
                                }

                                if (Level.lastCheckPointNumber < ((CheckPoint)element).number) Level.setCheckPoint((CheckPoint)element);
                                ((CheckPoint)element).hited = true;
                                break;

                            case ObjectV2.ObjectID.wood_door:
                                //if (collectedKey != null)
                                //  if (actor.NumOfTriggerObject == collectedKey.NumOfTriggerObject)
                                //    actor.isLocked = false;

                                for(int o = 0; o < collectedObjects.Count; o++)
                                {
                                    if (((Door)actor).trigger == ((Key)collectedObjects[o]).trigger && ((Door)actor).hitbox.isEnabled)
                                    {
                                        ((Door)actor).isLocked = false;
                                        actor.hitbox.isEnabled = false;
                                        collectedObjects.Remove(collectedObjects[o]);

                                        if (NetPlay.IsMultiplaying)
                                        {
                                            if (NetPlay.MyPlayerID() == 1)
                                                NetworkEngine_5._0.Server.ServerSender.SendOpenDoor(i, NetPlay.MyPlayerID());
                                            else
                                                NetworkEngine_5._0.Client.ClientSender.SendOpenDoor(i, NetPlay.MyPlayerID());
                                        }
                                            
                                    }
                                }

                                break;

                            case ObjectV2.ObjectID.gold_key:
                                ((Key)actor).isCollected = true;
                                collectedObjects.Add((ObjectV2)actor);
                                Handler.RemoveActor(actor);
                                break;

                            case ObjectV2.ObjectID.spring:
                                SpringTouched = true;
                                break;

                            case ObjectV2.ObjectID.wood_ladder:


                                if (GetRectangleForLadder().Intersects(((Ladder)actor).GetRectangle()))
                                {
                                    if ((KeyInput.getKeyState().IsKeyDown(Keys.Space) || (KeyInput.getKeyState().IsKeyDown(Main.Down) && GetRectangle().Y + GetRectangle().Height != ((Ladder)actor).GetRectangle().Y + ((Ladder)actor).GetRectangle().Height)) && !OnLadder && GetRectangle().Y > ((Ladder)actor).GetRectangle().Y - 16)
                                    { OnLadder = true; isOnGround = false; Acceleration.Y = 0; Velocity.Y = 0; Position.X = ((Ladder)actor).GetRectangle().X - 1; } 
                                    if (GetRectangle().Y < ((Ladder)actor).GetRectangle().Y - 16)              ///  /!\ DANGER  \\\
                                        OnTopOfLadder = true;
                                    else
                                        OnTopOfLadder = false;
                                }
                                else
                                { OnLadder = false; OnTopOfLadder = false; }


                                break;

                            case ObjectV2.ObjectID.wood_ladder_snow:


                                if (GetRectangleForLadder().Intersects(((Ladder)actor).GetRectangle()))
                                {
                                    if ((KeyInput.getKeyState().IsKeyDown(Keys.Space) || (KeyInput.getKeyState().IsKeyDown(Main.Down) && GetRectangle().Y + GetRectangle().Height != ((Ladder)actor).GetRectangle().Y + ((Ladder)actor).GetRectangle().Height)) && !OnLadder && GetRectangle().Y > ((Ladder)actor).GetRectangle().Y - 16)
                                    { OnLadder = true; isOnGround = false; Acceleration.Y = 0; Velocity.Y = 0; Position.X = ((Ladder)actor).GetRectangle().X - 1; }
                                    if (GetRectangle().Y < ((Ladder)actor).GetRectangle().Y - 16)              ///  /!\ DANGER  \\\
                                        OnTopOfLadder = true;
                                    else
                                        OnTopOfLadder = false;
                                }
                                else
                                { OnLadder = false; OnTopOfLadder = false; }


                                break;

                        }
                        
                    }

                }

            }


            /*for (int i = 0; i < Handler.ladder.Count; i++)
            {

                Actor ladder = Handler.ladder[i];

                if (GetRectangleForLadder().Intersects(ladder.GetRectangle()))
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

            }*/



            if (hitEnemy && time > 0)
                time--;

            if (time == 0)
            {
                hitEnemy = false;
            }

        }

        public void RemovePV(int PV)
        {
            this.PV -= PV;
        }

        public override Rectangle GetRectangle()
        {
            return hitbox.rectangle;
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

            return Position; // + new Vector2(Random.Shared.Next(-1, 1), Random.Shared.Next(-1, 1));

        }

        public void AddCollectedObject(ObjectV2 key)
        {
            collectedObjects.Add(key);
        }


        public void UpdateCollectedObject(GameTime gameTime)
        {
            for (int i = 0; i < collectedObjects.Count; i++)
            {

                ObjectV2 obj1 = collectedObjects[i];
                ObjectV2 obj2 = collectedObjects[i];
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
                else if (i > 0)
                {
                    if (isRight)
                        diffx = obj1.Position.X - obj2.Position.X + 10;
                    else
                        diffx = obj1.Position.X - obj2.Position.X - 10;

                    diffy = obj1.Position.Y - obj2.Position.Y;
                }

                collectedObjects[i].Velocity = new Vector2(-diffx / 5, -diffy / 5);

            }
        }

        public List<ObjectV2> GetCollectedObjectList()
        {
            return collectedObjects;
        }





        /** COLLISION V3 **/

        public override void LeftDisplacement(GameTime gameTime)
        {
            Velocity.X = 0;
            if ((KeyInput.getKeyState().IsKeyDown(Main.Left) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad)) && ((!isLower && !isJump) || (isLower && !isOnGround) || isJump) && ID == myPlayerID)
                if (!KeyInput.getKeyState().IsKeyDown(Main.Right)) /// Solve collision between 2 blocks
                    if (!KeyInput.getKeyState().IsKeyDown(Keys.Space) || !OnLadder)
                        if(!KeyInput.getKeyState().IsKeyDown(Main.Down) || !OnLadder)
                        { Velocity.X = -2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60; isRight = false; OnLadder = false; }

            Position.X += Velocity.X;

            if (Wind.X < 0) Position.X += Wind.X;

            base.LeftDisplacement(gameTime);
        }

        public override void RightDisplacement(GameTime gameTime)
        {
            Velocity.X = 0;
            if ((KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad)) && ((!isLower && !isJump) || (isLower && !isOnGround) || isJump) && ID == myPlayerID)
                if (!KeyInput.getKeyState().IsKeyDown(Main.Left)) /// Solve collision between 2 blocks
                    if (!KeyInput.getKeyState().IsKeyDown(Keys.Space) || !OnLadder)
                        if (!KeyInput.getKeyState().IsKeyDown(Main.Down) || !OnLadder)

                        { Velocity.X = 2f * (float)gameTime.ElapsedGameTime.TotalSeconds * 60; isRight = true; OnLadder = false; }

            /// /!\ PHYSIQUE !!!!!
            //if (isOnSlope == TileV2.SlopeType.LeftDown)
                //Velocity.X /= 2;

            Position.X += Velocity.X;

            if (Wind.X > 0) Position.X += Wind.X;

            base.RightDisplacement(gameTime);
        }

        public override void DownDisplacement(GameTime gameTime)
        {
            if(!OnLadder)
                if(Velocity.Y > 0) Position.Y += (int)Velocity.Y;

            pressDown = false;
            if (KeyInput.getKeyState().IsKeyDown(Main.Down) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad))
                pressDown = true;

            if ((KeyInput.getKeyState().IsKeyDown(Main.Left) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.LeftPad)) && isOnSlope == TileV2.SlopeType.LeftDown && !isJump)
                Position.Y += 2;

            else if ((KeyInput.getKeyState().IsKeyDown(Main.Right) || GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.RightPad)) && isOnSlope == TileV2.SlopeType.RightDown && !isJump)
                Position.Y += 2;

            if (OnLadder && KeyInput.getKeyState().IsKeyDown(Main.Down))
                Position.Y += 1;


            UpdateHitbox();
        }

        public override void UpDisplacement(GameTime gameTime)
        {
            Jump();
            Lowing();
            if (Velocity.Y < 0) Position.Y += (int)Velocity.Y;

            if (OnLadder && !OnTopOfLadder && KeyInput.getKeyState().IsKeyDown(Keys.Space))
                Position.Y -= 1;

            UpdateHitbox();
        }


        #region Static Collsition Action

        public override void LeftStaticCollisionAction() { }

        public override void RightStaticCollisionAction() { }

        public override void DownStaticCollisionAction()
        {
            isJump = false;
            OnLadder = false;
        }

        public override void UpStaticCollisionAction() { }

        #endregion


        #region Dynamic Collision Action


        public override void LeftDynamicCollisionAction(MovingBlock block) { }

        public override void RightDynamicCollisionAction(MovingBlock block) { }

        public override void DownDynamicCollisionAction(MovingBlock block)
        {
            isJump = false;
            OnLadder = false;
        }

        public override void UpDynamicCollisionAction(MovingBlock block) { }


        #endregion


        public override void UpdateHitbox()
        {
            if (isLower)
                hitbox = new Hitbox((int)Position.X, (int)Position.Y + 0, 16, 16);
            else
                hitbox = new Hitbox((int)Position.X, (int)Position.Y, 16, 32);    //16, 32);
        }

    }
}
