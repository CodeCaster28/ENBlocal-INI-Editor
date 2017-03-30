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
using System.IO;

namespace enboost {
	public partial class Form1:Form {

		short memoryValue = 0;
		public bool lockDescription;
		public bool advancedMode;
		bool lockSaving;
		bool freshStart = true;

		DialogResult messageResult = DialogResult.Yes;
		Control[] allTabs;
		Control tollTipControl;

		private void PickAllTabs () {
			allTabs = new Control[tabsIn.TabCount];
			for (int i = 0; i < tabsIn.TabCount; i++) {
				allTabs[i] = tabsIn.TabPages[i];
			}
			tollTipControl = menuStrip1;
		}

		private void StartNewFile (bool fromFile) {
			if (freshStart == true)
				freshStart = false;
			else
				messageResult = MessageBox.Show("Do you want to load a new File? Unsaved changes will be lost.", "Confirm Reload", MessageBoxButtons.YesNo);
			if (messageResult == DialogResult.Yes) {
				advancedMode = false;
				lockSaving = false;
				lockDescription = false;
				advancedModeToolStripMenuItem.Checked = false;
				HideCantSaveTooltip();
				HideDescription();
				FillForms(fromFile);
				ValidateAll();
			}
		}

		private void FillForms (bool fromFile) {
			FillButtons(allTabs, fromFile);
			FillInputNameLabels(fromFile);
			SetRestrictedVars();
			DisableUnsaveOptions(allTabs);
		}

		private void FillButtons (Control[] tabPages, bool fromFile) {
			for (int i = 0; i < tabPages.Length; i++) {
				foreach (Control control in tabPages[i].Controls) {
					if (control != null && control.Name.Substring(0, 2) == "b_") {
						string currentName = control.Name.Substring(2);
						string currentItemDefVal;
						if (fromFile == false)
							currentItemDefVal = Data.item[currentName].DefaultValue;
						else
							currentItemDefVal = Data.tempItemOpenFile[currentName].DefaultValue;
						control.Text = currentItemDefVal;
					}
				}
			}
		}
		
		private void FillInputNameLabels (bool fromFile) {
			foreach (Control control in tabPage7.Controls) {
				if (control is Label inputLabel)
				{
					if (inputLabel.Name.Substring(0, 2) == "k_")
					{
						string keyCode;
						if (fromFile == false)
							keyCode = Data.item[inputLabel.Name.Substring(2)].DefaultValue;
						else
							keyCode = Data.tempItemOpenFile[inputLabel.Name.Substring(2)].DefaultValue;
						Keys key_restored = (Keys)Enum.Parse(typeof(Keys), keyCode);
						inputLabel.Text = key_restored.ToString();
					}
				}
			}
		}

		private void DisableUnsaveOptions (Control[] tabPages) {
			for (int i = 0; i < tabPages.Length; i++) {
				foreach (Control control in tabPages[i].Controls) {
					if (control != null && control.Name.Substring(0, 2) == "b_") {
						string currentName = control.Name.Substring(2);
						bool currentItemUnsafe = Data.item[currentName].UnsafeOption;
						if (currentItemUnsafe == true)
							control.Enabled = false;
					}
				}
			}
		}

		private void SetRestrictedVars () {
			memoryValue = short.Parse(b_ReservedMemorySizeMb.Text);
		}

		private void ToggleButton (object sender) {
			Button button = sender as Button;
			button.Text = button.Text.Equals("true") ? "false" : "true";
			ValidateAll();
		}

		private void ShowDescription (object sender) {
			if (lockDescription == false) {
				string label = (sender as Label).Name.Substring(2);
				descriptionBox.Text = label + "\n" + Data.item[label].Description;
				descriptionBox.Focus();
			}
		}

		private void HideDescription () {
			if (lockDescription == false)
				descriptionBox.Text = "";
		}

		private void GoToMemTestLink () {
			System.Diagnostics.Process.Start("http://enbdev.com/download_vramsizetest.htm");
		}

		private void IncrementMemoryValue () {
			if (memoryValue < 1024) {
				memoryValue += memoryValue.Equals(64) ? (short)64 : (short)128;
				b_ReservedMemorySizeMb.Text = memoryValue.ToString();
			}
		}

		private void DecrementMemoryValue () {
			if (memoryValue > 64) {
				memoryValue -= memoryValue.Equals(128) ? (short)64 : (short)128;
				b_ReservedMemorySizeMb.Text = memoryValue.ToString();
			}
		}

		private void ToggleAdvancedMode () {
			if (advancedMode == false) {
				advancedMode = true;
				EnableAllOptions(allTabs);
				advancedModeToolStripMenuItem.Checked = true;
			}
			else {
				advancedMode = false;
				DisableUnsaveOptions(allTabs);
				advancedModeToolStripMenuItem.Checked = false;
			}
		}

		private void EnableAllOptions (Control[] tabPages) {
			for (int i = 0; i < tabPages.Length; i++) {
				foreach (Control control in tabPages[i].Controls) {
					if (control != null)
						control.Enabled = true;
				}
			}
		}

