namespace SS13AutoRecorder
{
    partial class TrayMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayMenu));
            this.List_Servers = new System.Windows.Forms.ListBox();
            this.Input_ServerName = new System.Windows.Forms.TextBox();
            this._Text_Name = new System.Windows.Forms.Label();
            this.Input_ServerIP = new System.Windows.Forms.TextBox();
            this._Text_IP = new System.Windows.Forms.Label();
            this._Text_Port = new System.Windows.Forms.Label();
            this.Input_ServerPort = new System.Windows.Forms.NumericUpDown();
            this._Box_ServerStatus = new System.Windows.Forms.GroupBox();
            this.Label_RoundDuration = new System.Windows.Forms.Label();
            this.Label_MapName = new System.Windows.Forms.Label();
            this.Label_GameState = new System.Windows.Forms.Label();
            this._Text_RoundDuration = new System.Windows.Forms.Label();
            this._Text_MapName = new System.Windows.Forms.Label();
            this._Text_GameState = new System.Windows.Forms.Label();
            this.Label_RoundID = new System.Windows.Forms.Label();
            this._Text_RoundID = new System.Windows.Forms.Label();
            this.Label_ServerStatus = new System.Windows.Forms.Label();
            this._Text_ServerStatus = new System.Windows.Forms.Label();
            this._Box_ServerData = new System.Windows.Forms.GroupBox();
            this._Text_DSKeywords = new System.Windows.Forms.Label();
            this.Input_ServerKeyword = new System.Windows.Forms.TextBox();
            this.Button_AddServerKeyword = new System.Windows.Forms.Button();
            this.Button_DeleteServerKeyword = new System.Windows.Forms.Button();
            this.List_ServerKeywords = new System.Windows.Forms.ListBox();
            this._Text_APIType = new System.Windows.Forms.Label();
            this.Input_ServerAPIType = new System.Windows.Forms.ComboBox();
            this._Box_Settings = new System.Windows.Forms.GroupBox();
            this.Label_OBSStatus = new System.Windows.Forms.Label();
            this._Text_UserAgent = new System.Windows.Forms.Label();
            this.Input_UserAgent = new System.Windows.Forms.TextBox();
            this._Text_StopDelay = new System.Windows.Forms.Label();
            this.Input_StopRecordingDelay = new System.Windows.Forms.NumericUpDown();
            this.Input_OBSDirectory = new System.Windows.Forms.TextBox();
            this._Text_SaveFolder = new System.Windows.Forms.Label();
            this._Text_OBSScene = new System.Windows.Forms.Label();
            this.Input_OBSScene = new System.Windows.Forms.TextBox();
            this.Input_OBSPassword = new System.Windows.Forms.TextBox();
            this._Text_OBSPassword = new System.Windows.Forms.Label();
            this.Input_OBSPort = new System.Windows.Forms.NumericUpDown();
            this._Text_OBSPort = new System.Windows.Forms.Label();
            this.Button_DelServer = new System.Windows.Forms.Button();
            this.Button_AddServer = new System.Windows.Forms.Button();
            this._Text_SeekerFound = new System.Windows.Forms.Label();
            this.Label_SeekerStatus = new System.Windows.Forms.Label();
            this.autoRecorderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Input_ServerPort)).BeginInit();
            this._Box_ServerStatus.SuspendLayout();
            this._Box_ServerData.SuspendLayout();
            this._Box_Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Input_StopRecordingDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_OBSPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoRecorderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // List_Servers
            // 
            resources.ApplyResources(this.List_Servers, "List_Servers");
            this.List_Servers.FormattingEnabled = true;
            this.List_Servers.Name = "List_Servers";
            this.List_Servers.SelectedIndexChanged += new System.EventHandler(this.ServerList_SelectedIndexChanged);
            // 
            // Input_ServerName
            // 
            resources.ApplyResources(this.Input_ServerName, "Input_ServerName");
            this.Input_ServerName.Name = "Input_ServerName";
            this.Input_ServerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_ServerName_KeyDown);
            this.Input_ServerName.Leave += new System.EventHandler(this.Input_ServerName_Submit);
            // 
            // _Text_Name
            // 
            resources.ApplyResources(this._Text_Name, "_Text_Name");
            this._Text_Name.Name = "_Text_Name";
            // 
            // Input_ServerIP
            // 
            resources.ApplyResources(this.Input_ServerIP, "Input_ServerIP");
            this.Input_ServerIP.Name = "Input_ServerIP";
            this.Input_ServerIP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_ServerIP_KeyDown);
            this.Input_ServerIP.Leave += new System.EventHandler(this.Input_ServerIP_Submit);
            // 
            // _Text_IP
            // 
            resources.ApplyResources(this._Text_IP, "_Text_IP");
            this._Text_IP.Name = "_Text_IP";
            // 
            // _Text_Port
            // 
            resources.ApplyResources(this._Text_Port, "_Text_Port");
            this._Text_Port.Name = "_Text_Port";
            // 
            // Input_ServerPort
            // 
            resources.ApplyResources(this.Input_ServerPort, "Input_ServerPort");
            this.Input_ServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.Input_ServerPort.Name = "Input_ServerPort";
            this.Input_ServerPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_ServerPort_KeyDown);
            this.Input_ServerPort.Leave += new System.EventHandler(this.Input_ServerPort_Submit);
            // 
            // _Box_ServerStatus
            // 
            resources.ApplyResources(this._Box_ServerStatus, "_Box_ServerStatus");
            this._Box_ServerStatus.Controls.Add(this.Label_RoundDuration);
            this._Box_ServerStatus.Controls.Add(this.Label_MapName);
            this._Box_ServerStatus.Controls.Add(this.Label_GameState);
            this._Box_ServerStatus.Controls.Add(this._Text_RoundDuration);
            this._Box_ServerStatus.Controls.Add(this._Text_MapName);
            this._Box_ServerStatus.Controls.Add(this._Text_GameState);
            this._Box_ServerStatus.Controls.Add(this.Label_RoundID);
            this._Box_ServerStatus.Controls.Add(this._Text_RoundID);
            this._Box_ServerStatus.Controls.Add(this.Label_ServerStatus);
            this._Box_ServerStatus.Controls.Add(this._Text_ServerStatus);
            this._Box_ServerStatus.Name = "_Box_ServerStatus";
            this._Box_ServerStatus.TabStop = false;
            // 
            // Label_RoundDuration
            // 
            resources.ApplyResources(this.Label_RoundDuration, "Label_RoundDuration");
            this.Label_RoundDuration.Name = "Label_RoundDuration";
            // 
            // Label_MapName
            // 
            resources.ApplyResources(this.Label_MapName, "Label_MapName");
            this.Label_MapName.Name = "Label_MapName";
            // 
            // Label_GameState
            // 
            resources.ApplyResources(this.Label_GameState, "Label_GameState");
            this.Label_GameState.Name = "Label_GameState";
            // 
            // _Text_RoundDuration
            // 
            resources.ApplyResources(this._Text_RoundDuration, "_Text_RoundDuration");
            this._Text_RoundDuration.Name = "_Text_RoundDuration";
            // 
            // _Text_MapName
            // 
            resources.ApplyResources(this._Text_MapName, "_Text_MapName");
            this._Text_MapName.Name = "_Text_MapName";
            // 
            // _Text_GameState
            // 
            resources.ApplyResources(this._Text_GameState, "_Text_GameState");
            this._Text_GameState.Name = "_Text_GameState";
            // 
            // Label_RoundID
            // 
            resources.ApplyResources(this.Label_RoundID, "Label_RoundID");
            this.Label_RoundID.Name = "Label_RoundID";
            // 
            // _Text_RoundID
            // 
            resources.ApplyResources(this._Text_RoundID, "_Text_RoundID");
            this._Text_RoundID.Name = "_Text_RoundID";
            // 
            // Label_ServerStatus
            // 
            resources.ApplyResources(this.Label_ServerStatus, "Label_ServerStatus");
            this.Label_ServerStatus.Name = "Label_ServerStatus";
            // 
            // _Text_ServerStatus
            // 
            resources.ApplyResources(this._Text_ServerStatus, "_Text_ServerStatus");
            this._Text_ServerStatus.Name = "_Text_ServerStatus";
            // 
            // _Box_ServerData
            // 
            resources.ApplyResources(this._Box_ServerData, "_Box_ServerData");
            this._Box_ServerData.Controls.Add(this._Text_DSKeywords);
            this._Box_ServerData.Controls.Add(this.Input_ServerKeyword);
            this._Box_ServerData.Controls.Add(this.Button_AddServerKeyword);
            this._Box_ServerData.Controls.Add(this.Button_DeleteServerKeyword);
            this._Box_ServerData.Controls.Add(this.List_ServerKeywords);
            this._Box_ServerData.Controls.Add(this._Text_APIType);
            this._Box_ServerData.Controls.Add(this.Input_ServerAPIType);
            this._Box_ServerData.Controls.Add(this._Text_Name);
            this._Box_ServerData.Controls.Add(this.Input_ServerName);
            this._Box_ServerData.Controls.Add(this.Input_ServerPort);
            this._Box_ServerData.Controls.Add(this.Input_ServerIP);
            this._Box_ServerData.Controls.Add(this._Text_Port);
            this._Box_ServerData.Controls.Add(this._Text_IP);
            this._Box_ServerData.Name = "_Box_ServerData";
            this._Box_ServerData.TabStop = false;
            // 
            // _Text_DSKeywords
            // 
            resources.ApplyResources(this._Text_DSKeywords, "_Text_DSKeywords");
            this._Text_DSKeywords.Name = "_Text_DSKeywords";
            // 
            // Input_ServerKeyword
            // 
            resources.ApplyResources(this.Input_ServerKeyword, "Input_ServerKeyword");
            this.Input_ServerKeyword.Name = "Input_ServerKeyword";
            // 
            // Button_AddServerKeyword
            // 
            resources.ApplyResources(this.Button_AddServerKeyword, "Button_AddServerKeyword");
            this.Button_AddServerKeyword.Name = "Button_AddServerKeyword";
            this.Button_AddServerKeyword.UseVisualStyleBackColor = true;
            this.Button_AddServerKeyword.Click += new System.EventHandler(this.Button_AddServerKeyword_Click);
            // 
            // Button_DeleteServerKeyword
            // 
            resources.ApplyResources(this.Button_DeleteServerKeyword, "Button_DeleteServerKeyword");
            this.Button_DeleteServerKeyword.Image = global::SS13AutoRecorder.Properties.Resources.del_18px;
            this.Button_DeleteServerKeyword.Name = "Button_DeleteServerKeyword";
            this.Button_DeleteServerKeyword.UseVisualStyleBackColor = true;
            this.Button_DeleteServerKeyword.Click += new System.EventHandler(this.Button_DeleteServerKeyword_Click);
            // 
            // List_ServerKeywords
            // 
            resources.ApplyResources(this.List_ServerKeywords, "List_ServerKeywords");
            this.List_ServerKeywords.FormattingEnabled = true;
            this.List_ServerKeywords.Name = "List_ServerKeywords";
            // 
            // _Text_APIType
            // 
            resources.ApplyResources(this._Text_APIType, "_Text_APIType");
            this._Text_APIType.Name = "_Text_APIType";
            // 
            // Input_ServerAPIType
            // 
            this.Input_ServerAPIType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.Input_ServerAPIType, "Input_ServerAPIType");
            this.Input_ServerAPIType.Name = "Input_ServerAPIType";
            // 
            // _Box_Settings
            // 
            resources.ApplyResources(this._Box_Settings, "_Box_Settings");
            this._Box_Settings.Controls.Add(this.Label_OBSStatus);
            this._Box_Settings.Controls.Add(this._Text_UserAgent);
            this._Box_Settings.Controls.Add(this.Input_UserAgent);
            this._Box_Settings.Controls.Add(this._Text_StopDelay);
            this._Box_Settings.Controls.Add(this.Input_StopRecordingDelay);
            this._Box_Settings.Controls.Add(this.Input_OBSDirectory);
            this._Box_Settings.Controls.Add(this._Text_SaveFolder);
            this._Box_Settings.Controls.Add(this._Text_OBSScene);
            this._Box_Settings.Controls.Add(this.Input_OBSScene);
            this._Box_Settings.Controls.Add(this.Input_OBSPassword);
            this._Box_Settings.Controls.Add(this._Text_OBSPassword);
            this._Box_Settings.Controls.Add(this.Input_OBSPort);
            this._Box_Settings.Controls.Add(this._Text_OBSPort);
            this._Box_Settings.Name = "_Box_Settings";
            this._Box_Settings.TabStop = false;
            // 
            // Label_OBSStatus
            // 
            resources.ApplyResources(this.Label_OBSStatus, "Label_OBSStatus");
            this.Label_OBSStatus.Name = "Label_OBSStatus";
            // 
            // _Text_UserAgent
            // 
            resources.ApplyResources(this._Text_UserAgent, "_Text_UserAgent");
            this._Text_UserAgent.Name = "_Text_UserAgent";
            // 
            // Input_UserAgent
            // 
            resources.ApplyResources(this.Input_UserAgent, "Input_UserAgent");
            this.Input_UserAgent.Name = "Input_UserAgent";
            this.Input_UserAgent.TextChanged += new System.EventHandler(this.Input_UserAgent_TextChanged);
            // 
            // _Text_StopDelay
            // 
            resources.ApplyResources(this._Text_StopDelay, "_Text_StopDelay");
            this._Text_StopDelay.Name = "_Text_StopDelay";
            // 
            // Input_StopRecordingDelay
            // 
            resources.ApplyResources(this.Input_StopRecordingDelay, "Input_StopRecordingDelay");
            this.Input_StopRecordingDelay.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.Input_StopRecordingDelay.Name = "Input_StopRecordingDelay";
            this.Input_StopRecordingDelay.ValueChanged += new System.EventHandler(this.Input_StopRecordingDelay_ValueChanged);
            // 
            // Input_OBSDirectory
            // 
            resources.ApplyResources(this.Input_OBSDirectory, "Input_OBSDirectory");
            this.Input_OBSDirectory.Name = "Input_OBSDirectory";
            this.Input_OBSDirectory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_OBSDirectory_KeyDown);
            this.Input_OBSDirectory.Leave += new System.EventHandler(this.Input_OBSDirectory_Submit);
            this.Input_OBSDirectory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Input_OBSDirectory_MouseDoubleClick);
            // 
            // _Text_SaveFolder
            // 
            resources.ApplyResources(this._Text_SaveFolder, "_Text_SaveFolder");
            this._Text_SaveFolder.Name = "_Text_SaveFolder";
            // 
            // _Text_OBSScene
            // 
            resources.ApplyResources(this._Text_OBSScene, "_Text_OBSScene");
            this._Text_OBSScene.Name = "_Text_OBSScene";
            // 
            // Input_OBSScene
            // 
            resources.ApplyResources(this.Input_OBSScene, "Input_OBSScene");
            this.Input_OBSScene.Name = "Input_OBSScene";
            this.Input_OBSScene.TextChanged += new System.EventHandler(this.Input_OBSScene_TextChanged);
            // 
            // Input_OBSPassword
            // 
            resources.ApplyResources(this.Input_OBSPassword, "Input_OBSPassword");
            this.Input_OBSPassword.Name = "Input_OBSPassword";
            this.Input_OBSPassword.TextChanged += new System.EventHandler(this.Input_OBSPassword_TextChanged);
            // 
            // _Text_OBSPassword
            // 
            resources.ApplyResources(this._Text_OBSPassword, "_Text_OBSPassword");
            this._Text_OBSPassword.Name = "_Text_OBSPassword";
            // 
            // Input_OBSPort
            // 
            resources.ApplyResources(this.Input_OBSPort, "Input_OBSPort");
            this.Input_OBSPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.Input_OBSPort.Name = "Input_OBSPort";
            this.Input_OBSPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Input_OBSPort.ValueChanged += new System.EventHandler(this.Input_OBSPort_ValueChanged);
            // 
            // _Text_OBSPort
            // 
            resources.ApplyResources(this._Text_OBSPort, "_Text_OBSPort");
            this._Text_OBSPort.Name = "_Text_OBSPort";
            // 
            // Button_DelServer
            // 
            resources.ApplyResources(this.Button_DelServer, "Button_DelServer");
            this.Button_DelServer.Image = global::SS13AutoRecorder.Properties.Resources.del_18px;
            this.Button_DelServer.Name = "Button_DelServer";
            this.Button_DelServer.UseVisualStyleBackColor = true;
            this.Button_DelServer.Click += new System.EventHandler(this.Button_DelServer_Click);
            // 
            // Button_AddServer
            // 
            resources.ApplyResources(this.Button_AddServer, "Button_AddServer");
            this.Button_AddServer.Name = "Button_AddServer";
            this.Button_AddServer.UseVisualStyleBackColor = true;
            this.Button_AddServer.Click += new System.EventHandler(this.Button_AddServer_Click);
            // 
            // _Text_SeekerFound
            // 
            resources.ApplyResources(this._Text_SeekerFound, "_Text_SeekerFound");
            this._Text_SeekerFound.Name = "_Text_SeekerFound";
            // 
            // Label_SeekerStatus
            // 
            resources.ApplyResources(this.Label_SeekerStatus, "Label_SeekerStatus");
            this.Label_SeekerStatus.Name = "Label_SeekerStatus";
            // 
            // autoRecorderBindingSource
            // 
            this.autoRecorderBindingSource.DataSource = typeof(SS13AutoRecorder.AutoRecorder);
            // 
            // TrayMenu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label_SeekerStatus);
            this.Controls.Add(this._Text_SeekerFound);
            this.Controls.Add(this._Box_Settings);
            this.Controls.Add(this._Box_ServerData);
            this.Controls.Add(this._Box_ServerStatus);
            this.Controls.Add(this.Button_DelServer);
            this.Controls.Add(this.Button_AddServer);
            this.Controls.Add(this.List_Servers);
            this.Name = "TrayMenu";
            this.Load += new System.EventHandler(this.TrayMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Input_ServerPort)).EndInit();
            this._Box_ServerStatus.ResumeLayout(false);
            this._Box_ServerStatus.PerformLayout();
            this._Box_ServerData.ResumeLayout(false);
            this._Box_ServerData.PerformLayout();
            this._Box_Settings.ResumeLayout(false);
            this._Box_Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Input_StopRecordingDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Input_OBSPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoRecorderBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox List_Servers;
        private System.Windows.Forms.Button Button_AddServer;
        private System.Windows.Forms.Button Button_DelServer;
        private System.Windows.Forms.TextBox Input_ServerName;
        private System.Windows.Forms.Label _Text_Name;
        private System.Windows.Forms.TextBox Input_ServerIP;
        private System.Windows.Forms.Label _Text_IP;
        private System.Windows.Forms.Label _Text_Port;
        private System.Windows.Forms.NumericUpDown Input_ServerPort;
        private System.Windows.Forms.GroupBox _Box_ServerStatus;
        private System.Windows.Forms.GroupBox _Box_ServerData;
        private System.Windows.Forms.Label _Text_APIType;
        private System.Windows.Forms.ComboBox Input_ServerAPIType;
        private System.Windows.Forms.TextBox Input_ServerKeyword;
        private System.Windows.Forms.Button Button_AddServerKeyword;
        private System.Windows.Forms.Button Button_DeleteServerKeyword;
        private System.Windows.Forms.ListBox List_ServerKeywords;
        private System.Windows.Forms.Label _Text_DSKeywords;
        private System.Windows.Forms.GroupBox _Box_Settings;
        private System.Windows.Forms.NumericUpDown Input_OBSPort;
        private System.Windows.Forms.Label _Text_OBSPort;
        private System.Windows.Forms.TextBox Input_OBSPassword;
        private System.Windows.Forms.Label _Text_OBSPassword;
        private System.Windows.Forms.Label _Text_OBSScene;
        private System.Windows.Forms.TextBox Input_OBSScene;
        private System.Windows.Forms.TextBox Input_OBSDirectory;
        private System.Windows.Forms.Label _Text_SaveFolder;
        private System.Windows.Forms.Label _Text_StopDelay;
        private System.Windows.Forms.NumericUpDown Input_StopRecordingDelay;
        private System.Windows.Forms.Label _Text_UserAgent;
        private System.Windows.Forms.TextBox Input_UserAgent;
        private System.Windows.Forms.Label Label_ServerStatus;
        private System.Windows.Forms.Label _Text_ServerStatus;
        private System.Windows.Forms.Label Label_RoundID;
        private System.Windows.Forms.Label _Text_RoundID;
        private System.Windows.Forms.Label _Text_GameState;
        private System.Windows.Forms.Label _Text_RoundDuration;
        private System.Windows.Forms.Label _Text_MapName;
        private System.Windows.Forms.Label Label_MapName;
        private System.Windows.Forms.Label Label_GameState;
        private System.Windows.Forms.Label Label_RoundDuration;
        private System.Windows.Forms.BindingSource autoRecorderBindingSource;
        private System.Windows.Forms.Label _Text_SeekerFound;
        private System.Windows.Forms.Label Label_SeekerStatus;
        private System.Windows.Forms.Label Label_OBSStatus;
    }
}

