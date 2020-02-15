using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Celones.DisplayManager.Simulation
{
    public class ControlPanel : Form
    {
        public Button aButton;
        public Button dButton;
        public Button bButton;
        public Button cButton;
        private Timer refresher;
        private IContainer components;
        public PixelBox screenImage;

        public ControlPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.refresher = new System.Windows.Forms.Timer(this.components);
            this.cButton = new Celones.DisplayManager.Simulation.Button();
            this.bButton = new Celones.DisplayManager.Simulation.Button();
            this.dButton = new Celones.DisplayManager.Simulation.Button();
            this.aButton = new Celones.DisplayManager.Simulation.Button();
            this.screenImage = new Celones.DisplayManager.Simulation.PixelBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenImage)).BeginInit();
            this.SuspendLayout();
            // 
            // refresher
            // 
            this.refresher.Interval = 50;
            this.refresher.Tick += new System.EventHandler(this.refresher_Tick);
            // 
            // cButton
            // 
            this.cButton.Location = new System.Drawing.Point(100, 114);
            this.cButton.Name = "cButton";
            this.cButton.Size = new System.Drawing.Size(35, 23);
            this.cButton.TabIndex = 4;
            this.cButton.Text = "▼";
            this.cButton.UseVisualStyleBackColor = true;
            // 
            // bButton
            // 
            this.bButton.Location = new System.Drawing.Point(56, 114);
            this.bButton.Name = "bButton";
            this.bButton.Size = new System.Drawing.Size(35, 23);
            this.bButton.TabIndex = 3;
            this.bButton.Text = "▶";
            this.bButton.UseVisualStyleBackColor = true;
            // 
            // dButton
            // 
            this.dButton.Location = new System.Drawing.Point(144, 114);
            this.dButton.Name = "dButton";
            this.dButton.Size = new System.Drawing.Size(35, 23);
            this.dButton.TabIndex = 2;
            this.dButton.Text = "▲";
            this.dButton.UseVisualStyleBackColor = true;
            // 
            // aButton
            // 
            this.aButton.Location = new System.Drawing.Point(12, 114);
            this.aButton.Name = "aButton";
            this.aButton.Size = new System.Drawing.Size(35, 23);
            this.aButton.TabIndex = 1;
            this.aButton.Text = "◀";
            this.aButton.UseVisualStyleBackColor = true;
            // 
            // screenImage
            // 
            this.screenImage.Location = new System.Drawing.Point(12, 12);
            this.screenImage.Name = "screenImage";
            this.screenImage.Size = new System.Drawing.Size(168, 96);
            this.screenImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.screenImage.TabIndex = 0;
            this.screenImage.TabStop = false;
            // 
            // ControlPanel
            // 
            this.ClientSize = new System.Drawing.Size(192, 148);
            this.Controls.Add(this.screenImage);
            this.Controls.Add(this.aButton);
            this.Controls.Add(this.bButton);
            this.Controls.Add(this.cButton);
            this.Controls.Add(this.dButton);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ControlPanel";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.ControlPanel_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ControlPanel_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ControlPanel_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.screenImage)).EndInit();
            this.ResumeLayout(false);

        }

        private void refresher_Tick(object sender, EventArgs e)
        {
            lock (screenImage.Image)
            {
                screenImage.Refresh();
            }
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            }
            refresher.Start();
        }

        private void ControlPanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: aButton.Press(); break;
                case Keys.Right: bButton.Press(); break;
                case Keys.Down: cButton.Press(); break;
                case Keys.Up: dButton.Press(); break;
            }
        }

        private void ControlPanel_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: aButton.Release(); break;
                case Keys.Right: bButton.Release(); break;
                case Keys.Down: cButton.Release(); break;
                case Keys.Up: dButton.Release(); break;
            }
        }

        private void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }
    }
}
