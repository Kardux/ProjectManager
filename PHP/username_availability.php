<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $Username = mysql_real_escape_string($_GET['Username'], $DB);
 
    $Query = "SELECT * FROM `users` WHERE `USERNAME` = '" . $Username . "'";
    $Result = mysql_query($Query) or die('Query failed: ' . mysql_error());
 
    $NbResults = mysql_num_rows($Result);

    if ($NbResults > 0)
    {
        echo "false";
    }
    else
    {
        echo "true";
    }
?>