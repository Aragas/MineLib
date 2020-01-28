<?php
$Conn;
CreateDatabase();
function CreateDatabase()
{
    global $Conn;
    $Conn = pg_connect("host=postgres dbname=minelib user=minelib password=minelib");
    // Check connection
    #if (!$Conn)
	#{
    #    echo "Failed to connect to MySQL @CreateDatabse: " . mysqli_connect_error();
    #}
    AddTables();
}

function AddTables()
{
    global $Conn;
    // Check connection
    #if (mysqli_connect_errno())
    #{
    #	echo "Failed to connect to MySQL: " . mysqli_connect_error();
    #}
    $sql = "CREATE TABLE servers(ID INT NOT NULL AUTO_INCREMENT, 
PRIMARY KEY(ID),ServerName CHAR(60),Url CHAR(100),Players INT, MaxPlayers INT, Uptime INT, LastTimeSeen DATETIME)";

    // Execute query
    if (pg_exec($Conn, $sql))
    {
        echo "Table servers created successfully";
    }
    else
    {
        #echo "Error creating table: " . mysqli_error();
		echo "Error creating table:";
    }
}
?>