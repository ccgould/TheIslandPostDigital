using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using TheIslandPostManager.Models;
using System.Security.Cryptography;
using System.Globalization;
using static TheIslandPostManager.ViewModels.EarningsPageViewmodel;

namespace TheIslandPostManager.Services;

public class MySQLService: IMySQLService
{
    private const string MYSQL_DATE_FORMAT = "yyyy-MM-dd";
    private const string MYSQL_TIME_FORMAT = "HH:mm:ss";
    private readonly IFileService fileService;
    private readonly IMessageService messageService;
    private string connectionString;
    private AppSettings? settings => App.AppConfig.GetSection("AppSettings") as AppSettings;

    public MySQLService(IFileService fileService,IMessageService messageService)
    {
        this.fileService = fileService;
        this.messageService = messageService;
        SetConnectionString();
    }

    public void SetConnectionString()
    {
        connectionString = $"datasource={settings.MysqlDatasource};port={settings.MysqlPortNumber};username={settings.MySqlUsername};password={settings.MysqlPassword};database={settings.MysqlDatabaseName};";
    }

    public async Task AddPendingOrder(Order order)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_pendingorders` (`orderID`, `thumbnail`,`name`, `date`, `pendingPath`) VALUES (@orderID, @thumbnail,@name, @date, @pendingPath)", connection);
            command.Parameters.AddWithValue("@orderID", order.CustomerID);
            command.Parameters.AddWithValue("@thumbnail", order.CurrentImages.FirstOrDefault().ImageUrl);
            command.Parameters.AddWithValue("@name", order.Name);
            command.Parameters.AddWithValue("@date", order.Date.ToString(MYSQL_DATE_FORMAT));
            command.Parameters.AddWithValue("@pendingPath", order.DownloadURL);
            command.ExecuteNonQuery();

            var orderID = command.LastInsertedId;


            if (order.CurrentImages?.Any() ?? false)
            {
                foreach (var image in order.CurrentImages)
                {
                    MySqlCommand command1 = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_pendingimages` (`pendingOrderID`,`orderID`, `name`, `imageURL`, `isSelected`, `isPending`, `isPrinting`,`printAmount`) VALUES (@pendingOrderID,@orderID, @name, @imageURL, @isSelected, @isPending, @isPrinting,@printAmount)", connection);
                    command1.Parameters.AddWithValue("@pendingOrderID", orderID);
                    command1.Parameters.AddWithValue("@orderID", order.CustomerID);
                    command1.Parameters.AddWithValue("@name", image.Name);
                    command1.Parameters.AddWithValue("@imageURL", image.ImageUrl.Replace("\\", "\\\\"));
                    command1.Parameters.AddWithValue("@isSelected", image.IsSelected);
                    command1.Parameters.AddWithValue("@isPending", image.IsPending);
                    command1.Parameters.AddWithValue("@isPrinting", image.IsPrintable);
                    command1.Parameters.AddWithValue("@printAmount", image.PrintAmount);

