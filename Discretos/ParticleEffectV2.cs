using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.NetCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    public static class ParticleEffectV2
    {

        public static int type;
        public static float wind;
        private static int intensity;
        private static float scale;

        private static float timer;

        public static List<ParticleV2> particles = new List<ParticleV2>();

        public static bool Actived = false;

        private static int MaxParticle = 10000;

        public static Random randomGenerator = new Random();

        public static List<int> randomList = new List<int>();

        private static int i = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="wind"> Max = +-6 </param>
        /// <param name="intensity"></param>
        /// <param name="scale"></param>

        public static void Update(GameTime gameTime)
        {

            for (i = 0; i < particles.Count; i++)
            {
                if(particles[i] != null)
                    particles[i].Update(wind);
            }

        }


        public static void Draw(SpriteBatch spriteBatch)
        {
            for(i = 0; i < particles.Count; i++)
            {
                if(particles[i] != null)
                    particles[i].Draw(spriteBatch);
            }

        }

        public static void Generate(object data)
        {

            Actived = true;

            while (Actived)
            {

                if (!Main.isPaused)
                {
                    int GeneratorBorderMinX;
                    int GeneratorBorderMaxX;

                    int Multiplier = 60;

                    GeneratorBorderMinX = (int)Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().X - 1920 / 2;
                    GeneratorBorderMaxX = (int)Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().X + 1920 / 2;

                    if (wind > 0)
                        GeneratorBorderMinX *= (int)(Util.PositiveOf(wind) * Multiplier);
                    else if (wind < 0)
                        GeneratorBorderMaxX *= (int)(Util.PositiveOf(wind) * Multiplier);

                    if (GeneratorBorderMinX < -5 * Util.PositiveOf(wind) * Multiplier)
                        GeneratorBorderMinX = (int)(-5 * Util.PositiveOf(wind) * Multiplier);

                    if (GeneratorBorderMaxX > Handler.Level.GetLength(0) * 16 + 5 * Util.PositiveOf(wind) * Multiplier)
                        GeneratorBorderMaxX = (int)(Handler.Level.GetLength(0) * 16 + 5 * Util.PositiveOf(wind) * Multiplier);

                    if (GeneratorBorderMinX > GeneratorBorderMaxX)
                        GeneratorBorderMinX = GeneratorBorderMaxX - 1;

                    int randomX;

                    for (int i = 0; i < intensity; i++)
                    {
                        if (particles.Count < MaxParticle) // 10000
                        {
                            randomX = randomGenerator.Next(GeneratorBorderMinX, GeneratorBorderMaxX);

                            randomList.Add(randomX);
                            if (randomList.Count >= 10)
                            {
                                for (int r = 0; r < randomList.Count; r++)
                                {
                                    if (randomList[r] != 0)
                                        goto L_R;

                                }

                                RestartRandom();

                            L_R:;
                                randomList = new List<int>();

                            }


                            if(type == 1)
                            {
                                if (wind < 0)
                                    particles.Add(new SnowParticleV2(type, wind, new Vector2(randomX, -20), scale));
                                else if (wind > 0)
                                    particles.Add(new SnowParticleV2(type, wind, new Vector2(randomX, -20), scale));
                                else
                                    particles.Add(new SnowParticleV2(type, wind, new Vector2(randomX, -20), scale));
                            }
                            else if (type == 2)
                            {
                                if (wind < 0)
                                    particles.Add(new RainParticleV2(type, wind, new Vector2(randomX, -20), scale));
                                else if (wind > 0)
                                    particles.Add(new RainParticleV2(type, wind, new Vector2(randomX, -20), scale));
                                else
                                    particles.Add(new RainParticleV2(type, wind, new Vector2(randomX, -20), scale));
                            }

                        }
                        //particles.Add(new Particle(type, wind, new Vector2(Util.random.Next((int)Handler.players[0].GetRectangle().X - 1920 / 4 - 200, (int)(Handler.players[0].GetRectangle().X + 300 * -wind * 2.5f)), -20), scale));
                    }
                }

                Thread.Sleep(100);

            }

        }


        public static void RestartRandom()
        {
            randomList = new List<int>();
            randomGenerator = new Random();
        }
        

        public static void SetScale(float newScale)
        {
            scale = newScale;
        }


        public static void setWind(int newWind)
        {
            wind = newWind;
        }

        public static void setIntensity(int newIntensity)
        {
            intensity = newIntensity;
        }

        public static void RemoveParticle(ParticleV2 p)
        {
            particles.Remove(p);
            i--;
        }


    }

    public class ParticleV2
    {

        protected int type;
        protected float wind;
        protected Vector2 pos;
        protected float scale;
        protected float rotation;
        protected Rectangle sourceRectangle;
        protected Vector2 Velocity;

        public ParticleV2(int type, float wind, Vector2 pos, float scale)
        {
            this.type = type;
            this.wind = wind;
            this.pos = pos;
            this.scale = scale;

            sourceRectangle = new Rectangle(Util.random.Next(0, 4) * 4, 0, 3, 3);

            Velocity = new Vector2((Random.Shared.Next(1, 2) + (float)Random.Shared.NextDouble()), (float)Random.Shared.NextDouble() + Random.Shared.Next(1, 2));

        }

        public virtual void Update(float wind) { }


        public virtual void Draw(SpriteBatch spriteBatch) { }


        public bool isUnderScreen()
        {

            if (pos.Y > Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().Y + 600 || pos.Y > Handler.Level.GetLength(1) * 16 + 5)
                return true;

            if (Velocity.X * wind > 0 && pos.X > Handler.Level.GetLength(0) * 16 + 5 * Util.PositiveOf(wind) * 60)
                return true;

            if (Velocity.X * wind < 0 && pos.X < -5 * Util.PositiveOf(wind) * 60)
                return true;

            if (pos.X < Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().X - 1920 / 2 - 5 * Util.PositiveOf(wind) * 60)
                return true;

            if (pos.X > Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().X + 1920 / 2 + 5 * Util.PositiveOf(wind) * 60)
                return true;

            return false;

        }


    }


    public class SnowParticleV2 : ParticleV2
    {
        public SnowParticleV2(int type, float wind, Vector2 pos, float scale) : base(type, wind, pos, scale)
        {

            sourceRectangle = new Rectangle(Util.random.Next(0, 4) * 4, 0, 3, 3);

            Velocity = new Vector2((Random.Shared.Next(1, 2) + (float)Random.Shared.NextDouble()), (float)Random.Shared.NextDouble() + Random.Shared.Next(1, 2));

        }


        public override void Update(float wind)
        {
            pos.X += Velocity.X * wind;
            pos.Y += Velocity.Y;

            if (isUnderScreen())
                ParticleEffectV2.RemoveParticle(this);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pos.X > Main.camera.Position.X - 1920 / 8 - 5 && pos.X < Main.camera.Position.X + 1920 / 8 + 5)
                if (pos.Y > Main.camera.Position.Y - 1080 / 8 - 5 && pos.Y < Main.camera.Position.Y + 1080 / 8 + 5)
                    spriteBatch.Draw(Main.SnowParticle, new Vector2(pos.X, pos.Y), sourceRectangle, Color.White, rotation, new Vector2(1.5f, 1.5f), scale, SpriteEffects.None, 0f);

        }

    }

    public class RainParticleV2 : ParticleV2
    {
        public RainParticleV2(int type, float wind, Vector2 pos, float scale) : base(type, wind, pos, scale)
        {

            int size = Util.random.Next(0, 4);
            sourceRectangle = new Rectangle(size * 4, 0, 4, 12);

            Velocity = new Vector2(2, Random.Shared.Next(3, 5) + 4-size);

        }


        public override void Update(float wind)
        {
            pos.X += Velocity.X * wind;
            pos.Y += Velocity.Y;

            if (isUnderScreen())
                ParticleEffectV2.RemoveParticle(this);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (pos.X > Main.camera.Position.X - 1920 / 8 - 5 && pos.X < Main.camera.Position.X + 1920 / 8 + 5)
                if (pos.Y > Main.camera.Position.Y - 1080 / 8 - 5 && pos.Y < Main.camera.Position.Y + 1080 / 8 + 5)
                    spriteBatch.Draw(Main.RainParticle, new Vector2(pos.X, pos.Y), sourceRectangle, Color.White, rotation, new Vector2(1.5f, 1.5f), scale, SpriteEffects.None, 0f);

        }

    }

}