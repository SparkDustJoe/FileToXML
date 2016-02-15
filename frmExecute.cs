using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Xml.Schema;
using System.Resources;

namespace FileToXML
{
    public partial class frmExecute : Form
    {
        private Direction _direction = Direction.Encode;
        private bool _silent = false;
        private string _description = null;
        private Common.CompressionMethod _compressMethod = Common.CompressionMethod.None;

        public enum Direction: byte
        {
            Unspecified = 0,
            Encode,
            Decode
        }

        public frmExecute(Direction dir)
        {
            InitializeComponent();
            _direction = dir;
            if (_direction == Direction.Unspecified)
                throw new InvalidOperationException("A direction must be specified when creating a new frmExecute object");
        }

        private void frmExecute_Load(object sender, EventArgs e)
        {
            cmbCompression.SelectedIndex = 0;
            string cmdLine = Environment.CommandLine + " ";
            if (cmdLine.Contains(" -compress "))
            {
                chkCompress.Checked = true;
                chkCompress.Enabled = false;
            }
            if (cmdLine.Contains(" -clipboard "))
            {
                chkClipboard.Checked = true;
            }
            if (cmdLine.Contains(" -s ") || cmdLine.Contains(" -silent "))
            {
                _silent = true;
                chkOverWrite.Checked = true;
                chkOverWrite.Enabled = false;
            }
            if (cmdLine.Contains(" -o ") || cmdLine.Contains(" -overwrite "))
            {
                chkOverWrite.Checked = true;
                chkOverWrite.Enabled = false;
            }
            if (_direction == Direction.Encode)
            {
                this.Text = "ENCODE A FILE";
                chkCompress.Visible = true;
                chkDescription.Visible = true;
                chkClipboard.Text = "Encode To Clipboard...";
            }
            else if (_direction == Direction.Decode)
            {
                this.Text = "DECODE A FILE";
                chkCompress.Visible = false;
                chkDescription.Visible = false;
                chkClipboard.Text = "Watch Clipboard...";
            }
            
        }

        private void frmExecute_Shown(object sender, EventArgs e)
        {
            string description = null;
            Exception ex = null;
            string cmdLine = Environment.CommandLine + " ";
            if (cmdLine.Contains(" -source "))
            {
                string stuff = cmdLine.Substring(cmdLine.IndexOf(" -source \"") + 10);
                StringBuilder sb = new StringBuilder();
                int infinite = 0;
                do
                {
                    if (stuff[infinite] != '\"')
                    {
                        sb.Append(stuff[infinite]);
                        //stuff.Remove(0, 1);
                        infinite++;
                    }
                    else
                        break;
                } while (stuff.Length > 0 && infinite < 1024);
                if (infinite < 1024)
                {
                    if (File.Exists(sb.ToString()))
                    {
                        txtSource.Text = sb.ToString();
                        if (_direction == Direction.Encode)
                            txtDestination.Text = Path.GetFullPath(txtSource.Text) + ".b64.txt";
                        else
                            txtDestination.Text = Path.GetFullPath(txtSource.Text).Replace(Path.GetFileName(txtSource.Text), "")
                                + Common.ExtractOriginalFileName(txtSource.Text, out description);
                    }
                    else
                        System.Media.SystemSounds.Beep.Play();
                }
                else
                {
                    Common.LogException(new InvalidOperationException("Infinite loop: source"), out ex);
                }
            }
            if (cmdLine.Contains(" -destination "))
            {
                string stuff = cmdLine.Substring(cmdLine.IndexOf(" -destination \"") + 15);
                StringBuilder sb = new StringBuilder();
                int infinite = 0;
                do
                {
                    if (stuff[infinite] != '\"')
                    {
                        sb.Append(stuff[infinite]);
                        //stuff.Remove(0, 1);
                        infinite++;
                    }
                    else
                        break;
                } while (stuff.Length > 0 && infinite < 1024);
                if (infinite < 1024)
                {
                    txtDestination.Text = sb.ToString();
                    chkOverWrite_CheckedChanged(this, new EventArgs());
                }
                else
                {
                    Common.LogException(new InvalidOperationException("Infinite loop: destination"), out ex);
                }
            }
            if (cmdLine.Contains(" -description \""))
            {
                string stuff = cmdLine.Substring(cmdLine.IndexOf(" -description \"") + 15);
                StringBuilder sb = new StringBuilder();
                int infinite = 0;
                do
                {
                    if (stuff[infinite] != '\"')
                    {
                        sb.Append(stuff[infinite]);
                        //stuff.Remove(0, 1);
                        infinite++;
                    }
                    else
                        break;
                } while (stuff.Length > 0 && infinite < 1024);
                if (infinite < 1024)
                {
                    _description = sb.ToString();
                    _description = _description.Replace("<", "").Replace(">", "");
                    _description = _description.Substring(0, 255);
                    chkDescription.Checked = true;
                    chkDescription_CheckedChanged(this, new EventArgs());
                }
                else
                {
                    Common.LogException(new InvalidOperationException("Infinite loop: description"), out ex);
                }
            }
            if (cmdLine.Contains(" -auto ") || cmdLine.Contains(" -autoexit "))
            {
                btnExecute_Click(this, new EventArgs());
            }
            if (cmdLine.Contains(" -autoexit "))
            {
                Application.Exit();
            }
        }

