using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using NWNOver.TLK;
using NWNOver.TwoDA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWNOver
{
    public partial class NWNOver : Form
    {
        private const string CONFIG_PATH = "config.json";
        DirectoryInfo CurrentSource;

        DirectoryInfo CurrentDirectory;
        FileSystemWatcher CurrentFileSystemWatcher;
        public ContentEnvironment Environment;

        Dictionary<string, TwoDAControl> TwoDAControls = new Dictionary<string, TwoDAControl>();
        Dictionary<string, TLKControl> TLKControls = new Dictionary<string, TLKControl>();

        JsonSerializer Serializer = JsonSerializer.Create();

        public NWNOver()
        {
            InitializeComponent();

            Environment = new ContentEnvironment();
            Environment.OnOpenTLKLine += Environment_OnOpenTLKLine;
            Environment.OnOpenTwoDALine += Environment_OnOpenTwoDALine;

            if (File.Exists(CONFIG_PATH))
            {
                Environment.Config = LoadConfig(CONFIG_PATH);
            }
            else
            {
                Environment.Config = new Configuration();
                SaveConfig(CONFIG_PATH, Environment.Config);
            }
            Environment.Config.OnConfigChanged += (sender) =>
            {
                SaveConfig(CONFIG_PATH, Environment.Config);
            };

            UpdateStrRefMenu();
            UpdateEnvironment();
        }

        private void SaveConfig(string path, Configuration config)
        {
            using (StreamWriter file = File.CreateText(CONFIG_PATH))
            {
                Serializer.Serialize(file, config);
            }
        }

        private Configuration LoadConfig(string path)
        {
            using (StreamReader file = File.OpenText(CONFIG_PATH))
            {
                return (Configuration)Serializer.Deserialize(file, typeof(Configuration));
            }
        }

        private void Environment_OnOpenTwoDALine(string filename, int line)
        {
            filename = filename.ToLower();
            if (TwoDAControls.ContainsKey(filename.ToLower()))
                TwoDAControls[filename.ToLower()].Open2DALine(line);
            else
            {
                string path = $"{filename}.2da";
                
                if (Environment.HasFile(path))
                {
                    if (MessageBox.Show("2DA file isn't currently open but was found in environment folder. Open 2DA file from environment folder?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        TwoDAFile file = new TwoDAFile(Path.GetFileNameWithoutExtension(path));
                        file.Read(File.OpenRead(Environment.GetFullPath(path)));
                        file.Schema = Environment.SchemaDatabase.GetSchema(Path.GetFileNameWithoutExtension(path));
                        var control = OpenTwoDAFile(path, file, false);
                        control.Open2DALine(line);
                    }
                }
            }
        }

        private void Environment_OnOpenTLKLine(string filename, uint line, bool edit)
        {
            if (TLKControls.ContainsKey(filename.ToLower()))
                TLKControls[filename.ToLower()].OpenTLKLine(line, edit);
        }

        private TwoDAControl GetCurrentTwoDAControl()
        {
            var selectedTab = tabControl1.SelectedTab;
            return GetTwoDAControl(selectedTab);
        }

        private static TwoDAControl GetTwoDAControl(TabPage selectedTab)
        {
            if (selectedTab != null && selectedTab.Controls.ContainsKey("TwoDAControl"))
                return (TwoDAControl)selectedTab.Controls["TwoDAControl"];
            return null;
        }

        private TLKControl GetCurrentTLKControl()
        {
            var selectedTab = tabControl1.SelectedTab;
            return GetTLKControl(selectedTab);
        }

        private static TLKControl GetTLKControl(TabPage selectedTab)
        {
            if (selectedTab != null && selectedTab.Controls.ContainsKey("TLKControl"))
                return (TLKControl)selectedTab.Controls["TLKControl"];
            return null;
        }

        private void btn_new_env_Click(object sender, EventArgs e)
        {
            CreateTLK();
        }

        private void menu_new_tlk_Click(object sender, EventArgs e)
        {
            CreateTLK();
        }

        private void btn_load_env_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void menu_open_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void btn_save_env_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void menu_save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btn_save_as_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void menu_save_as_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void menu_save_all_Click(object sender, EventArgs e)
        {
            foreach(TabPage page in tabControl1.TabPages)
            {
                var controlTwoDA = GetTwoDAControl(page);
                var controlTLK = GetTLKControl(page);
                if (controlTwoDA != null)
                {
                    SaveTwoDA(controlTwoDA, null);
                }
                if (controlTLK != null)
                {
                    SaveTLK(controlTLK, null);
                }
            }
        }

        private void menu_quit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menu_close_Click(object sender, EventArgs e)
        {
            CloseTab(tabControl1.SelectedTab);
        }

        private void CreateTLK()
        {
            TLKFile file = new TLKFile(Path.GetFileNameWithoutExtension("new"));
            OpenTLKFile("new", file, true);
        }
        
        private void Save()
        {
            var controlTwoDA = GetCurrentTwoDAControl();
            var controlTLK = GetCurrentTLKControl();
            if (controlTwoDA != null)
            {
                SaveTwoDA(controlTwoDA, null);
            }
            if (controlTLK != null)
            {
                SaveTLK(controlTLK, null);
            }
        }

        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var controlTwoDA = GetCurrentTwoDAControl();
                var controlTLK = GetCurrentTLKControl();
                string path = dialog.FileName;
                
                if (controlTwoDA != null)
                {
                    SaveTwoDA(controlTwoDA, path);
                }
                if (controlTLK != null)
                {
                    SaveTLK(controlTLK, path);
                }
            }
        }

        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = dialog.FileName;
                if (Path.GetExtension(path) == ".tlk")
                {
                    TLKFile file = new TLKFile(Path.GetFileNameWithoutExtension(path));
                    file.Read(File.OpenRead(path));
                    OpenTLKFile(path, file, true);
                }
                else if (Path.GetExtension(path) == ".2da")
                {
                    TwoDAFile file = new TwoDAFile(Path.GetFileNameWithoutExtension(path));
                    file.Read(File.OpenRead(path));
                    file.Schema = Environment.SchemaDatabase.GetSchema(Path.GetFileNameWithoutExtension(path));
                    OpenTwoDAFile(path, file, true);
                }
            }
        }

        private TLKControl OpenTLKFile(string path, TLKFile file, bool select)
        {
            if (TLKControls.ContainsKey(file.Name))
            {
                var oldControl = TLKControls[file.Name];
                var oldPage = (TabPage)oldControl.Parent;
                if (MessageBox.Show("A tlk file with this name is already open. Close it and open this one?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CloseTab(oldPage);
                }
                else
                {
                    if (select)
                        tabControl1.SelectedTab = oldPage;
                    return oldControl;
                }
            }
            var page = new TabPage(Path.GetFileName(path));
            var control = new TLKControl(file, this);
            control.Dock = DockStyle.Fill;
            page.Controls.Add(control);
            tabControl1.TabPages.Add(page);
            if (select)
                tabControl1.SelectedTab = page;
            TLKControls.Add(file.Name, control);
            control.SetPath(path);
            return control;
        }

        private TwoDAControl OpenTwoDAFile(string path, TwoDAFile file, bool select)
        {
            if (TwoDAControls.ContainsKey(file.Name))
            {
                var oldControl = TwoDAControls[file.Name];
                var oldPage = (TabPage)oldControl.Parent;
                if (MessageBox.Show("A 2da file with this name is already open. Close it and open this one?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CloseTab(oldPage);
                }
                else
                {
                    if (select)
                        tabControl1.SelectedTab = oldPage;
                    return oldControl;
                }
            }
            var page = new TabPage(Path.GetFileName(path));
            var control = new TwoDAControl(file, this);
            control.Dock = DockStyle.Fill;
            page.Controls.Add(control);
            tabControl1.TabPages.Add(page);
            TwoDAControls.Add(file.Name, control);
            Environment.AddTwoDA(file);
            control.SetPath(path);
            if (select)
                tabControl1.SelectedTab = page;
            return control;
        }

        private void SaveTwoDA(TwoDAControl control, string path)
        {
            if (path != null)
                control.SetPath(path);
            control.File.Write(File.OpenWrite(control.FilePath));
        }

        private void SaveTLK(TLKControl control, string path)
        {
            if(path != null)
                control.SetPath(path);
            control.File.Write(File.OpenWrite(control.FilePath));
        }

        public void CloseTab(TabPage selectedTab)
        {
            if (selectedTab == null)
                return;
            var controlTwoDA = GetCurrentTwoDAControl();
            var controlTLK = GetCurrentTLKControl();
            if(controlTLK != null)
            {
                Environment.UnsetTLK(controlTLK.File);
                TLKControls.Remove(controlTLK.File.Name);
            }
            if(controlTwoDA != null)
            {
                Environment.RemoveTwoDA(controlTwoDA.File);
                TwoDAControls.Remove(controlTwoDA.File.Name);
            }
            tabControl1.TabPages.Remove(selectedTab);
        } 

        private void SetCurrentEnvironment(string path)
        {
            CurrentDirectory = new DirectoryInfo(path);
            CurrentFileSystemWatcher = new FileSystemWatcher(path);
            CurrentFileSystemWatcher.IncludeSubdirectories = true;
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            var tabControl = sender as TabControl;

            if (e.Button == MouseButtons.Right)
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        var page = tabControl.TabPages[i];
                        if (page.Controls.ContainsKey("TLKControl"))
                        {
                            var control = (TLKControl)page.Controls["TLKControl"];
                            control.menu_tlk.Show(tabControl.PointToScreen(e.Location));
                        }
                        if (page.Controls.ContainsKey("TwoDAControl"))
                        {
                            var control = (TwoDAControl)page.Controls["TwoDAControl"];
                            control.menu_2da.Show(tabControl.PointToScreen(e.Location));
                        }
                    }
                }
        }

        private void viewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            UpdateStrRefMenu();
            UpdateBoolMenu();
            UpdateFileRefMenu();
            UpdateTwoDARefMenu();
        }

        private void UpdateStrRefMenu()
        {
            menu_strref_strref.Checked = Environment.StrRefFormat == StrRefFormat.StrRefOnly;
            menu_strref_string.Checked = Environment.StrRefFormat == StrRefFormat.StringOnly;
            menu_strref_both.Checked = Environment.StrRefFormat == StrRefFormat.StrRefAndString;
            menu_strref_full.Checked = Environment.StrRefFormat == StrRefFormat.Full;
        }

        private void UpdateBoolMenu()
        {
            menu_bool_text.Checked = Environment.BoolFormat == BoolFormat.Text;
            menu_bool_number.Checked = Environment.BoolFormat == BoolFormat.Number;
        }

        private void UpdateFileRefMenu()
        {
            menu_fileref_original.Checked = Environment.FileRefFormat == FileRefFormat.Original;
            menu_fileref_lower.Checked = Environment.FileRefFormat == FileRefFormat.LowerCase;
            menu_fileref_upper.Checked = Environment.FileRefFormat == FileRefFormat.UpperCase;
        }

        private void UpdateTwoDARefMenu()
        {
            menu_2daref_label.Checked = Environment.TwoDARefFormat == TwoDARefFormat.Label;
            menu_2daref_lower.Checked = Environment.TwoDARefFormat == TwoDARefFormat.LowerCaseNumber;
            menu_2daref_upper.Checked = Environment.TwoDARefFormat == TwoDARefFormat.UpperCaseNumber;
        }


        private void UpdateEnvironment()
        {
            var fullPath = Environment.Directory.FullName;
            if(fullPath.Length > 60)
            {
                int stringLength = 0;
                fullPath = "..." + Path.DirectorySeparatorChar + String.Join(new string(Path.DirectorySeparatorChar,1),fullPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Reverse().TakeWhile(x =>
                 {
                     stringLength += x.Length + 1;
                     return stringLength <= 60;
                 }).Reverse()) + Path.DirectorySeparatorChar;
            }
            menu_environment.Text = fullPath;
        }

        private void menu_strref_strref_Click(object sender, EventArgs e)
        {
            Environment.SetStrRefFormat(StrRefFormat.StrRefOnly);
        }

        private void menu_strref_string_Click(object sender, EventArgs e)
        {
            Environment.SetStrRefFormat(StrRefFormat.StringOnly);
     }

        private void menu_strref_both_Click(object sender, EventArgs e)
        {
            Environment.SetStrRefFormat(StrRefFormat.StrRefAndString);
        }

        private void menu_strref_full_Click(object sender, EventArgs e)
        {
            Environment.SetStrRefFormat(StrRefFormat.Full);
        }

        private void menu_environment_set_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.DefaultDirectory = Environment.Directory.FullName;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                Environment.Directory = new DirectoryInfo(dialog.FileName);
                UpdateEnvironment();
            }
            
        }

        private void menu_fileref_upper_Click(object sender, EventArgs e)
        {
            Environment.SetFileRefFormat(FileRefFormat.UpperCase);
        }

        private void menu_fileref_lower_Click(object sender, EventArgs e)
        {
            Environment.SetFileRefFormat(FileRefFormat.LowerCase);
        }

        private void menu_fileref_original_Click(object sender, EventArgs e)
        {
            Environment.SetFileRefFormat(FileRefFormat.Original);
        }

        private void menu_2daref_label_Click(object sender, EventArgs e)
        {
            Environment.SetTwoDARefFormat(TwoDARefFormat.Label);
        }

        private void menu_2daref_upper_Click(object sender, EventArgs e)
        {
            Environment.SetTwoDARefFormat(TwoDARefFormat.UpperCaseNumber);
        }

        private void menu_2daref_lower_Click(object sender, EventArgs e)
        {
            Environment.SetTwoDARefFormat(TwoDARefFormat.LowerCaseNumber);
        }

        private void menu_bool_text_Click(object sender, EventArgs e)
        {
            Environment.SetBoolFormat(BoolFormat.Text);
        }

        private void menu_bool_number_Click(object sender, EventArgs e)
        {
            Environment.SetBoolFormat(BoolFormat.Number);
        }

        private void menu_edit_DropDownOpening(object sender, EventArgs e)
        {
            menu_2da.Enabled = GetCurrentTwoDAControl() != null;
            menu_tlk.Enabled = GetCurrentTLKControl() != null;
        }

        private void menu_2da_set_rows_Click(object sender, EventArgs e)
        {
            var file = GetCurrentTwoDAControl().File;
            var dialog = new NumberInputDialog(file.Height);
            DialogResult result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                file.Resize(file.Width, dialog.RowsPicked);
            }
        }

        private void menu_2da_renumber_Click(object sender, EventArgs e)
        {
            var file = GetCurrentTwoDAControl().File;
            file.Renumber();
        }

        private void menu_2da_schema_Click(object sender, EventArgs e)
        {
            var file = GetCurrentTwoDAControl().File;
            var dialog = new SchemaSelect(Environment.SchemaDatabase,file.Schema);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                file.SetSchema(dialog.SelectedSchema);
            }
        }

        private void menu_tlk_set_rows_Click(object sender, EventArgs e)
        {
            var file = GetCurrentTLKControl().File;
            var dialog = new NumberInputDialog((int)file.StringCount);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                file.Resize(dialog.RowsPicked);
            }
        }

        private void menu_cut_Click(object sender, EventArgs e)
        {
            ClipCut();
        }

        private void btn_cut_Click(object sender, EventArgs e)
        {
            ClipCut();
        }

        private void menu_copy_Click(object sender, EventArgs e)
        {
            ClipCopy();
        }

        private void btn_copy_Click(object sender, EventArgs e)
        {
            ClipCopy();
        }

        private void menu_paste_Click(object sender, EventArgs e)
        {
            ClipPaste();
        }

        private void btn_paste_Click(object sender, EventArgs e)
        {
            ClipPaste();
        }

        private void ClipCut()
        {
            var controlTwoDA = GetCurrentTwoDAControl();
            var controlTLK = GetCurrentTLKControl();
            string v = null;

            if (controlTwoDA != null)
            {
                v = controlTwoDA.ClipCut();
            }
            if (controlTLK != null)
            {
                v = controlTLK.ClipCut();
            }

            if (v != null)
                Clipboard.SetText(v);
        }

        private void ClipCopy()
        {
            var controlTwoDA = GetCurrentTwoDAControl();
            var controlTLK = GetCurrentTLKControl();
            string v = null;

            if (controlTwoDA != null)
            {
                v = controlTwoDA.ClipCopy();
            }
            if (controlTLK != null)
            {
                v = controlTLK.ClipCopy();
            }

            if (v != null)
                Clipboard.SetText(v);
        }

        private void ClipPaste()
        {
            var controlTwoDA = GetCurrentTwoDAControl();
            var controlTLK = GetCurrentTLKControl();

            if (!Clipboard.ContainsText())
                return;
            if (controlTwoDA != null)
            {
                controlTwoDA.ClipPaste(Clipboard.GetText());
            }
            if (controlTLK != null)
            {
                controlTLK.ClipPaste(Clipboard.GetText());
            }
        }
    }
}
