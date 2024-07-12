using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap_ecs.Tilemaps
{
    public class TilemapRenderer
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
        public TilemapRenderer(string filepath, Dictionary<int, Texture2D> texturesDict, int tileSize, Vector2 origin, float scale = 1) 
        {
            this.filepath = filepath;
            textureDict = texturesDict;
            tilemap = new Dictionary<Vector2, int>();
            this.tileSize = tileSize;
            this.origin = origin;
            this.scale = scale;
        }

        // takes dict of vector or pos of texture in tilemap if use rect mathod
        public TilemapRenderer(string filepath, Dictionary<int, Rectangle> texturesDict,Texture2D tileSpritesheet,int tileSize, Vector2 origin, float scale = 1)
        {
            this.filepath = filepath;
            rectDict = texturesDict;
            tilemap = new Dictionary<Vector2, int>();
            this.tileSpritesheet = tileSpritesheet;
            this.tileSize = tileSize;
            this.origin = origin;
            this.scale = scale;

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
                    if(int.TryParse(items[x], out int val)) 
                    {
                        results[new Vector2(x, y)] = val;
                    } 
               }
               y++;
            }
            tilemap = results;
        }
        public void DrawMapTextures(SpriteBatch spriteBatch) 
        {
            foreach (var tile in tilemap)
            { 
                spriteBatch.Draw(textureDict[tile.Value], new Rectangle(new Point((int)(tile.Key.X * scale * tileSize + origin.X), (int)(tile.Key.Y * scale * tileSize + origin.Y)), new Point((int)(textureDict[tile.Value].Width * scale), (int)(textureDict[tile.Value].Height * scale))), Color.White);
            } 
        }
        public void DrawMapTilemap(SpriteBatch spriteBatch)
        {
            foreach (var tile in tilemap)
            {
                Rectangle src = rectDict[tile.Value];
                //spriteBatch.Draw(tileSpritesheet,new Rec,src,Color.White);
                spriteBatch.Draw(tileSpritesheet, new Rectangle(new Point((int)(tile.Key.X * scale * tileSize + origin.X), (int)(tile.Key.Y * scale * tileSize + origin.Y)), new Point((int)(src.Width * scale), (int)(src.Height * scale))),src, Color.White);

            }
    }   }
}
