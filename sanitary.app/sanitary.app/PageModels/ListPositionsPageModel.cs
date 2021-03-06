﻿using PropertyChanged;
using sanitary.app.Models;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using sanitary.app.Services;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class ListPositionsPageModel : FreshBasePageModel
    {
        readonly IDirectoryStorageService _directoryStorage;
        Directory _selectedPosition;
        Directory Directory;

        public ObservableCollection<Directory> Directories { get; set; } = new ObservableCollection<Directory>();

        private List<Directory> AllDirectories { get; set; } = new List<Directory>();

        public ListPositionsPageModel(IDirectoryStorageService directoryStorage)
        {
            _directoryStorage = directoryStorage;
        }

        public Directory SelectedPosition
        {
            get { return _selectedPosition; }
            set
            {
                _selectedPosition = value;
                if (value != null)
                    OpenPage(value);
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            Directory = (Directory)initData;
            CurrentPage.Title = Directory.Title;
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (IsThereInternet() == false)
            {
                return;
            }

            await CreateListsAsync();
        }

        async Task CreateListsAsync()
        {
            var updatedList = await _directoryStorage.GetPositionsAsync(Directory.uuid);

            if (AllDirectories.SequenceEqual(updatedList) == false)
            {
                AllDirectories = await _directoryStorage.GetPositionsAsync(Directory.uuid);
                Directories = new ObservableCollection<Directory>(AllDirectories.ToList());
            }
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        async void OpenPage(Directory selectedPosition)
        {
            await CoreMethods.PushPageModel<CardPositionPageModel>(selectedPosition);
        }
    }
}