                    command1.ExecuteNonQuery();
                }
            }
            messageService.ShowSnackBarMessage("Pending", "Pending Order Saved Successfully");
        }
        catch(Exception ex)
        {
           await messageService.ShowErrorMessage("Error Pending Order", ex.Message, ex.StackTrace, "9ccf1439-e690-439c-b701-22933c5d051c", true);
        }
        finally
        {
            connection?.Close();
        }

    }

    private async Task<MySqlConnection> OpenConnection()
    {
        MySqlConnection? connection = null;
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();

        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Mysql", "Failed to connect to mysql service. Please ensure login information is correct and servide is running. " + ex.Message);
            connection = null;
        }

        return connection;
    }

    public async Task RemovePendingOrder(Order pendingOrderID)
    {
        MySqlConnection connection = await OpenConnection();
        try
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM ipd_pendingorders WHERE id=@id", connection);
            command.Parameters.AddWithValue("@id", pendingOrderID.ID);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
           await messageService.ShowErrorMessage("Error Removing Pending Order", ex.Message, ex.StackTrace, "28b89937-71d9-4fd9-9543-e8a77869cff1", true);
        }
        finally
        {
            connection?.Close();
        }
    }

    public async Task<ObservableCollection<Order>> GetPendingOrders()
    {
         MySqlConnection connection = await OpenConnection();

        if (connection is null) return null;
        try
        {
            ObservableCollection<Order> returnThese = new ObservableCollection<Order>();

            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_pendingOrders", connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    await Task.Run(() =>
                    {
                        Order slip = new Order(0)
                        {
                            ID = reader.GetInt32("id"),
                            CustomerID = reader.GetInt32("orderID"),
                            Thumbnail = new ImageObj(reader.GetString("thumbnail")).LowImage,
                            Name = reader.GetString("name"),
                            DownloadURL = reader.GetString("pendingPath"),
                            Date = DateOnly.FromDateTime(reader.GetDateTime("date"))
                        };

                        returnThese.Add(slip);
                    });
                }
            }

            connection.Close();

            return returnThese;
        }
        catch(Exception ex)
        {
            await messageService.ShowErrorMessage("Error Retrieving Pending Order", ex.Message, ex.StackTrace, "524270c5-ec9d-4d65-ab2e-bf39d771484d");
        }
        finally
        {
            connection.Close();
        }
        return new();
    }

    public async Task<ObservableCollection<PurchaseItem>> GetProducts(bool retailOnly = false)
    {
        MySqlConnection connection = await OpenConnection();


        try
        {
            ObservableCollection<PurchaseItem> returnThese = new ObservableCollection<PurchaseItem>();

            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_products WHERE parent = -1", connection);
            command.Parameters.AddWithValue("@isRetail", retailOnly);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    var item = new PurchaseItem(reader.GetInt32("product_id"),
                        reader.GetString("description"),
                        reader.GetString("data"),
                        reader.GetDecimal("price"),
                        reader.GetInt32("imageCount"),
                        reader.GetInt32("printCount"),
                        reader.GetBoolean("isRetail"));
                        
                    returnThese.Add(item);
                }
            }

            foreach (var item in returnThese)
            {
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM ipd_products WHERE parent = @id", connection);
                command1.Parameters.AddWithValue("@id", item.ID);

                using (MySqlDataReader reader1 = command1.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        var child = new PurchaseItem(reader1.GetInt32("product_id"),
                        reader1.GetString("description"),
                        reader1.GetString("data"),
                        reader1.GetDecimal("price"),
                        reader1.GetInt32("imageCount"),
                        reader1.GetInt32("printCount"),
                        reader1.GetBoolean("isRetail"),
                        reader1.GetInt32("parent"));

                        item.ChildrenItems.Add(child);
                    }
                }
            }

            

            return returnThese;
        }
        catch(Exception ex)
        {
            await messageService.ShowErrorMessage("Error Getting Store Items",ex.Message, ex.StackTrace, "4e099ca3-a2b7-4bdb-b332-128b0760aea8");
        }
        finally
        {
            if(connection is not null)
            {
                await connection.CloseAsync();

            }
        }

        return new();
    }
    
    public async Task<ObservableCollection<Order>> GetPurchaseHistory(DateTime startTime, DateTime endTime,string searchText,string clerkName)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            ObservableCollection<Order> returnThese = new ObservableCollection<Order>();

            if (connection is null) return new();

            var stime = startTime.ToString("yyyy-MM-dd");
            var edTime = endTime.ToString("yyyy-MM-dd");

            string orderID = $"%{searchText?.Trim()}%";
       
            //MySqlCommand command = new MySqlCommand("SELECT * FROM  ipd_invoices WHERE (date >= @startDate AND date <= @endDate) AND name LIKE @searchText OR email LIKE @searchText ORDER BY date ASC;", connection);
            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_invoices WHERE (invoice_id LIKE @searchText) AND (date >= @startDate AND date <= @endDate);", connection);

            command.Parameters.AddWithValue("@startDate", stime);
            command.Parameters.AddWithValue("@endDate", edTime);
            command.Parameters.AddWithValue("@searchText", orderID);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var customer = await FindCustomerById(reader.GetInt32("customer_id"));
                    var id = reader.GetInt32("invoice_id");
                    Order slip = new Order(0)
                    {
                        ID = id,
                        EmployeeID = reader.GetInt32("employee_id"),
                        CustomerID = reader.GetInt32("customer_id"),
                        Date = DateOnly.FromDateTime(reader.GetDateTime("date")),
                        Time = TimeOnly.FromTimeSpan(await reader.GetFieldValueAsync<TimeSpan>("time")),
                        Name = $"Order {reader.GetInt32("invoice_id")}",
                        DownloadURL = reader.IsDBNull("downloadURL") ? string.Empty : reader.GetString("downloadURL"),
                        Email = customer.Email,
                        ApprovedImagesCount = await GetApprovedImageCount(id),
                        ApprovedPrintsCount = await GetApprovedImagePrintCount(id),
                        /*
                       Name = reader.GetString("name"),

                       VideoCount = reader.GetInt32("videoCount"),
                       OrderPath = reader.IsDBNull("path") ? string.Empty : reader.GetString("path"),*/
                        IsFinalized = reader.GetBoolean("hasEmailBeenSent"),
                        RetailCount = await GetRetailCount(reader.GetInt32("invoice_id")),
                        Employee = await GetEmployee(reader) ?? new(),
                        CartTotal = reader.GetDecimal("total_amount")

                    };

                    //command.Parameters.AddWithValue("@employee_id", order.EmployeeID);
                    //command.Parameters.AddWithValue("@photoCount", order.ApprovedImagesCount);
                    //command.Parameters.AddWithValue("@videoCount", 0);
                    //command.Parameters.AddWithValue("@printCount", order.ApprovedPrintsCount);
                    //command.Parameters.AddWithValue("@email", order.Email);
                    //command.Parameters.AddWithValue("@hasEmailBeenSent", order.IsFinalized);
                    //command.Parameters.AddWithValue("@path", order.DownloadURL);

                    returnThese.Add(slip);
                }
            }

            //await LoadOrderPurchaseHistory(returnThese);

            await LoadOrderImages(returnThese);

            return returnThese;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Getting Purchase History", ex.Message,ex.StackTrace,"");
        }
        finally
        {
            connection?.Close();
        }

        return new();
    }

    private async Task<Employee?> GetEmployee(System.Data.Common.DbDataReader reader)
    {
        var result =  await GetEmployees();
        if(result != null)
        {
          return  result.FirstOrDefault(x => x.EmployeeID == reader.GetInt32("employee_id"));
        }
        return null;
    }

    private async Task<int> GetApprovedImagePrintCount(int id)
    {
        MySqlConnection connection = await OpenConnection();

        if (connection is null) return 0;

        try
        {
            MySqlCommand command = new MySqlCommand("SELECT SUM(printAmount) AS total FROM ipd_orderimages where orderID = @id", connection);

            command.Parameters.AddWithValue("@id", id);

            var amount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    amount = reader.GetInt32("total");
                }
            }

            connection.Close();
            return amount;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Retrieving Pending Order", ex.Message, ex.StackTrace, "524270c5-ec9d-4d65-ab2e-bf39d771484d");
        }
        finally
        {
            connection.Close();
        }

        return 0;
    }

    private async Task<int> GetApprovedImageCount(int id)
    {
        MySqlConnection connection = await OpenConnection();

        if (connection is null) return 0;

        try
        {
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) AS total FROM ipd_orderimages where orderID = @id", connection);

            command.Parameters.AddWithValue("@id", id);

            var amount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    amount = reader.GetInt32("total");
                }
            }

            connection.Close();
            return amount;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Retrieving Pending Order", ex.Message, ex.StackTrace, "524270c5-ec9d-4d65-ab2e-bf39d771484d");
        }
        finally
        {
            connection.Close();
        }

        return 0;
    }

    private async Task<Customer> FindCustomerById(int id)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            ObservableCollection<Order> returnThese = new ObservableCollection<Order>();

            if (connection is null) return new();

            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_customers WHERE id = @id;", connection);

            command.Parameters.AddWithValue("@id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Customer customer = new Customer()
                    {
                        Id = reader.GetInt32("id"),
                        ReturnedCount = reader.GetInt32("returned_Count"),
                        Joined_date = DateOnly.FromDateTime(reader.GetDateTime("joined_date")),
                        Birthday = DateOnly.FromDateTime(reader.GetDateTime("birthday")),
                        Joined_time = TimeOnly.FromTimeSpan(await reader.GetFieldValueAsync<TimeSpan>("joined_time")),
                        FullName = reader.GetString("full_name"),                        
                        Email = reader.GetString("email"),
                        Is_member = reader.GetBoolean("is_member"),
                    };

                    return customer;
                }
            }
        }   
        catch(Exception ex)
        {

        }
        return null;
    }

    private async Task<int> GetRetailCount(int orderID)
    {
        MySqlConnection connection = await OpenConnection();

        int amount = 0;


        try
        {
            if (connection is null) return new();


            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_retailpurchases WHERE orderID = @orderID", connection);

            command.Parameters.AddWithValue("@orderID", orderID);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    amount += reader.GetInt32("customer");
                }

                return amount;
            }

        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Getting Retail count", ex.Message, ex.StackTrace, "");
        }
        finally
        {
            connection?.Close();
        }

        return 0;
    }

    public async Task<Tuple<ObservableCollection<ImageObj>, List<IImage>, List<IImage>>?> LoadImages(int customer_id)
    {
        MySqlConnection connection = await OpenConnection();
        var currentImages = new ObservableCollection<ImageObj>();
        var approvedImages = new List<IImage>();
        var approvedPrints = new List<IImage>();

        try
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_pendingimages WHERE userID=@userID", connection);
            command.Parameters.AddWithValue("@userID", customer_id);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var imagePath = reader.GetString("imageURL").Replace("\\\\", "\\");
                    if (File.Exists(imagePath))
                    {
                        var image = new ImageObj(imagePath);
                        image.IsPending = reader.GetBoolean("isPending");
                        image.IsPrintable = reader.GetBoolean("isPrinting");
                        image.IsSelected = reader.GetBoolean("isSelected");


                        currentImages.Add(image);

                        if (image.IsSelected)
                        {
                            approvedImages.Add(image);
                        }

                        if (image.IsPrintable)
                        {
                            approvedPrints.Add(image);
                        }
                    }
                }

                return new ( currentImages, approvedImages,approvedPrints );
            }
        }
        catch(Exception ex)
        {
            await messageService.ShowErrorMessage("Error Loading Images", ex.Message, ex.StackTrace, "1efb9bcb-caa2-4627-b719-52bcf24ff823");
            return null;
        }
        finally
        {
            connection?.Close();
        }
    }

    private async Task LoadOrderPurchaseHistory(ObservableCollection<Order> orders)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            foreach (var order in orders)
            {

                MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_orderimages WHERE orderID=@orderID", connection);
                command.Parameters.AddWithValue("@orderID", order.CustomerID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var purchaseH = new PurchaseHistoryItem();
                        purchaseH.PrintCount = reader.GetInt32("printAmount");
                        //purchaseH.PhotoAccount = reader.GetInt32("photoCount");
                        //purchaseH.Description = "Photo";
                        purchaseH.ImageLocation = reader.GetString("imageURL").Replace("\\\\", "\\");
                        order.PurchaseHistoryItems.Add(purchaseH);
                    }
                }

                //TODO Reenable
                //MySqlCommand command2 = new MySqlCommand("SELECT * FROM ipd_retailPurchases WHERE customer_id=@userID", connection);
                //command2.Parameters.AddWithValue("@userID", order.customer_id);

                //using (var reader = await command2.ExecuteReaderAsync())
                //{
                //    while (await reader.ReadAsync())
                //    {
                //        var purchaseH = new PurchaseHistoryItem();
                //        purchaseH.RetailCount = reader.GetInt32("amount");
                //        purchaseH.Description = reader.GetString("description");
                //        order.PurchaseHistoryItems.Add(purchaseH);
                //    }
                //}
