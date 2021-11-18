using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using LiteNetLib;
using LibSample;

namespace WinFormsApp1
{
    public class User
    {
        //[BsonId]
        //public ObjectId Id { get; set; }
        public string name { get; set; }
        public int number { get; set; }

        public User()
        {
        }

        public User(string name, int number)
        {
            //this.Id = id;
            this.name = name;
            this.number = number;
        }
    }

    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 216);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "MongoDB";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(183, 314);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.connectToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 76);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.maxToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
            this.showToolStripMenuItem.Text = "显示";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.ShowToolStripMenuItem_Click);
            // 
            // maxToolStripMenuItem
            // 
            this.maxToolStripMenuItem.Name = "maxToolStripMenuItem";
            this.maxToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.maxToolStripMenuItem.Text = "Max";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
            this.exitToolStripMenuItem.Text = "退出";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
            this.connectToolStripMenuItem.Text = "连接DB";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.ConnectToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(170, 420);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 27);
            this.button2.TabIndex = 2;
            this.button2.Text = "Start Server";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(183, 518);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 29);
            this.button3.TabIndex = 3;
            this.button3.Text = "NAT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 782);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Winform+NetCore3.1+MongoDB";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            //string connectionString = "mongodb://localhost";
            string connectionString = "mongodb://localhost:27017/?readPreference=primary&directConnection=true&ssl=false";
            //string connectionString = "mongodb+srv://<username>:<password>@<cluster-address>/test?w=majority";
            Debug.Print($"connect to: {connectionString}");

            var client = new MongoClient(connectionString);

            //var search1 = client.ListDatabaseNames().ToList();
            //Debug.Print($"有{search1.Count}个database");
            //for (int i = 0; i < search1.Count; i++)
            //{
            //    Debug.Print($"[{i}]---{search1[i]}");
            //}
            //有4个database
            //[0]---admin
            //[1]---config
            //[2]---local
            //[3]---stickerDB

            //  根据数据库名称实例化数据库
            var database = client.GetDatabase("stickerDB");

            // 根据集合名称获取集合
            var collection = database.GetCollection<BsonDocument>("Users");
            var filter = new BsonDocument();

            // 查询集合中的文档
            var search2 = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
            // 循环遍历输出
            search2.ForEach(p =>
            {
                Debug.Print($"姓名：{p["name"]}，球衣号码：{p["number"]}");
            });

            //$lt    <   (less  than)
            //$lte   <=  (less than  or equal to)
            //$gt    >   (greater  than)
            //$gte   >=  (greater  than or equal to)
            //var filter3 = Builders<BsonDocument>.Filter.Eq("number", 10); //等于
            var filter3 = Builders<BsonDocument>.Filter.Gte("number", 10); //大于等于
            var result = collection.Find(filter3);
            Debug.Print($"result={result.CountDocuments()}");
        }

        private void Button2_Click(object sender, System.EventArgs e)
        {
            Debug.Print($"启动服务器");

            TcpChatServer.TCPChatServer.Run();
        }
       
        private void Button3_Click(object sender, EventArgs e)
        {
            var lib = new HolePunchServerTest();
            lib.Run();
        }

        #endregion

        private Label label1;
        private Button button1;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem maxToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private Button button2;
        private Button button3;
    }
}