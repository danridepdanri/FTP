using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace FTP
{
    public partial class Form1 : Form
    {
        private string ftpHost;
        private string ftpUsername;
        private string ftpPassword;

        public Form1()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            ftpHost = hostTextBox.Text;
            ftpUsername = usernameTextBox.Text;
            ftpPassword = passwordTextBox.Text;

            // ������������ ���������� �� FTP-�������
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    // ��������� ������ ����� �� ����� � FTP-�������
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);

                    // �������� TreeView ����� ������������ ������ �����
                    serverTreeView.Nodes.Clear();

                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        // �������� ����� �� ����� ����
                        string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string fileName = fields[fields.Length - 1]; // ������ ���� ������ ��'� ����� ��� �����

                        // ��������� �������� �� TreeView
                        if (fields[0].StartsWith("d")) // �������� ���� (�����)
                        {
                            TreeNode node = new TreeNode(fileName);
                            serverTreeView.Nodes.Add(node);
                        }
                        else if (fields[0].StartsWith("-")) // �������� ���� (����)
                        {
                            TreeNode node = new TreeNode(fileName);
                            serverTreeView.Nodes.Add(node);
                        }

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    responseStream.Close();
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void uploadButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ������������ ����� �� FTP-������
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(filePath);

                        // ��������� ��'���� ��� ������������ �����
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + fileInfo.Name);
                        request.Method = WebRequestMethods.Ftp.UploadFile;

                        request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                        // ������������ �����
                        using (Stream fileStream = fileInfo.OpenRead())
                        {
                            using (Stream ftpStream = request.GetRequestStream())
                            {
                                byte[] buffer = new byte[1024];
                                int bytesRead = 0;

                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ftpStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }

                    MessageBox.Show("����� ������ ���������� �� FTP-������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void appendButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ���� ����� ��� ��������� �� ��������� �� FTP-������
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    FileInfo fileInfo = new FileInfo(filePath);

                    // ��������� ��'���� ��� ��������� �� �����
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + fileInfo.Name);
                    request.Method = WebRequestMethods.Ftp.AppendFile;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    // ��������� �� �����
                    using (Stream fileStream = fileInfo.OpenRead())
                    {
                        using (Stream ftpStream = request.GetRequestStream())
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead = 0;

                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ftpStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }

                    MessageBox.Show("���� ������ ������� �� ��������� �� FTP-������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ��������� ����� � FTP-�������
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        MessageBox.Show("���� ������ ��������� � FTP-�������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ���� ���� ��� ���������� ������������� �����
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = selectedPath.Substring(selectedPath.LastIndexOf("/") + 1);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // ������������ ����� � FTP-�������
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            using (Stream ftpStream = response.GetResponseStream())
                            {
                                using (Stream fileStream = File.Create(savePath))
                                {
                                    byte[] buffer = new byte[1024];
                                    int bytesRead = 0;

                                    while ((bytesRead = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        fileStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }

                            MessageBox.Show("���� ������ ������������ � FTP-�������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void createDirectoryButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // �������� ������ ���� ��� ��������
                string newDirectoryName = Microsoft.VisualBasic.Interaction.InputBox("������ ���� ��'� ��������:", "�������� �������", "");

                if (!string.IsNullOrEmpty(newDirectoryName))
                {
                    // ��������� �������� �� FTP-������
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + newDirectoryName);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            MessageBox.Show("������� ������ ��������� �� FTP-������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void getFileSizeButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ��������� ������ ����� �� FTP-������
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        long fileSize = response.ContentLength;
                        MessageBox.Show("����� �����: " + fileSize.ToString() + " ����", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void getDirectoryListingButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ��������� ������ ����� �� ����� �� FTP-������
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // ��������� ������ �������
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        // �������� TreeView ����� ������������ ������ �����
                        serverTreeView.Nodes.Clear();

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            // ��������� �������� �� TreeView
                            TreeNode node = new TreeNode(line);
                            serverTreeView.Nodes.Add(node);

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        responseStream.Close();
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void getDetailedListingButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ��������� ���������� ������ ����� �� ����� �� FTP-������
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // ��������� ������ �������
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        // �������� TreeView ����� ������������ ������ �����
                        serverTreeView.Nodes.Clear();

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            // ��������� �������� �� TreeView
                            TreeNode node = new TreeNode(line);
                            serverTreeView.Nodes.Add(node);

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        responseStream.Close();
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void createDirectoryButton1_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // �������� ������ ���� ��� �����
                string newDirectoryName = Microsoft.VisualBasic.Interaction.InputBox("������ ���� ����� �����:", "�������� �����", "");

                if (!string.IsNullOrEmpty(newDirectoryName))
                {
                    // ��������� ����� �� FTP-������
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + newDirectoryName);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            MessageBox.Show("����� ������ �������� �� FTP-������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void deleteDirectoryButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ��������� ����� �� FTP-������
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        MessageBox.Show("����� ������ �������� � FTP-�������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // �������� ������ ���� ��� ����� ��� �����
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("������ ���� ��'� ��� ����� ��� �����:", "�������������", "");

                if (!string.IsNullOrEmpty(newFileName))
                {
                    // �������������� ����� ��� ����� �� FTP-������
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                    request.Method = WebRequestMethods.Ftp.Rename;

                    request.RenameTo = newFileName;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            MessageBox.Show("���� ��� ����� ������ ������������ �� FTP-������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void uploadFileButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ���� ����� ��� ������������
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "������� ���� ��� ������������";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = openFileDialog.FileName;

                    // ������������ ����� �� FTP-������
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + Path.GetFileName(localFilePath));
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (Stream fileStream = File.OpenRead(localFilePath))
                        using (Stream ftpStream = request.GetRequestStream())
                        {
                            fileStream.CopyTo(ftpStream);
                        }

                        MessageBox.Show("���� ������ ������������ �� FTP-������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void uploadUniqueFileButton_Click(object sender, EventArgs e)
        {
            // ��������, �� ������� ������� � TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // ���� ����� ��� ������������
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "������� ���� ��� ������������";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = openFileDialog.FileName;

                    // ��������� ���������� ���� ��� �����
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(localFilePath);

                    // ������������ ����� �� FTP-������ � ��������� ������
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + uniqueFileName);
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (Stream fileStream = File.OpenRead(localFilePath))
                        using (Stream ftpStream = request.GetRequestStream())
                        {
                            fileStream.CopyTo(ftpStream);
                        }

                        MessageBox.Show("���� ������ ������������ �� FTP-������ � ��������� ������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void serverTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ReturnToParentFolder();
            }
        }





        private void OpenSelectedFolder()
        {
            TreeNode selectedNode = serverTreeView.SelectedNode;

            // ��������, �� ������� �����
            if (selectedNode != null && selectedNode.Nodes.Count == 0)
            {
                string selectedPath = GetNodePath(selectedNode);

                // ������������ ���������� �� FTP-������� ��� ��������� ������ ����� �� ����� � �����
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // ��������� ������ ����� �� ����� � FTP-�������
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        selectedNode.Nodes.Clear(); // �������� ����� ����� ������������ ������ �����

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            // �������� ����� �� ����� ����
                            string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string fileName = fields[fields.Length - 1]; // ������ ���� ������ ��'� ����� ��� �����

                            // ��������� �������� �� TreeView
                            if (fields[0].StartsWith("d")) // �������� ���� (�����)
                            {
                                TreeNode node = new TreeNode(fileName);
                                selectedNode.Nodes.Add(node);
                            }
                            else if (fields[0].StartsWith("-")) // �������� ���� (����)
                            {
                                TreeNode node = new TreeNode(fileName);
                                selectedNode.Nodes.Add(node);
                            }

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        responseStream.Close();
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ReturnToParentFolder()
        {
            TreeNode selectedNode = serverTreeView.SelectedNode;

            if (selectedNode != null)
            {
                TreeNode parentNode = selectedNode.Parent;

                if (parentNode != null)
                {
                    serverTreeView.SelectedNode = parentNode;
                }
            }
        }

        private string GetNodePath(TreeNode node)
        {
            string path = node.Text;

            TreeNode parent = node.Parent;
            while (parent != null)
            {
                path = parent.Text + "/" + path;
                parent = parent.Parent;
            }

            return path;
        }

        private void serverTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenSelectedFolder();
        }

        private void parentFolderButton_Click(object sender, EventArgs e)
        {
            ReturnToParentFolder();
        }

    }
}
