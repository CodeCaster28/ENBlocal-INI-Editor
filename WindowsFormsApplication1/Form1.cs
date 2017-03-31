using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace enboost {
	public partial class Form1:Form {
		// TODO: Add support for SSAWaterTransparencyFix
		public Form1 () {
			InitializeComponent();
			PickAllTabs();
		}

		private void Form1_Load (object sender, EventArgs e) {
			StartNewFile(false);
		}

		private void button_Click (object sender, EventArgs e) {
			ToggleButton(sender);
		}

		private void label_MouseEnter (object sender, EventArgs e) {
			ShowDescription(sender);
		}

		private void label_MouseLeave (object sender, EventArgs e) {
			HideDescription();
		}

		private void button_linked_Click (object sender, EventArgs e) {
			GoToMemTestLink();
		}

		private void buttonIncrement_Click (object sender, EventArgs e) {
			IncrementMemoryValue();
		}

		private void buttonDecrement_Click (object sender, EventArgs e) {
			DecrementMemoryValue();
		}
		
		private void advancedModeButton_Click (object sender, EventArgs e) {
			ToggleAdvancedMode();
		}

		private void suppressWarningsToolStripMenuItem_Click(object sender, EventArgs e) {
			ToggleSuppressWarnings();
		}

		private void l_lockDescription (object sender, EventArgs e) {
			lockDescription = lockDescription == false ? true : false;
			ShowDescription(sender);
			lockDescription = true;
		}

		private void unlockDescription (object sender, EventArgs e) {
			lockDescription = false;
			descriptionBox.Text = "";
		}

		private void SaveEnblocaliniToolStripMenuItem_Click (object sender, EventArgs e) {
			OpenSaveDialogue();
		}

		private void b_TextChanged (object sender, EventArgs e) {
			ValidateAll();
		}
		
		private void saveEnblocaliniToolStripMenuItem_MouseEnter (object sender, EventArgs e) {
			ShowCantSaveTooltip(e);
		}

		private void fileToolStripMenuItem_DropDownClosed (object sender, EventArgs e) {
			HideCantSaveTooltip();
		}
		
		private void textBoxBindableKeyUp (object sender, KeyEventArgs e) {
			BindKey(sender, e);
		}

		private void newToolStripMenuItem_Click (object sender, EventArgs e) {
			
			StartNewFile(false);
		}

		private void openToolStripMenuItem_Click (object sender, EventArgs e) {
			OpenFile();
		}

		private void aboutENBlocalINIEditorToolStripMenuItem_Click (object sender, EventArgs e) {
			Form form = new About();
			form.ShowDialog(this);
		}

		private void button_decrement2_Click(object sender, EventArgs e) {
			DecrementAnizotropyValue();
		}

		private void button_increment2_Click(object sender, EventArgs e) {
			IncrementAnizotropyValue();
		}
	}
}
