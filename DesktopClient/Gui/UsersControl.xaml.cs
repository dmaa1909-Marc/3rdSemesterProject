using DesktopClient.Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopClient {
    /// <summary>
    /// Interaction logic for UsersControl.xaml
    /// </summary>
    public partial class UsersControl : UserControl {
        private UserAccountCtr userAccountCtr = new UserAccountCtr();
        private AvatarsCtr avatarsCtr = new AvatarsCtr();

        private ObservableCollection<UserAccount> UserAccounts = new ObservableCollection<UserAccount>();
        private ObservableCollection<Avatar> Avatars = new ObservableCollection<Avatar>();

        public UsersControl() {
            InitializeComponent();

            UpdateUserProgressBar.IsIndeterminate = false;

            UserAccountsListBox.DataContext = UserAccounts;
            userAvatartCombobox.DataContext = Avatars;

            GetAllUserAccounts();
            UpdateAvatarsCombobox();
        }

        public UserAccount UserAccountBeingEdited {
            get { return (UserAccount)GetValue(UserAccountBeingEditedProperty); }
            set { SetValue(UserAccountBeingEditedProperty, value); }
        }

        public static readonly DependencyProperty UserAccountBeingEditedProperty =
            DependencyProperty.Register("UserAccountBeingEdited", typeof(UserAccount),
                typeof(UsersControl), new PropertyMetadata(null));


        private async void GetAllUserAccounts() {
            SearchUsersProgressBar.IsIndeterminate = true;
            IEnumerable<UserAccount> foundUserAccounts = await Task.Run(() => userAccountCtr.GetAllUserAccounts());
            SearchUsersProgressBar.IsIndeterminate = false;
            UpdateUserAccountsList(foundUserAccounts);
        }

        private async void SearchForUsers() {
            SearchUsersProgressBar.IsIndeterminate = true;
            string searchString = searchUsersTextBox.Text;

            IEnumerable<UserAccount> foundUserAccounts = await Task.Run(() => userAccountCtr.FindUserAccountByUsernameOrEmail(searchString));
            SearchUsersProgressBar.IsIndeterminate = false;
            UpdateUserAccountsList(foundUserAccounts);
        }

        private void UpdateUserAccountsList(IEnumerable<UserAccount> userAccounts) {
            UserAccounts.Clear();
            if(userAccounts != null) {
                foreach(UserAccount userAccount in userAccounts) {
                    UserAccounts.Add(userAccount);
                }
                UserAccountsListBox.SelectedIndex = 0;
            }
            else {
                MessageBox.Show("Couldn't update user accounts list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateAvatarsCombobox() {
            IEnumerable<Avatar> foundAvatars = await Task.Run(() => avatarsCtr.GetAllAvatars());
            Avatars.Clear();
            if(foundAvatars != null) {
                foreach(Avatar avatar in foundAvatars) {
                    Avatars.Add(avatar);
                }
                SelectUsersAvatar();
            }
            else {
                MessageBox.Show("Couldn't update avatars dropdown", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SelectUsersAvatar();
        }

        private async void UpdateUserAccountBtn_Click(object sender, RoutedEventArgs e) {
            UpdateUserProgressBar.IsIndeterminate = true;
            //Must load DP value into local variable, as it's not accessible from another thread
            UserAccount userAccount = UserAccountBeingEdited;
            await Task.Run(() => userAccountCtr.UpdateUserAccount(userAccount));
            UpdateUserProgressBar.IsIndeterminate = false;
            SearchForUsers();
        }

        private void UserAccountsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            UserAccount selectedAccount = (UserAccount)UserAccountsListBox.SelectedItem;
            if(selectedAccount != null) {
                UserAccountBeingEdited = selectedAccount.Clone();
                userDetailsGrid.IsEnabled = true;
            } else {
                UserAccountBeingEdited = null;
                userDetailsGrid.IsEnabled = false;
            }
            SelectUsersAvatar();
        }

        private void SelectUsersAvatar() {
            if(UserAccountBeingEdited != null) {
                Avatar foundAvatar = Avatars.FirstOrDefault(avatar => avatar.Id == UserAccountBeingEdited.AvatarId);
                userAvatartCombobox.SelectedItem = foundAvatar ?? null;
            }
            else {
                userAvatartCombobox.SelectedItem = null;
                UpdateAvatarsCombobox();
            }
        }

        private void UserAvatartCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(UserAccountBeingEdited != null && userAvatartCombobox.SelectedItem != null) {
                UserAccountBeingEdited.AvatarId = ((Avatar)userAvatartCombobox.SelectedItem).Id;
            }
        }

        private void SearchEmailTxt_TextChanged(object sender, TextChangedEventArgs e) {
            SearchForUsers();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            SearchForUsers();
        }


        
        /////////////// Cannot delete from desktop client... //////////////////
        
        //private async void DeleteUserBtn_Click(object sender, RoutedEventArgs e) {
        //    MessageBoxResult result = MessageBox.Show("Are you sure you want perform this deletion?\n" +
        //        "This action can't be undone!", "Delete user?", MessageBoxButton.YesNo, MessageBoxImage.Question);

        //    if(result == MessageBoxResult.Yes) {
        //        var userAccount = UserAccountBeingEdited;
        //        bool deleted = await Task.Run(() => userAccountCtr.DeleteUserAccount(userAccount.Id));
        //        if(!deleted) {
        //            MessageBox.Show("Delete failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        SearchForUsers();
        //    }
        //}
    }
}
