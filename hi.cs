using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Update
{
	// Token: 0x02000002 RID: 2
	public class Form1 : Form
	{
		// Token: 0x06000001 RID: 1
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWow64Process([In] IntPtr hProcess, out bool wow64Process);

		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public static bool InternalCheckIsWow64()
		{
			bool flag = (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major >= 6;
			if (flag)
			{
				using (Process currentProcess = Process.GetCurrentProcess())
				{
					bool result;
					bool flag2 = !Form1.IsWow64Process(currentProcess.Handle, out result);
					if (flag2)
					{
						return false;
					}
					return result;
				}
			}
			return false;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E8 File Offset: 0x000002E8
		public Form1()
		{
			this.InitializeComponent();
			base.CenterToScreen();
			base.TopMost = true;
			new Thread(delegate()
			{
				this.ExtractRAR(Application.StartupPath.ToString() + "\\update.rar", Application.StartupPath.ToString());
				File.Delete("update.rar");
				base.Hide();
				MessageBox.Show("Cập nhập thành công !");
				Application.Exit();
			})
			{
				IsBackground = true
			}.Start();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000213C File Offset: 0x0000033C
		private void ExtractRAR(string rar_file, string path_file)
		{
			Thread.Sleep(5000);
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			bool flag = Form1.is64BitOperatingSystem;
			if (flag)
			{
				processStartInfo.FileName = "Rar.exe";
			}
			else
			{
				processStartInfo.FileName = "Rar32bit.exe";
			}
			processStartInfo.Arguments = string.Concat(new string[]
			{
				"x -y \"",
				rar_file,
				"\" \"",
				path_file,
				"\""
			});
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Process process = Process.Start(processStartInfo);
			process.WaitForExit();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021C8 File Offset: 0x000003C8
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002200 File Offset: 0x00000400
		private void InitializeComponent()
		{
			this.label1 = new Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(112, 39);
			this.label1.Name = "label1";
			this.label1.Size = new Size(84, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "AUTO UPDATE";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(323, 95);
			base.Controls.Add(this.label1);
			base.Name = "Form1";
			this.Text = "Form1";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000001 RID: 1
		private static bool is64BitProcess = IntPtr.Size == 8;

		// Token: 0x04000002 RID: 2
		private static bool is64BitOperatingSystem = Form1.is64BitProcess || Form1.InternalCheckIsWow64();

		// Token: 0x04000003 RID: 3
		private IContainer components = null;

		// Token: 0x04000004 RID: 4
		private Label label1;
	}
}
