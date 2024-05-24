using System.Text.Json;
using Data.Model;
using Data.Interfaces;
using Data.Exceptions;
using System.Data.SqlClient;
using System.Data;

namespace Data
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private const string _connectionString = @"Server=SQL-DEV;Database=Ashwith_EmployeeDirectory;Trusted_Connection=True;";



        public bool DeleteData(string email)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                con.Open();
                string sqlQuery = $"Delete from Employee where Email=@email";
                SqlCommand command = new SqlCommand(sqlQuery, con);
                command.Parameters.Add("@email", SqlDbType.NVarChar, 350).Value = email;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<string> GetStaticData(string name)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"SELECT * FROM {name}";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    List<string> result = new List<string>();
                    SqlDataReader reader = command.ExecuteReader();
                    if (name == "Employee")
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Id"].ToString() + ' ' + reader["FirstName"].ToString()! + ' ' + reader["LastName"].ToString());
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"].ToString()!);
                        }
                    }

                    reader.Close();
                    return result;
                }
            }
            catch (Exception)
            {
                return [];
            }
        }
        public int? GetId(string table, string name)
        {
            if (name == null)
                return null;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"select * from {table} where Name='{name}'";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    List<string> departments = new List<string>();
                    SqlDataReader reader = command.ExecuteReader();
                    int id = -1;
                    while (reader.Read())
                    {
                        id = (int)reader["Id"];
                    }
                    reader.Close();
                    return id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public List<string> GetRoleNames(string department)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"SELECT * FROM Role where DepartmentId={GetId("Department", department)}";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    List<string> roles = new List<string>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        roles.Add(reader["Name"].ToString()!);
                    }
                    reader.Close();
                    return roles;
                }
            }
            catch (Exception)
            {
                return [];
            }
        }
        public List<string> GetLocations(string role)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"select Location.Name as LocationName from Location join RoleDetails on Location.Id=RoleDetails.LocationId join Role on Role.Id=RoleDetails.RoleId where role.Id={GetId("Role", role)};";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    List<string> locations = new List<string>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        locations.Add(reader["LocationName"].ToString()!);
                    }
                    reader.Close();
                    return locations;
                }
            }
            catch (Exception)
            {
                return [];
            }
        }

        public List<Role> GetRoles()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"select Role.id,Role.Name,Department.Name as Department,description,Location.Name as Location from Role join roledetails on Role.id=Roledetails.RoleId join Department on department.id=Role.departmentId join location on Location.id=Roledetails.locationid;";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    List<Role> roleList = new();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        roleList.Add(new Role
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"],
                            Department = (string)reader["Department"],
                            Description = reader["description"] == (object)DBNull.Value ? null : reader["description"].ToString(),
                            Location = (string)reader["location"]
                        });
                    }
                    reader.Close();
                    return roleList;
                }
            }
            catch (Exception)
            {
                return [];
            }
        }
        public void AddEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"Insert into Employee Values(@id,@firstName,@lastName,@email,@mobileNumber,@dateOfBirth,@joiningDate,@roleId,@departmentId,@locationId,@managerId,@projectId)";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.Parameters.Add("@id", SqlDbType.NVarChar, 6).Value = employee.Id;
                    command.Parameters.Add("@firstName", SqlDbType.NVarChar, 50).Value = employee.FirstName;
                    command.Parameters.Add("@lastName", SqlDbType.NVarChar, 50).Value = employee.LastName;
                    command.Parameters.Add("@email", SqlDbType.NVarChar, 350).Value = employee.Email;
                    command.Parameters.Add("@mobileNumber", SqlDbType.BigInt).Value = employee.MobileNumber;
                    command.Parameters.Add("@dateOfBirth", SqlDbType.DateTime).Value = employee.DateOfBirth;
                    command.Parameters.Add("@joiningDate", SqlDbType.DateTime).Value = employee.JoiningDate;
                    command.Parameters.Add("@roleId", SqlDbType.Int).Value = GetId("Role", employee.JobTitle);
                    command.Parameters.Add("@departmentId", SqlDbType.Int).Value = GetId("Department", employee.Department);
                    command.Parameters.Add("@locationId", SqlDbType.Int).Value = GetId("Location", employee.Location);
                    command.Parameters.Add("@managerId", SqlDbType.NVarChar, 6).Value = employee.Manager ?? (object)DBNull.Value;
                    command.Parameters.Add("@projectId", SqlDbType.Int).Value = GetId("Project", employee.Project!) ?? (object)DBNull.Value;
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public List<Employee> GetEmployees(string? id = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery;
                    if (id != null)
                    {
                        sqlQuery = "SELECT a1.Id,a1.FirstName,a1.LastName,a1.Email,a1.MobileNumber,a1.DateOfBirth,a1.JoiningDate,Role.Name AS JobTitle," +
                        "Department.Name AS Department,Location.Name AS Location,Project.Name AS Project,CASE WHEN a1.ManagerId IS NOT null THEN a2.FirstName " +
                        "ELSE NULL END AS Manager FROM Employee a1 LEFT JOIN Employee a2 ON a1.ManagerId = a2.Id JOIN Role ON Role.Id = a1.RoleId JOIN " +
                        $"Department ON Department.Id = a1.DepartmentId JOIN Location ON Location.Id = a1.LocationId LEFT JOIN Project ON a1.ProjectId = Project.Id where a1.email='{id}';";
                    }
                    else
                    {
                        sqlQuery = "SELECT a1.Id,a1.FirstName,a1.LastName,a1.Email,a1.MobileNumber,a1.DateOfBirth,a1.JoiningDate,Role.Name AS JobTitle," +
                       "Department.Name AS Department,Location.Name AS Location,Project.Name AS Project,CASE WHEN a1.ManagerId IS NOT null THEN a2.FirstName " +
                       "ELSE NULL END AS Manager FROM Employee a1 LEFT JOIN Employee a2 ON a1.ManagerId = a2.Id JOIN Role ON Role.Id = a1.RoleId JOIN " +
                       "Department ON Department.Id = a1.DepartmentId JOIN Location ON Location.Id = a1.LocationId LEFT JOIN Project ON a1.ProjectId = Project.Id;";
                    }
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    //command.Parameters.Add("@null",SqlDbType.Variant).Value = (object)DBNull.Value;
                    List<Employee> employeesList = new();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        employeesList.Add(new Employee
                        {
                            Id = reader["Id"].ToString()!,
                            FirstName = reader["FirstName"].ToString()!,
                            LastName = reader["LastName"].ToString()!,
                            Email = reader["Email"].ToString()!,
                            DateOfBirth = reader["DateOfBirth"] != null ? (DateTime)reader["DateOfBirth"] : null,
                            JoiningDate = (DateTime)reader["JoiningDate"],
                            MobileNumber = reader["MobileNumber"] != null ? (long)reader["MobileNumber"] : null,
                            JobTitle = reader["JobTitle"].ToString()!,
                            Department = reader["Department"].ToString()!,
                            Location = reader["Location"].ToString()!,
                            Project = reader["Project"] != null ? reader["Project"].ToString() : null,
                            Manager = reader["Manager"] != null ? reader["Manager"].ToString() : null
                        });
                    }
                    reader.Close();
                    return employeesList;
                }
            }
            catch (Exception)
            {
                return [];
            }
        }
        public void EditEmployeeData(Dictionary<int, string> pair, int choice, string value, string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    int? id = null;
                    bool flag = true;
                    con.Open();
                    if (pair[choice] == nameof(Employee.JobTitle))
                    {
                        pair[choice] = "RoleId";
                        id = GetId("Role", value) ?? -1;
                    }
                    else if (pair[choice] == nameof(Employee.Manager))
                    {
                        pair[choice] = "ManagerId";
                        id = GetId("Employee", value) ?? null;
                    }
                    else if (pair[choice] == nameof(Employee.Project))
                    {
                        pair[choice] = "ProjectId";
                        id = GetId("Project", value) ?? null;
                    }
                    else if (pair[choice] == nameof(Employee.Location))
                    {
                        pair[choice] = "LocationId";
                        id = GetId("Location", value) ?? null;
                    }
                    else
                    {
                        flag = false;
                    }
                    string sqlQuery = $"Update Employee set  {pair[choice]}=@id where Email=@email";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    if (flag)
                    {
                        command.Parameters.Add("@id", SqlDbType.Int).Value = id == null ? (object)DBNull.Value : id;
                    }
                    else
                    {
                        if (nameof(Employee.DateOfBirth) == pair[choice] || nameof(Employee.JoiningDate) == pair[choice])
                        {
                            command.Parameters.Add("@id", SqlDbType.DateTime).Value = value == null ? (object)DBNull.Value : DateTime.Parse(value);
                        }
                        else if (nameof(Employee.MobileNumber) == pair[choice])
                        {
                            command.Parameters.Add("@id", SqlDbType.Int).Value = value == null ? (object)DBNull.Value : int.Parse(value);
                        }
                        else
                        {
                            command.Parameters.Add("@id", SqlDbType.NVarChar, 70).Value = value == null ? (object)DBNull.Value : value;
                        }
                    }
                    command.Parameters.Add("@email", SqlDbType.NVarChar, 350).Value = email;
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public void AddRole(Role role)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"Insert into Role Values(@id,@Name,@description,@departmentId)";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.Parameters.Add("@id", SqlDbType.Int).Value = role.Id;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = role.Name;
                    command.Parameters.Add("@description", SqlDbType.NVarChar, 300).Value = role.Description;
                    command.Parameters.Add("@departmentId", SqlDbType.Int).Value = GetId("Department", role.Department);
                    command.ExecuteNonQuery();
                }
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string sqlQuery = $"Insert into RoleDetails Values(@roleId,@locationId)";
                    SqlCommand command = new SqlCommand(sqlQuery, con);
                    command.Parameters.Add("@roleId", SqlDbType.Int).Value = role.Id;
                    command.Parameters.Add("@locationId", SqlDbType.Int).Value = GetId("Location", role.Location);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

    }
}
