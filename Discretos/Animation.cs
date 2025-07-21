using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Plateform_2D_v9
{
    class Animation
    {

        private Texture2D Texture;
        private Rectangle Size;
        private int NumberOfImage;
        private int Vitesse;
        private int index = 0;


        private readonly List<Rectangle> _sourceRectangles = new List<Rectangle>();
        private readonly int frames;
        private int frame;
        private readonly float frameTime;
        private float frameTimeLeft;
        private bool active = true;

        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
        {
            Texture = texture;
            this.frameTime = frameTime;
            frameTimeLeft = frameTime;
            frames = framesX;
            var frameWidth = Texture.Width / framesX;
            var frameHeight = Texture.Height / framesY;

            for (int i = 0; i < frames; i++)
            {
                _sourceRectangles.Add(new Rectangle(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
            }
        }

        public void Stop()
        {
            active = false;
        }

        public void Start()
        {
            active = true;
        }

        public void Reset()
        {
            frame = 0;
            frameTimeLeft = frameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!active) return;

            frameTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimeLeft <= 0)
            {
                frameTimeLeft += frameTime;
                frame = (frame + 1) % frames;
            }
        }

        public void UpdateReverse(GameTime gameTime)
        {
            if (!active) return;

            frameTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimeLeft <= 0)
            {
                frameTimeLeft += frameTime;
                frame = (frame - 1) % frames;
                if (frame < 0) frame = frames + frame;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, SpriteEffects effect = SpriteEffects.None)
        {
            spriteBatch.Draw(Texture, pos, _sourceRectangles[frame], Color.White, 0, Vector2.Zero, 1f, effect, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color color, SpriteEffects effect = SpriteEffects.None)
        {
            spriteBatch.Draw(Texture, pos, _sourceRectangles[frame], color, 0, Vector2.Zero, 1f, effect, 0f);
        }

        public int GetFrame()
        {
            return frame;
        }

        public List<Rectangle> GetSourceRectangle()
        {
            return _sourceRectangles;
        }

    }
}
