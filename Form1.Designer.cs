using System;
using System.Threading;

namespace WinFormTest;

public partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>

    private void InitializeComponent()
    {
        int FormX = (int)Core.FormXY.X;
        int FormY = (int)Core.FormXY.Y;
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(FormX, FormY);
        this.Text = "Form1";
    }


    /// <summary>
    /// 重写ProcessDialogKey，来允许监听方向键
    /// </summary>
    /// <param name="keycode"></param>
    /// <returns></returns>

    public Button SGB = new Button();
    public Button KBlurdBu = new Button();
    public Button EGBu = new Button();
    public Label COXY = new Label();
    public bool SGBtn = true;
    public bool[] KeyEvt = {false,false,false,false};

    private void StartGameE(object sender, EventArgs e)
    {
        //private SynchronizationContext mainThreadSynContext;
        //mainThreadSynContext = SynchronizationContext.Current;
        SGBtn = false;
        this.Controls.Remove(SGB);
        ThreadStart OpenEM = new ThreadStart(ShowMap);
        Thread ExcuteMap = new Thread(OpenEM);
        ExcuteMap.Start();
        /*ThreadStart OpenKB = new ThreadStart(KeyCtrl);
        Thread StartKBCtl = new Thread(OpenKB);
        StartKBCtl.Start();*/
        KeyCtrl();
        ExitGame();
        CmtOfXY();
    }
    private void ExitGameE(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }
    
    protected override bool ProcessDialogKey(Keys keycode)
    {
        switch (keycode)
        {
            case Keys.Left:
            case Keys.Up:
            case Keys.Right:
            case Keys.Down:
            return false;
        }
        return true;
    }
    
    public void KBCtlDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
       {
            case Keys.Left:
                KeyEvt[0] = true;
                break;
            case Keys.Up:
                KeyEvt[1] = true;
                break;
            case Keys.Right:
                KeyEvt[2] = true;
                break;
            case Keys.Down:
                KeyEvt[3] = true;
                break;
        }
    }
    private void KBCtlUp(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Left:
                KeyEvt[0] = false;
                break;
            case Keys.Up:
                KeyEvt[1] = false;
                break;
            case Keys.Right:
                KeyEvt[2] = false;
                break;
            case Keys.Down:
                KeyEvt[3] = false;
                break;
        }
    }


    #endregion
}
