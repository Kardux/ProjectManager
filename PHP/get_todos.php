<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $UserMD5 = mysql_real_escape_string($_GET['UserMD5'], $DB);
 
    $Query = "SELECT * FROM `todos` WHERE `USERMD5` LIKE '%" . $UserMD5 . "%'";
    $Result = mysql_query($Query) or die('Query failed: ' . mysql_error());
 
    $NbResults = mysql_num_rows($Result);  
 
    echo "<GET_TODOS_RESULTS>\n";
    for($i = 0; $i < $NbResults; $i++)
    {
        echo "\t<TODO>\n";
        $Row = mysql_fetch_array($Result);
        echo "\t\t<ID>" . $Row['ID'] . "</ID>\n";
        echo "\t\t<Name>" . $Row['NAME'] . "</Name>\n";
        echo "\t</TODO>\n";
    }
    echo "</GET_TODOS_RESULTS>";
?>