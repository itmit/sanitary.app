using System.Collections.Generic;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class ListPositionsViewModel
	{
		#region Fields
		private List<Directory> _directoryList;
		#endregion

		public ListPositionsViewModel()
		{
			DirectoryList = new List<Directory>
			{
				new Directory
				{
					Image = "pic_example11_dir_512w.png",
					Title = "Заглушки (110) PPRC Pro Aqua"
				},
				new Directory
				{
					Image = "pic_example12_dir_512w.png",
					Title = "Заглушки (110) PPRC Pro Aqua"
				},
				new Directory
				{
					Image = "pic_example13_dir_512w.png",
					Title = "Заглушки (110) PPRC Pro Aqua"
				},
				new Directory
				{
					Image = "pic_example11_dir_512w.png",
					Title = "Заглушки (110) PPRC Pro Aqua"
				},
				new Directory
				{
					Image = "pic_example12_dir_512w.png",
					Title = "Заглушки (110) PPRC Pro Aqua"
				},
				new Directory
				{
					Image = "pic_example13_dir_512w.png",
					Title = "Заглушки (110) PPRC Pro Aqua"
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
