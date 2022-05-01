using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Input;

namespace WinFormTest;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        this.KeyPreview = true; 

        StartGame();
    }
    
    public void StartGame()
    {
        int SGBx = 120;
        int SGBy = 40;
        SGB.Size = new Size(SGBx, SGBy);
        SGB.Location = new Point(((int)Core.FormXY.X/2)-(SGBx/2), ((int)Core.FormXY.Y/2)-(SGBy/2));
        SGB.Text = "Start Game";
        this.Controls.Add(SGB);
        SGB.Click += new EventHandler(StartGameE);
        
    }

    public void KeyCtrl()
    {
        KBlurdBu.Size = new Size(0,0);
        KBlurdBu.Location = new Point(1000,1000);
        this.Controls.Add(KBlurdBu);
        KBlurdBu.KeyDown += new KeyEventHandler(KBCtlDown);
        KBlurdBu.KeyUp += new KeyEventHandler(KBCtlUp);
    }

    public void CmtOfXY()
    {
        COXY.Location = new Point(10,(int)Core.FormXY.Y+10);
        COXY.Size = new Size(100,40);
        this.Controls.Add(COXY);
    }

    public void ExitGame()
    {
        EGBu.Location = new Point(10+100+10,(int)Core.FormXY.Y+10);
        EGBu.Size = new Size(120,70);
        EGBu.Text = "Exit Game";
        this.Controls.Add(EGBu);
        EGBu.Click += new EventHandler(ExitGameE);
        
    }

    class ShowMap
    {
        Graphics Map = this.CreateGraphics();
        SolidBrush bbrush = new SolidBrush(Color.Blue);
        SolidBrush gbrush = new SolidBrush(Color.Green);

        SolidBrush PlayBh = new SolidBrush(Color.Cyan);
        SolidBrush Enemy = new SolidBrush(Color.Red);
        
        Core GetData = new Core();
        
        int? RandNum = null;
        int? Chunkx = null;
        int? Chunky = null; 
        int halfBlock = 2;
        int bWidth = (int)Core.BlockAtbs.Width;
        int bHeight = (int)Core.BlockAtbs.Height;
        
        object[] getAry = GetData.GenerateMap(RandNum,Chunkx,Chunky);
        int[,] MapChunk = (int[,])getAry[0]; 
        RandNum = (int)getAry[1];
        Chunkx = (int)getAry[2];
        Chunky = (int)getAry[3];
        int[] SpwanXY = GetData.SpwanPoint((int)RandNum,(int)Chunkx,(int)Chunky);
        int Pointx = SpwanXY[0];
        int Pointy = SpwanXY[1];

        int[ , ] NowChunk = MapChunk;

        //一次showcharacter事件后showmap
        
        public void ShowMap()
        {
            while(true)
            {  
                if (Pointx >= (int)Core.MapAtbs.Chunkx || Pointx <= -1)
                {
                    if (Pointx <= -1)
                    {
                        Chunkx --; 
                        Pointx = (int)Core.MapAtbs.Chunkx;
                    }
                    else
                    {
                        Chunkx ++;
                        Pointx = 0;
                    }
                    object[] GetMap = GetData.GenerateMap((int)RandNum , (int)Chunkx , (int)Chunky);
                    NowChunk = (int[,])GetMap[0];
                }
                if (Pointy >= (int)Core.MapAtbs.Chunky || Pointy <= -1)
                {
                    if (Pointy <= -1)
                    {
                        Chunky --;
                        Pointy = (int)Core.MapAtbs.Chunky;
                    }
                    else
                    {
                        Chunky ++;
                        Pointy = 0;
                    }
                    object[] GetMap = GetData.GenerateMap((int)RandNum , (int)Chunkx , (int)Chunky);
                    NowChunk = (int[,])GetMap[0];
                }
                COXY.Text = $"Pxy = {Pointx},{Pointy} \nCxy = {Chunkx},{Chunky}";
                for (int iy = 0 ; iy < (int)Core.MapAtbs.Chunky ; iy++)
                {
                    for (int ix = 0 ; ix < (int)Core.MapAtbs.Chunkx ; ix++)
                    {
                        Rectangle rect = new Rectangle(bWidth*ix , bHeight*iy , bWidth, bHeight);
                        if (ix == Pointx && iy == Pointy)
                        {
                            Map.FillRectangle(PlayBh,rect);
                        }
                        else if (NowChunk[ix,iy] <= halfBlock)
                        {   
                            Map.FillRectangle(bbrush,rect);
                        }
                        else
                        {
                            Map.FillRectangle(gbrush,rect);
                        }
                    }
                }
            }
        }
        public void ShowCharacter()
        {
            while (true)
            {
                ShowMap();
                while (true)
                {
                    //左 上 右 下
                    if(KeyEvt[0] == true)
                    {
                        Pointx --;
                        break;
                    }
                    if(KeyEvt[1] == true)
                    {
                        Pointy --;
                        break;
                    }
                    if(KeyEvt[2] == true)
                    {
                        Pointx ++;
                        break;
                    }
                    if(KeyEvt[3] == true)
                    {
                        Pointy ++;
                        break;
                    }
                }
            
            }
        }
        public void ShowNetCharacter()
        {
            Rectangle NetPlayer = new Rectangle();
            NetCha = CSObj();
            int NowNPCIC = NetCha[0];
            int ActionNum = -1;
            int LastPx;
            int LastPy;
            int LastPType;
            while (true)
            {
                NetCha = CSObj();
                ActionNum ++;
                int NowNPCx = NetCha[1];
                int NowNPCy = NetCha[2];
                int NowNPx = NetCha[3];
                int NowNPy = NetCha[4];
                string NowNPName = NetCha[5];
                if (NowNPCx != Chunkx || NowNPCy != Chunky)
                {
                    continue;
                }
                else
                {
                    NowChunk[NowNPCx,NowNPCy] = NowNPCIC;
                    if (ActionNum >= 1)
                    {
                        //如果上一次行动，则还原上一次行动
                        NowChunk[LastPx,LastPy] = LastPType;
                    }
                }
                if (ActionNum >= 1)
                {
                    LastPx = NowNPCx;
                    LastPy = NowNPCy;
                    LastPType = NowChunk[LastPx,LastPy];
                }
                ShowMap();
                
            }
        }
        public void ShowEvent()
        {

        }

        public int[] CSObj()
        {
            //socket
        }
    }

    public void ShowMap()
    {
        }
        

    }
}
