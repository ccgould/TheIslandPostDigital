using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.IO;
using TheIslandPostManager.Models;

namespace TheIslandPostManager.Services;
internal class MySQLService: IMySQLService
{
    private const string MYSQL_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    private readonly IFileService fileService;
    private readonly IMessageService messageService;
    private readonly string connectionString;

    public MySQLService(IFileService fileService,IMessageService messageService)
    {
        this.fileService = fileService;
        this.messageService = messageService;
        connectionString = "datasource=localhost;port=3306;username=root;password=www##2846;database=islandpost;";

    }

    public void AddPendingOrder(Order order)
    {
        MySqlConnection connection = OpenConnection();


        MySqlCommand command = new MySqlCommand("INSERT INTO `islandpost`.`ipd_pendingorders` (`userID`, `thumbnail`,`name`, `date`, `pendingPath` , `photoCount`, `videoCount`, `selectedCount`, `printingCount`) VALUES (@userID, @thumbnail,@name, @date, @pendingPath,@photoCount, @videoCount, @selectedCount, @printingCount)", connection);
        command.Parameters.AddWithValue("@userID", order.CustomerID);
        command.Parameters.AddWithValue("@thumbnail", order.CurrentImages.FirstOrDefault().ImageUrl);
        command.Parameters.AddWithValue("@name", order.Name);
        command.Parameters.AddWithValue("@date", order.Date.ToString(MYSQL_DATE_FORMAT));
        command.Parameters.AddWithValue("@pendingPath", order.DownloadURL);
        command.Parameters.AddWithValue("@photoCount", order.CurrentImages.Count);
        command.Parameters.AddWithValue("@videoCount", 0);
        command.Parameters.AddWithValue("@selectedCount", order.ApprovedImagesCount);
        command.Parameters.AddWithValue("@printingCount", order.ApprovedPrintsCount);
        command.ExecuteNonQuery();


        if(order.ApprovedImages?.Any() ?? false)
        {
            foreach (var image in order.CurrentImages)
            {
                MySqlCommand command1 = new MySqlCommand("INSERT INTO `islandpost`.`ipd_pendingimages` (`userID`, `name`, `imageURL`, `isSelected`, `isPending`, `isPrinting`, `height`, `width`) VALUES (@userID, @name, @imageURL, @isSelected, @isPending, @isPrinting, @height, @width)", connection);
                command1.Parameters.AddWithValue("@userID", order.CustomerID);
                command1.Parameters.AddWithValue("@name", image.Name);
                command1.Parameters.AddWithValue("@imageURL", image.ImageUrl.Replace("\\", "\\\\"));
                command1.Parameters.AddWithValue("@isSelected", image.IsSelected);
                command1.Parameters.AddWithValue("@isPending", image.IsPending);
                command1.Parameters.AddWithValue("@isPrinting", image.IsPrintable);
                command1.Parameters.AddWithValue("@height", 0);
                command1.Parameters.AddWithValue("@width", 0);
                command1.ExecuteNonQuery();
            }
        }
        messageService.ShowSnackBarMessage("Pending", "Pending Order Saved Successfully");
        connection.Close();
    }

    private MySqlConnection OpenConnection()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    public void RemovePendingOrder(Order pendingOrderID)
    {
        MySqlConnection connection = OpenConnection();
        MySqlCommand command = new MySqlCommand("DELETE FROM ipd_pendingorders WHERE userID=@userID", connection);
        command.Parameters.AddWithValue("@userID", pendingOrderID.CustomerID);
        command.ExecuteNonQuery();
        connection.Close();
    }