		private void ValidateAll () {
			lockSaving = false;
			ValidateTextboxes();
			ValidateButton();
			ValidateCombos();
			if (lockSaving == true) {
				saveEnblocaliniToolStripMenuItem.ForeColor = Color.Gray;
			}
			else {
				saveEnblocaliniToolStripMenuItem.ForeColor = SystemColors.ControlText;
			}
		}

		private void ValidateTextboxes () {
			
			for (int i = 0; i < allTabs.Length; i++) {
				foreach (Control control in allTabs[i].Controls) {
					if (control is TextBox textbox)
					{
						if (textbox.Name.Substring(0, 2) == "b_")
						{
							if (string.IsNullOrEmpty(textbox.Text))
							{
								lockSaving = true;
								textbox.BackColor = Color.LightPink;
							}
							else
								textbox.BackColor = SystemColors.Window;
						}
					}
				}
			}
		}

		private void ValidateButton () {
			for (int i = 0; i < allTabs.Length; i++) {
				foreach (Control control in allTabs[i].Controls) {
					if (control is Button button)
					{
						if (button.Name.Substring(0, 2) == "b_")
						{
							if (string.IsNullOrEmpty(button.Text))
							{
								button.BackColor = Color.LightPink;
								lockSaving = true;
							}
							else if (button.Text != "true" && button.Text != "false")
							{
								button.BackColor = Color.LightPink;
								lockSaving = true;
							}
							else
								button.BackColor = Color.Transparent;
						}
					}
				}
			}
		}

		private void ValidateCombos()
		{
			for (int i = 0; i < allTabs.Length; i++) {
				foreach (Control control in allTabs[i].Controls) {
					if (control is ComboBox combobox) {
						if (string.IsNullOrEmpty(combobox.Text)) {
							combobox.BackColor = Color.LightPink;
							lockSaving = true;
						}
						else if(combobox.Items.Contains(combobox.Text) == false){
							combobox.BackColor = Color.LightPink;
							lockSaving = true;
						}
						else
							combobox.BackColor = SystemColors.Window;
					}
				}
			}
		}

		private void ShowCantSaveTooltip (EventArgs e) {
			if (tollTipControl != null && lockSaving == true) {
				string toolTipString = "Can't save due to empty or incorrect fields.";
				saveEnblocaliniToolStripMenuItem.Enabled = false;
				toolTipSave.Show(toolTipString, tollTipControl, (tollTipControl.Width / 4) + 20, tollTipControl.Height * 3);
			}
		}

		private void HideCantSaveTooltip () {
			toolTipSave.Hide(tollTipControl);
			saveEnblocaliniToolStripMenuItem.Enabled = true;
		}

		private void OpenSaveDialogue () {
			if (lockSaving == false) {
				if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
					using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName)) {

						foreach (Description val in Enum.GetValues(typeof(Description))) {
							WriteSection(val.ToString(), writer);
						}
					}
				}
			}
		}

		private void BindKey (object sender, KeyEventArgs e) {
			TextBox textBox = sender as TextBox;
			textBox.Text = e.KeyValue.ToString();
			tabPage7.Controls.Find("k_" + textBox.Name.Substring(2), false)[0].Text = e.KeyCode.ToString();
		}

		private void WriteSection (string sectionName, StreamWriter writer) {
			writer.WriteLine("[" + sectionName + "]");
			for (int i = 0; i < allTabs.Length; i++) {
				foreach (Control control in allTabs[i].Controls) {
					if (control != null && control.Name.Substring(0, 2) == "b_") {
						string currentName = control.Name.Substring(2);
						Item currentItem = Data.item[currentName];
						if (currentItem.Section == sectionName) {
							string itemValue;
							if (advancedMode == true || (advancedMode == false && currentItem.UnsafeOption == false))
								itemValue = allTabs[i].Controls.Find(control.Name, true)[0].Text;
							else
								itemValue = currentItem.DefaultValue;
							writer.WriteLine(Data.item.FirstOrDefault(x => x.Value == currentItem).Key + "=" + itemValue);
						}
					}
				}
			}
			writer.WriteLine("");
		}

		private void OpenFile () {
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				bool invalidLines = false;
				List<string> invalidLinesRef = new List<string>();
				string[] lines;
				Data.tempItemOpenFile = Data.item;
				lines = File.ReadAllLines(openFileDialog1.FileName);
				foreach (string line in lines) {
					if (string.IsNullOrEmpty(line) != true) {
						if ((line[0] != '[') && (line[0] != '/')) {
							string[] items = line.Split('=');
							if (items.Length == 2 && (Data.tempItemOpenFile.ContainsKey(items[0]))) {
								Data.tempItemOpenFile[items[0]].DefaultValue = items[1];
							}
							else {
								invalidLines = true;
								invalidLinesRef.Add(line);
							}
						}
					}
				}
				StartNewFile(false);
				if (invalidLines == true) {
					invalidLines = false;
					string errorInfo = "\n";
					for (int i = 0; i < invalidLinesRef.Count; i++)
						errorInfo += invalidLinesRef[i] + "\n";
					messageResult = MessageBox.Show("Loaded file contains invalid lines at:" + errorInfo, "File Contains Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
