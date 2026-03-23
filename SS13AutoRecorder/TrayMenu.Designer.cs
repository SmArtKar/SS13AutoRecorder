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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayMenu));
            List_Servers = new System.Windows.Forms.ListBox();
            Input_ServerName = new System.Windows.Forms.TextBox();
            _Text_Name = new System.Windows.Forms.Label();
            Input_ServerIP = new System.Windows.Forms.TextBox();
            _Text_IP = new System.Windows.Forms.Label();
            _Text_Port = new System.Windows.Forms.Label();
            Input_ServerPort = new System.Windows.Forms.NumericUpDown();
            _Box_ServerStatus = new System.Windows.Forms.GroupBox();
            Label_RoundDuration = new System.Windows.Forms.Label();
            Label_MapName = new System.Windows.Forms.Label();
            Label_GameState = new System.Windows.Forms.Label();
            _Text_RoundDuration = new System.Windows.Forms.Label();
            _Text_MapName = new System.Windows.Forms.Label();
            _Text_GameState = new System.Windows.Forms.Label();
            Label_RoundID = new System.Windows.Forms.Label();
            _Text_RoundID = new System.Windows.Forms.Label();
            Label_ServerStatus = new System.Windows.Forms.Label();
            _Text_ServerStatus = new System.Windows.Forms.Label();
            _Box_ServerData = new System.Windows.Forms.GroupBox();
            _Text_DSKeywords = new System.Windows.Forms.Label();
            Input_ServerKeyword = new System.Windows.Forms.TextBox();
            Button_AddServerKeyword = new System.Windows.Forms.Button();
            Button_DeleteServerKeyword = new System.Windows.Forms.Button();
            List_ServerKeywords = new System.Windows.Forms.ListBox();
            _Text_APIType = new System.Windows.Forms.Label();
            Input_ServerAPIType = new System.Windows.Forms.ComboBox();
            _Box_Settings = new System.Windows.Forms.GroupBox();
            Input_DiscardOnExit = new System.Windows.Forms.CheckBox();
            Input_OBSScene = new System.Windows.Forms.ComboBox();
            Label_OBSStatus = new System.Windows.Forms.Label();
            _Text_UserAgent = new System.Windows.Forms.Label();
            Input_UserAgent = new System.Windows.Forms.TextBox();
            _Text_StopDelay = new System.Windows.Forms.Label();
            Input_StopRecordingDelay = new System.Windows.Forms.NumericUpDown();
            Input_OBSDirectory = new System.Windows.Forms.TextBox();
            _Text_SaveFolder = new System.Windows.Forms.Label();
            _Text_OBSScene = new System.Windows.Forms.Label();
            Input_OBSPassword = new System.Windows.Forms.TextBox();
            _Text_OBSPassword = new System.Windows.Forms.Label();
            Input_OBSPort = new System.Windows.Forms.NumericUpDown();
            _Text_OBSPort = new System.Windows.Forms.Label();
            Button_DelServer = new System.Windows.Forms.Button();
            Button_AddServer = new System.Windows.Forms.Button();
            _Text_SeekerFound = new System.Windows.Forms.Label();
            Label_SeekerStatus = new System.Windows.Forms.Label();
            autoRecorderBindingSource = new System.Windows.Forms.BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)Input_ServerPort).BeginInit();
            _Box_ServerStatus.SuspendLayout();
            _Box_ServerData.SuspendLayout();
            _Box_Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Input_StopRecordingDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Input_OBSPort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)autoRecorderBindingSource).BeginInit();
            SuspendLayout();
            // 
            // List_Servers
            // 
            resources.ApplyResources(List_Servers, "List_Servers");
            List_Servers.FormattingEnabled = true;
            List_Servers.Name = "List_Servers";
            List_Servers.SelectedIndexChanged += ServerList_SelectedIndexChanged;
            // 
            // Input_ServerName
            // 
            resources.ApplyResources(Input_ServerName, "Input_ServerName");
            Input_ServerName.Name = "Input_ServerName";
            Input_ServerName.KeyDown += Input_ServerName_KeyDown;
            Input_ServerName.Leave += Input_ServerName_Submit;
            // 
            // _Text_Name
            // 
            resources.ApplyResources(_Text_Name, "_Text_Name");
            _Text_Name.Name = "_Text_Name";
            // 
            // Input_ServerIP
            // 
            resources.ApplyResources(Input_ServerIP, "Input_ServerIP");
            Input_ServerIP.Name = "Input_ServerIP";
            Input_ServerIP.KeyDown += Input_ServerIP_KeyDown;
            Input_ServerIP.Leave += Input_ServerIP_Submit;
            // 
            // _Text_IP
            // 
            resources.ApplyResources(_Text_IP, "_Text_IP");
            _Text_IP.Name = "_Text_IP";
            // 
            // _Text_Port
            // 
            resources.ApplyResources(_Text_Port, "_Text_Port");
            _Text_Port.Name = "_Text_Port";
            // 
            // Input_ServerPort
            // 
            resources.ApplyResources(Input_ServerPort, "Input_ServerPort");
            Input_ServerPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            Input_ServerPort.Name = "Input_ServerPort";
            Input_ServerPort.KeyDown += Input_ServerPort_KeyDown;
            Input_ServerPort.Leave += Input_ServerPort_Submit;
            // 
            // _Box_ServerStatus
            // 
            resources.ApplyResources(_Box_ServerStatus, "_Box_ServerStatus");
            _Box_ServerStatus.Controls.Add(Label_RoundDuration);
            _Box_ServerStatus.Controls.Add(Label_MapName);
            _Box_ServerStatus.Controls.Add(Label_GameState);
            _Box_ServerStatus.Controls.Add(_Text_RoundDuration);
            _Box_ServerStatus.Controls.Add(_Text_MapName);
            _Box_ServerStatus.Controls.Add(_Text_GameState);
            _Box_ServerStatus.Controls.Add(Label_RoundID);
            _Box_ServerStatus.Controls.Add(_Text_RoundID);
            _Box_ServerStatus.Controls.Add(Label_ServerStatus);
            _Box_ServerStatus.Controls.Add(_Text_ServerStatus);
            _Box_ServerStatus.Name = "_Box_ServerStatus";
            _Box_ServerStatus.TabStop = false;
            // 
            // Label_RoundDuration
            // 
            resources.ApplyResources(Label_RoundDuration, "Label_RoundDuration");
            Label_RoundDuration.Name = "Label_RoundDuration";
            // 
            // Label_MapName
            // 
            resources.ApplyResources(Label_MapName, "Label_MapName");
            Label_MapName.Name = "Label_MapName";
            // 
            // Label_GameState
            // 
            resources.ApplyResources(Label_GameState, "Label_GameState");
            Label_GameState.Name = "Label_GameState";
            // 
            // _Text_RoundDuration
            // 
            resources.ApplyResources(_Text_RoundDuration, "_Text_RoundDuration");
            _Text_RoundDuration.Name = "_Text_RoundDuration";
            // 
            // _Text_MapName
            // 
            resources.ApplyResources(_Text_MapName, "_Text_MapName");
            _Text_MapName.Name = "_Text_MapName";
            // 
            // _Text_GameState
            // 
            resources.ApplyResources(_Text_GameState, "_Text_GameState");
            _Text_GameState.Name = "_Text_GameState";
            // 
            // Label_RoundID
            // 
            resources.ApplyResources(Label_RoundID, "Label_RoundID");
            Label_RoundID.Name = "Label_RoundID";
            // 
            // _Text_RoundID
            // 
            resources.ApplyResources(_Text_RoundID, "_Text_RoundID");
            _Text_RoundID.Name = "_Text_RoundID";
            // 
            // Label_ServerStatus
            // 
            resources.ApplyResources(Label_ServerStatus, "Label_ServerStatus");
            Label_ServerStatus.Name = "Label_ServerStatus";
            // 
            // _Text_ServerStatus
            // 
            resources.ApplyResources(_Text_ServerStatus, "_Text_ServerStatus");
            _Text_ServerStatus.Name = "_Text_ServerStatus";
            // 
            // _Box_ServerData
            // 
            resources.ApplyResources(_Box_ServerData, "_Box_ServerData");
            _Box_ServerData.Controls.Add(_Text_DSKeywords);
            _Box_ServerData.Controls.Add(Input_ServerKeyword);
            _Box_ServerData.Controls.Add(Button_AddServerKeyword);
            _Box_ServerData.Controls.Add(Button_DeleteServerKeyword);
            _Box_ServerData.Controls.Add(List_ServerKeywords);
            _Box_ServerData.Controls.Add(_Text_APIType);
            _Box_ServerData.Controls.Add(Input_ServerAPIType);
            _Box_ServerData.Controls.Add(_Text_Name);
            _Box_ServerData.Controls.Add(Input_ServerName);
            _Box_ServerData.Controls.Add(Input_ServerPort);
            _Box_ServerData.Controls.Add(Input_ServerIP);
            _Box_ServerData.Controls.Add(_Text_Port);
            _Box_ServerData.Controls.Add(_Text_IP);
            _Box_ServerData.Name = "_Box_ServerData";
            _Box_ServerData.TabStop = false;
            // 
            // _Text_DSKeywords
            // 
            resources.ApplyResources(_Text_DSKeywords, "_Text_DSKeywords");
            _Text_DSKeywords.Name = "_Text_DSKeywords";
            // 
            // Input_ServerKeyword
            // 
            resources.ApplyResources(Input_ServerKeyword, "Input_ServerKeyword");
            Input_ServerKeyword.Name = "Input_ServerKeyword";
            // 
            // Button_AddServerKeyword
            // 
            resources.ApplyResources(Button_AddServerKeyword, "Button_AddServerKeyword");
            Button_AddServerKeyword.Name = "Button_AddServerKeyword";
            Button_AddServerKeyword.UseVisualStyleBackColor = true;
            Button_AddServerKeyword.Click += Button_AddServerKeyword_Click;
            // 
            // Button_DeleteServerKeyword
            // 
            resources.ApplyResources(Button_DeleteServerKeyword, "Button_DeleteServerKeyword");
            Button_DeleteServerKeyword.Image = Properties.Resources.del_18px;
            Button_DeleteServerKeyword.Name = "Button_DeleteServerKeyword";
            Button_DeleteServerKeyword.UseVisualStyleBackColor = true;
            Button_DeleteServerKeyword.Click += Button_DeleteServerKeyword_Click;
            // 
            // List_ServerKeywords
            // 
            resources.ApplyResources(List_ServerKeywords, "List_ServerKeywords");
            List_ServerKeywords.FormattingEnabled = true;
            List_ServerKeywords.Name = "List_ServerKeywords";
            // 
            // _Text_APIType
            // 
            resources.ApplyResources(_Text_APIType, "_Text_APIType");
            _Text_APIType.Name = "_Text_APIType";
            // 
            // Input_ServerAPIType
            // 
            Input_ServerAPIType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(Input_ServerAPIType, "Input_ServerAPIType");
            Input_ServerAPIType.Name = "Input_ServerAPIType";
            // 
            // _Box_Settings
            // 
            resources.ApplyResources(_Box_Settings, "_Box_Settings");
            _Box_Settings.Controls.Add(Input_DiscardOnExit);
            _Box_Settings.Controls.Add(Input_OBSScene);
            _Box_Settings.Controls.Add(Label_OBSStatus);
            _Box_Settings.Controls.Add(_Text_UserAgent);
            _Box_Settings.Controls.Add(Input_UserAgent);
            _Box_Settings.Controls.Add(_Text_StopDelay);
            _Box_Settings.Controls.Add(Input_StopRecordingDelay);
            _Box_Settings.Controls.Add(Input_OBSDirectory);
            _Box_Settings.Controls.Add(_Text_SaveFolder);
            _Box_Settings.Controls.Add(_Text_OBSScene);
            _Box_Settings.Controls.Add(Input_OBSPassword);
            _Box_Settings.Controls.Add(_Text_OBSPassword);
            _Box_Settings.Controls.Add(Input_OBSPort);
            _Box_Settings.Controls.Add(_Text_OBSPort);
            _Box_Settings.Name = "_Box_Settings";
            _Box_Settings.TabStop = false;
            // 
            // Input_DiscardOnExit
            // 
            resources.ApplyResources(Input_DiscardOnExit, "Input_DiscardOnExit");
            Input_DiscardOnExit.Name = "Input_DiscardOnExit";
            Input_DiscardOnExit.UseVisualStyleBackColor = true;
            Input_DiscardOnExit.CheckedChanged += Input_DiscardOnExit_CheckedChanged;
            // 
            // Input_OBSScene
            // 
            Input_OBSScene.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(Input_OBSScene, "Input_OBSScene");
            Input_OBSScene.Name = "Input_OBSScene";
            // 
            // Label_OBSStatus
            // 
            resources.ApplyResources(Label_OBSStatus, "Label_OBSStatus");
            Label_OBSStatus.Name = "Label_OBSStatus";
            // 
            // _Text_UserAgent
            // 
            resources.ApplyResources(_Text_UserAgent, "_Text_UserAgent");
            _Text_UserAgent.Name = "_Text_UserAgent";
            // 
            // Input_UserAgent
            // 
            resources.ApplyResources(Input_UserAgent, "Input_UserAgent");
            Input_UserAgent.Name = "Input_UserAgent";
            Input_UserAgent.TextChanged += Input_UserAgent_TextChanged;
            // 
            // _Text_StopDelay
            // 
            resources.ApplyResources(_Text_StopDelay, "_Text_StopDelay");
            _Text_StopDelay.Name = "_Text_StopDelay";
            // 
            // Input_StopRecordingDelay
            // 
            resources.ApplyResources(Input_StopRecordingDelay, "Input_StopRecordingDelay");
            Input_StopRecordingDelay.Maximum = new decimal(new int[] { 6000, 0, 0, 0 });
            Input_StopRecordingDelay.Name = "Input_StopRecordingDelay";
            Input_StopRecordingDelay.ValueChanged += Input_StopRecordingDelay_ValueChanged;
            // 
            // Input_OBSDirectory
            // 
            resources.ApplyResources(Input_OBSDirectory, "Input_OBSDirectory");
            Input_OBSDirectory.Name = "Input_OBSDirectory";
            Input_OBSDirectory.KeyDown += Input_OBSDirectory_KeyDown;
            Input_OBSDirectory.Leave += Input_OBSDirectory_Submit;
            Input_OBSDirectory.MouseDoubleClick += Input_OBSDirectory_MouseDoubleClick;
            // 
            // _Text_SaveFolder
            // 
            resources.ApplyResources(_Text_SaveFolder, "_Text_SaveFolder");
            _Text_SaveFolder.Name = "_Text_SaveFolder";
            // 
            // _Text_OBSScene
            // 
            resources.ApplyResources(_Text_OBSScene, "_Text_OBSScene");
            _Text_OBSScene.Name = "_Text_OBSScene";
            // 
            // Input_OBSPassword
            // 
            resources.ApplyResources(Input_OBSPassword, "Input_OBSPassword");
            Input_OBSPassword.Name = "Input_OBSPassword";
            Input_OBSPassword.KeyDown += Input_OBSPassword_KeyDown;
            Input_OBSPassword.Leave += Input_OBSPassword_Submit;
            // 
            // _Text_OBSPassword
            // 
            resources.ApplyResources(_Text_OBSPassword, "_Text_OBSPassword");
            _Text_OBSPassword.Name = "_Text_OBSPassword";
            // 
            // Input_OBSPort
            // 
            resources.ApplyResources(Input_OBSPort, "Input_OBSPort");
            Input_OBSPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            Input_OBSPort.Name = "Input_OBSPort";
            Input_OBSPort.ValueChanged += Input_OBSPort_Submit;
            Input_OBSPort.KeyDown += Input_OBSPort_KeyDown;
            Input_OBSPort.Leave += Input_OBSPort_Submit;
            // 
            // _Text_OBSPort
            // 
            resources.ApplyResources(_Text_OBSPort, "_Text_OBSPort");
            _Text_OBSPort.Name = "_Text_OBSPort";
            // 
            // Button_DelServer
            // 
            resources.ApplyResources(Button_DelServer, "Button_DelServer");
            Button_DelServer.Image = Properties.Resources.del_18px;
            Button_DelServer.Name = "Button_DelServer";
            Button_DelServer.UseVisualStyleBackColor = true;
            Button_DelServer.Click += Button_DelServer_Click;
            // 
            // Button_AddServer
            // 
            resources.ApplyResources(Button_AddServer, "Button_AddServer");
            Button_AddServer.Name = "Button_AddServer";
            Button_AddServer.UseVisualStyleBackColor = true;
            Button_AddServer.Click += Button_AddServer_Click;
            // 
            // _Text_SeekerFound
            // 
            resources.ApplyResources(_Text_SeekerFound, "_Text_SeekerFound");
            _Text_SeekerFound.Name = "_Text_SeekerFound";
            // 
            // Label_SeekerStatus
            // 
            resources.ApplyResources(Label_SeekerStatus, "Label_SeekerStatus");
            Label_SeekerStatus.Name = "Label_SeekerStatus";
            // 
            // autoRecorderBindingSource
            // 
            autoRecorderBindingSource.DataSource = typeof(AutoRecorder);
            // 
            // TrayMenu
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(Label_SeekerStatus);
            Controls.Add(_Text_SeekerFound);
            Controls.Add(_Box_Settings);
            Controls.Add(_Box_ServerData);
            Controls.Add(_Box_ServerStatus);
            Controls.Add(Button_DelServer);
            Controls.Add(List_Servers);
            Controls.Add(Button_AddServer);
            Name = "TrayMenu";
            Load += TrayMenu_Load;
            ((System.ComponentModel.ISupportInitialize)Input_ServerPort).EndInit();
            _Box_ServerStatus.ResumeLayout(false);
            _Box_ServerStatus.PerformLayout();
            _Box_ServerData.ResumeLayout(false);
            _Box_ServerData.PerformLayout();
            _Box_Settings.ResumeLayout(false);
            _Box_Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Input_StopRecordingDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)Input_OBSPort).EndInit();
            ((System.ComponentModel.ISupportInitialize)autoRecorderBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.ComboBox Input_OBSScene;
        private System.Windows.Forms.CheckBox Input_DiscardOnExit;
    }
}

