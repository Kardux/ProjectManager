<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $Username = mysql_real_escape_string($_GET['Username'], $DB);
    $UserMD5 = mysql_real_escape_string($_GET['UserMD5'], $DB);
 
    $Query = "SELECT * FROM `users` WHERE `USERNAME` LIKE '%" . $Username . "%' AND `USERMD5` LIKE '%" . $UserMD5 . "%'";
    $Result = mysql_query($Query) or die('Query failed: ' . mysql_error());
 
    $NbResults = mysql_num_rows($Result);

    if ($NbResults > 0)
    {
        echo "true";
    }
    else
    {
        echo "false";
    }
?>