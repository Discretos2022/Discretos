using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Objects
{
    public class Door : ObjectV2
    {

        public DoorVariante variante;

        public DoorState state;
        public DoorState oldState;

        public new bool isLocked;

        public int trigger; 

        public Door(Vector2 Position, DoorVariante variante = DoorVariante.wood, int trigger = -1) : base(Position)
        {

            this.variante = variante;
            this.trigger = trigger;

            Init();

        }

        public override void Init()
        {
            objectID = ObjectID.wood_door;
            hitbox.isEnabled = true;

            if (trigger == -1) hitbox.isEnabled = false;

        }

        public override void Update(GameTime gameTime)
        {

            oldState = state;
            state = DoorState.close;

            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Actor player = Handler.playersV2[i];

                if (GetRectangle().Intersects(player.GetRectangle()))
                {
                    if(oldState == DoorState.close)
                    {
                        if (GetRectangle().X < player.GetRectangle().X)
                            state = DoorState.openLeft;
                        else if (GetRectangle().X > player.GetRectangle().X)
                            state = DoorState.openRight;
                    }
                    else
                        state = oldState;

                }
                    

            }

            for (int i = 0; i < Handler.actors.Count; i++)
            {
                Actor a = Handler.actors[i];

                if (a.actorType == ActorType.Enemy)
                    if (GetRectangle().Intersects(a.GetRectangle()))
                    {
                        if (oldState == DoorState.close)
                        {
                            if (GetRectangle().X < a.GetRectangle().X)
                                state = DoorState.openLeft;
                            else if (GetRectangle().X > a.GetRectangle().X)
                                state = DoorState.openRight;
                        }
                        else
                            state = oldState;

                    }

            }


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            base.Draw(spriteBatch, gameTime);


            if (state == DoorState.openRight)
            {
                if (!hitbox.isEnabled)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(5, 0), new Rectangle(0, 0, 20, 32), Color.White);
                else
                {
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(5, 0), new Rectangle(21, 0, 16, 32), Color.White);
                    spriteBatch.Draw(Main.Cadenas, Position + new Vector2(0, 10), new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
            else if (state == DoorState.openLeft)
            {
                if (!hitbox.isEnabled)
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(5, 0) - new Vector2(15, 0), new Rectangle(0, 0, 20, 32), Color.White, NumOfTriggerObject, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                else
                {
                    spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(5, 0), new Rectangle(21, 0, 16, 32), Color.White);
                    spriteBatch.Draw(Main.Cadenas, Position + new Vector2(0, 10), new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
            else
            {
                spriteBatch.Draw(Main.Object[(int)objectID], Position + new Vector2(5, 0), new Rectangle(21, 0, 16, 32), Color.White);
                if (hitbox.isEnabled)
                    spriteBatch.Draw(Main.Cadenas, Position + new Vector2(0, 10), new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }


        }

        public override void UpdateHitbox()
        {
            hitbox.rectangle = new Rectangle((int)Position.X + 5, (int)Position.Y, 6, 32);
        }


        public enum DoorVariante
        {

            wood = 1,

        }

        public enum DoorState
        {
            close = 0,
            openRight = 1,
            openLeft = 2,
        }

    }
}
