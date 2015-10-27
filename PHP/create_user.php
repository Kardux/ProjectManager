<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $Username = mysql_real_escape_string($_GET['Username'], $DB);
    $UserMD5 = mysql_real_escape_string($_GET['UserMD5'], $DB); 

    $CheckHash = $_GET['CheckHash']; 

    $SecretKey="YOUR_SECRET_KEY";

    $CurrentHash = md5($UserMD5 . $SecretKey); 
    if($CurrentHash == $CheckHash)
    { 
        $Query = "INSERT INTO users VALUES (NULL, '$Username', '$UserMD5');"; 
        $Result = mysql_query($Query) or die('Query failed: ' . mysql_error()); 
    } 
?>