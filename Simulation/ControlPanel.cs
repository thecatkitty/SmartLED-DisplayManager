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
        private Button aButton;
        private Button dButton;
        private Button bButton;
        private Button cButton;
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
            this.aButton = new System.Windows.Forms.Button();
            this.dButton = new System.Windows.Forms.Button();
            this.bButton = new System.Windows.Forms.Button();
            this.cButton = new System.Windows.Forms.Button();
            this.refresher = new System.Windows.Forms.Timer(this.components);
            this.screenImage = new Celones.DisplayManager.Simulation.PixelBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenImage)).BeginInit();
            this.SuspendLayout();
            // 
            // aButton
            // 
            this.aButton.Location = new System.Drawing.Point(12, 114);
            this.aButton.Name = "aButton";
            this.aButton.Size = new System.Drawing.Size(35, 23);
            this.aButton.TabIndex = 1;
            this.aButton.Text = "A";
            this.aButton.UseVisualStyleBackColor = true;
            // 
            // dButton
            // 
            this.dButton.Location = new System.Drawing.Point(144, 114);
            this.dButton.Name = "dButton";
            this.dButton.Size = new System.Drawing.Size(35, 23);
            this.dButton.TabIndex = 2;
            this.dButton.Text = "D";
            this.dButton.UseVisualStyleBackColor = true;
            // 
            // bButton
            // 
            this.bButton.Location = new System.Drawing.Point(56, 114);
            this.bButton.Name = "bButton";
            this.bButton.Size = new System.Drawing.Size(35, 23);
            this.bButton.TabIndex = 3;
            this.bButton.Text = "B";
            this.bButton.UseVisualStyleBackColor = true;
            // 
            // cButton
            // 
            this.cButton.Location = new System.Drawing.Point(100, 114);
            this.cButton.Name = "cButton";
            this.cButton.Size = new System.Drawing.Size(35, 23);
            this.cButton.TabIndex = 4;
            this.cButton.Text = "C";
            this.cButton.UseVisualStyleBackColor = true;
            // 
            // refresher
            // 
            this.refresher.Interval = 50;
            this.refresher.Tick += new System.EventHandler(this.refresher_Tick);
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
            this.Controls.Add(this.cButton);
            this.Controls.Add(this.bButton);
            this.Controls.Add(this.dButton);
            this.Controls.Add(this.aButton);
            this.Controls.Add(this.screenImage);
            this.MaximizeBox = false;
            this.Name = "ControlPanel";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.ControlPanel_Load);
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
            refresher.Start();
        }
    }
}
