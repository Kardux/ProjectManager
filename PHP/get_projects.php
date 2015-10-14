<?php 
    $DB = mysql_connect('YOUR_HOST_NAME', 'YOUR_USERNAME', 'YOUR_PASSWORD') or die('Could not connect: ' . mysql_error()); 
    mysql_select_db('YOUR_DATABASE') or die('Could not select database.');

    $UserMD5 = mysql_real_escape_string($_GET['UserMD5'], $DB);
 
    $Query = "SELECT * FROM `projects` WHERE `USERMD5` LIKE '%" . $UserMD5 . "%' ORDER BY `STARTDATE` ASC";
    $Result = mysql_query($Query) or die('Query failed: ' . mysql_error());
 
    $NbResults = mysql_num_rows($Result);  
 
    echo "<GET_PROJECTS_RESULTS>\n";
    for($i = 0; $i < $NbResults; $i++)
    {
        echo "\t<PROJECT>\n";
        $Row = mysql_fetch_array($Result);
        echo "\t\t<ID>" . $Row['ID'] . "</ID>\n";
        echo "\t\t<Name>" . $Row['NAME'] . "</Name>\n";
        echo "\t\t<StartDate>" . $Row['STARTDATE'] . "</StartDate>\n";
        echo "\t\t<TimeUnits>" . $Row['TIMEUNITS'] . "</TimeUnits>\n";
        echo "\t</PROJECT>\n";
    }
    echo "</GET_PROJECTS_RESULTS>";
?>