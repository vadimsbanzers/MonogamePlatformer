using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace myPlatformer;

public class MapReader
{


    public bool isRedPressed = true;

    public bool IsRedPressed
    {
        get { return isRedPressed; }
        set { isRedPressed = value; }
    }

    public bool isBluePressed = true;
    public bool IsBluePressed
    {
        get { return isBluePressed; }
        set { isBluePressed = value; }
    }
    
    public bool isYellowPressed = true;
    public bool IsYellowPressed
    {
        get { return isYellowPressed; }
        set { isYellowPressed = value; }
    }
    private readonly RenderTarget2D _target;
    private readonly RenderTarget2D _target2;
    public static readonly int TILE_SIZE = 16;

    List<Vector2> texturesRed = new List<Vector2>();
    List<Vector2> texturesBlue = new List<Vector2>();
    List<Vector2> texturesYellow = new List<Vector2>();

    Texture2D tileRedTex;
    Texture2D tileBlueTex;
    Texture2D tileYellowTex;

    private Texture2D cannonballTexture;

    private static MapReader instance;
    public static MapReader Instance
    {
        get
        {
            if (instance == null)
                instance = new MapReader();
            return instance;
        }
    }
    // the level map
    public static int[,] tiles = {
        {10 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 8 , 11},
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 ,40 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 ,40 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        {12 , 2 , 3 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 ,53 , 0 , 4 },
        { 6 , 6 , 6 , 0 , 0 ,33 , 33, 33, 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 8 , 1 , 2 , 2 , 2 ,13 },
        {10 , 8 , 9 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 , 35, 35, 0 , 7 , 8 , 8 , 8 , 8 , 8 ,11 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 ,34 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 ,40 , 0 ,34 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 ,34 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 ,41 ,41 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 ,41 , 0 ,41 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 ,50 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 4 },
        {12 , 2 , 3 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 4 },
        {10 , 8 , 9 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 , 0 , 0 ,35 ,35 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 ,33 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 2 , 2 , 2 ,13 },
        { 6 , 0 , 0 , 0 , 0 , 0 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 ,41 , 0 , 0 , 0 , 7 , 8 , 8 , 8 ,11 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 ,40 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 ,41 ,41 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,50 ,40 ,40 ,40 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 2 , 2 , 2 ,13 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 ,35 , 0 , 0 , 0 , 7 , 8 , 8 , 8 ,11 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 ,34 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 ,34 ,34 ,34 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,34 ,34 ,34 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,33 ,33 ,33 ,33 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,51 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 ,40 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,35 ,35 ,35 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 ,41 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 ,50 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 0 , 0 , 1 , 3 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 4 },
        { 6 , 0 , 0 , 0 , 1 , 2 ,13 , 6 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 ,41 ,40 , 0 , 0 ,40 , 0 , 4 },
        {12 , 2 , 2 , 2 ,13 , 5 , 5 ,12 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 ,20 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 , 2 ,13 },
    };
    public int[,] tilesSaved;// level map for reset
    
    public  int mapWidth;
    public  int mapHeight;

    public List<Items> items { get; private set; } = new List<Items>();
    public List<Objects> objects { get; private set; } = new List<Objects>();
    public List<Objects> cannonBalls { get; private set; } = new List<Objects>();

    public void RemoveItems(Predicate<Items> match) // remove items
    {
        items.RemoveAll(match);
    }
    public void RemoveObject(Predicate<Objects> match) // remove objects
    {
        cannonBalls.RemoveAll(match);
        //Debug.WriteLine($"Items remaining in list: {items.Count}");
    }

    public Vector2 spawnPoint;
    private static Rectangle[,] Colliders { get; } = new Rectangle[tiles.GetLength(0), tiles.GetLength(1)];

