using MySql.Data.MySqlClient;
using Self_Inspection.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Self_Inspection_III.Class
{
    class Database
    {
        private string tableName;
        private Column[] columns;
        protected const string ConnectInfo = "Server=10.1.125.245;Port=3307;Database=Self_Inspection;user id=mfg;Password=chromamfg;default command timeout=1080;";

        protected string Database_Name { get => "`Self_Inspection`"; }
        protected string Table_Name { get => $"`{tableName}`"; set => tableName = Regex.Replace(value, "`", ""); }
        protected string Schema_Table { get => $"{Database_Name}.{Table_Name}"; }
        protected Column[] Columns { get => columns; set => columns = value; }
        protected string yyyyMMdd_HHmmss { get => "yyyy-MM-dd HH:mm:ss"; }
        protected bool CreateTableIfNotExists(MySqlConnection conn, string tableName, Column[] columns)
        {
            try
            {
                StringBuilder cmd = new StringBuilder($"CREATE TABLE IF NOT EXISTS {Table_Name} (");
                cmd.Append($"{columns[0].Name} {columns[0].Type} PRIMARY KEY {columns[0].Increment}");
                for (int i = 1; i < columns.Length; i++)
                    cmd.Append($", {columns[i].Name} {columns[i].Type} {columns[i].Null} {columns[1].Increment}");
                cmd.Append($")ENGINE=InnoDB DEFAULT CHARSET=utf8;");

                //Console.WriteLine(cmd.ToString());

                MySqlCommand mCmd = new MySqlCommand(cmd.ToString(), conn);
                mCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                //Email.SendErrorEmail("CreateTableIfNotExists: " + tableName, ex.Message, ex.ToString());
                return false;
            }
            return true;
        }
        public bool IfExists(string Name)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[0].Name} FROM {Database_Name}.{Table_Name} WHERE {Columns[0].Name} = '{Name}';";
                    Console.WriteLine(cmd);
                    if (string.IsNullOrEmpty((string)new MySqlCommand(cmd, Conn).ExecuteScalar()))
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                    return false;
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
        }
        public bool Delete(string Name)
        {
            Console.WriteLine($"==Delete({Name})==");
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    /*刪除*/
                    string schema_Table = $"{Database_Name}.{Table_Name}";
                    string Delete = $"DELETE FROM {schema_Table} WHERE {Columns[0].Name} LIKE '{Name}' ";
                    Console.WriteLine(Delete);
                    new MySqlCommand(Delete, Conn).ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("CreateTableIfNotExists: " + TableName, ex.Message, ex.ToString());
                    return false;
                }
                finally { Conn.Close(); }
            }
            return true;
        }
        public void ClearCache() //清除緩存
        {
            // 宣告資料表名稱
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();

                    StringBuilder clearCmd = new StringBuilder();
                    clearCmd.Append("FLUSH QUERY CACHE;");  // #清理查詢緩存內存碎片
                    clearCmd.Append("RESET QUERY CACHE;");  // #從查詢緩存中移除所有查詢
                    clearCmd.Append("FLUSH TABLES;");       // #關閉所有打開的表，同時該操作會清空查詢緩存中的內容

                    MySqlCommand ModifyCmd = new MySqlCommand(clearCmd.ToString(), Conn);
                    //執行新增
                    ModifyCmd.ExecuteNonQuery();
                    //關閉資料庫
                    Conn.Close();
                }
                catch (Exception ex) { Console.WriteLine($"[{DateTime.Now}] {ex.ToString()}"); }
            }
        }
    }

    class VersionDB : Database
    {
        public VersionDB()
        {
            Table_Name = "Version";
            Columns = new Column[4]
            {
                new Column("Number", "INT", 11, false, true),
                new Column("Version", "VARCHAR", 30, false, false),
                new Column("DateTime", "VARCHAR", 30, false, false),
                new Column("Description", "TEXT", 0, false, false)
            };
        }
        public int Read(string version, ref int ver_no) //回傳 0: 版本不變; 1: MySQL版本須更新; 2: Local版本須更新
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    if (!CreateTableIfNotExists(Conn, Table_Name, Columns)) return 0;
                    MySqlCommand com = new MySqlCommand($"SELECT Version FROM {Schema_Table} WHERE Number = (SELECT MAX(Number) FROM {Schema_Table});", Conn);
                    string[] MySQL_ver = com.ExecuteScalar().ToString().Split('.');
                    string[] Local_ver = version.Split('.');
                    int mysql_ver = 0, local_ver = 0;
                    for (int i = 0; i < 3; i++) // Build以上變更時才更新
                    {
                        mysql_ver = Convert.ToInt32(MySQL_ver[i]);
                        local_ver = Convert.ToInt32(Local_ver[i]);
                        // 版本不變
                        if (mysql_ver == local_ver) continue;
                        // MySQL版本須更新
                        else if (mysql_ver < local_ver)
                        {
                            com = new MySqlCommand($"SELECT Number FROM {Schema_Table} WHERE Number = (SELECT MAX(Number) FROM {Schema_Table});", Conn);
                            ver_no = Convert.ToInt32(com.ExecuteScalar().ToString());
                            if (i == 2) return 1;
                        }
                        else // Local版本須更新 (mysql_ver > local_ver)
                            return 2;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail($"{TableName} Read()", ex.Message, ex.ToString());
                    return 0;
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
            return 0;
        }
        public bool Write(int version_no, string version, string description)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);                    
                    MySqlCommand com = new MySqlCommand($"SELECT Number FROM {Schema_Table} WHERE Number = '{version_no}';", Conn);
                    string verNo = (string)com.ExecuteScalar();
                    if (verNo == "" || verNo == null)
                    {
                        /*新增*/
                        string AddNew = string.Format("INSERT INTO " + Schema_Table +
                            $" ({Columns[0]},{Columns[1]},{Columns[2]},{Columns[3]})" +
                            " VALUES('{0}','{1}','{2}','{3}')", version_no, version, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), description);
                        MySqlCommand AddNewCmd = new MySqlCommand(AddNew, Conn) { Connection = Conn };
                        //執行新增
                        AddNewCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail($"{TableName} Write()", ex.Message, ex.ToString());
                    return false;
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
                return true;
            }
        }
    }
    class ItemDB : Database
    {
        enum Col { ID = 0, Station, ShowName, Time_Stamp, XML, FuncCount }
        public ItemDB()
        {
            Table_Name = "Item_List";
            Columns = new Column[6]
            {
                new Column(Col.ID, "VARCHAR", 30, false, false),
                new Column(Col.Station, "VARCHAR", 30, false, false),
                new Column(Col.ShowName, "VARCHAR", 50, false, false),
                new Column(Col.Time_Stamp, "VARCHAR", 30, false, false),
                new Column(Col.XML, "TEXT", 0, false, false),
                new Column(Col.FuncCount, "INT", 11, false, false)
            };
        }

        public bool IsItemExist(string ID)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[(int)Col.ID].Name} FROM {Schema_Table} WHERE {Columns[(int)Col.ID].Name} = '{ID}';";
                    Console.WriteLine(ID);
                    return !string.IsNullOrEmpty((string)new MySqlCommand(cmd, Conn).ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //MessageBox.Show(ex.Message);
                    Email.SendErrorEmail("GetItem: " + Schema_Table, ex.Message, ex.ToString());
                    return false;
                }
                finally { Conn.Close(); } //關閉資料庫
            }
        }
        public List<string> GetItems()
        {
            Console.WriteLine("==ItemDB.GetItems()==");
            DataTable dt = new DataTable();
            List<string> ItemList = new List<string>();

            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[(int)Col.Station].Name}, {Columns[(int)Col.ShowName].Name} FROM {Schema_Table};";
                    Console.WriteLine(cmd);
                    new MySqlDataAdapter(cmd, Conn).Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                            ItemList.Add($"[{row[0]}] {row[1]}");
                        Console.WriteLine("GetItems Succeed");
                    }
                    else
                        throw new Exception("No Item found");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    Email.SendErrorEmail("GetItems: " + Schema_Table, ex.Message, ex.ToString());
                }
                finally { Conn.Close(); } //關閉資料庫
            }
            return ItemList;
        }
        public Item[] GetItems(string Station)
        {
            Console.WriteLine($"==ItemDB.GetItems({Station})==");
            List<Item> itemLst = new List<Item>();
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    string cmd = $"SELECT `{Col.ID}`, `{Col.ShowName}`, `{Col.XML}` FROM {Schema_Table} WHERE `{Col.Station}` = '{Station}';";
                    Console.WriteLine(cmd);
                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(cmd, Conn).Fill(dt);
                    Console.WriteLine($"Get Items from {Station}");
                    foreach (DataRow row in dt.Rows)
                    {
                        Console.WriteLine($"  {row[0]}: {row[1]}");
                        Item item = XmlUtil.Deserialize(typeof(Item), row[2].ToString()) as Item;
                        item.ID = row[0].ToString();
                        itemLst.Add(item);
                    }
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
                finally { Conn.Close(); }
            }
            return itemLst.ToArray();
        }
        public Item GetItem(string Station, string ShowName)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[(int)Col.XML].Name} FROM {Schema_Table} WHERE {Columns[(int)Col.ShowName].Name} = '{ShowName}' AND {Columns[(int)Col.Station].Name} = '{Station}'";
                    string itemXml = new MySqlCommand(cmd, Conn).ExecuteScalar().ToString();
                    Item item = XmlUtil.Deserialize(typeof(Item), itemXml) as Item;
                    item.ID = GetID(Station, ShowName);
                    return item;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //MessageBox.Show(ex.Message);
                    Email.SendErrorEmail("GetItem: " + Schema_Table, ex.Message, ex.ToString());
                    return null;
                }
                finally { Conn.Close(); } //關閉資料庫                
            }
        }
        public string GetID(string StationName, string ShowName)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT `{Col.ID}` FROM {Schema_Table} WHERE `{Col.ShowName}` = '{ShowName}' AND `{Col.Station}` = '{StationName}'";
                    return new MySqlCommand(cmd, Conn).ExecuteScalar().ToString();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); return string.Empty; }
                finally { Conn.Close(); } //關閉資料庫                
            }
        }
        public void ModifyID()
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    ProgramDB pdb = new ProgramDB();
                    MySqlCommand Comm = new MySqlCommand() { Connection = Conn };
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    DataSet ds = new DataSet();
                    MySqlDataAdapter DataAdpr = new MySqlDataAdapter($"SELECT `{Col.ID}`, `{Col.Station}`, `{Col.ShowName}` FROM {Schema_Table}", Conn);
                    Console.WriteLine(DataAdpr.SelectCommand.CommandText);
                    DataAdpr.Fill(ds,"Table1");
                    foreach (DataRow row in ds.Tables["Table1"].Rows)
                    {
                        string stID = pdb.GetID(row[1].ToString());
                        try
                        {
                            string newID = stID + row[0].ToString().Substring(4, 4);
                            Console.WriteLine($"{row[1]}.{row[2]}: {row[0]} -> {newID}");
                            Comm.CommandText = $"UPDATE {Schema_Table} SET `{Col.ID}` = '{newID}' WHERE `{Col.ID}` = '{row[0]}'";
                            Console.WriteLine(Comm.CommandText);
                            Comm.ExecuteNonQuery();
                        }
                        catch (MySqlException mySqlEx)
                        {
                            try
                            {
                                Console.WriteLine(mySqlEx.Message);
                                Comm.CommandText = $"UPDATE {Schema_Table} SET `{Col.ID}` = '{NewID(stID)}' WHERE `{Col.ID}` = '{row[0]}'";
                                Console.WriteLine(Comm.CommandText);
                                Comm.ExecuteNonQuery();
                            }
                            catch (Exception exp) { Console.WriteLine(exp.ToString()); }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                finally { Conn.Close(); } //關閉資料庫                
            }
        }
        public string NewID(string StationID)
        {
            using (MySqlCommand Comm = new MySqlCommand { Connection = new MySqlConnection(ConnectInfo) })
            {
                try
                {
                    Comm.Connection.Open();
                    Comm.CommandText = $"SELECT `{Col.ID}` FROM {Schema_Table} WHERE `{Col.ID}` LIKE '{StationID}%'";
                    DataSet ds = new DataSet();
                    MySqlDataAdapter DataAdpr = new MySqlDataAdapter { SelectCommand = Comm };
                    Console.WriteLine(DataAdpr.SelectCommand.CommandText);
                    DataAdpr.Fill(ds, StationID);
                    List<int> idLst = new List<int>();
                    foreach (DataRow row in ds.Tables[StationID].Rows)
                    {
                        int i = Convert.ToInt32(row[0].ToString().Substring(4, 4));
                        Console.WriteLine($"idLst.Add({i})\t dr[0]: {row[0].ToString().Substring(4, 4)}");
                        idLst.Add(i);
                    }
                    int newID = 1;
                    while (true)
                    {
                        if (idLst.Exists(x => x == newID)) newID++;
                        else
                        {
                            return StationID + newID.ToString("d4");
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                finally { Comm.Connection.Close(); }
            }
            return "0000";
        }
        public string GetTableRowCount()
        {
            int TableRowCount = 0;
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    MySqlCommand comm = new MySqlCommand { Connection = Conn };
                    while (true)
                    {
                        comm.CommandText = $"SELECT COUNT(*) FROM {Schema_Table} WHERE {Columns[(int)Col.ID].Name} = '{TableRowCount.ToString("d8")}'";
                        if (Convert.ToInt32(comm.ExecuteScalar()) == 0) break;
                        else TableRowCount++;
                    }
                }
                catch (Exception ex)
                {
                    TableRowCount = 0;
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                }
                finally { Conn.Close(); }
            }
            return TableRowCount.ToString("d8");
        }
        public int GetFuncCount(string ID)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    string cmd = $"SELECT FuncCount FROM {Schema_Table} WHERE `{Col.ID}` = '{ID}'";
                    Console.WriteLine(cmd);
                    return Convert.ToInt32(new MySqlCommand(cmd, Conn).ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                    return 0;
                }
                finally { Conn.Close(); } //關閉資料庫
            }
        }
        public bool WriteFuncCount(string ID, int FuncCount)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    string Edit = $"UPDATE {Schema_Table} SET `{Col.FuncCount}` = '{FuncCount}' WHERE `{Col.ID}` = '{ID}';";
                    Console.WriteLine(Edit);
                    //執行修改
                    new MySqlCommand(Edit, Conn).ExecuteNonQuery();
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                    return true;
                }
                finally { Conn.Close(); } //關閉資料庫               
            }
        }
        public bool Write(string ID, string Station, Item Item)
        {
            string Xml = XmlUtil.Serializer(typeof(Item), Item);
            string now = DateTime.Now.ToString(yyyyMMdd_HHmmss);
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);                 
                    if (IsItemExist(ID))
                    {
                        /*修改*/
                        string Edit = "UPDATE " + Schema_Table + " SET " +
                            $"{Columns[(int)Col.Station].Name} = '{Station}', " +
                            $"{Columns[(int)Col.ShowName].Name} = '{Item.Name}', " +
                            $"{Columns[(int)Col.XML].Name} = '{Xml}', " +
                            $"{Columns[(int)Col.Time_Stamp].Name} = '{now}' WHERE " +
                            $"{Columns[(int)Col.ID].Name} = '{ID}';";
                        Console.WriteLine(Edit);
                        //執行修改
                        new MySqlCommand(Edit, Conn).ExecuteNonQuery();
                    }
                    else
                    {
                        /*新增*/
                        InsertInto(Conn);
                    }
                }
                catch { InsertInto(Conn); }
                finally { Conn.Close(); } //關閉資料庫               
            }
            return true;

            void InsertInto(MySqlConnection Conn)
            {
                //int count = 0;
                //try
                //{
                //    DataTable dt = new DataTable();
                //    string c = $"SELECT {Columns[(int)Col.ID].Name} FROM {Table_Name}";
                //    Console.WriteLine(c);
                //    new MySqlDataAdapter(c, Conn).Fill(dt);
                //    Console.WriteLine("dt.Rows.Count: " + dt.Rows.Count);
                //    for (count = 0; count < dt.Rows.Count; count++)
                //    {
                //        Console.WriteLine($"dt.Rows[{count}]: {dt.Rows[count][0].ToString()}");
                //        if (dt.Rows[count][0].ToString() != count.ToString("d8"))
                //            break;
                //    }
                //}
                //catch { }

                string AddNew = string.Format(
                    $"INSERT INTO {Table_Name}" +
                    $"({Columns[(int)Col.ID].Name}," +
                    $"{Columns[(int)Col.Station].Name}," +
                    $"{Columns[(int)Col.ShowName].Name}," +
                    $"{Columns[(int)Col.Time_Stamp].Name}," +
                    $"{Columns[(int)Col.XML].Name}) " +
                    $"VALUES('{ID}','{Station}','{Item.Name}','{now}','{Xml}')");
                Console.WriteLine(AddNew);
                //執行新增
                new MySqlCommand(AddNew, Conn).ExecuteNonQuery();
            }
        }
    }
    class DeviceDB : Database
    {
        enum Col { ModelName = 0, Type }
        public DeviceDB()
        {
            Table_Name = "Device_List";
            Columns = new Column[2]
            {
                new Column(Col.ModelName, "VARCHAR", 30, false, false),
                new Column(Col.Type, "VARCHAR", 30, false, false)
            };
        }
        public static DeviceTypes GetType(object ModelName)
        {
            Console.WriteLine($"==DeviceDB.GetType({ModelName})==");
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    string cmd = $"SELECT {Col.Type} FROM `Self_Inspection`.`Device_List` WHERE {Col.ModelName} LIKE '%{ModelName}%'";
                    Console.WriteLine(cmd);
                    string DevicType = new MySqlCommand(cmd, Conn).ExecuteScalar().ToString();
                    foreach (DeviceTypes dt in Enum.GetValues(typeof(DeviceTypes)))
                        if (DevicType == dt.ToString())
                            return dt;
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); /*MessageBox.Show(ModelName + " not found!", "Error");*/ }
                finally { Conn.Close(); } 
                return DeviceTypes.Null;
            }
        }
        public List<Device> GetDevices(string Type)
        {
            Console.WriteLine($"==DeviceDB.GetDevices({Type})==");
            List<Device> Devices = new List<Device>();
            DataTable dt = new DataTable();
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[(int)Col.ModelName].Name} FROM {Schema_Table} WHERE {Columns[(int)Col.Type].Name} = '{Type}'";
                    new MySqlDataAdapter(cmd, Conn).Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        Devices.Add(new Device() { Type = Type, ModelName = row[0].ToString() });

                    return Devices;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(Type + " not Exists!", "Error");
                    return Devices;
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
        }
        public bool Write(string DeviceName, string DeviceType)
        {
            Console.WriteLine($"==DeviceDB.Write({DeviceName}, {DeviceType})==");
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[0].Name} FROM {Schema_Table} WHERE {Columns[0].Name} = '{DeviceName}';";
                    if (string.IsNullOrEmpty((string)new MySqlCommand(cmd, Conn).ExecuteScalar()))
                    {
                        /*新增*/
                        string AddNew = string.Format("INSERT INTO " + Schema_Table +
                           $" ({Columns[0].Name},{Columns[1].Name})" +
                            " VALUES('{0}','{1}')"
                            , DeviceName, DeviceType);
                        Console.WriteLine(AddNew);
                        //執行新增
                        new MySqlCommand(AddNew, Conn).ExecuteNonQuery();
                    }
                    else
                    {
                        /*修改*/
                        string Edit = $"UPDATE {Schema_Table} SET " +
                            $"{Columns[1].Name} = '{DeviceType}' " +
                            $"WHERE {Columns[0].Name} = '{DeviceName}';";
                        Console.WriteLine(Edit);
                        //執行修改
                        new MySqlCommand(Edit, Conn).ExecuteNonQuery();
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(DeviceName + " not Exists!", "Error");
                    return true;
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
        }
    }
    class ProgramDB : Database
    {
        enum Col { ID = 0, Station, Time_Stamp, XML }
        public ProgramDB()
        {
            Table_Name = "Program_List";
            Columns = new Column[4]
            {
            /*00*/ new Column(Col.ID, "VARCHAR", 4, false, false),
            /*01*/ new Column(Col.Station, "VARCHAR", 40, false, false),
            /*02*/ new Column(Col.Time_Stamp, "VARCHAR", 30, false, false),
            /*03*/ new Column(Col.XML, "TEXT", 0, false, false)
            };
        }
        public string GetID(string stationName)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    string cmd = $"SELECT `{Col.ID}` FROM {Schema_Table} WHERE `{Col.Station}` = '{stationName}';";
                    Console.WriteLine(cmd);
                    return new MySqlCommand(cmd, Conn).ExecuteScalar().ToString();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); return "0000"; }
                finally { Conn.Close(); }
            }
        }
        public Station GetStation(string StationName)
        {
            Console.WriteLine($"==ProgramDB.GetStation({StationName})==");
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[(int)Col.XML].Name} FROM {Schema_Table} WHERE {Columns[(int)Col.Station].Name} = '{StationName}';";
                    Console.WriteLine(cmd);

                    string stationXml = (string)new MySqlCommand(cmd, Conn).ExecuteScalar();
                    Console.WriteLine(stationXml);
                    return XmlUtil.Deserialize(typeof(Station), stationXml) as Station;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    Email.SendErrorEmail("GetStation: " + Schema_Table, ex.Message, ex.ToString());
                    return new Station();
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
        }
        public DataTable GetStationList()
        {
            Console.WriteLine("==ProgramDB.GetStationList()==");
            DataTable dt = new DataTable();

            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[(int)Col.Station].Name}, {Columns[(int)Col.XML].Name} FROM {Schema_Table};";
                    Console.WriteLine(cmd);
                    new MySqlDataAdapter(cmd, Conn).Fill(dt);
                    if (dt.Rows.Count > 0)
                        Console.WriteLine("GetStationList Succeed");
                    else
                        throw new Exception("No Station found");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    Email.SendErrorEmail("GetStationList: " + Schema_Table, ex.Message, ex.ToString());
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
            return dt;
        }
        public bool Write(Station station)
        {
            Console.WriteLine($"==ProgramDB.Write({station.Name}: {station.ID})==");
            station.ClearSpItem_None();

            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    MySqlCommand Comm = new MySqlCommand
                    {
                        Connection = Conn,
                        CommandText = $"SELECT COUNT(*) FROM {Schema_Table} WHERE `{Col.ID}` = '{station.ID}';"
                    };
                    Console.WriteLine(Comm.CommandText);

                    string UpdateTime = DateTime.Now.ToString(yyyyMMdd_HHmmss);
                    if (Convert.ToInt32(Comm.ExecuteScalar()) == 0)
                    {
                        //DataTable dt = new DataTable();
                        //string cmd = $"SELECT `{Col.ID}` FROM {Schema_Table} WHERE `{Col.ID}` LIKE '{station.Name[0]}%'";
                        //Console.WriteLine(cmd);
                        //new MySqlDataAdapter(cmd, Conn).Fill(dt);
                        //int max = 0; foreach (DataRow row in dt.Rows)
                        //{
                        //    int cnt = Convert.ToInt32(row[0].ToString().Substring(1, 3));
                        //    max = cnt > max ? cnt : max;
                        //}
                        //station.ID = station.Name[0] + (max + 1).ToString("d3");

                        /*新增*/
                        Comm.CommandText = $"INSERT INTO {Schema_Table} (`{Col.ID}`,`{Col.Station}`,`{Col.Time_Stamp}`,`{Col.XML}`)VALUES('{station.ID}','{station.Name}','{UpdateTime}','{XmlUtil.Serializer(typeof(Station), station)}')";
                    }
                    else
                    {
                        /*修改*/
                        Comm.CommandText = $"UPDATE {Schema_Table} SET `{Col.Time_Stamp}` = '{UpdateTime}', `{Col.Station}` = '{station.Name}', `{Col.XML}` = '{XmlUtil.Serializer(typeof(Station), station)}' WHERE `{Col.ID}` = '{station.ID}';";
                    }
                    Console.WriteLine(Comm.CommandText);
                    Comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("CreateTableIfNotExists: " + TableName, ex.Message, ex.ToString());
                    return false;
                }
                finally { Conn.Close(); }
            }
            return true;
        }
    }
    class ResultDB : Database
    {
        public enum Col { Station = 0, Device_Name, No, Serial_Number, Status, Expect_Test_Time, Test_Time, Employee, Test_Item, Test_Value, Spec_Min, Spec_Max, Test_Program }
        public ResultDB()
        {
            Table_Name = "Test_Results";
            Columns = new Column[13]
            {
             /*00*/ new Column(Col.Station, "VARCHAR", 30, false, false),
             /*01*/ new Column(Col.Device_Name, "VARCHAR", 30, false, false),
             /*02*/ new Column(Col.No + ".", "INT", 11, false, false),
             /*03*/ new Column(Col.Status, "VARCHAR", 10, false, false),
             /*04*/ new Column(Col.Serial_Number, "VARCHAR", 15, true, false),
             /*05*/ new Column(Col.Expect_Test_Time, "VARCHAR", 30, false, false),
             /*06*/ new Column(Col.Test_Time, "DATETIME", 0, false, false),
             /*07*/ new Column(Col.Employee, "VARCHAR", 10, false, false),
             /*08*/ new Column(Col.Test_Item, "VARCHAR", 30, false, false),
             /*09*/ new Column(Col.Test_Value, "VARCHAR", 30, false, false),
             /*10*/ new Column(Col.Spec_Min, "VARCHAR", 30, false, false),
             /*11*/ new Column(Col.Spec_Max, "VARCHAR", 30, false, false),
             /*12*/ new Column(Col.Test_Program, "VARCHAR", 100, true, false)
            };
        }
        public DataTable Read(string station, string modelName, string number)
        {
            Console.WriteLine("==ResultDB.Read()==");
            string tableName = $"{Database_Name}.{Table_Name}";
            DataTable dt = new DataTable();

            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[0].Name}, {Columns[1].Name}, {Columns[2].Name} FROM {tableName} " +
                        $"WHERE {Columns[0].Name} = '{station}' AND {Columns[1].Name} = '{modelName}' AND {Columns[2].Name} = '{number}';";
                    Console.WriteLine(cmd);
                    new MySqlDataAdapter(cmd, Conn).Fill(dt);
                    if (dt.Rows.Count > 0)
                        Console.WriteLine("ResultDB.Read() Succeed");
                    else
                        throw new Exception("ResultDB.Read() Fail");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
            return dt;
        }
        public DataTable DataSearch(string Station, string Dates, string Status)
        {
            Console.WriteLine($"==ResultDB.DataSearch('{Station}', '{Dates}', '{Status}')==");
            DataTable dt = new DataTable();

            string startDate = Dates.Split('~')[0].Replace('\\', '-').Trim();
            string endDate = Dates.Split('~')[1].Replace('\\', '-').Trim();
            using (MySqlCommand command = new MySqlCommand() { Connection = new MySqlConnection(ConnectInfo) })
            {
                try
                {
                    command.Connection.Open();
                    command.CommandText = $"SELECT DISTINCT `{Col.Test_Program}`,`{Col.Device_Name}`,`{Col.No}.`,`{Col.Employee}`,`{Col.Test_Time}`,`{Col.Status}` FROM {Schema_Table} WHERE" +
                        $" `{Col.Test_Time}` BETWEEN '{startDate}' AND '{endDate} 23:59:59'" +
                        $" AND `{Col.Employee}` NOT LIKE 'TestID'";

                    if (!string.IsNullOrEmpty(Station)) command.CommandText += $" AND `{Col.Station}` LIKE '{Station}'";
                    if (!string.IsNullOrEmpty(Status)) command.CommandText += $" AND `{Col.Status}` LIKE '{Status}'";

                    command.CommandText += $" ORDER BY {Col.Test_Time} DESC, `{Col.No}.` ASC";

                    Console.WriteLine(command.CommandText);
                    new MySqlDataAdapter(command.CommandText, command.Connection).Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            return dt;
        }
        public DataTable ItemSearch(object Station, object UUT, object Employee, object TestTime)
        {
            Console.WriteLine($"==ResultDB.DataSearch('{Station}', '{UUT}', '{Employee}', '{TestTime}')==");
            DataTable dt = new DataTable();
            string deviceName = UUT.ToString().Split('-')[0];
            int deviceNo = Convert.ToInt32(UUT.ToString().Split('-')[1]);

            using (MySqlCommand command = new MySqlCommand() { Connection = new MySqlConnection(ConnectInfo) })
            {
                try
                {
                    command.Connection.Open();
                    command.CommandText = $"SELECT DISTINCT `{Col.Test_Item}`,`{Col.Test_Value}`,`{Col.Spec_Min}`,`{Col.Spec_Max}` FROM {Schema_Table} WHERE" +
                        $" `{Col.Test_Time}` LIKE '{TestTime}'" +
                        $" AND `{Col.Station}` LIKE '{Station}' AND `{Col.Device_Name}` LIKE '{deviceName}' AND `{Col.No}.` LIKE '{deviceNo}' AND `{Col.Employee}` LIKE '{Employee}'" +
                        $" AND `{Col.Employee}` NOT LIKE 'TestID'" +
                        $" ORDER BY {Col.Test_Item} ASC";

                    Console.WriteLine(command.CommandText);
                    new MySqlDataAdapter(command.CommandText, command.Connection).Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            return dt;
        }
        public string GetStationByItem(object UUT, object Employee, object TestTime)
        {
            string station = string.Empty;
            string deviceName = UUT.ToString().Split('-')[0];
            int deviceNo = Convert.ToInt32(UUT.ToString().Split('-')[1]);
            using (MySqlCommand command = new MySqlCommand() { Connection = new MySqlConnection(ConnectInfo) })
            {
                try
                {
                    command.Connection.Open();
                    command.CommandText = $"SELECT DISTINCT `{Col.Station}` FROM {Schema_Table} WHERE" +
                        $" `{Col.Test_Time}` LIKE '{TestTime}' AND `{Col.Device_Name}` LIKE '{deviceName}' AND `{Col.No}.` LIKE '{deviceNo}' AND `{Col.Employee}` LIKE '{Employee}'";

                    Console.WriteLine(command.CommandText);
                    station = command.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            return station;
        }
        public bool Write(string station, SPItem spItem, bool rootMode)
        {
            Console.WriteLine($"==ResultDB.Write({spItem.Name})==");
            string UpdateTime = DateTime.Now.ToString(yyyyMMdd_HHmmss);
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    //CreateTableIfNotExists(Conn, Table_Name, Columns);

                    Console.WriteLine("Insert:\n" + XmlUtil.Serializer(typeof(SPItem), spItem));

                    foreach (TestItem ti in spItem.TestItems)
                    {
                        if (string.IsNullOrEmpty(ti.Value)) continue;

                        string modelName = spItem.UUT.Substring(0, spItem.UUT.Length - 3);
                        string modelNo = spItem.UUT.Substring(spItem.UUT.Length - 2, 2);

                        StringBuilder AddNew = new StringBuilder($"INSERT INTO {Schema_Table} ({Columns[0].Name}");
                        for (int i = 1; i < Columns.Length; i++)
                            AddNew.Append($",{Columns[i].Name}");
                        AddNew.Append($") VALUES ('{station}','{modelName}','{modelNo}','{ti.PF.Text}',NULL,'{ExpectTime()}','{spItem.TestTime}','{spItem.Employee}',");
                        AddNew.Append($"'{ti.Name}','{ti.Value}','{ti.Min}','{ti.Max}','{spItem.Name}');");
                        Console.WriteLine(AddNew);
                        //執行新增
                        new MySqlCommand(AddNew.ToString(), Conn).ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Email.SendErrorEmail("ResultDB.Write Error: " + station, ex.Message, ex.ToString());
                    Console.WriteLine(ex.ToString());
                    if (rootMode) MessageBox.Show(ex.Message);
                    return true;
                }
                finally { Conn.Close(); }
            }
            return false;

            string ExpectTime()
            {
                DateTime dt;
                switch (spItem.Period)
                {
                    case TestPeriod.Day:
                        dt = DateTime.Now;
                        break;
                    case TestPeriod.Week:
                        dt = DateTime.Now;
                        while (dt.DayOfWeek != DayOfWeek.Friday) { dt = dt.AddDays(1); }
                        break;
                    case TestPeriod.Season:
                        dt = DateTime.Now;
                        while (dt.Month % 3 != 0) { dt.AddMonths(1); }
                        int daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
                        while (dt.Day > daysInMonth) { dt.AddDays(-1); }
                        while (dt.Day < daysInMonth) { dt.AddDays(1); }
                        break;
                    default: throw new Exception("spItem.Period Error: " + spItem.Name);
                }
                return dt.ToString("yyyy-MM-dd") + " 17:00:00";
            }
        }
    }
    class SettingDB : Database
    {
        public enum List { Interface, Email, Command, Version_Temp }
        public SettingDB()
        {
            Table_Name = "Setting";
            Columns = new Column[2]
            {
                new Column("Item", "VARCHAR", 20, false, false),
                new Column("Value", "TEXT", 0, false, false)
            };
        }

        public string GetSettingList(List ListName)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[1].Name} FROM {Schema_Table} WHERE {Columns[0].Name} = '{ListName.ToString()}'";
                    string xml = (string)new MySqlCommand(cmd, Conn).ExecuteScalar();
                    Console.WriteLine(xml);
                    return xml;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
            return string.Empty;
        }
        public bool WriteSettingList(List ListName, string xml)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT {Columns[0].Name} FROM {Schema_Table} WHERE {Columns[0].Name} = '{ListName.ToString()}';";
                    Console.WriteLine(cmd);
                    if (string.IsNullOrEmpty((string)new MySqlCommand(cmd, Conn).ExecuteScalar()))
                    {
                        /*新增*/
                        InsertInto(Schema_Table, Conn);
                    }
                    else
                    {
                        /*修改*/
                        string Edit = "UPDATE " + Schema_Table +
                            $" SET {Columns[1].Name} = '{xml}'" +
                            $" WHERE {Columns[0].Name} = '{ListName.ToString()}';";
                        Console.WriteLine(Edit);
                        //執行修改
                        new MySqlCommand(Edit, Conn).ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    InsertInto(Schema_Table, Conn);
                    //Console.WriteLine(ex.ToString()); MessageBox.Show(ex.Message);
                    //Email.SendErrorEmail("GetStationList: " + tableName, ex.Message, ex.ToString());
                }
                finally
                {
                    Conn.Close(); //關閉資料庫
                }
            }
            return false;

            void InsertInto(string schema_Table, MySqlConnection Conn)
            {
                string AddNew = string.Format($"INSERT INTO {schema_Table} ({Columns[0].Name},{Columns[1].Name}) VALUES('{ListName.ToString()}','{xml}')");
                Console.WriteLine(AddNew);
                //執行新增
                new MySqlCommand(AddNew, Conn).ExecuteNonQuery();
            }
        }
    }

    class IOCardDB : Database
    {
        public IOCardDB()
        {
            Table_Name = "IO_Card_Type";
            Columns = new Column[2]
            {
                new Column("CardTypeName", "VARCHAR", 30, false, false),
                new Column("CardTypeValue", "INT", 10, false, false)
            };
        }
        public ushort Get_IOCard(string CardTypeName)
        {
            using (MySqlConnection Conn = new MySqlConnection(ConnectInfo))
            {
                try
                {
                    Conn.Open();
                    CreateTableIfNotExists(Conn, Table_Name, Columns);
                    string cmd = $"SELECT `CardTypeValue` FROM {Schema_Table} WHERE `CardTypeName` LIKE '{CardTypeName}'";
                    return ushort.Parse(new MySqlCommand(cmd, Conn).ExecuteScalar().ToString());
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); return 0; }
                finally { Conn.Close(); } //關閉資料庫                
            }
        }
    }
    class Column
    {
        private string name;
        private string type;
        private int length;
        private bool _null;
        private bool _increment;

        public Column(object Name, string Type, int Length, bool allowNull, bool autoIncrement)
        {
            name = Regex.Replace(Name.ToString(), "`", "");
            type = Type;
            length = Length;
            _null = allowNull;
            _increment = autoIncrement;
        }

        public string Name { get => $"`{name}`"; }
        public string Type { get => $"{type}{(length == 0 ? "" : $"({length})")}"; }
        public string Null { get => _null ? "" : "NOT NULL"; }
        public string Increment { get => _increment ? "AUTO_INCREMENT=1" : ""; }
    }
}