    public ObservableCollection<Order> GetPendingOrders()
    {
        ObservableCollection<Order> returnThese = new ObservableCollection<Order>();

        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_pendingOrders", connection);



        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Order slip = new Order(0)
                {
                    CustomerID = reader.GetString("userID"),
                    Thumbnail = new ImageObj(reader.GetString("thumbnail"))?.LowImage,
                    Name = reader.GetString("name"),
                    DownloadURL = reader.GetString("pendingPath"),
                    Date = reader.GetDateTime("date")
                };

                returnThese.Add(slip);
            }
        }

        connection.Close();

        LoadImages(returnThese);

        return returnThese;
    }

    public ObservableCollection<Order> GetPurchaseHistory(DateTime startTime, DateTime endTime,string searchText,string clerkName)
    {
        ObservableCollection<Order> returnThese = new ObservableCollection<Order>();
        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        var stime = startTime.ToString("yyyy-MM-dd");
        var edTime = endTime.ToString("yyyy-MM-dd");
        var f = $"{searchText?.Trim()}%";

        MySqlCommand command = new MySqlCommand("SELECT * FROM  ipd_orders WHERE (date >= @startDate AND date <= @endDate) AND name LIKE @searchText OR email LIKE @searchText ORDER BY date ASC;", connection);
        command.Parameters.AddWithValue("@startDate", stime);
        command.Parameters.AddWithValue("@endDate", edTime);
        command.Parameters.AddWithValue("@searchText", f);
        command.Parameters.AddWithValue("@employeeID", clerkName);

        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Order slip = new Order(0)
                {
                    CustomerID = reader.GetString("userID"),
                    Name = reader.GetString("name"),
                    Date = reader.GetDateTime("date"),
                    ApprovedImagesCount = reader.GetInt32("photoCount"),
                    ApprovedPrintsCount = reader.GetInt32("printCount"),
                };

                //command.Parameters.AddWithValue("@employeeID", order.EmployeeID);
                //command.Parameters.AddWithValue("@photoCount", order.ApprovedImagesCount);
                //command.Parameters.AddWithValue("@videoCount", 0);
                //command.Parameters.AddWithValue("@printCount", order.ApprovedPrintsCount);
                //command.Parameters.AddWithValue("@email", order.Email);
                //command.Parameters.AddWithValue("@hasEmailBeenSent", order.IsFinalized);
                //command.Parameters.AddWithValue("@path", order.DownloadURL);

                returnThese.Add(slip);
            }
        }

        return returnThese;
    }

    private void LoadImages(ObservableCollection<Order> orders)
    {
        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        foreach (var order in orders)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_pendingimages WHERE userID=@userID", connection);
            command.Parameters.AddWithValue("@userID", order.CustomerID);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var imagePath = reader.GetString("imageURL");
                    if(File.Exists(imagePath))
                    {
                        var image = new ImageObj(imagePath);
                        image.IsPending = reader.GetBoolean("isPending");
                        image.IsPrintable = reader.GetBoolean("isPrinting");
                        image.IsSelected = reader.GetBoolean("isSelected");
                        order.CurrentImages.Add(image);

                        if(image.IsSelected)
                        {
                            order.ApprovedImages.Add(image);
                        }

                        if(image.IsPrintable)
                        {
                            order.ApprovedPrints.Add(image);
                        }


                    }
                }
            }
        }

        connection.Close();
    }

    public void AddCompletedOrder(Order order)
    {
        MySqlConnection connection = OpenConnection();


        MySqlCommand command = new MySqlCommand("INSERT INTO `islandpost`.`ipd_orders` (`userID`,`employeeID`,`photoCount`,`videoCount`,`printCount`,`email`,`name`,`hasEmailBeenSent`,`date`,`path`) VALUES (@userID,@employeeID, @photoCount, @videoCount, @printCount, @email, @name,@hasEmailBeenSent,@date,@path)", connection);
        command.Parameters.AddWithValue("@userID", order.CustomerID);
        command.Parameters.AddWithValue("@employeeID", order.EmployeeID);
        command.Parameters.AddWithValue("@photoCount", order.ApprovedImagesCount);
        command.Parameters.AddWithValue("@videoCount", 0);
        command.Parameters.AddWithValue("@printCount", order.ApprovedPrintsCount);
        command.Parameters.AddWithValue("@email", order.Email);
        command.Parameters.AddWithValue("@name", order.Name);
        command.Parameters.AddWithValue("@hasEmailBeenSent", order.IsFinalized);
        command.Parameters.AddWithValue("@date", order.Date.ToString(MYSQL_DATE_FORMAT));
        command.Parameters.AddWithValue("@path", order.DownloadURL);
        command.ExecuteNonQuery();


        if (order.ApprovedImages?.Any() ?? false)
        {
            foreach (var image in order.ApprovedImages)
            {
                MySqlCommand command1 = new MySqlCommand("INSERT INTO `islandpost`.`ipd_orderimages` (`userID`, `name`, `imageURL`, `isSelected`, `isPending`, `isPrinting`, `height`, `width`) VALUES (@userID, @name, @imageURL, @isSelected, @isPending, @isPrinting, @height, @width)", connection);
                command1.Parameters.AddWithValue("@userID", order.CustomerID);
                command1.Parameters.AddWithValue("@name", image.Name);
                command1.Parameters.AddWithValue("@imageURL", image.ImageUrl.Replace("\\", "\\\\"));
                command1.Parameters.AddWithValue("@isSelected", image.IsSelected);
                command1.Parameters.AddWithValue("@isPending", image.IsPending);
                command1.Parameters.AddWithValue("@isPrinting", image.IsPrintable);
                command1.Parameters.AddWithValue("@height", 0);
                command1.Parameters.AddWithValue("@width", 0);
                command1.ExecuteNonQuery();
            }
        }
        messageService.ShowSnackBarMessage("Order", "Order Saved Successfully");
        connection.Close();
    }

    public ObservableCollection<Employee> GetEmployees()
    {
        ObservableCollection<Employee> returnThese = new();

        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_employees ORDER BY firstName ASC", connection);



        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Employee employee = new Employee()
                {
                    EmployeeID = reader.GetInt32("employeeID"),
                    FirstName = reader.GetString("firstName"),
                    LastName = reader.GetString("lastName"),
                    IsTerminated = reader.GetBoolean("isTerminated"),
                    Pin = reader.GetInt32("pin"),
                };

                returnThese.Add(employee);
            }
        }

        connection.Close();

        return returnThese;
    }
}

public interface IMySQLService
{
    void AddCompletedOrder(Order currentOrder);
    void AddPendingOrder(Order order);
    ObservableCollection<Employee> GetEmployees();
    ObservableCollection<Order> GetPendingOrders();
    ObservableCollection<Order> GetPurchaseHistory(DateTime startTime, DateTime endTime, string searchText, string clerkName);
    void RemovePendingOrder(Order order);
}