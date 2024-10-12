using AutoMapper;
using Caliburn.Micro;
using RetailManager.DesktopUI.Models;
using RetailManager.DesktopUI.UiEvents;
using RetailManager.UI.Core.Exceptions;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System.ComponentModel;
using System.Windows;

namespace RetailManager.DesktopUI.ViewModels
{
    public class UserDashboardViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private UserDisplayModel _selectedUser;
        private RoleModel _roleToAdd;
        private RoleModel _roleToRemove;
        private BindingList<UserDisplayModel> _users;
        private IEnumerable<RoleModel> _roles;

        public UserDashboardViewModel(
            IEventAggregator events,
            IAdminService adminService,
            IMapper mapper)
        {
            _events = events;
            _adminService = adminService;
            _mapper = mapper;
        }

        protected async override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);

            try
            {
                await InitializeFormAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await TryCloseAsync();
            }
        }

        private async Task InitializeFormAsync()
        {
            try
            {
                Users = new BindingList<UserDisplayModel>(await LoadUsers());
                _roles = await _adminService.GetRolesAsync();
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show("Your session has expired", "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await _events.PublishOnUIThreadAsync(new LogoutUiEvent());

                throw;
            }
        }

        private async Task<List<UserDisplayModel>> LoadUsers()
        {
            var users = await _adminService.GetUsersAsync();

            return _mapper.Map<List<UserDisplayModel>>(users);
        }

        #region UI Properties

        public BindingList<UserDisplayModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;

                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserDisplayModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        public RoleModel RoleToAdd
        {
            get => _roleToAdd;
            set
            {
                _roleToAdd = value;
                NotifyOfPropertyChange(() => RoleToAdd);
                NotifyOfPropertyChange(() => CanAddToRole);
            }
        }

        public RoleModel RoleToRemove
        {
            get => _roleToRemove;
            set
            {
                _roleToRemove = value;
                NotifyOfPropertyChange(() => RoleToRemove);
                NotifyOfPropertyChange(() => CanRemoveFromRole);
            }
        }

        public BindingList<RoleModel> AvailableRoles
        {
            get
            {
                if (SelectedUser == null)
                {
                    return new BindingList<RoleModel>();
                }

                return new BindingList<RoleModel>(
                    _roles
                    .Where(role => !SelectedUser.Roles.Any(userRole => userRole.Id == role.Id))
                    .ToList());
            }
        }

        public bool CanAddToRole => SelectedUser != null && RoleToAdd != null;

        #endregion

        public async void AddToRole()
        {
            var userRole = new UserRoleModel
            {
                UserId = SelectedUser.Id,
                Role = RoleToAdd.Name
            };

            try
            {
                await _adminService.AddUserToRoleAsync(userRole);

                SelectedUser.Roles.Add(RoleToAdd);
                var temp = SelectedUser.Roles;
                SelectedUser.Roles = null;
                SelectedUser.Roles = temp;

                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => AvailableRoles);
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show("Your session has expired", "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await _events.PublishOnUIThreadAsync(new LogoutUiEvent());

                throw;
            }
            catch (Exception)
            {
                // TODO: Handle the exception.
                throw;
            }
        }

        public bool CanRemoveFromRole => SelectedUser != null && RoleToRemove != null;

        public async void RemoveFromRole()
        {
            var userRole = new UserRoleModel
            {
                UserId = SelectedUser.Id,
                Role = RoleToRemove.Name
            };

            try
            {
                await _adminService.RemoveUserFromRoleAsync(userRole);

                SelectedUser.Roles = SelectedUser.Roles
                    .Where(role => role.Id != RoleToRemove.Id)
                    .ToList();

                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => AvailableRoles);
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show("Your session has expired", "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await _events.PublishOnUIThreadAsync(new LogoutUiEvent());

                throw;
            }
            catch (Exception)
            {
                // TODO: Handle the exception.
                throw;
            }
        }
    }
}
