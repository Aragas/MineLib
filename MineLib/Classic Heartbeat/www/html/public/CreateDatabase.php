<?php
$Conn;
CreateDatabase();
function CreateDatabase()
{
    global $Conn;
    $Conn = pg_connect("host=postgres dbname=minelib user=minelib password=minelib") or die('Failed to connect to DB');
    AddTables();
    pg_close($Conn);
}

function AddTables()
{
    global $Conn;
	pg_exec($Conn, "DROP TABLE classic_servers");
	
    $sql = "CREATE TABLE classic_servers(ID SERIAL, PRIMARY KEY(ID), Name VARCHAR(64), Port INTEGER, Salt VARCHAR(256), Players INT, MaxPlayers INT, IsPublic BOOLEAN, Software VARCHAR(256), IsSupportingWeb BOOLEAN, LastTimeSeen TIMESTAMP, Url VARCHAR(32))";
    
    // Execute query
    if (pg_exec($Conn, $sql))
	{
        echo "Table servers created successfully";
    }
	else
	{
        echo "Error creating table: " . pg_last_error($Conn);
    }
}
?>