        void frmExecute_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSource.Text) || txtSource.Text.Contains("::INVALID::"))
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDestination.Text) && !chkClipboard.Checked)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            if (File.Exists(txtDestination.Text) && !chkOverWrite.Checked && !_silent)
            {
                if (!_silent)
                    MessageBox.Show(this, "Oops!  The destination file already exists! \r\n" +
                        "Check the box to overwrite or change the destination filename and try again.",
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Media.SystemSounds.Beep.Play();
                return;
            }
            if (this._direction == Direction.Encode && string.IsNullOrWhiteSpace(this._description) && chkDescription.Checked)
            {
                if (!_silent)
                    MessageBox.Show(this, "Oops!  You forgot your description! \r\n" +
                        "Click the '...' button next to the Description checkbox to set the description or uncheck the box.",
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Media.SystemSounds.Beep.Play();
                return;
            }
            this.Enabled = false;
            Exception exceptionArg = null;
            bool success = false;
            if (_direction == Direction.Encode)
            {
                string tempDesc = (chkDescription.Checked == true) ? _description : null;
                if (!chkClipboard.Checked)
                    success = Common.WriteEncodedFile(
                        txtSource.Text, txtDestination.Text, chkCompress.Checked, _compressMethod, tempDesc, out exceptionArg);
                else
                {
                    string TempPath = GetMyCurrentRunFromPath() + "!!clipboard.b64.txt";
                    success = Common.WriteEncodedFile(txtSource.Text, TempPath, chkCompress.Checked, _compressMethod, tempDesc, out exceptionArg);
                    if (exceptionArg == null)
                    {
                        Clipboard.SetText(File.ReadAllText(TempPath));
                        Application.DoEvents();
                        File.Delete(TempPath);
                    }
                }
            }
            else
            {
                success = Common.WriteDecodedFile(txtSource.Text, txtDestination.Text, out exceptionArg);
            }
            if (!success)
            {
                if (!_silent)
                    MessageBox.Show(this, "Oops!  Something went wrong: " + exceptionArg.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = Common.LogException(exceptionArg, out exceptionArg);
                if (!success)
                    MessageBox.Show(this, "Double oops!  Something went wrong trying to even log the error: " + exceptionArg.Message,
                        "ERROR - SILENT FLAG IGNORED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                if (!_silent)
                    MessageBox.Show("Done.");
            this.Enabled = true;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            lblSource.Text = "SOURCE:";
            if (!string.IsNullOrWhiteSpace(txtSource.Text) && !txtSource.Text.Contains("::INVALID::"))
            {
                ofdLoad.FileName = Path.GetFileName(txtSource.Text);
                ofdLoad.InitialDirectory = Path.GetFullPath(txtSource.Text);
            }
            if (_direction == Direction.Encode)
            {
                ofdLoad.Filter = "All Files *.*|*.*";
                sfdSave.Filter = "XML Files *.xml|*.xml|Base64 Encoded Files *.b64.txt|*.b64.txt|All Files *.*|*.*";
            }
            else
            {
                ofdLoad.Filter = "XML Files *.xml|*.xml|Base64 Encoded Files *.b64.txt|*.b64.txt|All Files *.*|*.*";
                sfdSave.Filter = "All Files *.*|*.*";
            }
            DialogResult ret = ofdLoad.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                txtSource.Text = ofdLoad.FileName;
                if (_direction == Direction.Encode)
                {//ENCODING==================================================================
                    if (string.IsNullOrWhiteSpace(txtDestination.Text) && !chkClipboard.Checked)
                    {
                        txtDestination.Text = txtSource.Text + ".xml";
                        if (File.Exists(txtDestination.Text))
                        {
                            if (!_silent && !chkOverWrite.Checked)
                                MessageBox.Show(this, "Caution:  The destination filename that was auto-populated already exists.\r\n\r\n" +
                                    "If this file should be overwritten, please check the box to overwrite.",
                                    "CAUTION: FILE ALREADY EXISTS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            if (!_silent && !chkOverWrite.Checked)
                                lblDestination.Text = "DESTINATION:  CAUTION - FILE ALREADY EXISTS";
                        }
                    }
                }
                else
                {//DECODING==================================================================
                    string description = null;
                    string origFilename = Common.ExtractOriginalFileName(txtSource.Text, out description);
                    if (string.IsNullOrWhiteSpace(origFilename))
                    {
                        txtSource.Text = "::INVALID:: Source is not valid XML.  Please choose a different file.";
                        txtDestination.Text = "";
                        //txtDestination.Text = Path.GetFullPath(txtSource.Text) +
                        //    Path.GetFileName(txtSource.Text).ToLower().Replace(".b64.txt", "");
                    }
                    else
                        txtDestination.Text = Path.GetFullPath(txtSource.Text).Replace(Path.GetFileName(txtSource.Text), "")
                            + origFilename;
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        MessageBox.Show(this, "The file contains the following description:\r\n\r\n" + description,
                            "FILE DESCRIPTION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void LabelSourceAsInvalid()
        {
            lblSource.Text = "SOURCE IS NOT A VALID DOCUMENT FOR THIS PROGRAM TO DECODE!";
            txtSource.Text = "::INVALID::" + txtSource.Text;
            txtDestination.Clear();
        }

        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                sfdSave.FileName = Path.GetFileName(txtDestination.Text);
                sfdSave.InitialDirectory = Path.GetFullPath(txtDestination.Text);
            }
            lblDestination.Text = "DESTINATION:";
            if (_direction == Direction.Encode)
            {
                ofdLoad.Filter = "All Files *.*|*.*";
                sfdSave.Filter = "XML Files *.xml|*.xml|Base64 Encoded Files *.b64.txt|*.b64.txt|All Files *.*|*.*";
            }
            else
            {
                ofdLoad.Filter = "XML Files *.xml|*.xml|Base64 Encoded Files *.b64.txt|*.b64.txt|All Files *.*|*.*";
                sfdSave.Filter = "All Files *.*|*.*";
            }
            DialogResult ret = sfdSave.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                txtDestination.Text = sfdSave.FileName;
            }
        }

        private void chkOverWrite_CheckedChanged(object sender, EventArgs e)
        {
            txtDestination_TextChanged(null, null);
        }

        private void chkDescription_CheckedChanged(object sender, EventArgs e)
        {
            btnEditDescription.Visible = chkDescription.Checked;
        }

        private void btnEditDescription_Click(object sender, EventArgs e)
        {
            frmFileDescription fileDesc = new frmFileDescription(_description);
            DialogResult ret = fileDesc.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                _description = fileDesc.Tag as string;
            }
            fileDesc.Dispose();
        }

        private void chkClipboard_CheckedChanged(object sender, EventArgs e)
        {
            if (this._direction == Direction.Encode)
            {
                if (chkClipboard.Checked)
                {
                    btnBrowseDestination.Visible = false;
                    txtDestination.Visible = false;
                    txtDestination.Text = "";
                    chkOverWrite.Visible = false;
                    chkOverWrite.Checked = false;
                }
                else
                {
                    chkOverWrite.Visible = true;
                    btnBrowseDestination.Visible = true;
                    txtDestination.Visible = true;
                    txtDestination.Text = "";
                }
            }
            else
            {
                if (chkClipboard.Checked)
                {
                    btnBrowseSource.Enabled = false;
                    btnBrowseSource.Visible = false;
                    txtSource.Text = "Listening for clipboard activity...";
                    btnExecute.Enabled = false;
                    tmrCilpboardPoll.Enabled = true;
                }
                else
                {
                    btnBrowseSource.Enabled = true;
                    btnBrowseSource.Visible = true;
                    txtSource.Text = "";
                    tmrCilpboardPoll.Enabled = false;
                    btnExecute.Enabled = true;
                }
            }
        }

        private void tmrCilpboardPoll_Tick(object sender, EventArgs e)
        {
            tmrCilpboardPoll.Enabled = false;
            if (Clipboard.ContainsText(TextDataFormat.Text) || Clipboard.ContainsText(TextDataFormat.UnicodeText))
            {
                string cText = Clipboard.GetText();
                if (cText.Contains("<EncodedFile>") && cText.Contains("</EncodedFile>"))
                {
                    tmrCilpboardPoll.Enabled = false;
                    txtSource.Text = "Found something... analyzing...";
                    Application.DoEvents();
                    Assembly a = Assembly.GetExecutingAssembly();
                    string TempPath = GetMyCurrentRunFromPath() + "!!clipboard.b64.txt";
                    File.WriteAllText(TempPath, cText);

                    string xsd = Properties.Resources.EncodedFileSchema;

                    XmlSchemaSet schema = new XmlSchemaSet();
                    schema.Add("", XmlReader.Create(new StringReader(xsd)));
                    XDocument x = XDocument.Load(TempPath);

                    bool problems = false;
                    ValidationEventArgs problem = null;
                    x.Validate(schema, (o, ee) => { problems = true; problem = ee; });
                    if (problems)
                    {
                        MessageBox.Show(this, "The encoded file in the clipboard currently has issues and will be removed: \r\n" + problem.Message +
                            "\r\n\r\nIf you think you've received this message in error, examine the XML to make sure that the format is correct, that there " +
                            "are no unsual characters, and that all XML fields are properly terminated.  A sample encoded file and XSD should have been " +
                            "included with the program for reference.");
                        chkClipboard.Checked = false;
                        chkClipboard_CheckedChanged(null, null);
                        File.Delete(TempPath);
                        Clipboard.Clear();
                        return;
                    }
                    //if everything is ok, then get ready to decode the temp file.
                    txtSource.Text = TempPath;
                    Application.DoEvents();
                    string description = null;
                    string origFilename = Common.ExtractOriginalFileName(txtSource.Text, out description);
                    if (string.IsNullOrWhiteSpace(origFilename))
                        txtDestination.Text = Path.GetFullPath(txtSource.Text) +
                            Path.GetFileName(txtSource.Text).ToLower().Replace(".b64.txt", "");
                    else
                        txtDestination.Text = Path.GetFullPath(txtSource.Text).Replace(Path.GetFileName(txtSource.Text), "")
                            + origFilename;
                    DialogResult res = DialogResult.Yes;
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        res = MessageBox.Show(this, "The file contains the following description:\r\n\r\n" + description + "\r\n\r\n" +
                            "Do you still wish to write the original file to disk?\r\n" +
                            origFilename, "FILE DESCRIPTION", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    }
                    else
                    {
                        res = MessageBox.Show(this, "The file contains no description.\r\n\r\n" +
                                "Do you still wish to write the original file to disk?\r\n" +
                                origFilename, "FILE DESCRIPTION", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    }
                    if (res == DialogResult.Yes)
                    {
                        btnBrowseDestination_Click(null, null);
                        btnExecute_Click(null, null);
                    }
                    else
                    {
                        txtDestination.Text = "*Operation Canceled*";
                    }
                    File.Delete(TempPath);
                    chkClipboard.Checked = false;
                    Clipboard.Clear();
                }
                else
                    cText = null;
            }
            if (chkClipboard.Checked)
                tmrCilpboardPoll.Enabled = true;
        }

        public static string GetMyCurrentRunFromPath()
        {
            string theAssembly = Assembly.GetAssembly(typeof(frmExecute)).CodeBase;
            string thePath = Path.GetFullPath(
                theAssembly.Replace("file:///", "")).Replace(
                Path.GetFileName(theAssembly), "");
            if (!thePath.EndsWith(@"\"))
                thePath += @"\";
            return thePath;
        }

        private void txtDestination_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(txtDestination.Text) && !chkClipboard.Checked && !chkOverWrite.Checked && !_silent)
            {
                lblDestination.Text = "DESTINATION:  CAUTION - FILE ALREADY EXISTS!";
            }
            else
                lblDestination.Text = "DESTINATION:";

        }

        private void chkCompress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCompress.Checked)
            {
                cmbCompression.SelectedIndex = 1;
                _compressMethod = Common.CompressionMethod.GZip;
            }
            else
            {
                _compressMethod = Common.CompressionMethod.None;
                cmbCompression.SelectedIndex = 0;
            }
        }

        private void btnModes_Click(object sender, EventArgs e)
        {
            DialogResult res = 
                MessageBox.Show(
                    this, "Are you sure you want to change modes?","CHANGE MODES?",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.OK)
            {
                Application.Restart();
            }
        }

        private void cmbCompression_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if using a method other than GZip, set that here
            if (cmbCompression.SelectedIndex == 0)
                _compressMethod = Common.CompressionMethod.None;
            else
            {
                _compressMethod = Common.CompressionMethod.GZip;
            }
        }
    }
}
