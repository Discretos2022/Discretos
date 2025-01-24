using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.NetCore;
using Plateform_2D_v9.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Plateform_2D_v9
{
    class Handler
    {

        public static List<Actor> actors = new List<Actor>();
        public static List<TileV2> solids = new List<TileV2>();
        public static List<Wall> walls = new List<Wall>();
        public static List<Actor> ladder = new List<Actor>();

        public static Dictionary<int, PlayerV2> playersV2 = new Dictionary<int, PlayerV2>();

        public static TileV2[,] Level;

        public static Wall[,] Walls;

        public static List<MovingBlock> blocks;

        public static float earthquake = 0.0f;


        public static void Initialize()
        {
            solids = new List<TileV2>();
            walls = new List<Wall>();
            actors = new List<Actor>();
            ladder = new List<Actor>();
            blocks = new List<MovingBlock>();

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

            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Update(gameTime);
            }

            for (int i = 1; i <= playersV2.Count; i++)
            {
                playersV2[i].Update(gameTime);
            }




            #region Left Collision

            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].LeftDisplacement(gameTime);
            for (int i = 0; i < actors.Count; i++) actors[i].LeftDisplacement(gameTime);

            for (int i = 0; i < blocks.Count; i++) blocks[i].RightDisplacement(gameTime);


            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].LeftDynamicCollision();
            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].LeftStaticCollision();

            for (int i = 0; i < actors.Count; i++) actors[i].LeftDynamicCollision();
            for (int i = 0; i < actors.Count; i++) actors[i].LeftStaticCollision();

            #endregion


            #region Right Collision

            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].RightDisplacement(gameTime);
            for (int i = 0; i < actors.Count; i++) actors[i].RightDisplacement(gameTime);


            for (int i = 0; i < blocks.Count; i++) blocks[i].LeftDisplacement(gameTime);
            
            


            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].RightDynamicCollision();
            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].RightStaticCollision();

            /*Bug*/ //if (!playersV2[1].isDead) playersV2[1].LeftDynamicCollision();
            /*Bug*/ //if (!playersV2[1].isDead) playersV2[1].LeftStaticCollision();

            for (int i = 0; i < actors.Count; i++) actors[i].RightDynamicCollision();
            for (int i = 0; i < actors.Count; i++) actors[i].RightStaticCollision();

            #endregion


            #region Down Collision

            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].DownDisplacement(gameTime);
            for (int i = 0; i < actors.Count; i++) actors[i].DownDisplacement(gameTime);


            for (int i = 0; i < blocks.Count; i++) blocks[i].UpDisplacement(gameTime);


            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].DownDynamicCollision();
            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].DownStaticCollision();
            for (int i = 0; i < actors.Count; i++) actors[i].DownDynamicCollision();
            for (int i = 0; i < actors.Count; i++) actors[i].DownStaticCollision();

            #endregion


            #region Up Collision

            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].UpDisplacement(gameTime);
            for (int i = 0; i < actors.Count; i++) actors[i].UpDisplacement(gameTime);


            for (int i = 0; i < blocks.Count; i++) blocks[i].DownDisplacement(gameTime);


            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].UpDynamicCollision();
            if (!playersV2[NetPlay.MyPlayerID()].isDead) playersV2[NetPlay.MyPlayerID()].UpStaticCollision();
            for (int i = 0; i < actors.Count; i++) actors[i].UpDynamicCollision();
            for (int i = 0; i < actors.Count; i++) actors[i].UpStaticCollision();

            #endregion



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
                    if ((int)(Walls[i, j].ID) > 0)
                        Walls[i, j].Draw(spriteBatch, gameTime);



            for (int j = yMin; j < yMax; j++)
                for (int i = xMin; i < xMax; i++)
                    if (Level[i, j].ID > 0)
                        Level[i, j].Draw(spriteBatch, gameTime);


            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw(spriteBatch, gameTime);
            }



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
                    if (actor.actorType == Actor.ActorType.Object && ((ObjectV2)actor).objectID == ObjectV2.ObjectID.gold_key)
                        NetworkEngine_5._0.Server.ServerSender.SendCollectedKey(actors.IndexOf(actor), NetPlay.MyPlayerID());
                    else
                        NetworkEngine_5._0.Server.ServerSender.SendDistroyedObject(actors.IndexOf(actor));
                }
                else
                {
                    if (actor.actorType == Actor.ActorType.Object && ((ObjectV2)actor).objectID == ObjectV2.ObjectID.gold_key)
                        NetworkEngine_5._0.Client.ClientSender.SendCollectedKey(actors.IndexOf(actor), NetPlay.MyPlayerID());
                    else
                        NetworkEngine_5._0.Client.ClientSender.SendDistroyedObject(actors.IndexOf(actor));
                }
            }

            actors.Remove(actor);

        }


    }
}