;            }
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Order History Items", ex.Message, ex.StackTrace, "");
        }
        finally
        {
            connection?.Close();
        }
    }

    private async Task LoadOrderImages(ObservableCollection<Order> orders)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            foreach (var order in orders)
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_orderimages WHERE orderID=@orderID", connection);
                command.Parameters.AddWithValue("@orderID", order.ID);
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        await Task.Run(() =>
                        {
                            var imagePath = reader.GetString("imageURL").Replace("\\\\", "\\");

                            var image = new ImageObj(imagePath, false);

                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                order.CurrentImages.Add(image);
                            }));
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Loading Orders", ex.Message, ex.StackTrace, "165f16c2-3877-4041-a03a-1f70b74b8c90");
        }
        finally
        { 
            connection?.Close();
        }
    }

    public async Task<long> AddCompletedOrder(Order order)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_invoices` (`customer_id`,`employee_id`,`hasEmailBeenSent`,`date`,`time`,`downloadURL`,`total_amount`,`total_card`,`total_cash`,`total_bank`) VALUES (@customer_id,@employee_id,@hasEmailBeenSent,@date,@time,@downloadURL,@total_amount,@total_card,@total_cash,@total_bank)", connection);
            command.Parameters.AddWithValue("@customer_id", order.CustomerID);
            command.Parameters.AddWithValue("@employee_id", 1);
            command.Parameters.AddWithValue("@hasEmailBeenSent", order.IsFinalized);
            command.Parameters.AddWithValue("@date", order.Date.ToString(MYSQL_DATE_FORMAT));
            command.Parameters.AddWithValue("@time", order.Date.ToString(MYSQL_TIME_FORMAT));
            command.Parameters.AddWithValue("@downloadURL", order.DownloadURL);
            command.Parameters.AddWithValue("@total_amount", order.CartTotal);
            command.Parameters.AddWithValue("@total_card", order.PaymentTransactions.Where(x =>x.PurchaseType == PurchaseType.Card).Sum(x=>x.Amount));
            command.Parameters.AddWithValue("@total_cash", order.PaymentTransactions.Where(x => x.PurchaseType == PurchaseType.Cash).Sum(x => x.Amount));
            command.Parameters.AddWithValue("@total_bank", order.PaymentTransactions.Where(x => x.PurchaseType == PurchaseType.Bank).Sum(x => x.Amount));

            command.ExecuteNonQuery();


            var orderID = command.LastInsertedId;

            if (order.ApprovedImages?.Any() ?? false)
            {
                foreach (var image in order.ApprovedImages)
                {
                    MySqlCommand command1 = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_orderimages` (`orderID`, `name`, `imageURL`,`printAmount`) VALUES (@orderID, @name, @imageURL, @printAmount)", connection);
                    command1.Parameters.AddWithValue("@orderID", orderID);
                    command1.Parameters.AddWithValue("@name", image.Name);
                    command1.Parameters.AddWithValue("@imageURL", image.ImageUrl.Replace("\\", "\\\\"));
                    command1.Parameters.AddWithValue("@printAmount", image.PrintAmount);
                    command1.ExecuteNonQuery();
                }
            }

            foreach (var cartItem in order.Cart)
            {
                MySqlCommand command2 = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_sales` (`invoice_id`, `product_id`,`quantity`,`unit_price`,`sub_total`) VALUES (@invoice_id, @product_id,@quantity,@unit_price,@sub_total)", connection);
                command2.Parameters.AddWithValue("@invoice_id", orderID);
                command2.Parameters.AddWithValue("@product_id", cartItem.ID);
                command2.Parameters.AddWithValue("@quantity", cartItem.Amount);
                command2.Parameters.AddWithValue("@unit_price", cartItem.Cost);
                command2.Parameters.AddWithValue("@sub_total", cartItem.TotalCost);
                command2.ExecuteNonQuery();
            }

            foreach (var payment in order.PaymentTransactions)
            {
                MySqlCommand command2 = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_transactions` (`invoice_id`, `purchase_type`,`date`,`time`) VALUES (@invoice_id, @purchase_type,@date,@time)", connection);
                command2.Parameters.AddWithValue("@invoice_id", orderID);
                command2.Parameters.AddWithValue("@purchase_type", (int)payment.PurchaseType);
                command2.Parameters.AddWithValue("@date", payment.Date.ToString(MYSQL_DATE_FORMAT));
                command2.Parameters.AddWithValue("@time", payment.Time.ToString(MYSQL_TIME_FORMAT));
                command2.ExecuteNonQuery();
            }

            /* Removing due to ipd_sales 
            foreach (var cartItem in order.Cart.Where(x => x.IsRetailItem))
            {
                MySqlCommand command1 = new MySqlCommand("INSERT INTO `ipd_db`.`ipd_retailpurchases` (`orderID`, `itemID`,`amount`) VALUES (@orderID, @itemID,@amount)", connection);
                command1.Parameters.AddWithValue("@orderID", orderID);
                command1.Parameters.AddWithValue("@itemID", cartItem.ID);
                command1.Parameters.AddWithValue("@amount", cartItem.Amount);
                command1.ExecuteNonQuery();
            }
            */

            messageService.ShowSnackBarMessage("Order", "Order Saved Successfully");


            // Get the auto-increment ID
            return command.LastInsertedId;
        }
        catch(Exception ex)
        {
            await messageService.ShowErrorMessage("Error Adding Completed Order", ex.Message, ex.StackTrace, "9a5840d3-9840-4cf9-a906-a8465f1f71f5");
            return -1;
        }
        finally
        {
            connection?.Close();
        }        
    }

    public async Task UpdateHistoryOrder(Order order)
    {
        MySqlConnection connection = await OpenConnection();
        try
        {

            MySqlCommand command = new MySqlCommand("UPDATE ipd_invoices SET photoCount= @photoCount, videoCount = @videoCount, printCount = @printCount, customer = @customer, name = @name,hasEmailBeenSent = @hasEmailBeenSent,date = @date,downloadURL = @downloadURL WHERE userID=@userID", connection);

            command.Parameters.AddWithValue("@userID", order.CustomerID);
            command.Parameters.AddWithValue("@photoCount", order.ApprovedImagesCount);
            command.Parameters.AddWithValue("@videoCount", 0);
            command.Parameters.AddWithValue("@printCount", order.ApprovedPrintsCount);
            command.Parameters.AddWithValue("@customer", order.Email);
            command.Parameters.AddWithValue("@name", order.Name);
            command.Parameters.AddWithValue("@hasEmailBeenSent", order.IsFinalized);
            command.Parameters.AddWithValue("@date", order.Date.ToString(MYSQL_DATE_FORMAT));
            command.Parameters.AddWithValue("@downloadURL", order.DownloadURL);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Updating Order History", ex.Message, ex.StackTrace, "14249d65-27af-4230-9cff-7eb215fe62f4");
        }
        finally
        {
            connection?.Close();
        }
    }

    public async Task<ObservableCollection<Employee>> GetEmployees(bool onlyAdmins = false)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            ObservableCollection<Employee> returnThese = new();
            MySqlCommand command;

            if (onlyAdmins)
            {
                command = new MySqlCommand("SELECT * FROM ipd_employees WHERE isTerminated = 0 AND isAdmin = true ORDER BY firstName ASC", connection);
            }
            else
            {
                command = new MySqlCommand("SELECT * FROM ipd_employees WHERE isTerminated = 0 ORDER BY firstName ASC", connection);
            }

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Employee employee = new Employee()
                    {
                        EmployeeID = reader.GetInt32("employee_ID"),
                        FirstName = reader.GetString("firstName"),
                        LastName = reader.GetString("lastName"),
                        IsTerminated = reader.GetBoolean("isTerminated"),   
                    };

                    returnThese.Add(employee);
                }
            }

            connection.Close();

            return returnThese;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Getting Employees",ex.Message,ex.StackTrace, "9dd1c50f-52c8-4d20-a6b6-91adf9d79213");
        }
        finally
        {
            connection?.Close();
        }

        return new();
    }

    public async Task<bool> ValidatePin(int id,string pin)
    {
        MySqlConnection connection = await OpenConnection();
        bool result = false;
        try
        {
            ObservableCollection<Employee> returnThese = new();

            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_employees WHERE employee_id = @employee_id", connection);
            command.Parameters.AddWithValue("@employee_id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    Employee employee = new Employee()
                    {
                        FirstName = reader.GetString("firstName"),
                        //Pin = reader.GetString("pin"),
                        Salt = reader.GetString("salt"),
                    };

                    result = SHA1(SHA1(pin + employee.Salt)).Equals(employee.Pin);
                }
            }

            connection.Close();

            return result;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Getting Employees", ex.Message, ex.StackTrace, "9dd1c50f-52c8-4d20-a6b6-91adf9d79213");
        }
        finally
        {
            connection?.Close();
        }
        return false;
    }

    private string SHA1(string input)
    {
        byte[] hash;
        using (var sha1 = new SHA1CryptoServiceProvider())
        {
            hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
        }

        var sb = new StringBuilder();
        foreach (byte b in hash)
        {
            sb.AppendFormat("{0:x2}", b);
        }

        return sb.ToString();
    }

    public async Task<double> GetTodaysTotal(DateTime date, TransactionType transactionType)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            double amount = 0.0;

            MySqlCommand command = null;

            switch (transactionType)
            {
                case TransactionType.Total:
                    command = new MySqlCommand("SELECT SUM(saleTotal) AS TotalItemsOrdered FROM ipd_invoices WHERE DATE = @date", connection);
                    break;
                case TransactionType.Cash:
                case TransactionType.Card:
                case TransactionType.Both:
                    command = new MySqlCommand("SELECT SUM(saleTotal) AS TotalItemsOrdered FROM ipd_invoices WHERE DATE = @date AND cashType = @cashType", connection);
                    break;
            }

            if (command is null)
            {
                await messageService.ShowErrorMessage("Error",$"Command returned null when trying to get the transaction total {transactionType}");
                return amount;
            }

            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@cashType", transactionType.ToString());

            command.ExecuteNonQuery();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    amount = reader.IsDBNull("TotalItemsOrdered") ? 0.0 : reader.GetDouble("TotalItemsOrdered");
                }
            }

            connection.Close();

            return amount;
        }
        catch (Exception ex)
        {
           await messageService.ShowErrorMessage("Error Getting Employees", ex.Message, ex.StackTrace, "9dd1c50f-52c8-4d20-a6b6-91adf9d79213");
            return 0.0;
        }
        finally
        {
            connection?.Close();
        }
    }

    public async Task<ObservableCollection<Customer>> SearchByEmail(string email)
    {
        MySqlConnection connection = await OpenConnection();

        try
        {
            var customers = new ObservableCollection<Customer>();

            MySqlCommand command = new MySqlCommand("SELECT * FROM ipd_customers WHERE email  like @email", connection);

            command.Parameters.AddWithValue("@email", $"%{email}%");
            command.ExecuteNonQuery();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var customer = new Customer()
                    {
                        Id = reader.IsDBNull("id") ? -1 : reader.GetInt32("id"),
                        Email = reader.IsDBNull("email") ? string.Empty : reader.GetString("email"),
                        FullName = reader.IsDBNull("full_name") ? string.Empty : reader.GetString("full_name"),
                        Joined_date = reader.IsDBNull("joined_date") ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.FromDateTime(reader.GetDateTime("joined_date")),
                        Joined_time = reader.IsDBNull("joined_time") ? TimeOnly.FromDateTime(DateTime.Now) : TimeOnly.FromTimeSpan(reader.GetTimeSpan("joined_time")),
                        ReturnedCount = reader.IsDBNull("id") ? -1 : reader.GetInt32("returned_Count"),
                        Birthday = reader.IsDBNull("birthday") ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.FromDateTime(reader.GetDateTime("birthday")),
                        Is_member = reader.IsDBNull("is_member") ? false : reader.GetBoolean("is_member"),
                    };
                    
                    customers.Add(customer);
                }
            }

            connection.Close();
            return customers;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage(
                "Error Searching By Email", 
                ex.Message, 
                ex.StackTrace, 
                "9dd1c50f-52c8-4d20-a6b6-91adf9d79213");
            return null;
        }
        finally
        {
            connection?.Close();
        }
    }

    public async Task<bool> DeleteOrderByID(int id)
    {
        MySqlConnection connection = await OpenConnection();
        try
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM ipd_invoices WHERE invoice_id=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            await messageService.ShowErrorMessage("Error Removing Pending Order", ex.Message, ex.StackTrace, "28b89937-71d9-4fd9-9543-e8a77869cff1", true);
        }
        finally
        {
            connection?.Close();
        }
        return false;
    }
}

public interface IMySQLService
{
    Task<long> AddCompletedOrder(Order currentOrder);
    Task AddPendingOrder(Order order);
    Task<ObservableCollection<Employee>> GetEmployees(bool getAdminOnly = false);
    Task<ObservableCollection<Order>> GetPendingOrders();
    Task<ObservableCollection<Order>> GetPurchaseHistory(DateTime startTime, DateTime endTime, string searchText, string clerkName);
    Task<ObservableCollection<PurchaseItem>> GetProducts(bool retailOnly = false);
    Task<double> GetTodaysTotal(DateTime date, ViewModels.EarningsPageViewmodel.TransactionType transactionType);
    Task<Tuple<ObservableCollection<ImageObj>, List<IImage>, List<IImage>>?> LoadImages(int id);
    Task RemovePendingOrder(Order order);
    void SetConnectionString();
    Task UpdateHistoryOrder(Order order);
    Task<bool> ValidatePin(int employeeID,string link);
    Task<ObservableCollection<Customer>> SearchByEmail(string text);
    Task<bool> DeleteOrderByID(int id);
}
