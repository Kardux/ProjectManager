<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $UserMD5 = mysql_real_escape_string($_GET['UserMD5'], $DB); 
    $ID = mysql_real_escape_string($_GET['ID'], $DB);
    $CheckHash = $_GET['CheckHash']; 

    $SecretKey="YOUR_SECRET_KEY";

    $CurrentHash = md5($UserMD5 . $SecretKey); 
    if($CurrentHash == $CheckHash)
    { 
        $Query = "DELETE FROM `projects` WHERE `USERMD5` LIKE '%" . $UserMD5 . "%' AND `ID`=" . $ID;
        $Result = mysql_query($Query) or die('Query failed: ' . mysql_error()); 
    } 
?>