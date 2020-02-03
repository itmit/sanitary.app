using System.Collections.Generic;
using sanitary.app.Models;

namespace sanitary.app.ViewModels
{
	public class ListSubcategoriesViewModel
	{
			#region Fields
			private List<Directory> _directoryList;
			#endregion
			public ListSubcategoriesViewModel()
			{
				DirectoryList = new List<Directory>
				{
					new Directory
					{
						Image = "pic_example11_dir_512w.png",
						Title = "Заглушки"
					},
					new Directory
					{
						Image = "pic_example12_dir_512w.png",
						Title = "Угольник"
					},
					new Directory
					{
						Image = "pic_example13_dir_512w.png",
						Title = "Тройники"
					},
					new Directory
					{
						Image = "pic_example11_dir_512w.png",
						Title = "Заглушки"
					},
					new Directory
					{
						Image = "pic_example12_dir_512w.png",
						Title = "Угольник"
					},
					new Directory
					{
						Image = "pic_example13_dir_512w.png",
						Title = "Тройники"
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

