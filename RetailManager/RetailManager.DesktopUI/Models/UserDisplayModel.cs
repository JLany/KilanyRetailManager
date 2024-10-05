using Caliburn.Micro;
using RetailManager.UI.Core.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace RetailManager.DesktopUI.Models
{
    public class UserDisplayModel : INotifyPropertyChanged
    {
        private List<RoleModel> _roles;

        public string Id { get; set; }
        public string Email { get; set; }

        public List<RoleModel> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                NotifyOfPropertyChange(() => Roles);
                NotifyOfPropertyChange(() => RolesList);
            }
        }

        public string RolesList => string.Join(", ", Roles?.Select(r => r.Name) ?? new List<string>());

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void NotifyOfPropertyChange<TResult>(Expression<Func<TResult>> property)
        {
            PropertyChanged?
                .Invoke(this, new PropertyChangedEventArgs(property.GetMemberInfo().Name));
        }
    }
}
