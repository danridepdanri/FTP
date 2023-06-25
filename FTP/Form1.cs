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

            // Встановлення підключення до FTP-сервера
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    // Отримання списку файлів та папок з FTP-сервера
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);

                    // Очищення TreeView перед відображенням нового вмісту
                    serverTreeView.Nodes.Clear();

                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        // Розбиття рядка на окремі поля
                        string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string fileName = fields[fields.Length - 1]; // Останнє поле містить ім'я файлу або папки

                        // Додавання елементів до TreeView
                        if (fields[0].StartsWith("d")) // Перевірка типу (папка)
                        {
                            TreeNode node = new TreeNode(fileName);
                            serverTreeView.Nodes.Add(node);
                        }
                        else if (fields[0].StartsWith("-")) // Перевірка типу (файл)
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
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void uploadButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Завантаження файлів на FTP-сервер
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(filePath);

                        // Створення об'єкта для завантаження файлу
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + fileInfo.Name);
                        request.Method = WebRequestMethods.Ftp.UploadFile;

                        request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                        // Завантаження файлу
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

                    MessageBox.Show("Файли успішно завантажені на FTP-сервер.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void appendButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Вибір файлу для додавання до існуючого на FTP-сервері
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    FileInfo fileInfo = new FileInfo(filePath);

                    // Створення об'єкта для додавання до файлу
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + fileInfo.Name);
                    request.Method = WebRequestMethods.Ftp.AppendFile;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    // Додавання до файлу
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

                    MessageBox.Show("Файл успішно доданий до існуючого на FTP-сервері.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Видалення файлу з FTP-сервера
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        MessageBox.Show("Файл успішно видалений з FTP-сервера.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Вибір місця для збереження завантаженого файлу
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = selectedPath.Substring(selectedPath.LastIndexOf("/") + 1);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;

                    // Завантаження файлу з FTP-сервера
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

                            MessageBox.Show("Файл успішно завантажений з FTP-сервера.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void createDirectoryButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Введення нового імені для каталогу
                string newDirectoryName = Microsoft.VisualBasic.Interaction.InputBox("Введіть нове ім'я каталогу:", "Створити каталог", "");

                if (!string.IsNullOrEmpty(newDirectoryName))
                {
                    // Створення каталогу на FTP-сервері
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + newDirectoryName);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            MessageBox.Show("Каталог успішно створений на FTP-сервері.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void getFileSizeButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Отримання розміру файлу на FTP-сервері
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        long fileSize = response.ContentLength;
                        MessageBox.Show("Розмір файлу: " + fileSize.ToString() + " байт", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void getDirectoryListingButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Отримання списку файлів та папок на FTP-сервері
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // Отримання відповіді сервера
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        // Очищення TreeView перед відображенням нового вмісту
                        serverTreeView.Nodes.Clear();

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            // Додавання елементів до TreeView
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
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void getDetailedListingButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Отримання детального списку файлів та папок на FTP-сервері
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // Отримання відповіді сервера
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        // Очищення TreeView перед відображенням нового вмісту
                        serverTreeView.Nodes.Clear();

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            // Додавання елементів до TreeView
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
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void createDirectoryButton1_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Введення нового імені для папки
                string newDirectoryName = Microsoft.VisualBasic.Interaction.InputBox("Введіть нову назву папки:", "Створити папку", "");

                if (!string.IsNullOrEmpty(newDirectoryName))
                {
                    // Створення папки на FTP-сервері
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath + "/" + newDirectoryName);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;

                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            MessageBox.Show("Папка успішно створена на FTP-сервері.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void deleteDirectoryButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Видалення папки на FTP-сервері
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        MessageBox.Show("Папка успішно видалена з FTP-сервера.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Введення нового імені для файлу або папки
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Введіть нове ім'я для файлу або папки:", "Перейменувати", "");

                if (!string.IsNullOrEmpty(newFileName))
                {
                    // Перейменування файлу або папки на FTP-сервері
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                    request.Method = WebRequestMethods.Ftp.Rename;

                    request.RenameTo = newFileName;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    try
                    {
                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            MessageBox.Show("Файл або папка успішно перейменовані на FTP-сервері.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void uploadFileButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Вибір файлу для завантаження
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Виберіть файл для завантаження";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = openFileDialog.FileName;

                    // Завантаження файлу на FTP-сервер
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

                        MessageBox.Show("Файл успішно завантажений на FTP-сервер.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void uploadUniqueFileButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано елемент в TreeView
            if (serverTreeView.SelectedNode != null)
            {
                string selectedPath = serverTreeView.SelectedNode.FullPath;

                // Вибір файлу для завантаження
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Виберіть файл для завантаження";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = openFileDialog.FileName;

                    // Генерація унікального імені для файлу
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(localFilePath);

                    // Завантаження файлу на FTP-сервер з унікальним іменем
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

                        MessageBox.Show("Файл успішно завантажений на FTP-сервер з унікальним іменем.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (WebException ex)
                    {
                        MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Перевірка, чи вибрано папку
            if (selectedNode != null && selectedNode.Nodes.Count == 0)
            {
                string selectedPath = GetNodePath(selectedNode);

                // Встановлення підключення до FTP-сервера для отримання списку файлів та папок в папці
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpHost + "/" + selectedPath);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // Отримання списку файлів та папок з FTP-сервера
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);

                        selectedNode.Nodes.Clear(); // Очищення вузла перед відображенням нового вмісту

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            // Розбиття рядка на окремі поля
                            string[] fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string fileName = fields[fields.Length - 1]; // Останнє поле містить ім'я файлу або папки

                            // Додавання елементів до TreeView
                            if (fields[0].StartsWith("d")) // Перевірка типу (папка)
                            {
                                TreeNode node = new TreeNode(fileName);
                                selectedNode.Nodes.Add(node);
                            }
                            else if (fields[0].StartsWith("-")) // Перевірка типу (файл)
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
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
