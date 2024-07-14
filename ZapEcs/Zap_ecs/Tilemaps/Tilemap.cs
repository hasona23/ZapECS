using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Zap_ecs.Tilemaps
{
    public class Tilemap
    {
        //takes a .csv file and a dict of rextures to use
        private Dictionary<int,Texture2D> textureDict;
        private Dictionary<int, Rectangle> rectDict;
        private Texture2D tileSpritesheet;
        private Dictionary<Vector2, int> tilemap;
        private string filepath;
        private int tileSize;
        private Vector2 origin;
        private float scale;

        
        public CollisionMode collisionMode;

        public Tilemap(string filepath, Dictionary<int, Texture2D> texturesDict, int tileSize, Vector2 origin,CollisionMode collisionMode = CollisionMode.None, float scale = 1) 
        {
            this.filepath = filepath;
            textureDict = texturesDict;
            tilemap = new Dictionary<Vector2, int>();
            this.tileSize = tileSize;
            this.origin = origin;
            this.scale = scale;
            this.collisionMode = collisionMode;
        }

        // takes dict of vector or pos of texture in tilemap if use rect mathod
        public Tilemap(string filepath, Dictionary<int, Rectangle> texturesDict,Texture2D tileSpritesheet,int tileSize, Vector2 origin,CollisionMode collisionMode = CollisionMode.None, float scale = 1)
        {
            this.filepath = filepath;
            rectDict = texturesDict;
            tilemap = new Dictionary<Vector2, int>();
            this.tileSpritesheet = tileSpritesheet;
            this.tileSize = tileSize;
            this.origin = origin;
            this.scale = scale;
            this.collisionMode = collisionMode;

        }

        public void LoadMap() 
        {
            StreamReader file = new StreamReader(filepath);
            Dictionary<Vector2,int> results = new Dictionary<Vector2, int>();
            string line;
            int y = 0;
            while ((line = file.ReadLine()) != null) 
            {
               string[] items = line.Split(',');
               
               for(int x = 0; x < items.Length; x++) 
               {
                    if(int.TryParse(items[x], out int val) && val != -1) 
                    {
                        results[new Vector2(x, y)] = val;
                    } 
               }
               y++;
            }
            tilemap = results;
        }

        public List<Tile> GetCollisions(params int[] keys) 
        {

            if (collisionMode == CollisionMode.None)
                return new List<Tile>();

            List<Tile> collisions = new List<Tile>();
            foreach (var tile in tilemap)
            {
                if (!keys.Contains(tile.Value))
                    continue;
                Tile tileRect = new Tile();
                tileRect.rect = new Rectangle(GetXPos(tile), GetYPos(tile),GetScale(), GetScale());
                collisions.Add(tileRect);

                Debug.WriteLine(tileRect.rect);
            }
            return collisions;
        }
        
        

        public void DrawMapTextures(SpriteBatch spriteBatch) 
        {
            foreach (var tile in tilemap)
            {
                if (textureDict.ContainsKey(tile.Value)) 
                {
                    spriteBatch.Draw(textureDict[tile.Value], new Rectangle(GetXPos(tile), GetYPos(tile),GetScale(),GetScale()),null, Color.White);
                }
                
            } 
        }
        
        public void DrawMapTilemap(SpriteBatch spriteBatch)
        {
            foreach (var tile in tilemap)
            {
                if (rectDict.ContainsKey(tile.Value)) 
                {
                    Rectangle src = rectDict[tile.Value];
                    //spriteBatch.Draw(tileSpritesheet,new Rec,src,Color.White);
                    spriteBatch.Draw(tileSpritesheet, new Rectangle(new Point(GetXPos(tile), GetYPos(tile)), new Point((int)(src.Width * scale), (int)(src.Height * scale))), src, Color.White);
                }
            }
        
        }
        private int GetScale()
        {
            return (int)(tileSize * scale);
        }
        private int GetXPos(KeyValuePair<Vector2, int> tile)
        {
            return (int)(tile.Key.X * scale * tileSize + origin.X);
        }
        private int GetYPos(KeyValuePair<Vector2, int> tile)
        {
            return (int)(tile.Key.Y * scale * tileSize + origin.Y);
        }
    }
    public class Tile 
    {
        //public Collision Component
        public Rectangle rect;
    }
    public enum CollisionMode
    {
        None = default,
        Trigger,
        RigidBody,
    }

}
