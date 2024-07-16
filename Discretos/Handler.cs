using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Plateform_2D_v9
{
    class Handler
    {

        public static List<Actor> actors = new List<Actor>();
        public static List<Solid> solids = new List<Solid>();
        public static List<Solid> walls = new List<Solid>();
        public static List<Actor> ladder = new List<Actor>();

        public static Dictionary<int, PlayerV2> playersV2 = new Dictionary<int, PlayerV2>();

        public static TileV2[,] Level;

        public static Wall[,] Walls;


        public static void Initialize()
        {
            solids = new List<Solid>();
            walls = new List<Solid>();
            actors = new List<Actor>();
            ladder = new List<Actor>();

        }

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < solids.Count; i++)
            {
                solids[i].Update(gameTime);
            }

            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].Update(gameTime);
            }

            for (int i = 1; i <= playersV2.Count; i++)
            {
                playersV2[i].Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            #region Tiles and Walls Optimization

            int xMin = ((int)Main.camera.Position.X - 1920 / 8) / 16 - 2;
            int xMax = ((int)Main.camera.Position.X + 1920 / 8) / 16 + 2;

            int yMin = ((int)Main.camera.Position.Y - 1080 / 8) / 16 - 2;
            int yMax = ((int)Main.camera.Position.Y + 1080 / 8) / 16 + 2;

            if (xMin < 0) xMin = 0;
            if (xMax > Level.GetLength(0)) xMax = Level.GetLength(0);
            if (yMin < 0) yMin = 0;
            if (yMax > Level.GetLength(1)) yMax = Level.GetLength(1);

            #endregion


            for (int j = yMin; j < yMax; j++)
                for (int i = xMin; i < xMax; i++)
                    if (Handler.Walls[i, j].getType() > 0)
                        Handler.Walls[i, j].Draw(spriteBatch, gameTime);



            for (int j = yMin; j < yMax; j++)
                for (int i = xMin; i < xMax; i++)
                    if (Handler.Level[i, j].getType() > 0)
                        Handler.Level[i, j].Draw(spriteBatch, gameTime);

            for (int i = 0; i < solids.Count; i++)
            {
                solids[i].Draw(spriteBatch, gameTime);
            }

            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].Draw(spriteBatch, gameTime);
            }

            for (int i = 1; i <= playersV2.Count; i++)
            {
                playersV2[i].Draw(spriteBatch, gameTime);
            }


        }

        public static void InitPlayersList()
        {
            playersV2.Clear();
        }

        public static void AddPlayerV2(int ID)
        {
            if(!playersV2.ContainsKey(ID))
                playersV2.Add(ID, new PlayerV2(Vector2.Zero, ID));
        }

        /// <summary>
        /// For multiplayer
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="_myPlayerID"></param>
        /// <param name="_clientID"></param>
        public static void AddPlayerV2(int ID, int _myPlayerID)
        {
            if (!playersV2.ContainsKey(ID))
                playersV2.Add(ID, new PlayerV2(Vector2.Zero, ID, _myPlayerID));
        }


        public static void RemoveActor(Actor actor)
        {

            if (NetPlay.IsMultiplaying)
            {
                if (NetPlay.MyPlayerID() == 1)
                {
                    if (actor.actorType == Actor.ActorType.Object && actor.GetID() == 5)
                        NetworkEngine_5._0.Server.ServerSender.SendCollectedKey(actors.IndexOf(actor), NetPlay.MyPlayerID());
                    else
                        NetworkEngine_5._0.Server.ServerSender.SendDistroyedObject(actors.IndexOf(actor));
                }
                else
                {
                    if (actor.actorType == Actor.ActorType.Object && actor.GetID() == 5)
                        NetworkEngine_5._0.Client.ClientSender.SendCollectedKey(actors.IndexOf(actor), NetPlay.MyPlayerID());
                    else
                        NetworkEngine_5._0.Client.ClientSender.SendDistroyedObject(actors.IndexOf(actor));
                }
            }

            actors.Remove(actor);

        }


    }
}
