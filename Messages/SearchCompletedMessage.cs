using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;

namespace QuanLyDaiLy.Messages;

public class SearchCompletedMessage<T>(ObservableCollection<T> results) : ValueChangedMessage<ObservableCollection<T>>(results) {}