    public MapReader()
    {
        _target = new(Globals.GraphicsDevice, tiles.GetLength(1) * TILE_SIZE, tiles.GetLength(0) * TILE_SIZE);
        Initialize();

    }
    public void Initialize()
    {
        InitializeResources();

       // loading level assets

        var tile0tex = Globals.Content.Load<Texture2D>("Tiles\\0");
        Texture2D finishTexture = Globals.Content.Load<Texture2D>("Tiles/finish");
        Texture2D itemAppleTexture = Globals.Content.Load<Texture2D>("Items/Apple");
        Texture2D itemKiwiTexture = Globals.Content.Load<Texture2D>("Items/Kiwi");

        Texture2D cannonTexture = Globals.Content.Load<Texture2D>("Objects/cannonAnim");
        Texture2D trampolineTexture = Globals.Content.Load<Texture2D>("Objects/trampoline");

        cannonballTexture = Globals.Content.Load<Texture2D>("Objects/cannonball");


        var tile1tex = Globals.Content.Load<Texture2D>("Tiles\\1");
        var tile2tex = Globals.Content.Load<Texture2D>("Tiles\\2");
        var tile3tex = Globals.Content.Load<Texture2D>("Tiles\\3");
        var tile4tex = Globals.Content.Load<Texture2D>("Tiles\\4");
        var tile5tex = Globals.Content.Load<Texture2D>("Tiles\\5");
        var tile6tex = Globals.Content.Load<Texture2D>("Tiles\\6");
        var tile7tex = Globals.Content.Load<Texture2D>("Tiles\\7");
        var tile8tex = Globals.Content.Load<Texture2D>("Tiles\\8");
        var tile9tex = Globals.Content.Load<Texture2D>("Tiles\\9");
        var tile10tex = Globals.Content.Load<Texture2D>("Tiles\\10");
        var tile11tex = Globals.Content.Load<Texture2D>("Tiles\\11");
        var tile12tex = Globals.Content.Load<Texture2D>("Tiles\\12");
        var tile13tex = Globals.Content.Load<Texture2D>("Tiles\\13");

        


        mapWidth = tiles.GetLength(1);
        mapHeight = tiles.GetLength(0);
        Globals.GraphicsDevice.SetRenderTarget(_target);
        Globals.GraphicsDevice.Clear(Color.Transparent);

        Globals.SpriteBatch.Begin();

        for (int x = 0; x < tiles.GetLength(0); x++) // iterates through 2d array tiles fills up the level with textures
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Texture2D temp;
                var posX = y * TILE_SIZE;
                var posY = x * TILE_SIZE;

                if ((tiles[x, y] == 0) ||   (tiles[x, y] == 40) || (tiles[x, y] == 41) || (tiles[x, y] == 50) || (tiles[x, y] == 51) || (tiles[x, y] == 53) || (tiles[x, y] == 33) || (tiles[x, y] == 34) || (tiles[x, y] == 35) )
                {
                    temp = tile0tex;  // Use the background texture
                }

                else if (tiles[x, y] == 1) temp = tile1tex;
                else if (tiles[x, y] == 2) temp = tile2tex;
                else if (tiles[x, y] == 3) temp = tile3tex;
                else if (tiles[x, y] == 4) temp = tile4tex;
                else if (tiles[x, y] == 5) temp = tile5tex;

                else if (tiles[x, y] == 6) temp = tile6tex;
                else if (tiles[x, y] == 7) temp = tile7tex;
                else if (tiles[x, y] == 8) temp = tile8tex;
                else if (tiles[x, y] == 9) temp = tile9tex;
                else if (tiles[x, y] == 10) temp = tile10tex;

                else if (tiles[x, y] == 11) temp = tile11tex;
                else if (tiles[x, y] == 12) temp = tile12tex;
                else if (tiles[x, y] == 13) temp = tile13tex;
                
                else
                {
                    if (tiles[x, y] == 20) // spawn
                    {
                        spawnPoint = new Vector2(y * TILE_SIZE, x * TILE_SIZE - 128);
                        temp = tile2tex;
                    }
                    else continue;
                }

                var tex = temp;
                if ((tiles[x, y] != 40) && (tiles[x, y] != 41) && (tiles[x, y] != 50) && (tiles[x, y] != 51) && (tiles[x, y] != 53) && (tiles[x, y] != 33) && (tiles[x, y] != 34) && (tiles[x, y] != 35)) // colliders
                {
                    Colliders[x, y] = new(posX, posY, TILE_SIZE, TILE_SIZE);
                }
                Globals.SpriteBatch.Draw(tex, new Vector2(posX, posY), Color.White);
            }
        }

        Globals.SpriteBatch.End();
        Globals.GraphicsDevice.SetRenderTarget(null);

        for (int x = 0; x < tiles.GetLength(0); x++) // draws items and objcts
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y] == 40)
                {
                    Vector2 itemPosition = new Vector2(y * TILE_SIZE, x * TILE_SIZE);
                    
                    items.Add(new Items(itemAppleTexture, 14, 16, 16, itemPosition, 0.1f, 100));
                }
                if (tiles[x, y] == 41)
                {
                    Vector2 itemPosition = new Vector2(y * TILE_SIZE, x * TILE_SIZE);

                    items.Add(new Items(itemKiwiTexture, 17, 16, 16, itemPosition, 0.1f, 100));
                }
                if (tiles[x, y] == 50)
                {

                    Vector2 itemPosition = new Vector2(y * TILE_SIZE, x * TILE_SIZE);

                    objects.Add(new Objects(trampolineTexture, 1, itemPosition, TILE_SIZE, TILE_SIZE, 0.0f, 100, 50,false, this));

                }
                if (tiles[x, y] == 51)
                {

                    Vector2 itemPosition = new Vector2(y * TILE_SIZE, x * TILE_SIZE);

                    objects.Add(new Objects(cannonTexture, 1, itemPosition, TILE_SIZE, TILE_SIZE, 0.1f, 100, 51,false, this));

                }
                if (tiles[x, y] == 53)
                {

                    Vector2 itemPosition = new Vector2(y * TILE_SIZE, x * TILE_SIZE);

                    objects.Add(new Objects(finishTexture, 1, itemPosition, TILE_SIZE, TILE_SIZE, 0.1f, 100, 53, false, this));

                }
            }
        }

        tilesSaved = new int[tiles.GetLength(0) , tiles.GetLength(1)];
        for (int i = 0; i < tiles.GetLength(0); i++) // creates backup map for resets
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tilesSaved[i, j] = tiles[i, j];
            }
        }

    }

    private void InitializeResources()
    {
        // Load textures here
        tileRedTex = Globals.Content.Load<Texture2D>("Tiles\\34");
        tileBlueTex = Globals.Content.Load<Texture2D>("Tiles\\33");
        tileYellowTex = Globals.Content.Load<Texture2D>("Tiles\\35");

        
    }
    public static List<Rectangle> GetNearestColliders(Rectangle bounds, bool includeRed, bool includeBlue, bool includeYellow) // colliders for changing tiles
    {
        int leftTile = (int)Math.Floor((float)bounds.Left / TILE_SIZE);
        int rightTile = (int)Math.Ceiling((float)bounds.Right / TILE_SIZE) - 1;
        int topTile = (int)Math.Floor((float)bounds.Top / TILE_SIZE);
        int bottomTile = (int)Math.Ceiling((float)bounds.Bottom / TILE_SIZE) - 1;

        leftTile = MathHelper.Clamp(leftTile, 0, tiles.GetLength(1));
        rightTile = MathHelper.Clamp(rightTile, 0, tiles.GetLength(1));
        topTile = MathHelper.Clamp(topTile, 0, tiles.GetLength(0));
        bottomTile = MathHelper.Clamp(bottomTile, 0, tiles.GetLength(0));

        List<Rectangle> result = new List<Rectangle>();

        for (int x = topTile; x <= bottomTile; x++)
        {
            for (int y = leftTile; y <= rightTile; y++)
            {
                if (tiles[x, y] == 0) continue;

                bool isRedTile = tiles[x, y] == 34; 
                bool isBlueTile = tiles[x, y] == 33; 
                bool isYellowTile = tiles[x, y] == 35; 

                // Skip adding the collider if the tile should not be included
                if (isRedTile && !includeRed || isBlueTile && !includeBlue || isYellowTile && !includeYellow)
                {
                    continue;
                }

                result.Add(Colliders[x, y]);
            }
        }

        return result;
    }

    public void Update(GameTime gt)
    {


        var keyboardState = Keyboard.GetState(); // change platforms
        if (keyboardState.IsKeyDown(Keys.NumPad1) && (isBluePressed || isRedPressed))
        {
            isYellowPressed = true;
            isBluePressed = false;
            isRedPressed = false;
            PlatformUpdate();
        }
        if (keyboardState.IsKeyDown(Keys.NumPad3) && (isBluePressed || isYellowPressed))
        {
            
            isRedPressed = true;
            isBluePressed = false;
            isYellowPressed = false;
            PlatformUpdate();

        }
        if (keyboardState.IsKeyDown(Keys.NumPad2) && (isRedPressed || isYellowPressed))
        {
            
            isBluePressed = true;
            isRedPressed = false;
            isYellowPressed = false;
            PlatformUpdate();
        }
        
        foreach (var item in items) // updating items list
        {
            
            if (item.IsActive)  
            {
 
                item.Update(gt);
            }
        }
        foreach (var i in objects) // updating objects list
        {

            i.Update(gt);  
        }
        foreach (var i in cannonBalls) // updating cannonballs list
        {
            i.Update(gt);
        }

    }
    public void CreateCannonball(Vector2 position) // creates cannonballs
    {
        
        cannonBalls.Add(new Objects(cannonballTexture, 3, new Vector2(position.X - 5, position.Y + 2),14,11, 0.1f, 0, 52, false, this));
    }
    public void PlatformUpdate() // updates changing platforms and their colliders
    {
       


        var tileRedTex = Globals.Content.Load<Texture2D>("Tiles\\34");
        var tileBlueTex = Globals.Content.Load<Texture2D>("Tiles\\33");
        var tileYellowTex = Globals.Content.Load<Texture2D>("Tiles\\35");



        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {

                if (tiles[x, y] > 0 && tiles[x, y] < 14) continue;

                var posX = y * TILE_SIZE;
                var posY = x * TILE_SIZE;
                Rectangle newCollider = new(posX, posY, TILE_SIZE, TILE_SIZE);

                if (tiles[x, y] == 33 && isBluePressed)
                {
                    texturesBlue.Add(new Vector2(posX, posY));
                    Colliders[x, y] = newCollider;
                }
                else if (tiles[x, y] == 34 && isRedPressed)
                {
                    texturesRed.Add(new Vector2(posX, posY));
                    Colliders[x, y] = newCollider;
                }
                else if (tiles[x, y] == 35 && isYellowPressed)
                {
                    texturesYellow.Add(new Vector2(posX, posY));
                    Colliders[x, y] = newCollider;
                }
                else
                {
                    Colliders[x, y] = Rectangle.Empty; 
                }
            }
        }

    }

    public void Draw() // drawing the level assets
    {

        Globals.SpriteBatch.Draw(_target, Vector2.Zero, Color.White);

        if (isBluePressed)
        {
            var tileBlueTex = Globals.Content.Load<Texture2D>("Tiles\\33");
            for (int i = 0; i < texturesBlue.Count; i++)
            {
                Globals.SpriteBatch.Draw(tileBlueTex, texturesBlue[i], Color.White);
            }
        }
        if (isRedPressed)
        {
            var tileRedTex = Globals.Content.Load<Texture2D>("Tiles\\34");
            for (int i = 0; i < texturesRed.Count; i++)
            {
                Globals.SpriteBatch.Draw(tileRedTex, texturesRed[i], Color.White);
            }
        }
        if (isYellowPressed)
        {
            var tileYellowTex = Globals.Content.Load<Texture2D>("Tiles\\35");
            for (int i = 0; i < texturesYellow.Count; i++)
            {
                Globals.SpriteBatch.Draw(tileYellowTex, texturesYellow[i], Color.White);
            }
        }
        var tile0tex = Globals.Content.Load<Texture2D>("Tiles\\0");
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Texture2D temp;
                var posX = y * TILE_SIZE;
                var posY = x * TILE_SIZE;

                if (tiles[x, y] == 33 && !isBluePressed ||
                    tiles[x, y] == 34 && !isRedPressed ||
                    tiles[x, y] == 35 && !isYellowPressed)
                {
                    temp = tile0tex;  
                }

                else continue;
                Globals.SpriteBatch.Draw(temp, new Vector2(posX, posY), Color.White);
            }
        }
        foreach (var item in items)
        {
            
            if (item.IsActive)
            {
                
                item.Draw();
            }
            else
            {
                //Console.WriteLine($"Skipping drawing deactivated item at {item.Position}");
            }
        }
        foreach(var i in objects){
            i.Draw();
        }
        foreach(var i in cannonBalls){
            i.Draw();
        }

    }
    public void Reset() //resets the map to play again
    {
        cannonBalls.Clear();
        items.Clear();
        objects.Clear();

        tiles = tilesSaved;
        for (int i = 0; i < tilesSaved.GetLength(0); i++)
        {
            for (int j = 0; j < tilesSaved.GetLength(1); j++)
            {
                if (tiles[i, j] > 13)
                {


                    tiles[i, j] = tilesSaved[i, j];
                }
            }
        }

        Initialize();
    }
}