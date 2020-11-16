using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMacbeth.Client.VVM.SearchResultsVVM
{
    public class SearchResultsDispatch
    {
        public Order order;
        public SearchResultsDispatch(object selectedItem)
        {

            order = new Order();
            if (selectedItem is Book)
            {
                Book book = (Book)selectedItem;
                order.Book = book;
                CreateOrder(order);
                MessageBox.Show($"Book \"{book.Name}\" added to your orders", "Book added", MessageBoxButton.OK,
              MessageBoxImage.Information);
            }
            else if (selectedItem is Article)
            {
                Article article = (Article)selectedItem;
                order.Article = article;
                CreateOrder(order);
                MessageBox.Show($"Article \"{article.Name}\" added to your orders", "Article added", MessageBoxButton.OK,
                   MessageBoxImage.Information);
            }
            else if (selectedItem is Periodical)
            {
                Periodical periodical = (Periodical)selectedItem;
                order.Periodical = periodical;
                CreateOrder(order);
                MessageBox.Show($"Periodical \"{periodical.Name}\" added to your orders", "Periodical added", MessageBoxButton.OK,
                 MessageBoxImage.Information);
            }
            else if (selectedItem is Newspaper)
            {
                Newspaper newspaper = (Newspaper)selectedItem;
                order.Newspaper = newspaper;
                CreateOrder(order);
                MessageBox.Show($"Newspaper \"{newspaper.Name}\" added to your orders", "Newspaper added", MessageBoxButton.OK,
               MessageBoxImage.Information);
            }
            else if (selectedItem is Thesis)
            {
                Thesis theses = (Thesis)selectedItem;
                order.Thesis = theses;
                CreateOrder(order);
                MessageBox.Show($"Theses \"{theses.Name}\" added to your orders", "Theses added", MessageBoxButton.OK,
               MessageBoxImage.Information);
            }

        }
        public void CreateReadingRoomOrder(ReadingRoomOrder readingRoomOrder)
        {
            var editDateTimeViewModel = new EditDateTimeViewModel();
            new EditDateTimeWindow { DataContext = editDateTimeViewModel }.ShowDialog();
            DateTime receiveDateTime = editDateTimeViewModel.ReceiveDate;
            order.ReceiveDate = receiveDateTime.ToUniversalTime();
            order.UserLogin = UserService.LoggedInUser.Login;
            order.TypeOfOrder = "Reading room";
            order.StatusOfOrder = "Order in processing";
            DateTime dateOfOrdering = DateTime.Now.ToUniversalTime();
            order.DateOfOrder = dateOfOrdering;
             
            ServerAdapter.Instance.CreateOrder(order);
        }


        public void CreateOrder(Order order)
        {
            var editDateTimeViewModel = new EditDateTimeViewModel();
            new EditDateTimeWindow { DataContext = editDateTimeViewModel }.ShowDialog();
            DateTime receiveDateTime = editDateTimeViewModel.ReceiveDate;
            order.ReceiveDate = receiveDateTime.ToUniversalTime();
            order.UserLogin = UserService.LoggedInUser.Login;
            order.TypeOfOrder = "Issueing order";
            order.StatusOfOrder = "Order in processing";
            DateTime dateOfOrdering = DateTime.Now.ToUniversalTime();
            order.DateOfOrder = dateOfOrdering;
            DateTime dateOfReturning = dateOfOrdering.AddMonths(1);
            order.DateOfReturn = dateOfReturning;
            ServerAdapter.Instance.CreateOrder(order);
        }
    }
}
