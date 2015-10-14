<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $UserMD5 = mysql_real_escape_string($_GET['UserMD5'], $DB); 
    $Name = mysql_real_escape_string($_GET['Name'], $DB);
    $StartDate = mysql_real_escape_string($_GET['StartDate'], $DB); 
    $TimeUnits = mysql_real_escape_string($_GET['TimeUnits'], $DB); 
    $CheckHash = $_GET['CheckHash']; 

    $SecretKey="YOUR_SECRET_KEY";

    $CurrentHash = md5($UserMD5 . $SecretKey); 
    if($CurrentHash == $CheckHash)
    { 
        $Query = "INSERT INTO projects VALUES (NULL, '$UserMD5', '$Name', '$StartDate', '$TimeUnits');"; 
        $Result = mysql_query($Query) or die('Query failed: ' . mysql_error()); 
    } 
?>