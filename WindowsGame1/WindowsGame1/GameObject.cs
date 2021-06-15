using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/*
 * Used for game objects (sprites) 
 */
namespace WindowsGame1
{
    class GameObject
    {
        private Texture2D objectTexture;
        private Vector2 objectPosition;
        private Vector2 collisionOffset = new Vector2(3,3);
        private int objectHeight;
        private int objectWidth;
        private Rectangle objectHitBox;
        private float objectScale = 1;


        public float objectAngle = 0;
        public Vector2 objectSpeed;
        public Vector2 objectAcceleration;


        /// <summary>
        /// constructor with texture + position
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="pos"></param>
        public GameObject(Texture2D texture, Vector2 pos)
        {
            objectTexture = texture;
            objectHeight = texture.Height;
            objectWidth = texture.Width;
            objectPosition = pos;
            RecalcHitBox();
        }


        /// <summary>
        /// Return Vector2 of the objects position
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 GetObjectPosition()
        {
            return objectPosition;
        }

        /// <summary>
        /// Setter for object position, updates hitbox
        /// </summary>
        /// <param name="pos"></param>
        public virtual void SetObjectPostition(Vector2 pos)
        {
            objectPosition = pos;
            RecalcHitBox();
        }

        /// <summary>
        /// Adds the input vector to the object position
        /// </summary>
        /// <param name="posDelta"></param>
        public virtual void UpdateObjectPosition (Vector2 posDelta)
        {
            objectPosition += posDelta;
            RecalcHitBox();
        }


        /// <summary>
        ///  Greater values will shrink bounding box tighter to object
        /// </summary>
        /// <param name="offset"></param>
        public virtual void UpdateCollisionOffest(Vector2 offset)
        {
            collisionOffset.X = (int)(offset.X * objectScale);
            collisionOffset.Y = (int)(offset.Y * objectScale);
            RecalcHitBox();
        }

        /// <summary>
        /// Used to update the hitbox of the object after a change has been made to its member variables
        /// </summary>
        protected virtual void RecalcHitBox()
        {
            objectHitBox = new Rectangle
                 ((int)(objectPosition.X - (objectWidth / 2) + collisionOffset.X), (int)(objectPosition.Y - (objectHeight / 2) + collisionOffset.Y),
                 (int)(objectWidth - (collisionOffset.X * 2)), (int)(objectHeight - (collisionOffset.Y * 2)));
        }

        /// <summary>
        /// Updates the scale of the image and its hitbox
        /// </summary>
        /// <param name="scale"></param>
        public virtual void UpdateScale(float scale)
        {
            objectScale = scale;
            collisionOffset.X = (int)(collisionOffset.X * objectScale);
            collisionOffset.Y = (int)(collisionOffset.Y * objectScale);
            objectHeight = (int) (objectHeight* scale);
            objectWidth = (int)(objectWidth * scale);
            RecalcHitBox();
        }

        /// <summary>
        /// Draw image in Back to Front order and AlphaBlend
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            Vector2 origin = new Vector2(objectWidth / (2 * objectScale), objectHeight / (2 * objectScale));
            spriteBatch.Draw(objectTexture, objectPosition, null, Color.White, 0, origin, objectScale, SpriteEffects.None, 1);
            spriteBatch.End();
        }

        /// <summary>
        /// Checks if this objects hitbox intersects with the input Rect
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public virtual bool IsCollision(Rectangle otherObject)
        {
            if (objectHitBox.Intersects(otherObject))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Displays this objects hitbox on screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphics"></param>
        public virtual void DrawHitBox(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            // At the top of your class:
            Texture2D pixel;

            // Somewhere in your LoadContent() method:
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(objectHitBox.X, objectHitBox.Y, objectHitBox.Width, (int)1), Color.GreenYellow);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(objectHitBox.X, objectHitBox.Y, 1, objectHitBox.Height), Color.GreenYellow);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((objectHitBox.X + objectHitBox.Width - 1),
                                            objectHitBox.Y,
                                            1,
                                            objectHitBox.Height), Color.GreenYellow);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(objectHitBox.X,
                                            objectHitBox.Y + objectHitBox.Height - 1,
                                            objectHitBox.Width,
                                            1), Color.GreenYellow);
            spriteBatch.End();
        }
    }
}
