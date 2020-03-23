using System.Collections.Generic;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
    public class DirectoryViewModel
	{
		#region Fields
		private List<Directory> _directoryList;
		#endregion
		public DirectoryViewModel()
		{
			DirectoryList = new List<Directory>
			{
				new Directory
				{
					Title = "Унитаз",
					Image = "pic_example_dir_512w.png"
				},
				new Directory
				{
					Title = "Раковина",
					Image = "pic_example1_dir_512w.png"
				},
				new Directory
				{
					Title = "Смесители",
					Image = "pic_example2_dir_512w.png"
				},
				new Directory
				{
					Title = "Полипропилен ПП",
					Image = "pic_example3_dir_512w.png"
				},
				new Directory
				{
					Title = "Краны",
					Image = "pic_example5_dir_512w.png"
				},
				new Directory
				{
					Title = "Счетчики",
					Image = "pic_example4_dir_512w.png"
				}
			};
		}

		#region Prop
		public List<Directory> DirectoryList
		{
			get => _directoryList;
			set => _directoryList = value;
		}
		#endregion
	}
}
