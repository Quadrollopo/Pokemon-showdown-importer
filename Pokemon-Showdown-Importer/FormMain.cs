using System;
using System.Timers;
using System.Windows.Forms;

namespace PokemonShowdownImporter {
	public partial class FormMain : Form {
		public FormMain() {
			InitializeComponent();
		}
		

		private void importButton_Click(object sender, EventArgs e) {
			if (!BitConverter.IsLittleEndian) {
				throw new NotSupportedException("This program don't support (yet) your architecture");
			}
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter =
				@"Sav files | *.sav| Dsv files | *.dsv| All files (*.*)|*.*";
			dialog.Multiselect = false; 
			if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
			{
				string import = Importer.ReadSaveFile(dialog.FileName);
				richTextBox1.Text = import;
				copyButton.Visible = true;
			}
		}

		private void copyButton_Click(object sender, EventArgs e) {
			Clipboard.SetText(richTextBox1.Text);
			notify.Visible = true;
			timer1.Enabled = true;
		}

		private void timer1_Elapsed(object sender, ElapsedEventArgs e) {
			notify.Visible = false;
		}
	}
}