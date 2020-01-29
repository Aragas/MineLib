<?php
function generateRandomString($length = 32) {
    $characters = '0123456789abcdefghijklmnopqrstuvwxyz';
    $charactersLength = strlen($characters);
    $randomString = '';
    for ($i = 0; $i < $length; $i++) {
        $randomString .= $characters[rand(0, $charactersLength - 1)];
    }
    return $randomString;
}
function url(){
    if(isset($_SERVER['HTTPS'])){
        $protocol = ($_SERVER['HTTPS'] && $_SERVER['HTTPS'] != "off") ? "https" : "http";
    }
    else{
        $protocol = 'http';
    }
    return $protocol . "://" . parse_url($_SERVER['REQUEST_URI'], PHP_URL_HOST);
}

error_reporting(E_ALL);
ini_set('error_reporting', E_ALL);
ini_set('display_errors', 1);
$Name            = $_REQUEST['name'];
$Port            = $_REQUEST['port'];
$Players         = $_REQUEST['users'];
$MaxPlayers      = $_REQUEST['max'];
$IsPublic        = $_REQUEST['public'];
$Salt            = $_REQUEST['salt'];
$Software        = $_REQUEST['software'] ?? '';
$IsSupportingWeb = $_REQUEST['web'] ?? 'false';
$Time            = date('Y-m-d H:i:s');

$Conn = pg_connect("host=postgres dbname=minelib user=minelib password=minelib") or die('Failed to connect to DB');

if (!pg_exec($Conn, "DELETE FROM classic_servers WHERE Name='" . $Name . "'")) //delete old server
{
    printf('Error: ' . pg_last_error($Conn));
}
$Name = strip_tags($Name);
$Url = generateRandomString();
if (!pg_exec($Conn, "INSERT INTO classic_servers(Name, Port, Salt, Players, MaxPlayers, IsPublic, Software, IsSupportingWeb, LastTimeSeen)
VALUES ('$Name', '$Port', '$Salt', '$Players', '$MaxPlayers', '$IsPublic', '$Software', '$IsSupportingWeb', '$Time')"))
{
    die('Error: ' . pg_last_error($Conn));
}

pg_close($Conn);
$actual_link = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on' ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
#$myDomain = preg_replace('/^www\./', '', parse_url($actual_link, PHP_URL_HOST));
$myDomain = parse_url($actual_link, PHP_URL_HOST);
echo parse_url($actual_link, PHP_URL_SCHEME) . '://' . parse_url($actual_link, PHP_URL_HOST) . ':' . parse_url($actual_link, PHP_URL_PORT) . '/play?' . $Url;
?>
</body>
</html>