namespace WinFormTest;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}

class Core
{
    //定义了 窗口的大小 方块的绘制属性 地图的属性
    public enum FormXY
    {
        X = 640,
        Y = 640,
    }
    public enum BlockAtbs
    {
        Height = 20,
        Width = 20,
    }

    public enum EnemyType
    {

    }

    public enum MapAtbs
    {
        Chunk = MapAtbs.VisableChunkx*MapAtbs.VisableChunky,
        Chunkx = 32,
        Chunky = 32,
        ScrX = FormXY.X/BlockAtbs.Width,
        ScrY = FormXY.Y/BlockAtbs.Height,
        VisableChunkx = 3,
        VisableChunky = 3,
    }

    //地图的核心算法
    private int Noise(int xPos, int yPos, int xChunk, int yChunk, int RandCut)
    {
        int ChunkCo = (xChunk + RandCut) * (yChunk + RandCut);
        int xyPosCo = (xPos + RandCut) * (yPos + RandCut);
        int xyChunkCo = ChunkCo * xyPosCo;
        int noise = (( 1 + ChunkCo) ^ 4) % (xyChunkCo ^ 3+ 1) ;
        return noise;
        
    }

    //出生点的算法
    private int[] SpwanPoint(int RandNum , int Chunkx , int Chunky)
    {
        string RN = Convert.ToString(RandNum);
        int RNlen = RN.Length;
        int RSpwanChunkx = Convert.ToInt32(RN.Substring(RNlen-1,1));
        int RSpwanChunky = Convert.ToInt32(RN.Substring(RNlen-2,1));
        int Pointx = Convert.ToInt32(RN.Substring(((RSpwanChunkx+1)^2)%RNlen,1));
        int Pointy = Convert.ToInt32(RN.Substring(((RSpwanChunky+1)^2)%RNlen,1));
        int[] Pointxy = new int[2];
        Pointxy[0] = (int)Pointx;
        Pointxy[1] = (int)Pointy;
        return Pointxy;
    }

    //出生区块的算法
    private int[] SpwanChunk(int RandNum , int SeedLength)
    {
        string RN = Convert.ToString(RandNum);
        int RSpwanChunkx = Convert.ToInt32(RN.Substring(0,1));
        int RSpwanChunky = Convert.ToInt32(RN.Substring(1,1));
        int Chunkx = Convert.ToInt32(RN.Substring(((RSpwanChunkx+1)^2)%SeedLength,1));
        int Chunky = Convert.ToInt32(RN.Substring(((RSpwanChunky+1)^2)%SeedLength,1));
        int[] Chunkxy = new int[2];
        Chunkxy[0] = (int)Chunkx;
        Chunkxy[1] = (int)Chunky;
        return Chunkxy;
    }

    //生成地图 打包成object
    public object[] GenerateMap(int? NRandNum , int? NChunkx , int? NChunky)
    {
        int? Chunky;
        int? Chunkx;
        int RandNum;
        if (NRandNum == null)
        {
            Random Rand = new Random();
            RandNum = Rand.Next();   
        }
        else
        {
            RandNum = (int)NRandNum;
        }
        string MapSeedCuts = Convert.ToString(RandNum);
        int SeedLength = MapSeedCuts.Length;
        if (NChunkx == null || NChunky == null)
        {
            int[] Chunkxy = SpwanChunk(RandNum , SeedLength);
            Chunkx = (int)Chunkxy[0];
            Chunky = (int)Chunkxy[1];
        }
        else
        {
            Chunkx = (int)NChunkx;
            Chunky = (int)NChunky;
        }

        int[ , ] MapBlocks = new int [(int)MapAtbs.Chunkx,(int)MapAtbs.Chunky];

        for (int iy = 0 ; iy < (int)MapAtbs.Chunky ; iy++)
        {
            for (int ix = 0 ; ix < (int)MapAtbs.Chunkx ; ix++)
            {
                int SeedCut = Convert.ToInt32(MapSeedCuts.Substring((ix^3*iy^2) % SeedLength, 1));
                if (SeedCut == 0)
                {
                    SeedCut = 5;
                }
                int noise = Noise(ix ,iy , (int)Chunkx , (int)Chunky , SeedCut);
                MapBlocks[ix , iy] = noise;
                
            }
        }
        object[] retVal;
        if (NRandNum == null || NChunkx == null || NChunky == null)
        {
            retVal = new object[4];
            retVal[0] = (object)MapBlocks;
            retVal[1] = (object)RandNum;
            retVal[2] = (object)Chunkx;
            retVal[3] = (object)Chunky;
            
        }
        else
        {
            retVal = new object[1];
            retVal[0] = MapBlocks;
        }
        MapBlocks = new int [(int)MapAtbs.Chunkx,(int)MapAtbs.Chunky];
        return retVal;
    }

    //敌人的生成算法
    public int[,] SpawnEnemy()
    {
        int[,] MapEnemies= new int[,];
        for (int iy = 0 ;iy <(int)MapAtbs.Chunky ; iy ++)
        {
            for (int ix = 0; ix < (int)MapAtbs.Chunkx ; ix ++)
            {
                Random rand = new Random();
                int RBase = rand.Next(0,5);
                //int MosType = RBase ^ 2 % (ix * iy);
                if (RBase == 0)
                {
                    MapEnemies[ix,iy] = 1;

                }
                else
                {
                    MapEnemies[ix,iy] = 0;
                }
            }
        }
        return MapEnemies;
    }
}
