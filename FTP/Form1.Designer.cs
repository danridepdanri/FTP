namespace FTP
{
    partial class Form1
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
            hostTextBox = new TextBox();
            label1 = new Label();
            lav = new Label();
            label2 = new Label();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            connectButton = new Button();
            serverTreeView = new TreeView();
            uploadButton = new Button();
            appendButton = new Button();
            deleteButton = new Button();
            downloadButton = new Button();
            createDirectoryButton = new Button();
            getFileSizeButton = new Button();
            getDirectoryListingButton = new Button();
            getDetailedListingButton = new Button();
            createDirectoryButton1 = new Button();
            deleteDirectoryButton = new Button();
            renameButton = new Button();
            uploadUniqueFileButton = new Button();
            parentFolderButton = new Button();
            openFolderButton = new Button();
            SuspendLayout();
            // 
            // hostTextBox
            // 
            hostTextBox.Location = new Point(127, 12);
            hostTextBox.Name = "hostTextBox";
            hostTextBox.Size = new Size(180, 23);
            hostTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 16);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 1;
            label1.Text = "hostTextBox";
            // 
            // lav
            // 
            lav.AutoSize = true;
            lav.Location = new Point(12, 47);
            lav.Name = "lav";
            lav.Size = new Size(100, 15);
            lav.TabIndex = 2;
            lav.Text = "usernameTextBox";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 87);
            label2.Name = "label2";
            label2.Size = new Size(98, 15);
            label2.TabIndex = 4;
            label2.Text = "passwordTextBox";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(127, 47);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(180, 23);
            usernameTextBox.TabIndex = 3;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(127, 84);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(180, 23);
            passwordTextBox.TabIndex = 5;
            // 
            // connectButton
            // 
            connectButton.Location = new Point(14, 117);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(293, 23);
            connectButton.TabIndex = 6;
            connectButton.Text = "connectButton";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // serverTreeView
            // 
            serverTreeView.Location = new Point(313, 23);
            serverTreeView.Name = "serverTreeView";
            serverTreeView.Size = new Size(1003, 580);
            serverTreeView.TabIndex = 7;
            serverTreeView.MouseClick += serverTreeView_MouseClick;
            serverTreeView.MouseDoubleClick += serverTreeView_MouseDoubleClick;
            // 
            // uploadButton
            // 
            uploadButton.Location = new Point(12, 146);
            uploadButton.Name = "uploadButton";
            uploadButton.Size = new Size(293, 23);
            uploadButton.TabIndex = 8;
            uploadButton.Text = "uploadButton";
            uploadButton.UseVisualStyleBackColor = true;
            uploadButton.Click += uploadButton_Click;
            // 
            // appendButton
            // 
            appendButton.Location = new Point(12, 175);
            appendButton.Name = "appendButton";
            appendButton.Size = new Size(293, 23);
            appendButton.TabIndex = 9;
            appendButton.Text = "appendButton";
            appendButton.UseVisualStyleBackColor = true;
            appendButton.Click += appendButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(12, 204);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(293, 23);
            deleteButton.TabIndex = 10;
            deleteButton.Text = "deleteButton";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // downloadButton
            // 
            downloadButton.Location = new Point(12, 233);
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(293, 23);
            downloadButton.TabIndex = 11;
            downloadButton.Text = "downloadButton";
            downloadButton.UseVisualStyleBackColor = true;
            downloadButton.Click += downloadButton_Click;
            // 
            // createDirectoryButton
            // 
            createDirectoryButton.Location = new Point(12, 262);
            createDirectoryButton.Name = "createDirectoryButton";
            createDirectoryButton.Size = new Size(293, 23);
            createDirectoryButton.TabIndex = 12;
            createDirectoryButton.Text = "createDirectoryButton";
            createDirectoryButton.UseVisualStyleBackColor = true;
            createDirectoryButton.Click += createDirectoryButton_Click;
            // 
            // getFileSizeButton
            // 
            getFileSizeButton.Location = new Point(12, 291);
            getFileSizeButton.Name = "getFileSizeButton";
            getFileSizeButton.Size = new Size(293, 23);
            getFileSizeButton.TabIndex = 13;
            getFileSizeButton.Text = "getFileSizeButton";
            getFileSizeButton.UseVisualStyleBackColor = true;
            getFileSizeButton.Click += getFileSizeButton_Click;
            // 
            // getDirectoryListingButton
            // 
            getDirectoryListingButton.Location = new Point(12, 320);
            getDirectoryListingButton.Name = "getDirectoryListingButton";
            getDirectoryListingButton.Size = new Size(293, 23);
            getDirectoryListingButton.TabIndex = 14;
            getDirectoryListingButton.Text = "getDirectoryListingButton";
            getDirectoryListingButton.UseVisualStyleBackColor = true;
            getDirectoryListingButton.Click += getDirectoryListingButton_Click;
            // 
            // getDetailedListingButton
            // 
            getDetailedListingButton.Location = new Point(12, 349);
            getDetailedListingButton.Name = "getDetailedListingButton";
            getDetailedListingButton.Size = new Size(293, 23);
            getDetailedListingButton.TabIndex = 15;
            getDetailedListingButton.Text = "getDetailedListingButton";
            getDetailedListingButton.UseVisualStyleBackColor = true;
            getDetailedListingButton.Click += getDetailedListingButton_Click;
            // 
            // createDirectoryButton1
            // 
            createDirectoryButton1.Location = new Point(14, 378);
            createDirectoryButton1.Name = "createDirectoryButton1";
            createDirectoryButton1.Size = new Size(293, 23);
            createDirectoryButton1.TabIndex = 16;
            createDirectoryButton1.Text = "createDirectoryButton";
            createDirectoryButton1.UseVisualStyleBackColor = true;
            createDirectoryButton1.Click += createDirectoryButton1_Click;
            // 
            // deleteDirectoryButton
            // 
            deleteDirectoryButton.Location = new Point(14, 407);
            deleteDirectoryButton.Name = "deleteDirectoryButton";
            deleteDirectoryButton.Size = new Size(293, 23);
            deleteDirectoryButton.TabIndex = 17;
            deleteDirectoryButton.Text = "deleteDirectoryButton";
            deleteDirectoryButton.UseVisualStyleBackColor = true;
            deleteDirectoryButton.Click += deleteDirectoryButton_Click;
            // 
            // renameButton
            // 
            renameButton.Location = new Point(12, 436);
            renameButton.Name = "renameButton";
            renameButton.Size = new Size(293, 23);
            renameButton.TabIndex = 18;
            renameButton.Text = "renameButton";
            renameButton.UseVisualStyleBackColor = true;
            renameButton.Click += renameButton_Click;
            // 
            // uploadUniqueFileButton
            // 
            uploadUniqueFileButton.Location = new Point(12, 465);
            uploadUniqueFileButton.Name = "uploadUniqueFileButton";
            uploadUniqueFileButton.Size = new Size(293, 23);
            uploadUniqueFileButton.TabIndex = 19;
            uploadUniqueFileButton.Text = "uploadUniqueFileButton";
            uploadUniqueFileButton.UseVisualStyleBackColor = true;
            // 
            // parentFolderButton
            // 
            parentFolderButton.Location = new Point(12, 571);
            parentFolderButton.Name = "parentFolderButton";
            parentFolderButton.Size = new Size(293, 23);
            parentFolderButton.TabIndex = 20;
            parentFolderButton.Text = "parentFolderButton";
            parentFolderButton.UseVisualStyleBackColor = true;
            parentFolderButton.Click += parentFolderButton_Click;
            // 
            // openFolderButton
            // 
            openFolderButton.Location = new Point(12, 542);
            openFolderButton.Name = "openFolderButton";
            openFolderButton.Size = new Size(293, 23);
            openFolderButton.TabIndex = 21;
            openFolderButton.Text = "openFolderButton";
            openFolderButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1328, 615);
            Controls.Add(openFolderButton);
            Controls.Add(parentFolderButton);
            Controls.Add(uploadUniqueFileButton);
            Controls.Add(renameButton);
            Controls.Add(deleteDirectoryButton);
            Controls.Add(createDirectoryButton1);
            Controls.Add(getDetailedListingButton);
            Controls.Add(getDirectoryListingButton);
            Controls.Add(getFileSizeButton);
            Controls.Add(createDirectoryButton);
            Controls.Add(downloadButton);
            Controls.Add(deleteButton);
            Controls.Add(appendButton);
            Controls.Add(uploadButton);
            Controls.Add(serverTreeView);
            Controls.Add(connectButton);
            Controls.Add(passwordTextBox);
            Controls.Add(label2);
            Controls.Add(usernameTextBox);
            Controls.Add(lav);
            Controls.Add(label1);
            Controls.Add(hostTextBox);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox hostTextBox;
        private Label label1;
        private Label lav;
        private Label label2;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button connectButton;
        private TreeView serverTreeView;
        private Button uploadButton;
        private Button appendButton;
        private Button deleteButton;
        private Button downloadButton;
        private Button createDirectoryButton;
        private Button getFileSizeButton;
        private Button getDirectoryListingButton;
        private Button getDetailedListingButton;
        private Button createDirectoryButton1;
        private Button deleteDirectoryButton;
        private Button renameButton;
        private Button uploadUniqueFileButton;
        private Button parentFolderButton;
        private Button openFolderButton;
    }
}